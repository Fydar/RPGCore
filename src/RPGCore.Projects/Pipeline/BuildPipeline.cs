using System.Collections.Generic;

namespace RPGCore.Projects
{
	public class BuildPipeline
	{
		public ImportPipeline ImportPipeline { get; set; }
		public List<ResourceExporter> Exporters { get; }
		public BuildActionCollection BuildActions { get; }

		public BuildPipeline()
		{
			Exporters = new List<ResourceExporter>();
			BuildActions = new BuildActionCollection();
		}

		public ResourceExporter GetExporter(ProjectResource resource)
		{
			foreach (var exporter in Exporters)
			{
				if (exporter.CanExport(resource))
				{
					return exporter;
				}
			}
			return null;
		}
	}
}
