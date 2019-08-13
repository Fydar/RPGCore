using RPGCore.Behaviour;
using RPGCore.Stats;

namespace RPGCore
{
	[NodeInformation ("Item/Grant Armour Stat", "Attribute")]
	public class ArmourGrantStatsNode : StatCollectionWriteNode<ArmourStatFloatInputCollection,
		ArmourStatInstanceCollection, ArmourStatEntry, ArmourInputNode>
	{
	}
}
