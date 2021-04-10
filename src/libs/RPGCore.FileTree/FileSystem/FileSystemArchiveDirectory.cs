using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RPGCore.FileTree.FileSystem
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(FileSystemArchiveDirectoryDebugView))]
	public class FileSystemArchiveDirectory : IArchiveDirectory
	{
		internal readonly DirectoryInfo directoryInfo;

		public string Name { get; internal set; }
		public string FullName { get; internal set; }
		public FileSystemArchive Archive { get; }
		public FileSystemArchiveDirectory Parent { get; internal set; }
		public FileSystemArchiveDirectoryCollection Directories { get; }
		public FileSystemArchiveFileCollection Files { get; }

		private int Count
		{
			get
			{
				return Directories.Count + Files.Count;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectoryCollection IArchiveDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveFileCollection IArchiveDirectory.Files => Files;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchiveEntry.Parent => Parent;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchive IArchiveEntry.Archive => Archive;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectoryCollection IReadOnlyArchiveDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveFileCollection IReadOnlyArchiveDirectory.Files => Files;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchiveEntry.Parent => Parent;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchive IReadOnlyArchiveEntry.Archive => Archive;

		internal FileSystemArchiveDirectory(FileSystemArchive archive, FileSystemArchiveDirectory parent, DirectoryInfo directoryInfo)
		{
			Archive = archive;
			Parent = parent;
			this.directoryInfo = directoryInfo;

			if (parent != null)
			{
				Name = directoryInfo.Name;
				FullName = MakeFullName(parent, directoryInfo.Name);
			}

			Directories = new FileSystemArchiveDirectoryCollection(archive, this);
			Files = new FileSystemArchiveFileCollection(archive, this);
		}

		public Task CopyIntoAsync(IArchiveDirectory destination, string name)
		{
			static void CopyIntoRecursive(FileSystemArchiveDirectory from, IArchiveDirectory to, string rename)
			{
				foreach (var fromFile in from.Files)
				{
					var toFile = to.Files.GetFile(fromFile.Name);

					if (toFile is FileSystemArchiveFile toFileSystemFile)
					{
						fromFile.FileInfo.CopyTo(toFileSystemFile.FileInfo.FullName, true);
					}
					else
					{
						using var readStream = fromFile.OpenRead();
						using var writeStream = toFile.OpenWrite();

						readStream.CopyTo(writeStream);
					}
				}
				foreach (var childFromDirectory in from.Directories)
				{
					var childToDirectory = to.Directories.GetDirectory(childFromDirectory.Name);

					CopyIntoRecursive(childFromDirectory, childToDirectory, childFromDirectory.Name);
				}
			}

			CopyIntoRecursive(this, destination, name);

			return Task.CompletedTask;
		}

		public Task MoveInto(IArchiveDirectory destination, string name)
		{
			throw new System.NotImplementedException();
		}

		private static string MakeFullName(IArchiveDirectory parent, string key)
		{
			if (parent == null || string.IsNullOrEmpty(parent.FullName))
			{
				return key;
			}
			else
			{
				return $"{parent.FullName}/{key}";
			}
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return FullName;
		}

		private class FileSystemArchiveDirectoryDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}",
				Target = typeof(IArchiveDirectory), TargetTypeName = nameof(IArchiveDirectory), Type = "Directory")]
			internal struct DebuggerRow
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public IArchiveEntry Value;

				public DebuggerRow(string key, IArchiveEntry value)
				{
					Key = key;
					Value = value;
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly FileSystemArchiveDirectory source;

			public FileSystemArchiveDirectoryDebugView(FileSystemArchiveDirectory source)
			{
				this.source = source;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerRow[] Keys
			{
				get
				{
					// bool hasParent = source.Parent != null;
					bool hasParent = false;

					int count = source.Directories.Count
						+ source.Files.Count
						+ (hasParent ? 1 : 0);

					var keys = new DebuggerRow[count];

					int i = 0;

					/*if (hasParent)
					{
						keys[i] = new DebuggerRow("..", source.Parent);
						i++;
					}*/
					foreach (var directory in source.Directories)
					{
						keys[i] = new DebuggerRow(directory.Name, directory);
						i++;
					}
					foreach (var file in source.Files)
					{
						keys[i] = new DebuggerRow(file.Name, file);
						i++;
					}
					return keys;
				}
			}
		}
	}
}
