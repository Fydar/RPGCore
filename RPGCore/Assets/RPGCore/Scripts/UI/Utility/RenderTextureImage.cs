using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Utility
{
	[RequireComponent (typeof (RawImage))]
	public class RenderTextureImage : MonoBehaviour
	{
		private enum RTDepth
		{
			None = 0,
			Standard = 16,
			SencilBuffer = 24
		}

		[SerializeField] private Camera targetCamera;

		[Space]
		[SerializeField] private RenderTextureFormat format = RenderTextureFormat.ARGB32;
		[SerializeField] private RTDepth depth = RTDepth.Standard;
		[SerializeField] private int Antialiasing = 4;

		private RawImage targetImage;
		private int lastWidth = -1;
		private int lastHeight = -1;

		private void Awake ()
		{
			targetImage = GetComponent<RawImage> ();
			targetImage.enabled = true;
		}

		private void Start ()
		{
			RefreshRenderTexture ();
		}

		private void Update ()
		{
			if (lastWidth != Screen.width || lastHeight != Screen.height)
			{
				RefreshRenderTexture ();
			}
		}

		private void RefreshRenderTexture ()
		{
			RenderTexture newRT = new RenderTexture (Screen.width, Screen.height, (int)depth, format);
			newRT.antiAliasing = Antialiasing;

			RenderTexture oldRT = targetCamera.targetTexture;

			targetCamera.targetTexture = newRT;
			targetImage.texture = newRT;

			if (oldRT != null)
				oldRT.Release ();

			lastWidth = Screen.width;
			lastHeight = Screen.height;
		}
	}
}