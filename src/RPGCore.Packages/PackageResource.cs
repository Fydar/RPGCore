using RPGCore.FileTree;
using System.Diagnostics;

namespace RPGCore.Packages
{
	public sealed class PackageResource : IResource
	{
		public string Name { get; }
		public string FullName { get; }
		public string Extension { get; }

		public PackageExplorer Explorer { get; }
		public PackageResourceContent Content { get; }
		public PackageDirectory Directory { get; }
		public PackageResourceDependencies Dependencies { get; }
		public PackageResourceTags Tags { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IExplorer IResource.Explorer => Explorer;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceContent IResource.Content => Content;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectory IResource.Directory => Directory;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceDependencyCollection IResource.Dependencies => Dependencies;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceTags IResource.Tags => Tags;

		public IResourceDependencyCollection Dependants => throw new System.NotImplementedException();

		internal PackageResource(PackageExplorer packageExplorer, PackageDirectory directory, IReadOnlyArchiveFile contentEntry, PackageResourceMetadataModel metadataModel)
		{
			Explorer = packageExplorer;
			Directory = directory;
			Dependencies = new PackageResourceDependencies(packageExplorer, metadataModel);
			Tags = new PackageResourceTags();

			Name = contentEntry.Name;

			string withoutData = contentEntry.FullName.Substring(5);
			FullName = withoutData;

			Content = new PackageResourceContent(packageExplorer, contentEntry.FullName, contentEntry);

			int dotIndex = contentEntry.FullName.LastIndexOf('.');
			Extension = dotIndex != -1
				? contentEntry.FullName.Substring(dotIndex)
				: null;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
