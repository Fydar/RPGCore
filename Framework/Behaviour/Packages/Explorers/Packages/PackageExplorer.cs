using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RPGCore.Behaviour.Packages
{
	public class PackageExplorer : IPackageExplorer
	{
		private class PackageFolderCollection : IPackageFolderCollection
		{
			private Dictionary<string, PackageFolder> items;

			public PackageFolder this[string key]
			{
				get
				{
					return items[key];
				}
			}

			public void Add(PackageFolder folder)
			{
				if (items == null)
					items = new Dictionary<string, PackageFolder> ();
				items.Add (folder.ToString (), folder);
			}

			public IEnumerator<PackageFolder> GetEnumerator ()
			{
				return items.Values.GetEnumerator ();
			}

			IEnumerator IEnumerable.GetEnumerator ()
			{
				return items.Values.GetEnumerator ();
			}
		}

		private BProjModel bProj;

		public string Name => bProj.Name;
		public string Version => bProj.Version;
		public PackageDependancy[] Dependancies => bProj.Dependancies;
		public IPackageFolderCollection Folders { get; private set; }

		public PackageExplorer ()
		{
			Folders = new PackageFolderCollection ();
		}

		public static PackageExplorer Load (string path)
		{
			var package = new PackageExplorer
			{
			};
			using (var fileStream = new FileStream (path, FileMode.Open))
			{
				using (var archive = new ZipArchive (fileStream, ZipArchiveMode.Read, true))
				{
					var entry = archive.GetEntry ("Main.bmft");

					byte[] buffer = new byte[entry.Length];
					using (var zipStream = entry.Open ())
					{
						zipStream.Read (buffer, 0, (int)entry.Length);
						string json = Encoding.UTF8.GetString (buffer);
					}

					string pathPrefix = null;
					var pathEntries = new List<ZipArchiveEntry> ();
					foreach (var projectEntry in archive.Entries)
					{
						int pathPrefixIndex = projectEntry.FullName.IndexOf('/');
						if (pathPrefixIndex == -1)
						{
							Console.WriteLine ("Not adding \"" + projectEntry.FullName + "\" as an item.");
							continue;
						}
						string newPathIndex = projectEntry.FullName.Substring (0, pathPrefixIndex);

						if (pathPrefix == null)
							pathPrefix = newPathIndex;

						if (pathPrefix != newPathIndex)
						{
							pathPrefix = newPathIndex;

							var folder = new PackageFolder (pathPrefix, pathEntries.ToArray ());
							package.Folders.Add (folder);

							pathEntries.Clear ();
						}
						pathEntries.Add (projectEntry);
					}
				}
			}

			return package;
		}
	}
}
