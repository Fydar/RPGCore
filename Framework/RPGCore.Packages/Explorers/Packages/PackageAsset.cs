using System;
using System.IO.Compression;

namespace RPGCore.Packages
{
	public struct PackageAsset
	{
		public PackageExplorer Package;
		public readonly string Root;
		public readonly PackageResource[] Assets;

		public PackageAsset (PackageExplorer package, string root, ZipArchiveEntry[] entries)
		{
			Package = package;
			Root = root;

			Assets = new PackageResource[entries.Length];
			for (int i = 0; i < entries.Length; i++)
			{
				Assets[i] = new PackageResource (package, entries[i]);
			}
		}

		public override string ToString ()
		{
			return Root;
		}

		public PackageResource GetResource(string path)
		{
			foreach (var asset in Assets)
			{
				Console.WriteLine(asset.Name + " - " + Root);
				if (asset.Name.Substring(Root.Length) == path)
				{
					return asset;
				}
			}
			return default(PackageResource);
		}
	}
}
