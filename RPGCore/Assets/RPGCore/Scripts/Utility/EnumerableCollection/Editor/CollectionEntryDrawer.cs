using UnityEditor;
using UnityEngine;

namespace RPGCore.Utility.Editors
{
	[CustomPropertyDrawer (typeof (CollectionEntry))]
	public class CollectionEntryDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			object[] attributeObjects = fieldInfo.GetCustomAttributes (typeof (CollectionTypeAttribute), true);

			if (attributeObjects.Length == 0)
			{
				EditorGUI.HelpBox (position, "There are no " + typeof (CollectionEntry).Name + "'s on \"" +
					fieldInfo.Name + "\".", MessageType.None);
				return;
			}

			CollectionTypeAttribute collectionAttribute = (CollectionTypeAttribute)attributeObjects[0];

			SerializedProperty idProperty = property.FindPropertyRelative ("field");

			CollectionInformation info = EnumerableCollection.GetReflectionInformation (
											 collectionAttribute.collectionType);

			string[] names = info.fieldNames;

			int currentIndex = 0;
			for (int i = 0; i < names.Length; i++)
			{
				if (names[i] == idProperty.stringValue)
				{
					currentIndex = i;
					break;
				}
			}

			EditorGUI.BeginChangeCheck ();

			idProperty.stringValue = names[EditorGUI.Popup (position, label.text, currentIndex, names)];

			if (EditorGUI.EndChangeCheck ())
			{
				CollectionEntry entry = (CollectionEntry)PropertyUtility.GetTargetObjectOfProperty (property);

				entry.entryIndex = -1;
			}
		}
	}
}

