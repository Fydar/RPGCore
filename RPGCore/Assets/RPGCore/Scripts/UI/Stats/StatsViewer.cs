using RPGCore.UI;
using System;
using UnityEngine;

namespace RPGCore.Stats
{
	[Serializable] public class StatDrawerPool : UIPool<StatDrawer> { }

	public class StatsViewer : MonoBehaviour
	{
		public RPGCharacter Char;

		public StatDrawerPool PrefabDrawer;
		public RectTransform Holder;

		void Start ()
		{
			foreach (StatInstance stat in Char.Stats)
			{
				StatDrawer drawer = PrefabDrawer.Grab (Holder);

				drawer.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
				drawer.transform.localScale = Vector3.one;

				drawer.Setup (stat);
			}
		}
	}
}