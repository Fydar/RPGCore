using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;
using System.Collections.Generic;

namespace RPGCore
{
	[NodeInformation ("Item/Armour Input", "Input", OnlyOne = true)]
	public class ArmourInputNode : StatCollectionInputNode<ArmourStatFloatInputCollection, ArmourStatInstanceCollection>
	{

	}
}
