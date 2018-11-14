using UnityEngine;

namespace RPGCore.World
{
	public class ItemDropper : MonoBehaviour
	{
		public ItemDrop DropPrefab;

		private static ItemDropper instance;

		private void Awake ()
		{
			instance = this;
		}

		public static void DropItem (Vector3 position, ItemSurrogate item)
		{
			ItemDrop drop = Instantiate (instance.DropPrefab) as ItemDrop;

			drop.transform.position = position;

			drop.SetItem (item);
		}
	}
}