﻿using System.IO;

namespace RPGCore.FileTree;

public interface IReadOnlyArchiveFile : IReadOnlyArchiveEntry
{
	string Extension { get; }
	long UncompressedSize { get; }

	Stream OpenRead();
}
