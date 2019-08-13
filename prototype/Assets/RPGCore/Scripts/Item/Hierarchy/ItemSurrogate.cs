using RPGCore.Behaviour;
using RPGCore.UI;
using RPGCore.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[Serializable]
	public partial class ItemSurrogate : IItemSeed, IBehaviourContext
	{
		public ItemTemplate template;
		public EventField<RPGCharacter> owner = new EventField<RPGCharacter> ();
		public ItemData data = new ItemData ();

		public ItemTier ItemTier;

		[Header ("Enchantments")]
		[SerializeField]
		private Enchantment prefix;
		[SerializeField]
		private Enchantment suffix;
		[SerializeField]
		private List<Enchantment> craftingMods = new List<Enchantment> ();

		public Enchantment Prefix
		{
			get
			{
				return prefix;
			}
			set
			{
				if (prefix != null)
				{
					prefix.Template.RemoveGraph (prefix);
				}

				if (value == null)
				{
					data.prefixData.Reset ();
					prefix = null;
					return;
				}

				data.prefixData = new EnchantmantData (value.Template);
				prefix = value;

				value.Setup (this, data.prefixData);

				if (prefix != null)
				{
					prefix.Template.SetupGraph (prefix);

					IInputNode<ItemSurrogate>[] itemNodes = prefix.Template.GetNodes<IInputNode<ItemSurrogate>> ();
					foreach (var itemNode in itemNodes)
					{
						itemNode.SetTarget (prefix, this);
					}
				}
			}
		}

		public Enchantment Suffix
		{
			get
			{
				return suffix;
			}
			set
			{
				if (suffix != null)
				{
					suffix.Template.RemoveGraph (suffix);
				}

				if (value == null)
				{
					data.suffixData.Reset ();
					suffix = null;
					return;
				}

				data.suffixData = new EnchantmantData (value.Template);
				suffix = value;

				value.Setup (this, data.suffixData);

				if (suffix != null)
				{
					suffix.Template.SetupGraph (suffix);

					IInputNode<ItemSurrogate>[] itemNodes = suffix.Template.GetNodes<IInputNode<ItemSurrogate>> ();
					foreach (var itemNode in itemNodes)
					{
						itemNode.SetTarget (suffix, this);
					}
				}
			}
		}

		public IEnumerable<Enchantment> Enchantments
		{
			get
			{
				yield return Prefix;
				yield return Suffix;

				foreach (Enchantment enchantment in craftingMods)
				{
					yield return enchantment;
				}
			}
		}

		public string BaseName
		{
			get
			{
				return template.BaseName;
			}
		}

		public string FullName
		{
			get
			{
				return ((Prefix != null) ? PrefixName + " " : "") +
					template.BaseName +
					((Suffix != null) ? " " + SuffixName : "");
			}
		}

		public string PrefixName
		{
			get
			{
				if (Prefix != null)
				{
					return Prefix.Affix;
				}
				return "";
			}
		}

		public string SuffixName
		{
			get
			{
				if (Suffix != null)
				{
					return Suffix.Affix;
				}
				return "";
			}
		}

		public string Description
		{
			get
			{
				return template.Description;
			}
		}

		public int Weight
		{
			get
			{
				return template.Weight;
			}
		}

		public ShortEventField Seed
		{
			get
			{
				return data.seed;
			}
		}

		public IEnumerable<float[]> PositiveOverrides
		{
			get
			{
				return template.PositiveOverrides;
			}
		}

		public IEnumerable<float[]> NegativeOverrides
		{
			get
			{
				return template.NegativeOverrides;
			}
		}

		public Sprite Icon
		{
			get
			{
				return template.Icon;
			}
		}

		public int Quantity
		{
			get
			{
				return data.quantity.Value;
			}
			set
			{
				data.quantity.Value = value;
			}
		}

		public ItemTemplate Template
		{
			get
			{
				return template;
			}
			set
			{
				template = value;

				data.seed.ResetEvents ();
				data.experiance.ResetEvents ();
				data.level.ResetEvents ();
				data.quantity.ResetEvents ();

				Suffix = null;
				Prefix = null;
			}
		}

		public Slot EquiptableSlot
		{
			get
			{
				EquiptableItemNode equiptable = template.GetNode<EquiptableItemNode> ();

				if (equiptable == null)
					return Slot.None;

				return equiptable.slot;
			}
		}

		public bool IsActivatable
		{
			get
			{
				return template.GetNode<ActivatableNode> () != null;
			}
		}

		public string RichText
		{
			get
			{
				return "<color=#" + ColorUtility.ToHtmlStringRGBA (Template.Rarity.NameColour) + ">[" + FullName + "]</color>";
			}
		}

		public override string ToString ()
		{
			if (template == null)
				return "Unknown Item";
			return FullName;
		}

		public void TryUse ()
		{
			Chat.Instance.Log (owner.Value.name + " used " + RichText);

			ActivatableNode activatableNode = template.GetNode<ActivatableNode> ();
			activatableNode.TryUse (this, owner.Value);
		}

		public void AddCraftingMod (Enchantment enchantment)
		{
			EnchantmantData newData = new EnchantmantData (enchantment);

			craftingMods.Add (enchantment);
			data.modsData.Add (newData);
		}

		public void RemoveCraftingMod (Enchantment enchantment)
		{
			RemoveCraftingMod (craftingMods.IndexOf (enchantment));
		}

		public void RemoveAllCraftingMods ()
		{
			foreach (Enchantment enchantment in craftingMods)
			{
				RemoveCraftingMod (craftingMods.IndexOf (enchantment));
			}
		}

		public void RemoveCraftingMod (int id)
		{
			Enchantment enchant = craftingMods[id];

			craftingMods.RemoveAt (id);
			data.modsData.RemoveAt (id);
		}
	}
}
