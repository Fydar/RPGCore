using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.DataEditor.Manifest
{
	public sealed class FieldInformation
	{
		public string Name;
		public string Description;
		public string Type;
		public JToken DefaultValue;
		public string[] Attributes;
		public FieldWrapper Wrapper;

		public abstract class ModelMember
		{
			public abstract string Name { get; }

			public abstract Type ValueType { get; }
			public abstract object[] GetCustomAttributes(bool inherit);
			public abstract object GetValue(object instance);
		}

		public class ModelField : ModelMember
		{
			public FieldInfo Field { get; }

			public override Type ValueType => Field.FieldType;

			public override string Name => Field.Name;

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

			public override string Name => Property.Name;

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
			// Attributes
			object[] attributes = member.GetCustomAttributes(false);
			string[] attributeIds = new string[attributes.Length];
			for (int i = 0; i < attributes.Length; i++)
			{
				attributeIds[i] = attributes.GetType().Name;
			}

			// Default Value
			object defaultValue = null;
			if (defaultInstance != null)
			{
				defaultValue = member.GetValue(defaultInstance);
			}
			JToken defaultValueToken;
			try
			{
				defaultValueToken = new JValue(defaultValue);
			}
			catch
			{
				//defaultValueToken = JObject.FromObject(defaultValue);
				defaultValueToken = JToken.FromObject(defaultValue);
			}

			// Wrappers and inner-most type
			FieldWrapper wrapper = null;
			FieldWrapper currentWrapper = null;
			var currentType = member.ValueType;

			void PushWrapper(FieldWrapperType fieldWrapperType)
			{
				var newWrapper = new FieldWrapper()
				{
					Type = fieldWrapperType
				};
				if (wrapper == null)
				{
					wrapper = newWrapper;
					currentWrapper = newWrapper;
				}
				else
				{
					currentWrapper.Child = newWrapper;
				}
			}
			while (true)
			{
				if (typeof(IDictionary).IsAssignableFrom(currentType))
				{
					currentType = currentType.GetGenericArguments()[1];

					PushWrapper(FieldWrapperType.Dictionary);
				}
				else if (currentType.IsArray)
				{
					currentType = currentType.GetElementType();

					PushWrapper(FieldWrapperType.List);
				}
				else if (currentType.IsGenericType
					&& !currentType.IsGenericTypeDefinition
					&& currentType.GetGenericTypeDefinition() == typeof(List<>))
				{
					currentType = currentType.GenericTypeArguments[0];

					PushWrapper(FieldWrapperType.List);
				}
				else
				{
					break;
				}
			}

			return new FieldInformation
			{
				Name = member.Name,
				Description = null,
				Type = currentType.Name,
				Attributes = attributeIds,
				DefaultValue = defaultValueToken,
				Wrapper = wrapper
			};
		}
	}
}
