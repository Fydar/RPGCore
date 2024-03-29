﻿using RPGCore.Packages;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Projects;

[DebuggerDisplay("Count = {Count,nq}")]
[DebuggerTypeProxy(typeof(ProjectResourceTagsDebugView))]
public class ProjectResourceTags : IResourceTags
{
	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	internal readonly List<string> importerTags;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public int Count => importerTags.Count;

	internal ProjectResourceTags()
	{
		importerTags = new List<string>();
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

	private class ProjectResourceTagsDebugView
	{
		[DebuggerDisplay("{Value}", Name = "{Key,nq}")]
		internal struct DebuggerRow
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			public string Key;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			public string Value;
		}

		private readonly ProjectResourceTags source;

		public ProjectResourceTagsDebugView(ProjectResourceTags source)
		{
			this.source = source;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public DebuggerRow[] Keys
		{
			get
			{
				var keys = new DebuggerRow[source.importerTags.Count];

				int i = 0;
				foreach (string tag in source.importerTags)
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
