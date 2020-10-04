using RPGCore.Packages.Archives;

namespace RPGCore.Packages.Extensions.MetaFiles
{
	internal class JsonMetaFileSuffixImportFilter : ImportFilter
	{
		private readonly string suffix;

		public JsonMetaFileSuffixImportFilter(string suffix)
		{
			this.suffix = suffix;
		}

		public override bool AllowFile(IArchiveFile archiveEntry)
		{
			return !archiveEntry.Name.EndsWith(suffix);
		}
	}
}
