using RPGCore.Packages.Pipeline;

namespace RPGCore.Demo.BoardGame
{
	/// <summary>
	/// Tags appropriate resources using it's directory.
	/// </summary>
	public class BoardGameResourceImporter : ProjectResourceImportProcessor
	{
		public override void ProcessImport(ProjectResourceImporter projectResource)
		{
			// TODO: Should be changed to use directory APIs.

			if (projectResource.FileInfo.FullName.Contains("buildings"))
			{
				projectResource.Tags.Add("type-building");
			}
			else if (projectResource.FileInfo.FullName.Contains("resources"))
			{
				projectResource.Tags.Add("type-resource");
			}
		}
	}
}
