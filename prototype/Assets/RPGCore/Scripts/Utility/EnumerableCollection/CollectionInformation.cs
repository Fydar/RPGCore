using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore
{
	public class CollectionInformation
	{
		public FieldInfo[] directFields;
		public string[] fieldNames;

		public int IndexOf(string fieldName)
		{
			for (int i = 0; i < fieldNames.Length; i++)
			{
				if (fieldNames[i] == fieldName)
				{
					return i;
				}
			}
			UnityEngine.Debug.LogError("\"" + fieldName + "\" is not a member of " + GetType().Name + ".");
			return -1;
		}

		public CollectionInformation(Type type)
		{
			var collectionType = EnumerableCollection.BaseCollectionType(type);

			directFields = collectionType.GetFields(
				BindingFlags.Instance | BindingFlags.Public |
				BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

			var foundNames = new List<string>(directFields.Length);

			foreach (var targetField in directFields)
			{
				string lastName = targetField.Name;
				if (targetField.FieldType.Name == "T")
				{
					foundNames.Add(lastName);
				}
				else if (typeof(EnumerableCollection).IsAssignableFrom(targetField.FieldType))
				{
					var information = EnumerableCollection.GetReflectionInformation(targetField.FieldType);

					foreach (var childInfo in information.directFields)
					{
						if (childInfo.FieldType.Name == "T")
						{
							foundNames.Add(lastName + "/" + childInfo.Name);
						}
					}
				}
			}
			fieldNames = foundNames.ToArray();
		}
	}
}

