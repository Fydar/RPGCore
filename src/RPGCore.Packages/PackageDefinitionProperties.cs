using Newtonsoft.Json;

namespace RPGCore.Packages
{
	public class PackageDefinitionProperties : IDefinitionProperties
	{
		public string Name { get; internal set; }

		public string Version { get; internal set; }

		public PackageDefinitionProperties()
		{
		}

		[JsonConstructor]
		public PackageDefinitionProperties(string name, string version)
		{
			Name = name;
			Version = version;
		}
	}
}
