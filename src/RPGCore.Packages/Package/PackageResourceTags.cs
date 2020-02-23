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
		private IReadOnlyList<string> TagsInternal;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => TagsInternal?.Count ?? 0;

		public PackageResourceTags(IReadOnlyDictionary<string, IReadOnlyList<string>> tagsDocument, PackageResource resource)
		{
			var tags = new List<string>();

			foreach (var tagCollection in tagsDocument)
			{
				if (tagCollection.Value.Contains(resource.FullName))
				{
					tags.Add(tagCollection.Key);
				}
			}

			TagsInternal = tags;
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
