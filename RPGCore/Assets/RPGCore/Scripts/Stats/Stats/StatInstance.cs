using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace RPGCore.Stats
{
	[Serializable]
	public class StatInstance : AttributeInstance
	{
		public class Modifier
		{
			public StatInstance Parent;
			public float currentValue;

			public float Value
			{
				get
				{
					return currentValue;
				}
				set
				{
					currentValue = value;
					Parent.isDirty = true;
					Parent.InvokeChanged ();
				}
			}

			public Modifier (StatInstance parent)
			{
				Parent = parent;
				Value = 0.0f;
			}

			public Modifier (StatInstance parent, float value)
			{
				Parent = parent;
				Value = value;
			}
		}

		public StatInformation Info;

		[SerializeField] private float baseValue = 0.0f;

		private float lastValue = 0.0f;
		private bool isDirty = true;

		private List<Modifier> MultBaseModifiers = new List<Modifier> ();
		private List<Modifier> FlatModifiers = new List<Modifier> ();
		private List<Modifier> MultModifiers = new List<Modifier> ();

		public override float Value
		{
			get
			{
				if (isDirty)
				{
					float modifiedValue = baseValue;

					modifiedValue *= GetMultiplier (MultBaseModifiers);
					modifiedValue += Sum (FlatModifiers);
					modifiedValue *= GetMultiplier (MultModifiers);

					if (Info != null)
						modifiedValue = Info.Filter (modifiedValue);

					lastValue = modifiedValue;
				}

				return lastValue;
			}
			set
			{
				BaseValue = value;
			}
		}

		public float BaseValue
		{
			get
			{
				return baseValue;
			}
			set
			{
				baseValue = value;
				isDirty = true;
			}
		}

		public Modifier AddFlatModifier (float startingValue)
		{
			Modifier mod = new Modifier (this, startingValue);
			FlatModifiers.Add (mod);

			isDirty = true;
			InvokeChanged ();
			return mod;
		}

		public Modifier AddBaseMultiplierModifier (float value)
		{
			Modifier mod = new Modifier (this, value);
			MultBaseModifiers.Add (mod);

			isDirty = true;
			InvokeChanged ();
			return mod;
		}

		public Modifier AddMultiplierModifier (float value)
		{
			Modifier mod = new Modifier (this, value);
			MultModifiers.Add (mod);

			isDirty = true;
			InvokeChanged ();
			return mod;
		}

		public void RemoveFlatModifier (Modifier modifier)
		{
			FlatModifiers.Remove (modifier);

			isDirty = true;
			InvokeChanged ();
		}

		public void RemoveBaseMultiplierModifier (Modifier modifier)
		{
			MultBaseModifiers.Remove (modifier);

			isDirty = true;
			InvokeChanged ();
		}

		public void RemoveMultiplierModifier (Modifier modifier)
		{
			MultModifiers.Remove (modifier);

			isDirty = true;
			InvokeChanged ();
		}

		protected float Sum (List<Modifier> modifiers)
		{
			float total = 0.0f;

			for (int i = 0; i < modifiers.Count; i++)
			{
				total += modifiers[i].Value;
			}

			return total;
		}

		protected float GetMultiplier (List<Modifier> multiply)
		{
			if (multiply.Count == 0)
				return 1.0f;
			if (multiply.Count == 1)
				return 1.0f + multiply[0].Value;

			float total = 1.0f + multiply[0].Value;

			for (int i = 1; i < multiply.Count; i++)
			{
				total *= 1.0f + multiply[i].Value;
			}

			return total;
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer (typeof (StatInstance))]
	public class StatInstanceDrawer : PropertyDrawer
	{

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight (property, label);
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty infoProperty = property.FindPropertyRelative ("Info");
			SerializedProperty valueProperty = property.FindPropertyRelative ("baseValue");

			StatInformation statInfo = (StatInformation)infoProperty.objectReferenceValue;

			if (statInfo != null)
			{
				label.tooltip = statInfo.Description;

				if (statInfo.MaxValue.Enabled && statInfo.MinValue.Enabled)
				{
					if (statInfo.accuracy == AttributeInformation.Accuracy.Integer)
						valueProperty.floatValue = EditorGUI.IntSlider (position, label.text, Mathf.RoundToInt (valueProperty.floatValue),
							Mathf.RoundToInt (statInfo.MinValue.Value), Mathf.RoundToInt (statInfo.MaxValue.Value));
					else
						valueProperty.floatValue = EditorGUI.Slider (position, label, valueProperty.floatValue, statInfo.MinValue.Value, statInfo.MaxValue.Value);

				}
				else
				{
					if (statInfo.accuracy == AttributeInformation.Accuracy.Integer)
						valueProperty.floatValue = EditorGUI.IntField (position, label, Mathf.RoundToInt (valueProperty.floatValue));
					else
						valueProperty.floatValue = EditorGUI.FloatField (position, label, valueProperty.floatValue);
				}

				valueProperty.floatValue = statInfo.Filter (valueProperty.floatValue);

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