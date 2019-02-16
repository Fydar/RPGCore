using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;

namespace Behaviour.Packages
{
    public class Package
    {
        public Dictionary<string, PackageItem> Items;

        private BProjModel bProj;

        public string Name => bProj.Name;
        public string Version => bProj.Version;
        public PackageDependancy[] Dependancies => bProj.Dependancies;

        public static Package Load(string path)
        {
            var rootFiles = Directory.GetFiles(path);
            string bprojPath = null;
            for (int i = 0; i < rootFiles.Length; i++)
            {
                string rootFile = rootFiles[i];
                if (rootFile.EndsWith(".bproj"))
                {
                    bprojPath = rootFile;
                    break;
                }
            }

            var package = new Package();
            package.bProj = BProjModel.Load(bprojPath);
            return package;
        }

        public void WritePackage(string path)
        {
            string json = JsonConvert.SerializeObject(this);

            using (var fileStream = new FileStream(path, FileMode.CreateNew))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {
                    var entry = archive.CreateEntry("Main.bmft");

                    using (var zipStream = entry.Open())
                    {
                        var bytes = Encoding.UTF8.GetBytes(json);
                        zipStream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }
    }
}
