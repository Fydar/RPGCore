using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;

namespace RPGCore
{
	[NodeInformation ("Item/Grant Armour Stat", "Attribute")]
	public class ArmourGrantStatsNode : StatCollectionWriteNode<ArmourStatFloatInputCollection,
		ArmourStatInstanceCollection, ArmourStatEntry, ArmourInputNode>
	{

	}
}
