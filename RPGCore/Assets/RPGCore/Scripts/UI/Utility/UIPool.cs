using RPGCore.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.UI
{
	[Serializable] public class RectTransformPool : UIPool<RectTransform> { }
	[Serializable] public class ContextButtonPool : UIPool<UIContextButton> { }
	[Serializable] public class ContextFolderPool : UIPool<UIContextFolder> { }

	public class UIPool<T>
		where T : Component
	{
		[ErrorIfNull]
		public T SampleButton;
		public bool Reuse = true;

		public List<T> Pool = new List<T> ();
		private int currentGrabIndex = 0;

		public T Grab (Transform parent)
		{
			if (Pool.Count == 0)
			{
				if (Reuse)
				{
					SampleButton.gameObject.SetActive (false);
					Pool.Add (SampleButton);
				}
				else
				{
					SampleButton.gameObject.SetActive (false);
				}
			}

			if (Pool.Count == currentGrabIndex)
				ExpandPool (parent);

			T item = Pool[currentGrabIndex];
			item.gameObject.SetActive (true);
			if (item.transform.parent != parent)
			{
				item.transform.SetParent (parent);
			}
			else
			{
				item.transform.SetAsLastSibling ();
			}
			currentGrabIndex++;

			return item;
		}

		public void Flush ()
		{
			if (Pool.Count == 0)
			{
				if (Reuse)
					Pool.Add (SampleButton);
				else
					SampleButton.gameObject.SetActive (false);
			}

			foreach (T item in Pool)
				item.gameObject.SetActive (false);

			currentGrabIndex = 0;
		}

		public void Return (T item)
		{
			int itemIndex = Pool.IndexOf (item);

			if (itemIndex == -1)
				Debug.LogError ("Item being returned to the pool doesn't belong in it.");

			Pool.RemoveAt (itemIndex);
			Pool.Add (item);
			item.gameObject.SetActive (false);
			currentGrabIndex--;
		}

		private void ExpandPool (Transform parent)
		{
			GameObject clone = GameObject.Instantiate (SampleButton.gameObject, parent) as GameObject;
			clone.transform.localScale = Vector3.one;
			clone.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;

			T button = clone.GetComponent<T> ();
			Pool.Add (button);
		}
	}
}