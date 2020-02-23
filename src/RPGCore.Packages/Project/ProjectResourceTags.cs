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
		private readonly List<string> tags;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => tags?.Count ?? 0;

		public ProjectResourceTags(ProjectResourceImporter projectResourceImporter)
		{
			tags = projectResourceImporter.Tags;
		}

		public IEnumerator<string> GetEnumerator()
		{
			return tags == null
				? Enumerable.Empty<string>().GetEnumerator()
				: tags.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
