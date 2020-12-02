using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(PackageResourceCollectionDebugView))]
	public sealed class PackageResourceCollection : IResourceCollection
	{
		private readonly Dictionary<string, PackageResource> resources;

		public int Count => resources.Count;

		public PackageResource this[string key] => resources[key];

		IResource IResourceCollection.this[string key] => this[key];

		internal PackageResourceCollection()
		{
			resources = new Dictionary<string, PackageResource>();
		}

		internal void Add(string key, PackageResource asset)
		{
			resources.Add(key, asset);
		}

		public bool Contains(string key)
		{
			return resources.ContainsKey(key);
		}

		public bool TryGetResource(string key, out PackageResource value)
		{
			return resources.TryGetValue(key, out value);
		}

		bool IResourceCollection.TryGetResource(string key, out IResource value)
		{
			bool result = TryGetResource(key, out var resource);
			value = resource;
			return result;
		}

		public IEnumerator<PackageResource> GetEnumerator()
		{
			return resources.Values.GetEnumerator();
		}

		IEnumerator<IResource> IEnumerable<IResource>.GetEnumerator()
		{
			return resources.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return resources.Values.GetEnumerator();
		}

		private class PackageResourceCollectionDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}")]
			internal struct DebuggerRow
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public IResource Value;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly PackageResourceCollection source;

			public PackageResourceCollectionDebugView(PackageResourceCollection source)
			{
				this.source = source;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerRow[] Keys
			{
				get
				{
					var keys = new DebuggerRow[source.resources.Count];

					int i = 0;
					foreach (var kvp in source.resources)
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
