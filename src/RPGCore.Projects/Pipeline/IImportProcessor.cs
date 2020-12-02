using RPGCore.Packages;
using System.Collections.Generic;

namespace RPGCore.Projects.Pipeline
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
