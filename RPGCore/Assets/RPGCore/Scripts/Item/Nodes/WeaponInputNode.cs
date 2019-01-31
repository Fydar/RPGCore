using RPGCore.Behaviour;
using RPGCore.Stats;

namespace RPGCore
{
	[NodeInformation ("Item/Weapon Input", "Input", OnlyOne = true)]
	public class WeaponInputNode : StatCollectionInputNode<WeaponStatFloatInputCollection, WeaponStatInstanceCollection>
	{
	}
}
