using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;

namespace RPGCore
{
	[NodeInformation ("Item/Read Stat", "Attribute")]
	public class ItemStatsNode : BehaviourNode
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
				valueInput.Value = targetInput.Value.Template.GetNode<WeaponInputNode> ().AttackDamage[targetInput.Value].Value;
			};

			if (targetInput.Value != null)
			{
				targetInput.Value.Template.GetNode<WeaponInputNode> ()
					.AttackDamage[targetInput.Value].OnAfterChanged += updateListener;

				updateListener ();
			}

			targetInput.OnBeforeChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				targetInput.Value.Template.GetNode<WeaponInputNode> ()
					.AttackDamage[targetInput.Value].OnAfterChanged -= updateListener;
			};

			targetInput.OnAfterChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				targetInput.Value.Template.GetNode<WeaponInputNode> ()
					.AttackDamage[targetInput.Value].OnAfterChanged += updateListener;
				updateListener ();
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}