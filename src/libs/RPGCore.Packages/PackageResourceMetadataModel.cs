using Newtonsoft.Json;
using System;

namespace RPGCore.Packages
{
	public class PackageResourceMetadataModel
	{
		public string Name { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public string ContentId { get; set; } = string.Empty;

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public PackageResourceMetadataDependencyModel[] Dependencies { get; set; } = Array.Empty<PackageResourceMetadataDependencyModel>();
	}
}
