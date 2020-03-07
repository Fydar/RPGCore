using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour.Manifest
{
	public class TypeInformation
	{
		public string Description;
		public Dictionary<string, TypeConversion> ImplicitConversions;

		public Dictionary<string, FieldInformation> Fields;
		public JToken DefaultValue;

		public static TypeInformation Construct(Type type)
		{
			var information = new TypeInformation();
			ConstructTypeInformation(type, information);
			return information;
		}

		protected static void ConstructTypeInformation(Type type, TypeInformation typeInformation)
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
			try
			{
				defaultInstance = Activator.CreateInstance(type);
			}
			catch
			{
			}

			// Default Value
			if (defaultInstance != null)
			{
				typeInformation.DefaultValue = JToken.FromObject(defaultInstance);
			}

			// Fields
			if (typeInformation.DefaultValue != null &&
				typeInformation.DefaultValue.Type == JTokenType.Object)
			{
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

					if (field.IsPrivate)
					{
						continue;
					}

					fieldInfos.Add(field.Name, FieldInformation.ConstructFieldInformation(field, defaultInstance));
				}
				foreach (var field in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					if (field.PropertyType == typeof(OutputSocket))
					{
						continue;
					}

					if (field.GetCustomAttribute<JsonIgnoreAttribute>() != null)
					{
						continue;
					}

					var getter = field.GetGetMethod();
					var setter = field.GetSetMethod();

					if (getter == null
						|| getter.IsPrivate
						|| setter == null)
					{
						continue;
					}

					fieldInfos.Add(field.Name, FieldInformation.ConstructFieldInformation(field, defaultInstance));
				}
				typeInformation.Fields = fieldInfos;
			}
		}
	}
}
