using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour.Manifest
{
	public sealed class FieldInformation
	{
		public string Description;
		public string Type;
		public JToken DefaultValue;
		public string[] Attributes;
		public FieldFormat Format;
		public FieldInformation ValueFormat;

		public abstract class ModelMember
		{
			public abstract Type ValueType { get; }
			public abstract object[] GetCustomAttributes(bool inherit);
			public abstract object GetValue(object instance);
		}

		public class ModelField : ModelMember
		{
			public FieldInfo Field { get; }

			public override Type ValueType => Field.FieldType;

			public ModelField(FieldInfo field)
			{
				Field = field;
			}

			public override object[] GetCustomAttributes(bool inherit)
			{
				return Field.GetCustomAttributes(inherit);
			}

			public override object GetValue(object instance)
			{
				return Field.GetValue(instance);
			}
		}

		public class ModelProperty : ModelMember
		{
			public PropertyInfo Property { get; }

			public override Type ValueType => Property.PropertyType;

			public ModelProperty(PropertyInfo property)
			{
				Property = property;
			}

			public override object[] GetCustomAttributes(bool inherit)
			{
				return Property.GetCustomAttributes(inherit);
			}

			public override object GetValue(object instance)
			{
				return Property.GetValue(instance);
			}
		}

		public static FieldInformation ConstructFieldInformation(FieldInfo field, object defaultInstance)
		{
			return ConstructFieldInformation(new ModelField(field), defaultInstance);
		}

		public static FieldInformation ConstructFieldInformation(PropertyInfo property, object defaultInstance)
		{
			return ConstructFieldInformation(new ModelProperty(property), defaultInstance);
		}

		public static FieldInformation ConstructFieldInformation(ModelMember member, object defaultInstance)
		{
			object[] attributes = member.GetCustomAttributes(false);
			string[] attributeIds = new string[attributes.Length];
			for (int i = 0; i < attributes.Length; i++)
			{
				attributeIds[i] = attributes.GetType().Name;
			}

			FieldInformation fieldInformation;
			if (typeof(InputSocket).IsAssignableFrom(member.ValueType))
			{
				fieldInformation = new FieldInformation()
				{
					Type = "InputSocket",
					Attributes = attributeIds,
					DefaultValue = new JValue((object)null),
					Format = FieldFormat.Object,
				};
			}
			else
			{
				object defaultValue = null;

				if (defaultInstance != null)
				{
					defaultValue = member.GetValue(defaultInstance);
				}

				string typeName = member.ValueType.Name;

				try
				{
					fieldInformation = new FieldInformation
					{
						Type = typeName,
						Attributes = attributeIds,
						DefaultValue = new JValue(defaultValue),
						Format = FieldFormat.Object,
					};
				}
				catch
				{
					fieldInformation = new FieldInformation
					{
						Type = typeName,
						Attributes = attributeIds,
						DefaultValue = JObject.FromObject(defaultValue),
						Format = FieldFormat.Object,
					};
				}

				if (typeof(IDictionary).IsAssignableFrom(member.ValueType))
				{
					fieldInformation.Format = FieldFormat.Dictionary;
					fieldInformation.Type = member.ValueType.GetGenericArguments()[1].Name;

					fieldInformation.ValueFormat = new FieldInformation()
					{
						Type = member.ValueType.GetGenericArguments()[1].Name,
						Format = FieldFormat.Object
					};
				}
				else if (member.ValueType.IsArray)
				{
					var elementType = member.ValueType.GetElementType();

					fieldInformation.Format = FieldFormat.List;
					fieldInformation.Type = elementType.Name;

					fieldInformation.ValueFormat = new FieldInformation()
					{
						Type = elementType.Name,
						Format = FieldFormat.Object
					};
				}
				else if (member.ValueType.IsGenericType
					&& !member.ValueType.IsGenericTypeDefinition
					&& member.ValueType.GetGenericTypeDefinition() == typeof(List<>))
				{
					var elementType = member.ValueType.GenericTypeArguments[0];

					fieldInformation.Format = FieldFormat.List;
					fieldInformation.Type = elementType.Name;

					fieldInformation.ValueFormat = new FieldInformation()
					{
						Type = elementType.Name,
						Format = FieldFormat.Object
					};
				}
				else
				{
					fieldInformation.Format = FieldFormat.Object;
				}
			}

			return fieldInformation;
		}
	}
}
