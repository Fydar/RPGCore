using System;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Tooltips.Editors
{
	[CustomEditor (typeof (TooltipElement), true)]
	public class TooltipElementEditor : Editor
	{
		private static GUIStyle textStyle;

		public override void OnInspectorGUI ()
		{
			DrawTooltipTargets (target.GetType ());

			SerializedProperty iterator = serializedObject.GetIterator ();
			iterator.NextVisible (true);

			EditorGUI.BeginDisabledGroup (true);
			EditorGUILayout.PropertyField (iterator);
			EditorGUI.EndDisabledGroup ();

			while (iterator.NextVisible (false))
			{
				EditorGUILayout.PropertyField (iterator, true);
			}

			serializedObject.ApplyModifiedProperties ();
		}

		private static void DrawTooltipTargets (Type elementType)
		{
			if (textStyle == null)
			{
				textStyle = new GUIStyle (EditorStyles.label)
				{
					fontSize = 9,
					alignment = TextAnchor.MiddleLeft,
					richText = true
				};
			}

			Type[] interfaces = elementType.GetInterfaces ();

			GUILayout.Space (EditorGUIUtility.standardVerticalSpacing * 2);

			EditorGUILayout.BeginVertical (EditorStyles.helpBox);

			foreach (Type inheriting in interfaces)
			{
				if (inheriting.GetGenericTypeDefinition () != typeof (ITooltipTarget<>))
					continue;

				string text = inheriting.GetGenericArguments ()[0].ToString ();
				int endIndex = text.LastIndexOf ('.');

				if (endIndex != -1)
				{
					text = "<color=#777>" + text.Insert (endIndex + 1, "</color>");
				}

				EditorGUILayout.LabelField (text, textStyle);

			}
			EditorGUILayout.EndVertical ();
		}
	}
}
