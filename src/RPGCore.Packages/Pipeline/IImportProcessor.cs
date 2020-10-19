using System.Collections.Generic;

namespace RPGCore.Packages.Pipeline
{
	/// <summary>
	/// Applies additional modifications to resources that have already been imported.
	/// </summary>
	public interface IImportProcessor
	{
		bool CanProcess(IResource resource);
		IEnumerable<ProjectResourceUpdate> ProcessImport(ImportProcessorContext context, IResource resource);
	}
}
