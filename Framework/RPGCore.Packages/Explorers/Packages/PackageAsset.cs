using System.IO.Compression;

namespace RPGCore.Packages
{
	public struct PackageAsset : IAsset
	{
		public PackageExplorer Package;
		public readonly string Root;
		public readonly PackageResource[] Resources;

		public string Name
		{
			get
			{
				return Root;
			}
		}

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

		public IResource GetResource (string path)
		{
			foreach (var resource in Resources)
			{
				if (resource.Name.Substring (Root.Length) == path)
				{
					return resource;
				}
			}
			return default (PackageResource);
		}

		public override string ToString ()
		{
			return Name;
		}
	}
}
