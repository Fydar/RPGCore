using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace RPGCore.Tables
{
	[Serializable]
	public class TableEntry
	{
		public float Balance;

		public TableEntry (float balance)
		{
			Balance = balance;
		}
	}

	public class GenericTableEntry<T> : TableEntry
	{
		public T Item;

		public GenericTableEntry (T item, float balance) : base (balance)
		{
			Item = item;
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer (typeof (TableEntry), true)]
	class EntryDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return StandardHeight (property, label);
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			StandardOnGUI (position, property, label);
		}

		public static void StandardOnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty itemProperty = property.FindPropertyRelative ("Item");
			SerializedProperty balanceProperty = property.FindPropertyRelative ("Balance");

			Rect itemRect = new Rect (position.x, position.y, position.width * 0.95f - 40, position.height);
			Rect balanceRect = new Rect (itemRect.xMax, position.y, position.width - itemRect.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.PropertyField (itemRect, itemProperty, GUIContent.none, false);

			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			EditorGUI.PropertyField (balanceRect, balanceProperty, GUIContent.none, false);
			EditorGUI.indentLevel = indent;
		}

		public static float StandardHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight (property.FindPropertyRelative ("Item"));
		}
	}
#endif
}