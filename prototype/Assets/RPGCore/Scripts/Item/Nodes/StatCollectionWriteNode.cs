using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;

namespace RPGCore
{
	[NodeInformation("Item/Grant Weapon Stat", "Attribute")]
	public class StatCollectionWriteNode<A, B, C, D> : BehaviourNode, INodeDescription
		where A : EnumerableCollection<FloatInput>
		where B : EnumerableCollection<StatInstance>, new()
		where C : CollectionEntry
		where D : StatCollectionInputNode<A, B>, new()
	{
		public C Stat;

		public ItemInput Target;
		public BoolInput Active;
		public FloatInput Effect;

		public AttributeInformation.ModifierType Scaling;
		public string Display = "{0}";

		public string Description(IBehaviourContext context)
		{
			var targetInput = Target[context];
			var effectInput = Effect[context];

			if (targetInput.Value == null)
			{
				return "";
			}

			var info = WeaponStatInformationDatabase.Instance.WeaponStatInfos[Stat];

			return Display.Replace("{0}", info.RenderModifier(effectInput.Value, Scaling));
		}

		protected override void OnSetup(IBehaviourContext context)
		{
			var targetInput = Target[context];
			var activeInput = Active[context];
			var effectInput = Effect[context];

			StatInstance.Modifier modifier = null;
			bool isActive = false;

			Action changeHandler = () =>
			{
				if (targetInput.Value == null)
				{
					return;
				}

				if (activeInput.Value)
				{
					if (!isActive)
					{
						var weaponNode = targetInput.Value.Template.GetNode<D>();
						var weaponStat = weaponNode.GetStat(targetInput.Value, Stat);

						if (Scaling == AttributeInformation.ModifierType.Additive)
						{
							modifier = weaponStat.AddFlatModifier(effectInput.Value);
						}
						else
						{
							modifier = weaponStat.AddMultiplierModifier(effectInput.Value);
						}

						isActive = true;
					}
				}
				else if (isActive)
				{
					var weaponNode = targetInput.Value.Template.GetNode<D>();
					var weaponStat = weaponNode.GetStat(targetInput.Value, Stat);

					if (Scaling == AttributeInformation.ModifierType.Additive)
					{
						weaponStat.RemoveFlatModifier(modifier);
					}
					else
					{
						weaponStat.RemoveMultiplierModifier(modifier);
					}

					isActive = false;
				}
			};

			targetInput.OnBeforeChanged += () =>
			{
				if (targetInput.Value == null)
				{
					isActive = false;
					return;
				}

				if (!isActive)
				{
					return;
				}

				var weaponNode = targetInput.Value.Template.GetNode<D>();
				var weaponStat = weaponNode.GetStat(targetInput.Value, Stat);

				if (Scaling == AttributeInformation.ModifierType.Additive)
				{
					weaponStat.RemoveFlatModifier(modifier);
				}
				else
				{
					weaponStat.RemoveMultiplierModifier(modifier);
				}

				isActive = false;
			};

			targetInput.OnAfterChanged += changeHandler;
			activeInput.OnAfterChanged += changeHandler;

			changeHandler();

			effectInput.OnAfterChanged += () =>
			{
				if (modifier == null)
				{
					return;
				}

				modifier.Value = effectInput.Value;
			};
		}

		protected override void OnRemove(IBehaviourContext context)
		{
		}
	}
}
