using RPGCore.FileTree.Packed;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.FileTree.FileSystem
{
	public class FileSystemArchiveFile : IArchiveFile
	{
		public FileSystemArchive Archive { get; }
		public FileInfo FileInfo { get; internal set; }

		/// <inheritdoc/>
		public string Name { get; internal set; }

		/// <inheritdoc/>
		public string FullName { get; internal set; }

		/// <inheritdoc/>
		public string Extension { get; internal set; }

		/// <inheritdoc/>
		public long CompressedSize => FileInfo.Length;

		/// <inheritdoc/>
		public long UncompressedSize => FileInfo.Length;

		public FileSystemArchiveDirectory Parent { get; private set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchive IReadOnlyArchiveEntry.Archive => Archive;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchiveEntry.Parent => Parent;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchive IArchiveEntry.Archive => Archive;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchiveEntry.Parent => Parent;

		internal FileSystemArchiveFile(FileSystemArchive archive, FileSystemArchiveDirectory parent, FileInfo fileInfo)
		{
			Archive = archive;
			Parent = parent;
			FileInfo = fileInfo;

			Name = fileInfo.Name;
			FullName = MakeFullName(parent, fileInfo.Name);
			Extension = fileInfo.Extension;
		}

		internal void MoveAndRename(FileSystemArchiveDirectory parent, FileInfo fileInfo)
		{
			Parent = parent;
			Name = fileInfo.Name;
			FullName = MakeFullName(parent, fileInfo.Name);
			Extension = fileInfo.Extension;
		}

		Task IArchiveEntry.MoveInto(IArchiveDirectory destination, string name)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc/>
		public Task DeleteAsync()
		{
			return Task.Run(() => FileInfo.Delete());
		}

		/// <inheritdoc/>
		public Task RenameAsync(string destination)
		{
			return Task.Run(() => FileInfo.MoveTo(destination));
		}

		/// <inheritdoc/>
		public Stream OpenRead()
		{
			return FileInfo.OpenRead();
		}

		/// <inheritdoc/>
		public Stream OpenWrite()
		{
			FileInfo.Directory.Create();
			return FileInfo.Open(FileMode.Create, FileAccess.Write);
		}

		/// <inheritdoc/>
		public async Task CopyIntoAsync(IArchiveDirectory destination, string name)
		{
			var toFile = destination.Files.GetFile(Name);

			if (toFile is FileSystemArchiveFile toFileSystemFile)
			{
				FileInfo.CopyTo(toFileSystemFile.FileInfo.FullName, true);
			}
			else if (toFile is PackedArchiveFile toParckedFile)
			{
				toParckedFile.Archive.ZipArchive.CreateEntryFromFile(FileInfo.FullName, toFile.FullName);
			}
			else
			{
				using var readStream = OpenRead();
				using var writeStream = toFile.OpenWrite();

				await readStream.CopyToAsync(writeStream);
			}
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
	}
}
