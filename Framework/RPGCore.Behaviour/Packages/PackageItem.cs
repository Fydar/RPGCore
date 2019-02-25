using System.IO.Compression;

namespace RPGCore.Behaviour.Packages
{
	public struct PackageItem
	{
		private readonly ZipArchiveEntry zipArchiveEntry;

		public PackageItem (ZipArchiveEntry zipArchiveEntry)
		{
			this.zipArchiveEntry = zipArchiveEntry;
		}
	}
}
