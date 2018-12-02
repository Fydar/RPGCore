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
			ConnectionEntry<RPGCharacter> equippedOutput = Owner[context];
			EventEntry onReceiveOutput = OnReceive[context];
			EventEntry onLooseOutput = OnLoose[context];
			ConnectionEntry<int> stackSizeOutput = StackSize[context];

			//equippedOutput.Value = character;
			equippedOutput.OnBeforeChanged += () =>
			{
				if (equippedOutput.Value != null)
				{
					onLooseOutput.Invoke ();
				}
			};

			equippedOutput.OnAfterChanged += () =>
			{
				if (equippedOutput.Value != null)
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
			Item[target].Value = target;

			Owner[target].Value = target.owner.Value;
			UnityEngine.Debug.Log (context.GetHashCode () + ": " + Owner[target].Value);
			target.owner.onChanged += () =>
			{
				Owner[target].Value = target.owner.Value;
				UnityEngine.Debug.Log (context.GetHashCode() + "; " + Owner[target].Value);
			};

			StackSize[target].Value = target.Quantity;
			target.data.quantity.onChanged += () =>
			{
				StackSize[target].Value = target.Quantity;
			};
		}
	}
}
