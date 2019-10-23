using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;

namespace RPGCore.Behaviour.Manifest
{
	public enum FieldFormat
	{
		None,
		Object,
		Dictionary,
		List
	}

	public sealed class FieldInformation
	{
		public string Description;
		public string Type;
		public JToken DefaultValue;
		public string[] Attributes;
		public FieldFormat Format;
		public FieldInformation ValueFormat;

		public static FieldInformation Construct (FieldInfo field, object defaultInstance)
		{
			object[] attributes = field.GetCustomAttributes (false);
			string[] attributeIds = new string[attributes.Length];
			for (int i = 0; i < attributes.Length; i++)
			{
				attributeIds[i] = attributes.GetType ().Name;
			}

			FieldInformation fieldInformation;
			if (typeof (InputSocket).IsAssignableFrom (field.FieldType))
			{
				fieldInformation = new FieldInformation ()
				{
					Type = "InputSocket",
					Attributes = attributeIds,
					DefaultValue = new JValue ((object)null)
				};
			}
			else
			{
				object defaultValue = null;

				if (defaultInstance != null)
				{
					field.GetValue (defaultInstance);
				}

				string typeName = field.FieldType.Name;

				try
				{
					fieldInformation = new FieldInformation
					{
						Type = typeName,
						Attributes = attributeIds,
						DefaultValue = new JValue (defaultValue)
					};
				}
				catch
				{
					fieldInformation = new FieldInformation
					{
						Type = typeName,
						Attributes = attributeIds,
						DefaultValue = JObject.FromObject (defaultValue)
					};
				}

				if (typeof (IDictionary).IsAssignableFrom (field.FieldType))
				{
					fieldInformation.Format = FieldFormat.Dictionary;
					fieldInformation.Type = field.FieldType.GetGenericArguments ()[1].Name;

					fieldInformation.ValueFormat = new FieldInformation ()
					{
						Type = field.FieldType.GetGenericArguments ()[1].Name,
						Format = FieldFormat.Object
					};
				}
				else if (field.FieldType.IsArray)
				{
					fieldInformation.Format = FieldFormat.List;
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
