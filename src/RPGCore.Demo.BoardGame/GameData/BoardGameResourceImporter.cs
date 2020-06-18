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
				projectResource.ImporterTags.Add("type-building");
			}
			else if (projectResource.FileInfo.FullName.Contains("resources"))
			{
				projectResource.ImporterTags.Add("type-resource");
			}
			else if (projectResource.FileInfo.FullName.Contains("building-packs"))
			{
				projectResource.ImporterTags.Add("type-buildingpack");
			}
			else if (projectResource.FileInfo.FullName.Contains("gamerules"))
			{
				projectResource.ImporterTags.Add("gamerules");
			}
		}
	}
}
