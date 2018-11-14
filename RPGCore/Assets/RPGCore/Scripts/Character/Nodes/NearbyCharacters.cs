using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.World;
using System;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Character/Nearby")]
	public class NearbyCharacters : BehaviourNode
	{
		public CharacterInput Center;
		public BoolInput IncludeCaster;
		public FloatInput Distance;
		public CharacterListOutput Targets;

		protected override void OnSetup (IBehaviourContext character)
		{
			ConnectionEntry<RPGCharacter> centerInput = Center.GetEntry (character);
			ConnectionEntry<bool> includeCasterInput = IncludeCaster.GetEntry (character);
			ConnectionEntry<float> distanceInput = Distance.GetEntry (character);
			CharacterConnection.EntryCollection targetsOutput = Targets.GetEntry (character);

			GameObject proximityHolder = new GameObject ("Proximity Cheacker");
			ProximityChecker proximity = proximityHolder.AddComponent<ProximityChecker> ();
			proximity.enabled = false;

			proximity.Conditions += (RPGCharacter target) =>
			{
				return includeCasterInput.Value ? true : target != centerInput.Value;
			};

			proximity.OnEnter += (RPGCharacter target) =>
			{
				targetsOutput.Add (target);
			};

			proximity.OnExit += (RPGCharacter target) =>
			{
				targetsOutput.Remove (target);
			};

			Action changeHandler = () =>
			{
				if (centerInput.Value == null)
				{
					proximityHolder.transform.SetParent (null);
					proximity.enabled = false;
					return;
				}

				proximityHolder.transform.SetParent (centerInput.Value.transform);
				proximityHolder.transform.localPosition = Vector3.zero;
				proximity.enabled = true;
			};

			Action distanceChangeHandler = () =>
			{
				proximity.Distance = distanceInput.Value;
			};

			distanceChangeHandler ();
			changeHandler ();

			centerInput.OnAfterChanged += changeHandler;
			distanceInput.OnAfterChanged += distanceChangeHandler;
		}

		protected override void OnRemove (IBehaviourContext character)
		{

		}
	}
}