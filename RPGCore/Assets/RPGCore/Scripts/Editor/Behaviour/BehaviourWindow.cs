#if UNITY_EDITOR
#define HOVER_EFFECTS

using RPGCore.Behaviour.Connections;
using RPGCore.Utility.Editors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPGCore.Behaviour.Editor
{
	public class BehaviourWindow : EditorWindow
	{
		private const float TopRight_Icon = 54.0f;

		private static Event currentEvent;
		private Rect screenRect;
		private IBehaviourGraph targetGraph;

		private int selectedWindow = -1;
		private GUI.WindowFunction drawNodeGUI;

		private GenericMenu AddNodeMenu;
		private GenericMenu BasicNodeMenu;
		private Vector2 rightClickedPosition;
		private BehaviourNode rightClickedNode;

		private Vector2 dragging_Position = Vector2.zero;
		private bool dragging_IsDragging;

		[NonSerialized] private OutputSocket connection_Start;
		[NonSerialized] private InputSocket connection_End;
		[NonSerialized] private Vector2 connection_LastMousePosition;
		[NonSerialized] private bool connection_CanEdit = true;

		private List<ConnectionInformationAttribute> help_Info;
		private bool help_ShowToggle;
		private bool help_Faded = true;

		private bool screenshot_TakeScreenshot;
		private RenderTexture screenshot_RenderTexture;
		private RenderTexture screenshot_OldTexture;

		private int controlID;

		[MenuItem ("Window/Behaviour")]
		private static void ShowEditor ()
		{
			BehaviourWindow editor = GetWindow<BehaviourWindow> ();

			editor.Show ();
		}

		[OnOpenAsset (0)]
		public static bool OpenGraph (int instanceID, int line)
		{
			UnityEngine.Object targetObject = EditorUtility.InstanceIDToObject (instanceID);

			if (!typeof (IBehaviourGraph).IsAssignableFrom (targetObject.GetType ()))
				return false;

			BehaviourWindow editor = GetWindow<BehaviourWindow> ();
			editor.Show ();

			editor.OpenGraph ((IBehaviourGraph)targetObject);

			return true;
		}

		public void OpenGraph (IBehaviourGraph openGraph)
		{
			targetGraph = openGraph;
			dragging_Position = Vector2.zero;
			ResetView ();

			Repaint ();
		}

		public void CloseGraph ()
		{
			targetGraph = null;
		}

		private void OnEnable ()
		{
			wantsMouseMove = true;

			Undo.undoRedoPerformed += Repaint;

			if (EditorGUIUtility.isProSkin)
				titleContent = new GUIContent ("Behaviour", BehaviourGraphResources.Instance.DarkThemeIcon);
			else
				titleContent = new GUIContent ("Behaviour", BehaviourGraphResources.Instance.LightThemeIcon);

			BasicNodeMenu = new GenericMenu ();

			BasicNodeMenu.AddItem (new GUIContent ("Duplicate"), false, DuplicateNodeCallback);
			BasicNodeMenu.AddItem (new GUIContent ("Delete"), false, DeleteNodeCallback);
			BasicNodeMenu.AddSeparator ("");
			BasicNodeMenu.AddItem (new GUIContent ("Ping Source"), false, PingSourceCallback);
			BasicNodeMenu.AddItem (new GUIContent ("Open Script"), false, OpenScriptCallback);

			help_Info = new List<ConnectionInformationAttribute> ();

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies ();
			for (int a = 0; a < assemblies.Length; a++)
			{
				Assembly assembly = assemblies[a];
				Type[] types = assembly.GetTypes ();

				for (int t = 0; t < types.Length; t++)
				{
					Type type = types[t];

					if (type.IsAbstract)
						continue;

					if (typeof (BehaviourNode).IsAssignableFrom (type))
					{

					}
					else
					{
						object[] obj = type.GetCustomAttributes (typeof (ConnectionInformationAttribute), false);
						if (obj.Length == 1)
						{
							var attri = (ConnectionInformationAttribute)obj[0];
							attri.Type = type;
							help_Info.Add (attri);
						}
					}
				}
			}
		}

		private void OnGUI ()
		{
			if (screenshot_TakeScreenshot && currentEvent.type == EventType.Repaint)
			{
				screenshot_OldTexture = RenderTexture.active;
				screenshot_RenderTexture = new RenderTexture ((int)position.width, (int)position.height + 24, 16, RenderTextureFormat.ARGB32);
				RenderTexture.active = screenshot_RenderTexture;

				Color backgroundColor = EditorGUIUtility.isProSkin
					? new Color32 (56, 56, 56, 255)
					: new Color32 (194, 194, 194, 255);

				GL.Clear (true, true, backgroundColor);
			}

			screenRect = new Rect (0, EditorGUIUtility.singleLineHeight + 1,
				position.width, position.height - (EditorGUIUtility.singleLineHeight + 1));

			currentEvent = Event.current;

			if (currentEvent.type == EventType.MouseUp && dragging_IsDragging)
			{
				dragging_IsDragging = false;
				currentEvent.Use ();
			}

			HandleDragAndDrop (screenRect);

			DrawBackground (screenRect, dragging_Position);

			if (targetGraph == null)
			{
				DrawTopBar ();
				return;
			}

			connection_CanEdit = !Application.isPlaying;

			DrawConnections ();
			DrawCurrentConnections ();
			DrawNodes ();
			DrawSockets ();

			DrawHelp ();

			HandleInput ();

			DrawOverlay ();
			DrawTopBar ();

			if (screenshot_TakeScreenshot && currentEvent.type == EventType.Repaint)
			{
				Texture2D tex = new Texture2D (screenshot_RenderTexture.width, screenshot_RenderTexture.height - 18, TextureFormat.RGB24, false);

				tex.ReadPixels (new Rect (0, 18, screenshot_RenderTexture.width, screenshot_RenderTexture.height), 0, 0);
				tex.Apply ();

				var bytes = tex.EncodeToPNG ();
				File.WriteAllBytes ("Screenshots.png", bytes);

				screenshot_TakeScreenshot = false;

				RenderTexture.active = screenshot_OldTexture;
			}
		}
		private string OurTempSquareImageLocation ()
		{
			string r = Application.persistentDataPath + "/p.png";
			return r;
		}

		private void AddNodeCallback (object type)
		{
			AddNodeCallback (type, rightClickedPosition);
		}

		private void AddNodeCallback (object type, Vector2 mousePosition)
		{
			BehaviourNode node = (BehaviourNode)AddNode ((Type)type);

			node.Position = (mousePosition - dragging_Position) - (node.GetDiamentions () * 0.5f);

			Repaint ();
		}

		private void DuplicateNodeCallback ()
		{
			DuplicateNode (rightClickedNode);
		}

		private void DeleteNodeCallback ()
		{
			DeleteNode (rightClickedNode);
		}

		private void PingSourceCallback ()
		{
			EditorUtility.FocusProjectWindow ();
			EditorGUIUtility.PingObject (MonoScript.FromScriptableObject (rightClickedNode));
		}

		private void OpenScriptCallback ()
		{
			AssetDatabase.OpenAsset (MonoScript.FromScriptableObject (rightClickedNode));
		}

		private void DuplicateNode (BehaviourNode original)
		{
			BehaviourNode node = (BehaviourNode)DuplicateNode ((ScriptableObject)original);

			Vector2 diamentions = rightClickedNode.GetDiamentions ();
			node.Position += new Vector2 (diamentions.x * 0.25f, diamentions.y * 0.5f);

			Repaint ();
		}

		private void DrawNodes ()
		{
			if (Event.current.type == EventType.MouseMove)
				return;

			if (drawNodeGUI == null)
				drawNodeGUI = new GUI.WindowFunction (DrawNodeWindow);

			Color originalColor = GUI.color;

			BeginWindows ();

			for (int i = 0; i < targetGraph.AllNodes.Count; i++)
			{
				BehaviourNode node = targetGraph.AllNodes[i];

				if (node == null)
				{
					targetGraph.AllNodes.RemoveAt (i);
					EndWindows ();
					return;
				}

				if (dragging_IsDragging && Event.current.type == EventType.MouseDrag)
					return;

				Vector2 contentSize = node.GetDiamentions ();

				GUI.color = originalColor;
				Rect newRect = GUI.Window (i, new Rect (node.Position.x + dragging_Position.x,
					node.Position.y + dragging_Position.y, contentSize.x,
					contentSize.y + 22), drawNodeGUI, new GUIContent (node.name));

				node.LastRect = newRect;

				node.Position.x = newRect.x - dragging_Position.x;
				node.Position.y = newRect.y - dragging_Position.y;
			}

			GUI.color = originalColor;
			EndWindows ();
			GUI.color = originalColor;
		}

		private void DrawNodeWindow (int id)
		{
			if (Event.current.type == EventType.Layout)
				return;

			BehaviourNode node = targetGraph.AllNodes[id];

			if (EventType.MouseDown == currentEvent.type || EventType.MouseUp == currentEvent.type ||
				EventType.MouseDrag == currentEvent.type || EventType.MouseMove == currentEvent.type)
			{
				selectedWindow = id;
				EditorUtility.SetDirty (node);
			}

			if (!node.LastRect.Overlaps (screenRect))
				return;

			Rect settingsRect = new Rect (node.LastRect.width - 20, 5, 20, 20);
			Rect iconRect = new Rect (0, 0, 18, 18);

			if (GUI.Button (settingsRect, "", BehaviourGUIStyles.Instance.settingsStyle))
			{
				rightClickedNode = node;
				BasicNodeMenu.ShowAsContext ();
			}

			Color originalColour = GUI.color;
			GUI.color = new Color (1.0f, 1.0f, 1.0f, 0.65f);
			EditorGUI.LabelField (iconRect, new GUIContent (AssetPreview.GetMiniThumbnail (node)));
			GUI.color = originalColour;

			if (currentEvent.type == EventType.MouseDown)
			{
				if (iconRect.Contains (currentEvent.mousePosition))
				{
					if (currentEvent.button == 0)
					{
						if (currentEvent.clickCount == 1)
						{
							rightClickedNode = node;
							PingSourceCallback ();
						}
						else
						{
							rightClickedNode = node;
							OpenScriptCallback ();
						}
						currentEvent.Use ();
					}
				}
			}

			SerializedObject serializedObject = SerializedObjectPool.Grab (node);

			//Undo.RecordObject (node, "Edit Node");

			float originalLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = node.LastRect.width / 2;

			serializedObject.FindProperty ("Position").vector2Value = node.Position;

			//EditorGUI.BeginChangeCheck ();
			//string newName = EditorGUILayout.DelayedTextField (node.name);
			//if (EditorGUI.EndChangeCheck ())
			//{
			//	RenameAction (node, newName);
			//}

			Rect contentRect = BehaviourGraphResources.Instance.NodeStyle.padding.Remove (node.LastRect);

			node.DrawGUI (serializedObject, new Rect (contentRect.x - node.Position.x - dragging_Position.x,
				contentRect.y - node.Position.y - dragging_Position.y,
				contentRect.width, contentRect.height));

			serializedObject.ApplyModifiedProperties ();

			if (currentEvent.type == EventType.MouseDown)
			{
				if (currentEvent.button == 1)
				{
					rightClickedNode = node;
					BasicNodeMenu.ShowAsContext ();
				}
			}

			EditorGUIUtility.labelWidth = originalLabelWidth;

#if HOVER_EFFECTS
			if (connection_Start == null && connection_End == null)
			{
				EditorGUIUtility.AddCursorRect (new Rect (node.LastRect.x - node.Position.x - dragging_Position.x,
					node.LastRect.y - node.Position.y - dragging_Position.y,
					node.LastRect.width, node.LastRect.height), MouseCursor.MoveArrow);
			}
#endif

			if (currentEvent.button != 2)
			{
				GUI.DragWindow ();
			}
		}

		private void HandleInput ()
		{
			if (currentEvent.type == EventType.KeyDown)
			{
				if (currentEvent.keyCode == KeyCode.D)
				{
					if (selectedWindow != -1)
						DuplicateNode (targetGraph.AllNodes[selectedWindow]);
				}
				else if (currentEvent.keyCode == KeyCode.Delete)
				{
					if (selectedWindow != -1)
						DeleteNode (targetGraph.AllNodes[selectedWindow]);
				}
			}
			else if (currentEvent.type == EventType.MouseDrag && dragging_IsDragging)
			{
				dragging_Position += currentEvent.delta;
				Repaint ();
			}
			else if (screenRect.Contains (currentEvent.mousePosition))
			{
				if (currentEvent.type == EventType.MouseDown)
				{
					GUI.UnfocusWindow ();
					GUI.FocusControl ("");

					if (currentEvent.button != 2)
					{
						if (connection_Start != null || connection_End != null)
						{
							connection_Start = null;
							connection_End = null;

							currentEvent.Use ();
							Repaint ();
						}

						if (currentEvent.button == 1)
						{
							dragging_IsDragging = false;
							rightClickedPosition = currentEvent.mousePosition;
							BuildAddMenu ();
							AddNodeMenu.ShowAsContext ();
						}
						else
						{
							dragging_IsDragging = true;
						}
					}
					else
					{
						dragging_IsDragging = true;
					}

					currentEvent.Use ();
					Repaint ();
				}
			}
		}

		private void DrawCurrentConnections ()
		{
			if (connection_Start == null && connection_End == null)
				return;

			if (connection_LastMousePosition != Event.current.mousePosition)
			{
				connection_LastMousePosition = Event.current.mousePosition;

				Repaint ();
			}

			if (connection_Start != null)
			{
				connection_Start.DrawConnection (new Vector2 (connection_Start.socketRect.xMin + dragging_Position.x,
					connection_Start.socketRect.center.y + dragging_Position.y)
					+ connection_Start.ParentNode.Position, currentEvent.mousePosition, Vector3.right, Vector3.left);
			}
			else if (connection_End != null)
			{
				connection_End.DrawConnection (new Vector2 (connection_End.SocketRect.xMax + dragging_Position.x,
					connection_End.SocketRect.center.y + dragging_Position.y)
					+ connection_End.ParentNode.Position, currentEvent.mousePosition, Vector3.left, Vector3.right);
			}
		}

		private void DrawConnections ()
		{
			if (Event.current.type == EventType.MouseMove)
				return;

			if (Event.current.type != EventType.Repaint)
				return;

			for (int i = 0; i < targetGraph.AllNodes.Count; i++)
			{
				BehaviourNode node = targetGraph.AllNodes[i];

				if (node == null)
					continue;

				foreach (InputSocket inputSocket in node.Inputs)
				{
					if (inputSocket == null)
						continue;

					OutputSocket outputSocket = inputSocket.SourceSocket;
					if (outputSocket == null)
						continue;

					Rect inputSocketRect = new Rect (
						inputSocket.SocketRect.x + node.Position.x + dragging_Position.x,
						inputSocket.SocketRect.y + node.Position.y + dragging_Position.y,
						inputSocket.SocketRect.width, inputSocket.SocketRect.height);

					Rect outputSocketRect = new Rect (
						outputSocket.socketRect.x + inputSocket.SourceNode.Position.x + dragging_Position.x,
						outputSocket.socketRect.y + inputSocket.SourceNode.Position.y + dragging_Position.y,
						outputSocket.socketRect.width, outputSocket.socketRect.height);

					inputSocket.DrawConnection (new Vector3 (inputSocketRect.xMax, inputSocketRect.center.y),
						new Vector3 (outputSocketRect.xMin, outputSocketRect.center.y),
						Vector3.left, Vector3.right);
				}
			}
		}

		private void DrawSockets ()
		{
			Color originalColor = GUI.color;
			for (int i = 0; i < targetGraph.AllNodes.Count; i++)
			{
				BehaviourNode node = targetGraph.AllNodes[i];

				if (node == null)
					continue;

				foreach (InputSocket thisInput in node.Inputs)
				{
					if (thisInput == null)
						continue;

					Rect socketRect = thisInput.SocketRect;
					socketRect = new Rect (socketRect.x + node.Position.x + dragging_Position.x,
						socketRect.y + node.Position.y + dragging_Position.y,
						socketRect.width, socketRect.height);

					thisInput.DrawSocket (socketRect);

#if HOVER_EFFECTS
					if (connection_CanEdit)
					{
						OutputSocket linkedSocket = thisInput.SourceSocket;
						if (connection_Start != null)
						{
							if (IsValidAttach (connection_Start, thisInput))
							{
								if (linkedSocket == null)
									EditorGUIUtility.AddCursorRect (socketRect, MouseCursor.ArrowPlus);
								else
									EditorGUIUtility.AddCursorRect (socketRect, MouseCursor.ArrowMinus);
							}
						}
						else
						{
							if (linkedSocket == null)
								EditorGUIUtility.AddCursorRect (socketRect, MouseCursor.ArrowPlus);
							else
								EditorGUIUtility.AddCursorRect (socketRect, MouseCursor.ArrowMinus);
						}
					}
#endif
					if (connection_CanEdit && (currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseUp) &&
						socketRect.Contains (currentEvent.mousePosition))
					{
						if (currentEvent.button == 0)
						{
							if (connection_Start != null || currentEvent.type != EventType.MouseUp)
								connection_End = thisInput;

							if (connection_Start != null)
							{
								Attach (connection_Start, connection_End);

								connection_End = null;
								connection_Start = null;
								currentEvent.Use ();
							}
						}
						else if (currentEvent.button == 1)
						{
							Detatch (thisInput);
						}

						currentEvent.Use ();
					}
				}

				foreach (OutputSocket thisOutput in node.Outputs)
				{
					if (thisOutput == null)
						continue;

					Rect socketRect = thisOutput.socketRect;
					socketRect = new Rect (socketRect.x + node.Position.x + dragging_Position.x,
						socketRect.y + node.Position.y + dragging_Position.y,
						socketRect.width, socketRect.height);

					thisOutput.DrawSocket (socketRect);

					if (connection_CanEdit)
					{
#if HOVER_EFFECTS
						if (connection_End != null)
						{
							if (IsValidAttach (thisOutput, connection_End))
							{
								EditorGUIUtility.AddCursorRect (socketRect, MouseCursor.ArrowPlus);
							}
						}
						else
						{
							EditorGUIUtility.AddCursorRect (socketRect, MouseCursor.ArrowPlus);
						}
#endif
						if ((currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseUp) &&
							socketRect.Contains (currentEvent.mousePosition))
						{
							if (currentEvent.button == 0)
							{
								if (connection_End != null || currentEvent.type != EventType.MouseUp)
									connection_Start = thisOutput;

								if (connection_End != null)
								{
									Attach (connection_Start, connection_End);

									connection_End = null;
									connection_Start = null;
								}

								currentEvent.Use ();
							}
							else if (currentEvent.button == 1)
							{
								Detatch (thisOutput);

								Repaint ();
								currentEvent.Use ();
							}
						}
					}
				}
			}
			GUI.color = originalColor;
		}

		private bool IsValidAttach (OutputSocket start, InputSocket end)
		{
			if (start.GetType () == typeof (EventOutput))
			{
				return end.GetType () == typeof (EventInput);
			}
			else if (end.GetType () == typeof (EventInput))
				return false;

			if (start.GetType () == typeof (OutputSocket)
				|| end.GetType () == typeof (InputSocket))
			{
				return true;
			}

			Type endInputType = typeof (ISocketConvertable<>).MakeGenericType (end.GetType ().BaseType.GetGenericArguments ()[0]);
			Type outputConnection = start.GetType ().BaseType.GetGenericArguments ()[2];

			return endInputType.IsAssignableFrom (outputConnection);
		}

		private void Attach (OutputSocket start, InputSocket end)
		{
			if (!IsValidAttach (start, end))
				return;

			SerializedObject obj = SerializedObjectPool.Grab (end.ParentNode);

			SerializedProperty socketProperty = obj.FindProperty (end.SocketName);

			SerializedProperty sourceNodeProperty = socketProperty.FindPropertyRelative ("SourceNode");
			SerializedProperty sourceFieldProperty = socketProperty.FindPropertyRelative ("SourceField");

			sourceNodeProperty.objectReferenceValue = start.ParentNode;
			sourceFieldProperty.stringValue = start.SocketName;

			obj.ApplyModifiedProperties ();

			Repaint ();
		}

		private void Detatch (InputSocket socket)
		{
			SerializedObject obj = SerializedObjectPool.Grab (socket.ParentNode);

			SerializedProperty socketProperty = obj.FindProperty (socket.SocketName);

			SerializedProperty sourceNodeProperty = socketProperty.FindPropertyRelative ("SourceNode");
			SerializedProperty sourceFieldProperty = socketProperty.FindPropertyRelative ("SourceField");

			sourceNodeProperty.objectReferenceValue = null;
			sourceFieldProperty.stringValue = "";

			obj.ApplyModifiedProperties ();

			Repaint ();
		}

		private void Detatch (OutputSocket socket)
		{
			foreach (BehaviourNode detachNode in targetGraph.AllNodes)
			{
				foreach (InputSocket detachInput in detachNode.Inputs)
				{
					if (detachInput.SourceNode == socket.ParentNode &&
						detachInput.SourceField == socket.SocketName)
					{
						SerializedObject detatchObject = SerializedObjectPool.Grab (detachNode);

						SerializedProperty detatchSocketProperty = detatchObject.FindProperty (detachInput.SocketName);

						SerializedProperty detatchSourceNodeProperty = detatchSocketProperty.FindPropertyRelative ("SourceNode");
						SerializedProperty detatchSourceFieldProperty = detatchSocketProperty.FindPropertyRelative ("SourceField");

						detatchSourceNodeProperty.objectReferenceValue = null;
						detatchSourceFieldProperty.stringValue = "";

						detatchObject.ApplyModifiedProperties ();
					}
				}
			}
			Repaint ();
		}

		private void HandleDragAndDrop (Rect dropArea)
		{
			if (dropArea.Contains (Event.current.mousePosition))
			{
				if (!IsValidImport (DragAndDrop.objectReferences))
				{
					// DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
					return;
				}

				DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

				if (Event.current.type == EventType.DragPerform)
				{
					DragAndDrop.AcceptDrag ();

					for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
					{
						object importObject = DragAndDrop.objectReferences[i];
						if (importObject.GetType () == typeof (MonoScript))
						{
							MonoScript monoScriptImport = (MonoScript)importObject;

							Type monoScriptType = monoScriptImport.GetClass ();
							if (monoScriptType != null &&
								typeof (BehaviourNode).IsAssignableFrom (monoScriptType))
							{
								AddNodeCallback (monoScriptType, Event.current.mousePosition);
							}
						}
						else if (typeof (IBehaviourGraph).IsAssignableFrom (importObject.GetType ()))
						{
							IBehaviourGraph graphImport = (IBehaviourGraph)importObject;

							OpenGraph (graphImport);
						}
					}
				}
			}
		}

		private bool IsValidImport (object[] objects)
		{
			if (objects.Length == 0)
				return false;

			for (int i = 0; i < objects.Length; i++)
			{
				object importObject = objects[i];

				if (importObject.GetType () == typeof (MonoScript))
				{
					MonoScript monoScriptImport = (MonoScript)importObject;

					Type monoScriptType = monoScriptImport.GetClass ();
					if (monoScriptType == null ||
						!typeof (BehaviourNode).IsAssignableFrom (monoScriptType))
						return false;
				}
				else if (!typeof (IBehaviourGraph).IsAssignableFrom (importObject.GetType ()))
				{
					return false;
				}
			}
			return true;
		}

		private void DrawTopBar ()
		{
			EditorGUILayout.BeginHorizontal (EditorStyles.toolbar, GUILayout.ExpandWidth (true));

			if (GUILayout.Button ("Load Graph", EditorStyles.toolbarButton, GUILayout.Width (100)))
			{
				controlID = GUIUtility.GetControlID (FocusType.Passive);
				EditorGUIUtility.ShowObjectPicker<ScriptableObject> ((ScriptableObject)targetGraph, false, "", controlID);
			}

			string commandName = currentEvent.commandName;
			if (commandName == "ObjectSelectorUpdated")
			{
				Repaint ();
			}
			else if (commandName == "ObjectSelectorClosed")
			{
				if (EditorGUIUtility.GetObjectPickerControlID () == controlID)
				{
					IBehaviourGraph nextGraph = EditorGUIUtility.GetObjectPickerObject () as IBehaviourGraph;

					if (nextGraph == null)
					{
						// CloseGraph ();
					}
					else
					{
						if (targetGraph != nextGraph)
						{
							OpenGraph (((UnityEngine.Object)nextGraph).GetInstanceID (), 0);
						}
					}
					return;
				}
			}

			GUILayout.Space (5.0f);

			EditorGUI.BeginDisabledGroup (targetGraph == null);
			if (GUILayout.Button ("Goto Asset", EditorStyles.toolbarButton, GUILayout.Width (100)))
			{
				EditorUtility.FocusProjectWindow ();
				EditorGUIUtility.PingObject ((UnityEngine.Object)targetGraph);
			}
			EditorGUI.EndDisabledGroup ();

			GUILayout.FlexibleSpace ();

			if (GUILayout.Button ("Screenshot", EditorStyles.toolbarButton, GUILayout.Width (100)))
			{
				screenshot_TakeScreenshot = true;
				Repaint ();
				EditorApplication.delayCall += Repaint;
			}
			EditorGUILayout.EndHorizontal ();
		}

		private void DrawHelp ()
		{
			float height;
			float width;

			if (help_ShowToggle)
			{
				height = (help_Info.Count * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing)) + 28;
				width = 120;
			}
			else
			{
				height = 29;
				width = 80;
			}

			Rect helpArea = new Rect (screenRect.xMax - width, screenRect.yMax - height, width - 8, height - 8);

			if (helpArea.Contains (Event.current.mousePosition) && !dragging_IsDragging)
			{
				if (!help_Faded)
				{
					help_Faded = true;
					Repaint ();
				}
				if (Event.current.type == EventType.MouseDown)
				{
					help_ShowToggle = !help_ShowToggle;
				}
				GUI.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			}
			else
			{
				if (help_Faded)
				{
					help_Faded = false;
					Repaint ();
				}

				if (help_ShowToggle)
					GUI.color = new Color (1.0f, 1.0f, 1.0f, 0.65f);
				else
					GUI.color = new Color (1.0f, 1.0f, 1.0f, 0.35f);
			}

			if (Event.current.type == EventType.Repaint)
			{
				GUI.skin.window.Draw (helpArea, false, false, false, false);

				Rect headerRect = new Rect (helpArea.x, helpArea.y + 0, helpArea.width, EditorGUIUtility.singleLineHeight);

				EditorGUI.LabelField (headerRect, "Help", BehaviourGUIStyles.Instance.HelpHeaderTextStyle);
			}

			if (Event.current.type == EventType.Repaint && help_ShowToggle)
			{
				Rect marchingRect = new Rect (helpArea.x, helpArea.y + 18, helpArea.width, EditorGUIUtility.singleLineHeight);

				for (int i = 0; i < help_Info.Count; i++)
				{
					var information = help_Info[i];

					Rect socketRect = new Rect (marchingRect.x + 4, marchingRect.y, marchingRect.height, marchingRect.height);
					Rect infoRect = new Rect (socketRect.xMax + 4, marchingRect.y, marchingRect.xMax - socketRect.xMax, marchingRect.height);

					Color originalColour = GUI.color;
					GUI.color = (Color)information.Type.GetField ("SocketColour", BindingFlags.Static | BindingFlags.Public).GetValue (null);
					GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, originalColour.a);

					EditorStyles.helpBox.Draw (socketRect, false, false, false, false);

					GUI.color = originalColour;

					EditorGUI.LabelField (infoRect, information.Name);

					marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;
				}
			}
			GUI.color = Color.white;
		}

		private void DrawOverlay ()
		{
			if (targetGraph != null)
			{
				float padding = 1;
				Rect topRight = new Rect (screenRect.xMax - TopRight_Icon - 8, screenRect.y + 8, TopRight_Icon, TopRight_Icon);
				Rect iconRect = new Rect (topRight.x + padding, topRight.y + padding, topRight.width - (padding * 2), topRight.height - (padding * 2));

				Texture graphIcon = AssetPreview.GetAssetPreview ((UnityEngine.Object)targetGraph);
				if (graphIcon == null)
					graphIcon = AssetPreview.GetMiniThumbnail ((UnityEngine.Object)targetGraph);

				GUI.Box (topRight, "", EditorStyles.textArea);
				GUI.DrawTexture (iconRect, graphIcon);
			}
		}

		private void DrawBackground (Rect backgroundRect, Vector2 viewPosition)
		{
			if (Event.current.type == EventType.MouseMove)
				return;

			if (targetGraph == null)
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

		private static void DrawImageTiled (Rect rect, Texture2D texture, Vector2 positon, float zoom = 0.8f)
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

		private void ResetView ()
		{
			if (targetGraph.AllNodes == null || targetGraph.AllNodes.Count == 0)
			{
				dragging_Position = Vector2.zero;
				return;
			}

			Rect bounds = new Rect (targetGraph.AllNodes[0].Position.x, targetGraph.AllNodes[0].Position.y, 0, 0);

			for (int i = 0; i < targetGraph.AllNodes.Count; i++)
			{
				var node = targetGraph.AllNodes[i];
				Vector2 viewPosition = targetGraph.AllNodes[i].Position;
				bounds.xMin = Mathf.Min (bounds.xMin, viewPosition.x);
				bounds.xMax = Mathf.Max (bounds.xMax, viewPosition.x + node.GetDiamentions ().x);

				bounds.yMin = Mathf.Min (bounds.yMin, viewPosition.y);
				bounds.yMax = Mathf.Max (bounds.yMax, viewPosition.y + node.GetDiamentions ().y);
			}

			dragging_Position = new Vector2 (screenRect.width * 0.5f, screenRect.height * 0.5f) - Vector2Int.RoundToInt (bounds.center);
		}

		private void RenameNode (UnityEngine.Object action, string newName)
		{
			action.name = newName;

			ReloadSubassets ();

			EditorUtility.SetDirty (action);
		}

		private void DeleteNode (UnityEngine.Object action)
		{
			DestroyImmediate (action, true);

			ReloadSubassets ();

			EditorUtility.SetDirty ((UnityEngine.Object)targetGraph);
			Repaint ();
		}

		private T Duplicate<T> (T original)
			where T : BehaviourNode
		{
			return (T)DuplicateNode ((ScriptableObject)original);
		}

		private ScriptableObject DuplicateNode (ScriptableObject original)
		{
			ScriptableObject action = Instantiate (original) as ScriptableObject;
			action.name = original.name;
			action.hideFlags = HideFlags.HideInHierarchy;

			AssetDatabase.AddObjectToAsset (action, (UnityEngine.Object)targetGraph);

			ReloadSubassets ();

			return action;
		}

		private T AddNode<T> ()
			where T : BehaviourNode
		{
			return (T)AddNode (typeof (T));
		}

		private ScriptableObject AddNode (Type type)
		{
			ScriptableObject action = CreateInstance (type);
			action.name = ObjectNames.NicifyVariableName (type.Name).Replace (" Node", "");
			action.hideFlags = HideFlags.HideInHierarchy;

			AssetDatabase.AddObjectToAsset (action, (UnityEngine.Object)targetGraph);

			ReloadSubassets ();

			return action;
		}

		private void ReloadSubassets ()
		{
			UnityEngine.Object[] objects = AssetDatabase.LoadAllAssetsAtPath (
				AssetDatabase.GetAssetPath ((UnityEngine.Object)targetGraph));

			List<BehaviourNode> actions = new List<BehaviourNode> (objects.Length);

			for (int i = 0; i < objects.Length; i++)
			{
				UnityEngine.Object obj = objects[i];

				if (obj == null)
					continue;

				if (typeof (BehaviourNode).IsAssignableFrom (obj.GetType ()))
				{
					actions.Add ((BehaviourNode)obj);
				}
			}

			targetGraph.AllNodes = actions;

			EditorUtility.SetDirty ((UnityEngine.Object)targetGraph);
		}
		private void BuildAddMenu ()
		{
			AddNodeMenu = new GenericMenu ();

			var MenuStructure = new Dictionary<string, Dictionary<string, List<Type>>> ();

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies ();

			for (int a = 0; a < assemblies.Length; a++)
			{
				Assembly assembly = assemblies[a];
				Type[] types = assembly.GetTypes ();

				for (int t = 0; t < types.Length; t++)
				{
					Type type = types[t];

					if (type.IsAbstract)
						continue;

					if (typeof (BehaviourNode).IsAssignableFrom (type))
					{
						string elementName;
						string nodeGroup = "";

						object[] obj = type.GetCustomAttributes (typeof (NodeInformationAttribute), false);
						if (obj.Length == 0)
						{
							elementName = ObjectNames.NicifyVariableName (type.Name).Replace (" Node", "");
							if (elementName.Contains ("Input"))
								nodeGroup = "Input";
						}
						else
						{
							NodeInformationAttribute attribute = (NodeInformationAttribute)obj[0];
							nodeGroup = attribute.Group;
							elementName = attribute.Name;
						}

						int lastSeperator = elementName.LastIndexOf ('/');
						string nodePath;
						if (lastSeperator == -1)
						{
							nodePath = "";
						}
						else
						{
							nodePath = elementName.Substring (0, lastSeperator);
						}

						Dictionary<string, List<Type>> groupsDictionary;
						bool result = MenuStructure.TryGetValue (nodePath, out groupsDictionary);
						if (!result)
						{
							groupsDictionary = new Dictionary<string, List<Type>> ();
							MenuStructure.Add (nodePath, groupsDictionary);
						}

						List<Type> group;
						result = groupsDictionary.TryGetValue (nodeGroup, out group);
						if (!result)
						{
							group = new List<Type> ();
							groupsDictionary.Add (nodeGroup, group);
						}

						group.Add (type);
					}
				}
			}

			foreach (var path in MenuStructure)
			{
				bool seperated = false;
				foreach (var group in path.Value)
				{
					if (seperated)
						AddNodeMenu.AddSeparator (path.Key + "/");
					else
						seperated = true;

					foreach (var node in group.Value)
					{
						object[] obj = node.GetCustomAttributes (typeof (NodeInformationAttribute), false);

						if (obj.Length == 0)
						{
							string elementName = ObjectNames.NicifyVariableName (node.Name).Replace (" Node", "");

							AddNodeMenu.AddItem (new GUIContent (elementName),
									false, AddNodeCallback, node);
						}
						else
						{
							NodeInformationAttribute nodeInformation = (NodeInformationAttribute)obj[0];

							if (nodeInformation.OnlyOne && targetGraph.GetNode (node) != null)
							{
								AddNodeMenu.AddDisabledItem (new GUIContent (nodeInformation.Name));
							}
							else
							{
								AddNodeMenu.AddItem (new GUIContent (nodeInformation.Name),
										false, AddNodeCallback, node);
							}
						}
					}
				}
			}
		}
	}
}
#endif
