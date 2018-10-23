using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Inventories
{
	public interface IItemCollection
	{
		AddResult Add (ItemSurrogate item);
	}
}