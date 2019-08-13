using RPGCore.Inventories;
using UnityEngine;

namespace RPGCore.World
{
	public class ItemDrop : MonoBehaviour
	{
		private ItemSurrogate item;

		public void SetItem (ItemSurrogate _item)
		{
			item = _item;

			GameObject render = Instantiate (item.Template.RenderPrefab, transform) as GameObject;

			render.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			render.transform.position = transform.position;
		}

		private void OnTriggerEnter (Collider other)
		{
			RPGCharacter character = other.GetComponent<RPGCharacter> ();

			AddResult result = character.inventory.Add (item);

			if (result == AddResult.Complete)
				Destroy (gameObject);
		}
	}
}

