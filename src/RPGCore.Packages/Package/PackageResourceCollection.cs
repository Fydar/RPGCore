using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(PackageResourceCollectionDebugView))]
	internal sealed class PackageResourceCollection : IPackageResourceCollection, IResourceCollection
	{
		private Dictionary<string, PackageResource> resources;

		public int Count => resources?.Count ?? 0;

		public PackageResource this[string key] => resources[key];
		IResource IResourceCollection.this[string key] => this[key];

		internal void Add(PackageResource asset)
		{
			if (resources == null)
			{
				resources = new Dictionary<string, PackageResource>();
			}

			resources.Add(asset.FullName, asset);
		}

		public IEnumerator<PackageResource> GetEnumerator()
		{
			return resources?.Values == null
				? Enumerable.Empty<PackageResource>().GetEnumerator()
				: resources.Values.GetEnumerator();
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
