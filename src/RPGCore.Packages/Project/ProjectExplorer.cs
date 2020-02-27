using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
		/// <para>The project definition for this project.</para>
		/// </summary>
		public ProjectDefinitionFile Definition { get; private set; }

		/// <summary>
		/// <para>The path of the project folder on disk.</para>
		/// </summary>
		public string ProjectPath { get; private set; }

		/// <summary>
		/// <para>The size of all of the projects resources on disk.</para>
		/// </summary>
		public long UncompressedSize { get; private set; }

		/// <summary>
		/// <para>The name of this package, specified in it's definition file.</para>
		/// </summary>
		public string Name => Definition?.Properties?.Name;

		/// <summary>
		/// <para>The version of the project, specified in it's definition file.</para>
		/// </summary>
		public string Version => Definition?.Properties?.Version;

		/// <summary>
		/// <para>A collection of all of the resources contained in this project.</para>
		/// </summary>
		public IProjectResourceCollection Resources => resources;

		/// <summary>
		/// <para>An index of the tags contained within this project for performing asset queries.</para>
		/// </summary>
		public ITagsCollection Tags => tags;

		/// <summary>
		/// <para>A directory representing the root of the project.</para>
		/// </summary>
		public PackageDirectory RootDirectory => rootDirectory;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceCollection IExplorer.Resources => resources;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] ITagsCollection IExplorer.Tags => tags;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectory IExplorer.RootDirectory => rootDirectory;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private ProjectResourceCollection resources;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private PackageTagsCollection tags;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private PackageDirectory rootDirectory;

		internal ProjectExplorer()
		{
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
				string[] rootFiles = Directory.GetFiles(projectPath);
				for (int i = 0; i < rootFiles.Length; i++)
				{
					string rootFile = rootFiles[i];
					if (rootFile.EndsWith(".bproj", StringComparison.Ordinal))
					{
						bprojPath = rootFile;
						break;
					}
				}
			}

			var projectExplorer = new ProjectExplorer
			{
				ProjectPath = projectPath,
				Definition = ProjectDefinitionFile.Load(bprojPath),
				resources = new ProjectResourceCollection()
			};

			var ignoredDirectories = new List<string>()
			{
				Path.Combine(projectPath, "bin")
			};

			string normalizedPath = projectPath.Replace('\\', '/');
			long uncompressedSize = 0;

			foreach (string filePath in Directory.EnumerateFiles(projectPath, "*", SearchOption.AllDirectories))
			{
				if (ignoredDirectories.Any(p => filePath.StartsWith(p)))
				{
					continue;
				}

				var resourceFileInfo = new FileInfo(filePath);

				if (resourceFileInfo.Extension == ".bproj")
				{
					continue;
				}

				string projectKey = filePath
					.Replace('\\', '/')
					.Replace(normalizedPath + "/", "");

				var resource = importPipeline.ImportResource(projectExplorer, resourceFileInfo, projectKey);

				projectExplorer.Resources.Add(resource);

				uncompressedSize += resource.UncompressedSize;
			}
			projectExplorer.UncompressedSize = uncompressedSize;

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
					taggedResources.Add(resource);
				}
			}

			projectExplorer.tags = new PackageTagsCollection(tags);

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
