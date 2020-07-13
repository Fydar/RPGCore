using RPGCore.Packages.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Packages
{
	public class ImportPipelineBuilder : IImportPipelineBuilder
	{
		public List<ImportProcessor> Processors { get; }
		public List<ImportFilter> Filters { get; }

		internal ImportPipelineBuilder()
		{
			Processors = new List<ImportProcessor>();
			Filters = new List<ImportFilter>();
		}

		public ImportPipeline Build()
		{
			return new ImportPipeline(this);
		}

		public IImportPipelineBuilder UseProcessor(ImportProcessor importProcessor)
		{
			Processors.Add(importProcessor);
			return this;
		}

		public IImportPipelineBuilder UseFilter(ImportFilter importFilter)
		{
			Filters.Add(importFilter);
			return this;
		}
	}
}
