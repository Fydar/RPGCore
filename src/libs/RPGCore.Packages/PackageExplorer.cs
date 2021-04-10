using Newtonsoft.Json;
using RPGCore.FileTree;
using RPGCore.FileTree.FileSystem;
using RPGCore.FileTree.Packed;
using System;
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
	/// <para>Cannot be used to modify the content of the package.</para>
	/// </remarks>
	public sealed class PackageExplorer : IExplorer
	{
		/// <summary>
		/// <para>An archive directory that this package was loaded from.</para>
		/// </summary>
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
			await Task.CompletedTask;

			var definitionEntry = source.Files.GetFile("definition.json");
			var definitionDocument = LoadJsonDocument<PackageDefinition>(definitionEntry);

			var tagsEntry = source.Files.GetFile("tags.json");
			var tagsDocument = LoadJsonDocument<IReadOnlyDictionary<string, IReadOnlyList<string>>>(tagsEntry);
			var tags = new Dictionary<string, IResourceCollection>();

			var rootDirectiory = new PackageDirectory("", null);

			var packageExplorer = new PackageExplorer
			{
				Source = source,
				RootDirectory = rootDirectiory,
				Definition = definitionDocument
			};

			var resourcesDirectory = source.Directories.GetDirectory("resources");
			var contentsDirectory = source.Directories.GetDirectory("contents");

			// Does this package contain any resources?
			if (resourcesDirectory != null)
			{
				foreach (var resourceFile in resourcesDirectory.Files)
				{
					var metadataModel = LoadJsonDocument<PackageResourceMetadataModel>(resourceFile);

					// Directory
					string[] elements = metadataModel.FullName
						.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
					var parentDirectory = rootDirectiory;
					for (int i = 0; i < elements.Length - 1; i++)
					{
						string element = elements[i];

						PackageDirectory? findDirectory = null;
						foreach (var directory in parentDirectory.Directories)
						{
							if (directory.Name == element)
							{
								findDirectory = directory;
								break;
							}
						}
						if (findDirectory == null)
						{
							findDirectory = new PackageDirectory(element, parentDirectory);
							parentDirectory.Directories.Add(findDirectory);
						}
						parentDirectory = findDirectory;
					}

					// Resource
					var contentFile = contentsDirectory.Files.GetFile(metadataModel.ContentId);
					var resource = new PackageResource(packageExplorer, parentDirectory, contentFile, metadataModel);

					packageExplorer.Resources.Add(resource.FullName, resource);
					parentDirectory.Resources.Add(resource.Name, resource);

					// Tags
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

			packageExplorer.Tags = new PackageTagsCollection(tags);

			return packageExplorer;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
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
