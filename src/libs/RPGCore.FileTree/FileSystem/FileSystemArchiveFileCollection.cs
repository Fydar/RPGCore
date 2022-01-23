using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RPGCore.FileTree.FileSystem;

[DebuggerDisplay("Count = {Count,nq}")]
public class FileSystemArchiveFileCollection : IArchiveFileCollection
{
	private readonly FileSystemArchive archive;
	private readonly FileSystemArchiveDirectory owner;

	private readonly List<FileSystemArchiveFile> internalList;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly SemaphoreSlim synchronize;

	internal int Count
	{
		get
		{
			return internalList.Count;
		}
	}

	internal FileSystemArchiveFileCollection(FileSystemArchive archive, FileSystemArchiveDirectory owner)
	{
		this.archive = archive;
		this.owner = owner;

		internalList = new List<FileSystemArchiveFile>();
		synchronize = new SemaphoreSlim(1, 1);

		foreach (var fileInfo in owner.directoryInfo
			.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
		{
			internalList.Add(new FileSystemArchiveFile(archive, owner, fileInfo));
		}
	}

	/// <inheritdoc/>
	public FileSystemArchiveFile GetFile(string key)
	{
		synchronize.Wait();
		var result = GetFileInternal(key);
		synchronize.Release();

		return result;
	}

	/// <inheritdoc/>
	public FileSystemArchiveFile GetOrCreateFile(string key)
	{
		synchronize.Wait();
		var file = GetFileInternal(key);

		if (file == null)
		{
			var info = new FileInfo(Path.Combine(owner.directoryInfo.FullName, key));
			// info.Create();
			file = new FileSystemArchiveFile(archive, owner, info);
			internalList.Add(file);
		}
		synchronize.Release();

		return file;
	}

	internal void UntrackFileInternal(string key)
	{
		synchronize.Wait();
		var file = GetFileInternal(key);
		internalList.Remove(file);
		synchronize.Release();
	}

	internal void UntrackFileInternal(FileSystemArchiveFile file)
	{
		synchronize.Wait();
		internalList.Remove(file);
		synchronize.Release();
	}

	internal void TrackFileInternal(FileSystemArchiveFile file)
	{
		synchronize.Wait();
		internalList.Add(file);
		synchronize.Release();
	}

	private FileSystemArchiveFile GetFileInternal(string key)
	{
		for (int i = 0; i < internalList.Count; i++)
		{
			var file = internalList[i];
			if (file.Name == key)
			{
				return file;
			}
		}
		return null;
	}

	public IEnumerator<FileSystemArchiveFile> GetEnumerator()
	{
		return internalList.GetEnumerator();
	}

	IEnumerator<IReadOnlyArchiveFile> IEnumerable<IReadOnlyArchiveFile>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator<IArchiveFile> IArchiveFileCollection.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	IReadOnlyArchiveFile IReadOnlyArchiveFileCollection.GetFile(string key)
	{
		return GetFile(key);
	}

	IArchiveFile IArchiveFileCollection.GetFile(string key)
	{
		return GetFile(key);
	}

	IArchiveFile IArchiveFileCollection.GetOrCreateFile(string key)
	{
		return GetOrCreateFile(key);
	}
}
