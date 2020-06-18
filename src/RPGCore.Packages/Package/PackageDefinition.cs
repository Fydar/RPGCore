using Newtonsoft.Json;
using System.Diagnostics;

namespace RPGCore.Packages
{
	public class PackageDefinition : IDefinition
	{
		public PackageDefinitionProperties Properties { get; set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDefinitionProperties IDefinition.Properties => Properties;

		[JsonConstructor]
		internal PackageDefinition(PackageDefinitionProperties properties)
		{
			Properties = properties;
		}
	}
}
