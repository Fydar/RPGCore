using System.Collections.Generic;

namespace Packages
{
    public struct PackageItem
    {
        public string Name;
        public string Description;
        public string Type;
        public Dictionary<string, string> CustomData;
    }
}