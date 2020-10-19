namespace RPGCore.Packages.Pipeline
{
	public static class IResourceExtensions
	{
		public static ProjectResourceUpdate AuthorUpdate(this IResource resource)
		{
			return new ProjectResourceUpdate((ProjectExplorer)resource.Explorer, resource.FullName);
		}
	}
}
