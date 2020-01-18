using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
using RPGCore.Demo.Nodes;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class BehaviourEditor : EditorWindow
	{
		public BehaviourEditorView View;

		private ProjectImport CurrentPackage;
		private ProjectResource CurrentResource;
		private Rect ScreenRect;
		private Event CurrentEvent;
		private readonly JsonSerializer Serializer = JsonSerializer.Create (new JsonSerializerSettings ()
		{
			Converters = new List<JsonConverter> ()
			{
				new LocalPropertyIdJsonConverter()
			}
		});

		[MenuItem ("Window/Behaviour")]
		public static void Open()
		{
			var window = GetWindow<BehaviourEditor> ();

			window.Show ();
		}

		private void OnEnable()
		{
			if (EditorGUIUtility.isProSkin)
			{
				titleContent = new GUIContent ("Behaviour", BehaviourGraphResources.Instance.DarkThemeIcon);
			}
			else
			{
				titleContent = new GUIContent ("Behaviour", BehaviourGraphResources.Instance.LightThemeIcon);
			}
		}

		private void OnGUI()
		{
			if (View == null)
			{
				View = new BehaviourEditorView ();
			}

			ScreenRect = new Rect (0, EditorGUIUtility.singleLineHeight + 1,
				position.width, position.height - (EditorGUIUtility.singleLineHeight + 1));

			CurrentEvent = Event.current;

			DrawBackground (ScreenRect, View.PanPosition);
			DrawTopBar ();
			DrawAssetSelection ();

			DrawNodes ();
			DrawConnections ();
			HandleInput ();
		}

		private void DrawAssetSelection()
		{
			CurrentPackage = (ProjectImport)EditorGUILayout.ObjectField (CurrentPackage, typeof (ProjectImport), true);
			if (CurrentPackage == null)
			{
				return;
			}

			var explorer = CurrentPackage.Explorer;

			foreach (var resource in explorer.Resources)
			{
				if (!resource.Name.EndsWith (".bhvr"))
				{
					continue;
				}

				if (GUILayout.Button (resource.ToString ()))
				{
					CurrentResource = resource;
					JObject editorTarget;
					using (var editorTargetData = CurrentResource.LoadStream ())
					using (var sr = new StreamReader (editorTargetData))
					using (var reader = new JsonTextReader (sr))
					{
						editorTarget = JObject.Load (reader);
					}

					var nodes = NodeManifest.Construct (new Type[] {
							typeof (AddNode),
							typeof (RollNode),
							typeof (OutputValueNode),
							typeof (ItemInputNode),
							typeof (ActivatableItemNode),
							typeof (IterateNode),
							typeof (GetStatNode),
						});
					var types = TypeManifest.ConstructBaseTypes ();

					var manifest = new BehaviourManifest ()
					{
						Nodes = nodes,
						Types = types,
					};
					Debug.Log (editorTarget);
					var graphEditor = new EditorSession (manifest, editorTarget, "SerializedGraph", Serializer);

					View.BeginSession (graphEditor);
				}
			}
		}

		private void DrawNodes()
		{
			if (View.Session == null)
			{
				return;
			}
			var graphEditorNodes = View.Session.Root["Nodes"];

			// Draw Nodes
			foreach (var node in graphEditorNodes)
			{
				var nodeEditor = node["Editor"];
				var nodeEditorPosition = nodeEditor["Position"];

				var nodeRect = new Rect (
					View.PanPosition.x + nodeEditorPosition["x"].GetValue<int> (),
					View.PanPosition.y + nodeEditorPosition["y"].GetValue<int> (),
					200,
					1000
				);

				GUILayout.BeginArea (nodeRect);

				var finalRect = EditorGUILayout.BeginVertical ();
				finalRect.xMax -= 2;
				finalRect.yMax += 4;
				if (CurrentEvent.type == EventType.Repaint)
				{
					GUI.skin.window.Draw (finalRect,
						false, View.Selection.Contains (node.Name), false, false);
				}

				var nodeType = node["Type"];
				EditorGUILayout.LabelField (nodeType.GetValue<string> ());

				var nodeData = node["Data"];
				foreach (var childField in nodeData)
				{
					DrawField (childField);
				}
				EditorGUILayout.EndVertical ();
				GUILayout.EndArea ();

				if (CurrentEvent.type == EventType.MouseDown)
				{
					var globalFinalRect = new Rect (
						nodeRect.x,
						nodeRect.y,
						finalRect.width,
						finalRect.height
					);

					if (globalFinalRect.Contains (CurrentEvent.mousePosition))
					{
						View.Selection.Clear ();
						View.Selection.Add (node.Name);

						View.CurrentMode = BehaviourEditorView.Mode.NodeDragging;
						GUI.UnfocusWindow ();
						GUI.FocusControl ("");

						CurrentEvent.Use ();
					}
				}
			}
		}

		public void DrawConnections()
		{
			if (View.Session == null)
			{
				return;
			}

			if (CurrentEvent.type != EventType.Repaint
				&& CurrentEvent.type != EventType.MouseDown
				&& CurrentEvent.type != EventType.MouseUp)
			{
				return;
			}

			var graphEditorNodes = View.Session.Root["Nodes"];

			// Foreach output
			foreach (var node in graphEditorNodes)
			{
				var nodeEditor = node["Editor"];
				var nodeEditorPosition = nodeEditor["Position"];

				float nodePositionX = nodeEditorPosition["x"].GetValue<int> () + View.PanPosition.x;
				float nodePositionY = nodeEditorPosition["y"].GetValue<int> () + View.PanPosition.y;

				var nodeData = node["Data"];

				// Foreach Output
				var nodeInfo = (NodeInformation)nodeData.Type;
				if (nodeInfo?.Outputs != null)
				{
					var outputRect = new Rect (nodePositionX + 202, nodePositionY + 6, 20, 20);
					foreach (var output in nodeInfo.Outputs)
					{
						if (CurrentEvent.type == EventType.Repaint)
						{
							EditorStyles.helpBox.Draw (outputRect, false, false, false, false);
						}
						else if (CurrentEvent.type == EventType.MouseDown && outputRect.Contains (CurrentEvent.mousePosition))
						{
							var outputId = new LocalPropertyId (new LocalId (node.Name), output.Key);
							View.BeginConnectionFromOutput (outputId);

							GUI.UnfocusWindow ();
							GUI.FocusControl ("");

							CurrentEvent.Use ();
						}
						else if (CurrentEvent.type == EventType.MouseUp && outputRect.Contains (CurrentEvent.mousePosition))
						{
							if (View.CurrentMode == BehaviourEditorView.Mode.CreatingConnection)
							{
								if (!View.IsOutputSocket)
								{
									var thisOutputSocket = new LocalPropertyId (new LocalId (node.Name), output.Key);

									View.ConnectionInput.SetValue (thisOutputSocket);
									View.ConnectionInput.ApplyModifiedProperties ();
									View.CurrentMode = BehaviourEditorView.Mode.None;

									GUI.UnfocusWindow ();
									GUI.FocusControl ("");

									CurrentEvent.Use ();
								}
							}
						}

						outputRect.y += outputRect.height + 4;
					}
				}

				// Foreach Input
				foreach (var childField in nodeData)
				{
					if (childField.Field.Type != "InputSocket")
					{
						continue;
					}
					var fieldFeature = childField.GetOrCreateFeature<FieldFeature> ();

					fieldFeature.GlobalRenderedPosition = new Rect (
						fieldFeature.LocalRenderedPosition.x + nodePositionX,
						fieldFeature.LocalRenderedPosition.y + nodePositionY,
						fieldFeature.LocalRenderedPosition.width,
						fieldFeature.LocalRenderedPosition.height);

					var socketRect = fieldFeature.InputSocketPosition;

					if (CurrentEvent.type == EventType.MouseDown && socketRect.Contains (CurrentEvent.mousePosition))
					{
						var thisInputId = new LocalPropertyId (new LocalId (node.Name), childField.Name);

						View.BeginConnectionFromInput (childField, node.Name);

						GUI.UnfocusWindow ();
						GUI.FocusControl ("");

						CurrentEvent.Use ();
					}
					else if (CurrentEvent.type == EventType.MouseUp && socketRect.Contains (CurrentEvent.mousePosition))
					{
						if (View.CurrentMode == BehaviourEditorView.Mode.CreatingConnection)
						{
							if (View.IsOutputSocket)
							{
								childField.SetValue (View.ConnectionOutput);
								childField.ApplyModifiedProperties ();
								View.CurrentMode = BehaviourEditorView.Mode.None;

								GUI.UnfocusWindow ();
								GUI.FocusControl ("");

								CurrentEvent.Use ();
							}
						}
					}
					else if (CurrentEvent.type == EventType.Repaint)
					{
						EditorStyles.helpBox.Draw (socketRect, false, false, false, false);

						var thisInputConnectedTo = childField.GetValue<LocalPropertyId> ();
						if (thisInputConnectedTo != LocalPropertyId.None)
						{
							bool foundNode = false;
							var otherOutputRect = new Rect ();
							foreach (var otherNode in graphEditorNodes)
							{
								var otherNodeEditor = otherNode["Editor"];
								var otherNodeEditorPosition = otherNodeEditor["Position"];

								float otherNodePositionX = otherNodeEditorPosition["x"].GetValue<int> () + View.PanPosition.x;
								float otherNodePositionY = otherNodeEditorPosition["y"].GetValue<int> () + View.PanPosition.y;

								var otherNodeData = otherNode["Data"];

								// Foreach Output
								otherOutputRect = new Rect (otherNodePositionX + 202, otherNodePositionY + 6, 20, 20);
								var otherNodeInfo = (NodeInformation)otherNodeData.Type;
								foreach (var output in otherNodeInfo.Outputs)
								{
									var otherOutputId = new LocalPropertyId (new LocalId (otherNode.Name), output.Key);

									if (otherOutputId == thisInputConnectedTo)
									{
										foundNode = true;
										break;
									}

									otherOutputRect.y += otherOutputRect.height + 4;
								}
								if (foundNode)
								{
									break;
								}
							}
							if (foundNode)
							{
								var start = new Vector3 (otherOutputRect.x, otherOutputRect.center.y);
								var end = new Vector3 (fieldFeature.GlobalRenderedPosition.x, fieldFeature.GlobalRenderedPosition.center.y);
								var startDir = new Vector3 (1, 0);
								var endDir = new Vector3 (-1, 0);

								DrawConnection (start, end, startDir, endDir);
							}
						}
					}
				}
			}

			// Draw active connection
			if (View.CurrentMode == BehaviourEditorView.Mode.CreatingConnection)
			{
				if (View.IsOutputSocket)
				{
					// Draw Nodes
					bool isFound = false;
					var outputRect = new Rect ();
					foreach (var node in graphEditorNodes)
					{
						var nodeEditor = node["Editor"];
						var nodeEditorPosition = nodeEditor["Position"];

						float nodePositionX = nodeEditorPosition["x"].GetValue<int> () + View.PanPosition.x;
						float nodePositionY = nodeEditorPosition["y"].GetValue<int> () + View.PanPosition.y;

						var nodeData = node["Data"];

						// Foreach Output
						var nodeInfo = (NodeInformation)nodeData.Type;
						outputRect = new Rect (nodePositionX + 202, nodePositionY + 6, 20, 20);
						foreach (var output in nodeInfo.Outputs)
						{
							var otherOutputId = new LocalPropertyId (new LocalId (node.Name), output.Key);

							if (otherOutputId == View.ConnectionOutput)
							{
								isFound = true;
								break;
							}

							outputRect.y += outputRect.height + 4;
						}
						if (isFound)
						{
							break;
						}
					}

					if (isFound)
					{
						var start = new Vector3 (outputRect.x, outputRect.center.y);
						var end = new Vector3 (CurrentEvent.mousePosition.x, CurrentEvent.mousePosition.y);
						var startDir = new Vector3 (1, 0);
						var endDir = new Vector3 (-1, 0);

						DrawConnection (start, end, startDir, endDir);
					}
				}
				else
				{
					var startFieldFeature = View.ConnectionInput.GetOrCreateFeature<FieldFeature> ();

					var inputNode = graphEditorNodes[View.ConnectionInputNodeId.ToString ()];
					var nodeEditor = inputNode["Editor"];
					var nodeEditorPosition = nodeEditor["Position"];

					float nodePositionX = nodeEditorPosition["x"].GetValue<int> () + View.PanPosition.x;
					float nodePositionY = nodeEditorPosition["y"].GetValue<int> () + View.PanPosition.y;

					var nodeData = inputNode["Data"];

					var socketRect = startFieldFeature.InputSocketPosition;

					var start = new Vector3 (CurrentEvent.mousePosition.x, CurrentEvent.mousePosition.y);
					var end = new Vector3 (socketRect.xMax, socketRect.center.y);
					var startDir = new Vector3 (1, 0);
					var endDir = new Vector3 (-1, 0);

					DrawConnection (start, end, startDir, endDir);
				}
			}
		}

		public static void DrawEditor(EditorSession editor)
		{
			foreach (var field in editor.Root)
			{
				DrawField (field);
			}
		}

		public static void DrawField(EditorField field)
		{
			// EditorGUILayout.LabelField(field.Json.Path);
			if (field.Field.Format == FieldFormat.List)
			{
				var fieldFeature = field.GetOrCreateFeature<FieldFeature> ();

				fieldFeature.Expanded = EditorGUILayout.Foldout (fieldFeature.Expanded, field.Name, true);

				if (fieldFeature.Expanded)
				{
					EditorGUI.indentLevel++;

					EditorGUI.BeginChangeCheck ();
					int newIndex = EditorGUILayout.DelayedIntField ("Size", field.Count);
					if (EditorGUI.EndChangeCheck ())
					{

					}

					foreach (var childField in field)
					{
						DrawField (childField);
					}
					EditorGUI.indentLevel--;
				}
			}
			else if (field.Field.Type == "Int32")
			{
				EditorGUI.BeginChangeCheck ();
				int newValue = EditorGUILayout.IntField (field.Name, field.GetValue<int> ());
				if (EditorGUI.EndChangeCheck ())
				{
					field.SetValue (newValue);
					field.ApplyModifiedProperties ();
				}
			}
			else if (field.Field.Type == "String")
			{
				EditorGUI.BeginChangeCheck ();
				string newValue = EditorGUILayout.TextField (field.Name, field.GetValue<string> ());
				if (EditorGUI.EndChangeCheck ())
				{
					field.SetValue (newValue);
					field.ApplyModifiedProperties ();
				}
			}
			else if (field.Field.Type == "Boolean")
			{
				EditorGUI.BeginChangeCheck ();
				bool newValue = EditorGUILayout.Toggle (field.Name, field.GetValue<bool> ());
				if (EditorGUI.EndChangeCheck ())
				{
					field.SetValue (newValue);
					field.ApplyModifiedProperties ();
				}
			}
			else if (field.Field.Type == "InputSocket")
			{
				var fieldFeature = field.GetOrCreateFeature<FieldFeature> ();

				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.LabelField (field.Name, field.GetValue<LocalPropertyId> ().ToString ());
				var renderPos = GUILayoutUtility.GetLastRect ();
				fieldFeature.LocalRenderedPosition = renderPos;
				if (EditorGUI.EndChangeCheck ())
				{
					//field.Json.Value = newValue;
				}

				// EditorGUI.DrawRect(renderPos, Color.red);
			}
			else if (field.Field.Format == FieldFormat.Dictionary)
			{
				EditorGUILayout.LabelField (field.Name);

				EditorGUI.indentLevel++;
				foreach (var childField in field)
				{
					DrawField (childField);
				}
				EditorGUI.indentLevel--;
			}
			else if (field.Field.Format == FieldFormat.Object)
			{
				var fieldFeature = field.GetOrCreateFeature<FieldFeature> ();

				fieldFeature.Expanded = EditorGUILayout.Foldout (fieldFeature.Expanded, field.Name, true);

				if (fieldFeature.Expanded)
				{
					EditorGUI.indentLevel++;
					foreach (var childField in field)
					{
						DrawField (childField);
					}
					EditorGUI.indentLevel--;
				}
			}
			else
			{
				EditorGUILayout.LabelField (field.Name, "Unknown Type");
			}
		}

		private static void DrawConnection(Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
		{
			float distance = Vector3.Distance (start, end);
			var startTan = start + (startDir * distance * 0.5f);
			var endTan = end + (endDir * distance * 0.5f);

			var connectionColour = new Color (1.0f, 0.8f, 0.8f);
			Handles.DrawBezier (start, end, startTan, endTan, connectionColour,
				BehaviourGraphResources.Instance.SmallConnection, 10);
		}

		private void HandleInput()
		{
			if (CurrentEvent.type == EventType.MouseUp)
			{
				switch (View.CurrentMode)
				{
					case BehaviourEditorView.Mode.NodeDragging:
						CurrentEvent.Use ();

						foreach (string selectedNode in View.Selection)
						{
							var pos = View.Session.Root["Nodes"][selectedNode]["Editor"]["Position"];

							var posX = pos["x"];
							posX.ApplyModifiedProperties ();

							var posY = pos["y"];
							posY.ApplyModifiedProperties ();
						}
						View.CurrentMode = BehaviourEditorView.Mode.None;
						break;

					case BehaviourEditorView.Mode.ViewDragging:
						View.CurrentMode = BehaviourEditorView.Mode.None;
						break;

					case BehaviourEditorView.Mode.CreatingConnection:
						View.CurrentMode = BehaviourEditorView.Mode.None;
						break;
				}
				Repaint ();
			}
			else if (CurrentEvent.type == EventType.KeyDown)
			{

			}
			else if (CurrentEvent.type == EventType.MouseDrag)
			{
				switch (View.CurrentMode)
				{
					case BehaviourEditorView.Mode.NodeDragging:
						foreach (string selectedNode in View.Selection)
						{
							var pos = View.Session.Root["Nodes"][selectedNode]["Editor"]["Position"];

							var posX = pos["x"];
							posX.SetValue (posX.GetValue<int> () + ((int)CurrentEvent.delta.x));

							var posY = pos["y"];
							posY.SetValue (posY.GetValue<int> () + ((int)CurrentEvent.delta.y));
						}
						break;

					case BehaviourEditorView.Mode.ViewDragging:
						View.PanPosition += CurrentEvent.delta;
						break;
				}
				Repaint ();
			}
			else if (CurrentEvent.type == EventType.MouseDown)
			{
				if (ScreenRect.Contains (CurrentEvent.mousePosition))
				{
					GUI.UnfocusWindow ();
					GUI.FocusControl ("");

					View.CurrentMode = BehaviourEditorView.Mode.ViewDragging;

					CurrentEvent.Use ();
					Repaint ();
				}
			}
		}

		private void DrawBackground(Rect backgroundRect, Vector2 viewPosition)
		{
			if (CurrentEvent.type != EventType.Repaint)
			{
				return;
			}

			if (View.Session == null)
			{
				EditorGUI.LabelField (backgroundRect, "No Graph Selected", BehaviourGUIStyles.Instance.informationTextStyle);
				return;
			}

#if HOVER_EFFECTS
			if (dragging_IsDragging)
			{
				EditorGUIUtility.AddCursorRect (backgroundRect, MouseCursor.Pan);
			}
#endif

			/*if (Application.isPlaying)
				EditorGUI.DrawRect (screenRect, new Color (0.7f, 0.7f, 0.7f));*/

			float gridScale = 0.5f;

			DrawImageTiled (backgroundRect, BehaviourGraphResources.Instance.WindowBackground, viewPosition, gridScale * 3);

			var originalTintColour = GUI.color;

			GUI.color = new Color (1, 1, 1, 0.6f);
			DrawImageTiled (backgroundRect, BehaviourGraphResources.Instance.WindowBackground, viewPosition, gridScale);

			GUI.color = originalTintColour;

			if (Application.isPlaying)
			{
				var runtimeInfo = new Rect (backgroundRect);
				runtimeInfo.yMin = runtimeInfo.yMax - 48;
				EditorGUI.LabelField (runtimeInfo, "Playmode Enabled: You may change values but you can't edit connections",
					BehaviourGUIStyles.Instance.informationTextStyle);
			}
		}

		private void DrawImageTiled(Rect rect, Texture2D texture, Vector2 positon, float zoom = 0.8f)
		{
			if (texture == null)
			{
				return;
			}

			if (CurrentEvent.type != EventType.Repaint)
			{
				return;
			}

			var tileOffset = new Vector2 ((-positon.x / texture.width) * zoom, (positon.y / texture.height) * zoom);

			var tileAmount = new Vector2 (Mathf.Round (rect.width * zoom) / texture.width,
				Mathf.Round (rect.height * zoom) / texture.height);

			tileOffset.y -= tileAmount.y;
			GUI.DrawTextureWithTexCoords (rect, texture, new Rect (tileOffset, tileAmount), true);
		}

		private void DrawTopBar()
		{
			EditorGUILayout.BeginHorizontal (EditorStyles.toolbar, GUILayout.ExpandWidth (true));

			if (GUILayout.Button (CurrentPackage?.name, EditorStyles.toolbarButton, GUILayout.Width (100)))
			{
			}

			GUILayout.Space (6);

			if (GUILayout.Button ("Save", EditorStyles.toolbarButton, GUILayout.Width (100)))
			{
				using (var file = CurrentResource.WriteStream ())
				{
					Serializer.Serialize (
						new JsonTextWriter (file)
						{
							Formatting = Formatting.Indented
						},
						View.Session.Instance
					);
				}
			}

			if (GUILayout.Button (View.DescribeCurrentAction, EditorStyles.toolbarButton))
			{
			}

			EditorGUILayout.EndHorizontal ();
		}
	}
}
