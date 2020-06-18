using System.Collections.Generic;
using System.IO;

namespace RPGCore.Packages.Pipeline
{
	/// <summary>
	/// The target of the project resource importation pipeline.
	/// </summary>
	public class ProjectResourceImporter
	{
		public ProjectExplorer ProjectExplorer { get; }
		public FileInfo FileInfo { get; }
		public string ProjectKey { get; }

		public HashSet<string> ImporterTags { get; }

		internal ProjectResourceImporter(ProjectExplorer projectExplorer, FileInfo fileInfo, string projectKey)
		{
			ProjectExplorer = projectExplorer;
			FileInfo = fileInfo;
			ProjectKey = projectKey;

			ImporterTags = new HashSet<string>();
		}

		public ProjectResource Import()
		{
			return new ProjectResource(this);
		}
	}
}
