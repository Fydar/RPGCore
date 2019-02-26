using System.IO;
using System.IO.Compression;

namespace RPGCore.Behaviour.Packages
{
	public struct PackageResource
	{
		private readonly ZipArchiveEntry zipArchiveEntry;

		public string Name
		{
			get
			{
				return zipArchiveEntry.Name;
			}
		}

		public PackageResource (ZipArchiveEntry zipArchiveEntry)
		{
			this.zipArchiveEntry = zipArchiveEntry;
		}

		public override string ToString ()
		{
			return zipArchiveEntry.Name;
		}

		public byte[] LoadData()
		{
			byte[] data = new byte[zipArchiveEntry.Length];
			using (var stream = zipArchiveEntry.Open ())
			{
				stream.Read (data, 0, (int)zipArchiveEntry.Length);
			}
			return data;
		}
	}
}
