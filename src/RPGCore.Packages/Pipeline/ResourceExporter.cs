using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RPGCore.Packages
{
	public interface IResourceDescription
	{
		IReadOnlyList<string> Tags { get; }


	}

	public class ProjectResourceDescription : IResourceDescription
	{
		public List<string> Tags { get; }

		IReadOnlyList<string> IResourceDescription.Tags => Tags;

		public ProjectResourceDescription(ProjectResource resource)
		{
		}
	}

	public class PackageResourceDescription : IResourceDescription
	{
		public IReadOnlyList<string> Tags { get; }

		public PackageResourceDescription(PackageExplorer package, PackageResource resource)
		{
			if (package.Tags == null)
			{
				return;
			}

			var tags = new List<string>();

			foreach (var tagCollection in package.Tags)
			{
				if (tagCollection.Value.Contains(resource.FullName))
				{
					tags.Add(tagCollection.Key);
				}
			}

			Tags = tags;
		}
	}

	public class ResourcePostProcessor
	{
		public virtual void DescribeResource(IResource resource)
		{

		}
	}


	/// <summary>
	/// A single resource exporter is selected to write a resource to a package.
	/// An exporter is used to 
	/// </summary>
	public abstract class ResourceExporter
	{
		public abstract string ExportExtensions { get; }

		public abstract void BuildResource(IResource resource, Stream writer);
	}
}
