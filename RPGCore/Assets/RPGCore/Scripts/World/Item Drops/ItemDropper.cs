using UnityEngine;

namespace RPGCore.World
{
	public class ItemDropper : MonoBehaviour
	{
		private static ItemDropper instance;

		public ItemDrop DropPrefab;
		
		private void Awake ()
		{
			instance = this;
		}

		public static void DropItem (Vector3 position, ItemSurrogate item)
		{
			if (instance == null)
			{
				Debug.LogError ("There is no ItemDropper in the world.");
				return;
			}

			ItemDrop drop = Instantiate (instance.DropPrefab) as ItemDrop;

			drop.transform.position = position;

			drop.SetItem (item);
		}
	}
}
