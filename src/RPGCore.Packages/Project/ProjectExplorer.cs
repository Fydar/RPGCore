using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RPGCore.Packages
{
	public sealed class ProjectExplorer : IExplorer
	{
		[DebuggerDisplay("Count = {Count,nq}")]
		[DebuggerTypeProxy(typeof(ProjectResourceCollectionDebugView))]
		private sealed class ProjectResourceCollection : IProjectResourceCollection, IResourceCollection
		{
			private Dictionary<string, ProjectResource> Resources;

			public int Count => Resources?.Count ?? 0;

			public ProjectResource this[string key] => Resources[key];
			IResource IResourceCollection.this[string key] => this[key];

			public void Add(ProjectResource folder)
			{
				if (Resources == null)
				{
					Resources = new Dictionary<string, ProjectResource>();
				}

				Resources.Add(folder.FullName, folder);
			}

			public IEnumerator<ProjectResource> GetEnumerator()
			{
				return Resources?.Values == null
					? Enumerable.Empty<ProjectResource>().GetEnumerator()
					: Resources.Values.GetEnumerator();
			}

			IEnumerator<IResource> IEnumerable<IResource>.GetEnumerator()
			{
				return GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			private class ProjectResourceCollectionDebugView
			{
				[DebuggerDisplay("{Value}", Name = "{Key}")]
				internal struct DebuggerRow
				{
					public string Key;

					[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
					public IResource Value;
				}

				private readonly ProjectResourceCollection Source;

				public ProjectResourceCollectionDebugView(ProjectResourceCollection source)
				{
					Source = source;
				}

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public DebuggerRow[] Keys
				{
					get
					{
						var keys = new DebuggerRow[Source.Resources.Count];

						int i = 0;
						foreach (var kvp in Source.Resources)
						{
							keys[i] = new DebuggerRow
							{
								Key = kvp.Key,
								Value = kvp.Value
							};
							i++;
						}
						return keys;
					}
				}
			}
		}

		public ProjectDefinitionFile Definition;

		public string Name => Definition.Properties.Name;
		public string Version => Definition.Properties.Version;

		public long UncompressedSize { get; private set; }

		public IProjectResourceCollection Resources => ResourcesInternal;
		public ITagsCollection Tags => TagsInternal;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceCollection IExplorer.Resources => ResourcesInternal;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] ITagsCollection IExplorer.Tags => TagsInternal;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private ProjectResourceCollection ResourcesInternal;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private PackageTagsCollection TagsInternal;

		public ProjectExplorer()
		{
			ResourcesInternal = new ProjectResourceCollection();
		}

		public static ProjectExplorer Load(string path, ImportPipeline importPipeline)
		{
			string bprojPath = null;
			if (path.EndsWith(".bproj"))
			{
				bprojPath = path;
				path = new DirectoryInfo(path).Parent.FullName;
			}
			else
			{
				string[] rootFiles = Directory.GetFiles(path);
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
				Definition = ProjectDefinitionFile.Load(bprojPath)
			};

			var ignoredDirectories = new List<string>()
			{
				Path.Combine(path, "bin")
			};

			string normalizedPath = path.Replace('\\', '/');
			long totalSize = 0;

			foreach (string filePath in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
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

				totalSize += resource.UncompressedSize;
			}
			projectExplorer.UncompressedSize = totalSize;

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

			projectExplorer.TagsInternal = new PackageTagsCollection(tags);

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
