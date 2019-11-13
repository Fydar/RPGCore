#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RPGCore.Utility.Editors
{
	[CustomPropertyDrawer(typeof(CollectionEntry), true)]
	public class CollectionEntryDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var entry = (CollectionEntry)PropertyUtility.GetTargetObjectOfProperty(property);

			var idProperty = property.FindPropertyRelative("field");
			var info = EnumerableCollection.GetReflectionInformation(entry.IndexType);

			string[] names = info.fieldNames;

			int currentIndex = -1;
			for (int i = 0; i < names.Length; i++)
			{
				if (names[i] == idProperty.stringValue)
				{
					currentIndex = i;
					break;
				}
			}
			if (currentIndex == -1)
			{
				currentIndex = 0;
				idProperty.stringValue = names[EditorGUI.Popup(position, label.text, currentIndex, names)];
			}

			EditorGUI.BeginChangeCheck();

			idProperty.stringValue = names[EditorGUI.Popup(position, label.text, currentIndex, names)];

			if (EditorGUI.EndChangeCheck())
			{
				entry.entryIndex = -1;
			}
		}
	}
}
#endif
