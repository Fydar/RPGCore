using System;
using System.Diagnostics;
using System.IO;

namespace RPGCore.FileTree.FileSystem
{
	public class FileSystemArchive : IArchive
	{
		public DirectoryInfo RootDirectoryInfo { get; }
		public FileSystemArchiveDirectory RootDirectory { get; }
		public int MaximumWriteThreads => 6;

		public event Action<ArchiveEventParameters> OnEntryChanged;

		private readonly object watcherLock = new object();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchive.RootDirectory => RootDirectory;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchive.RootDirectory => RootDirectory;

		public FileSystemArchive(DirectoryInfo rootDirectory, bool fireEvents = true)
		{
			rootDirectory.Create();

			RootDirectoryInfo = rootDirectory;
			RootDirectory = new FileSystemArchiveDirectory(this, null, rootDirectory);

			if (fireEvents)
			{
				var fileSystemWatcher = new FileSystemWatcher(rootDirectory.FullName)
				{
					IncludeSubdirectories = true
				};

				fileSystemWatcher.Created += FileWatcherEventHandler;
				fileSystemWatcher.Deleted += FileWatcherEventHandler;
				fileSystemWatcher.Changed += FileWatcherEventHandler;
				fileSystemWatcher.Renamed += FileWatcherRenamedEventHandlers;

				fileSystemWatcher.EnableRaisingEvents = true;
			}
		}

		private void FileWatcherEventHandler(object sender, FileSystemEventArgs args)
		{
			lock (watcherLock)
			{
				if (args.ChangeType.HasFlag(WatcherChangeTypes.Created))
				{
					var attr = File.GetAttributes(args.FullPath);

					var parentDirectory = ParentDirectoryForEntry(args.FullPath);

					if (attr.HasFlag(FileAttributes.Directory))
					{
						var newDirectory = new FileSystemArchiveDirectory(this, parentDirectory, new DirectoryInfo(args.FullPath));
						parentDirectory.Directories.internalList.Add(newDirectory);

						OnEntryChanged?.Invoke(new ArchiveEventParameters()
						{
							ActionType = ArchiveActionType.Created,
							Entry = newDirectory,
						});
					}
					else
					{
						var newFile = new FileSystemArchiveFile(this, parentDirectory, new FileInfo(args.FullPath));
						parentDirectory.Files.internalList.Add(newFile);

						OnEntryChanged?.Invoke(new ArchiveEventParameters()
						{
							ActionType = ArchiveActionType.Created,
							Entry = newFile,
						});
					}
				}

				if (args.ChangeType.HasFlag(WatcherChangeTypes.Changed))
				{
					if (TryGetFileFromFullPath(args.FullPath, out var file))
					{
						OnEntryChanged?.Invoke(new ArchiveEventParameters()
						{
							ActionType = ArchiveActionType.Changed,
							Entry = file,
						});
					}
				}

				if (args.ChangeType.HasFlag(WatcherChangeTypes.Deleted))
				{
					if (TryGetFileFromFullPath(args.FullPath, out var file))
					{
						file.Parent.Files.internalList.Remove(file);

						OnEntryChanged?.Invoke(new ArchiveEventParameters()
						{
							ActionType = ArchiveActionType.Deleted,
							Entry = file,
						});
					}
					else if (TryGetDirectoryFromFullPath(args.FullPath, out var directory))
					{
						directory.Parent.Directories.internalList.Remove(directory);

						OnEntryChanged?.Invoke(new ArchiveEventParameters()
						{
							ActionType = ArchiveActionType.Deleted,
							Entry = directory,
						});
					}
				}
			}
		}

		private void FileWatcherRenamedEventHandlers(object sender, RenamedEventArgs args)
		{
			lock (watcherLock)
			{
				var attr = File.GetAttributes(args.FullPath);
				if (attr.HasFlag(FileAttributes.Directory))
				{
					if (TryGetDirectoryFromFullPath(args.OldFullPath, out var directory))
					{
						var newParentDirectory = ParentDirectoryForEntry(args.FullPath);
						var newDirectoryInfo = new DirectoryInfo(args.FullPath);

						directory.Parent.Directories.internalList.Remove(directory);
						newParentDirectory.Directories.internalList.Add(directory);

						directory.Name = newDirectoryInfo.Name;

						OnEntryChanged?.Invoke(new ArchiveEventParameters()
						{
							ActionType = ArchiveActionType.Changed,
							Entry = directory,
						});
					}
				}
				else
				{
					if (TryGetFileFromFullPath(args.OldFullPath, out var file))
					{
						var newParentDirectory = ParentDirectoryForEntry(args.FullPath);
						var newFileInfo = new FileInfo(args.FullPath);

						file.Parent.Files.internalList.Remove(file);
						newParentDirectory.Files.internalList.Add(file);

						file.MoveAndRename(file.Parent, newFileInfo);

						OnEntryChanged?.Invoke(new ArchiveEventParameters()
						{
							ActionType = ArchiveActionType.Changed,
							Entry = file,
						});
					}
				}
			}
		}

		private bool TryGetFileFromFullPath(string fullPath, out FileSystemArchiveFile value)
		{
			string relativePath = fullPath.Substring(RootDirectoryInfo.FullName.Length + 1);
			string[] elements = relativePath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

			var parentDirectory = RootDirectory;
			for (int i = 0; i < elements.Length - 1; i++)
			{
				parentDirectory = parentDirectory.Directories.GetDirectory(elements[i]);
			}

			foreach (var file in parentDirectory.Files)
			{
				if (file.Name == elements[elements.Length - 1])
				{
					value = file;
					return true;
				}
			}
			value = null;
			return false;
		}

		private bool TryGetDirectoryFromFullPath(string fullPath, out FileSystemArchiveDirectory value)
		{
			string relativePath = fullPath.Substring(RootDirectoryInfo.FullName.Length + 1);
			string[] elements = relativePath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

			var parentDirectory = RootDirectory;

			for (int i = 0; i < elements.Length; i++)
			{
				parentDirectory = parentDirectory.Directories.GetDirectory(elements[i]);
			}

			if (parentDirectory != null)
			{
				value = parentDirectory;
				return true;
			}
			else
			{
				value = null;
				return false;
			}
		}

		private FileSystemArchiveDirectory ParentDirectoryForEntry(string fullPath)
		{
			string relativePath = fullPath.Substring(RootDirectoryInfo.FullName.Length + 1);
			string[] elements = relativePath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

			var parentDirectory = RootDirectory;
			for (int i = 0; i < elements.Length - 1; i++)
			{
				parentDirectory = parentDirectory.Directories.GetDirectory(elements[i]);
			}
			return parentDirectory;
		}
	}
}
