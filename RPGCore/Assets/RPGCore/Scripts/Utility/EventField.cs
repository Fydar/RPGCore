using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace RPGCore.Utility
{
	public class EventField<T> : EventField
	{
		[SerializeField]
		private T internalValue;

		public EventField () { }

		public EventField (T defaultValue)
		{
			internalValue = defaultValue;
		}

		public T Value
		{
			get
			{
				return internalValue;
			}
			set
			{
				if (!EqualityComparer<T>.Default.Equals (internalValue, value))
				{
					internalValue = value;

					InvokeChanged ();
				}
			}
		}


#if UNITY_EDITOR
		[CustomPropertyDrawer (typeof (EventField<>), true)]
		class EventFieldDrawer : PropertyDrawer
		{
			public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
			{
				property.Next (true);

				return EditorGUI.GetPropertyHeight (property);
			}

			public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
			{
				EventField target = (EventField)GetTargetObjectOfProperty (property);

				property.Next (true);

				EditorGUI.BeginChangeCheck ();

				EditorGUI.PropertyField (position, property, label, true);

				property.serializedObject.ApplyModifiedProperties ();

				if (EditorGUI.EndChangeCheck ())
				{
					target.InvokeChanged ();
				}
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

	public abstract class EventField
	{
		public event Action onChanged;

		public void ResetEvents ()
		{
			onChanged = null;
		}

		public void InvokeChanged ()
		{
			if (onChanged != null)
			{
				onChanged ();
			}
		}
	}
}

