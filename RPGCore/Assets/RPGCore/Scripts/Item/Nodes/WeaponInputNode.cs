using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;
using System.Collections.Generic;

namespace RPGCore
{
	[NodeInformation ("Item/Weapon Input", "Input", OnlyOne = true)]
	public class WeaponInputNode : StatCollectionInputNode<WeaponStatFloatInputCollection, WeaponStatInstanceCollection>
	{

	}
}
