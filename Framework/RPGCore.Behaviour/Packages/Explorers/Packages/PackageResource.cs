using System.IO.Compression;

namespace RPGCore.Behaviour.Packages
{
	public struct PackageResource
	{
		private readonly ZipArchiveEntry zipArchiveEntry;

		public PackageResource (ZipArchiveEntry zipArchiveEntry)
		{
			this.zipArchiveEntry = zipArchiveEntry;
		}

		public override string ToString ()
		{
			return zipArchiveEntry.Name;
		}
	}
}
