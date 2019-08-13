using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation ("Item/Item Input", "Input", OnlyOne = true)]
	public class ItemInputNode : BehaviourNode, IInputNode<ItemSurrogate>
	{
		public ItemOutput Item;
		public CharacterOutput Owner;
		public EventOutput OnReceive;
		public EventOutput OnLoose;
		public IntOutput StackSize;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<RPGCharacter> ownerOutput = Owner[context];
			EventEntry onReceiveOutput = OnReceive[context];
			EventEntry onLooseOutput = OnLoose[context];
			ConnectionEntry<int> stackSizeOutput = StackSize[context];

			//equippedOutput.Value = character;
			ownerOutput.OnBeforeChanged += () =>
			{
				if (ownerOutput.Value != null)
				{
					onLooseOutput.Invoke ();
				}
			};

			ownerOutput.OnAfterChanged += () =>
			{
				if (ownerOutput.Value != null)
				{
					onReceiveOutput.Invoke ();
				}
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{
			ConnectionEntry<RPGCharacter> equippedOutput = Owner[context];

			equippedOutput.Value = null;
		}

		public void SetTarget (IBehaviourContext context, ItemSurrogate target)
		{
			Item[context].Value = target;

			Owner[context].Value = target.owner.Value;
			target.owner.onChanged += () =>
			{
				Owner[context].Value = target.owner.Value;
			};

			StackSize[context].Value = target.Quantity;
			target.data.quantity.onChanged += () =>
			{
				StackSize[context].Value = target.Quantity;
			};
		}
	}
}
