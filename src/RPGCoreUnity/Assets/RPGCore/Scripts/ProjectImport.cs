using RPGCore.Demo.BoardGame;
using RPGCore.Packages;
using System.IO;
using UnityEngine;

namespace RPGCore.Unity
{
	[CreateAssetMenu(menuName = "RPGCore/Package Import")]
	public class ProjectImport : ScriptableObject
	{
		public string RelativePath;
		public string OutputPath => Path.Combine(RelativePath, "bin/build.bpkg");

		private ProjectExplorer sourceFiles;
		private PackageExplorer lastBuild;

		public ProjectExplorer SourceFiles
		{
			get
			{
				if (sourceFiles == null)
				{
					var importPipeline = new ImportPipeline();
					importPipeline.ImportProcessors.Add(new BoardGameResourceImporter());

					sourceFiles = ProjectExplorer.Load(RelativePath, importPipeline);
				}
				return sourceFiles;
			}
		}

		public PackageExplorer LastBuild
		{
			get
			{
				if (lastBuild == null)
				{
					lastBuild = PackageExplorer.Load(OutputPath);


				}
				return lastBuild;
			}
		}

		public void Reload()
		{
			sourceFiles = null;
		}
	}
}
