using UnityEngine;

namespace RPGCore.Inventories
{
	public class EquipmentCollection<T> : EnumerableCollection<T>
	{
		[Header ("Armour")]
		public T Helmet;
		public T Chestplate;
		public T Gauntlets;
		public T Belt;
		public T Boots;

		[Header ("Jewellery")]
		public T Amulet;
		public T LeftRing;
		public T RightRing;

		[Space]
		public T Flask1;
		public T Flask2;
		public T Flask3;
		public T Flask4;
		public T Flask5;

		[Header ("Weaponry")]
		public T MainHand;
		public T OffHand;
	}
}

