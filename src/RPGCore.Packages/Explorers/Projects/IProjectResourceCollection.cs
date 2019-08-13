using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IProjectResourceCollection : IEnumerable<ProjectResource>
	{
		ProjectResource this[string key] { get; }

		void Add (ProjectResource folder);
	}
}
