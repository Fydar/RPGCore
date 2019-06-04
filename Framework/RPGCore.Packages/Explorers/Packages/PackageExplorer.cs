using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RPGCore.Packages
{
    public class PackageExplorer : IPackageExplorer
    {
        private class PackageAssetCollection : IPackageAssetCollection
        {
            private Dictionary<string, PackageAsset> items;

            public PackageAsset this[string key]
            {
                get
                {
                    return items[key];
                }
            }

            public void Add(PackageAsset asset)
            {
                if (items == null)
                    items = new Dictionary<string, PackageAsset>();
                items.Add(asset.ToString(), asset);
            }

            public IEnumerator<PackageAsset> GetEnumerator()
            {
                return items.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return items.Values.GetEnumerator();
            }
        }

        private ProjectDefinitionFile bProj;
        private string Path;

        public string Name => bProj.Properties.Name;
        public string Version => bProj.Properties.Version;
        public IPackageAssetCollection Assets { get; private set; }

        public PackageExplorer()
        {
            Assets = new PackageAssetCollection();
        }

        public void Dispose()
        {

        }

        public Stream LoadStream(string packageKey)
        {
            var fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true);

            var entry = archive.GetEntry(packageKey);

            byte[] buffer = new byte[entry.Length];
            var zipStream = entry.Open();

            return new PackageStream(zipStream, fileStream, archive);
        }

        public byte[] OpenAsset(string packageKey)
        {
            using (var fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true))
            {
                var entry = archive.GetEntry(packageKey);

                byte[] buffer = new byte[entry.Length];
                using (var zipStream = entry.Open())
                {
                    zipStream.Read(buffer, 0, (int)entry.Length);
                    return buffer;
                }
            }
        }

        public static PackageExplorer Load(string path)
        {
            var package = new PackageExplorer
            {
                Path = path
            };
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true))
                {
                    var entry = archive.GetEntry("Main.bmft");

                    byte[] buffer = new byte[entry.Length];
                    using (var zipStream = entry.Open())
                    {
                        zipStream.Read(buffer, 0, (int)entry.Length);
                        string json = Encoding.UTF8.GetString(buffer);
                    }

                    string pathPrefix = null;
                    var pathEntries = new List<ZipArchiveEntry>();
                    foreach (var projectEntry in archive.Entries)
                    {
                        int pathPrefixIndex = projectEntry.FullName.IndexOf('/');
                        if (pathPrefixIndex == -1)
                        {
                            Console.WriteLine("Not adding \"" + projectEntry.FullName + "\" as an item.");
                            continue;
                        }
                        string newPathIndex = projectEntry.FullName.Substring(0, pathPrefixIndex);

                        if (pathPrefix == null)
                            pathPrefix = newPathIndex;

                        if (pathPrefix != newPathIndex)
                        {
                            var asset = new PackageAsset(package, pathPrefix, pathEntries.ToArray());
                            pathEntries.Clear();
                            package.Assets.Add(asset);
                            pathPrefix = newPathIndex;
                        }
                        pathEntries.Add(projectEntry);
                    }
                    if (pathEntries.Count != 0)
                    {
                        var asset = new PackageAsset(package, pathPrefix, pathEntries.ToArray());
                        package.Assets.Add(asset);
                    }
                }
                fileStream.Close();
            }

            return package;
        }
    }
}
