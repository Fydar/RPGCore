using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public static class RPGCoreEditor
	{
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

				EditorGUI.indentLevel--;
				fieldFeature.Expanded = EditorGUILayout.Foldout(fieldFeature.Expanded, field.Name, true);
				EditorGUI.indentLevel++;

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
					PrefixLable(fieldRect, out var lable, out var content);
					content.width = 140;

					EditorGUI.LabelField(lable, field.Name);

					if (GUI.Button(content, $"Edit Graph"))
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

					EditorGUI.indentLevel--;
					fieldFeature.Expanded = EditorGUILayout.Foldout(fieldFeature.Expanded, field.Name, true);
					EditorGUI.indentLevel++;

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

		public static void PrefixLable (Rect position, out Rect lable, out Rect content)
		{
			content = EditorGUI.PrefixLabel (position, new GUIContent (" "));

			float indent = EditorGUI.indentLevel * 15.0f;
			position.xMin += indent;

			lable = new Rect (position)
			{
				xMax = content.xMin
			};
		}
	}
}
