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
		private List<string> TagsInternal;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => TagsInternal?.Count ?? 0;

		public ProjectResourceTags(ProjectResourceImporter projectResourceImporter)
		{
			TagsInternal = projectResourceImporter.Tags;
		}

		public IEnumerator<string> GetEnumerator()
		{
			return TagsInternal == null
				? Enumerable.Empty<string>().GetEnumerator()
				: TagsInternal.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
