using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Projects.Pipeline
{
	public class ProjectResourceUpdateDependencyCollection : IEnumerable<ProjectResourceUpdateDependency>
	{
		private readonly ProjectResourceUpdate projectResourceImporter;

		private readonly List<ProjectResourceUpdateDependency> dependancies;

		internal ProjectResourceUpdateDependencyCollection(ProjectResourceUpdate projectResourceImporter)
		{
			this.projectResourceImporter = projectResourceImporter;

			dependancies = new List<ProjectResourceUpdateDependency>();
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

			dependancies.Add(new ProjectResourceUpdateDependency()
			{
				Resource = resource,
				DependencyFlags = dependencyFlags,
				Metadata = metadata
			});
		}

		public IEnumerator<ProjectResourceUpdateDependency> GetEnumerator()
		{
			return dependancies.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return dependancies.GetEnumerator();
		}
	}
}
