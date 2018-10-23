using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace RPGCore.Utility
{
	[Serializable]
	public class TogglableField<T>
	{
		public bool Enabled = false;
		public T Value;

		public TogglableField (bool enabled, T value)
		{
			Enabled = enabled;
			Value = value;
		}

#if UNITY_EDITOR
		[CustomPropertyDrawer (typeof (TogglableField<>), true)]
		class TogglableFieldDrawer : PropertyDrawer
		{
			public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
			{
				SerializedProperty valueProperty = property.FindPropertyRelative ("Value");

				return EditorGUI.GetPropertyHeight (valueProperty);
			}

			public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
			{
				SerializedProperty enabledProperty = property.FindPropertyRelative ("Enabled");
				SerializedProperty valueProperty = property.FindPropertyRelative ("Value");

				Rect toggleRect = new Rect (position);
				toggleRect.xMax = toggleRect.xMin + 16;

				Rect fieldRect = new Rect (position);
				fieldRect.xMin += 16;

				enabledProperty.boolValue = EditorGUI.ToggleLeft (toggleRect, GUIContent.none, enabledProperty.boolValue);

				EditorGUI.BeginDisabledGroup (!enabledProperty.boolValue);

				EditorGUI.PropertyField (fieldRect, valueProperty, label, true);

				EditorGUI.EndDisabledGroup ();
			}
		}
#endif
	}
}