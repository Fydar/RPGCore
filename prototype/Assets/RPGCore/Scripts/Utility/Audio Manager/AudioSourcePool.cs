using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Audio
{
	[Serializable]
	public class AudioSourcePool
	{
		public int PrewarmAmount = 20;
		public List<AudioSource> Pool = new List<AudioSource> ();

		private int currentGrabIndex = 0;

		[NonSerialized]
		private GameObject holder;

		public void Initialise (GameObject _holder)
		{
			holder = _holder;

			for (int i = 0; i < PrewarmAmount; i++)
			{
				ExpandPool ();
			}
		}

		public AudioSource Grab ()
		{
			if (Pool.Count == currentGrabIndex)
				ExpandPool ();

			AudioSource item = Pool[currentGrabIndex];
			item.enabled = true;
			currentGrabIndex++;

			return item;
		}

		public void Flush ()
		{
			foreach (AudioSource item in Pool)
				item.enabled = false;

			currentGrabIndex = 0;
		}

		public void Return (AudioSource item)
		{
			int itemIndex = Pool.IndexOf (item);

			if (itemIndex == -1)
				Debug.LogError ("Item being returned to the pool doesn't belong in it.");

			Pool.RemoveAt (itemIndex);
			Pool.Add (item);
			item.enabled = false;
			currentGrabIndex--;
		}

		private void ExpandPool ()
		{
			Pool.Add (holder.AddComponent<AudioSource> ());
		}
	}
}

