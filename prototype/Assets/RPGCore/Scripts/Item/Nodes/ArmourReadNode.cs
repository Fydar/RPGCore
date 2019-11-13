using RPGCore.Behaviour;
using RPGCore.Stats;

namespace RPGCore
{
	[NodeInformation("Item/Read Armour Stat", "Attribute")]
	public class ArmourReadNode : StatCollectionReadNode<ArmourStatFloatInputCollection,
		ArmourStatInstanceCollection, ArmourStatEntry, ArmourInputNode>
	{
	}
}
