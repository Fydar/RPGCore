using Newtonsoft.Json;
using RPGCore.FileTree;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPGCore.Projects
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

		public void PerformBuild(IArchiveDirectory destination)
		{
			var serializer = new JsonSerializer()
			{
				Formatting = Formatting.None
			};

			var manifest = destination.Files.GetOrCreateFile("definition.json");
			using (var zipStream = manifest.OpenWrite())
			{
				var packageDefinition = new PackageDefinition(new PackageDefinitionProperties(
					Project.Definition?.Properties.Name,
					Project.Definition?.Properties.Version));

				string json = JsonConvert.SerializeObject(packageDefinition);
				byte[] bytes = Encoding.UTF8.GetBytes(json);
				zipStream.Write(bytes, 0, bytes.Length);
			}

			var tagsEntry = destination.Files.GetOrCreateFile("tags.json");
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
			var maxThread = new SemaphoreSlim(destination.Archive.MaximumWriteThreads);
			var tasks = new List<Task>();
			int failed = 0;

			int contentCounter = 0;
			int resourceCounter = 0;

			var contentsDirectory = destination.Directories.GetOrCreateDirectory("contents");
			var resourcesDirectory = destination.Directories.GetOrCreateDirectory("resources");

			foreach (var scheduleResource in Project.Resources)
			{
				var processResource = scheduleResource;

				int contentId = contentCounter++;
				int resourceId = resourceCounter++;

				maxThread.Wait();
				tasks.Add(Task.Factory.StartNew(() =>
				{
					string contentIdString = contentId.ToString("X8");
					string resourceIdString = resourceId.ToString("X8");

					Pipeline.BuildActions.OnBeforeExportResource(this, processResource);

					// Export Resource Metadata
					PackageResourceMetadataDependencyModel[] dependencies = null;
					if (processResource.Dependencies.Count > 0)
					{
						dependencies = new PackageResourceMetadataDependencyModel[processResource.Dependencies.Count];
						for (int i = 0; i < dependencies.Length; i++)
						{
							var dependency = processResource.Dependencies[i];
							dependencies[i] = new PackageResourceMetadataDependencyModel()
							{
								Resource = dependency.Key
							};
						}
					}
					var metadata = new PackageResourceMetadataModel()
					{
						Name = processResource.Name,
						FullName = processResource.FullName,
						ContentId = contentIdString,
						Dependencies = dependencies
					};

					var metadataEntry = resourcesDirectory.Files.GetOrCreateFile(resourceIdString);
					using (var zipStream = metadataEntry.OpenWrite())
					using (var streamWriter = new StreamWriter(zipStream))
					{
						serializer.Serialize(streamWriter, metadata);
					}

					// Export Resource Contents
					var destinationContent = contentsDirectory.Files.GetOrCreateFile(contentIdString);

					using (var stream = processResource.Content.OpenRead())
					using (var zipStream = destinationContent.OpenWrite())
					{
						stream.CopyTo(zipStream);
					}

					// Post-build
					currentProgress += processResource.Content.UncompressedSize;
					Progress = currentProgress / (double)Project.UncompressedSize;

					Pipeline.BuildActions.OnAfterExportResource(this, processResource);

				}, TaskCreationOptions.LongRunning)
				.ContinueWith((task) =>
				{
					tasks.Remove(task);
					return maxThread.Release();
				}));
			}

			Task.WaitAll(tasks.ToArray());

			if (failed > 0)
			{
				throw new InvalidOperationException($"Failed to export {failed} resources.");
			}
		}

		private static void WriteJsonDocument(IArchiveFile entry, object value)
		{
			using var zipStream = entry.OpenWrite();
			using var sr = new StreamWriter(zipStream);
			using var writer = new JsonTextWriter(sr);

			var serializer = new JsonSerializer();
			serializer.Serialize(writer, value);
		}
	}
}
