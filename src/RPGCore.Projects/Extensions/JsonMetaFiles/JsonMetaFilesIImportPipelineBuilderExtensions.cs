using System;

namespace RPGCore.Projects.Extensions.MetaFiles
{
	public static class JsonMetaFilesIImportPipelineBuilderExtensions
	{
		public static IImportPipelineBuilder UseJsonMetaFiles(this IImportPipelineBuilder importPipelineBuilder, Action<JsonMetaFilesOptions> options = null)
		{
			var optionsModel = new JsonMetaFilesOptions();
			options?.Invoke(optionsModel);

			importPipelineBuilder.UseImporter(new JsonMetaFileSuffixImporter(optionsModel.MetaFileSuffix));
			importPipelineBuilder.UseProcessor(new JsonMetaFileImportProcessor(optionsModel));

			return importPipelineBuilder;
		}
	}
}
