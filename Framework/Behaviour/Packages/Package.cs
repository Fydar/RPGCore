using System.Collections.Generic;

namespace Packages
{
    public class Package
    {
        public string Name;
        public string Version;
        public string Sha256;
        public PackageDependancy[] Dependancies;
        public Dictionary<string, PackageItem> Items;
    }
}