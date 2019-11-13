﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RPGCore.Utility.Editors
{
	[CustomPropertyDrawer(typeof(UneditableAttribute))]
	public class UneditableAttributeDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUI.PropertyField(position, property, label, true);
			EditorGUI.EndDisabledGroup();
		}
	}
}
#endif
