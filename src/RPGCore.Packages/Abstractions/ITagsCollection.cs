using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface ITagsCollection : IEnumerable<KeyValuePair<string, IResourceCollection>>
	{
		int Count { get; }

		IEnumerable<string> Keys { get; }

		IResourceCollection this[string tag] { get; }

		bool ContainsKey(string key);

		bool TryGetValue(string key, out IResourceCollection value);
	}
}
