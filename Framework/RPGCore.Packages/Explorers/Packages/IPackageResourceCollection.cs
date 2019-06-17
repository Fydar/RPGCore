using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IPackageResourceCollection : IEnumerable<PackageResource>
	{
		PackageResource this[string key] { get; }

		void Add (PackageResource folder);
	}
}
