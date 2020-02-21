using System.Collections.Generic;
using UnityEngine;

namespace RPGCoreUnity.Demo.Browser
{
	public class UIPool<T>
		where T : Component
	{
		public T SampleButton;
		public bool Reuse = true;

		public List<T> Pool = new List<T>();
		private int currentGrabIndex;

		public T Grab(Transform parent)
		{
			if (Pool.Count == 0)
			{
				if (Reuse)
				{
					SampleButton.gameObject.SetActive(false);
					Pool.Add(SampleButton);
				}
				else
				{
					SampleButton.gameObject.SetActive(false);
				}
			}

			if (Pool.Count == currentGrabIndex)
			{
				ExpandPool(parent);
			}

			var item = Pool[currentGrabIndex];
			item.gameObject.SetActive(true);
			if (item.transform.parent != parent)
			{
				item.transform.SetParent(parent);
			}
			else
			{
				item.transform.SetAsLastSibling();
			}
			currentGrabIndex++;

			return item;
		}

		public void Flush()
		{
			if (Pool.Count == 0)
			{
				if (Reuse)
				{
					Pool.Add(SampleButton);
				}
				else
				{
					SampleButton.gameObject.SetActive(false);
				}
			}

			foreach (var item in Pool)
			{
				item.gameObject.SetActive(false);
			}

			currentGrabIndex = 0;
		}

		public void Return(T item)
		{
			int itemIndex = Pool.IndexOf(item);

			if (itemIndex == -1)
			{
				Debug.LogError("Item being returned to the pool doesn't belong in it.");
			}

			Pool.RemoveAt(itemIndex);
			Pool.Add(item);
			item.gameObject.SetActive(false);
			currentGrabIndex--;
		}

		private void ExpandPool(Transform parent)
		{
			var clone = UnityEngine.Object.Instantiate(SampleButton.gameObject, parent) as GameObject;
			clone.transform.localScale = Vector3.one;
			clone.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

			var button = clone.GetComponent<T>();
			Pool.Add(button);
		}
	}
}
