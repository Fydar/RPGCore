using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore
{
	[System.Serializable]
	public class ItemGenerator
	{
		public ItemTemplate RewardTemplate;

		public int MinCount = 1;
		public int MaxCount = 1;

		public bool OverridePrefix;
		public bool OverrideSuffix;
		public bool OverrideModifiers;

		public EnchantmentSelector Prefix;
		public EnchantmentSelector Suffix;
		public EnchantmentSelector[] Modifiers;

		public virtual ItemSurrogate Generate()
		{
			if (RewardTemplate == null)
			{
				return null;
			}

			int count = Random.Range(MinCount, MaxCount);
			var generatedItem = RewardTemplate.GenerateItem();
			generatedItem.Quantity = count;
			return generatedItem;
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(ItemGenerator))]
	public class ItemGeneratorDrawer : PropertyDrawer
	{
		private static float INNER_SPACING = 6.0f;
		private static float OUTER_SPACING = 4.0f;

		private const float FoldoutIndent = 8;
		public const float countWidth = 26;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var itemProperty = property.FindPropertyRelative("RewardTemplate");

			var template = (ItemTemplate)itemProperty.objectReferenceValue;

			if (!property.isExpanded || template == null || template.StackSize != 1)
			{
				return EditorGUIUtility.singleLineHeight;
			}

			float totalHeight = (EditorGUIUtility.singleLineHeight * 2) +
				(EditorGUIUtility.standardVerticalSpacing * 2);

			var modifiersProperty = property.FindPropertyRelative("Modifiers");
			var overrideModifiersProperty = property.FindPropertyRelative("OverrideModifiers");

			if (overrideModifiersProperty.boolValue && modifiersProperty.isExpanded)
			{
				totalHeight += (EditorGUIUtility.standardVerticalSpacing * 2) +
				EditorGUI.GetPropertyHeight(modifiersProperty);
			}
			else
			{
				totalHeight += EditorGUIUtility.singleLineHeight;
			}

			totalHeight += INNER_SPACING * 2;
			totalHeight += OUTER_SPACING * 2;

			return totalHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var marchingRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

			var itemProperty = property.FindPropertyRelative("RewardTemplate");
			var minProperty = property.FindPropertyRelative("MinCount");
			var maxProperty = property.FindPropertyRelative("MaxCount");
			var prefixProperty = property.FindPropertyRelative("Prefix");
			var suffixProperty = property.FindPropertyRelative("Suffix");
			var modifiersProperty = property.FindPropertyRelative("Modifiers");
			var overridePrefixProperty = property.FindPropertyRelative("OverridePrefix");
			var overrideSuffixProperty = property.FindPropertyRelative("OverrideSuffix");
			var overrideModifiersProperty = property.FindPropertyRelative("OverrideModifiers");

			var itemRect = new Rect(marchingRect.x + FoldoutIndent, marchingRect.y, (marchingRect.width - countWidth * 2.0f) - FoldoutIndent, marchingRect.height);
			var foldoutRect = new Rect(itemRect.x, itemRect.y, 10, marchingRect.height)
			{
				xMax = EditorGUI.IndentedRect(itemRect).xMin + 7
			};

			var minRect = new Rect(itemRect.xMax, marchingRect.y, countWidth, marchingRect.height);
			var maxRect = new Rect(minRect.xMax, marchingRect.y, countWidth, marchingRect.height);

			var template = (ItemTemplate)itemProperty.objectReferenceValue;

			if (template != null && template.StackSize == 1)
			{
				property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "", false);
			}

			EditorGUI.PropertyField(itemRect, itemProperty, GUIContent.none, false);

			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			if (template != null)
			{
				EditorGUI.PropertyField(minRect, minProperty, GUIContent.none, false);
				EditorGUI.PropertyField(maxRect, maxProperty, GUIContent.none, false);

				maxProperty.intValue = Mathf.Max(1, maxProperty.intValue);
				minProperty.intValue = Mathf.Min(Mathf.Max(1, minProperty.intValue), maxProperty.intValue);
			}
			else
			{
				EditorGUI.BeginDisabledGroup(true);

				EditorGUI.DelayedIntField(minRect, GUIContent.none, 0);
				EditorGUI.DelayedIntField(maxRect, GUIContent.none, 0);

				EditorGUI.EndDisabledGroup();
			}
			EditorGUI.indentLevel = indent;

			if (property.isExpanded && template != null && template.StackSize == 1)
			{
				int dropdownOriginalIndent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;

				marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing + INNER_SPACING + OUTER_SPACING;

				float modifiersHeight = EditorGUI.GetPropertyHeight(modifiersProperty);

				EditorGUI.indentLevel += 1;
				var backgroundIndent = EditorGUI.IndentedRect(marchingRect);
				EditorGUI.indentLevel -= 1;

				EditorGUI.indentLevel += 2;
				var indentRect = EditorGUI.IndentedRect(marchingRect);
				EditorGUI.indentLevel -= 2;

				if (overrideModifiersProperty.boolValue)
				{
					EditorGUI.HelpBox(new Rect(backgroundIndent.x, backgroundIndent.y - INNER_SPACING, backgroundIndent.width,
						(EditorGUIUtility.standardVerticalSpacing * 2) + EditorGUIUtility.singleLineHeight + modifiersHeight +
						(INNER_SPACING * 2)),
						"", MessageType.None);
				}
				else
				{
					EditorGUI.HelpBox(new Rect(backgroundIndent.x, backgroundIndent.y - INNER_SPACING, backgroundIndent.width,
						(EditorGUIUtility.standardVerticalSpacing * 2) + (EditorGUIUtility.singleLineHeight * 2) +
						(INNER_SPACING * 2)),
						"", MessageType.None);
				}

				var prefixToggleRect = new Rect(indentRect.x - 10, indentRect.y, EditorGUIUtility.singleLineHeight, indentRect.height);
				var prefixRect = new Rect(prefixToggleRect.xMax, indentRect.y, (indentRect.width * 0.5f) - EditorGUIUtility.singleLineHeight, indentRect.height);

				var suffixToggleRect = new Rect(prefixRect.xMax, indentRect.y, EditorGUIUtility.singleLineHeight, indentRect.height);
				var suffixRect = new Rect(suffixToggleRect.xMax, indentRect.y, prefixRect.width, indentRect.height);

				int originalIndent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;

				overridePrefixProperty.boolValue = !EditorGUI.Toggle(prefixToggleRect, !overridePrefixProperty.boolValue);
				if (overridePrefixProperty.boolValue)
				{
					EditorGUI.PropertyField(prefixRect, prefixProperty, GUIContent.none);
				}
				else
				{
					EditorGUI.LabelField(prefixRect, "Default Prefix");
				}

				overrideSuffixProperty.boolValue = !EditorGUI.Toggle(suffixToggleRect, !overrideSuffixProperty.boolValue);
				if (overrideSuffixProperty.boolValue)
				{
					EditorGUI.PropertyField(suffixRect, suffixProperty, GUIContent.none);
				}
				else
				{
					EditorGUI.LabelField(suffixRect, "Default Suffix");
				}

				EditorGUI.indentLevel = originalIndent;

				marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;

				marchingRect.height = modifiersHeight;
				marchingRect.height = EditorGUIUtility.standardVerticalSpacing;

				EditorGUI.indentLevel += 2;
				indentRect = EditorGUI.IndentedRect(marchingRect);

				var modifiersToggleRect = new Rect(indentRect.x - 10, indentRect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
				var modifiersRect = new Rect(modifiersToggleRect.xMax + 12, modifiersToggleRect.y,
					marchingRect.xMax - (modifiersToggleRect.xMax + 18), modifiersHeight);

				EditorGUI.indentLevel -= 2;
				originalIndent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				overrideModifiersProperty.boolValue = !EditorGUI.Toggle(modifiersToggleRect, !overrideModifiersProperty.boolValue);

				if (overrideModifiersProperty.boolValue)
				{
					marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;

					EditorGUI.PropertyField(modifiersRect, modifiersProperty, true);
				}
				else
				{
					EditorGUI.LabelField(new Rect(modifiersRect.x - 12, modifiersRect.y,
						modifiersRect.width, modifiersRect.height), "Default Modifiers");
				}
				EditorGUI.indentLevel = originalIndent;

				EditorGUI.indentLevel = dropdownOriginalIndent;
			}
			else
			{
				prefixProperty.objectReferenceValue = null;
				suffixProperty.objectReferenceValue = null;
				modifiersProperty.ClearArray();
			}
		}
	}
#endif
}
