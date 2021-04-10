using RPGCore.FileTree;
using RPGCore.Projects.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Projects
{
	public sealed class DefaultArchiveDirectoryImporter : IArchiveDirectoryImporter
	{
		public bool CanImport(IArchiveDirectory archiveDirectory)
		{
			return true;
		}

		public IEnumerable<ProjectResourceUpdate> ImportDirectory(ArchiveDirectoryImporterContext context, IArchiveDirectory archiveDirectory)
		{
			foreach (var directory in archiveDirectory.Directories.All)
			{
				if (directory.Name == "bin"
					|| directory.Name == "temp")
				{
					continue;
				}

				foreach (var resource in context.Explorer.ImportPipeline.ImportDirectory(context.Explorer, directory))
				{
					yield return resource;
				}
			}

			foreach (var file in archiveDirectory.Files)
			{
				if (file.Extension == ".bproj")
				{
					continue;
				}

				foreach (var resource in context.Explorer.ImportPipeline.ImportFile(context.Explorer, file))
				{
					yield return resource;
				}
			}
		}
	}
}
