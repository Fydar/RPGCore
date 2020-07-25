using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RPGCore.DataEditor.Manifest
{
	public class TypeInformation
	{
		public string Description;
		public Dictionary<string, TypeConversion> ImplicitConversions;
		public Dictionary<string, FieldInformation> Fields;
		public JToken DefaultValue;

		internal static TypeInformation Construct(Type type)
		{
			var information = new TypeInformation();
			PopulateTypeInformation(type, information);
			return information;
		}

		protected static void PopulateTypeInformation(Type type, TypeInformation typeInformation)
		{
			// Implicit Conversions
			var conversions = new Dictionary<string, TypeConversion>();
			var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			foreach (var method in methods)
			{
				if (method.Name == "op_implicit")
				{
					conversions.Add(method.ReturnType.FullName, TypeConversion.Construct(method));
				}
			}
			typeInformation.ImplicitConversions = conversions;

			// Instancing
			object defaultInstance = null;

			if (type.IsValueType)
			{
				try
				{
					defaultInstance = Activator.CreateInstance(type);
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
						defaultInstance = Activator.CreateInstance(type);
					}
					catch
					{
					}
				}
			}

			// Default Value
			if (defaultInstance != null)
			{
				typeInformation.DefaultValue = JToken.FromObject(defaultInstance);
			}
			else
			{
				typeInformation.DefaultValue = JValue.CreateNull();
			}

			// Fields
			// if (typeInformation.DefaultValue.Type == JTokenType.Object)
			// {
			var fieldInfos = new Dictionary<string, FieldInformation>();
			foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (field.FieldType == typeof(OutputSocket))
				{
					continue;
				}

				if (field.GetCustomAttribute<JsonIgnoreAttribute>() != null)
				{
					continue;
				}

				if (field.IsPrivate
					|| field.Name.StartsWith("m_"))
				{
					continue;
				}

				fieldInfos.Add(field.Name, FieldInformation.ConstructFieldInformation(field, defaultInstance));
			}
			foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (property.PropertyType == typeof(OutputSocket))
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
					|| setter == null
					|| property.Name.StartsWith("m_"))
				{
					continue;
				}

				fieldInfos.Add(property.Name, FieldInformation.ConstructFieldInformation(property, defaultInstance));
			}
			typeInformation.Fields = fieldInfos;
		}
	}
}
