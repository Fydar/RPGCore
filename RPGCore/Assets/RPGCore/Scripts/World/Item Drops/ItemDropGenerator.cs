using UnityEngine;

namespace RPGCore.World
{
	[RequireComponent (typeof (ItemDrop))]
	public class ItemDropGenerator : MonoBehaviour
	{
		public ItemGenerator generator;

		private ItemDrop drop;

		private void Awake ()
		{
			drop = GetComponent<ItemDrop> ();
		}

		private void Start ()
		{
			ItemSurrogate item = generator.Generate ();

			drop.SetItem (item);
		}
	}
}