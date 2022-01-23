using RPGCore.FileTree;
using RPGCore.FileTree.FileSystem;
using RPGCore.FileTree.Packed;
using RPGCore.Packages;
using RPGCore.Projects.Pipeline;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace RPGCore.Projects;

/// <summary>
/// <para>Used for loading the content of an uncompiled package.</para>
/// </summary>
/// <remarks>
/// <para>Can be used to analyse, but not modify the content, of a package.</para>
/// </remarks>
public sealed class ProjectExplorer : IExplorer
{
	public ImportPipeline ImportPipeline { get; private set; }

	/// <summary>
	/// <para>The source for this project explorer.</para>
	/// </summary>
	public IArchive Archive { get; private set; }

	/// <summary>
	/// <para>The size of all of the projects resources on disk.</para>
	/// </summary>
	public long UncompressedSize { get; private set; }

	/// <summary>
	/// <para>The project definition for this project.</para>
	/// </summary>
	public ProjectDefinition Definition { get; private set; }

	/// <summary>
	/// <para>A collection of all of the resources contained in this project.</para>
	/// </summary>
	public ProjectResourceCollection Resources { get; private set; }

	/// <summary>
	/// <para>An index of the tags contained within this project for performing asset queries.</para>
	/// </summary>
	public ITagsCollection Tags { get; private set; }

	/// <summary>
	/// <para>A directory representing the root of the project.</para>
	/// </summary>
	public ProjectDirectory RootDirectory { get; private set; }

