using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;

namespace RPGCore
{
	[NodeInformation ("Item/Read Armour Stat", "Attribute")]
	public class ArmourReadNode : BehaviourNode
	{
		[CollectionType (typeof (WeaponStatCollection<>))]
		public CollectionEntry Stat;

		public ItemInput Target;
		public FloatOutput Value;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<ItemSurrogate> targetInput = Target[context];
			ConnectionEntry<float> valueInput = Value[context];

			Action updateListener = () =>
			{
				var weaponNode = targetInput.Value.Template.GetNode<WeaponInputNode> ();

				if (weaponNode == null)
				{
					valueInput.Value = 0;
					return;
				}

				var localStatInput = weaponNode.GetStat (context, Stat);
				
				valueInput.Value = localStatInput.Value;
			};

			if (targetInput.Value != null)
			{
				targetInput.Value.Template.GetNode<ArmourInputNode> ()
					.Armour[targetInput.Value].OnAfterChanged += updateListener;

				updateListener ();
			}

			targetInput.OnBeforeChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				targetInput.Value.Template.GetNode<ArmourInputNode> ()
					.Armour[targetInput.Value].OnAfterChanged -= updateListener;
			};

			targetInput.OnAfterChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				targetInput.Value.Template.GetNode<ArmourInputNode> ()
					.Armour[targetInput.Value].OnAfterChanged += updateListener;
				updateListener ();
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}
