using Newtonsoft.Json;

namespace RPGCore.Packages
{
	public class PackageDefinitionProperties : IDefinitionProperties
	{
		public string Name { get; internal set; }

		public string Version { get; internal set; }

		internal PackageDefinitionProperties()
		{
		}

		[JsonConstructor]
		internal PackageDefinitionProperties(string name, string version)
		{
			Name = name;
			Version = version;
		}
	}
}
