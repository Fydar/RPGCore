using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	[DebuggerDisplay("Count = {Count,nq}")]
	[DebuggerTypeProxy(typeof(PackageResourceTagsDebugView))]
	public class PackageResourceTags : IResourceTags
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		internal readonly List<string> tags;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count => tags.Count;

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
			return tags.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return tags.GetEnumerator();
		}

		private class PackageResourceTagsDebugView
		{
			[DebuggerDisplay("{Value}", Name = "{Key,nq}")]
			internal struct DebuggerRow
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				public string Key;

				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				public string Value;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly PackageResourceTags source;

			public PackageResourceTagsDebugView(PackageResourceTags source)
			{
				this.source = source;
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public DebuggerRow[] Keys
			{
				get
				{
					var keys = new DebuggerRow[source.tags.Count];

					int i = 0;
					foreach (string tag in source.tags)
					{
						keys[i] = new DebuggerRow
						{
							Key = $"[{i}]",
							Value = tag
						};
						i++;
					}
					return keys;
				}
			}
		}
	}
}
