using System.Collections.Generic;

namespace RPGCore.Packages
{
	public class ProjectDirectory : IDirectory
	{
		public string Name => throw new System.NotImplementedException();

		public string FullName => throw new System.NotImplementedException();

		public IReadOnlyList<IDirectory> Directories => throw new System.NotImplementedException();

		public IResourceCollection Resources => throw new System.NotImplementedException();
	}
}
