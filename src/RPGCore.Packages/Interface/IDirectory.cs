using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IDirectory
	{
		string Name { get; }
		string FullName { get; }

		IDirectory Parent { get; }
		IReadOnlyList<IDirectory> Directories { get; }
		IResourceCollection Resources { get; }
	}
}
