namespace RPGCore.Packages
{
	public interface IResourceBuildStep : IBuildAction
	{
		void OnAfterBuildResource(ProjectBuildProcess process, ProjectResource resource);
	}
}
