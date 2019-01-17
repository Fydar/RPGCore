using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;

namespace RPGCore
{
    [NodeInformation ("Item/Read Armour Stat", "Attribute")]
	public class ArmourReadNode : StatCollectionReadNode<ArmourStatFloatInputCollection,
		ArmourStatInstanceCollection, ArmourStatEntry, ArmourInputNode>
	{

	}
}
