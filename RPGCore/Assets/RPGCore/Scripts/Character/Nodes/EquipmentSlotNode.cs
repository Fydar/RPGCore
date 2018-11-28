using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Inventories;
using System;

namespace RPGCore
{
	[NodeInformation ("Character/Equipment Slot")]
	public class EquipmentSlotNode : BehaviourNode
	{
		[CollectionType (typeof (EquipmentCollection<>))]
		public CollectionEntry EquipmentSlot;

		public CharacterInput Target;

		public ItemOutput Item;
		public BoolOutput IsEquipped;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<RPGCharacter> targetInput = Target[context];
			ConnectionEntry<ItemSurrogate> itemOutput = Item[context];
			ConnectionEntry<bool> isEquippedOutput = IsEquipped[context];


			Action eventHandler = () =>
			{
				var slotItem = targetInput.Value.equipment.Items[EquipmentSlot.entryIndex].Item;
				if (slotItem != null)
				{
					isEquippedOutput.Value = true;
					itemOutput.Value = slotItem;
				}
				else
				{
					isEquippedOutput.Value = false;
					itemOutput.Value = null;
				}
			};/*

			bool isActive = false;
			Action subscriber = () =>
			{
				if (targetInput.Value == null)
				{
					isActive = false;
					return;
				}

				if (!isActive)
				{
					targetInput.Value.States.CurrentHealth.OnValueChanged += eventHandler;
				}

				isActive = true;
			};

			subscriber ();

			targetInput.OnBeforeChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				if (isActive)
					targetInput.Value.States.CurrentHealth.OnValueChanged -= eventHandler;
			};

			targetInput.OnAfterChanged += subscriber;*/
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}