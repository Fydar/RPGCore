using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(PackageResourceDependenciesDebugView))]
	public class PackageResourceDependencies : IResourceDependencies
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly PackageExplorer packageExplorer;

		private readonly List<PackageResourceDependency> dependencies;

		public int Count
		{
			get
			{
				return dependencies.Count;
			}
		}

		public PackageResourceDependency this[int index]
		{
			get
			{
				return dependencies[index];
			}
		}

		IResourceDependency IResourceDependencies.this[int index] => this[index];

		internal PackageResourceDependencies(PackageExplorer packageExplorer, PackageResourceMetadataModel metadataModel)
		{
			this.packageExplorer = packageExplorer;
			dependencies = new List<PackageResourceDependency>();

			foreach (var importerDependency in metadataModel.Dependencies)
			{
				dependencies.Add(
					new PackageResourceDependency(packageExplorer, importerDependency));
			}
		}

		public IEnumerator<IResourceDependency> GetEnumerator()
		{
			return dependencies.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return dependencies.GetEnumerator();
		}

		private class PackageResourceDependenciesDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}")]
			internal struct DebuggerRow
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public PackageResourceDependency Value;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly PackageResourceDependencies source;

			public PackageResourceDependenciesDebugView(PackageResourceDependencies source)
			{
				this.source = source;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerRow[] Keys
			{
				get
				{
					var keys = new DebuggerRow[source.dependencies.Count];

					int i = 0;
					foreach (var dependency in source.dependencies)
					{
						keys[i] = new DebuggerRow
						{
							Key = dependency.Key,
							Value = dependency
						};
						i++;
					}
					return keys;
				}
			}
		}
	}
}
