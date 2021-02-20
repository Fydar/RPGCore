using System;

namespace RPGCore.Projects.Extensions.MetaFiles
{
	/// <summary>
	/// <para>Extensions used to add json metafiles to an import pipeline.</para>
	/// </summary>
	public static class JsonMetaFilesIImportPipelineBuilderExtensions
	{
		/// <summary>
		/// <para>Adds the nessessary services and filters for locating metafiles.</para>
		/// </summary>
		/// <param name="importPipelineBuilder">The <see cref="IImportPipelineBuilder"/> to register with.</param>
		/// <param name="options">Options used to configure the feature.</param>
		/// <returns>The builder object.</returns>
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
