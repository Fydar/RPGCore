using RPGCore.DataEditor.Manifest.Source.RuntimeSource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCore.DataEditor.Manifest.Source.JsonSerializer
{
	public class JsonSerializerRuntimeTypeConverter : IRuntimeTypeConverter
	{
		private readonly JsonSerializerOptions jsonSerializerOptions;

		internal JsonSerializerRuntimeTypeConverter(JsonSerializerOptions jsonSerializerOptions)
		{
			this.jsonSerializerOptions = jsonSerializerOptions;
		}

		public SchemaType Convert(Type type)
		{
			// Instancing
			string? instatedValue = null;
			object? instancedObject = RuntimeUtility.CreateInstance(type);

			if (instancedObject != null)
			{
				instatedValue = Serialize(instancedObject);
			}

			// Fields
			List<SchemaField>? fields = null;
			if (!scalarTypes.Contains(type))
			{
				fields = new List<SchemaField>();

				if (jsonSerializerOptions.IncludeFields)
				{
					var fieldBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
					var typeFields = type.GetFields(fieldBindingFlags);
					foreach (var field in typeFields)
					{
						if (field.GetCustomAttribute<JsonIgnoreAttribute>() != null)
						{
							continue;
						}

						if (field.IsPrivate)
						{
							continue;
						}

						if (field.IsInitOnly && jsonSerializerOptions.IgnoreReadOnlyFields)
						{
							continue;
						}

						fields.Add(ConstructFieldInformation(type, field, instancedObject));
					}
				}

				var propertyBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
				var typeProperties = type.GetProperties(propertyBindingFlags);
				foreach (var property in typeProperties)
				{
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

					if (getter == null || getter.IsPrivate)
					{
						continue;
					}
					if (setter == null && jsonSerializerOptions.IgnoreReadOnlyProperties)
					{
						continue;
					}

					fields.Add(ConstructFieldInformation(type, property, instancedObject));
				}
			}

			return new SchemaType
			{
				Name = RuntimeUtility.ToIdentifier(type),
				Fields = fields,
				InstatedValue = instatedValue
			};
		}

		private static readonly Type[] scalarTypes = new[]
		{
			typeof(string),
			typeof(char),
			typeof(bool),
			typeof(int),
			typeof(byte),
			typeof(long),
			typeof(short),
			typeof(uint),
			typeof(ulong),
			typeof(ushort),
			typeof(sbyte),
			typeof(float),
			typeof(double),
			typeof(decimal),
		};

		private static readonly Type[] serializeWithToString = new[]
		{
			typeof(int),
			typeof(byte),
			typeof(long),
			typeof(short),
			typeof(uint),
			typeof(ulong),
			typeof(ushort),
			typeof(sbyte),
			typeof(float),
			typeof(double),
			typeof(decimal),
		};

		private string? Serialize(object instancedObject)
		{
			var type = instancedObject.GetType();

			if (type == typeof(string)
				|| type == typeof(char))
			{
				return $"\"{instancedObject}\"";
			}
			else if (type == typeof(bool))
			{
				return (bool)instancedObject ? "true" : "false";
			}
			else if (serializeWithToString.Contains(type))
			{
				return instancedObject.ToString();
			}
			else
			{
				return System.Text.Json.JsonSerializer.Serialize(instancedObject, jsonSerializerOptions);
			}
		}

		private SchemaField ConstructFieldInformation(Type containerType, FieldInfo field, object? instancedObject)
		{
			return ConstructFieldInformation(containerType, new ModelField(field), instancedObject);
		}

		private SchemaField ConstructFieldInformation(Type containerType, PropertyInfo property, object? instancedObject)
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

			public bool IsNullable => RuntimeUtility.IsNullable(Field);

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

			public bool IsNullable => RuntimeUtility.IsNullable(Property);

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

		private SchemaField ConstructFieldInformation(Type containerType, IModelMember member, object? instancedObject)
		{
			var typeName = RuntimeUtility.DescribeType(member.ValueType);

			if (!typeName.IsNullable)
			{
				typeName.IsNullable = member.IsNullable;
			}

			string? serializedInstatedValue = null;
			if (instancedObject == null)
			{
				instancedObject = RuntimeUtility.CreateInstance(containerType);
			}
			if (instancedObject != null)
			{
				object? fieldValue = member.GetValue(instancedObject);

				if (fieldValue == null)
				{
					fieldValue = RuntimeUtility.CreateInstance(member.ValueType);
				}

				if (fieldValue != null)
				{
					serializedInstatedValue = Serialize(fieldValue);
				}
			}

			string name = member.Name;
			string? description = null;
			foreach (object attribute in member.GetCustomAttributes(true))
			{
				if (attribute is JsonPropertyNameAttribute nameAttribute)
				{
					name = nameAttribute.Name;
				}
				else if (attribute is DescriptionAttribute descriptionAttribute)
				{
					description = descriptionAttribute.Description;
				}
			}

			return new SchemaField(name, description, typeName, serializedInstatedValue);
		}
	}
}
