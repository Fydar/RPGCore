using UnityEngine;
using UnityEditor;
using RPGCore.Packages;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Manifest;
using RPGCore.Behaviour.Editor;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RPGCore.Unity.Editors
{
	public class BehaviourEditor : EditorWindow
	{
		public string selectedNode = null;
		private Vector2 dragging_Position = Vector2.zero;
		private bool dragging_IsDragging;
		private bool dragging_NodeDragging;

		private Rect screenRect;

		private Event currentEvent;

		public ProjectImport CurrentPackage;

		public bool HasCurrentResource;
		public bool HasEditor;
		public ProjectResource CurrentResource;
		public JObject editorTarget;
		public EditorObject graphEditor;


		private JsonSerializer serializer = new JsonSerializer();

		[MenuItem("Window/Behaviour")]
		public static void Open()
		{
			var window = EditorWindow.GetWindow<BehaviourEditor>();

			window.Show();
		}

		private void OnEnable ()
		{
			if (EditorGUIUtility.isProSkin)
				titleContent = new GUIContent ("Behaviour", BehaviourGraphResources.Instance.DarkThemeIcon);
			else
				titleContent = new GUIContent ("Behaviour", BehaviourGraphResources.Instance.LightThemeIcon);
		}

		public void OnGUI()
		{
			screenRect = new Rect (0, EditorGUIUtility.singleLineHeight + 1,
				position.width, position.height - (EditorGUIUtility.singleLineHeight + 1));

			currentEvent = Event.current;

			if (currentEvent.type == EventType.MouseUp && dragging_IsDragging)
			{
				dragging_IsDragging = false;
				dragging_NodeDragging = false;
				currentEvent.Use ();
			}

			// HandleDragAndDrop (screenRect);


			DrawBackground (screenRect, dragging_Position);
			DrawTopBar ();




			CurrentPackage = (ProjectImport)EditorGUILayout.ObjectField(CurrentPackage, typeof(ProjectImport), true);

			var explorer = CurrentPackage.Explorer;

			foreach (var resource in explorer.Resources)
			{
				if (!resource.Name.EndsWith(".bhvr"))
				{
					continue;
				}

				if (GUILayout.Button(resource.ToString()))
				{
					CurrentResource = resource;
					HasCurrentResource = true;
					HasEditor = false;
				}
			}

			if (HasCurrentResource && CurrentResource != null)
			{
				if (HasEditor == false)
				{
					Debug.Log(CurrentResource);

					var editorTargetData = CurrentResource.LoadStream();

					using (var sr = new StreamReader(editorTargetData))
					using (var reader = new JsonTextReader(sr))
					{
						editorTarget = JObject.Load(reader);
						// editorTarget = serializer.Deserialize(reader);
					}

					var nodes = NodeManifest.Construct(new Type[] { typeof(AddNode), typeof(RollNode) });
					var types = TypeManifest.ConstructBaseTypes();

					var manifest = new BehaviourManifest()
					{
						Nodes = nodes,
						Types = types
					};

					graphEditor = new EditorObject(manifest, manifest.Types.ObjectTypes["SerializedGraph"], editorTarget);
					HasEditor = true;
				}

				if (GUILayout.Button("Save"))
				{
					using (var file = CurrentResource.WriteStream())
					{
						serializer.Serialize(new JsonTextWriter(file)
						{ Formatting = Formatting.Indented }, editorTarget);
					}
				}

				foreach (var node in graphEditor["Nodes"])
				{
					var nodeEditor = node["Editor"];
					var nodeEditorPosition = nodeEditor["Position"];

					var nodeRect = new Rect(
						dragging_Position.x + nodeEditorPosition["x"].Json.ToObject<int>(),
						dragging_Position.y + nodeEditorPosition["y"].Json.ToObject<int>(),
						200,
						160
					);

					bool startDrag = false;
					if (Event.current.type == EventType.Repaint)
					{
						BehaviourGraphResources.Instance.NodeStyle.Draw(nodeRect,
							false, node.Name == selectedNode, false, false);
					}
					else if (Event.current.type == EventType.MouseDown)
					{
						if (nodeRect.Contains(Event.current.mousePosition))
						{
							selectedNode = node.Name;
							dragging_IsDragging = true;
							dragging_NodeDragging = true;

							startDrag = true;
						}
					}

					GUILayout.BeginArea(nodeRect);

					var nodeData = node.Json["Data"];
					var nodeType = node["Type"];
					
					var fieldInformation = new FieldInformation();
					fieldInformation.Type = nodeType.Json.ToObject<string>();

					var field = new EditorField();
					TypeInformation typeInformation;
					NodeInformation nodeInformation;
					if (node.Manifest.Types.JsonTypes.TryGetValue(nodeType.Json.ToObject<string>(), out typeInformation))
					{
						field = new EditorField(node.Manifest, node.Name, fieldInformation, typeInformation, nodeData);
					}
					else if (node.Manifest.Types.ObjectTypes.TryGetValue(nodeType.Json.ToObject<string>(), out typeInformation))
					{
						field = new EditorField(node.Manifest, node.Name, fieldInformation, typeInformation, nodeData);
					}
					else if (node.Manifest.Nodes.Nodes.TryGetValue(nodeType.Json.ToObject<string>(), out nodeInformation))
					{
						field = new EditorField(node.Manifest, node.Name, fieldInformation, nodeInformation, nodeData);
					}

					foreach (var childField in field)
					{
						DrawField(childField);
					}

					if (startDrag)
					{
						GUI.UnfocusWindow ();
						GUI.FocusControl ("");

						Event.current.Use();
					}
					
					GUILayout.EndArea();
				}
			}
			
			HandleInput();
		}

		public static void DrawField(EditorField field)
		{
			// EditorGUILayout.LabelField(field.Json.Path);
			if (field.Field.Type == "Int32")
			{
				EditorGUI.BeginChangeCheck();
				int newValue = EditorGUILayout.IntField(field.Name, field.Json.ToObject<int>());
				if (EditorGUI.EndChangeCheck())
				{
					var replace = JToken.FromObject(newValue);
					field.Json.Replace(replace);
				}
			}
			else if (field.Field.Type == "String")
			{
				EditorGUI.BeginChangeCheck();
				string newValue = EditorGUILayout.TextField(field.Name, field.Json.ToObject<string>());
				if (EditorGUI.EndChangeCheck())
				{
					var replace = JToken.FromObject(newValue);
					field.Json.Replace(replace);
				}
			}
			else if (field.Field.Type == "Boolean")
			{
				EditorGUI.BeginChangeCheck();
				bool newValue = EditorGUILayout.Toggle(field.Name, field.Json.ToObject<bool>());
				if (EditorGUI.EndChangeCheck())
				{
					var replace = JToken.FromObject(newValue);
					field.Json.Replace(replace);
				}
			}
			else if (field.Field.Type == "InputSocket")
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.LabelField(field.Name, field.Json.ToObject<string>());
				if (EditorGUI.EndChangeCheck())
				{
					//field.Json.Value = newValue;
				}
			}
			else if (field.Field.Format == FieldFormat.Dictionary)
			{
				EditorGUILayout.LabelField(field.Name);

				EditorGUI.indentLevel++;
				foreach (var childField in field)
				{
					DrawField(childField);
				}
				EditorGUI.indentLevel--;
			}
			else if (field.Field != null)
			{
				EditorGUILayout.LabelField(field.Name);

				EditorGUI.indentLevel++;
				foreach (var childField in field)
				{
					DrawField(childField);
				}
				EditorGUI.indentLevel--;
			}
			else
			{
				EditorGUILayout.LabelField(field.Name, "Unknown Type");
			}
		}

		public static void DrawEditor(EditorObject editor)
		{
			foreach (var field in editor)
			{
				DrawField(field);
			}
		}

		
		private void HandleInput ()
		{
			if (currentEvent.type == EventType.KeyDown)
			{
				
			}
			else if (currentEvent.type == EventType.MouseDrag && dragging_IsDragging)
			{
				if (dragging_NodeDragging)
				{
					var pos = graphEditor["Nodes"].Json[selectedNode]["Editor"]["Position"];

					var replace = JToken.FromObject(pos["x"].ToObject<int>() + ((int)currentEvent.delta.x));
					pos["x"].Replace(replace);
					
					replace = JToken.FromObject(pos["y"].ToObject<int>() + ((int)currentEvent.delta.y));
					pos["y"].Replace(replace);
				}
				else
				{
					dragging_Position += currentEvent.delta;
				}
				Repaint ();
			}
			else if (currentEvent.type == EventType.MouseDown)
			{	
				if (screenRect.Contains (currentEvent.mousePosition))
				{
					GUI.UnfocusWindow ();
					GUI.FocusControl ("");

					dragging_IsDragging = true;
					dragging_NodeDragging = false;

					currentEvent.Use ();
					Repaint ();
				}
			}
		}


		private void DrawBackground (Rect backgroundRect, Vector2 viewPosition)
		{
			if (Event.current.type == EventType.MouseMove)
				return;

			if (!HasEditor)
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

			Color originalTintColour = GUI.color;

			GUI.color = new Color (1, 1, 1, 0.6f);
			DrawImageTiled (backgroundRect, BehaviourGraphResources.Instance.WindowBackground, viewPosition, gridScale);

			GUI.color = originalTintColour;

			if (Application.isPlaying)
			{
				Rect runtimeInfo = new Rect (backgroundRect);
				runtimeInfo.yMin = runtimeInfo.yMax - 48;
				EditorGUI.LabelField (runtimeInfo, "Playmode Enabled: You may change values but you can't edit connections",
					BehaviourGUIStyles.Instance.informationTextStyle);
			}
		}

		private void DrawImageTiled (Rect rect, Texture2D texture, Vector2 positon, float zoom = 0.8f)
		{
			if (texture == null)
				return;

			if (currentEvent.type != EventType.Repaint)
				return;

			Vector2 tileOffset = new Vector2 ((-positon.x / texture.width) * zoom, (positon.y / texture.height) * zoom);

			Vector2 tileAmount = new Vector2 (Mathf.Round (rect.width * zoom) / texture.width,
				Mathf.Round (rect.height * zoom) / texture.height);

			tileOffset.y -= tileAmount.y;
			GUI.DrawTextureWithTexCoords (rect, texture, new Rect (tileOffset, tileAmount), true);
		}







		private void DrawTopBar ()
		{
			EditorGUILayout.BeginHorizontal (EditorStyles.toolbar, GUILayout.ExpandWidth (true));

			if (GUILayout.Button (CurrentPackage?.name, EditorStyles.toolbarButton, GUILayout.Width (100)))
			{
			}

			
			if (GUILayout.Button (selectedNode + " " + dragging_NodeDragging, EditorStyles.toolbarButton, GUILayout.Width (100)))
			{
			}

			EditorGUILayout.EndHorizontal ();
		}
	}
}