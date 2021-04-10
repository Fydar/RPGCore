using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCore.DataEditor.CSharp
{
	public static class BuiltInTypes
	{
		internal static readonly Type[] frameworkTypes = new[]
		{
			typeof(string),
			typeof(bool),
			typeof(int),
			typeof(byte),
			typeof(long),
			typeof(short),
			typeof(uint),
			typeof(ulong),
			typeof(ushort),
			typeof(sbyte),
			typeof(char),
			typeof(float),
			typeof(double),
			typeof(decimal),
		};

		internal static SchemaType Construct(Type type)
		{
			var information = new SchemaType();
			PopulateTypeInformation(type, information);

			return information;
		}

		public static SchemaQualifiedType DescribeType(Type type)
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
				var parameterTypes = new SchemaQualifiedType[]
				{
					DescribeType(elementType)
				};
				return new SchemaQualifiedType("[Array]", parameterTypes);
			}
			else if (type.IsGenericType
				&& !type.IsGenericTypeDefinition
				&& type.GetGenericTypeDefinition() == typeof(List<>))
			{
				var elementType = type.GenericTypeArguments[0];
				var parameterTypes = new SchemaQualifiedType[]
				{
					DescribeType(elementType)
				};
				return new SchemaQualifiedType("[Array]", parameterTypes);
			}
			else if (type.IsGenericType
				&& !type.IsGenericTypeDefinition
				&& type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
			{
				var genericArguments = type.GetGenericArguments();
				var parameterTypes = new SchemaQualifiedType[]
				{
					DescribeType(genericArguments[0]),
					DescribeType(genericArguments[1])
				};
				return new SchemaQualifiedType("[Dictionary]", parameterTypes);
			}
			else
			{
				return new SchemaQualifiedType(ToIdentifier(type));
			}
		}

		private static object? CreateInstance(Type type)
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

		private static void PopulateTypeInformation(Type type, SchemaType typeInformation)
		{
			typeInformation.Name = ToIdentifier(type);

			// Instancing
			object? instancedObject = CreateInstance(type);

			if (instancedObject != null)
			{
				string? result = Serialize(instancedObject);

				typeInformation.InstatedValue = result;
			}

			// Fields
			var fieldInfos = new List<SchemaField>();
			foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (field.FieldType.Name == "OutputSocket")
				{
					continue;
				}

				if (field.GetCustomAttribute<JsonIgnoreAttribute>() != null)
				{
					continue;
				}

				if (field.IsPrivate)
				{
					continue;
				}

				fieldInfos.Add(ConstructFieldInformation(type, field, instancedObject));
			}
			foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (property.PropertyType.Name == "OutputSocket")
				{
					continue;
				}

				if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null)
				{
					continue;
				}

				if (property.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
				{
					continue;
				}

				var getter = property.GetGetMethod();
				var setter = property.GetSetMethod();

				if (getter == null
					|| getter.IsPrivate
					|| setter == null)
				{
					continue;
				}

				fieldInfos.Add(ConstructFieldInformation(type, property, instancedObject));
			}
			typeInformation.Fields = fieldInfos;
		}

		private static string? Serialize(object instancedObject)
		{
			var type = instancedObject.GetType();

			if (type == typeof(string) || type == typeof(char))
			{
				return $"\"{instancedObject}\"";
			}
			if (type != typeof(object) && frameworkTypes.Contains(type))
			{
				return instancedObject.ToString();
			}

			string? serializedValue = JsonSerializer.Serialize(instancedObject);

			return serializedValue;
		}

		private static SchemaField ConstructFieldInformation(Type containerType, FieldInfo field, object? instancedObject)
		{
			return ConstructFieldInformation(containerType, new ModelField(field), instancedObject);
		}

		private static SchemaField ConstructFieldInformation(Type containerType, PropertyInfo property, object? instancedObject)
		{
			return ConstructFieldInformation(containerType, new ModelProperty(property), instancedObject);
		}

		private interface IModelMember
		{
			string Name { get; }
			Type ValueType { get; }
			bool IsNullable { get; }

			object[] GetCustomAttributes(bool inherit);
			object GetValue(object instance);
		}

		private class ModelField : IModelMember
		{
			public FieldInfo Field { get; }

			public Type ValueType => Field.FieldType;

			public string Name => Field.Name;

			public bool IsNullable => IsNullable(Field);

			public ModelField(FieldInfo field)
			{
				Field = field;
			}

			public object[] GetCustomAttributes(bool inherit)
			{
				return Field.GetCustomAttributes(inherit);
			}

			public object GetValue(object instance)
			{
				return Field.GetValue(instance);
			}
		}

		private class ModelProperty : IModelMember
		{
			public PropertyInfo Property { get; }

			public Type ValueType => Property.PropertyType;

			public string Name => Property.Name;

			public bool IsNullable => IsNullable(Property);

			public ModelProperty(PropertyInfo property)
			{
				Property = property;
			}

			public object[] GetCustomAttributes(bool inherit)
			{
				return Property.GetCustomAttributes(inherit);
			}

			public object GetValue(object instance)
			{
				return Property.GetValue(instance);
			}
		}

		private static SchemaField ConstructFieldInformation(Type containerType, IModelMember member, object? instancedObject)
		{
			var type = DescribeType(member.ValueType);

			if (!type.IsNullable)
			{
				type.IsNullable = member.IsNullable;
			}

			string? result = null;
			if (instancedObject == null)
			{
				instancedObject = CreateInstance(containerType);
			}
			if (instancedObject != null)
			{
				object? fieldValue = member.GetValue(instancedObject);

				if (fieldValue == null)
				{
					fieldValue = CreateInstance(member.ValueType);
				}

				result = Serialize(fieldValue);
			}

			return new SchemaField(member.Name, "", type, result);
		}

		private static string ToIdentifier(Type type)
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

		private static bool IsNullable(PropertyInfo property) =>
			IsNullableHelper(property.PropertyType, property.DeclaringType, property.CustomAttributes);

		private static bool IsNullable(FieldInfo field) =>
			IsNullableHelper(field.FieldType, field.DeclaringType, field.CustomAttributes);

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

			for (var type = declaringType; type != null; type = type.DeclaringType)
			{
				var context = type.CustomAttributes
					.FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
				if (context != null &&
					context.ConstructorArguments.Count == 1 &&
					context.ConstructorArguments[0].ArgumentType == typeof(byte))
				{
					return (byte)context.ConstructorArguments[0].Value! == 2;
				}
			}

			// Couldn't find a suitable attribute
			return false;
		}
	}
}
