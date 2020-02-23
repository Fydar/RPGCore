using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace RPGCore.Packages
{
	public sealed class PackageExplorer : IExplorer
	{
		[DebuggerDisplay("Count = {Count,nq}")]
		[DebuggerTypeProxy(typeof(PackageResourceCollectionDebugView))]
		private sealed class PackageResourceCollection : IPackageResourceCollection, IResourceCollection
		{
			private Dictionary<string, PackageResource> Resources;

			public int Count => Resources?.Count ?? 0;

			public PackageResource this[string key] => Resources[key];
			IResource IResourceCollection.this[string key] => this[key];

			internal void Add(PackageResource asset)
			{
				if (Resources == null)
				{
					Resources = new Dictionary<string, PackageResource>();
				}

				Resources.Add(asset.FullName, asset);
			}

			public IEnumerator<PackageResource> GetEnumerator()
			{
				return Resources?.Values == null
					? Enumerable.Empty<PackageResource>().GetEnumerator()
					: Resources.Values.GetEnumerator();
			}

			IEnumerator<IResource> IEnumerable<IResource>.GetEnumerator()
			{
				return GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			private class PackageResourceCollectionDebugView
			{
				[DebuggerDisplay("{Value}", Name = "{Key}")]
				internal struct DebuggerRow
				{
					public string Key;

					[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
					public IResource Value;
				}

				private readonly PackageResourceCollection Source;

				public PackageResourceCollectionDebugView(PackageResourceCollection source)
				{
					Source = source;
				}

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public DebuggerRow[] Keys
				{
					get
					{
						var keys = new DebuggerRow[Source.Resources.Count];

						int i = 0;
						foreach (var kvp in Source.Resources)
						{
							keys[i] = new DebuggerRow
							{
								Key = kvp.Key,
								Value = kvp.Value
							};
							i++;
						}
						return keys;
					}
				}
			}
		}

		private readonly ProjectDefinitionFile Definition;
		private string path;

		public string Name => Definition?.Properties?.Name;
		public string Version => Definition?.Properties?.Version;

		public IPackageResourceCollection Resources => ResourcesInternal;
		public ITagsCollection Tags => TagsInternal;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceCollection IExplorer.Resources => ResourcesInternal;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] ITagsCollection IExplorer.Tags => TagsInternal;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private PackageResourceCollection ResourcesInternal;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private PackageTagsCollection TagsInternal;

		public PackageExplorer()
		{
		}

		public void Dispose()
		{

		}

		public Stream LoadStream(string packageKey)
		{
			var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true);

			var entry = archive.GetEntry(packageKey);

			var zipStream = entry.Open();

			return new PackageStream(zipStream, fileStream, archive);
		}

		public byte[] OpenAsset(string packageKey)
		{
			using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			using var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true);

			var entry = archive.GetEntry(packageKey);

			byte[] buffer = new byte[entry.Length];
			using var zipStream = entry.Open();
			zipStream.Read(buffer, 0, (int)entry.Length);
			return buffer;
		}

		public static PackageExplorer Load(string path)
		{
			var package = new PackageExplorer
			{
				path = path,
				ResourcesInternal = new PackageResourceCollection()
			};

			var tags = new Dictionary<string, IResourceCollection>();

			using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true))
			{
				var entry = archive.GetEntry("Main.bmft");

				byte[] buffer = new byte[entry.Length];
				using (var zipStream = entry.Open())
				{
					zipStream.Read(buffer, 0, (int)entry.Length);
					string json = Encoding.UTF8.GetString(buffer);
				}

				var tagsEntry = archive.GetEntry("tags.json");
				var tagsDocument = LoadJsonDocument<IReadOnlyDictionary<string, IReadOnlyList<string>>>(tagsEntry);

				foreach (var projectEntry in archive.Entries)
				{
					var resource = new PackageResource(package, projectEntry, tagsDocument);
					package.ResourcesInternal.Add(resource);

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
						}
					}
				}
			}

			package.TagsInternal = new PackageTagsCollection(tags);

			return package;
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
