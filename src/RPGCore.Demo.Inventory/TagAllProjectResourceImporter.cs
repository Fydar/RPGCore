using RPGCore.Packages.Pipeline;

namespace RPGCore.Demo.Inventory
{
	/// <summary>
	/// Tags all resources processed by this importer.
	/// </summary>
	public class TagAllProjectResourceImporter : ProjectResourceImportProcessor
	{
		public override void ProcessImport(ProjectResourceImporter projectResource)
		{
			projectResource.ImporterTags.Add("basic-tag");
		}
	}
}
