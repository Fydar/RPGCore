using System.IO;

namespace RPGCore.Packages.Pipeline
{
	/// <summary>
	/// Tags all resources imported by this importer 
	/// </summary>
	public class TagAllProjectResourceImporter : ProjectResourceImporter
	{
		public override ProjectResource ImportResource(ProjectExplorer projectExplorer, FileInfo fileInfo, string projectKey)
		{
			return new ProjectResource(projectExplorer, projectKey, fileInfo);
		}
	}
}
