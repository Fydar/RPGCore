using RPGCore.FileTree;
using System.Collections.Generic;

namespace RPGCore.Packages.Pipeline
{
	/// <summary>
	/// The target of the project resource importation pipeline.
	/// </summary>
	public class ProjectResourceUpdate
	{
		public ProjectExplorer Explorer { get; }
		public string ProjectKey { get; }

		public HashSet<string> ImporterTags { get; }
		public ProjectResourceUpdateDependencyCollection Dependencies { get; }

		internal IArchiveFile FileContent { get; private set; }
		internal IContentWriter DeferredContent { get; private set; }

		internal ProjectResourceUpdate(ProjectExplorer explorer, string projectKey)
		{
			Explorer = explorer;
			ProjectKey = projectKey;

			ImporterTags = new HashSet<string>();
			Dependencies = new ProjectResourceUpdateDependencyCollection(this);
		}

		public ProjectResourceUpdate WithContent(IArchiveFile content)
		{
			FileContent = content;
			return this;
		}

		public ProjectResourceUpdate WithContent(IContentWriter content)
		{
			DeferredContent = content;
			return this;
		}
	}
}
