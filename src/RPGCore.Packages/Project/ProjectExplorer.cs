using RPGCore.Packages.Archives;
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
			File.WriteAllText(bprojPath,
				@"<Project>
	
</Project>");

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
			ProjectDirectory ForPath(string path)
			{
				int currentIndex = 0;
				var currentDirectory = rootDirectiory;
				while (true)
				{
					int nextIndex = path.IndexOf('/', currentIndex);
					if (nextIndex == -1)
					{
						break;
					}
					string segment = path.Substring(currentIndex, nextIndex - currentIndex);

					bool found = false;
					foreach (var directory in currentDirectory.Directories)
					{
						if (directory.Name == segment)
						{
							currentDirectory = directory;
							found = true;
							break;
						}
					}
					if (!found)
					{
						var newDirectory = new ProjectDirectory(segment, path, currentDirectory);
						currentDirectory.Directories.Add(newDirectory);
						currentDirectory = newDirectory;
					}

					currentIndex = nextIndex + 1;
				}

				return currentDirectory;
			}

			var projectExplorer = new ProjectExplorer
			{
				Archive = archive,
				Definition = ProjectDefinition.Load(bprojPath),
				Resources = new ProjectResourceCollection(resources),
				RootDirectory = rootDirectiory
			};

			// Resources
			foreach (var projectEntry in archive.Files)
			{
				if (projectEntry.FullName.StartsWith("bin/")
					|| projectEntry.FullName.EndsWith(".bproj"))
				{
					continue;
				}

				if (!importPipeline.IsResource(projectEntry))
				{
					continue;
				}

				var forPath = ForPath(projectEntry.FullName);

				var resource = importPipeline.ImportResource(projectExplorer, forPath, projectEntry, projectEntry.FullName);

				projectExplorer.Resources.Add(resource.FullName, resource);
				forPath.Resources.Add(resource.Name, resource);
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
			buildProcess.PerformBuild(archive);
		}

		public void ExportFoldersToDirectory(BuildPipeline pipeline, string path)
		{
			var directoryInfo = new DirectoryInfo(path);
			directoryInfo.Create();

			var buildProcess = new ProjectBuildProcess(pipeline, this, directoryInfo.FullName);

			string folderPath = Path.Combine(directoryInfo.FullName, $"{Definition.Properties.Name}");
			var archive = new FileSystemArchive(new DirectoryInfo(folderPath));
			buildProcess.PerformBuild(archive);
		}
	}
}
