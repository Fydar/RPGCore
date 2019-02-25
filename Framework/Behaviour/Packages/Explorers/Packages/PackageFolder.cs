using System.IO.Compression;

namespace RPGCore.Behaviour.Packages
{
	public struct PackageFolder
	{
		public readonly string Root;
		public readonly PackageAsset[] Assets;

		public PackageFolder (string root, ZipArchiveEntry[] entries)
		{
			Root = root;

			Assets = new PackageAsset[entries.Length];
			for (int i = 0; i < entries.Length; i++)
			{
				Assets[i] = new PackageAsset (entries[i]);
			}
		}

		public override string ToString ()
		{
			return Root;
		}
	}
}
