using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.World;
using System;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation("Character/Nearby")]
	public class NearbyCharacters : BehaviourNode
	{
		public CharacterInput Center;
		public BoolInput IncludeCaster;
		public FloatInput Distance;
		public CharacterListOutput Targets;

		protected override void OnSetup(IBehaviourContext context)
		{
			var centerInput = Center[context];
			var includeCasterInput = IncludeCaster[context];
			var distanceInput = Distance[context];
			var targetsOutput = Targets.GetEntry(context);

			var proximityHolder = new GameObject("Proximity Cheacker");
			var proximity = proximityHolder.AddComponent<ProximityChecker>();
			proximity.enabled = false;

			proximity.Conditions += (RPGCharacter target) =>
			{
				return includeCasterInput.Value ? true : target != centerInput.Value;
			};

			proximity.OnEnter += (RPGCharacter target) =>
			{
				targetsOutput.Add(target);
			};

			proximity.OnExit += (RPGCharacter target) =>
			{
				targetsOutput.Remove(target);
			};

			Action changeHandler = () =>
			{
				if (centerInput.Value == null)
				{
					proximityHolder.transform.SetParent(null);
					proximity.enabled = false;
					return;
				}

				proximityHolder.transform.SetParent(centerInput.Value.transform);
				proximityHolder.transform.localPosition = Vector3.zero;
				proximity.enabled = true;
			};

			Action distanceChangeHandler = () =>
			{
				proximity.Distance = distanceInput.Value;
			};

			distanceChangeHandler();
			changeHandler();

			centerInput.OnAfterChanged += changeHandler;
			distanceInput.OnAfterChanged += distanceChangeHandler;
		}

		protected override void OnRemove(IBehaviourContext context)
		{
		}
	}
}

