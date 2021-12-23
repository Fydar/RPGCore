using System.Diagnostics;
using System.Threading.Tasks;

namespace RPGCore.FileTree.Packed
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(PackedArchiveDirectoryDebugView))]
	public class PackedArchiveDirectory : IArchiveDirectory
	{
		/// <inheritdoc/>
		public string Name { get; }

		/// <inheritdoc/>
		public string FullName { get; }

		public PackedArchive Archive { get; }
		public PackedArchiveDirectory? Parent { get; }
		public PackedArchiveDirectoryCollection Directories { get; }
		public PackedArchiveFileCollection Files { get; }

		private int Count
		{
			get
			{
				return Directories.internalList.Count + Files.internalList.Count;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectoryCollection IArchiveDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveFileCollection IArchiveDirectory.Files => Files;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory? IArchiveEntry.Parent => Parent;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchive IArchiveEntry.Archive => Archive;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectoryCollection IReadOnlyArchiveDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveFileCollection IReadOnlyArchiveDirectory.Files => Files;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchiveEntry.Parent => Parent;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchive IReadOnlyArchiveEntry.Archive => Archive;

		internal PackedArchiveDirectory(PackedArchive archive, PackedArchiveDirectory? parent, string name)
		{
			Archive = archive;
			Parent = parent;

			Directories = new PackedArchiveDirectoryCollection(archive, this);
			Files = new PackedArchiveFileCollection(archive, this);

			if (parent != null)
			{
				Name = name;
				FullName = MakeFullName(parent, name);
			}
		}

		/// <inheritdoc/>
		public Task CopyIntoAsync(IArchiveDirectory destination, string name)
		{
			static void CopyIntoRecursive(PackedArchiveDirectory from, IArchiveDirectory to, string rename)
			{
				foreach (var file in from.Files)
				{
					var destFile = to.Files.GetFile(file.Name);

					using var openStream = file.OpenRead();
					var dest = file.OpenWrite();
					openStream.CopyTo(dest);
				}
				foreach (var directory in from.Directories.All)
				{
					var destDirectory = to.Directories.GetDirectory(directory.Name);

					CopyIntoRecursive(directory, destDirectory, directory.Name);
				}
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc/>
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
			return FullName ?? "";
		}

		private class PackedArchiveDirectoryDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}")]
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
			private readonly PackedArchiveDirectory source;

			public PackedArchiveDirectoryDebugView(PackedArchiveDirectory source)
			{
				this.source = source;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerRow[] Keys
			{
				get
				{
					bool hasParent = source.Parent != null;

					int count = source.Directories.internalList.Count
						+ source.Files.internalList.Count
						+ (hasParent ? 1 : 0);

					var keys = new DebuggerRow[count];

					int i = 0;

					if (hasParent)
					{
						keys[i] = new DebuggerRow("..", source.Parent);
						i++;
					}
					foreach (var directory in source.Directories.All)
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
