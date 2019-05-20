using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RPGCore.Packages
{
	public class ProjectExplorer : IPackageExplorer
	{
		private class ProjectFolderCollection : IProjectAssetCollection
		{
			private Dictionary<string, ProjectAsset> assets;

			public ProjectAsset this[string key]
			{
				get
				{
					return assets[key];
				}
			}

			public void Add (ProjectAsset folder)
			{
				if (assets == null)
					assets = new Dictionary<string, ProjectAsset> ();

				assets.Add (folder.Archive.Name, folder);
			}

			public IEnumerator<ProjectAsset> GetEnumerator ()
			{
				return assets.Values.GetEnumerator ();
			}

			IEnumerator IEnumerable.GetEnumerator ()
			{
				return assets.Values.GetEnumerator ();
			}
		}

		public BProjModel bProj;

		public string Name => bProj.Properties.Name;
		public string Version => bProj.Properties.Version;
		public Reference[] References => bProj.References;
		public IProjectAssetCollection Assets { get; private set; }

		IPackageAssetCollection IPackageExplorer.Assets => (IPackageAssetCollection)Assets;

		public ProjectExplorer ()
		{
			Assets = new ProjectFolderCollection ();
		}

		public static ProjectExplorer Load (string path)
		{
			string[] rootFiles = Directory.GetFiles (path);
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

			string[] directories = Directory.GetDirectories (path);
			foreach (string folder in directories)
			{
				var directoryInfo = new DirectoryInfo (folder);
				var projectFolder = new ProjectAsset (directoryInfo);
				project.Assets.Add (projectFolder);
			}
			return project;
		}

		public void Dispose()
		{

		}

		public void Export (string path)
		{
			using (var fileStream = new FileStream (path, FileMode.Create, FileAccess.Write))
			using (var archive = new ZipArchive (fileStream, ZipArchiveMode.Create, false))
			{
				var manifest = archive.CreateEntry ("Main.bmft");
				using (var zipStream = manifest.Open ())
				{
					string json = JsonConvert.SerializeObject (bProj);
					byte[] bytes = Encoding.UTF8.GetBytes (json);
					zipStream.Write (bytes, 0, bytes.Length);
				}

				foreach (var asset in Assets)
				{
					foreach (var resource in asset.ProjectResources)
					{
						archive.CreateEntryFromFile (resource.Entry.FullName, asset.Archive.Name + "/" + resource.Name, CompressionLevel.Fastest);
						Console.WriteLine ("Exported " + resource);
					}
				}
			}
		}
	}
}
