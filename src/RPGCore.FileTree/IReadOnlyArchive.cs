namespace RPGCore.FileTree
{
	public interface IReadOnlyArchive
	{
		IReadOnlyArchiveDirectory RootDirectory { get; }
	}
}
