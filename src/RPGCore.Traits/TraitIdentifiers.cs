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
			if (statsCache.TryGetValue(source, out var cache))
			{
				return cache;
			}

			cache = GetAllMembers<StatIdentifier>(source);
			statsCache[source] = cache;
			return cache;
		}

		public static StateIdentifier[] AllStates(Type source)
		{
			if (statesCache.TryGetValue(source, out var cache))
			{
				return cache;
			}

			cache = GetAllMembers<StateIdentifier>(source);
			statesCache[source] = cache;
			return cache;
		}

		private static T[] GetAllMembers<T>(Type source)
			where T : struct
		{
			var foundMembers = new List<T>();

			foreach (var field in source.GetFields(BindingFlags.Public | BindingFlags.Static))
			{
				if (field.FieldType == typeof(T))
				{
					object fieldValueObject = field.GetValue(null);
					var fieldValue = (T)fieldValueObject;

					foundMembers.Add(fieldValue);
				}
			}

			return foundMembers.ToArray();
		}
	}
}
