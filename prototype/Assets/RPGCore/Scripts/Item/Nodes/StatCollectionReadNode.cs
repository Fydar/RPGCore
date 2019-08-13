using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;

namespace RPGCore
{
	public class StatCollectionReadNode<A, B, C, D> : BehaviourNode
		where A : EnumerableCollection<FloatInput>
		where B : EnumerableCollection<StatInstance>, new()
		where C : CollectionEntry
		where D : StatCollectionInputNode<A, B>, new()

	{
		public C Stat;

		public ItemInput Target;
		public FloatOutput Value;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<ItemSurrogate> targetInput = Target[context];
			ConnectionEntry<float> valueOutput = Value[context];

			Action updateListener = () =>
			{
				if (targetInput.Value == null)
				{
					valueOutput.Value = 0.0f;
					return;
				}

				var weaponNode = targetInput.Value.Template.GetNode<D> ();

				if (weaponNode == null)
				{
					valueOutput.Value = 0;
					return;
				}
				valueOutput.Value = weaponNode.GetStats (targetInput.Value)[Stat].Value;
			};

			if (targetInput.Value != null)
			{
				targetInput.Value.Template.GetNode<D> ()
					.GetStats (targetInput.Value)[Stat].OnValueChanged += updateListener;

				updateListener ();
			}

			targetInput.OnBeforeChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				targetInput.Value.Template.GetNode<D> ()
					.GetStats (targetInput.Value)[Stat].OnValueChanged -= updateListener;
			};

			targetInput.OnAfterChanged += () =>
			{
				updateListener ();
				if (targetInput.Value == null)
					return;

				targetInput.Value.Template.GetNode<D> ()
					.GetStats (targetInput.Value)[Stat].OnValueChanged += updateListener;
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{
		}
	}
}
