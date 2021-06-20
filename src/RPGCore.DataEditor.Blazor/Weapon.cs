using RPGCore.Data;
using System;

namespace RPGCore.DataEditor.Blazor
{
	[EditableType]
	public class Weapon
	{
		public string Name { get; set; } = "Longsword";
		public int Damage { get; set; } = 20;
		public long Durability { get; set; } = 100;
		public bool IsEnchantable { get; set; } = true;
		public WeaponSlot[] Slots { get; set; } = Array.Empty<WeaponSlot>();
	}

	[EditableType]
	public class WeaponSlot
	{
		public string SlotName { get; set; } = string.Empty;
	}
}
