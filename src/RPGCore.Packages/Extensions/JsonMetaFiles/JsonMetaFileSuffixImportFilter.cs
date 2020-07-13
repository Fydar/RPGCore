using System.IO;

namespace RPGCore.Packages.Extensions.MetaFiles
{
	internal class JsonMetaFileSuffixImportFilter : ImportFilter
	{
		private readonly string suffix;

		public JsonMetaFileSuffixImportFilter(string suffix)
		{
			this.suffix = suffix;
		}

		public override bool AllowFile(FileInfo file)
		{
			return !file.Name.EndsWith(suffix);
		}
	}
}
