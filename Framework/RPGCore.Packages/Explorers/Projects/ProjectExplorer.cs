using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RPGCore.Packages
{
	public class ProjectExplorer
	{
		private class ProjectFolderCollection : IProjectAssetCollection
		{
			private Dictionary<string, ProjectAsset> folders;

			public ProjectAsset this[string key]
			{
				get
				{
					return folders[key];
				}
			}

			public void Add (ProjectAsset folder)
			{
				if (folders == null)
					folders = new Dictionary<string, ProjectAsset> ();

				folders.Add (folder.Archive.Name, folder);
			}

			public IEnumerator<ProjectAsset> GetEnumerator ()
			{
				return folders.Values.GetEnumerator ();
			}

			IEnumerator IEnumerable.GetEnumerator ()
			{
				return folders.Values.GetEnumerator ();
			}
		}

		private BProjModel bProj;

		public string Name => bProj.Name;
		public string Version => bProj.Version;
		public PackageDependancy[] Dependancies => bProj.Dependancies;
		public IProjectAssetCollection Folders { get; private set; }

		public ProjectExplorer ()
		{
			Folders = new ProjectFolderCollection ();
		}

		public static ProjectExplorer Load (string path)
		{
			var rootFiles = Directory.GetFiles (path);
			string bprojPath = null;
			for (int i = 0; i < rootFiles.Length; i++)
			{
				string rootFile = rootFiles[i];
				if (rootFile.EndsWith (".bproj", StringComparison.Ordinal))
				{
					bprojPath = rootFile;
					break;
				}
			}

			var project = new ProjectExplorer
			{
				bProj = BProjModel.Load (bprojPath)
			};

			var directories = Directory.GetDirectories (path);
			foreach (var folder in directories)
			{
				var directoryInfo = new DirectoryInfo (folder);
				var projectFolder = new ProjectAsset (directoryInfo);
				project.Folders.Add (projectFolder);
			}
			return project;
		}

		public void Export (string path)
		{
			if (File.Exists (path))
				File.Delete (path);

			using (var fileStream = new FileStream (path, FileMode.CreateNew))
			{
				using (var archive = new ZipArchive (fileStream, ZipArchiveMode.Create, true))
				{
					var manifest = archive.CreateEntry ("Main.bmft");
					using (var zipStream = manifest.Open ())
					{
						string json = JsonConvert.SerializeObject (bProj);
						var bytes = Encoding.UTF8.GetBytes (json);
						zipStream.Write (bytes, 0, bytes.Length);
					}

					foreach (var folder in Folders)
					{
						foreach (var asset in folder.Assets)
						{
							archive.CreateEntryFromFile (asset.Entry.FullName, folder.Archive.Name + "/" + asset.ToString (), CompressionLevel.Fastest);
							Console.WriteLine ("Exported " + asset);
						}
					}
				}
			}
		}
	}
}
