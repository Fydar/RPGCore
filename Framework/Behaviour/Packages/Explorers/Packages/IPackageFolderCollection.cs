using System.Collections.Generic;

namespace RPGCore.Behaviour.Packages
{
	public interface IPackageFolderCollection : IEnumerable<PackageFolder>
	{
		PackageFolder this[string key] { get; }

		void Add (PackageFolder folder);
	}
}
