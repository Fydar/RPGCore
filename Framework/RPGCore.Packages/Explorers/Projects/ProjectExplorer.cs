using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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

            public void Add(ProjectAsset folder)
            {
                if (assets == null)
                    assets = new Dictionary<string, ProjectAsset>();

                assets.Add(folder.Archive.Name, folder);
            }

            public IEnumerator<ProjectAsset> GetEnumerator()
            {
                return assets.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return assets.Values.GetEnumerator();
            }
        }

        public ProjectDefinitionFile Definition;

        public string Name => Definition.Properties.Name;
        public string Version => Definition.Properties.Version;
        public IProjectAssetCollection Assets { get; private set; }

        public List<ResourceImporter> Importers;

        IPackageAssetCollection IPackageExplorer.Assets => (IPackageAssetCollection)Assets;

        public ProjectExplorer()
        {
            Assets = new ProjectFolderCollection();
        }

        public static ProjectExplorer Load(string path, List<ResourceImporter> importers)
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

            foreach (var filePath in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
            {
                if (ignoredDirectories.Any(p => filePath.StartsWith(p)))
                {
                    continue;
                }
                
            }

            project.Importers = importers;

            string[] directories = Directory.GetDirectories(path);
            foreach (string folder in directories)
            {
                var directoryInfo = new DirectoryInfo(folder);
                var projectFolder = new ProjectAsset(directoryInfo);
                project.Assets.Add(projectFolder);
            }
            return project;
        }

        public void Dispose()
        {

        }

        public void Export(string path)
        {
            string bpkgPath = Path.Combine(path, Name + ".bpkg");
            foreach (var reference in Definition.References)
            {
                reference.IncludeInBuild(this, path);
            }

            Directory.CreateDirectory(path);

            using (var fileStream = new FileStream(bpkgPath, FileMode.Create, FileAccess.Write))
            using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, false))
            {
                var manifest = archive.CreateEntry("Main.bmft");
                using (var zipStream = manifest.Open())
                {
                    string json = "{\"placeholder\": \"json manifest\"}";
                    //string json = JsonConvert.SerializeObject (Definition);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    zipStream.Write(bytes, 0, bytes.Length);
                }

                foreach (var asset in Assets)
                {
                    foreach (var resource in asset.ProjectResources)
                    {
                        ResourceImporter porter = null;
                        foreach (var importer in Importers)
                        {
                            if (resource.Name.EndsWith("." + importer.ImportExtensions))
                            {
                                porter = importer;
                                break;
                            }
                        }

                        string entryName = asset.Archive.Name + "/" + resource.Name;
                        long size = resource.UncompressedSize;

                        ZipArchiveEntry entry;
                        if (porter == null)
                        {
                            entry = archive.CreateEntryFromFile(resource.Entry.FullName, entryName, CompressionLevel.Fastest);
                        }
                        else
                        {
                            entry = archive.CreateEntry(entryName);

                            using (var zipStream = entry.Open())
                            {
                                porter.BuildResource(resource, zipStream);
                            }
                        }

                        Console.WriteLine($"Exported {entryName}, {size:#,##0} bytes");
                    }
                }
            }
        }
    }
}
