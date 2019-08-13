using RPGCore.Behaviour;
using RPGCore.Stats;

namespace RPGCore
{
	[NodeInformation ("Item/Armour Input", "Input", OnlyOne = true)]
	public class ArmourInputNode : StatCollectionInputNode<ArmourStatFloatInputCollection, ArmourStatInstanceCollection>
	{
	}
}
