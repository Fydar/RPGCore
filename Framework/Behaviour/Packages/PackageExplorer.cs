using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RPGCore.Behaviour.Packages
{
	public class PackageExplorer : IPackageExplorer
	{
		private struct PackageItemCollection : IPackageItemCollection
		{
			private PackageExplorer explorer;

			public PackageItem this[string key]
			{
				get
				{
					return explorer.Items[key];
				}
			}

			public PackageItemCollection (PackageExplorer explorer)
			{
				this.explorer = explorer;
			}
		}
		
		private readonly Dictionary<string, PackageItem> itemsDictionary;
		private BProjModel bProj;

		public string Name => bProj.Name;
		public string Version => bProj.Version;
		public PackageDependancy[] Dependancies => bProj.Dependancies;
		public IPackageItemCollection Items { get; private set; }

		public PackageExplorer ()
		{
			Items = new PackageItemCollection (this);
			itemsDictionary = new Dictionary<string, PackageItem> ();
		}

		public static PackageExplorer Load (string path)
		{
			using (var fileStream = new FileStream (path, FileMode.Open))
			{
				using (var archive = new ZipArchive (fileStream, ZipArchiveMode.Read, true))
				{
					var entry = archive.GetEntry ("Main.bmft");

					using (var zipStream = entry.Open ())
					{
						byte[] buffer = new byte[zipStream.Length];
						zipStream.Read (buffer, 0, (int)zipStream.Length);
						string json = Encoding.UTF8.GetString (buffer);
					}
				}
			}

			var package = new PackageExplorer
			{
			};
			return package;
		}
	}
}
