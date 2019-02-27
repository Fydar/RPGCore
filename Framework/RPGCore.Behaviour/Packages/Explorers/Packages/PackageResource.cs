using System.IO;
using System.IO.Compression;

namespace RPGCore.Behaviour.Packages
{
	public struct PackageResource
	{
		private readonly PackageExplorer Package;
		private readonly string PackageKey;

		public string Name
		{
			get
			{
				return PackageKey.Substring(PackageKey.LastIndexOf('/') + 1);
			}
		}

		public PackageResource (PackageExplorer package, string packageKey)
		{
			Package = package;
			PackageKey = packageKey;
		}

		public override string ToString ()
		{
			return Name;
		}

		public byte[] LoadData()
		{
			return Package.OpenAsset(PackageKey);
		}
	}
}
