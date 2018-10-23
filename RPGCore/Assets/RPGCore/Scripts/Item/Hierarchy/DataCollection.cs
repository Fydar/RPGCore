using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[Serializable]
	public class DataCollection : ISerializationCallbackReceiver
	{
		private List<string> Names = new List<string> ();
		private List<DataEntry> Entries = new List<DataEntry> ();

		private Dictionary<string, DataEntry> Collection = new Dictionary<string, DataEntry> ();

		public void OnBeforeSerialize () { }

		public void OnAfterDeserialize ()
		{
			Collection.Clear ();
			for (int i = 0; i < Names.Count; i++)
			{
				string name = Names[i];
				DataEntry entry = Entries[i];
				Collection.Add (name, entry);
			}
		}

		public DataEntry GetElement (string key)
		{
			DataEntry entry;
			Collection.TryGetValue (key, out entry);
			return entry;
		}

		public DataEntry AssureElement (string key)
		{
			DataEntry entry;

			bool result = Collection.TryGetValue (key, out entry);

			if (!result)
			{
				entry = new DataEntry ();

				Names.Add (key);
				Entries.Add (entry);
				Collection.Add (key, entry);
			}

			return entry;
		}
	}
}