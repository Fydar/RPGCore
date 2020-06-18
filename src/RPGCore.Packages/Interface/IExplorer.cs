using System;

namespace RPGCore.Packages
{
	public interface IExplorer : IDisposable
	{
		IDefinition Definition { get; }
		IResourceCollection Resources { get; }
		ITagsCollection Tags { get; }
		IDirectory RootDirectory { get; }
	}
}
