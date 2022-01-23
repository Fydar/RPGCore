using System;

namespace RPGCore.FileTree;

public interface IArchive : IReadOnlyArchive
{
	int MaximumWriteThreads { get; }
	new IArchiveDirectory RootDirectory { get; }
	event Action<ArchiveEventParameters> OnEntryChanged;
}
