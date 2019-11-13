using RPGCore.Behaviour;
using RPGCore.Stats;

namespace RPGCore
{
	[NodeInformation("Item/Read Weapon Stat", "Attribute")]
	public class WeaponStatsNode : StatCollectionReadNode<WeaponStatFloatInputCollection,
		WeaponStatInstanceCollection, WeaponStatEntry, WeaponInputNode>
	{
	}
}
