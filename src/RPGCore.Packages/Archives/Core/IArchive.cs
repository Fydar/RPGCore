namespace RPGCore.Packages.Archives
{
	public interface IArchive : IReadOnlyArchive
	{
		int MaximumWriteThreads { get; }
		new IArchiveDirectory RootDirectory { get; }
	}
}
