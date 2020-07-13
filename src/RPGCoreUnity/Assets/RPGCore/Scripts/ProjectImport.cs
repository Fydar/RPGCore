using RPGCore.Demo.BoardGame;
using RPGCore.Packages;
using RPGCore.Packages.Extensions.MetaFiles;
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
					var importPipeline = ImportPipeline.Create()
						.UseProcessor(new BoardGameResourceImporter())
						.UseJsonMetaFiles()
						.Build();

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
