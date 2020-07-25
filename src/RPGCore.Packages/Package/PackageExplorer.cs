using Newtonsoft.Json;
using RPGCore.Packages.Archives;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	/// <summary>
	/// <para>Used for loading the content of a compiled package.</para>
	/// </summary>
	/// <remarks>
	/// <para>Can be used to analyse and modify the content of a project.</para>
	/// </remarks>
	public sealed class PackageExplorer : IExplorer
	{
		public IReadOnlyArchive Archive { get; private set; }

		/// <summary>
		/// <para>The project definition for this package.</para>
		/// </summary>
		public PackageDefinition Definition { get; private set; }

		/// <summary>
		/// <para>A collection of all of the resources contained in this package.</para>
		/// </summary>
		public PackageResourceCollection Resources { get; }

		/// <summary>
		/// <para>An index of the tags contained within this package for performing asset queries.</para>
		/// </summary>
		public ITagsCollection Tags { get; private set; }

		/// <summary>
		/// <para>A directory representing the root of the package.</para>
		/// </summary>
		public IDirectory RootDirectory { get; private set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDefinition IExplorer.Definition => Definition;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceCollection IExplorer.Resources => Resources;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] ITagsCollection IExplorer.Tags => Tags;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectory IExplorer.RootDirectory => RootDirectory;

		private PackageExplorer()
		{
			Resources = new PackageResourceCollection();
		}

		public static Task<PackageExplorer> LoadFromFileAsync(string file)
		{
			var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
			var za = new ZipArchive(fs, ZipArchiveMode.Read, true);
			var archive = new PackedArchive(za);

			return LoadAsync(archive);
		}

		public static Task<PackageExplorer> LoadFromDirectoryAsync(string directory)
		{
			var archive = new FileSystemArchive(new DirectoryInfo(directory));

			return LoadAsync(archive);
		}

		public static async Task<PackageExplorer> LoadAsync(IReadOnlyArchive archive)
		{
			var definitionEntry = archive.Files.GetFile("definition.json");
			var definitionDocument = LoadJsonDocument<PackageDefinition>(definitionEntry);

			var tagsEntry = archive.Files.GetFile("tags.json");
			var tagsDocument = LoadJsonDocument<IReadOnlyDictionary<string, IReadOnlyList<string>>>(tagsEntry);
			var tags = new Dictionary<string, IResourceCollection>();

			var rootDirectiory = new PackageDirectory("", "", null);
			PackageDirectory ForPath(string path)
			{
				int currentIndex = 5;
				var currentDirectory = rootDirectiory;
				while (true)
				{
					int nextIndex = path.IndexOf('/', currentIndex);
					if (nextIndex == -1)
					{
						break;
					}
					string segment = path.Substring(currentIndex, nextIndex - currentIndex);

					bool found = false;
					foreach (var directory in currentDirectory.Directories)
					{
						if (directory.Name == segment)
						{
							currentDirectory = directory;
							found = true;
							break;
						}
					}
					if (!found)
					{
						var newDirectory = new PackageDirectory(segment, path.Substring(5, nextIndex - 5), currentDirectory);
						currentDirectory.Directories.Add(newDirectory);
						currentDirectory = newDirectory;
					}

					currentIndex = nextIndex + 1;
				}

				return currentDirectory;
			}

			var package = new PackageExplorer
			{
				Archive = archive,
				RootDirectory = rootDirectiory,
				Definition = definitionDocument
			};

			foreach (var metadataEntry in archive.Files)
			{
				/*
				if (!packageEntry.FullName.StartsWith("data/")
					|| packageEntry.FullName.EndsWith(".pkgmeta"))
				{
					continue;
				}
				var metadataEntry = archive.Files.GetFile($"{packageEntry.FullName}.pkgmeta");
				*/

				if (!metadataEntry.FullName.EndsWith(".pkgmeta"))
				{
					continue;
				}
				var contentEntry = archive.Files.GetFile(metadataEntry.FullName.Substring(0, metadataEntry.FullName.Length - 8));

				var metadataModel = LoadJsonDocument<PackageResourceMetadataModel>(metadataEntry);

				var forPath = ForPath(contentEntry.FullName);

				var resource = new PackageResource(package, forPath, contentEntry, metadataModel);

				package.Resources.Add(resource.FullName, resource);
				forPath.Resources.Add(resource.Name, resource);

				foreach (var tagCategory in tagsDocument)
				{
					if (tagCategory.Value.Contains(resource.FullName))
					{
						if (!tags.TryGetValue(tagCategory.Key, out var taggedResourcesCollection))
						{
							taggedResourcesCollection = new PackageResourceCollection();
							tags[tagCategory.Key] = taggedResourcesCollection;
						}

						var taggedResources = (PackageResourceCollection)taggedResourcesCollection;

						taggedResources.Add(resource.FullName, resource);
						resource.Tags.tags.Add(tagCategory.Key);
					}
				}
			}

			package.Tags = new PackageTagsCollection(tags);

			return package;
		}

		public void Dispose()
		{
		}

		internal Stream LoadStream(string packageKey)
		{
			return Archive.Files.GetFile(packageKey).OpenRead();
		}

		internal byte[] OpenAsset(string packageKey)
		{
			var entry = Archive.Files.GetFile(packageKey);

			byte[] buffer = new byte[entry.UncompressedSize];
			using var zipStream = entry.OpenRead();
			zipStream.Read(buffer, 0, (int)entry.UncompressedSize);
			return buffer;
		}

		private static T LoadJsonDocument<T>(IReadOnlyArchiveEntry entry)
		{
			using var zipStream = entry.OpenRead();
			using var sr = new StreamReader(zipStream);
			using var reader = new JsonTextReader(sr);

			var serializer = new JsonSerializer();
			return serializer.Deserialize<T>(reader);
		}
	}
}
