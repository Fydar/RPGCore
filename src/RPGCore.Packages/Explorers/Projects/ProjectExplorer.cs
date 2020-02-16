using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RPGCore.Packages
{
	public sealed class ProjectExplorer : IPackageExplorer
	{
		public long UncompressedSize { get; private set; }

		private sealed class ProjectResourceCollection : IProjectResourceCollection, IResourceCollection
		{
			private Dictionary<string, ProjectResource> Resources;

			public ProjectResource this[string key] => Resources[key];

			IResource IResourceCollection.this[string key] => Resources[key];

			public void Add(ProjectResource folder)
			{
				if (Resources == null)
				{
					Resources = new Dictionary<string, ProjectResource>();
				}

				Resources.Add(folder.FullName, folder);
			}

			public void Add(IResource folder) => throw new NotImplementedException();

			public IEnumerator<ProjectResource> GetEnumerator()
			{
				if (Resources?.Values == null)
				{
					return Enumerable.Empty<ProjectResource>().GetEnumerator();
				}

				return Resources.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				if (Resources?.Values == null)
				{
					return Enumerable.Empty<ProjectResource>().GetEnumerator();
				}

				return Resources.Values.GetEnumerator();
			}

			IEnumerator<IResource> IEnumerable<IResource>.GetEnumerator()
			{
				if (Resources?.Values == null)
				{
					return Enumerable.Empty<ProjectResource>().GetEnumerator();
				}

				return Resources.Values.GetEnumerator();
			}
		}

		public ProjectDefinitionFile Definition;

		public string Name => Definition.Properties.Name;
		public string Version => Definition.Properties.Version;
		public IProjectResourceCollection Resources { get; private set; }

		IResourceCollection IPackageExplorer.Resources => (IResourceCollection)Resources;

		public ProjectExplorer()
		{
			Resources = new ProjectResourceCollection();
		}

		public static ProjectExplorer Load(string path)
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

			var project = new ProjectExplorer
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

				var file = new FileInfo(filePath);

				if (file.Extension == ".bproj")
				{
					continue;
				}

				string packageKey = filePath
					.Replace('\\', '/')
					.Replace(normalizedPath + "/", "");

				var resource = new ProjectResource(packageKey, file);

				project.Resources.Add(resource);

				totalSize += resource.UncompressedSize;
			}
			project.UncompressedSize = totalSize;

			return project;
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
