using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

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


			// Fields
			object defaultInstance = null;
			try
			{
				defaultInstance = Activator.CreateInstance(type);
			}
			catch
			{
			}

			var fieldInfos = new Dictionary<string, FieldInformation>();
			foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (field.FieldType == typeof(OutputSocket))
				{
					continue;
				}

				fieldInfos.Add(field.Name, FieldInformation.Construct(field, defaultInstance));
			}
			typeInformation.Fields = fieldInfos;

			// Default Value
			if (defaultInstance != null)
			{
				typeInformation.DefaultValue = JToken.FromObject(defaultInstance);
			}
		}
	}
}
