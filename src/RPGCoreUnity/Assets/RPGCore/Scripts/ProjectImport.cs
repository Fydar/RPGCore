using Portfolio.Pipeline;
using RPGCore.Demo.BoardGame;
using RPGCore.Packages;
using RPGCore.Packages.Extensions.MetaFiles;
using System.IO;
using UnityEngine;

namespace RPGCoreUnity
{
	[CreateAssetMenu(menuName = "RPGCore/Package Import")]
	public class ProjectImport : ScriptableObject
	{
		public string RelativePath;
		public string ImportPipelineName;
		public string OutputPath => Path.Combine(RelativePath, "bin/build.bpkg");

		private ProjectExplorer sourceFiles;
		private PackageExplorer lastBuild;

		public ProjectExplorer SourceFiles
		{
			get
			{
				if (sourceFiles == null)
				{
					ImportPipeline importPipeline;
					if (ImportPipelineName == "Portfolio")
					{
						importPipeline = PortfolioPipelines.Import;
					}
					else
					{
						importPipeline = ImportPipeline.Create()
						.UseProcessor(new BoardGameResourceImporter())
						.UseJsonMetaFiles()
						.Build();
					}

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
					lastBuild = PackageExplorer.LoadFromFileAsync(OutputPath).Result;


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
