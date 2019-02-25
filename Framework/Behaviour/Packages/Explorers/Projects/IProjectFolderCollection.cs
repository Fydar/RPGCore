using System.Collections.Generic;

namespace RPGCore.Behaviour.Packages
{
	public interface IProjectFolderCollection : IEnumerable<ProjectFolder>
	{
		ProjectFolder this[string key] { get; }

		void Add (ProjectFolder folder);
	}
}
