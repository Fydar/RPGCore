namespace RPGCore.FileTree;

public interface IArchiveDirectory : IReadOnlyArchiveDirectory, IArchiveEntry
{
	new IArchiveDirectoryCollection Directories { get; }
	new IArchiveFileCollection Files { get; }
}
