using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IResourceCollection : IEnumerable<IResource>
	{
		int Count { get; }

		IResource this[string key] { get; }

		bool Contains(string key);
		bool TryGetResource(string key, out IResource value);
	}
}
