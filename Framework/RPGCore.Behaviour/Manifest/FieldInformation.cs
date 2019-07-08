using System.Collections;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace RPGCore.Behaviour.Manifest
{
	public class FieldInformation
	{
		public string Description;
		public string Type;
		public JToken DefaultValue;
		public string[] Attributes;

		public static FieldInformation Construct(FieldInfo field, object defaultInstance)
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
					DefaultValue = new JValue((object)null)
				};
			}
			else
			{
				object defaultValue = null;
				
				if (defaultInstance != null)
				{
					field.GetValue(defaultInstance);
				}

				string typeName;
				if (typeof(IDictionary).IsAssignableFrom(field.FieldType))
				{
					typeName = $"Dictionary of {field.FieldType.GetGenericArguments()[1].Name}";
				}
				else
				{
					typeName = field.FieldType.Name;
				}

				try
				{
					fieldInformation = new FieldInformation
					{
						Type = typeName,
						Attributes = attributeIds,
						DefaultValue = new JValue(defaultValue)
					};
				}
				catch
				{
					fieldInformation = new FieldInformation
					{
						Type = typeName,
						Attributes = attributeIds,
						DefaultValue = JObject.FromObject(defaultValue)
					};
				}
			}


			return fieldInformation;
		}
	}
}
