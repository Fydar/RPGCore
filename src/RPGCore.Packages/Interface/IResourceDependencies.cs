using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IResourceDependencies : IEnumerable<IResourceDependency>
	{
		int Count { get; }

		IResourceDependency this[int index] { get; }
	}
}
