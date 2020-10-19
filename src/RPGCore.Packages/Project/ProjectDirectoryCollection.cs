using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(ProjectDirectoryCollectionDebugView))]
	public sealed class ProjectDirectoryCollection : IDirectoryCollection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<ProjectDirectory> directories;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => directories.Count;

		public ProjectDirectory this[string key]
		{
			get
			{
				foreach (var directory in directories)
				{
					if (directory.Name == key)
					{
						return directory;
					}
				}
				return null;
			}
		}

		public ProjectDirectory this[int key] => directories[key];

		IDirectory IReadOnlyList<IDirectory>.this[int key] => this[key];

		internal ProjectDirectoryCollection()
		{
			directories = new List<ProjectDirectory>();
		}

		internal void Add(ProjectDirectory item)
		{
			directories.Add(item);
		}

		public IEnumerator<ProjectDirectory> GetEnumerator()
		{
			return directories.GetEnumerator();
		}

		IEnumerator<IDirectory> IEnumerable<IDirectory>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		private class ProjectDirectoryCollectionDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}")]
			internal struct DebuggerRow
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
