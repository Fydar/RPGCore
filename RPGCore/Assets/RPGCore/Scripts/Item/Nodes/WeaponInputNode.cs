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
		public FloatInput AttackDamage;
		public FloatInput AttackSpeed;
		public FloatInput CriticalChance;
		public FloatInput CriticalMultiplier;

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
			ConnectionEntry<float> attackInput = AttackDamage[context];
			ConnectionEntry<float> attackSpeedInput = AttackSpeed[context];
			ConnectionEntry<float> criticalChanceInput = CriticalChance[context];
			ConnectionEntry<float> criticalMultiplierInput = CriticalMultiplier[context];

			var statsCollection = new WeaponStatInstanceCollection ();
			StatsMapping[context] = statsCollection;
			statsCollection.GetEnumerator ();

			var attackModifier = statsCollection.Attack.AddFlatModifier (attackInput.Value);
			attackInput.OnAfterChanged += () => attackModifier.Value = attackInput.Value;

			var attackSpeedModifier = statsCollection.AttackSpeed.AddFlatModifier (attackSpeedInput.Value);
			attackSpeedInput.OnAfterChanged += () => attackSpeedModifier.Value = attackSpeedInput.Value;

			var criticalChanceModifier = statsCollection.CriticalStrikeChance.AddFlatModifier (criticalChanceInput.Value);
			criticalChanceInput.OnAfterChanged += () => criticalChanceModifier.Value = criticalChanceInput.Value;

			var criticalMultiplierModifier = statsCollection.CriticalStrikeMultiplier.AddFlatModifier (criticalMultiplierInput.Value);
			criticalMultiplierInput.OnAfterChanged += () => criticalMultiplierModifier.Value = criticalMultiplierInput.Value;
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}

		public void SetTarget (IBehaviourContext context, ItemSurrogate target)
		{

		}
	}
}
