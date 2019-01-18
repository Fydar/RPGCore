using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;

namespace RPGCore
{
	[NodeInformation ("Item/Grant Weapon Stat", "Attribute")]
	public class GrantWeaponStatsNode : StatCollectionWriteNode<WeaponStatFloatInputCollection,
		WeaponStatInstanceCollection, WeaponStatEntry, WeaponInputNode>
	{

	}
}
