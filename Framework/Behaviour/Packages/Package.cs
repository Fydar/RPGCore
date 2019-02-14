using System.Collections.Generic;

namespace Behaviour.Packages
{
    public class Package
    {
        public string Name;
        public string Version;
        public PackageDependancy[] Dependancies;
        public Dictionary<string, PackageItem> Items;
    }
}