#define HOVER_EFFECTS

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Reflection;
using System.Collections.Generic;
using RPGCore.Utility.Editors;

namespace RPGCore
{
	public class BehaviourWindow : EditorWindow
	{
		private const float TopRight_Icon = 54.0f;

		private static Event currentEvent;
		private Rect screenRect;
		private BehaviourGraph targetGraph = null;

		private int selectedWindow = -1;
		private GUI.WindowFunction drawNodeGUI;

		private GenericMenu AddNodeMenu;
		private GenericMenu BasicNodeMenu;
		private Vector2 rightClickedPosition;
		private BehaviourNode rightClickedNode = null;

		private Vector2 dragging_Position = Vector2.zero;
		private bool dragging_IsDragging = false;

		[NonSerialized] private OutputSocket connection_Start;
		[NonSerialized] private InputSocket connection_End;
		[NonSerialized] private Vector2 connection_LastMousePosition;
		[NonSerialized] private bool connection_CanEdit = true;

		private List<ConnectionInformationAttribute> help_Info;
		private bool help_ShowToggle = false;
		private bool help_Faded = true;

		[MenuItem ("Window/Behaviour")]
		private static void ShowEditor ()
		{
			BehaviourWindow editor = EditorWindow.GetWindow<BehaviourWindow> ();

			editor.Show ();
		}

		[OnOpenAsset (0)]
		public static bool OpenGraph (int instanceID, int line)
		{
			UnityEngine.Object targetObject = EditorUtility.InstanceIDToObject (instanceID);

			if (!typeof (BehaviourGraph).IsAssignableFrom (targetObject.GetType ()))
				return false;

			BehaviourWindow editor = EditorWindow.GetWindow<BehaviourWindow> ();
			editor.Show ();

			editor.OpenGraph ((BehaviourGraph)targetObject);

			return true;
		}

		public void OpenGraph (BehaviourGraph openGraph)
		{
			targetGraph = openGraph;
			dragging_Position = Vector2.zero;

			Repaint ();
		}

		public void CloseGraph ()
		{
			targetGraph = null;
		}

