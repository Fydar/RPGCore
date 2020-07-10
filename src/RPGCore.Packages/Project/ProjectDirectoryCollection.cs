using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(ProjectDirectoryCollectionDebugView))]
	internal sealed class ProjectDirectoryCollection : IDirectoryCollection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<ProjectDirectory> directories;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => directories?.Count ?? 0;

		public ProjectDirectory this[int key] => directories[key];

		IDirectory IReadOnlyList<IDirectory>.this[int key] => this[key];

		internal ProjectDirectoryCollection()
		{
			directories = new List<ProjectDirectory>();
		}

		internal ProjectDirectoryCollection(List<ProjectDirectory> directories)
		{
			this.directories = directories;
		}

		internal void Add(ProjectDirectory item)
		{
			directories.Add(item);
		}

		public IEnumerator<ProjectDirectory> GetEnumerator()
		{
			return directories == null
				? Enumerable.Empty<ProjectDirectory>().GetEnumerator()
				: directories.GetEnumerator();
		}

		IEnumerator<IDirectory> IEnumerable<IDirectory>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private class ProjectDirectoryCollectionDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}")]
			internal struct DebuggerRow
			{
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public IDirectory Value;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly ProjectDirectoryCollection source;

			public ProjectDirectoryCollectionDebugView(ProjectDirectoryCollection source)
			{
				this.source = source;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerRow[] Keys
			{
				get
				{
					if (source.directories == null
						|| source.directories.Count == 0)
					{
						return null;
					}

					var keys = new DebuggerRow[source.directories.Count];

					int i = 0;
					foreach (var directory in source.directories)
					{
						keys[i] = new DebuggerRow
						{
							Key = directory.Name,
							Value = directory
						};
						i++;
					}
					return keys;
				}
			}
		}
	}
}
