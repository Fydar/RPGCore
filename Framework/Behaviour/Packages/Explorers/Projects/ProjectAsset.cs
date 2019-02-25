using System.IO;

namespace RPGCore.Behaviour.Packages
{
	public struct ProjectAsset
	{
		public readonly FileInfo Entry;

		public ProjectAsset (FileInfo entry)
		{
			Entry = entry;
		}

		public override string ToString ()
		{
			return Entry.Name;
		}
	}
}
