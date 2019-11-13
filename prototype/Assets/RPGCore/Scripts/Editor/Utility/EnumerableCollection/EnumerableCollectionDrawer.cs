﻿#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPGCore
{
	[CustomPropertyDrawer(typeof(EnumerableCollection), true)]
	public class EnumerableCollectionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!property.isExpanded)
			{
				return EditorGUIUtility.singleLineHeight;
			}

			var directoriesProperty = property.FindPropertyRelative("fieldDirectories");
			var valuesProperty = property.FindPropertyRelative("fieldValues");
			float total = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			foreach (var child in GetChildren(property))
			{
				total += EditorGUI.GetPropertyHeight(child, label, true) + EditorGUIUtility.standardVerticalSpacing;
			}

			string lastDirectory = "";
			int lastDirectorySeperator = -1;
			bool lastExpanded = true;

			for (int i = 0; i < directoriesProperty.arraySize; i++)
			{
				var directory = directoriesProperty.GetArrayElementAtIndex(i);
				var value = valuesProperty.GetArrayElementAtIndex(i);

				lastDirectorySeperator = directory.stringValue.IndexOf('/');
				string folderName = directory.stringValue.Substring(0, lastDirectorySeperator);

				if (folderName != lastDirectory)
				{
					lastExpanded = directory.isExpanded;
					total += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
				}

				if (lastExpanded)
				{
					total += EditorGUI.GetPropertyHeight(value) + EditorGUIUtility.standardVerticalSpacing;
				}

				lastDirectory = folderName;
			}

			return total;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var marchingRect = new Rect(position)
			{
				height = EditorGUIUtility.singleLineHeight
			};

			property.isExpanded = EditorGUI.Foldout(marchingRect, property.isExpanded, label, true);

			if (!property.isExpanded)
			{
				return;
			}

			EditorGUI.indentLevel++;
			marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;

			var valuesProperty = property.FindPropertyRelative("fieldValues");
			var directoriesProperty = property.FindPropertyRelative("fieldDirectories");

			foreach (var child in GetChildren(property))
			{
				marchingRect.height = EditorGUI.GetPropertyHeight(child);

				EditorGUI.PropertyField(marchingRect, child, true);

				marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;
			}

			string lastDirectory = "";
			int lastDirectorySeperator = -1;
			bool lastExpanded = true;

			for (int i = 0; i < directoriesProperty.arraySize; i++)
			{
				var directory = directoriesProperty.GetArrayElementAtIndex(i);

				lastDirectorySeperator = directory.stringValue.IndexOf('/');
				string folderName = directory.stringValue.Substring(0, lastDirectorySeperator);

				if (folderName != lastDirectory)
				{
					marchingRect.height = EditorGUIUtility.singleLineHeight;

					directory.isExpanded = EditorGUI.Foldout(marchingRect, directory.isExpanded, folderName, true);
					lastExpanded = directory.isExpanded;

					marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;
				}

				if (lastExpanded)
				{
					var value = valuesProperty.GetArrayElementAtIndex(i);
					EditorGUI.indentLevel++;
					marchingRect.height = EditorGUI.GetPropertyHeight(value);

					EditorGUI.PropertyField(marchingRect, value, new GUIContent(
						directory.stringValue.Substring(lastDirectorySeperator + 1)), true);

					marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;
					EditorGUI.indentLevel--;
				}

				lastDirectory = folderName;
			}

			EditorGUI.indentLevel--;
		}

		private static IEnumerable<SerializedProperty> GetChildren(SerializedProperty property)
		{
			property = property.Copy();
			var nextElement = property.Copy();

			if (!nextElement.NextVisible(false))
			{
				nextElement = null;
			}

			property.NextVisible(true);

			while (true)
			{
				if ((SerializedProperty.EqualContents(property, nextElement)))
				{
					yield break;
				}

				yield return property;

				if (!property.NextVisible(false))
				{
					break;
				}
			}
		}
	}
}
#endif
