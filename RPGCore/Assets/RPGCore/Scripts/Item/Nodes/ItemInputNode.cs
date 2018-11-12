using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Item Input", "Input")]
	public class ItemInputNode : BehaviourNode
	{
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
	}
}