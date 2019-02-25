using System.Collections.Generic;

namespace RPGCore.Behaviour.Packages
{
	public interface IProjectAssetCollection : IEnumerable<ProjectAsset>
	{
		ProjectAsset this[string key] { get; }

		void Add (ProjectAsset folder);
	}
}
