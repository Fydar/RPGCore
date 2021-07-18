using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(PackageDirectoryCollectionDebugView))]
	public sealed class PackageDirectoryCollection : IDirectoryCollection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<PackageDirectory> directories;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => directories.Count;

		public PackageDirectory this[int key] => directories[key];

		IDirectory IReadOnlyList<IDirectory>.this[int key] => this[key];

		internal PackageDirectoryCollection()
		{
			directories = new List<PackageDirectory>();
		}

		internal void Add(PackageDirectory item)
		{
			directories.Add(item);
		}

		public IEnumerator<PackageDirectory> GetEnumerator()
		{
			return directories.GetEnumerator();
		}

		IEnumerator<IDirectory> IEnumerable<IDirectory>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private class PackageDirectoryCollectionDebugView
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
			private readonly PackageDirectoryCollection source;

			public PackageDirectoryCollectionDebugView(PackageDirectoryCollection source)
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
