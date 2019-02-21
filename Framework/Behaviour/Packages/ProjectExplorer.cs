using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RPGCore.Behaviour.Packages
{
	public class ProjectExplorer : IPackageExplorer
	{
		private struct ProjectItemCollection : IPackageItemCollection
		{
			private ProjectExplorer explorer;

			public PackageItem this[string key]
			{
				get
				{
					return explorer.Items[key];
				}
			}

			public ProjectItemCollection (ProjectExplorer explorer)
			{
				this.explorer = explorer;
			}
		}
		
		private readonly Dictionary<string, PackageItem> itemsDictionary;
		private BProjModel bProj;

		public string Name => bProj.Name;
		public string Version => bProj.Version;
		public PackageDependancy[] Dependancies => bProj.Dependancies;
		public IPackageItemCollection Items { get; private set; }

		public ProjectExplorer ()
		{
			Items = new ProjectItemCollection (this);
			itemsDictionary = new Dictionary<string, PackageItem> ();
		}

		public static ProjectExplorer Load (string path)
		{
			var rootFiles = Directory.GetFiles (path);
			string bprojPath = null;
			for (int i = 0; i < rootFiles.Length; i++)
			{
				string rootFile = rootFiles[i];
				if (rootFile.EndsWith (".bproj", System.StringComparison.Ordinal))
				{
					bprojPath = rootFile;
					break;
				}
			}

			var Project = new ProjectExplorer
			{
				bProj = BProjModel.Load (bprojPath)
			};
			return Project;
		}

		public void WriteProject (string path)
		{
			string json = JsonConvert.SerializeObject (this);

			if (File.Exists (path))
				File.Delete (path);

			using (var fileStream = new FileStream (path, FileMode.CreateNew))
			{
				using (var archive = new ZipArchive (fileStream, ZipArchiveMode.Create, true))
				{
					var entry = archive.CreateEntry ("Main.bmft");

					using (var zipStream = entry.Open ())
					{
						var bytes = Encoding.UTF8.GetBytes (json);
						zipStream.Write (bytes, 0, bytes.Length);
					}
				}
			}
		}
	}
}
