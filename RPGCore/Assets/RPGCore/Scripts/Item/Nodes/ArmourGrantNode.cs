﻿using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;

namespace RPGCore
{
	[NodeInformation ("Item/Grant Armour Stat", "Attribute")]
	public class ArmourGrantNode : BehaviourNode, INodeDescription
	{
		public ArmourStatEntry Stat;

		public ItemInput Target;
		public BoolInput Active;
		public FloatInput Effect;
		public AttributeInformation.ModifierType Scaling;
		public string Display = "{0}";

		public string Description (IBehaviourContext context)
		{
			ConnectionEntry<ItemSurrogate> targetInput = Target[context];
			ConnectionEntry<float> effectInput = Effect[context];

			if (targetInput.Value == null)
				return "";

			var info = ArmourStatInformationDatabase.Instance.ArmourStatInfos[Stat];

			return Display.Replace ("{0}", info.RenderModifier (effectInput.Value, Scaling));
		}

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<ItemSurrogate> targetInput = Target[context];
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
						var armourNode = targetInput.Value.Template.GetNode<ArmourInputNode> ();
						var armourStat = armourNode.GetStat (targetInput.Value, Stat);

						if (Scaling == AttributeInformation.ModifierType.Additive)
							modifier = armourStat.AddFlatModifier (effectInput.Value);
						else
							modifier = armourStat.AddMultiplierModifier (effectInput.Value);

						isActive = true;
					}
				}
				else if (isActive)
				{
					var armourNode = targetInput.Value.Template.GetNode<ArmourInputNode> ();
					var weaponStat = armourNode.GetStat (targetInput.Value, Stat);

					if (Scaling == AttributeInformation.ModifierType.Additive)
						weaponStat.RemoveFlatModifier (modifier);
					else
						weaponStat.RemoveMultiplierModifier (modifier);

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
					return;

				var weaponNode = targetInput.Value.Template.GetNode<ArmourInputNode> ();
				var weaponStat = weaponNode.GetStat (targetInput.Value, Stat);

				if (Scaling == AttributeInformation.ModifierType.Additive)
					weaponStat.RemoveFlatModifier (modifier);
				else
					weaponStat.RemoveMultiplierModifier (modifier);

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
				UnityEngine.Debug.Log ("Effect value " + modifier.Value);
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}
