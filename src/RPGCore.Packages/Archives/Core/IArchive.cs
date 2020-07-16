namespace RPGCore.Packages.Archives
{
	public interface IArchive : IReadOnlyArchive
	{
		new IArchiveEntryCollection Files { get; }
	}
}
