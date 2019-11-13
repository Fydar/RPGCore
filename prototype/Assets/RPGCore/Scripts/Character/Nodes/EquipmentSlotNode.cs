﻿using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;

namespace RPGCore
{
	[NodeInformation("Character/Equipment Slot")]
	public class EquipmentSlotNode : BehaviourNode
	{
		public EquipmentEntry EquipmentSlot;

		public CharacterInput Target;

		public ItemOutput Item;
		public BoolOutput IsEquipped;

		protected override void OnSetup(IBehaviourContext context)
		{
			var targetInput = Target[context];
			var itemOutput = Item[context];
			var isEquippedOutput = IsEquipped[context];

			Action eventHandler = () =>
			{
				if (targetInput.Value == null)
				{
					itemOutput.Value = null;
					isEquippedOutput.Value = false;
					return;
				}
				var slotItem = targetInput.Value.equipment.Items[EquipmentSlot.Index].Item;
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
			};

			eventHandler();

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
					targetInput.Value.equipment.Items[EquipmentSlot.Index].onAfterChanged += eventHandler;
					eventHandler();
				}

				isActive = true;
			};

			subscriber();

			targetInput.OnBeforeChanged += () =>
			{
				if (targetInput.Value == null)
				{
					return;
				}

				if (isActive)
				{
					targetInput.Value.equipment.Items[EquipmentSlot.Index].onAfterChanged -= eventHandler;
				}
			};

			targetInput.OnAfterChanged += subscriber;
		}

		protected override void OnRemove(IBehaviourContext context)
		{
		}
	}
}
