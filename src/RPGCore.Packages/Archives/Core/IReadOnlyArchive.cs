namespace RPGCore.Packages.Archives
{
	public interface IReadOnlyArchive
	{
		IReadOnlyArchiveDirectory RootDirectory { get; }
	}
}
