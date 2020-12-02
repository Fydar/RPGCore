using System.Collections.Generic;

namespace RPGCore.Projects.Extensions.MetaFiles
{
	public class JsonMetaFileModel
	{
		public string[] Tags { get; set; }
		public Dictionary<string, string> Metadata { get; set; }
	}
}