	[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDefinition IExplorer.Definition => Definition;
	[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceCollection IExplorer.Resources => Resources;
	[DebuggerBrowsable(DebuggerBrowsableState.Never)] ITagsCollection IExplorer.Tags => Tags;
	[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectory IExplorer.RootDirectory => RootDirectory;

	internal ProjectExplorer()
	{
	}

	public static ProjectExplorer CreateProject(string projectDirectoryPath, ImportPipeline importPipeline)
	{
		var directoryInfo = new DirectoryInfo(projectDirectoryPath);

		// Ensure the directory exists.
		if (!directoryInfo.Exists)
		{
			Directory.CreateDirectory(directoryInfo.FullName);
		}

		string bprojPath = Path.Combine(directoryInfo.FullName, $"{directoryInfo.Name}.bproj");
		File.WriteAllText(bprojPath, "<Project>\r\n\t\r\n</Project>");

		return Load(projectDirectoryPath, importPipeline);
	}

	public static ProjectExplorer Load(string projectPath, ImportPipeline importPipeline)
	{
		return LoadAsync(projectPath, importPipeline).Result;
	}

	public static async Task<ProjectExplorer> LoadAsync(string projectPath, ImportPipeline importPipeline)
	{
		string bprojPath = null;
		if (projectPath.EndsWith(".bproj"))
		{
			bprojPath = projectPath;
			projectPath = new DirectoryInfo(projectPath).Parent.FullName;
		}
		else
		{
			foreach (string rootFile in Directory.EnumerateFiles(projectPath, "*.bproj"))
			{
				bprojPath = rootFile;
				break;
			}
		}

		var sourceArchive = new FileSystemArchive(new DirectoryInfo(projectPath));

		var resources = new Dictionary<string, ProjectResource>();

		var rootDirectiory = new ProjectDirectory(null, null);

		var projectExplorer = new ProjectExplorer
		{
			Archive = sourceArchive,
			Definition = ProjectDefinition.Load(bprojPath),
			Resources = new ProjectResourceCollection(resources),
			RootDirectory = rootDirectiory,
			ImportPipeline = importPipeline
		};

		// Organise temporary directory
		var tempDirectory = new DirectoryInfo(Path.Combine(projectPath, "../temp"));
		tempDirectory.Create();
		var tempArchive = new FileSystemArchive(tempDirectory, false);


		void ApplyUpdate(ProjectResourceUpdate update)
		{
			string[] elements = update.ProjectKey.Split(new char[] { '/' });

			var placementDirectory = rootDirectiory;
			for (int i = 0; i < elements.Length - 1; i++)
			{
				string element = elements[i];

				var parent = placementDirectory;
				placementDirectory = placementDirectory.Directories[element];

				if (placementDirectory == null)
				{
					placementDirectory = new ProjectDirectory(placementDirectory, element);
					parent.Directories.Add(placementDirectory);
				}
			}

			string fileName = elements[elements.Length - 1];

			// Locate or create resource to apply updates to.
			if (!placementDirectory.Resources.TryGetResource(fileName, out var projectResource))
			{
				projectResource = new ProjectResource(projectExplorer, placementDirectory, update.ProjectKey);

				projectExplorer.Resources.Add(projectResource.FullName, projectResource);
				placementDirectory.Resources.Add(projectResource.Name, projectResource);
			}

			// Apply updates to resource
			projectResource.Tags.importerTags.AddRange(update.ImporterTags);

			foreach (var importerDependency in update.Dependencies)
			{
				projectResource.Dependencies.dependencies.Add(
					new ProjectResourceDependency(projectExplorer, importerDependency.Resource, importerDependency.Metadata));
			}

			if (update.FileContent != null)
			{
				projectResource.Content = new ProjectResourceContent(update.FileContent);
			}
			if (update.DeferredContent != null)
			{
				string tempId = Guid.NewGuid().ToString();
				var tempFile = tempArchive.RootDirectory.Files.GetOrCreateFile(tempId);

				using (var tempOutput = tempFile.OpenWrite())
				{
					update.DeferredContent.WriteContentAsync(tempOutput).Wait();
				}
				projectResource.Content = new ProjectResourceContent(tempFile);
			}
		}

		// Import resources
		foreach (var update in importPipeline.ImportDirectory(projectExplorer, sourceArchive.RootDirectory))
		{
			ApplyUpdate(update);
		}

		// Attach dependencies
		foreach (var resource in projectExplorer.Resources)
		{
			foreach (var dependency in resource.Dependencies)
			{
				var dependencyResource = dependency.Resource;

				if (dependencyResource == null)
				{
					Console.WriteLine($"ERROR: Unable to resolve dependency for '{dependency.Key}'");
				}
				else
				{
					dependencyResource.Dependants.dependencies.Add(new ProjectResourceDependency(projectExplorer, resource.FullName, dependency.metdadata));
				}
			}
		}

		// Process Resoures
		var importProcessorContext = new ImportProcessorContext()
		{
			Explorer = projectExplorer
		};
		foreach (var processor in importPipeline.importProcessors)
		{
			var preExistingResources = new List<ProjectResource>();
			foreach (var resource in projectExplorer.Resources)
			{
				preExistingResources.Add(resource);
			}

			int maxThreads = 32;
			var semaphore = new SemaphoreSlim(maxThreads);

			foreach (var resource in preExistingResources)
			{
				if (processor.CanProcess(resource))
				{
					semaphore.Wait();
					_ = Task.Factory.StartNew(() =>
					{
						try
						{
							var updates = processor.ProcessImport(importProcessorContext, resource);

							if (updates != null)
							{
								foreach (var update in updates)
								{
									ApplyUpdate(update);
								}
							}
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
			}

			for (int i = 0; i < maxThreads; i++)
			{
				semaphore.Wait();
			}
		}

		// Size Calculation
		long uncompressedSize = 0;
		foreach (var resource in projectExplorer.Resources)
		{
			uncompressedSize += resource.Content.UncompressedSize;
		}
		projectExplorer.UncompressedSize = uncompressedSize;

		// Tag Indexing
		var tags = new Dictionary<string, IResourceCollection>();
		foreach (var resource in projectExplorer.Resources)
		{
			foreach (string tag in resource.Tags)
			{
				if (!tags.TryGetValue(tag, out var taggedResourcesCollection))
				{
					taggedResourcesCollection = new ProjectResourceCollection();
					tags[tag] = taggedResourcesCollection;
				}

				var taggedResources = (ProjectResourceCollection)taggedResourcesCollection;
				taggedResources.Add(resource.FullName, resource);
			}
		}

		projectExplorer.Tags = new ProjectTagsCollection(tags);

		return projectExplorer;
	}

	public void Dispose()
	{
	}

	public FileInfo ExportZippedToDirectory(BuildPipeline pipeline, string path)
	{
		var directoryInfo = new DirectoryInfo(path);
		if (directoryInfo.Exists)
		{
			directoryInfo.Delete(true);
		}
		directoryInfo.Create();

		var buildProcess = new ProjectBuildProcess(pipeline, this, directoryInfo.FullName);

		string packageFile = Path.Combine(directoryInfo.FullName, $"{Definition.Properties.Name}.bpkg");
		var packageFileInfo = new FileInfo(packageFile);

		using var fileStream = new FileStream(packageFile, FileMode.Create, FileAccess.Write);
		using var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create, false);

		var archive = new PackedArchive(zipArchive);
		buildProcess.PerformBuild(archive.RootDirectory);

		return packageFileInfo;
	}

	public DirectoryInfo ExportFoldersToDirectory(BuildPipeline pipeline, string path)
	{
		var directoryInfo = new DirectoryInfo(path);
		if (directoryInfo.Exists)
		{
			directoryInfo.Delete(true);
		}
		directoryInfo.Create();

		var buildProcess = new ProjectBuildProcess(pipeline, this, directoryInfo.FullName);

		string folderPath = Path.Combine(directoryInfo.FullName, $"{Definition.Properties.Name}");
		var folderInfo = new DirectoryInfo(folderPath);
		var archive = new FileSystemArchive(folderInfo, false);
		buildProcess.PerformBuild(archive.RootDirectory);

		return folderInfo;
	}
}
