using UnityEngine;
using System;

namespace RPGCore
{
	[Serializable]
	public class CollectionEntry
	{
		[SerializeField]
		private string field = "";
		[NonSerialized]
		public int entryIndex = -1;

		public string Field
		{
			get
			{
				return field;
			}
			set
			{
				field = value;
				entryIndex = -1;
			}
		}

		public override string ToString ()
		{
			return Field;
		}
	}
}