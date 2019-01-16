using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;
using System.Collections.Generic;

namespace RPGCore
{
	[NodeInformation ("Item/Weapon Input", "Input", OnlyOne = true)]
	public class WeaponInputNode : BehaviourNode, IInputNode<ItemSurrogate>
	{
		public WeaponStatFloatInputCollection Inputs;

		public CharacterOutput HitTarget;
		public EventOutput OnHit;

		[NonSerialized]
		public Dictionary<IBehaviourContext, WeaponStatInstanceCollection> StatsMapping = new Dictionary<IBehaviourContext, WeaponStatInstanceCollection> ();

		public WeaponStatInstanceCollection GetStats (IBehaviourContext context)
		{
			return StatsMapping[context];
		}

		public StatInstance GetStat (ItemSurrogate context, CollectionEntry entry)
		{
			WeaponStatInstanceCollection statsCollection = StatsMapping[context];
			StatInstance stat = statsCollection[entry];
			return stat;
		}

		protected override void OnSetup (IBehaviourContext context)
		{
			var statsCollection = new WeaponStatInstanceCollection ();
			StatsMapping[context] = statsCollection;
			statsCollection.GetEnumerator ();

			var inputsEnumerator = Inputs.GetEnumerator();
			foreach(var stat in GetStats(context))
			{
				inputsEnumerator.MoveNext();
				var currentInput = inputsEnumerator.Current[context];

				var modifier = stat.AddFlatModifier(currentInput.Value);
				currentInput.OnAfterChanged += () => modifier.Value = currentInput.Value;
			}}

		protected override void OnRemove (IBehaviourContext context)
		{

		}

		public void SetTarget (IBehaviourContext context, ItemSurrogate target)
		{

		}
	}
}
