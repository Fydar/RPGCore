using RPGCore.Behaviour;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[CreateAssetMenu (menuName = "RPGCore/Enchantment/Template")]
	public class EnchantmentTemplate : EnchantmentSelector, IBehaviourGraph
	{
		public string Affix = "";

		[UnityEngine.Serialization.FormerlySerializedAs ("Nodes")]
		[SerializeField, HideInInspector] private List<BehaviourNode> nodes;

		public List<BehaviourNode> AllNodes
		{
			get
			{
				return nodes;
			}
			set
			{
				nodes = value;
			}
		}

		public override EnchantmentTemplate GetEnchantment ()
		{
			return this;
		}

		public override string ToString ()
		{
			return Affix;
		}
	}
}
