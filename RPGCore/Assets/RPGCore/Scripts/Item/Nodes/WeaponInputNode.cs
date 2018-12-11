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
		public Dictionary<IBehaviourContext, WeaponStatInstanceCollection> StatsMapping;

		public WeaponStatInstanceCollection GetStats (IBehaviourContext context)
		{
			return StatsMapping[context];
		}

		public StatInstance GetStat (IBehaviourContext context, CollectionEntry entry)
		{
			if (entry.entryIndex == -1)
			{
				var temp = ArmourStatInformationDatabase.Instance.ArmourStatInfos[entry];
			}

			WeaponStatInstanceCollection statsCollection = StatsMapping[context];
			StatInstance stat = null;

			if (entry.entryIndex == 0)
				stat = statsCollection.Attack;
			else if (entry.entryIndex == 1)
				stat = statsCollection.AttackSpeed;
			else if (entry.entryIndex == 2)
				stat = statsCollection.CriticalStrikeChance;
			else if (entry.entryIndex == 3)
				stat = statsCollection.CriticalStrikeMultiplier;

			return stat;
		}

		/*public FloatInput GetSocket(CollectionEntry entry)
		{
			if (entry.entryIndex == -1)
			{
				var temp = ArmourStatInformationDatabase.Instance.ArmourStatInfos[entry];
			}

			FloatInput localStatInput = null;

			if (entry.entryIndex == 0)
				localStatInput = AttackDamage;
			else if (entry.entryIndex == 1)
				localStatInput = AttackSpeed;
			else if (entry.entryIndex == 2)
				localStatInput = CriticalChance;
			else if (entry.entryIndex == 3)
				localStatInput = CriticalMultiplier;

			return localStatInput;
		}*/

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<float> attackInput = AttackDamage[context];
			ConnectionEntry<float> attackSpeedInput = AttackSpeed[context];
			ConnectionEntry<float> criticalChanceInput = CriticalChance[context];
			ConnectionEntry<float> criticalMultiplierInput = CriticalMultiplier[context];

			var statsCollection = new WeaponStatInstanceCollection ();
			StatsMapping[context] = statsCollection;

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
