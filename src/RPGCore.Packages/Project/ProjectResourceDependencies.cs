using RPGCore.Packages.Pipeline;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(ProjectResourceDependenciesDebugView))]
	public sealed class ProjectResourceDependencies : IResourceDependencies
	{
		private readonly ProjectExplorer projectExplorer;
		private readonly List<ProjectResourceDependency> dependencies;

		public int Count
		{
			get
			{
				return dependencies.Count;
			}
		}

		public ProjectResourceDependency this[int index]
		{
			get
			{
				return dependencies[index];
			}
		}

		IResourceDependency IResourceDependencies.this[int index] => this[index];

		internal ProjectResourceDependencies(ProjectResourceImporter projectResourceImporter)
		{
			projectExplorer = projectResourceImporter.ProjectExplorer;
			dependencies = new List<ProjectResourceDependency>();

			foreach (var importerDependency in projectResourceImporter.Dependencies)
			{
				dependencies.Add(
					new ProjectResourceDependency(projectExplorer, importerDependency.Resource));
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

		private class ProjectResourceDependenciesDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key}")]
			internal struct DebuggerRow
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public ProjectResourceDependency Value;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly ProjectResourceDependencies source;

			public ProjectResourceDependenciesDebugView(ProjectResourceDependencies source)
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
