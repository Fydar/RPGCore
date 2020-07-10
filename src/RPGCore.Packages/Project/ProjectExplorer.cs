using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
		/// <para>The path of the project folder on disk.</para>
		/// </summary>
		public string ProjectPath { get; private set; }

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

		private static ProjectDirectory CreateDirectory(string normalisedRootPath, string directoryPath, string projectKey, ProjectExplorer projectExplorer, ImportPipeline importPipeline)
		{
			var newRootDirectory = new ProjectDirectory(new DirectoryInfo(directoryPath), projectKey);

			foreach (string filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.TopDirectoryOnly))
			{
				var resourceFileInfo = new FileInfo(filePath);

				if (resourceFileInfo.Extension == ".bproj")
				{
					continue;
				}

				string resourceProjectKey = filePath
					.Replace('\\', '/')
					.Replace(normalisedRootPath + "/", "");

				var resource = importPipeline.ImportResource(projectExplorer, resourceFileInfo, resourceProjectKey);

				projectExplorer.Resources.Add(resource.FullName, resource);

				newRootDirectory.AddChildResource(resource);
			}

			foreach (string directoryFullName in Directory.EnumerateDirectories(directoryPath, "*", SearchOption.TopDirectoryOnly))
			{
				var directoryInformation = new DirectoryInfo(directoryFullName);

				if (directoryInformation.Name == "bin")
				{
					continue;
				}

				string directoryProjectKey = directoryFullName
					.Replace('\\', '/')
					.Replace(normalisedRootPath + "/", "");

				var childDirectory = CreateDirectory(normalisedRootPath, directoryFullName, directoryProjectKey, projectExplorer, importPipeline);

				newRootDirectory.AddChildDirectory(childDirectory);
			}

			return newRootDirectory;
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

			var resources = new Dictionary<string, ProjectResource>();

			var projectExplorer = new ProjectExplorer
			{
				ProjectPath = projectPath,
				Definition = ProjectDefinition.Load(bprojPath),
				Resources = new ProjectResourceCollection(resources)
			};

			projectExplorer.RootDirectory = CreateDirectory(
				projectPath.Replace('\\', '/'),
				projectPath,
				"",
				projectExplorer,
				importPipeline
			);

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

		public void Export(BuildPipeline pipeline, string path)
		{
			Directory.CreateDirectory(path);

			var buildProcess = new ProjectBuildProcess(pipeline, this, path);

			buildProcess.PerformBuild();
		}
	}
}
