using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(ProjectResourceCollectionDebugView))]
	public sealed class ProjectResourceCollection : IResourceCollection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly Dictionary<string, ProjectResource> resources;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => resources.Count;

		public ProjectResource this[string key] => resources[key];

		IResource IResourceCollection.this[string key] => this[key];

		internal ProjectResourceCollection()
		{
			resources = new Dictionary<string, ProjectResource>();
		}

		internal ProjectResourceCollection(Dictionary<string, ProjectResource> resources)
		{
			this.resources = resources ?? throw new System.ArgumentNullException(nameof(resources));
		}

		internal void Add(ProjectResource resource)
		{
			resources.Add(resource.FullName, resource);
		}

		public IEnumerator<ProjectResource> GetEnumerator()
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

		private class ProjectResourceCollectionDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}")]
			internal struct DebuggerRow
			{
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public IResource Value;
			}

			private readonly ProjectResourceCollection source;

			public ProjectResourceCollectionDebugView(ProjectResourceCollection source)
			{
				this.source = source;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerRow[] Keys
			{
				get
				{
					if (source.resources.Count == 0)
					{
						return null;
					}

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
