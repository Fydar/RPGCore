using UnityEngine;
using System.Collections;
using RPGCore.Tables;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore
{
	[System.Serializable]
	public class ItemGenerator
	{
		public ItemTemplate RewardTemplate = null;

		public int MinCount = 1;
		public int MaxCount = 1;

		public bool OverridePrefix = false;
		public bool OverrideSuffix = false;
		public bool OverrideModifiers = false;

		public EnchantmentSelector Prefix = null;
		public EnchantmentSelector Suffix = null;
		public EnchantmentSelector[] Modifiers = null;

		public virtual ItemSurrogate Generate ()
		{
			if (RewardTemplate == null)
				return null;

			int count = Random.Range (MinCount, MaxCount);
			ItemSurrogate generatedItem = RewardTemplate.GenerateItem ();
			generatedItem.Quantity = count;
			return generatedItem;
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer (typeof (ItemGenerator))]
	public class ItemGeneratorDrawer : PropertyDrawer
	{
		private static float INNER_SPACING = 6.0f;
		private static float OUTER_SPACING = 4.0f;

		private const float FoldoutIndent = 8;
		public const float countWidth = 26;

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			SerializedProperty itemProperty = property.FindPropertyRelative ("RewardTemplate");

			ItemTemplate template = (ItemTemplate)itemProperty.objectReferenceValue;

			if (!property.isExpanded || template == null || template.StackSize != 1)
				return EditorGUIUtility.singleLineHeight;

			float totalHeight = (EditorGUIUtility.singleLineHeight * 2) +
				(EditorGUIUtility.standardVerticalSpacing * 2);

			SerializedProperty modifiersProperty = property.FindPropertyRelative ("Modifiers");
			SerializedProperty overrideModifiersProperty = property.FindPropertyRelative ("OverrideModifiers");

			if (overrideModifiersProperty.boolValue && modifiersProperty.isExpanded)
			{
				totalHeight += (EditorGUIUtility.standardVerticalSpacing * 2) +
				EditorGUI.GetPropertyHeight (modifiersProperty);
			}
			else
			{
				totalHeight += EditorGUIUtility.singleLineHeight;
			}

			totalHeight += INNER_SPACING * 2;
			totalHeight += OUTER_SPACING * 2;

			return totalHeight;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			Rect marchingRect = new Rect (position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

			SerializedProperty itemProperty = property.FindPropertyRelative ("RewardTemplate");
			SerializedProperty minProperty = property.FindPropertyRelative ("MinCount");
			SerializedProperty maxProperty = property.FindPropertyRelative ("MaxCount");
			SerializedProperty prefixProperty = property.FindPropertyRelative ("Prefix");
			SerializedProperty suffixProperty = property.FindPropertyRelative ("Suffix");
			SerializedProperty modifiersProperty = property.FindPropertyRelative ("Modifiers");
			SerializedProperty overridePrefixProperty = property.FindPropertyRelative ("OverridePrefix");
			SerializedProperty overrideSuffixProperty = property.FindPropertyRelative ("OverrideSuffix");
			SerializedProperty overrideModifiersProperty = property.FindPropertyRelative ("OverrideModifiers");

			Rect itemRect = new Rect (marchingRect.x + FoldoutIndent, marchingRect.y, (marchingRect.width - countWidth * 2.0f) - FoldoutIndent, marchingRect.height);
			Rect foldoutRect = new Rect (itemRect.x, itemRect.y, 10, marchingRect.height);
			foldoutRect.xMax = EditorGUI.IndentedRect (itemRect).xMin + 7;

			Rect minRect = new Rect (itemRect.xMax, marchingRect.y, countWidth, marchingRect.height);
			Rect maxRect = new Rect (minRect.xMax, marchingRect.y, countWidth, marchingRect.height);

			ItemTemplate template = (ItemTemplate)itemProperty.objectReferenceValue;

			if (template != null && template.StackSize == 1)
			{
				property.isExpanded = EditorGUI.Foldout (foldoutRect, property.isExpanded, "", false);
			}

			EditorGUI.PropertyField (itemRect, itemProperty, GUIContent.none, false);

			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			if (template != null)
			{
				EditorGUI.PropertyField (minRect, minProperty, GUIContent.none, false);
				EditorGUI.PropertyField (maxRect, maxProperty, GUIContent.none, false);

				maxProperty.intValue = Mathf.Max (1, maxProperty.intValue);
				minProperty.intValue = Mathf.Min (Mathf.Max (1, minProperty.intValue), maxProperty.intValue);
			}
			else
			{
				EditorGUI.BeginDisabledGroup (true);

				EditorGUI.DelayedIntField (minRect, GUIContent.none, 0);
				EditorGUI.DelayedIntField (maxRect, GUIContent.none, 0);

				EditorGUI.EndDisabledGroup ();
			}
			EditorGUI.indentLevel = indent;

			if (property.isExpanded && template != null && template.StackSize == 1)
			{
				int dropdownOriginalIndent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;

				marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing + INNER_SPACING + OUTER_SPACING;

				float modifiersHeight = EditorGUI.GetPropertyHeight (modifiersProperty);

				EditorGUI.indentLevel += 1;
				Rect backgroundIndent = EditorGUI.IndentedRect (marchingRect);
				EditorGUI.indentLevel -= 1;

				EditorGUI.indentLevel += 2;
				Rect indentRect = EditorGUI.IndentedRect (marchingRect);
				EditorGUI.indentLevel -= 2;

				if (overrideModifiersProperty.boolValue)
				{
					EditorGUI.HelpBox (new Rect (backgroundIndent.x, backgroundIndent.y - INNER_SPACING, backgroundIndent.width,
						(EditorGUIUtility.standardVerticalSpacing * 2) + EditorGUIUtility.singleLineHeight + modifiersHeight +
						(INNER_SPACING * 2)),
						"", MessageType.None);
				}
				else
				{
					EditorGUI.HelpBox (new Rect (backgroundIndent.x, backgroundIndent.y - INNER_SPACING, backgroundIndent.width,
						(EditorGUIUtility.standardVerticalSpacing * 2) + (EditorGUIUtility.singleLineHeight * 2) +
						(INNER_SPACING * 2)),
						"", MessageType.None);
				}

				Rect prefixToggleRect = new Rect (indentRect.x - 10, indentRect.y, EditorGUIUtility.singleLineHeight, indentRect.height);
				Rect prefixRect = new Rect (prefixToggleRect.xMax, indentRect.y, (indentRect.width * 0.5f) - EditorGUIUtility.singleLineHeight, indentRect.height);

				Rect suffixToggleRect = new Rect (prefixRect.xMax, indentRect.y, EditorGUIUtility.singleLineHeight, indentRect.height);
				Rect suffixRect = new Rect (suffixToggleRect.xMax, indentRect.y, prefixRect.width, indentRect.height);

				int originalIndent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;

				overridePrefixProperty.boolValue = !EditorGUI.Toggle (prefixToggleRect, !overridePrefixProperty.boolValue);
				if (overridePrefixProperty.boolValue)
					EditorGUI.PropertyField (prefixRect, prefixProperty, GUIContent.none);
				else
					EditorGUI.LabelField (prefixRect, "Default Prefix");


				overrideSuffixProperty.boolValue = !EditorGUI.Toggle (suffixToggleRect, !overrideSuffixProperty.boolValue);
				if (overrideSuffixProperty.boolValue)
					EditorGUI.PropertyField (suffixRect, suffixProperty, GUIContent.none);
				else
					EditorGUI.LabelField (suffixRect, "Default Suffix");


				EditorGUI.indentLevel = originalIndent;

				marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;

				marchingRect.height = modifiersHeight;
				marchingRect.height = EditorGUIUtility.standardVerticalSpacing;

				EditorGUI.indentLevel += 2;
				indentRect = EditorGUI.IndentedRect (marchingRect);

				Rect modifiersToggleRect = new Rect (indentRect.x - 10, indentRect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
				Rect modifiersRect = new Rect (modifiersToggleRect.xMax + 12, modifiersToggleRect.y,
					marchingRect.xMax - (modifiersToggleRect.xMax + 18), modifiersHeight);

				EditorGUI.indentLevel -= 2;
				originalIndent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				overrideModifiersProperty.boolValue = !EditorGUI.Toggle (modifiersToggleRect, !overrideModifiersProperty.boolValue);

				if (overrideModifiersProperty.boolValue)
				{
					marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;

					EditorGUI.PropertyField (modifiersRect, modifiersProperty, true);
				}
				else
				{
					EditorGUI.LabelField (new Rect (modifiersRect.x - 12, modifiersRect.y,
						modifiersRect.width, modifiersRect.height), "Default Modifiers");
				}
				EditorGUI.indentLevel = originalIndent;

				EditorGUI.indentLevel = dropdownOriginalIndent;
			}
			else
			{
				prefixProperty.objectReferenceValue = null;
				suffixProperty.objectReferenceValue = null;
				modifiersProperty.ClearArray ();
			}
		}
	}
#endif
}
