using UnityEditor;
using UnityEngine;

namespace RPGCore.Utility.Editors
{
	[CustomPropertyDrawer (typeof (ErrorIfNullAttribute))]
	public class ErrorIfNullAttributeDrawer : PropertyDrawer
	{
		//private const float Padding = 0;
		//private const float HalfPadding = Padding / 2.0f;

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight (property, label);
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.ObjectReference)
			{
				EditorGUI.HelpBox (position, "The " + typeof (ErrorIfNullAttribute).Name + " is not valid on "
					+ property.propertyType + " fields.", MessageType.Error);
				return;
			}

			Rect labelRect, fieldRect;
			PropertyUtility.PrefixLable (position, out labelRect, out fieldRect);
			EditorGUI.PropertyField (position, property, label);

			if (property.objectReferenceValue == null)
			{
				ErrorIfNullAttribute errorAttribute = (ErrorIfNullAttribute)attribute;

				GUIContent content = new GUIContent ("", DrawerLibraryResources.Instance.ErrorIcon, errorAttribute.ErrorMessage);
				GUI.Label (new Rect (fieldRect.x - fieldRect.height - 2, fieldRect.y,
					fieldRect.height, fieldRect.height), content);

				//GUI.DrawTexture (new Rect (fieldRect.x - fieldRect.height - 2 + HalfPadding, fieldRect.y + HalfPadding,
				//	fieldRect.height - Padding, fieldRect.height - Padding), DrawerLibraryResources.Instance.ErrorIcon);
			}
		}
	}
}

