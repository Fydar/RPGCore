using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation ("Item/Equiptable Input", "Input", OnlyOne = true)]
	public class EquiptableItemNode : BehaviourNode, IInputNode<ItemSurrogate>
	{
		public Slot slot;

		public BoolOutput Equipped;

		protected override void OnSetup (IBehaviourContext context)
		{
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}

		public void SetTarget (IBehaviourContext context, ItemSurrogate target)
		{
			/*ItemSurrogate item = (ItemSurrogate)context;
			ConnectionEntry<bool> equippedOutput = Equipped[context];

			equippedOutput.Value = target.Equipped.Value;

			target.Equipped.onChanged += () =>
			{
				equippedOutput.Value = character.Equipped.Value;
			};*/
		}
	}
}
