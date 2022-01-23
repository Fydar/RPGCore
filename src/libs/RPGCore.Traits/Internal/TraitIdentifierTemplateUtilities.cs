using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Traits.Internal;

internal static class TraitIdentifierTemplateUtilities
{
	internal static IEnumerable<T> GetLogicalMembers<T>(Type type)
	{
		return GetLogicalMembers<T>(type, null);
	}

	internal static IEnumerable<T> GetLogicalMembers<T>(object instance)
	{
		return GetLogicalMembers<T>(instance.GetType(), instance);
	}

	internal static IEnumerable<T> GetLogicalMembers<T>(Type source, object instance)
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
			object fieldValueObject = field.GetValue(instance);

			if (field.FieldType == typeof(T))
			{
				var fieldValue = (T)fieldValueObject;

				yield return fieldValue;
			}
			else if (typeof(ITraitIdentifierStructure).IsAssignableFrom(field.FieldType))
			{
				foreach (var childMember in GetLogicalMembers<T>(field.FieldType, fieldValueObject))
				{
					yield return childMember;
				}
			}
			else if (fieldValueObject is IEnumerable enumerable)
			{
				foreach (object child in enumerable)
				{
					if (child == null)
					{
						continue;
					}

					foreach (var childMember in GetLogicalMembers<T>(child.GetType(), child))
					{
						yield return childMember;
					}
				}
			}
		}

		foreach (var property in properties)
		{
			object fieldValueObject = property.GetValue(instance);

			if (property.PropertyType == typeof(T))
			{
				var fieldValue = (T)fieldValueObject;

				yield return fieldValue;
			}
			else if (typeof(ITraitIdentifierStructure).IsAssignableFrom(property.PropertyType))
			{
				foreach (var childMember in GetLogicalMembers<T>(property.PropertyType, fieldValueObject))
				{
					yield return childMember;
				}
			}
			else if (fieldValueObject is IEnumerable enumerable)
			{
				foreach (object child in enumerable)
				{
					if (child == null)
					{
						continue;
					}

					foreach (var childMember in GetLogicalMembers<T>(child.GetType(), child))
					{
						yield return childMember;
					}
				}
			}
		}
	}
}
