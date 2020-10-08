using System.Diagnostics;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public class PackedArchiveDirectory : IArchiveDirectory
	{
		public string Name { get; }
		public string FullName { get; }
		public PackedArchive Archive { get; }
		public PackedArchiveDirectory Parent { get; }
		public PackedArchiveDirectoryCollection Directories { get; }
		public PackedArchiveFileCollection Files { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectoryCollection IArchiveDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveFileCollection IArchiveDirectory.Files => Files;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchiveEntry.Parent => Parent;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchive IArchiveEntry.Archive => Archive;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectoryCollection IReadOnlyArchiveDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveFileCollection IReadOnlyArchiveDirectory.Files => Files;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchiveEntry.Parent => Parent;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchive IReadOnlyArchiveEntry.Archive => Archive;

		internal PackedArchiveDirectory(PackedArchive archive, PackedArchiveDirectory parent, string name)
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

		public Task CopyInto(IArchiveDirectory destination, string name)
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
				foreach (var directory in from.Directories)
				{
					var destDirectory = to.Directories.GetDirectory(directory.Name);

					CopyIntoRecursive(directory, destDirectory, directory.Name);
				}
			}

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
	}
}
