using System;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Tooltips.Editors
{
	[CustomEditor (typeof (TooltipElement), true)]
	public class TooltipElementEditor : Editor
	{
		private static GUIStyle textStyle = null;

		public override void OnInspectorGUI ()
		{
			DrawTooltipTargets (target.GetType ());

			SerializedProperty iterator = serializedObject.GetIterator ();
			iterator.NextVisible (true);
			EditorGUILayout.PropertyField (iterator);
			while (iterator.NextVisible (false))
			{
				EditorGUILayout.PropertyField (iterator);
			}
		}

		private static void DrawTooltipTargets (Type elementType)
		{
			if (textStyle == null)
			{
				textStyle = new GUIStyle (EditorStyles.label);

				textStyle.fontSize = 9;
				textStyle.alignment = TextAnchor.MiddleLeft;
				textStyle.richText = true;
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