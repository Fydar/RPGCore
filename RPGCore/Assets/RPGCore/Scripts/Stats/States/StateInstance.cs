using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore.Stats
{
	[System.Serializable]
	public class StateInstance : AttributeInstance
	{
		[SerializeField]
		private float _value = 0.0f;
		private float delta = 0.0f;

		public StateInformation Info;

		public float Delta
		{
			get
			{
				return delta;
			}
		}

		public override float Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (_value == value)
					return;

				delta = _value - value;

				_value = value;

				if (OnValueChanged != null)
					OnValueChanged ();
			}
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer (typeof (StateInstance))]
	public class StateInstanceDrawer : PropertyDrawer
	{

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight (property, label);
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty infoProperty = property.FindPropertyRelative ("Info");
			SerializedProperty valueProperty = property.FindPropertyRelative ("_value");

			StateInformation stateInfo = (StateInformation)infoProperty.objectReferenceValue;

			if (stateInfo != null)
			{
				label.tooltip = stateInfo.Description;

				if (stateInfo.accuracy != AttributeInformation.Accuracy.Float)
				{
					EditorGUI.BeginChangeCheck ();
					float newValue = EditorGUI.IntField (position, label, Mathf.RoundToInt (valueProperty.floatValue));
					if (EditorGUI.EndChangeCheck ())
					{
						valueProperty.floatValue = newValue;
					}
				}
				else
				{
					valueProperty.floatValue = EditorGUI.FloatField (position, label, valueProperty.floatValue);
				}

				GUI.tooltip = null;
			}
			else
			{
				EditorGUI.LabelField (position, label);
			}

			property.serializedObject.ApplyModifiedProperties ();
		}
	}
#endif
}