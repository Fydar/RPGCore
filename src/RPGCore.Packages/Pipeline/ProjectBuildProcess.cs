using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace RPGCore.Packages
{
	public class ProjectBuildProcess
	{
		public BuildPipeline Pipeline { get; }
		public ProjectExplorer Project { get; }

		public PackageDefinitionFile PackageDefinition { get; }
		public string OutputFolder { get; }
		public double Progress { get; private set; }

		public ProjectBuildProcess(BuildPipeline pipeline, ProjectExplorer project, string outputFolder)
		{
			Pipeline = pipeline;
			Project = project;
			OutputFolder = outputFolder;

			PackageDefinition = new PackageDefinitionFile();
		}

		public void PerformBuild()
		{
			string bpkgPath = Path.Combine(OutputFolder, Project.Name + ".bpkg");
			foreach (var reference in Project.Definition.References)
			{
				reference.IncludeInBuild(this, OutputFolder);
			}

			using var fileStream = new FileStream(bpkgPath, FileMode.Create, FileAccess.Write);
			using var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, false);

			var manifest = archive.CreateEntry("Main.bmft");
			using (var zipStream = manifest.Open())
			{
				string json = JsonConvert.SerializeObject(PackageDefinition);
				byte[] bytes = Encoding.UTF8.GetBytes(json);
				zipStream.Write(bytes, 0, bytes.Length);
			}

			var tagsEntry = archive.CreateEntry("tags.json");
			var tagsDocument = new Dictionary<string, List<string>>();
			foreach (var projectTagCategory in Project.Tags)
			{
				if (!tagsDocument.TryGetValue(projectTagCategory.Key, out var taggedResources))
				{
					taggedResources = new List<string>();
					tagsDocument[projectTagCategory.Key] = taggedResources;
				}
				taggedResources.AddRange(projectTagCategory.Value.Select(tag => tag.FullName));
			}
			WriteJsonDocument(tagsEntry, tagsDocument);

			long currentProgress = 0;

			foreach (var resource in Project.Resources)
			{
				var exporter = Pipeline.GetExporter(resource);

				string entryName = resource.FullName;
				long size = resource.UncompressedSize;

				ZipArchiveEntry entry;
				if (exporter == null)
				{
					entry = archive.CreateEntryFromFile(resource.Entry.FullName, entryName, CompressionLevel.Optimal);
				}
				else
				{
					entry = archive.CreateEntry(entryName);

					using var zipStream = entry.Open();
					exporter.BuildResource(resource, zipStream);
				}

				currentProgress += size;

				Progress = (currentProgress / (double)Project.UncompressedSize);

				foreach (var action in Pipeline.BuildActions)
				{
					if (action is IResourceBuildStep resourceBuildStep)
					{
						resourceBuildStep.OnAfterBuildResource(this, resource);
					}
				}
			}
		}

		private static void WriteJsonDocument(ZipArchiveEntry entry, object value)
		{
			using var zipStream = entry.Open();
			using var sr = new StreamWriter(zipStream);
			using var writer = new JsonTextWriter(sr);

			var serializer = new JsonSerializer();
			serializer.Serialize(writer, value);
		}
	}
}
