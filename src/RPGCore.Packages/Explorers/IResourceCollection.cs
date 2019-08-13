using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IResourceCollection : IEnumerable<IResource>
	{
		IResource this[string key] { get; }

		void Add (IResource folder);
	}
}
