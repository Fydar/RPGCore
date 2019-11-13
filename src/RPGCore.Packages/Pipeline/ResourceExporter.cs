using System.IO;

namespace RPGCore.Packages
{
	public abstract class ResourceExporter
	{
		public abstract string ExportExtensions { get; }

		public abstract void BuildResource(IResource resource, Stream writer);
	}
}
