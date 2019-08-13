using System;

namespace RPGCore.Packages
{
	public interface IPackageExplorer : IDisposable
	{
		string Name { get; }
		string Version { get; }
		IResourceCollection Resources { get; }
	}
}
