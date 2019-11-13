using RPGCore.Audio;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation("Item/Activatable Input", "Input", OnlyOne = true)]
	public class ActivatableNode : BehaviourNode
	{
		public IntInput ManaCost;
		public IntInput QuantityCost;
		public IntInput ChargesCost;
		public EventOutput OnActivate;
		public SfxGroup ActivateSound;

		protected override void OnSetup(IBehaviourContext context)
		{
		}

		protected override void OnRemove(IBehaviourContext context)
		{
		}

		public void TryUse(ItemSurrogate context, RPGCharacter character)
		{
			ConnectionEntry<int> manaCostInput = ManaCost[context];
			ConnectionEntry<int> quantityCostInput = QuantityCost[context];
			var onActivateOutput = OnActivate[context];

			if (CanCharacterUse(context, character))
			{
				context.owner.Value.States.CurrentMana.Value -= manaCostInput.Value;
				context.Quantity -= quantityCostInput.Value;

				onActivateOutput.OnEventFired();

				if (ActivateSound != null)
				{
					AudioManager.Play(ActivateSound);
				}
			}
		}

		public bool CanCharacterUse(ItemSurrogate context, RPGCharacter character)
		{
			ConnectionEntry<int> manaCostInput = ManaCost[context];
			ConnectionEntry<int> quantityCostInput = QuantityCost[context];

			if (context.Quantity < quantityCostInput.Value)
			{
				return false;
			}

			if (context.owner.Value.States.CurrentMana.Value < manaCostInput.Value)
			{
				return false;
			}

			return true;
		}
	}
}
