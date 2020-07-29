using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Packages.Pipeline
{
	public class ProjectResourceImporterDependencyCollection : IEnumerable<ProjectResourceImporterDependency>
	{
		private readonly ProjectResourceImporter projectResourceImporter;

		private readonly List<ProjectResourceImporterDependency> dependancies;

		internal ProjectResourceImporterDependencyCollection(ProjectResourceImporter projectResourceImporter)
		{
			this.projectResourceImporter = projectResourceImporter;

			dependancies = new List<ProjectResourceImporterDependency>();
		}

		public void Register(string resource, DependencyFlags dependencyFlags = DependencyFlags.None, Dictionary<string, string> metadata = null)
		{
			foreach (var dependency in dependancies)
			{
				if (dependency.Resource == resource)
				{
					dependency.DependencyFlags &= dependencyFlags;
					dependency.Metadata = metadata;
					return;
				}
			}

			dependancies.Add(new ProjectResourceImporterDependency()
			{
				Resource = resource,
				DependencyFlags = dependencyFlags,
				Metadata = metadata
			});
		}

		public IEnumerator<ProjectResourceImporterDependency> GetEnumerator()
		{
			return dependancies.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return dependancies.GetEnumerator();
		}
	}
}
