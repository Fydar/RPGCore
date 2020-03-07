using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public static class RPGCoreEditor
	{
		const int FoldoutIndent = -10;

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
			// EditorGUILayout.LabelField(field.Json.Path);

			if (field.Field.Format == FieldFormat.List)
			{
				var fieldFeature = field.GetOrCreateFeature<FieldFeature>();

				var fieldRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
				GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
				fieldRect.xMin += FoldoutIndent;

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
			}
			else if (field.Field.Format == FieldFormat.Object)
			{
				if (field.Field.Type == "Int32")
				{
					EditorGUI.BeginChangeCheck();
					int newValue = EditorGUILayout.IntField(field.Name, field.GetValue<int>());
					if (EditorGUI.EndChangeCheck())
					{
						field.SetValue(newValue);
						field.ApplyModifiedProperties();
					}
				}
				else if (field.Field.Type == "Single")
				{
					EditorGUI.BeginChangeCheck();
					float newValue = EditorGUILayout.FloatField(field.Name, field.GetValue<float>());
					if (EditorGUI.EndChangeCheck())
					{
						field.SetValue(newValue);
						field.ApplyModifiedProperties();
					}
				}
				else if(field.Field.Type == "Double")
				{
					EditorGUI.BeginChangeCheck();
					double newValue = EditorGUILayout.DoubleField(field.Name, field.GetValue<double>());
					if (EditorGUI.EndChangeCheck())
					{
						field.SetValue(newValue);
						field.ApplyModifiedProperties();
					}
				}
				else if (field.Field.Type == "String")
				{
					EditorGUI.BeginChangeCheck();
					string newValue = EditorGUILayout.TextField(field.Name, field.GetValue<string>());
					if (EditorGUI.EndChangeCheck())
					{
						field.SetValue(newValue);
						field.ApplyModifiedProperties();
					}
				}
				else if (field.Field.Type == "Boolean")
				{
					EditorGUI.BeginChangeCheck();
					bool newValue = EditorGUILayout.Toggle(field.Name, field.GetValue<bool>());
					if (EditorGUI.EndChangeCheck())
					{
						field.SetValue(newValue);
						field.ApplyModifiedProperties();
					}
				}
				else if (field.Field.Type == "InputSocket")
				{
					var fieldFeature = field.GetOrCreateFeature<FieldFeature>();

					EditorGUI.BeginChangeCheck();
					EditorGUILayout.LabelField(field.Name, field.GetValue<LocalPropertyId>().TargetIdentifier.ToString());
					var renderPos = GUILayoutUtility.GetLastRect();
					fieldFeature.LocalRenderedPosition = renderPos;
					if (EditorGUI.EndChangeCheck())
					{
						//field.Json.Value = newValue;
					}

					// EditorGUI.DrawRect(renderPos, Color.red);
				}
				else if (field.Field.Type == "SerializedGraph")
				{
					var fieldFeature = field.GetOrCreateFeature<FieldFeature>();

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
						BehaviourEditor.Open(field.Session, field);
					}

					var renderPos = GUILayoutUtility.GetLastRect();
					fieldFeature.LocalRenderedPosition = renderPos;
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
				else
				{
					var fieldFeature = field.GetOrCreateFeature<FieldFeature>();

					var fieldRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
					fieldRect.xMin += FoldoutIndent;
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
				}
			}
			else
			{
				EditorGUILayout.LabelField(field.Name, "Unknown Type");
			}
		}
	}
}
