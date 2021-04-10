using System.Collections.Generic;

namespace RPGCore.Projects.Pipeline
{
	public class ProjectResourceUpdateDependency
	{
		public string Resource { get; set; }
		public DependencyFlags DependencyFlags { get; set; }
		public Dictionary<string, string> Metadata { get; set; }
	}
}
