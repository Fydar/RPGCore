using RPGCore.Packages;
using RPGCore.Packages.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Demo.Inventory
{
	/// <summary>
	/// Tags all resources processed by this importer.
	/// </summary>
	public class TagAllProjectResourceImporter : IImportProcessor
	{
		public bool CanProcess(IResource resource)
		{
			return true;
		}

		public IEnumerable<ProjectResourceUpdate> ProcessImport(ImportProcessorContext context, IResource resource)
		{
			var update = resource.AuthorUpdate();

			update.ImporterTags.Add("basic-tag");

			yield return update;
		}
	}
}
