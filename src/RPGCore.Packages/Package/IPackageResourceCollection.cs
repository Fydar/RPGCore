using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IPackageResourceCollection : IEnumerable<PackageResource>
	{
		int Count { get; }

		PackageResource this[string key] { get; }
	}
}
