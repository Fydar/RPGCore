using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IAsset
	{
		string Name { get; }

		IEnumerable<IResource> Resources { get; }

		IResource GetResource (string path);
	}
}
