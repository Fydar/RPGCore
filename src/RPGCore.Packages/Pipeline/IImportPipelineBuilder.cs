using RPGCore.Packages.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IImportPipelineBuilder
	{
		List<ImportProcessor> Processors { get; }
		List<ImportFilter> Filters { get; }

		ImportPipeline Build();
		IImportPipelineBuilder UseFilter(ImportFilter importFilter);
		IImportPipelineBuilder UseProcessor(ImportProcessor importProcessor);
	}
}
