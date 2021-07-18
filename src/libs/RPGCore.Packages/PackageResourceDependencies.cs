using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	/// <summary>
	/// Represents a collection of <see cref="PackageResourceDependency"/>.
	/// </summary>
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(PackageResourceDependenciesDebugView))]
	public class PackageResourceDependencies : IResourceDependencyCollection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly PackageExplorer packageExplorer;

		private readonly List<PackageResourceDependency> dependencies;

		/// <inheritdoc/>
		public int Count => dependencies.Count;

		/// <summary>
		/// Retrieves a <see cref="PackageResourceDependency"/> from the collection by an index.
		/// </summary>
		/// <param name="index">The integer-index for the resource in the collection.</param>
		/// <returns>The <see cref="PackageResourceDependency"/> found within the collection; otherwise returns <c>null</c>.</returns>
		public PackageResourceDependency this[int index] => dependencies[index];

		IResourceDependency IResourceDependencyCollection.this[int index] => this[index];

		internal PackageResourceDependencies(PackageExplorer packageExplorer, PackageResourceMetadataModel metadataModel)
		{
			this.packageExplorer = packageExplorer;

			dependencies = new List<PackageResourceDependency>();
			if (metadataModel.Dependencies != null)
			{
				foreach (var importerDependency in metadataModel.Dependencies)
				{
					dependencies.Add(
						new PackageResourceDependency(packageExplorer, importerDependency));
				}
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
