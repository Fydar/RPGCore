using System;

namespace RPGCore.Packages.Extensions.MetaFiles
{
	public static class JsonMetaFilesIImportPipelineBuilderExtensions
	{
		public static IImportPipelineBuilder UseJsonMetaFiles(this IImportPipelineBuilder importPipelineBuilder, Action<JsonMetaFilesOptions> options = null)
		{
			var optionsModel = new JsonMetaFilesOptions();
			options?.Invoke(optionsModel);

			importPipelineBuilder.Filters.Add(new JsonFileSuffixImportFilter(optionsModel.MetaFileSuffix));
			importPipelineBuilder.Processors.Add(new JsonMetaFileImportProcessor(optionsModel));

			return importPipelineBuilder;
		}
	}
}
