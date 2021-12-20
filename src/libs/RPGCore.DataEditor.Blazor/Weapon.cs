using RPGCore.Data;
using System;
using System.Collections.Generic;

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
		public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>()
		{
			["Test1"] = "Tested Value"
		};

		public Dictionary<int, string> LevelData { get; set; } = new Dictionary<int, string>()
		{
			[13] = "Generated"
		};
	}

	[EditableType]
	public class WeaponSlot
	{
		public string SlotName { get; set; } = string.Empty;
	}
}
