using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace RPGCore.DataEditor.Manifest.Source.RuntimeSource;

public static class RuntimeUtility
{
	public static TypeName DescribeType(Type type)
	{
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
		{
			var nullableType = Nullable.GetUnderlyingType(type);
			var innerType = DescribeType(nullableType);
			innerType.IsNullable = true;
			return innerType;
		}
		else if (type.IsArray)
		{
			var elementType = type.GetElementType();
			return TypeName.ForArray(DescribeType(elementType));
		}
		else if (type.IsGenericType
			&& !type.IsGenericTypeDefinition
			&& type.GetGenericTypeDefinition() == typeof(List<>))
		{
			var elementType = type.GenericTypeArguments[0];
			return TypeName.ForArray(DescribeType(elementType));
		}
		else if (type.IsGenericType
			&& !type.IsGenericTypeDefinition
			&& type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
		{
			var genericArguments = type.GetGenericArguments();
			return TypeName.ForDictionary(
				DescribeType(genericArguments[0]),
				DescribeType(genericArguments[1])
			);
		}
		else
		{
			var genericArguements = type.GetGenericArguments();

			var genericTypes = new TypeName[genericArguements.Length];
			for (int i = 0; i < genericArguements.Length; i++)
			{
				genericTypes[i] = DescribeType(genericArguements[i]);
			}

			return new TypeName(ToIdentifier(type), genericTypes);
		}
	}

	public static object? CreateInstance(Type type)
	{
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
		{
			var nullableType = Nullable.GetUnderlyingType(type);
			return CreateInstance(nullableType);
		}

		object? instancedObject = null;
		if (type == typeof(string))
		{
			instancedObject = string.Empty;
		}
		else if (type.IsArray)
		{
			instancedObject = Array.CreateInstance(type.GetElementType(), 0);
		}
		else if (type.IsValueType)
		{
			try
			{
				instancedObject = Activator.CreateInstance(type);
			}
			catch
			{
			}
		}
		else
		{
			var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
			if (constructors.Length == 0 || constructors.Any(c => c.GetParameters().Length == 0))
			{
				try
				{
					instancedObject = Activator.CreateInstance(type);
				}
				catch
				{
				}
			}
		}
		return instancedObject;
	}

	public static string ToIdentifier(Type type)
	{
		string typeIdentifier = type.Name;

		if (type == typeof(string))
		{
			typeIdentifier = "string";
		}
		else if (type == typeof(bool))
		{
			typeIdentifier = "bool";
		}
		else if (type == typeof(int))
		{
			typeIdentifier = "int";
		}
		else if (type == typeof(byte))
		{
			typeIdentifier = "byte";
		}
		else if (type == typeof(long))
		{
			typeIdentifier = "long";
		}
		else if (type == typeof(short))
		{
			typeIdentifier = "short";
		}
		else if (type == typeof(uint))
		{
			typeIdentifier = "uint";
		}
		else if (type == typeof(ulong))
		{
			typeIdentifier = "ulong";
		}
		else if (type == typeof(ushort))
		{
			typeIdentifier = "ushort";
		}
		else if (type == typeof(sbyte))
		{
			typeIdentifier = "sbyte";
		}
		else if (type == typeof(char))
		{
			typeIdentifier = "char";
		}
		else if (type == typeof(float))
		{
			typeIdentifier = "float";
		}
		else if (type == typeof(double))
		{
			typeIdentifier = "double";
		}
		else if (type == typeof(decimal))
		{
			typeIdentifier = "decimal";
		}
		return typeIdentifier;
	}

	public static bool IsNullable(PropertyInfo property)
	{
		return IsNullableHelper(property.PropertyType, property.DeclaringType, property.CustomAttributes);
	}

	public static bool IsNullable(FieldInfo field)
	{
		return IsNullableHelper(field.FieldType, field.DeclaringType, field.CustomAttributes);
	}

	private static bool IsNullableHelper(Type memberType, MemberInfo? declaringType, IEnumerable<CustomAttributeData> customAttributes)
	{
		if (memberType.IsValueType)
		{
			return Nullable.GetUnderlyingType(memberType) != null;
		}

		var nullable = customAttributes
			.FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
		if (nullable != null && nullable.ConstructorArguments.Count == 1)
		{
			var attributeArgument = nullable.ConstructorArguments[0];
			if (attributeArgument.ArgumentType == typeof(byte[]))
			{
				var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;
				if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
				{
					return (byte)args[0].Value! == 2;
				}
			}
			else if (attributeArgument.ArgumentType == typeof(byte))
			{
				return (byte)attributeArgument.Value! == 2;
			}
		}

		var type = declaringType;
		while (type != null)
		{
			var context = type.CustomAttributes
				.FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");

			if (context != null
				&& context.ConstructorArguments.Count == 1
				&& context.ConstructorArguments[0].ArgumentType == typeof(byte))
			{
				return (byte)context.ConstructorArguments[0].Value! == 2;
			}

			type = type.DeclaringType;
		}

		// Couldn't find a suitable attribute
		return false;
	}
}
