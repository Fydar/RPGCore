using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Utility
{
	/// <summary>
	/// Used for creating quick and easy loops inside a coroutine for an amount of seconds.
	/// </summary>
	/// <example>
	/// TimedLoop timer = new TimedLoop (0.5f);
	///
	///	foreach (float perc in timer)
	///	{
	///		Holder.pivot = Vector2.Lerp (fromPivot, toPivot, perc);
	///
	///		// Optionally
	///
	///		timer.Reset ();
	///		timer.Break ();
	///		timer.End ();
	///
	///		yield return null;
	/// }
	/// </example>
	public class TimedLoop : IEnumerator<float>, IEnumerable<float>
	{
		private float time;
		private float duration;
		private bool endNext;

		public float Current
		{
			get
			{
				return Percent;
			}
		}

		public float Duration
		{
			get
			{
				return duration;
			}
			set
			{
				duration = value;

				if (time > duration)
				{
					time = duration;
					endNext = true;
				}
			}
		}

		public float Time
		{
			get
			{
				return time;
			}
			set
			{
				time = value;
			}
		}

		public float Percent
		{
			get
			{
				return time / duration;
			}
			set
			{
				time = duration * value;
			}
		}

		TimedLoop GetEnumerator ()
		{
			return this;
		}

		IEnumerator<float> IEnumerable<float>.GetEnumerator ()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return this;
		}

		object IEnumerator.Current
		{
			get
			{
				return Percent;
			}
		}

		public TimedLoop (float _duration)
		{
			time = 0.0f;
			duration = _duration;
			endNext = false;
		}

		public void End ()
		{
			time = duration;
		}

		public void Break ()
		{
			time = duration;
			endNext = true;
		}

		public bool MoveNext ()
		{
			time += UnityEngine.Time.deltaTime;

			if (time < duration)
				return true;

			if (endNext == false)
			{
				endNext = true;
				time = duration;
				return true;
			}

			return false;
		}

		public void Reset ()
		{
			time = 0.0f;
			endNext = false;
		}

		public void Dispose ()
		{

		}
	}
}

