using RPGCore.Packages;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Projects;

[DebuggerDisplay("Count = {Count,nq}")]
[DebuggerTypeProxy(typeof(ProjectResourceDependenciesDebugView))]
public sealed class ProjectResourceDependencies : IResourceDependencyCollection
{
	private readonly ProjectExplorer projectExplorer;
	internal readonly List<ProjectResourceDependency> dependencies;

	public int Count => dependencies.Count;

	public ProjectResourceDependency this[int index] => dependencies[index];

	IResourceDependency IResourceDependencyCollection.this[int index] => this[index];

	internal ProjectResourceDependencies(ProjectExplorer projectExplorer)
	{
		this.projectExplorer = projectExplorer;
		dependencies = new List<ProjectResourceDependency>();
	}

	public IEnumerator<ProjectResourceDependency> GetEnumerator()
	{
		return dependencies.GetEnumerator();
	}

	IEnumerator<IResourceDependency> IEnumerable<IResourceDependency>.GetEnumerator()
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
