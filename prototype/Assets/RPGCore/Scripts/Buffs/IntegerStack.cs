using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace RPGCore
{
	[Serializable]
	public class IntegerStack
	{
		public Action<int> OnValueChanged;

		private int delta;

		public float Delta
		{
			get
			{
				return delta;
			}
		}

		public void InvokeChanged ()
		{
			delta = Value - lastValue;
			lastValue = Value;

			if (OnValueChanged != null)
			{
				OnValueChanged (Value);
			}
		}

		public class Modifier
		{
			public IntegerStack Parent;
			public int currentValue;

			public int Value
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

			public Modifier (IntegerStack parent)
			{
				Parent = parent;
				Value = 0;
			}

			public Modifier (IntegerStack parent, int value)
			{
				Parent = parent;
				Value = value;
			}
		}
		public List<Modifier> FlatModifiers = new List<Modifier> ();

		[SerializeField]
		[FormerlySerializedAs ("BaseValue")]
		private int baseValue;

		[NonSerialized]
		private int lastValue;
		[NonSerialized]
		private bool isDirty = true;

		public int Value
		{
			get
			{
				if (isDirty)
				{
					int modifiedValue = baseValue;

					modifiedValue += Sum (FlatModifiers);

					lastValue = modifiedValue;
				}

				return lastValue;
			}
			set
			{
				BaseValue = value;
			}
		}

		public int BaseValue
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

		public Modifier AddFlatModifier (int startingValue)
		{
			Modifier mod = new Modifier (this, startingValue);
			FlatModifiers.Add (mod);

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

		protected int Sum (List<Modifier> modifiers)
		{
			int total = 0;

			for (int i = 0; i < modifiers.Count; i++)
			{
				total += modifiers[i].Value;
			}

			return total;
		}

		/*protected int GetMultiplier (List<Modifier> modifiers)
		{
			if (modifiers.Count == 0)
				return 1.0f;
			if (modifiers.Count == 1)
				return modifiers[0].Value;

			int total = modifiers[0].Value;

			for (int i = 1; i < modifiers.Count; i++)
			{
				total *= modifiers[i].Value;
			}

			return total;
		}*/
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer (typeof (IntegerStack))]
	public class IntegerStackDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight (property, label);
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty infoProperty = property.FindPropertyRelative ("Info");
			SerializedProperty valueProperty = property.FindPropertyRelative ("baseValue");

			if (infoProperty.objectReferenceValue == null)
				return;

			IntegerStack statInst = (IntegerStack)GetTargetObjectOfProperty (property);

			Rect fieldRect;

			if (Application.isPlaying)
			{
				fieldRect = new Rect (position.x, position.y, position.width - 40, position.height);
			}
			else
			{
				fieldRect = position;
			}

			valueProperty.intValue = EditorGUI.IntField (fieldRect, label, valueProperty.intValue);

			if (GUI.changed)
				statInst.InvokeChanged ();

			property.serializedObject.ApplyModifiedProperties ();
		}

		public static object GetTargetObjectOfProperty (SerializedProperty prop)
		{
			var path = prop.propertyPath.Replace (".Array.data[", "[");
			object obj = prop.serializedObject.targetObject;
			var elements = path.Split ('.');

			foreach (var element in elements)
			{
				if (element.Contains ("["))
				{
					var elementName = element.Substring (0, element.IndexOf ("["));
					var index = System.Convert.ToInt32 (element.Substring (element.IndexOf ("[")).Replace ("[", "").Replace ("]", ""));
					obj = GetObjectValue (obj, elementName, index);
				}
				else
				{
					obj = GetObjectValue (obj, element);
				}
			}
			return obj;
		}

		private static object GetObjectValue (object source, string name)
		{
			if (source == null)
				return null;

			var type = source.GetType ();

			while (type != null)
			{
				var f = type.GetField (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (f != null)
					return f.GetValue (source);

				var p = type.GetProperty (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (p != null)
					return p.GetValue (source, null);

				type = type.BaseType;
			}

			return null;
		}

		private static object GetObjectValue (object source, string name, int index)
		{
			var enumerable = GetObjectValue (source, name) as System.Collections.IEnumerable;

			if (enumerable == null)
				return null;

			var enm = enumerable.GetEnumerator ();

			for (int i = 0; i <= index; i++)
			{
				if (!enm.MoveNext ()) return null;
			}

			return enm.Current;
		}
	}
#endif
}

