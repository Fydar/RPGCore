using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.DataEditor;
using RPGCore.DataEditor.Manifest;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public static class RPGCoreEditor
	{
		private const int foldoutIndent = -10;

		public static void DrawEditor(EditorSession editor)
		{
			if (editor == null)
			{
				return;
			}
			EditorGUI.indentLevel++;
			foreach (var field in editor.Root.Fields)
			{
				DrawField(field.Value);
			}
			EditorGUI.indentLevel--;
		}

		public static void DrawField(EditorField editorField)
		{
			var fieldFeature = editorField.GetOrCreateFeature<FieldFeature>();

			switch (editorField.Value)
			{
				case EditorValue editorValue:
				{
					switch (editorField.Field.Type)
					{
						case "Int16":
						{
							EditorGUI.BeginChangeCheck();
							int newValue = EditorGUILayout.IntField(editorField.Field.Name, editorValue.GetValue<short>());
							if (EditorGUI.EndChangeCheck())
							{
								editorValue.SetValue((short)newValue);
								editorValue.ApplyModifiedProperties();
							}
							break;
						}
						case "Int32":
						{
							EditorGUI.BeginChangeCheck();
							int newValue = EditorGUILayout.IntField(editorField.Field.Name, editorValue.GetValue<int>());
							if (EditorGUI.EndChangeCheck())
							{
								editorValue.SetValue(newValue);
								editorValue.ApplyModifiedProperties();
							}
							break;
						}
						case "Int64":
						{
							EditorGUI.BeginChangeCheck();
							long newValue = EditorGUILayout.LongField(editorField.Field.Name, editorValue.GetValue<long>());
							if (EditorGUI.EndChangeCheck())
							{
								editorValue.SetValue(newValue);
								editorValue.ApplyModifiedProperties();
							}
							break;
						}
						case "Single":
						{
							EditorGUI.BeginChangeCheck();
							float newValue = EditorGUILayout.FloatField(editorField.Field.Name, editorValue.GetValue<float>());
							if (EditorGUI.EndChangeCheck())
							{
								editorValue.SetValue(newValue);
								editorValue.ApplyModifiedProperties();
							}
							break;
						}
						case "Double":
						{
							EditorGUI.BeginChangeCheck();
							double newValue = EditorGUILayout.DoubleField(editorField.Field.Name, editorValue.GetValue<double>());
							if (EditorGUI.EndChangeCheck())
							{
								editorValue.SetValue(newValue);
								editorValue.ApplyModifiedProperties();
							}
							break;
						}
						case "String":
						{
							EditorGUI.BeginChangeCheck();
							string newValue = EditorGUILayout.TextField(editorField.Field.Name, editorValue.GetValue<string>());
							if (EditorGUI.EndChangeCheck())
							{
								editorValue.SetValue(newValue);
								editorValue.ApplyModifiedProperties();
							}
							break;
						}
						case "Boolean":
						{
							EditorGUI.BeginChangeCheck();
							bool newValue = EditorGUILayout.Toggle(editorField.Field.Name, editorValue.GetValue<bool>());
							if (EditorGUI.EndChangeCheck())
							{
								editorValue.SetValue(newValue);
								editorValue.ApplyModifiedProperties();
							}
							break;
						}
						case "InputSocket":
						{
							EditorGUI.BeginChangeCheck();
							EditorGUILayout.LabelField(editorField.Field.Name, editorValue.GetValue<LocalPropertyId>().TargetIdentifier.ToString());
							var renderPos = GUILayoutUtility.GetLastRect();
							fieldFeature.LocalRenderedPosition = renderPos;
							if (EditorGUI.EndChangeCheck())
							{
								//field.Json.Value = newValue;
							}
							break;
						}
						default:
						{
							EditorGUILayout.LabelField($"Unsupported value type {editorField.Field.Type}");
							break;
						}
					}
					break;
				}
				case EditorObject editorObject:
				{
					switch (editorField.Field.Type)
					{
						case "SerializedGraph":
						{
							/*
							if (field.GetValue<SerializedGraph>() == null)
							{
								field.SetValue(new SerializedGraph());
								field.ApplyModifiedProperties();
							}
							*/

							var fieldRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
							GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
							fieldRect.xMin += 4;
							var buttonRect = EditorGUI.PrefixLabel(fieldRect, new GUIContent(editorField.Field.Name));

							buttonRect.width = 160;

							if (GUI.Button(buttonRect, $"Edit Graph"))
							{
								var framedFeature = editorField.Session.GetFeature<FramedEditorSessionFeature>();
								if (framedFeature == null)
								{
									BehaviourEditor.Open(editorField.Session, editorObject);
								}
								else
								{
									foreach (var virtualTab in framedFeature.Frame.EditorContext.VirtualChildren)
									{
										if (virtualTab.Frame is BehaviourGraphFrame graphFrame)
										{
											if (graphFrame.View.EditorObject == editorObject)
											{
												framedFeature.Frame.EditorContext.CurrentTab = virtualTab;
											}
										}
									}
								}
							}

							var renderPos = GUILayoutUtility.GetLastRect();
							fieldFeature.LocalRenderedPosition = renderPos;
							break;
						}
						default:
						{
							var fieldRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
							fieldRect.xMin += foldoutIndent;
							GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

							fieldFeature.Expanded = EditorGUI.Foldout(fieldRect, fieldFeature.Expanded, editorField.Field.Name, true);

							if (fieldFeature.Expanded)
							{
								EditorGUI.indentLevel++;
								foreach (var childField in editorObject.Fields)
								{
									DrawField(childField.Value);
								}
								EditorGUI.indentLevel--;
							}
							break;
						}
					}
					break;
				}
				case EditorDictionary editorDictionary:
				{
					var fieldRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
					GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
					fieldRect.xMin += foldoutIndent;

					fieldFeature.Expanded = EditorGUI.Foldout(fieldRect, fieldFeature.Expanded, editorField.Field.Name, true);

					if (fieldFeature.Expanded)
					{
						EditorGUI.indentLevel++;

						foreach (var childField in editorDictionary.KeyValuePairs)
						{
							EditorGUILayout.LabelField($"{childField.Key}: {childField.Value}");
							// DrawField(childField);
						}

						var buttonRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
						GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
						buttonRect.xMin += foldoutIndent;
						buttonRect.width = 20;
						if (GUI.Button(buttonRect, "+"))
						{
							editorDictionary.Add("New Element");
						}
						EditorGUI.indentLevel--;
					}
					break;
				}
				case EditorList editorList:
				{
					var fieldRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
					GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
					fieldRect.xMin += foldoutIndent;

					fieldFeature.Expanded = EditorGUI.Foldout(fieldRect, fieldFeature.Expanded, editorField.Field.Name, true);

					if (fieldFeature.Expanded)
					{
						EditorGUI.indentLevel++;

						EditorGUI.BeginChangeCheck();
						int newSize = EditorGUILayout.DelayedIntField("Size", editorList.Elements.Count);
						if (EditorGUI.EndChangeCheck())
						{
							editorList.SetArraySize(newSize);
						}

						for (int i = 0; i < editorList.Elements.Count; i++)
						{
							var element = editorList.Elements[i];
							// DrawField(element);

							EditorGUILayout.LabelField($"[{i}]", element?.ToString());
						}
						EditorGUI.indentLevel--;
					}
					break;
				}
				default:
				{
					EditorGUILayout.LabelField($"Unsupported type {editorField?.Value?.GetType()}");
					break;
				}
			}
		}
	}
}
