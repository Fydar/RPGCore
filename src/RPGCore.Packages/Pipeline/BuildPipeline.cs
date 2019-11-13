using System.Collections.Generic;

namespace RPGCore.Packages
{
	public class BuildPipeline
	{
		public List<ResourceExporter> Exporters = new List<ResourceExporter> ();
		public List<IBuildAction> BuildActions = new List<IBuildAction> ();

		public ResourceExporter GetExporter(ProjectResource resource)
		{
			foreach (var exporter in Exporters)
			{
				if (resource.Name.EndsWith ("." + exporter.ExportExtensions))
				{
					return exporter;
				}
			}
			return null;
		}
	}
}
