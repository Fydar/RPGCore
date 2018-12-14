using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation ("Item/Grant Weapon Stat", "Attribute")]
	public class GrantWeaponStatsNode : BehaviourNode
	{
		[CollectionType (typeof (WeaponStatCollection<>))]
		public CollectionEntry Stat;

		public ItemInput Target;
		public FloatInput Value;

		protected override void OnSetup (IBehaviourContext context)
		{
			/*ConnectionEntry<ItemSurrogate> targetInput = Target[context];
			ConnectionEntry<bool> activeInput = Active[context];
			ConnectionEntry<float> effectInput = Effect[context];

			StatInstance.Modifier modifier = null;
			bool isActive = false;

			Action changeHandler = () =>
			{
				if (targetInput.Value == null)
					return;

				if (activeInput.Value)
				{
					if (!isActive)
					{
						FloatInput localStatInput = null;

						if(Stat.entryIndex == -1)
						{
							var temp = WeaponStatInformationDatabase.Instance.WeaponStatInfos[Stat];
						}

						if (Stat.entryIndex == 0)
							localStatInput = weaponNode.AttackDamage;
						else if (Stat.entryIndex == 1)
							localStatInput = weaponNode.AttackSpeed;
						else if (Stat.entryIndex == 2)
							localStatInput = weaponNode.CriticalChance;
						else if (Stat.entryIndex == 3)
							localStatInput = weaponNode.CriticalMultiplier;
						
						valueInput.Value = localStatInput[targetInput.Value].Value;

						if (Scaling == AttributeInformation.ModifierType.Additive)
							modifier = targetInput.Value.Stats[entry].AddFlatModifier (effectInput.Value);
						else
							modifier = targetInput.Value.Stats[entry].AddMultiplierModifier (effectInput.Value);

						isActive = true;
					}
				}
				else if (isActive)
				{
					if (Scaling == AttributeInformation.ModifierType.Additive)
						targetInput.Value.Stats[entry].RemoveFlatModifier (modifier);
					else
						targetInput.Value.Stats[entry].RemoveMultiplierModifier (modifier);

					isActive = false;
				};
			};

			targetInput.OnBeforeChanged += () =>
			{

				if (targetInput.Value == null)
				{
					isActive = false;
					return;
				}

				if (!isActive)
					return;

				if (Scaling == AttributeInformation.ModifierType.Additive)
					targetInput.Value.Stats[entry].RemoveFlatModifier (modifier);
				else
					targetInput.Value.Stats[entry].RemoveMultiplierModifier (modifier);

				isActive = false;
			};

			targetInput.OnAfterChanged += changeHandler;
			activeInput.OnAfterChanged += changeHandler;

			changeHandler ();

			effectInput.OnAfterChanged += () =>
			{
				if (modifier == null)
					return;

				modifier.Value = effectInput.Value;
			};*/
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}
