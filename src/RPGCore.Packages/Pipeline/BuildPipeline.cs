using System.Collections.Generic;

namespace RPGCore.Packages
{
	public class BuildPipeline
	{
		public ImportPipeline ImportPipeline { get; set; }
		public List<ResourceExporter> Exporters { get; set; }
		public List<IBuildAction> BuildActions { get; set; }

		public BuildPipeline()
		{
			ImportPipeline = new ImportPipeline();
			Exporters = new List<ResourceExporter>();
			BuildActions = new List<IBuildAction>();
		}

		public ResourceExporter GetExporter(ProjectResource resource)
		{
			foreach (var exporter in Exporters)
			{
				if (resource.Name.EndsWith("." + exporter.ExportExtensions))
				{
					return exporter;
				}
			}
			return null;
		}
	}
}
