using RPGCore.Packages.Archives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	/// <summary>
	/// <para>Used for loading the content of an uncompiled package.</para>
	/// </summary>
	/// <remarks>
	/// <para>Can be used to analyse, but not modify the content, of a package.</para>
	/// </remarks>
	public sealed class ProjectExplorer : IExplorer
	{
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

			var archive = new FileSystemArchive(new DirectoryInfo(projectPath));

			var resources = new Dictionary<string, ProjectResource>();

			var rootDirectiory = new ProjectDirectory("", "", null);

			var projectExplorer = new ProjectExplorer
			{
				Archive = archive,
				Definition = ProjectDefinition.Load(bprojPath),
				Resources = new ProjectResourceCollection(resources),
				RootDirectory = rootDirectiory
			};

			void ImportDirectory(IArchiveDirectory directory, ProjectDirectory projectDirectory)
			{
				foreach (var childDirectory in directory.Directories)
				{
					ImportDirectory(childDirectory,
						new ProjectDirectory(childDirectory.Name, childDirectory.FullName, projectDirectory));
				}

				foreach (var file in directory.Files)
				{
					if (file.FullName.StartsWith("bin/")
						|| file.FullName.EndsWith(".bproj")
						|| !importPipeline.IsResource(file))
					{
						continue;
					}

					var resource = importPipeline.ImportResource(projectExplorer, projectDirectory, file, file.FullName);

					projectExplorer.Resources.Add(resource.FullName, resource);
					projectDirectory.Resources.Add(resource.Name, resource);
				}
			}

			ImportDirectory(archive.RootDirectory, rootDirectiory);

			foreach (var resource in projectExplorer.Resources)
			{
				foreach (var dependency in resource.Dependencies)
				{
					var dependencyResource = dependency.Resource;

					if (dependencyResource == null)
					{
						Console.WriteLine($"ERROR: Unable to resolve dependency for \"{dependency.Key}\"");
					}
					else
					{
						dependencyResource.Dependants.dependencies.Add(new ProjectResourceDependency(projectExplorer, resource.FullName, dependency.metdadata));
					}
				}
			}

			// Size Calculation
			long uncompressedSize = 0;
			foreach (var resource in projectExplorer.Resources)
			{
				uncompressedSize += resource.UncompressedSize;
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

			projectExplorer.Tags = new PackageTagsCollection(tags);

			return projectExplorer;
		}

		public void Dispose()
		{
		}

		public void ExportZippedToDirectory(BuildPipeline pipeline, string path)
		{
			var directoryInfo = new DirectoryInfo(path);
			directoryInfo.Create();

			var buildProcess = new ProjectBuildProcess(pipeline, this, directoryInfo.FullName);


			string bpkgPath = Path.Combine(directoryInfo.FullName, $"{Definition.Properties.Name}.bpkg");

			using var fileStream = new FileStream(bpkgPath, FileMode.Create, FileAccess.Write);
			using var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create, false);

			var archive = new PackedArchive(zipArchive);
			buildProcess.PerformBuild(archive.RootDirectory);
		}

		public void ExportFoldersToDirectory(BuildPipeline pipeline, string path)
		{
			var directoryInfo = new DirectoryInfo(path);
			directoryInfo.Create();

			var buildProcess = new ProjectBuildProcess(pipeline, this, directoryInfo.FullName);

			string folderPath = Path.Combine(directoryInfo.FullName, $"{Definition.Properties.Name}");
			var archive = new FileSystemArchive(new DirectoryInfo(folderPath));
			buildProcess.PerformBuild(archive.RootDirectory);
		}
	}
}
