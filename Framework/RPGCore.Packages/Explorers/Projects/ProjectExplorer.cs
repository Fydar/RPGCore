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
        public long UncompressedSize { get; private set; }

        private class ProjectResourceCollection : IProjectResourceCollection
        {
            private Dictionary<string, ProjectResource> resources;

            public ProjectResource this[string key]
            {
                get
                {
                    return resources[key];
                }
            }

            public void Add(ProjectResource folder)
            {
                if (resources == null)
                    resources = new Dictionary<string, ProjectResource>();

                resources.Add(folder.FullName, folder);
            }

            public IEnumerator<ProjectResource> GetEnumerator()
            {
                return resources.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return resources.Values.GetEnumerator();
            }
        }

        public ProjectDefinitionFile Definition;

        public string Name => Definition.Properties.Name;
        public string Version => Definition.Properties.Version;
        public IProjectResourceCollection Resources { get; private set; }

        public List<ResourceImporter> Importers;

        IPackageResourceCollection IPackageExplorer.Resources => (IPackageResourceCollection)Resources;

        public ProjectExplorer()
        {
            Resources = new ProjectResourceCollection();
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
                Definition = ProjectDefinitionFile.Load(bprojPath),
                Importers = importers
            };

            var ignoredDirectories = new List<string>()
            {
                Path.Combine(path, "bin")
            };

            string normalizedPath = path.Replace('\\', '/');
            long totalSize = 0;

            foreach (var filePath in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
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

                long currentProgress = 0;

                foreach (var resource in Resources)
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

                    string entryName = resource.FullName;
                    long size = resource.UncompressedSize;

                    ZipArchiveEntry entry;
                    if (porter == null)
                    {
                        entry = archive.CreateEntryFromFile(resource.Entry.FullName, entryName, CompressionLevel.Optimal);
                    }
                    else
                    {
                        entry = archive.CreateEntry(entryName);

                        using (var zipStream = entry.Open())
                        {
                            porter.BuildResource(resource, zipStream);
                        }
                    }

                    currentProgress += size;

                    double progress = (currentProgress / (double)UncompressedSize) * 100;

                    Console.WriteLine($"{progress:0.0}% Exported {entryName}, {size:#,##0} bytes");
                }
            }
        }
    }
}
