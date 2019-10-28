using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Traits
{
	public static class TraitIdentifiers
	{
		private static readonly Dictionary<Type, StatIdentifier[]> StatsCache = new Dictionary<Type, StatIdentifier[]> ();
		private static readonly Dictionary<Type, StateIdentifier[]> StatesCache = new Dictionary<Type, StateIdentifier[]> ();

		public static StatIdentifier[] AllStats (Type source)
		{
			if (StatsCache.TryGetValue (source, out var cache))
			{
				return cache;
			}

			cache = GetAllMembers<StatIdentifier> (source);
			StatsCache[source] = cache;
			return cache;
		}

		public static StateIdentifier[] AllStates (Type source)
		{
			if (StatesCache.TryGetValue (source, out var cache))
			{
				return cache;
			}

			cache = GetAllMembers<StateIdentifier> (source);
			StatesCache[source] = cache;
			return cache;
		}

		private static T[] GetAllMembers<T> (Type source)
			where T : struct
		{
			var foundMembers = new List<T> ();

			foreach (var field in source.GetFields (BindingFlags.Public | BindingFlags.Static))
			{
				if (field.FieldType == typeof (T))
				{
					object fieldValueObject = field.GetValue (null);
					var fieldValue = (T)fieldValueObject;

					foundMembers.Add (fieldValue);
				}
			}

			return foundMembers.ToArray ();
		}
	}
}
