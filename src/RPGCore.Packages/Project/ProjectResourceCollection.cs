using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(ProjectResourceCollectionDebugView))]
	internal sealed class ProjectResourceCollection : IProjectResourceCollection, IResourceCollection
	{
		private Dictionary<string, ProjectResource> resources;

		public int Count => resources?.Count ?? 0;

		public ProjectResource this[string key] => resources[key];
		IResource IResourceCollection.this[string key] => this[key];

		internal ProjectResourceCollection()
		{
		}

		internal ProjectResourceCollection(Dictionary<string, ProjectResource> resources)
		{
			this.resources = resources;
		}

		public void Add(ProjectResource folder)
		{
			if (resources == null)
			{
				resources = new Dictionary<string, ProjectResource>();
			}

			resources.Add(folder.FullName, folder);
		}

		public IEnumerator<ProjectResource> GetEnumerator()
		{
			return resources?.Values == null
				? Enumerable.Empty<ProjectResource>().GetEnumerator()
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
