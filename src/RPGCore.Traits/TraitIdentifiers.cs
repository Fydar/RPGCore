using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Traits
{
	public static class TraitIdentifiers
	{
		private static readonly Dictionary<Type, StatIdentifier[]> statsCache = new Dictionary<Type, StatIdentifier[]>();
		private static readonly Dictionary<Type, StateIdentifier[]> statesCache = new Dictionary<Type, StateIdentifier[]>();

		public static StatIdentifier[] AllStats(Type source)
		{
			if (!statsCache.TryGetValue(source, out var cache))
			{
				cache = GetAllMembers<StatIdentifier>(source);
				statsCache[source] = cache;
			}
			return cache;
		}

		public static StateIdentifier[] AllStates(Type source)
		{
			if (!statesCache.TryGetValue(source, out var cache))
			{
				cache = GetAllMembers<StateIdentifier>(source);
				statesCache[source] = cache;
			}
			return cache;
		}

		private static T[] GetAllMembers<T>(Type source)
			where T : struct
		{
			var foundMembers = new List<T>();
			foundMembers.AddRange(GetAllMembersIterator<T>(source, null));
			return foundMembers.ToArray();
		}

		private static IEnumerable<T> GetAllMembersIterator<T>(Type source, object instance)
		{
			BindingFlags bindingFlags;
			if (instance == null)
			{
				bindingFlags = BindingFlags.Public | BindingFlags.Static;
			}
			else
			{
				bindingFlags = BindingFlags.Public | BindingFlags.Instance;
			}

			var fields = source.GetFields(bindingFlags);
			var properties = source.GetProperties(bindingFlags);

			foreach (var field in fields)
			{
				if (field.FieldType == typeof(T))
				{
					object fieldValueObject = field.GetValue(instance);
					var fieldValue = (T)fieldValueObject;

					yield return fieldValue;
				}
				else if (typeof(ITraitTemplate).IsAssignableFrom(field.FieldType))
				{
					object fieldValueObject = field.GetValue(instance);

					foreach (var childMember in GetAllMembersIterator<T>(field.FieldType, fieldValueObject))
					{
						yield return childMember;
					}
				}
			}

			foreach (var property in properties)
			{
				if (property.PropertyType == typeof(T))
				{
					object fieldValueObject = property.GetValue(instance);
					var fieldValue = (T)fieldValueObject;

					yield return fieldValue;
				}
				else if (typeof(ITraitTemplate).IsAssignableFrom(property.PropertyType))
				{
					object fieldValueObject = property.GetValue(instance);

					foreach (var childMember in GetAllMembersIterator<T>(property.PropertyType, fieldValueObject))
					{
						yield return childMember;
					}
				}
			}
		}
	}
}
