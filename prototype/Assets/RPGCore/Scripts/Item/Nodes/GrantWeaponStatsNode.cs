using RPGCore.Behaviour;
using RPGCore.Stats;

namespace RPGCore
{
	[NodeInformation("Item/Grant Weapon Stat", "Attribute")]
	public class GrantWeaponStatsNode : StatCollectionWriteNode<WeaponStatFloatInputCollection,
		WeaponStatInstanceCollection, WeaponStatEntry, WeaponInputNode>
	{
	}
}
