using System;

namespace RPGCore.Packages
{
	public interface IPackageExplorer : IDisposable
	{
		string Name { get; }
		string Version { get; }
		Reference[] References { get; }
		IPackageAssetCollection Assets { get; }
	}
}
