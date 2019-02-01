
using System.Reflection;

namespace Behaviour.Manifest
{
	public struct FieldInformation
	{
		public string Name;
		public string Description;
		public string Type;
		public string[] Attributes;

		public static FieldInformation Construct(FieldInfo field)
		{
			object[] attributes = field.GetCustomAttributes(false);
			string[] attributeIds = new string[attributes.Length];
			for (int i = 0; i < attributes.Length; i++)
			{
				attributeIds[i] = attributes.GetType().Name;
			}

			var fieldInformation = new FieldInformation
			{
				Type = field.FieldType.Name,
				Name = field.Name,
				Attributes = attributeIds
			};

			return fieldInformation;
		}
	}
}
