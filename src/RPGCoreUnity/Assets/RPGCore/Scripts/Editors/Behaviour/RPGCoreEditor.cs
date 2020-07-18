using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
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
			foreach (var field in editor.Root)
			{
				DrawField(field);
			}
			EditorGUI.indentLevel--;
		}

		public static void DrawField(EditorField field)
		{
			var fieldFeature = field.GetOrCreateFeature<FieldFeature>();

			switch (field.Field.Format)
			{
				case FieldFormat.List:
				{
					var fieldRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
					GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
					fieldRect.xMin += foldoutIndent;

					fieldFeature.Expanded = EditorGUI.Foldout(fieldRect, fieldFeature.Expanded, field.Name, true);

					if (fieldFeature.Expanded)
					{
						EditorGUI.indentLevel++;

						EditorGUI.BeginChangeCheck();
						int newSize = EditorGUILayout.DelayedIntField("Size", field.Count);
						if (EditorGUI.EndChangeCheck())
						{
							field.SetArraySize(newSize);
						}

						foreach (var childField in field)
						{
							DrawField(childField);
						}
						EditorGUI.indentLevel--;
					}
					break;
				}
				case FieldFormat.Dictionary:
				{
					var fieldRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
					GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
					fieldRect.xMin += foldoutIndent;

					fieldFeature.Expanded = EditorGUI.Foldout(fieldRect, fieldFeature.Expanded, field.Name, true);

					if (fieldFeature.Expanded)
					{
						EditorGUI.indentLevel++;

						foreach (var childField in field)
						{
							DrawField(childField);
						}

						var buttonRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
						GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
						buttonRect.xMin += foldoutIndent;
						buttonRect.width = 20;
						if (GUI.Button(buttonRect, "+"))
						{
							field.Add("New Element");
						}
						EditorGUI.indentLevel--;
					}
					break;
				}
				case FieldFormat.Object:
				{
					switch (field.Field.Type)
					{
						case "Int16":
						{
							EditorGUI.BeginChangeCheck();
							int newValue = EditorGUILayout.IntField(field.Name, field.GetValue<short>());
							if (EditorGUI.EndChangeCheck())
							{
								field.SetValue((short)newValue);
								field.ApplyModifiedProperties();
							}
							break;
						}
						case "Int32":
						{
							EditorGUI.BeginChangeCheck();
							int newValue = EditorGUILayout.IntField(field.Name, field.GetValue<int>());
							if (EditorGUI.EndChangeCheck())
							{
								field.SetValue(newValue);
								field.ApplyModifiedProperties();
							}
							break;
						}
						case "Int64":
						{
							EditorGUI.BeginChangeCheck();
							long newValue = EditorGUILayout.LongField(field.Name, field.GetValue<long>());
							if (EditorGUI.EndChangeCheck())
							{
								field.SetValue(newValue);
								field.ApplyModifiedProperties();
							}
							break;
						}
						case "Single":
						{
							EditorGUI.BeginChangeCheck();
							float newValue = EditorGUILayout.FloatField(field.Name, field.GetValue<float>());
							if (EditorGUI.EndChangeCheck())
							{
								field.SetValue(newValue);
								field.ApplyModifiedProperties();
							}
							break;
						}
						case "Double":
						{
							EditorGUI.BeginChangeCheck();
							double newValue = EditorGUILayout.DoubleField(field.Name, field.GetValue<double>());
							if (EditorGUI.EndChangeCheck())
							{
								field.SetValue(newValue);
								field.ApplyModifiedProperties();
							}
							break;
						}
						case "String":
						{
							EditorGUI.BeginChangeCheck();
							string newValue = EditorGUILayout.TextField(field.Name, field.GetValue<string>());
							if (EditorGUI.EndChangeCheck())
							{
								field.SetValue(newValue);
								field.ApplyModifiedProperties();
							}
							break;
						}
						case "Boolean":
						{
							EditorGUI.BeginChangeCheck();
							bool newValue = EditorGUILayout.Toggle(field.Name, field.GetValue<bool>());
							if (EditorGUI.EndChangeCheck())
							{
								field.SetValue(newValue);
								field.ApplyModifiedProperties();
							}
							break;
						}
						case "InputSocket":
						{
							EditorGUI.BeginChangeCheck();
							EditorGUILayout.LabelField(field.Name, field.GetValue<LocalPropertyId>().TargetIdentifier.ToString());
							var renderPos = GUILayoutUtility.GetLastRect();
							fieldFeature.LocalRenderedPosition = renderPos;
							if (EditorGUI.EndChangeCheck())
							{
								//field.Json.Value = newValue;
							}
							break;
						}
						case "SerializedGraph":
						{
							if (field.GetValue<SerializedGraph>() == null)
							{
								field.SetValue(new SerializedGraph());
								field.ApplyModifiedProperties();
							}

							var fieldRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
							GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
							fieldRect.xMin += 4;
							var buttonRect = EditorGUI.PrefixLabel(fieldRect, new GUIContent(field.Name));

							buttonRect.width = 160;

							if (GUI.Button(buttonRect, $"Edit Graph"))
							{
								var framedFeature = field.Session.Root.GetFeature<FramedEditorSessionFeature>();
								if (framedFeature == null)
								{
									BehaviourEditor.Open(field.Session, field);
								}
								else
								{
									foreach (var virtualTab in framedFeature.Frame.EditorContext.VirtualChildren)
									{
										if (virtualTab.Frame is BehaviourGraphFrame graphFrame)
										{
											if (graphFrame.View.GraphField == field)
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

							fieldFeature.Expanded = EditorGUI.Foldout(fieldRect, fieldFeature.Expanded, field.Name, true);

							if (fieldFeature.Expanded)
							{
								EditorGUI.indentLevel++;
								foreach (var childField in field)
								{
									DrawField(childField);
								}
								EditorGUI.indentLevel--;
							}
							break;
						}
					}
					break;
				}
				default:
				{
					EditorGUILayout.LabelField(field.Name, "Unknown Type");
					break;
				}
			}
		}
	}
}
