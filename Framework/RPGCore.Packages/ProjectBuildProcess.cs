namespace RPGCore.Packages
{
	public class ProjectBuildProcess
	{
		public ProjectExplorer Source;
		public string OutputFolder;
		public string TargetPackagePath;

		public PackageDefinitionFile PackageDefinition;

		public ProjectBuildProcess(ProjectExplorer source, string outputFolder)
		{
			Source = source;
			OutputFolder = outputFolder;

			PackageDefinition = new PackageDefinitionFile();
		}
	}
}
