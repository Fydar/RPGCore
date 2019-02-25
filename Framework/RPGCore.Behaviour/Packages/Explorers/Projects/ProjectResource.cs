using System.IO;

namespace RPGCore.Behaviour.Packages
{
	public struct ProjectResource
	{
		public readonly FileInfo Entry;

		public ProjectResource (FileInfo entry)
		{
			Entry = entry;
		}

		public override string ToString ()
		{
			return Entry.Name;
		}
	}
}
