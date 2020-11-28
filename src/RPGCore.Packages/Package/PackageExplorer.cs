using Newtonsoft.Json;
using RPGCore.FileTree;
using RPGCore.FileTree.FileSystem;
using RPGCore.FileTree.Packed;
using RPGCore.FileTree;
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
		public IReadOnlyArchiveDirectory Source { get; private set; }

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

			return LoadAsync(archive.RootDirectory);
		}

		public static Task<PackageExplorer> LoadFromDirectoryAsync(string directory)
		{
			var archive = new FileSystemArchive(new DirectoryInfo(directory));

			return LoadAsync(archive.RootDirectory);
		}

		public static async Task<PackageExplorer> LoadAsync(IReadOnlyArchiveDirectory source)
		{
			var definitionEntry = source.Files.GetFile("definition.json");
			var definitionDocument = LoadJsonDocument<PackageDefinition>(definitionEntry);

			var tagsEntry = source.Files.GetFile("tags.json");
			var tagsDocument = LoadJsonDocument<IReadOnlyDictionary<string, IReadOnlyList<string>>>(tagsEntry);
			var tags = new Dictionary<string, IResourceCollection>();

			var rootDirectiory = new PackageDirectory("", "", null);

			var packageExplorer = new PackageExplorer
			{
				Source = source,
				RootDirectory = rootDirectiory,
				Definition = definitionDocument
			};

			void ImportDirectory(IReadOnlyArchiveDirectory directory, PackageDirectory packageDirectory)
			{
				foreach (var childDirectory in directory.Directories)
				{
					ImportDirectory(childDirectory,
						new PackageDirectory(childDirectory.Name, childDirectory.FullName, packageDirectory));
				}

				foreach (var file in directory.Files)
				{
					if (!file.FullName.EndsWith(".pkgmeta"))
					{
						continue;
					}
					var contentEntry = directory.Files.GetFile(file.Name.Substring(0, file.Name.Length - 8));

					var metadataModel = LoadJsonDocument<PackageResourceMetadataModel>(file);

					var resource = new PackageResource(packageExplorer, packageDirectory, contentEntry, metadataModel);

					packageExplorer.Resources.Add(resource.FullName, resource);
					packageDirectory.Resources.Add(resource.Name, resource);

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
			}

			ImportDirectory(source, rootDirectiory);

			packageExplorer.Tags = new PackageTagsCollection(tags);

			return packageExplorer;
		}

		public void Dispose()
		{
		}

		internal Stream LoadStream(string packageKey)
		{
			return Source.Files.GetFile(packageKey).OpenRead();
		}

		internal byte[] OpenAsset(string packageKey)
		{
			var entry = Source.Files.GetFile(packageKey);

			byte[] buffer = new byte[entry.UncompressedSize];
			using var zipStream = entry.OpenRead();
			zipStream.Read(buffer, 0, (int)entry.UncompressedSize);
			return buffer;
		}

		private static T LoadJsonDocument<T>(IReadOnlyArchiveFile entry)
		{
			using var zipStream = entry.OpenRead();
			using var sr = new StreamReader(zipStream);
			using var reader = new JsonTextReader(sr);

			var serializer = new JsonSerializer();
			return serializer.Deserialize<T>(reader);
		}
	}
}
