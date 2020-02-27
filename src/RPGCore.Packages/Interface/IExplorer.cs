using System;

namespace RPGCore.Packages
{
	public interface IExplorer : IDisposable
	{
		string Name { get; }
		string Version { get; }
		IResourceCollection Resources { get; }
		ITagsCollection Tags { get; }
		IDirectory RootDirectory { get; }
	}
}
