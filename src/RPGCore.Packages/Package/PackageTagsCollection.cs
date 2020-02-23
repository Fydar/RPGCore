using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(PackageTagsCollectionDebugView))]
	public class PackageTagsCollection : ITagsCollection
	{
		private readonly IReadOnlyDictionary<string, IResourceCollection> TagsInternal;

		public IResourceCollection this[string tag] => TagsInternal[tag];

		public int Count => TagsInternal?.Count ?? 0;
		public IEnumerable<string> Keys => TagsInternal.Keys;

		public PackageTagsCollection(IReadOnlyDictionary<string, IResourceCollection> tags)
		{
			TagsInternal = tags;
		}

		public bool ContainsKey(string key)
		{
			return TagsInternal?.ContainsKey(key) ?? false;
		}

		public bool TryGetValue(string key, out IResourceCollection value)
		{
			value = default;
			return TagsInternal?.TryGetValue(key, out value) ?? false;
		}

		public IEnumerator<KeyValuePair<string, IResourceCollection>> GetEnumerator()
		{
			return TagsInternal == null
				? Enumerable.Empty<KeyValuePair<string, IResourceCollection>>().GetEnumerator()
				: TagsInternal.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private class PackageTagsCollectionDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}")]
			internal struct DebuggerRow
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public IResourceCollection Value;
			}

			private readonly PackageTagsCollection Source;

			public PackageTagsCollectionDebugView(PackageTagsCollection source)
			{
				Source = source;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerRow[] Keys
			{
				get
				{
					var keys = new DebuggerRow[Source.TagsInternal.Count];

					int i = 0;
					foreach (var kvp in Source.TagsInternal)
					{
						keys[i] = new DebuggerRow
						{
							Key = kvp.Key,
							Value = kvp.Value
						};
						i++;
					}
					return keys;
				}
			}
		}
	}
}
