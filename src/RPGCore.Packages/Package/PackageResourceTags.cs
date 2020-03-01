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
		private readonly IReadOnlyList<string> tags;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => tags?.Count ?? 0;

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

			this.tags = tags;
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
