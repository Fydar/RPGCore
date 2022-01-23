using RPGCore.FileTree;
using RPGCore.Projects.Pipeline;

namespace RPGCore.Projects;

public class ArchiveFileImporterContext
{
	public ProjectExplorer Explorer { get; internal set; }
	public IArchiveFile Source { get; internal set; }

	public ProjectResourceUpdate AuthorUpdate(string name)
	{
		return new ProjectResourceUpdate(Explorer, name);
	}
}
