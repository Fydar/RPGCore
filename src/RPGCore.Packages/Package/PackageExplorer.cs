using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

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
		/// <summary>
		/// <para>The path of the package on disk.</para>
		/// </summary>
		public string PackagePath { get; private set; }

		/// <summary>
		/// <para>The size of the package on disk.</para>
		/// </summary>
		public long CompressedSize { get; private set; }

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

		public static PackageExplorer Load(string packagePath)
		{
			var packageFileInfo = new FileInfo(packagePath);

			using var fileStream = new FileStream(packagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			using var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true);


			var definitionEntry = archive.GetEntry("definition.json");
			var definitionDocument = LoadJsonDocument<PackageDefinition>(definitionEntry);

			var tagsEntry = archive.GetEntry("tags.json");
			var tagsDocument = LoadJsonDocument<IReadOnlyDictionary<string, IReadOnlyList<string>>>(tagsEntry);
			var tags = new Dictionary<string, IResourceCollection>();

			var rootDirectiory = new PackageDirectory();

			var package = new PackageExplorer
			{
				PackagePath = packagePath,
				CompressedSize = packageFileInfo.Length,
				RootDirectory = rootDirectiory,
				Definition = definitionDocument
			};

			foreach (var packageEntry in archive.Entries)
			{
				if (!packageEntry.FullName.StartsWith("data/")
					|| packageEntry.FullName.EndsWith(".pkgmeta"))
				{
					continue;
				}

				var metadataEntry = archive.GetEntry($"{packageEntry.FullName}.pkgmeta");
				var metadataModel = LoadJsonDocument<PackageResourceMetadataModel>(metadataEntry);

				var resource = new PackageResource(package, packageEntry, metadataModel);
				package.Resources.Add(resource);

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

						taggedResources.Add(resource);
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
			var fileStream = new FileStream(PackagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true);
			var entry = archive.GetEntry(packageKey);

			var zipStream = entry.Open();

			return new PackageStream(fileStream, archive, zipStream);
		}

		internal byte[] OpenAsset(string packageKey)
		{
			using var fileStream = new FileStream(PackagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			using var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true);
			var entry = archive.GetEntry(packageKey);

			byte[] buffer = new byte[entry.Length];
			using var zipStream = entry.Open();
			zipStream.Read(buffer, 0, (int)entry.Length);
			return buffer;
		}

		private static T LoadJsonDocument<T>(ZipArchiveEntry entry)
		{
			using var zipStream = entry.Open();
			using var sr = new StreamReader(zipStream);
			using var reader = new JsonTextReader(sr);

			var serializer = new JsonSerializer();
			return serializer.Deserialize<T>(reader);
		}
	}
}