		private void OnEnable ()
		{
			this.wantsMouseMove = true;

			Undo.undoRedoPerformed += Repaint;

			titleContent = new GUIContent ("Behaviour", BehaviourGraphResources.Instance.WindowIcon);

			BasicNodeMenu = new GenericMenu ();

			BasicNodeMenu.AddItem (new GUIContent ("Duplicate"), false, DuplicateNodeCallback);
			BasicNodeMenu.AddItem (new GUIContent ("Delete"), false, DeleteNodeCallback);
			BasicNodeMenu.AddSeparator ("");
			BasicNodeMenu.AddItem (new GUIContent ("Ping Source"), false, PingSourceCallback);
			BasicNodeMenu.AddItem (new GUIContent ("Open Script"), false, OpenScriptCallback);

			AddNodeMenu = new GenericMenu ();

			var MenuStructure = new Dictionary<string, Dictionary<string, List<Type>>> ();
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
						string nodePath = elementName.Substring (0, lastSeperator);

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
						string elementName;

						object[] obj = node.GetCustomAttributes (typeof (NodeInformationAttribute), false);

						if (obj.Length == 0)
							elementName = ObjectNames.NicifyVariableName (node.Name).Replace (" Node", "");
						else
							elementName = ((NodeInformationAttribute)obj[0]).Name;

						AddNodeMenu.AddItem (new GUIContent (elementName),
							false, AddNodeCallback, node);
					}
				}
			}
		}

		private void OnGUI ()
		{
			screenRect = new Rect (0, EditorGUIUtility.singleLineHeight + 1,
				position.width, position.height - (EditorGUIUtility.singleLineHeight + 1));

			currentEvent = Event.current;

			if (currentEvent.type == EventType.MouseUp && dragging_IsDragging)
			{
				dragging_IsDragging = false;
				currentEvent.Use ();
			}

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

			HandleDragAndDrop (screenRect);

			HandleInput ();

			if (targetGraph != null)
			{
				float padding = 1;
				Rect topRight = new Rect (screenRect.xMax - TopRight_Icon - 8, screenRect.y + 8, TopRight_Icon, TopRight_Icon);
				Rect iconRect = new Rect (topRight.x + padding, topRight.y + padding, topRight.width - (padding * 2), topRight.height - (padding * 2));

				Texture graphIcon = AssetPreview.GetAssetPreview (targetGraph);
				if (graphIcon == null)
					graphIcon = AssetPreview.GetMiniThumbnail (targetGraph);

				GUI.Box (topRight, "", EditorStyles.textArea);
				GUI.DrawTexture (iconRect, graphIcon);
			}

			DrawTopBar ();
		}

		private void AddNodeCallback (object type)
		{
			AddNodeCallback (type, rightClickedPosition);
		}

		private void AddNodeCallback (object type, Vector2 mousePosition)
		{
			BehaviourNode node = (BehaviourNode)AddAction ((Type)type);

			node.Position = (mousePosition - dragging_Position) - (node.GetDiamentions () * 0.5f);

			Repaint ();
		}

		private void DuplicateNodeCallback ()
		{
			DuplicateNode (rightClickedNode);
		}

		private void DeleteNodeCallback ()
		{
			DeleteAction (rightClickedNode);
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
			BehaviourNode node = (BehaviourNode)DuplicateAction (original);

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

			for (int i = 0; i < targetGraph.Nodes.Count; i++)
			{
				BehaviourNode node = targetGraph.Nodes[i];

				if (node == null)
				{
					targetGraph.Nodes.RemoveAt (i);
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

			BehaviourNode node = targetGraph.Nodes[id];

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
						DuplicateNode (targetGraph.Nodes[selectedWindow]);
				}
				else if (currentEvent.keyCode == KeyCode.Delete)
				{
					if (selectedWindow != -1)
						DeleteAction (targetGraph.Nodes[selectedWindow]);
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
				connection_End.DrawConnection (new Vector2 (connection_End.socketRect.xMax + dragging_Position.x,
					connection_End.socketRect.center.y + dragging_Position.y)
					+ connection_End.ParentNode.Position, currentEvent.mousePosition, Vector3.left, Vector3.right);
			}
		}

		private void DrawConnections ()
		{
			if (Event.current.type == EventType.MouseMove)
				return;

			if (Event.current.type != EventType.Repaint)
				return;

			for (int i = 0; i < targetGraph.Nodes.Count; i++)
			{
				BehaviourNode node = targetGraph.Nodes[i];

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
						inputSocket.socketRect.x + node.Position.x + dragging_Position.x,
						inputSocket.socketRect.y + node.Position.y + dragging_Position.y,
						inputSocket.socketRect.width, inputSocket.socketRect.height);

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
			for (int i = 0; i < targetGraph.Nodes.Count; i++)
			{
				BehaviourNode node = targetGraph.Nodes[i];

				if (node == null)
					continue;

				foreach (InputSocket thisInput in node.Inputs)
				{
					if (thisInput == null)
						continue;

					Rect socketRect = thisInput.socketRect;
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

			Type endInputType = typeof (ISocketConvertable<>).MakeGenericType (end.GetType ().BaseType.GenericTypeArguments[0]);
			Type outputConnection = start.GetType ().BaseType.GenericTypeArguments[2];

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
			foreach (BehaviourNode detachNode in targetGraph.Nodes)
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
					DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
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
						else if (typeof (BehaviourGraph).IsAssignableFrom (importObject.GetType ()))
						{
							BehaviourGraph graphImport = (BehaviourGraph)importObject;

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
				else if (!typeof (BehaviourGraph).IsAssignableFrom (importObject.GetType ()))
				{
					return false;
				}
			}
			return true;
		}

		int controlID;

		private void DrawTopBar ()
		{
			EditorGUILayout.BeginHorizontal (EditorStyles.toolbar, GUILayout.ExpandWidth (true));

			if (GUILayout.Button ("Load Graph", EditorStyles.toolbarButton, GUILayout.Width (100)))
			{
				controlID = EditorGUIUtility.GetControlID (FocusType.Passive);
				EditorGUIUtility.ShowObjectPicker<BehaviourGraph> (targetGraph, false, "", controlID);
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
					BehaviourGraph nextGraph = EditorGUIUtility.GetObjectPickerObject () as BehaviourGraph;

					if (nextGraph == null)
					{
						// CloseGraph ();
					}
					else
					{
						if (targetGraph != nextGraph)
						{
							OpenGraph (nextGraph.GetInstanceID (), 0);
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
				EditorGUIUtility.PingObject (targetGraph);
			}
			EditorGUI.EndDisabledGroup ();

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

		private void DrawBackground (Rect screenRect, Vector2 viewPosition)
		{
			if (Event.current.type == EventType.MouseMove)
				return;

			if (targetGraph == null)
			{
				EditorGUI.LabelField (screenRect, "No Graph Selected", BehaviourGUIStyles.Instance.informationTextStyle);
				return;
			}

#if HOVER_EFFECTS
			if (dragging_IsDragging)
			{
				EditorGUIUtility.AddCursorRect (screenRect, MouseCursor.Pan);
			}
#endif

			/*if (Application.isPlaying)
				EditorGUI.DrawRect (screenRect, new Color (0.7f, 0.7f, 0.7f));*/

			float gridScale = 0.5f;

			DrawImageTiled (screenRect, BehaviourGraphResources.Instance.WindowBackground, viewPosition, gridScale * 3);

			Color originalTintColour = GUI.color;

			GUI.color = new Color (1, 1, 1, 0.6f);
			DrawImageTiled (screenRect, BehaviourGraphResources.Instance.WindowBackground, viewPosition, gridScale);

			GUI.color = originalTintColour;

			if (Application.isPlaying)
			{
				Rect runtimeInfo = new Rect (screenRect);
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
			GUI.DrawTextureWithTexCoords (rect, texture, new Rect (tileOffset, tileAmount));
		}



		private void RenameAction (UnityEngine.Object action, string newName)
		{
			action.name = newName;
			//AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (action));

			ReloadSubassets ();

			EditorUtility.SetDirty (action);
		}

		private void DeleteAction (UnityEngine.Object action)
		{
			DestroyImmediate (action, true);
			//AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (targetGraph));

			ReloadSubassets ();

			EditorUtility.SetDirty (targetGraph);
			Repaint ();
		}

		private T Duplicate<T> (T original)
			where T : BehaviourNode
		{
			return (T)DuplicateAction (original);
		}

		private ScriptableObject DuplicateAction (ScriptableObject original)
		{
			ScriptableObject action = Instantiate (original) as ScriptableObject;
			action.name = original.name;
			action.hideFlags = HideFlags.HideInHierarchy;

			AssetDatabase.AddObjectToAsset (action, targetGraph);
			//AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (action));

			ReloadSubassets ();

			return action;
		}

		private T AddAction<T> ()
			where T : BehaviourNode
		{
			return (T)AddAction (typeof (T));
		}

		private ScriptableObject AddAction (Type type)
		{
			ScriptableObject action = ScriptableObject.CreateInstance (type);
			action.name = ObjectNames.NicifyVariableName (type.Name).Replace (" Node", "");
			action.hideFlags = HideFlags.HideInHierarchy;

			AssetDatabase.AddObjectToAsset (action, targetGraph);

			//AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (action));

			ReloadSubassets ();

			return action;
		}

		private void ReloadSubassets ()
		{
			UnityEngine.Object[] objects = AssetDatabase.LoadAllAssetsAtPath (
				AssetDatabase.GetAssetPath (targetGraph));

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

			targetGraph.Nodes = actions;

			EditorUtility.SetDirty (targetGraph);
		}
	}

	/*private void DrawNodes ()
	{
		for (int i = 0; i < targetGraph.Nodes.Count; i++)
		{
			BehaviourNode node = targetGraph.Nodes[i];

			if (node == null)
			{
				targetGraph.Nodes.RemoveAt (i);
				return;
			}

			Vector2 contentSize = node.GetDiamentions ();
			Vector2 nodeScreenPosition = node.Position + viewPosition;

			Rect nodeRect = new Rect (nodeScreenPosition.x, nodeScreenPosition.y,
				contentSize.x, contentSize.y + 20);

			if (currentEvent.type == EventType.Repaint)
			{
				GUI.skin.window.Draw (nodeRect, false, false, false, false);
			}

			//EditorGUI.DrawRect (nodeRect, Color.green);

			DrawNode (nodeRect, node);
		}
	}

	private void DrawNode (Rect nodeRect, BehaviourNode node)
	{
		Event windowEvent = currentEvent;

		Rect settingsRect = new Rect (nodeRect.width - 20, 5, 20, 20);

		if (GUI.Button (settingsRect, "", BehaviourGUIStyles.Instance.settingsStyle))
		{
			rightClickedNode = node;
			BasicNodeMenu.ShowAsContext ();
		}

		SerializedObject serializedObject = SerializedObjectPool.Grab (node);

		float originalLabelWidth = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = nodeRect.width / 2;

		node.DrawGUI (serializedObject, new Rect (nodeRect.x, nodeRect.y + 20, 
			nodeRect.width, nodeRect.height - 22));

		serializedObject.ApplyModifiedProperties ();

		if (windowEvent.type == EventType.MouseDown)
		{
			if (windowEvent.button == 1)
			{
				rightClickedNode = node;
				BasicNodeMenu.ShowAsContext ();
			}
		}

		EditorGUIUtility.labelWidth = originalLabelWidth;
	}*/
}