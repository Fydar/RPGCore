using System.IO.Compression;

namespace RPGCore.Behaviour.Packages
{
	public struct PackageAsset
	{
		private readonly ZipArchiveEntry zipArchiveEntry;

		public PackageAsset (ZipArchiveEntry zipArchiveEntry)
		{
			this.zipArchiveEntry = zipArchiveEntry;
		}

		public override string ToString ()
		{
			return zipArchiveEntry.Name;
		}
	}
}
