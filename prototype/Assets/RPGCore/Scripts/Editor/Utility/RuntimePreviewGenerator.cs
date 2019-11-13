#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace RPGCore.Utility.Editors
{
	public static class RuntimePreviewGenerator
	{
		// Source: https://github.com/MattRix/UnityDecompiled/blob/master/UnityEngine/UnityEngine/Plane.cs
		private struct ProjectionPlane
		{
			private readonly Vector3 m_Normal;
			private readonly float m_Distance;

			public ProjectionPlane(Vector3 inNormal, Vector3 inPoint)
			{
				m_Normal = Vector3.Normalize(inNormal);
				m_Distance = -Vector3.Dot(inNormal, inPoint);
			}

			public Vector3 ClosestPointOnPlane(Vector3 point)
			{
				float d = Vector3.Dot(m_Normal, point) + m_Distance;
				return point - m_Normal * d;
			}

			public float GetDistanceToPoint(Vector3 point)
			{
				float signedDistance = Vector3.Dot(m_Normal, point) + m_Distance;
				if (signedDistance < 0f)
				{
					signedDistance = -signedDistance;
				}

				return signedDistance;
			}
		}

		private class CameraSetup
		{
			private Vector3 position;
			private Quaternion rotation;

			private RenderTexture targetTexture;

			private Color backgroundColor;
			private bool orthographic;
			private float orthographicSize;
			private float nearClipPlane;
			private float farClipPlane;
			private float aspect;
			private CameraClearFlags clearFlags;

			public void GetSetup(Camera camera)
			{
				position = camera.transform.position;
				rotation = camera.transform.rotation;

				targetTexture = camera.targetTexture;

				backgroundColor = camera.backgroundColor;
				orthographic = camera.orthographic;
				orthographicSize = camera.orthographicSize;
				nearClipPlane = camera.nearClipPlane;
				farClipPlane = camera.farClipPlane;
				aspect = camera.aspect;
				clearFlags = camera.clearFlags;
			}

			public void ApplySetup(Camera camera)
			{
				camera.transform.position = position;
				camera.transform.rotation = rotation;

				camera.targetTexture = targetTexture;

				camera.backgroundColor = backgroundColor;
				camera.orthographic = orthographic;
				camera.orthographicSize = orthographicSize;
				camera.nearClipPlane = nearClipPlane;
				camera.farClipPlane = farClipPlane;
				camera.aspect = aspect;
				camera.clearFlags = clearFlags;

				targetTexture = null;

				camera.cameraType = CameraType.Preview;
				camera.scene = m_Scene;
			}
		}

		private static readonly Scene m_Scene;

		private const int PREVIEW_LAYER = 22;

		private static Camera renderCamera;
		private static CameraSetup cameraSetup = new CameraSetup();

		private static List<Renderer> renderersList = new List<Renderer>(64);
		private static List<int> layersList = new List<int>(64);

		private static float aspect;
		private static float minX, maxX, minY, maxY;
		private static float maxDistance;

		private static Vector3 boundsCenter;
		private static ProjectionPlane projectionPlaneHorizontal, projectionPlaneVertical;

		private static Camera m_internalCamera = null;
		private static Camera InternalCamera
		{
			get
			{
				if (m_internalCamera == null)
				{
					m_internalCamera = new GameObject("ModelPreviewGeneratorCamera").AddComponent<Camera>();
					m_internalCamera.enabled = false;
					m_internalCamera.nearClipPlane = 0.01f;
					m_internalCamera.cullingMask = 1 << PREVIEW_LAYER;
					m_internalCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
				}

				return m_internalCamera;
			}
		}
		public static Camera PreviewRenderCamera { get; set; }

		private static Vector3 m_previewDirection;
		public static Vector3 PreviewDirection
		{
			get { return m_previewDirection; }
			set { m_previewDirection = value.normalized; }
		}

		private static float m_padding;
		public static float Padding
		{
			get { return m_padding; }
			set { m_padding = Mathf.Clamp(value, -0.25f, 0.25f); }
		}
		public static Color BackgroundColor { get; set; }
		public static bool OrthographicMode { get; set; }
		public static bool TransparentBackground { get; set; }

		static RuntimePreviewGenerator()
		{
			m_Scene = EditorSceneManager.NewPreviewScene();

			var Light0 = CreateLight();
			Light0.transform.rotation = Quaternion.Euler(70, 180, 0);
			Light0.color = new Color(1.0f, 0.95f, 0.9f);
			Light0.intensity = 1.15f;

			var Light1 = CreateLight();
			Light1.transform.rotation = Quaternion.Euler(340, 218, 177);
			Light1.color = new Color(.4f, .4f, .45f, 0f) * .7f;
			Light1.intensity = 1;

			PreviewRenderCamera = null;
			PreviewDirection = new Vector3(-1f, -1f, -1f);
			Padding = 0f;
			BackgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
			OrthographicMode = false;
			TransparentBackground = false;
		}

		private static Light CreateLight()
		{
			var lightGO = EditorUtility.CreateGameObjectWithHideFlags("PreRenderLight", HideFlags.HideAndDontSave, typeof(Light));
			var light = lightGO.GetComponent<Light>();
			light.type = LightType.Directional;
			light.intensity = 1.0f;
			light.enabled = true;
			SceneManager.MoveGameObjectToScene(lightGO, m_Scene);

			return light;
		}

		public static Texture2D GenerateModel(Transform model, int width = 64, int height = 64)
		{
			if (model == null || model.Equals(null))
			{
				return null;
			}

			Texture2D result = null;

			Transform previewObject;

			previewObject = Object.Instantiate(model, null, false);
			previewObject.gameObject.hideFlags = HideFlags.HideAndDontSave;

			SceneManager.MoveGameObjectToScene(previewObject.gameObject, m_Scene);
			previewObject.transform.position = Vector3.zero;

			bool wasActive = previewObject.gameObject.activeSelf;
			var prevPos = previewObject.position;
			var prevRot = previewObject.rotation;

			try
			{
				SetupCamera();
				SetLayerRecursively(previewObject);

				if (!wasActive)
				{
					previewObject.gameObject.SetActive(true);
				}

				var previewDir = previewObject.rotation * m_previewDirection;

				renderersList.Clear();
				previewObject.GetComponentsInChildren(renderersList);

				var previewBounds = new Bounds();
				bool init = false;
				for (int i = 0; i < renderersList.Count; i++)
				{
					if (!renderersList[i].enabled)
					{
						continue;
					}

					if (!init)
					{
						previewBounds = renderersList[i].bounds;
						init = true;
					}
					else
					{
						previewBounds.Encapsulate(renderersList[i].bounds);
					}
				}

				if (!init)
				{
					return null;
				}

				boundsCenter = previewBounds.center;
				var boundsExtents = previewBounds.extents;
				var boundsSize = 2f * boundsExtents;

				aspect = (float)width / height;
				renderCamera.aspect = aspect;
				renderCamera.transform.rotation = Quaternion.LookRotation(previewDir, previewObject.up);

				float distance;
				if (OrthographicMode)
				{
					renderCamera.transform.position = boundsCenter;

					minX = minY = Mathf.Infinity;
					maxX = maxY = Mathf.NegativeInfinity;

					var point = boundsCenter + boundsExtents;
					ProjectBoundingBoxMinMax(point);
					point.x -= boundsSize.x;
					ProjectBoundingBoxMinMax(point);
					point.y -= boundsSize.y;
					ProjectBoundingBoxMinMax(point);
					point.x += boundsSize.x;
					ProjectBoundingBoxMinMax(point);
					point.z -= boundsSize.z;
					ProjectBoundingBoxMinMax(point);
					point.x -= boundsSize.x;
					ProjectBoundingBoxMinMax(point);
					point.y += boundsSize.y;
					ProjectBoundingBoxMinMax(point);
					point.x += boundsSize.x;
					ProjectBoundingBoxMinMax(point);

					distance = boundsExtents.magnitude + 1f;
					renderCamera.orthographicSize = (1f + m_padding * 2f) * Mathf.Max(maxY - minY, (maxX - minX) / aspect) * 0.5f;
				}
				else
				{
					projectionPlaneHorizontal = new ProjectionPlane(renderCamera.transform.up, boundsCenter);
					projectionPlaneVertical = new ProjectionPlane(renderCamera.transform.right, boundsCenter);

					maxDistance = Mathf.NegativeInfinity;

					var point = boundsCenter + boundsExtents;
					CalculateMaxDistance(point);
					point.x -= boundsSize.x;
					CalculateMaxDistance(point);
					point.y -= boundsSize.y;
					CalculateMaxDistance(point);
					point.x += boundsSize.x;
					CalculateMaxDistance(point);
					point.z -= boundsSize.z;
					CalculateMaxDistance(point);
					point.x -= boundsSize.x;
					CalculateMaxDistance(point);
					point.y += boundsSize.y;
					CalculateMaxDistance(point);
					point.x += boundsSize.x;
					CalculateMaxDistance(point);

					distance = (1f + m_padding * 2f) * Mathf.Sqrt(maxDistance);
				}

				renderCamera.transform.position = boundsCenter - previewDir * distance;
				renderCamera.farClipPlane = distance * 4f;

				var temp = RenderTexture.active;
				var renderTex = RenderTexture.GetTemporary(width, height, 16);
				RenderTexture.active = renderTex;

				if (TransparentBackground)
				{
					GL.Clear(false, true, BackgroundColor);
				}

				renderCamera.targetTexture = renderTex;

				renderCamera.Render();
				renderCamera.targetTexture = null;

				result = new Texture2D(width, height, TransparentBackground ? TextureFormat.RGBA32 : TextureFormat.RGB24, false);
				result.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
				result.Apply(false, true);

				RenderTexture.active = temp;
				RenderTexture.ReleaseTemporary(renderTex);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			finally
			{
				Object.DestroyImmediate(previewObject.gameObject);

				if (renderCamera == PreviewRenderCamera)
				{
					cameraSetup.ApplySetup(renderCamera);
				}
			}

			return result;
		}

		private static void SetupCamera()
		{
			if (PreviewRenderCamera != null && !PreviewRenderCamera.Equals(null))
			{
				cameraSetup.GetSetup(PreviewRenderCamera);

				renderCamera = PreviewRenderCamera;
				renderCamera.nearClipPlane = 0.01f;
			}
			else
			{
				renderCamera = InternalCamera;
			}

			renderCamera.scene = m_Scene;
			renderCamera.backgroundColor = BackgroundColor;
			renderCamera.orthographic = OrthographicMode;
			renderCamera.clearFlags = TransparentBackground ? CameraClearFlags.Depth : CameraClearFlags.Color;
		}

		private static void ProjectBoundingBoxMinMax(Vector3 point)
		{
			var localPoint = renderCamera.transform.InverseTransformPoint(point);
			if (localPoint.x < minX)
			{
				minX = localPoint.x;
			}

			if (localPoint.x > maxX)
			{
				maxX = localPoint.x;
			}

			if (localPoint.y < minY)
			{
				minY = localPoint.y;
			}

			if (localPoint.y > maxY)
			{
				maxY = localPoint.y;
			}
		}

		private static void CalculateMaxDistance(Vector3 point)
		{
			var intersectionPoint = projectionPlaneHorizontal.ClosestPointOnPlane(point);

			float horizontalDistance = projectionPlaneHorizontal.GetDistanceToPoint(point);
			float verticalDistance = projectionPlaneVertical.GetDistanceToPoint(point);

			float halfFrustumHeight = Mathf.Max(verticalDistance, horizontalDistance / aspect);
			float distance = halfFrustumHeight / Mathf.Tan(renderCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);

			float distanceToCenter = (intersectionPoint - m_previewDirection * distance - boundsCenter).sqrMagnitude;
			if (distanceToCenter > maxDistance)
			{
				maxDistance = distanceToCenter;
			}
		}

		private static void SetLayerRecursively(Transform obj)
		{
			obj.gameObject.layer = PREVIEW_LAYER;
			for (int i = 0; i < obj.childCount; i++)
			{
				SetLayerRecursively(obj.GetChild(i));
			}
		}

		private static void GetLayerRecursively(Transform obj)
		{
			layersList.Add(obj.gameObject.layer);
			for (int i = 0; i < obj.childCount; i++)
			{
				GetLayerRecursively(obj.GetChild(i));
			}
		}

		private static void SetLayerRecursively(Transform obj, ref int index)
		{
			obj.gameObject.layer = layersList[index++];
			for (int i = 0; i < obj.childCount; i++)
			{
				SetLayerRecursively(obj.GetChild(i), ref index);
			}
		}
	}
}
#endif
