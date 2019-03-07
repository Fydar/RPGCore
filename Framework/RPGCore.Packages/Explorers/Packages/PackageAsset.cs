using System;
using System.IO.Compression;

namespace RPGCore.Packages
{
	public struct PackageAsset
	{
		public PackageExplorer Package;
		public readonly string Root;
		public readonly PackageResource[] Resources;

		public PackageAsset (PackageExplorer package, string root, ZipArchiveEntry[] entries)
		{
			Package = package;
			Root = root;

			Resources = new PackageResource[entries.Length];
			for (int i = 0; i < entries.Length; i++)
			{
				Resources[i] = new PackageResource (package, entries[i]);
			}
		}

		public override string ToString ()
		{
			return Root;
		}

		public PackageResource GetResource(string path)
		{
			foreach (var resource in Resources)
			{
				if (resource.Name.Substring(Root.Length) == path)
				{
					return resource;
				}
			}
			return default(PackageResource);
		}
	}
}
