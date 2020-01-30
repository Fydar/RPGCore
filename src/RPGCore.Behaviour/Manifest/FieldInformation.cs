using Newtonsoft.Json.Linq;
using System.Collections;
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

		public static FieldInformation ConstructFieldInformation(FieldInfo field, object defaultInstance)
		{
			object[] attributes = field.GetCustomAttributes(false);
			string[] attributeIds = new string[attributes.Length];
			for (int i = 0; i < attributes.Length; i++)
			{
				attributeIds[i] = attributes.GetType().Name;
			}

			FieldInformation fieldInformation;
			if (typeof(InputSocket).IsAssignableFrom(field.FieldType))
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
					defaultValue = field.GetValue(defaultInstance);
				}

				string typeName = field.FieldType.Name;

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

				if (typeof(IDictionary).IsAssignableFrom(field.FieldType))
				{
					fieldInformation.Format = FieldFormat.Dictionary;
					fieldInformation.Type = field.FieldType.GetGenericArguments()[1].Name;

					fieldInformation.ValueFormat = new FieldInformation()
					{
						Type = field.FieldType.GetGenericArguments()[1].Name,
						Format = FieldFormat.Object
					};
				}
				else if (field.FieldType.IsArray)
				{
					var elementType = field.FieldType.GetElementType();

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
