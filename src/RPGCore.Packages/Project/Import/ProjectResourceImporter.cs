using RPGCore.Packages.Archives;
using System.Collections.Generic;

namespace RPGCore.Packages.Pipeline
{
	/// <summary>
	/// The target of the project resource importation pipeline.
	/// </summary>
	public class ProjectResourceImporter
	{
		public ProjectExplorer ProjectExplorer { get; }
		public ProjectDirectory Directory { get; }
		public IArchiveFile ArchiveEntry { get; }
		public string ProjectKey { get; }

		public HashSet<string> ImporterTags { get; }
		public ProjectResourceImporterDependencyCollection Dependencies { get; }

		internal ProjectResourceImporter(ProjectExplorer projectExplorer, ProjectDirectory directory, IArchiveFile archiveEntry, string projectKey)
		{
			ProjectExplorer = projectExplorer;
			Directory = directory;
			ArchiveEntry = archiveEntry;
			ProjectKey = projectKey;

			ImporterTags = new HashSet<string>();
			Dependencies = new ProjectResourceImporterDependencyCollection(this);
		}

		public ProjectResource Import()
		{
			return new ProjectResource(Directory, this);
		}
	}
}
