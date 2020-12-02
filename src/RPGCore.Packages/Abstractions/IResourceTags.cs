using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IResourceTags : IEnumerable<string>
	{
		int Count { get; }

		bool Contains(string tag);
	}
}
