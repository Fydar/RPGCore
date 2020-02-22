using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RPGCore.Packages
{
	public sealed class PackageExplorer : IPackageExplorer
	{
		public sealed class PackageResourceCollection : IResourceCollection
		{
			private Dictionary<string, IResource> items;

			public IResource this[string key] => items[key];

			public void Add(IResource asset)
			{
				if (items == null)
				{
					items = new Dictionary<string, IResource>();
				}

				items.Add(asset.FullName, asset);
			}

			public IEnumerator<IResource> GetEnumerator()
			{
				return items.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return items.Values.GetEnumerator();
			}
		}

		private readonly ProjectDefinitionFile bProj;
		private string path;

		public string Name => bProj.Properties.Name;
		public string Version => bProj.Properties.Version;

		public PackageResourceCollection Resources { get; }
		public IReadOnlyDictionary<string, IReadOnlyList<string>> Tags { get; private set; }

		IResourceCollection IPackageExplorer.Resources => Resources;

		public PackageExplorer()
		{
			Resources = new PackageResourceCollection();
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
				path = path
			};
			using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true))
				{
					var entry = archive.GetEntry("Main.bmft");

					byte[] buffer = new byte[entry.Length];
					using (var zipStream = entry.Open())
					{
						zipStream.Read(buffer, 0, (int)entry.Length);
						string json = Encoding.UTF8.GetString(buffer);
					}

					package.Tags = LoadJsonDocument<IReadOnlyDictionary<string, IReadOnlyList<string>>>(entry);

					foreach (var projectEntry in archive.Entries)
					{
						var resource = new PackageResource(package, projectEntry);
						package.Resources.Add(resource);
					}
				}
				fileStream.Close();
			}

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
