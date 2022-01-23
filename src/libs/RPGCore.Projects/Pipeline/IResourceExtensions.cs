using RPGCore.Packages;

namespace RPGCore.Projects.Pipeline;

public static class IResourceExtensions
{
	public static ProjectResourceUpdate AuthorUpdate(this IResource resource)
	{
		return new ProjectResourceUpdate((ProjectExplorer)resource.Explorer, resource.FullName);
	}
}
