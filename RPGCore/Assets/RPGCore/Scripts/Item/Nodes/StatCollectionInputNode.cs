using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;

namespace RPGCore
{
	public abstract class StatCollectionInputNode<A, B> : BehaviourNode
		where A : EnumerableCollection<FloatInput>
		where B : EnumerableCollection<StatInstance>, new()
	{
		public A Inputs;

		public CharacterOutput HitTarget;
		public EventOutput OnHit;

		[NonSerialized]
		public Dictionary<IBehaviourContext, B> StatsMapping = new Dictionary<IBehaviourContext, B> ();

		public B GetStats (IBehaviourContext context)
		{
			return StatsMapping[context];
		}

		public StatInstance GetStat (ItemSurrogate context, CollectionEntry entry)
		{
			B statsCollection = StatsMapping[context];
			StatInstance stat = statsCollection[entry];
			return stat;
		}

		protected override void OnSetup (IBehaviourContext context)
		{
			var statsCollection = new B ();
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
	}
}
