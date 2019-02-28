using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IProjectAssetCollection : IEnumerable<ProjectAsset>
	{
		ProjectAsset this[string key] { get; }

		void Add (ProjectAsset folder);
	}
}
