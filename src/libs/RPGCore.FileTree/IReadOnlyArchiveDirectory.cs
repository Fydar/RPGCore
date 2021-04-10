namespace RPGCore.FileTree
{
	public interface IReadOnlyArchiveDirectory : IReadOnlyArchiveEntry
	{
		IReadOnlyArchiveDirectoryCollection Directories { get; }
		IReadOnlyArchiveFileCollection Files { get; }
	}
}
