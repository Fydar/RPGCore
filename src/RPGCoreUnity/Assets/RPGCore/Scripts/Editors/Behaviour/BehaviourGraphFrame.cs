﻿using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.DataEditor;
using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPGCoreUnity.Editors
{
	public class BehaviourGraphFrame : WindowFrame
	{
		public static Dictionary<string, GraphTypeInformation> StyleLookup = new Dictionary<string, GraphTypeInformation>()
		{
			["Single"] = new GraphTypeInformation()
			{
				ConnectionColor = new Color(0.65f, 0.65f, 0.65f),
				SocketColor = new Color(0.65f, 0.65f, 0.65f),
			},
			["Int32"] = new GraphTypeInformation()
			{
				ConnectionColor = new Color(0.85f, 0.85f, 0.85f),
				SocketColor = new Color(0.85f, 0.85f, 0.85f),
			},
			["DemoPlayer"] = new GraphTypeInformation()
			{
				ConnectionColor = new Color(0.6f, 0.6f, 0.85f),
				SocketColor = new Color(0.6f, 0.6f, 0.85f),
			},
			["Error"] = new GraphTypeInformation()
			{
				ConnectionColor = new Color(0.85f, 0.6f, 0.6f),
				SocketColor = new Color(0.85f, 0.6f, 0.6f),
			}
		};

		public BehaviourEditorView View;

		private Event currentEvent;
		private GUIStyle nodePadding;

		public override void OnEnable()
		{
		}

		public override void OnGUI()
		{
			if (View == null)
			{
				View = new BehaviourEditorView();
			}

			if (nodePadding == null)
			{
				nodePadding = new GUIStyle()
				{
					padding = new RectOffset()
					{
						left = 4,
						right = 4
					}
				};
			}

			currentEvent = Event.current;

			DrawBackground(Position, View.PanPosition);

			GUI.BeginClip(Position);
			DrawNodes();
			DrawConnections();
			GUI.EndClip();

			HandleInput();
		}

		private void DrawNodes()
		{
			if (View.Session == null)
			{
				return;
			}

			var graphEditorNodesField = View.EditorObject.Fields["Nodes"];
			var graphEditorNodes = graphEditorNodesField.Value as EditorDictionary;

			// Draw Nodes
			foreach (var nodeField in graphEditorNodes.KeyValuePairs)
			{
				var node = nodeField.Value.Value as EditorObject;

				var nodeEditor = node.Fields["Editor"].Value as EditorObject;
				var nodeType = node.Fields["Type"].Value as EditorValue;
				var nodeData = node.Fields["Data"].Value as EditorObject;
				var nodeEditorPosition = nodeEditor.Fields["Position"].Value as EditorObject;

				var nodeRect = new Rect(
					View.PanPosition.x + (nodeEditorPosition.Fields["x"].Value as EditorValue).GetValue<int>(),
					View.PanPosition.y + (nodeEditorPosition.Fields["y"].Value as EditorValue).GetValue<int>(),
					220,
					1000
				);

				GUILayout.BeginArea(nodeRect);

				var finalRect = EditorGUILayout.BeginVertical(nodePadding);
				// finalRect.xMax -= 2;
				finalRect.yMax += 4;
				if (currentEvent.type == EventType.Repaint)
				{
					GUI.skin.window.Draw(finalRect,
						false, View.Selection.Contains(nodeField.Key), false, false);
				}

				string nodeTypeString = nodeType.GetValue<string>();
				string nodeHeader = nodeTypeString?.Substring(nodeTypeString.LastIndexOf(".") + 1) ?? "Unknown";
				EditorGUILayout.LabelField(nodeHeader, EditorStyles.boldLabel);


				float originalLabelWidth = EditorGUIUtility.labelWidth;
				EditorGUIUtility.labelWidth = 115;
				EditorGUI.indentLevel++;
				if (nodeData.Fields != null)
				{
					foreach (var childField in nodeData.Fields.Values)
					{
						RPGCoreEditor.DrawField(childField);
					}
				}
				else
				{
					EditorGUILayout.LabelField("There are no fields here");
				}
				EditorGUI.indentLevel--;
				EditorGUIUtility.labelWidth = originalLabelWidth;

				EditorGUILayout.EndVertical();
				GUILayout.EndArea();

				var globalFinalRect = new Rect(
					nodeRect.x,
					nodeRect.y,
					finalRect.width,
					finalRect.height
				);

				if (currentEvent.type == EventType.MouseDown)
				{
					if (globalFinalRect.Contains(currentEvent.mousePosition))
					{
						if (currentEvent.button == 1)
						{
							var menu = new GenericMenu();

							menu.AddItem(new GUIContent("Delete"), false, () => { graphEditorNodes.Remove(nodeField.Key); });

							menu.ShowAsContext();

							currentEvent.Use();
						}
						else
						{

							View.Selection.Clear();
							View.Selection.Add(nodeField.Key);

							View.CurrentMode = BehaviourEditorView.ControlMode.NodeDragging;
							GUI.UnfocusWindow();
							GUI.FocusControl("");

							currentEvent.Use();
						}
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

			if (currentEvent.type != EventType.Repaint
				&& currentEvent.type != EventType.MouseDown
				&& currentEvent.type != EventType.MouseUp)
			{
				return;
			}

			var graphEditorNodesField = View.EditorObject.Fields["Nodes"];
			var graphEditorNodes = graphEditorNodesField.Value as EditorDictionary;

			// Foreach output
			foreach (var nodeField in graphEditorNodes.KeyValuePairs)
			{
				var node = nodeField.Value.Value as EditorObject;

				var nodeEditor = node.Fields["Editor"].Value as EditorObject;
				var nodeType = node.Fields["Type"].Value as EditorValue;
				var nodeData = node.Fields["Data"].Value as EditorObject;
				var nodeEditorPosition = nodeEditor.Fields["Position"].Value as EditorObject;

				float nodePositionX = (nodeEditorPosition.Fields["x"].Value as EditorValue).GetValue<int>() + View.PanPosition.x;
				float nodePositionY = (nodeEditorPosition.Fields["y"].Value as EditorValue).GetValue<int>() + View.PanPosition.y;

				// Foreach Output
				var nodeInfo = (NodeInformation)nodeData.Type;
				if (nodeInfo?.Outputs != null)
				{
					var outputSocketRect = new Rect(nodePositionX + 220, nodePositionY + 6, 20, 20);
					foreach (var output in nodeInfo.Outputs)
					{
						if (currentEvent.type == EventType.Repaint)
						{
							if (!StyleLookup.TryGetValue(output.Value.Type, out var connectionStyle))
							{
								connectionStyle = StyleLookup["Error"];
								Debug.LogWarning($"Couldn't find a style for connections of type \"{output.Value.Type}\".");
							}

							var originalColor = GUI.color;
							GUI.color = connectionStyle.SocketColor;
							GUI.DrawTexture(outputSocketRect, BehaviourGraphResources.Instance.OutputSocket);
							GUI.color = originalColor;
						}
						else if (currentEvent.type == EventType.MouseDown && outputSocketRect.Contains(currentEvent.mousePosition))
						{
							var outputId = new LocalPropertyId(new LocalId(nodeField.Key), output.Key);
							View.BeginConnectionFromOutput(outputId);

							GUI.UnfocusWindow();
							GUI.FocusControl("");

							currentEvent.Use();
						}
						else if (currentEvent.type == EventType.MouseUp && outputSocketRect.Contains(currentEvent.mousePosition))
						{
							if (View.CurrentMode == BehaviourEditorView.ControlMode.CreatingConnection)
							{
								if (!View.IsOutputSocket)
								{
									var thisOutputSocket = new LocalPropertyId(new LocalId(nodeField.Key), output.Key);

									(View.ConnectionInput.Value as EditorValue).SetValue(thisOutputSocket);
									(View.ConnectionInput.Value as EditorValue).ApplyModifiedProperties();
									View.CurrentMode = BehaviourEditorView.ControlMode.None;

									GUI.UnfocusWindow();
									GUI.FocusControl("");

									currentEvent.Use();
								}
							}
						}

						outputSocketRect.y += outputSocketRect.height + 4;
					}
				}

				IEnumerable<EditorField> InputSocketFields(EditorObject nodeDataContainer)
				{
					foreach (var childField in nodeDataContainer.Fields.Values)
					{
						if (childField.Field.Type != "InputSocket")
						{
							continue;
						}
						/*
						if (childField.Field.Format == FieldFormat.List)
						{
							foreach (var listElement in childField)
							{
								yield return listElement;
							}
						}
						*/
						else if (childField.Field.Wrapper == null)
						{
							yield return childField;
						}
						else
						{
							Debug.LogError($"Unknown InputSocket format. InputSockets cannot be wrapped.");
						}
					}
				}

				// Foreach Input
				foreach (var childField in InputSocketFields(nodeData))
				{
					var childFieldValue = childField.Value as EditorValue;
					var inputFieldFeature = childField.GetOrCreateFeature<FieldFeature>();

					inputFieldFeature.GlobalRenderedPosition = new Rect(
						inputFieldFeature.LocalRenderedPosition.x + nodePositionX - 4,
						inputFieldFeature.LocalRenderedPosition.y + nodePositionY,
						inputFieldFeature.LocalRenderedPosition.width,
						inputFieldFeature.LocalRenderedPosition.height);

					var inputSocketRect = inputFieldFeature.InputSocketPosition;

					if (currentEvent.type == EventType.MouseDown && inputSocketRect.Contains(currentEvent.mousePosition))
					{
						View.BeginConnectionFromInput(childField, nodeField.Key);

						GUI.UnfocusWindow();
						GUI.FocusControl("");

						currentEvent.Use();
					}
					else if (currentEvent.type == EventType.MouseUp && inputSocketRect.Contains(currentEvent.mousePosition))
					{
						if (View.CurrentMode == BehaviourEditorView.ControlMode.CreatingConnection)
						{
							if (View.IsOutputSocket)
							{
								childFieldValue.SetValue(View.ConnectionOutput);
								childFieldValue.ApplyModifiedProperties();
								View.CurrentMode = BehaviourEditorView.ControlMode.None;

								GUI.UnfocusWindow();
								GUI.FocusControl("");

								currentEvent.Use();
							}
						}
					}
					else if (currentEvent.type == EventType.Repaint)
					{
						if (!StyleLookup.TryGetValue("Error", out var inputStyle))
						{
							inputStyle = StyleLookup["Error"];
							Debug.LogWarning($"Couldn't find a style for connections of type \"Error\".");
						}

						var originalColor = GUI.color;
						GUI.color = inputStyle.SocketColor;
						GUI.DrawTexture(inputSocketRect, BehaviourGraphResources.Instance.InputSocket);
						GUI.color = originalColor;

						var thisInputConnectedTo = childFieldValue.GetValue<LocalPropertyId>();
						if (thisInputConnectedTo != LocalPropertyId.None)
						{
							bool isFound = false;
							Rect otherOutputSocketRect = default;
							SocketInformation outputSocketInformation = default;

							foreach (var otherNodeField in graphEditorNodes.KeyValuePairs)
							{
								var otherNode = otherNodeField.Value.Value as EditorObject;

								var otherNodeEditor = otherNode.Fields["Editor"];
								var otherNodeEditorPositionField = (otherNodeEditor.Value as EditorObject).Fields["Position"];
								var otherNodeEditorPosition = otherNodeEditorPositionField.Value as EditorObject;

								float otherNodePositionX = (otherNodeEditorPosition.Fields["x"].Value as EditorValue).GetValue<int>() + View.PanPosition.x;
								float otherNodePositionY = (otherNodeEditorPosition.Fields["y"].Value as EditorValue).GetValue<int>() + View.PanPosition.y;

								var otherNodeKvp = node.Fields["Data"];
								var otherNodeData = otherNodeKvp.Value as EditorObject;

								// Foreach Output
								otherOutputSocketRect = new Rect(otherNodePositionX + 220, otherNodePositionY + 6, 20, 20);
								var otherNodeInfo = (NodeInformation)otherNodeData.Type;
								if (otherNodeInfo.Outputs != null)
								{
									foreach (var output in otherNodeInfo.Outputs)
									{
										var otherOutputId = new LocalPropertyId(new LocalId(otherNodeField.Key), output.Key);

										if (otherOutputId == thisInputConnectedTo)
										{
											isFound = true;
											outputSocketInformation = output.Value;
											break;
										}

										otherOutputSocketRect.y += otherOutputSocketRect.height + 4;
									}
								}
								else
								{
									Debug.Log("There are no outputs on this node");
								}
								if (isFound)
								{
									break;
								}
							}
							if (isFound)
							{
								var start = new Vector3(otherOutputSocketRect.x, otherOutputSocketRect.center.y);
								var end = new Vector3(inputFieldFeature.GlobalRenderedPosition.x, inputFieldFeature.GlobalRenderedPosition.center.y);
								var startDir = new Vector3(1, 0);
								var endDir = new Vector3(-1, 0);

								if (!StyleLookup.TryGetValue(outputSocketInformation.Type, out var connectionStyle))
								{
									connectionStyle = StyleLookup["Error"];
									Debug.LogWarning($"Couldn't find a style for connections of type \"{outputSocketInformation.Type}\".");
								}

								DrawConnection(start, end, startDir, endDir, connectionStyle.ConnectionColor);
							}
						}
					}
				}
			}

			// Draw active connection
			if (View.CurrentMode == BehaviourEditorView.ControlMode.CreatingConnection)
			{
				if (View.IsOutputSocket)
				{
					// Draw Nodes
					bool isFound = false;
					Rect outputRect = default;
					SocketInformation outputSocketInformation = default;

					foreach (var nodeField in graphEditorNodes.KeyValuePairs)
					{
						var node = nodeField.Value.Value as EditorObject;

						var nodeEditor = node.Fields["Editor"].Value as EditorObject;
						var nodeType = node.Fields["Type"].Value as EditorValue;
						var nodeData = node.Fields["Data"].Value as EditorObject;
						var nodeEditorPosition = nodeEditor.Fields["Position"].Value as EditorObject;

						float nodePositionX = (nodeEditorPosition.Fields["x"].Value as EditorValue).GetValue<int>() + View.PanPosition.x;
						float nodePositionY = (nodeEditorPosition.Fields["y"].Value as EditorValue).GetValue<int>() + View.PanPosition.y;

						// Foreach Output
						var nodeInfo = (NodeInformation)nodeData.Type;
						outputRect = new Rect(nodePositionX + 220, nodePositionY + 6, 20, 20);
						foreach (var output in nodeInfo.Outputs)
						{
							var otherOutputId = new LocalPropertyId(new LocalId(nodeField.Key), output.Key);

							if (otherOutputId == View.ConnectionOutput)
							{
								isFound = true;
								outputSocketInformation = output.Value;
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
						var start = new Vector3(outputRect.x, outputRect.center.y);
						var end = new Vector3(currentEvent.mousePosition.x, currentEvent.mousePosition.y);
						var startDir = new Vector3(1, 0);
						var endDir = new Vector3(-1, 0);

						if (!StyleLookup.TryGetValue(outputSocketInformation.Type, out var connectionStyle))
						{
							connectionStyle = StyleLookup["Error"];
							Debug.LogWarning($"Couldn't find a style for connections of type \"{outputSocketInformation.Type}\".");
						}

						DrawConnection(start, end, startDir, endDir, connectionStyle.ConnectionColor);
					}
				}
				else
				{
					if (!StyleLookup.TryGetValue("Error", out var inputStyle))
					{
						inputStyle = StyleLookup["Error"];
						Debug.LogWarning($"Couldn't find a style for connections of type \"Error\".");
					}

					var startFieldFeature = View.ConnectionInput.GetOrCreateFeature<FieldFeature>();

					var inputSocketRect = startFieldFeature.InputSocketPosition;

					var start = new Vector3(currentEvent.mousePosition.x, currentEvent.mousePosition.y);
					var end = new Vector3(inputSocketRect.xMax, inputSocketRect.center.y);
					var startDir = new Vector3(1, 0);
					var endDir = new Vector3(-1, 0);

					DrawConnection(start, end, startDir, endDir, inputStyle.ConnectionColor);
				}
			}
		}

		private static void DrawConnection(Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir, Color connectionColour)
		{
			float distance = Vector3.Distance(start, end);
			var startTan = start + (startDir * distance * 0.5f);
			var endTan = end + (endDir * distance * 0.5f);

			Handles.DrawBezier(start, end, startTan, endTan, connectionColour,
				BehaviourGraphResources.Instance.SmallConnection, 10);
		}

		private void HandleInput()
		{
			if (currentEvent.type == EventType.MouseUp)
			{
				switch (View.CurrentMode)
				{
					case BehaviourEditorView.ControlMode.NodeDragging:
						currentEvent.Use();

						foreach (string selectedNode in View.Selection)
						{
							var nodesField = View.EditorObject.Fields["Nodes"];
							var selectedNodeData = (nodesField.Value as EditorDictionary).KeyValuePairs[selectedNode].Value as EditorObject;
							var editor = selectedNodeData.Fields["Editor"].Value as EditorObject;
							var editorPosition = editor.Fields["Position"].Value as EditorObject;

							var posX = editorPosition.Fields["x"].Value as EditorValue;
							posX.ApplyModifiedProperties();

							var posY = editorPosition.Fields["y"].Value as EditorValue;
							posY.ApplyModifiedProperties();
						}
						View.CurrentMode = BehaviourEditorView.ControlMode.None;
						break;

					case BehaviourEditorView.ControlMode.ViewDragging:
						View.CurrentMode = BehaviourEditorView.ControlMode.None;
						break;

					case BehaviourEditorView.ControlMode.CreatingConnection:
						View.CurrentMode = BehaviourEditorView.ControlMode.None;
						break;
				}
				Window.Repaint();
			}
			else if (currentEvent.type == EventType.KeyDown)
			{
				if (currentEvent.keyCode == KeyCode.Space)
				{
					var position = new PackageNodePosition()
					{
						x = (int)currentEvent.mousePosition.x,
						y = (int)currentEvent.mousePosition.y
					};
					var window = new BehaviourGraphAddNodeDropdown(BehaviourManifest.CreateFromAppDomain(AppDomain.CurrentDomain), (newNodeObject) =>
					{
						string newNodeType = (string)newNodeObject;

						var graphEditorNodesField = View.EditorObject.Fields["Nodes"];
						var graphEditorNodes = graphEditorNodesField.Value as EditorDictionary;

						string newId = LocalId.NewId().ToString();
						graphEditorNodes.Add(newId);

						var newNode = graphEditorNodes.KeyValuePairs[newId];
						var nodeData = new JObject();
						/*newNode.SetValue(new SerializedNode()
						{
							Type = newNodeType,
							Data = nodeData,
							Editor = new PackageNodeEditor()
							{
								Position = position
							}
						});
						newNode.ApplyModifiedProperties();
						*/
						Window.Repaint();
					});

					window.Show(new Rect(currentEvent.mousePosition.x, currentEvent.mousePosition.y, 0, 0));
				}
				else if (currentEvent.keyCode == KeyCode.Delete)
				{
					var graphEditorNodesField = View.EditorObject.Fields["Nodes"];
					var graphEditorNodes = graphEditorNodesField.Value as EditorDictionary;

					foreach (string node in View.Selection)
					{
						graphEditorNodes.Remove(node);
					}
				}
			}
			else if (currentEvent.type == EventType.MouseDrag)
			{
				switch (View.CurrentMode)
				{
					case BehaviourEditorView.ControlMode.NodeDragging:
						foreach (string selectedNode in View.Selection)
						{

							var nodesField = View.EditorObject.Fields["Nodes"];
							var selectedNodeData = (nodesField.Value as EditorDictionary).KeyValuePairs[selectedNode].Value as EditorObject;
							var editor = selectedNodeData.Fields["Editor"].Value as EditorObject;
							var editorPosition = editor.Fields["Position"].Value as EditorObject;

							var posX = editorPosition.Fields["x"].Value as EditorValue;
							posX.SetValue(posX.GetValue<int>() + ((int)currentEvent.delta.x));

							var posY = editorPosition.Fields["y"].Value as EditorValue;
							posY.SetValue(posY.GetValue<int>() + ((int)currentEvent.delta.y));
						}
						break;

					case BehaviourEditorView.ControlMode.ViewDragging:
						View.PanPosition += currentEvent.delta;
						break;
				}
				Window.Repaint();
			}
			else if (currentEvent.type == EventType.MouseDown)
			{
				if (Position.Contains(currentEvent.mousePosition))
				{
					GUI.UnfocusWindow();
					GUI.FocusControl("");

					View.CurrentMode = BehaviourEditorView.ControlMode.ViewDragging;

					currentEvent.Use();
					Window.Repaint();
				}
			}
		}

		private void DrawBackground(Rect backgroundRect, Vector2 viewPosition)
		{
			if (currentEvent.type != EventType.Repaint)
			{
				return;
			}

			if (View.Session == null)
			{
				EditorGUI.LabelField(backgroundRect, "No Graph Selected", BehaviourGUIStyles.Instance.informationTextStyle);
				return;
			}

#if HOVER_EFFECTS
			if (dragging_IsDragging)
			{
				EditorGUIUtility.AddCursorRect (backgroundRect, MouseCursor.Pan);
			}
#endif
			float gridScale = 1.5f;

			/*if (Application.isPlaying)
				EditorGUI.DrawRect (screenRect, new Color (0.7f, 0.7f, 0.7f));*/

			var originalTintColour = GUI.color;
			GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);

			DrawImageTiled(backgroundRect, BehaviourGraphResources.Instance.WindowBackground, viewPosition, gridScale);

			GUI.color = originalTintColour;

			if (Application.isPlaying)
			{
				var runtimeInfo = new Rect(backgroundRect);
				runtimeInfo.yMin = runtimeInfo.yMax - 48;
				EditorGUI.LabelField(runtimeInfo, "Playmode Enabled: You may change values but you can't edit connections",
					BehaviourGUIStyles.Instance.informationTextStyle);
			}
		}

		private void DrawImageTiled(Rect rect, Texture2D texture, Vector2 positon, float zoom = 0.8f)
		{
			if (texture == null)
			{
				return;
			}

			if (currentEvent.type != EventType.Repaint)
			{
				return;
			}

			var tileOffset = new Vector2((-positon.x / texture.width) * zoom, (positon.y / texture.height) * zoom);

			var tileAmount = new Vector2(Mathf.Round(rect.width * zoom) / texture.width,
				Mathf.Round(rect.height * zoom) / texture.height);

			tileOffset.y -= tileAmount.y;
			GUI.DrawTextureWithTexCoords(rect, texture, new Rect(tileOffset, tileAmount), true);
		}
	}
}
