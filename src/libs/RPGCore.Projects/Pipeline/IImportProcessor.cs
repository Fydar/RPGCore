using RPGCore.Packages;
using System.Collections.Generic;

namespace RPGCore.Projects.Pipeline;

/// <summary>
/// <para>Applies additional modifications to resources that have already been imported.</para>
/// </summary>
public interface IImportProcessor
{
	/// <summary>
	/// <para>Determines whether a <see cref="IResource"/> can be modified by this processor.</para>
	/// </summary>
	/// <param name="resource"></param>
	/// <returns></returns>
	bool CanProcess(IResource resource);

	/// <summary>
	/// <para>Processes a resource.</para>
	/// </summary>
	/// <param name="context">Context for the importation process.</param>
	/// <param name="resource">The resource to process.</param>
	/// <returns>A collection of updates that will be applied to the resource.</returns>
	IEnumerable<ProjectResourceUpdate> ProcessImport(ImportProcessorContext context, IResource resource);
}
