using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RPGCore.FileTree.FileSystem
{
	public class FileSystemArchive : IArchive
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly SemaphoreSlim synchronize;

		public DirectoryInfo RootDirectoryInfo { get; }
		public FileSystemArchiveDirectory RootDirectory { get; }
		public int MaximumWriteThreads => 6;

		public event Action<ArchiveEventParameters> OnEntryChanged;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchive.RootDirectory => RootDirectory;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchive.RootDirectory => RootDirectory;

		public FileSystemArchive(DirectoryInfo rootDirectory, bool fireEvents = true)
		{
			synchronize = new SemaphoreSlim(1, 1);

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
			synchronize.Wait();

			if (args.ChangeType.HasFlag(WatcherChangeTypes.Created))
			{
				StartTrackingFile(args.FullPath);
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
				else
				{
					StartTrackingFile(args.FullPath);
				}
			}

			if (args.ChangeType.HasFlag(WatcherChangeTypes.Deleted))
			{
				if (TryGetFileFromFullPath(args.FullPath, out var file))
				{
					file.Parent.Files.UntrackFileInternal(file);

					OnEntryChanged?.Invoke(new ArchiveEventParameters()
					{
						ActionType = ArchiveActionType.Deleted,
						Entry = file,
					});
				}
				else if (TryGetDirectoryFromFullPath(args.FullPath, out var directory))
				{
					directory.Parent.Directories.UntrackDirectoryInternal(directory);

					OnEntryChanged?.Invoke(new ArchiveEventParameters()
					{
						ActionType = ArchiveActionType.Deleted,
						Entry = directory,
					});
				}
			}

			synchronize.Release();
		}

		private void StartTrackingFile(string fullPath)
		{
			var attr = File.GetAttributes(fullPath);

			var parentDirectory = ParentDirectoryForEntry(fullPath);

			if (attr.HasFlag(FileAttributes.Directory))
			{
				var newDirectory = new FileSystemArchiveDirectory(this, parentDirectory, new DirectoryInfo(fullPath));
				parentDirectory.Directories.TrackDirectoryInternal(newDirectory);

				OnEntryChanged?.Invoke(new ArchiveEventParameters()
				{
					ActionType = ArchiveActionType.Created,
					Entry = newDirectory,
				});
			}
			else
			{
				var newFile = new FileSystemArchiveFile(this, parentDirectory, new FileInfo(fullPath));
				parentDirectory.Files.TrackFileInternal(newFile);

				OnEntryChanged?.Invoke(new ArchiveEventParameters()
				{
					ActionType = ArchiveActionType.Created,
					Entry = newFile,
				});
			}
		}

		private void FileWatcherRenamedEventHandlers(object sender, RenamedEventArgs args)
		{
			synchronize.Wait();

			var attr = File.GetAttributes(args.FullPath);
			if (attr.HasFlag(FileAttributes.Directory))
			{
				if (TryGetDirectoryFromFullPath(args.OldFullPath, out var directory))
				{
					var newParentDirectory = ParentDirectoryForEntry(args.FullPath);
					var newDirectoryInfo = new DirectoryInfo(args.FullPath);

					directory.Parent.Directories.UntrackDirectoryInternal(directory);
					newParentDirectory.Directories.TrackDirectoryInternal(directory);

					directory.Name = newDirectoryInfo.Name;

					OnEntryChanged?.Invoke(new ArchiveEventParameters()
					{
						ActionType = ArchiveActionType.Changed,
						Entry = directory,
					});
				}
				else
				{

				}
			}
			else
			{
				if (TryGetFileFromFullPath(args.OldFullPath, out var file))
				{
					var newParentDirectory = ParentDirectoryForEntry(args.FullPath);
					var newFileInfo = new FileInfo(args.FullPath);

					file.Parent.Files.UntrackFileInternal(file);
					newParentDirectory.Files.TrackFileInternal(file);

					file.MoveAndRename(file.Parent, newFileInfo);

					OnEntryChanged?.Invoke(new ArchiveEventParameters()
					{
						ActionType = ArchiveActionType.Changed,
						Entry = file,
					});
				}
				else
				{
					StartTrackingFile(args.FullPath);
				}
			}

			synchronize.Release();
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
