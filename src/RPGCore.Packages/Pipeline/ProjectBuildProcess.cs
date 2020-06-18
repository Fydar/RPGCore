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

		public string OutputFolder { get; }
		public double Progress { get; private set; }

		public ProjectBuildProcess(BuildPipeline pipeline, ProjectExplorer project, string outputFolder)
		{
			Pipeline = pipeline;
			Project = project;
			OutputFolder = outputFolder;
		}

		public void PerformBuild()
		{
			foreach (var reference in Project.Definition.References)
			{
				reference.IncludeInBuild(this, OutputFolder);
			}

			string bpkgPath = Path.Combine(OutputFolder, $"{Project.Definition.Properties.Name}.bpkg");

			using var fileStream = new FileStream(bpkgPath, FileMode.Create, FileAccess.Write);
			using var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, false);

			var manifest = archive.CreateEntry("definition.json");
			using (var zipStream = manifest.Open())
			{
				var packageDefinition = new PackageDefinition(new PackageDefinitionProperties()
				{
					Name = Project.Definition?.Properties.Name,
					Version = Project.Definition?.Properties.Version
				});

				string json = JsonConvert.SerializeObject(packageDefinition);
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
				string entryName = $"data/{resource.FullName}";

				Pipeline.BuildActions.OnBeforeExportResource(this, resource);

				ZipArchiveEntry entry;
				if (exporter == null)
				{
					entry = archive.CreateEntryFromFile(resource.FileInfo.FullName, entryName, CompressionLevel.Optimal);
				}
				else
				{
					entry = archive.CreateEntry(entryName);

					using var zipStream = entry.Open();
					exporter.BuildResource(resource, zipStream);
				}

				currentProgress += resource.UncompressedSize;
				Progress = currentProgress / (double)Project.UncompressedSize;

				Pipeline.BuildActions.OnAfterExportResource(this, resource);
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
