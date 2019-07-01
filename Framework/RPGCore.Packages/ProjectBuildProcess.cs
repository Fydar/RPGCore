using System.Collections.Generic;

namespace RPGCore.Packages
{
	public struct ResourceDependancyDefinition
	{
		public string Name;
		public string Sha;
	}

	public class PackageDefinitionFile
	{
		public string Name;
		public string Description;

		public Dictionary<string, ResourceDependancyDefinition> ResourceDependancies;

		public PackageDefinitionFile ()
		{
			ResourceDependancies = new Dictionary<string, ResourceDependancyDefinition> ();
		}
	}

	public class ProjectBuildProcess
	{
		public ProjectExplorer Source;
		public string OutputFolder;
		public string TargetPackagePath;

		public PackageDefinitionFile PackageDefinition;

		public ProjectBuildProcess (ProjectExplorer source, string outputFolder)
		{
			Source = source;
			OutputFolder = outputFolder;

			PackageDefinition = new PackageDefinitionFile ();
		}
	}
}