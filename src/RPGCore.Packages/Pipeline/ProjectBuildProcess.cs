using Newtonsoft.Json;
using RPGCore.Packages.Archives;
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
			var serializer = new JsonSerializer()
			{
				Formatting = Formatting.None
			};

			foreach (var reference in Project.Definition.References)
			{
				reference.IncludeInBuild(this, OutputFolder);
			}

			string bpkgPath = Path.Combine(OutputFolder, $"{Project.Definition.Properties.Name}.bpkg");

			using var fileStream = new FileStream(bpkgPath, FileMode.Create, FileAccess.Write);
			using var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create, false);
			
			var archive = new PackedArchive(zipArchive);


			var manifest = archive.Files.GetFile("definition.json");
			using (var zipStream = manifest.OpenWrite())
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

			var tagsEntry = archive.Files.GetFile("tags.json");
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
				string contentName = $"data/{resource.FullName}";
				string metadataName = $"data/{resource.FullName}.pkgmeta";

				Pipeline.BuildActions.OnBeforeExportResource(this, resource);

				IArchiveEntry contentEntry;
				if (exporter == null)
				{
					// contentEntry = archive.CreateEntryFromFile(resource.Content.ArchiveEntry.FullName, contentName, CompressionLevel.Optimal);

					contentEntry = archive.Files.GetFile(contentName);

					using var stream = resource.Content.LoadStream();
					using var zipStream = contentEntry.OpenWrite();

					stream.CopyTo(zipStream);
				}
				else
				{
					contentEntry = archive.Files.GetFile(contentName);
					exporter.BuildResource(resource, contentEntry);
				}

				var dependencies = new PackageResourceMetadataDependencyModel[resource.Dependencies.Count];
				for (int i = 0; i < dependencies.Length; i++)
				{
					var dependency = resource.Dependencies[i];
					dependencies[i] = new PackageResourceMetadataDependencyModel()
					{
						Resource = dependency.Key
					};
				}
				var metadata = new PackageResourceMetadataModel()
				{
					Dependencies = dependencies
				};

				var metadataEntry = archive.Files.GetFile(metadataName);
				using (var zipStream = metadataEntry.OpenWrite())
				using (var streamWriter = new StreamWriter(zipStream))
				{
					serializer.Serialize(streamWriter, metadata);
				}

				currentProgress += resource.UncompressedSize;
				Progress = currentProgress / (double)Project.UncompressedSize;

				Pipeline.BuildActions.OnAfterExportResource(this, resource);
			}
		}

		private static void WriteJsonDocument(IArchiveEntry entry, object value)
		{
			using var zipStream = entry.OpenWrite();
			using var sr = new StreamWriter(zipStream);
			using var writer = new JsonTextWriter(sr);

			var serializer = new JsonSerializer();
			serializer.Serialize(writer, value);
		}
	}
}
