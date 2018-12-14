using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.World
{
	/// <summary>
	/// A very unoptimized method of getting all RPGCharacters in range. Please don't use this on a shipped title, it's for demo purposes only.
	/// </summary>
	public class ProximityChecker : MonoBehaviour
	{
		public float Distance = 5.0f;

		public Action<RPGCharacter> OnEnter;
		public Action<RPGCharacter> OnExit;
		public Func<RPGCharacter, bool> Conditions;

		private List<RPGCharacter> InProximity = new List<RPGCharacter> ();

		private void Start ()
		{
			InvokeRepeating ("Check", 0.0f, 0.25f);
		}

		private void OnDisable ()
		{
			foreach (RPGCharacter character in InProximity)
			{
				if (OnExit != null)
					OnExit (character);
			}

			InProximity.Clear ();
		}

		private void Check ()
		{
			if (!enabled)
				return;

			RPGCharacter[] characters = RPGCharacter.All.ToArray ();

			Vector3 position = transform.position;

			for (int i = characters.Length - 1; i >= 0; i--)
			{
				RPGCharacter character = characters[i];

				if (character.gameObject.activeInHierarchy && Distance > Vector3.Distance (character.transform.position, position))
				{
					if (!InProximity.Contains (character))
					{
						if (Conditions != null)
						{
							bool result = true;
							foreach (Func<RPGCharacter, bool> condition in Conditions.GetInvocationList ())
							{
								if (!condition (character))
								{
									result = false;
									break;
								}
							}
							if (!result)
								continue;
						}

						InProximity.Add (character);

						if (OnEnter != null)
							OnEnter (character);
					}
				}
				else
				{
					if (InProximity.Contains (character))
					{
						InProximity.Remove (character);

						if (OnExit != null)
							OnExit (character);
					}
				}
			}
		}
	}
}

