using System;

namespace RPGCore.Behaviour
{
	public class ConnectionInformationAttribute : Attribute
	{
		public string Name = "";
		public string Description = "";
		public Type Type;

		public ConnectionInformationAttribute (string name)
		{
			Name = name;
		}

		public ConnectionInformationAttribute (string name, string description)
		{
			Name = name;
			Description = description;
		}
	}
}