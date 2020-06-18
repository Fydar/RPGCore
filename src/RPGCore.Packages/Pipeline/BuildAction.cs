namespace RPGCore.Packages
{
	public abstract class BuildAction
	{
		public virtual void OnBeforeExportResource(ProjectBuildProcess process, ProjectResource resource)
		{
		}

		public virtual void OnAfterExportResource(ProjectBuildProcess process, ProjectResource resource)
		{
		}
	}
}
