namespace RPGCore.Projects;

public class BuildPipeline
{
	public ImportPipeline ImportPipeline { get; set; }
	public BuildActionCollection BuildActions { get; }

	public BuildPipeline()
	{
		BuildActions = new BuildActionCollection();
	}
}
