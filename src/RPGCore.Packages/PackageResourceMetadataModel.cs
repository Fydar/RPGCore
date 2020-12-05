using Newtonsoft.Json;

namespace RPGCore.Packages
{
	public class PackageResourceMetadataModel
	{
		public string Name { get; set; }
		public string FullName { get; set; }
		public string ContentId { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public PackageResourceMetadataDependencyModel[] Dependencies { get; set; }
	}
}
