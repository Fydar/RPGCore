using RPGCore.Packages.Pipeline;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	public class ProjectResourceTags : IResourceTags
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private readonly IReadOnlyList<string> importerTags;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => importerTags?.Count ?? 0;

		internal ProjectResourceTags(ProjectResourceImporter projectResourceImporter)
		{
			importerTags = projectResourceImporter.ImporterTags.ToList();
		}

		public bool Contains(string tag)
		{
			return importerTags.Contains(tag);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return importerTags.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
