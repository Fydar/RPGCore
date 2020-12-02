using RPGCore.Packages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Projects
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(ProjectTagsCollectionDebugView))]
	public class ProjectTagsCollection : ITagsCollection
	{
		private readonly IReadOnlyDictionary<string, IResourceCollection> tags;

		public IResourceCollection this[string tag] => tags[tag];

		public int Count => tags.Count;
		public IEnumerable<string> Keys => tags.Keys;

		internal ProjectTagsCollection(IReadOnlyDictionary<string, IResourceCollection> tags)
		{
			this.tags = tags ?? throw new ArgumentNullException(nameof(tags));
		}

		public bool ContainsKey(string key)
		{
			return tags.ContainsKey(key);
		}

		public bool TryGetValue(string key, out IResourceCollection value)
		{
			return tags.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<string, IResourceCollection>> GetEnumerator()
		{
			return tags.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return tags.GetEnumerator();
		}

		private class ProjectTagsCollectionDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}")]
			internal struct DebuggerRow
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public IResourceCollection Value;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly ProjectTagsCollection source;

			public ProjectTagsCollectionDebugView(ProjectTagsCollection source)
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
