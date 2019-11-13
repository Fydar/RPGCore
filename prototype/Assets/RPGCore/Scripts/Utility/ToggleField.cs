using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace RPGCore.Utility
{
	public class TogglableField
	{
		public bool Enabled;

#if UNITY_EDITOR
		[CustomPropertyDrawer(typeof(TogglableField), true)]
		private class TogglableFieldDrawer : PropertyDrawer
		{
			public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
			{
				var valueProperty = property.FindPropertyRelative("Value");

				return EditorGUI.GetPropertyHeight(valueProperty);
			}

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				var enabledProperty = property.FindPropertyRelative("Enabled");
				var valueProperty = property.FindPropertyRelative("Value");

				var toggleRect = new Rect(position);
				toggleRect.xMax = toggleRect.xMin + 16;

				var fieldRect = new Rect(position);
				fieldRect.xMin += 16;

				enabledProperty.boolValue = EditorGUI.ToggleLeft(toggleRect, GUIContent.none, enabledProperty.boolValue);

				EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);

				EditorGUI.PropertyField(fieldRect, valueProperty, label, true);

				EditorGUI.EndDisabledGroup();
			}
		}
#endif
	}

	[Serializable]
	public class TogglableField<T> : TogglableField
	{
		public T Value;

		public TogglableField(bool enabled, T value)
		{
			Enabled = enabled;
			Value = value;
		}
	}
}

