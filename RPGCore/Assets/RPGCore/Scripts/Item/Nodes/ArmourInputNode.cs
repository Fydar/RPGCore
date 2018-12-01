using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation ("Item/Armour Input", "Input", OnlyOne = true)]
	public class ArmourInputNode : BehaviourNode, IInputNode<ItemSurrogate>
	{
		public FloatInput Armour;

		protected override void OnSetup (IBehaviourContext context)
		{

		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}

		public void SetTarget (IBehaviourContext context, ItemSurrogate target)
		{

		}
	}
}
