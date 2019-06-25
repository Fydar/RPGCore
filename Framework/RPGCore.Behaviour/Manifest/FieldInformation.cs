using System.Reflection;
using Newtonsoft.Json.Linq;

namespace RPGCore.Behaviour.Manifest
{
	public class FieldInformation
	{
		public string Name;
		public string Description;
		public string Type;
		public JValue DefaultValue;
		public string[] Attributes;

		public static FieldInformation Construct (FieldInfo field, object defaultInstance)
		{
			object[] attributes = field.GetCustomAttributes (false);
			string[] attributeIds = new string[attributes.Length];
			for (int i = 0; i < attributes.Length; i++)
			{
				attributeIds[i] = attributes.GetType ().Name;
			}

			var defaultValue = field.GetValue(defaultInstance);

			var fieldInformation = new FieldInformation
			{
				Type = field.FieldType.Name,
				Name = field.Name,
				Attributes = attributeIds,
				DefaultValue = new JValue(defaultValue)
			};

			return fieldInformation;
		}
	}
}
