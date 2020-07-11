using System.Diagnostics;
using System.IO.Compression;

namespace RPGCore.Packages
{
	public sealed class PackageResource : IResource
	{
		public string Name { get; }
		public string FullName { get; }
		public string Extension { get; }

		public PackageExplorer Explorer { get; }
		public PackageResourceContent Content { get; }
		public PackageDirectory Directory { get; internal set; }
		public PackageResourceDependencies Dependencies { get; }
		public PackageResourceTags Tags { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IExplorer IResource.Explorer => Explorer;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceContent IResource.Content => Content;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectory IResource.Directory => Directory;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceDependencies IResource.Dependencies => Dependencies;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceTags IResource.Tags => Tags;

		internal PackageResource(PackageExplorer packageExplorer, ZipArchiveEntry contentEntry, PackageResourceMetadataModel metadataModel)
		{
			Explorer = packageExplorer;
			Dependencies = new PackageResourceDependencies(packageExplorer, metadataModel);
			Tags = new PackageResourceTags();

			Name = contentEntry.Name;

			string withoutData = contentEntry.FullName.Substring(5);
			FullName = withoutData;

			Content = new PackageResourceContent(packageExplorer, FullName, contentEntry);

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
