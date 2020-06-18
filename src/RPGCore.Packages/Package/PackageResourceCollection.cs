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

		internal void Add(PackageResource asset)
		{
			resources.Add(asset.FullName, asset);
		}

		public IEnumerator<PackageResource> GetEnumerator()
		{
			return resources.Values.GetEnumerator();
		}

		IEnumerator<IResource> IEnumerable<IResource>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private class PackageResourceCollectionDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}")]
			internal struct DebuggerRow
			{
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public IResource Value;
			}

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
