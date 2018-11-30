using RPGCore.Behaviour;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	public class Buff : IBehaviourContext
	{
		public event Action OnRemove;
		public Action OnTick;

		public List<BuffClock> Clocks = new List<BuffClock> ();

		public BuffTemplate buffTemplate;

		public IntEventField BaseStackSize = new IntEventField ();
		public IntEventField StackSize = new IntEventField ();

		protected BuffInputNode buffInput;

		public bool HasActiveClock
		{
			get
			{
				for (int i = 0; i < Clocks.Count; i++)
				{
					if (Clocks[i].GetType () == typeof (BuffClockDecaying))
						return true;
				}
				return false;
			}
		}

		public float DisplayPercent
		{
			get
			{
				float max = 0.0f;
				for (int i = 0; i < Clocks.Count; i++)
				{
					max = Mathf.Max (max, Clocks[i].DisplayPercent);
				}
				return max;
			}
		}

		public Buff (BuffForNode buffNode, IBehaviourContext context)
		{
			buffTemplate = buffNode.BuffToApply;
			buffTemplate.SetupGraph (this);

			buffInput = buffTemplate.GetNode<BuffInputNode> ();
			buffInput.Target[this].Value = buffNode.Target[context].Value;

			BaseStackSize.onChanged += RecalculateStackSize;

			OnRemove += () =>
			{
				buffTemplate.RemoveGraph (this);
			};
			buffInput.SetRPGCoreBuff (this, this);
		}

		public Buff (BuffWhilstNode buffNode, IBehaviourContext context)
		{
			buffTemplate = buffNode.BuffToApply;
			buffTemplate.SetupGraph (this);

			buffInput = buffTemplate.GetNode<BuffInputNode> ();
			buffInput.Target[this].Value = buffNode.Target[context].Value;

			BaseStackSize.onChanged += RecalculateStackSize;

			OnRemove += () =>
			{
				buffTemplate.RemoveGraph (this);
			};
			buffInput.SetRPGCoreBuff (this, this);
		}

		public Buff (BuffGrantNode buffNode, IBehaviourContext context)
		{
			buffTemplate = buffNode.BuffToApply;
			buffTemplate.SetupGraph (this);

			buffInput = buffTemplate.GetNode<BuffInputNode> ();
			buffInput.Target[this].Value = buffNode.Target[context].Value;

			BaseStackSize.onChanged += RecalculateStackSize;

			OnRemove += () =>
			{
				buffTemplate.RemoveGraph (this);
			};
			buffInput.SetRPGCoreBuff (this, this);
		}

		public Buff (BuffTemplate _buffTemplate, RPGCharacter _target, BuffClock baseClock)
		{
			buffTemplate = _buffTemplate;
			buffTemplate.SetupGraph (this);

			buffInput = buffTemplate.GetNode<BuffInputNode> ();
			buffInput.Target.GetEntry (this).Value = _target;

			AddClock (baseClock);

			BaseStackSize.onChanged += RecalculateStackSize;

			OnRemove += () =>
			{
				buffTemplate.RemoveGraph (this);
			};
			buffInput.SetRPGCoreBuff (this, this);
		}

		public BuffClock GetBaseClock ()
		{
			return Clocks[0];
		}

		public void AddClock (BuffClock clock)
		{
			Clocks.Add (clock);

			Action removeCallback = null;
			removeCallback = () =>
			{
				Clocks.Remove (clock);

				clock.StackSize.OnValueChanged -= RecalculateStackSizeCallback;
				clock.OnRemove -= removeCallback;
				clock.OnTick -= OnTick;

				if (Clocks.Count == 0)
					RemoveBuff ();
				else
					RecalculateStackSize ();
			};

			clock.StackSize.OnValueChanged += RecalculateStackSizeCallback;
			clock.OnRemove += removeCallback;
			clock.OnTick += OnTick;

			RecalculateStackSizeCallback (0);
		}
		public void RemoveClock (BuffClock clock)
		{
			clock.OnTick -= OnTick;
			clock.RemoveClock ();
		}

		public void Update (float deltaTime)
		{
			for (int i = Clocks.Count - 1; i >= 0; i--)
			{
				BuffClock clock = Clocks[i];

				clock.Update (deltaTime);
			}
		}

		public void RemoveBuff ()
		{
			if (OnRemove != null)
				OnRemove ();
		}

		private void RecalculateStackSizeCallback (int _)
		{
			RecalculateStackSize ();
		}

		private void RecalculateStackSize ()
		{
			int counter = BaseStackSize.Value;

			foreach (BuffClock addClock in Clocks)
			{
				counter += addClock.StackSize.Value;
			}
			StackSize.Value = counter;
		}

	}
}

