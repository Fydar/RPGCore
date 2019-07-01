using Newtonsoft.Json.Linq;
using System.Reflection;

namespace RPGCore.Behaviour.Manifest
{
	public class FieldInformation
	{
		public string Name;
		public string Description;
		public string Type;
		public JToken DefaultValue;
		public string[] Attributes;

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
					Name = field.Name,
					Attributes = attributeIds,
					DefaultValue = new JValue ((object)null)
				};
			}
			else
			{
				object defaultValue = field.GetValue (defaultInstance);

				try
				{
					fieldInformation = new FieldInformation
					{
						Type = field.FieldType.Name,
						Name = field.Name,
						Attributes = attributeIds,
						DefaultValue = new JValue (defaultValue)
					};
				}
				catch
				{
					fieldInformation = new FieldInformation
					{
						Type = field.FieldType.Name,
						Name = field.Name,
						Attributes = attributeIds,
						DefaultValue = JObject.FromObject (defaultValue)
					};
				}
			}


			return fieldInformation;
		}
	}
}
