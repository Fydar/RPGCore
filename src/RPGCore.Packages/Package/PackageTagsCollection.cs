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
		private readonly IReadOnlyDictionary<string, IResourceCollection> tags;

		public IResourceCollection this[string tag] => tags[tag];

		public int Count => tags?.Count ?? 0;
		public IEnumerable<string> Keys => tags.Keys;

		internal PackageTagsCollection(IReadOnlyDictionary<string, IResourceCollection> tags)
		{
			this.tags = tags;
		}

		public bool ContainsKey(string key)
		{
			return tags?.ContainsKey(key) ?? false;
		}

		public bool TryGetValue(string key, out IResourceCollection value)
		{
			value = default;
			return tags?.TryGetValue(key, out value) ?? false;
		}

		public IEnumerator<KeyValuePair<string, IResourceCollection>> GetEnumerator()
		{
			return tags == null
				? Enumerable.Empty<KeyValuePair<string, IResourceCollection>>().GetEnumerator()
				: tags.GetEnumerator();
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

			private readonly PackageTagsCollection source;

			public PackageTagsCollectionDebugView(PackageTagsCollection source)
			{
				this.source = source;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerRow[] Keys
			{
				get
				{
					var keys = new DebuggerRow[source.tags.Count];

					int i = 0;
					foreach (var kvp in source.tags)
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
