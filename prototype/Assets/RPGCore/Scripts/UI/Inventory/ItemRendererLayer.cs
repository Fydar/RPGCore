using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Inventories
{
	public class ItemRendererLayer : MonoBehaviour
	{
		public static ItemRendererLayer singleton;

		public static List<ItemRendererLayer> IDs = new List<ItemRendererLayer> ();

		public Camera layerCamera;
		public Canvas layerCanvas;

		public Transform objectHolder;

		public int ID = 0;

		private void Awake ()
		{
			singleton = this;

			while (IDs.Count <= ID)
			{
				IDs.Add (null);
			}

			IDs[ID] = this;
		}

		public static ItemRendererLayer Get (int id)
		{
			return IDs[id];
		}
	}
}

