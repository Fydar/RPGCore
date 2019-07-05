using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace RPGCore.Behaviour.Manifest
{
	public class JsonValueTypeInformation : TypeInformation
	{
		public FieldInformation[] Fields;
		public JValue DefaultValue;

		public static JsonValueTypeInformation Construct(Type type)
		{
			var typeInformation = new JsonValueTypeInformation();

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

			// Default Value
			object defaultInstance = null;
			try
			{
				defaultInstance = Activator.CreateInstance(type);
			}
			catch
			{
			}

			if (type == typeof(char))
			{
				typeInformation.DefaultValue = new JValue(default(char).ToString());
			}
			else
			{
				try
				{
					typeInformation.DefaultValue = new JValue(defaultInstance);
				}
				catch
				{
				}
			}

			return typeInformation;
		}
	}
}
