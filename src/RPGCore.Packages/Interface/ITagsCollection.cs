using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface ITagsCollection : IEnumerable<KeyValuePair<string, IResourceCollection>>
	{
		IResourceCollection this[string tag] { get; }

		int Count { get; }
		IEnumerable<string> Keys { get; }

		bool ContainsKey(string key);

		bool TryGetValue(string key, out IResourceCollection value);
	}
}
