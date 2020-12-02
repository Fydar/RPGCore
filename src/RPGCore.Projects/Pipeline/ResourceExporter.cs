using RPGCore.FileTree;
using RPGCore.Packages;

namespace RPGCore.Projects
{
	/// <summary>
	/// A single resource exporter is selected to write a resource to a package. An exporter is used
	/// to convert a <see cref="ProjectResource"/> into a <see cref="PackageResource"/>.
	/// </summary>
	public abstract class ResourceExporter
	{
		public abstract bool CanExport(IResource resource);

		public abstract void BuildResource(IResource resource, IArchiveDirectory destination);
	}
}
