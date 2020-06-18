using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	public class PackageResourceTags : IResourceTags
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		internal readonly List<string> tags;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => tags?.Count ?? 0;

		internal PackageResourceTags()
		{
			tags = new List<string>();
		}

		public bool Contains(string tag)
		{
			return tags.Contains(tag);
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
