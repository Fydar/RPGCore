
using System.Reflection;

namespace Behaviour.Manifest
{
	public struct FieldInformation
	{
		public string Name;
		public string Description;
		public string Type;

		public static FieldInformation Construct(FieldInfo field)
		{
			var fieldInformation = new FieldInformation
			{
				Type = field.FieldType,
				Name = field.Name
			};

			return fieldInformation;
		}
	}
}
