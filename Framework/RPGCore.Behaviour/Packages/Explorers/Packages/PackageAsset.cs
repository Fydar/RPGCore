using System.IO.Compression;

namespace RPGCore.Behaviour.Packages
{
	public struct PackageAsset
	{
		public readonly string Root;
		public readonly PackageResource[] Assets;

		public PackageAsset (string root, ZipArchiveEntry[] entries)
		{
			Root = root;

			Assets = new PackageResource[entries.Length];
			for (int i = 0; i < entries.Length; i++)
			{
				Assets[i] = new PackageResource (entries[i]);
			}
		}

		public override string ToString ()
		{
			return Root;
		}
	}
}
