using Newtonsoft.Json;
using RPGCore.FileTree;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RPGCore.Projects
{
	public class ProjectBuildProcess
	{
		private struct ResourceBuildStepResult
		{
			public Dictionary<string, List<string>> Tags { get; internal set; }
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly JsonSerializer serializer;

		public BuildPipeline Pipeline { get; }
		public ProjectExplorer Project { get; }

		public string OutputFolder { get; }
		public double Progress { get; private set; }

		public ProjectBuildProcess(BuildPipeline pipeline, ProjectExplorer project, string outputFolder)
		{
			Pipeline = pipeline;
			Project = project;
			OutputFolder = outputFolder;

			serializer = new JsonSerializer()
			{
				Formatting = Formatting.None
			};
		}

		public void PerformBuild(IArchiveDirectory destination)
		{
			var packageDefinition = new PackageDefinition(
				new PackageDefinitionProperties(
					Project.Definition?.Properties.Name,
					Project.Definition?.Properties.Version));

			var manifest = destination.Files.GetOrCreateFile("definition.json");
			WriteJsonDocument(manifest, packageDefinition);

			var result = BuildResourceStep(destination);

			var tagsEntry = destination.Files.GetOrCreateFile("tags.json");
			WriteJsonDocument(tagsEntry, result.Tags);
		}

		private ResourceBuildStepResult BuildResourceStep(IArchiveDirectory destination)
		{
			var tags = new Dictionary<string, List<string>>();
			foreach (var projectTagCategory in Project.Tags)
			{
				if (!tags.TryGetValue(projectTagCategory.Key, out var taggedResources))
				{
					taggedResources = new List<string>();
					tags[projectTagCategory.Key] = taggedResources;
				}
				taggedResources.AddRange(projectTagCategory.Value.Select(tag => tag.FullName));
			}

			long currentProgress = 0;
			int maxThreads = destination.Archive.MaximumWriteThreads;
			var semaphore = new SemaphoreSlim(maxThreads);
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

				semaphore.Wait();
				_ = Task.Factory.StartNew(() =>
				{
					try
					{
						string contentIdString = contentId.ToString("X8") + processResource.Extension;
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
					}
					catch (Exception exception)
					{
						Console.WriteLine(exception);
					}
					finally
					{
						semaphore.Release();
					}
				}, TaskCreationOptions.LongRunning);
			}

			for (int i = 0; i < maxThreads; i++)
			{
				semaphore.Wait();
			}

			if (failed > 0)
			{
				throw new InvalidOperationException($"Failed to export {failed} resources.");
			}

			return new ResourceBuildStepResult()
			{
				Tags = tags
			};
		}

		private void WriteJsonDocument(IArchiveFile entry, object value)
		{
			using var zipStream = entry.OpenWrite();
			using var sr = new StreamWriter(zipStream);
			using var writer = new JsonTextWriter(sr);

			serializer.Serialize(writer, value);
		}
	}
}
