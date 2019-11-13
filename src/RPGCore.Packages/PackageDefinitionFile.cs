using System.Collections.Generic;

namespace RPGCore.Packages
{
	public class PackageDefinitionFile
	{
		public string Name;
		public string Description;

		public Dictionary<string, ResourceDependancyDefinition> ResourceDependancies;

		public PackageDefinitionFile()
		{
			ResourceDependancies = new Dictionary<string, ResourceDependancyDefinition> ();
		}
	}
}
