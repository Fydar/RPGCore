using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour.Manifest
{
	public class JsonObjectTypeInformation : TypeInformation
	{
		public Dictionary<string, FieldInformation> Fields;

		public static JsonObjectTypeInformation Construct(Type type)
		{
			var typeInformation = new JsonObjectTypeInformation();

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

			return typeInformation;
		}
	}
}
