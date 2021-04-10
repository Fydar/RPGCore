namespace RPGCore.FileTree
{
	public class ArchiveEventParameters
	{
		public ArchiveActionType ActionType { get; internal set; }
		public IArchiveEntry Entry { get; internal set; }
	}
}
