using Newtonsoft.Json;
using System.Diagnostics;

namespace RPGCore.Packages
{
	public class PackageDefinition : IDefinition
	{
		public PackageDefinitionProperties Properties { get; set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDefinitionProperties IDefinition.Properties => Properties;

		[JsonConstructor]
		public PackageDefinition(PackageDefinitionProperties properties)
		{
			Properties = properties;
		}

		public override string ToString()
		{
			return $"{Properties.Name}, Version={Properties.Version}";
		}
	}
}
