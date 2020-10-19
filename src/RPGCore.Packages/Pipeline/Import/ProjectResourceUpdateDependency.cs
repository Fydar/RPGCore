using System.Collections.Generic;

namespace RPGCore.Packages.Pipeline
{
	public class ProjectResourceUpdateDependency
	{
		public string Resource { get; set; }
		public DependencyFlags DependencyFlags { get; set; }
		public Dictionary<string, string> Metadata { get; set; }
	}
}
