using System;

namespace RPGCore.Audio
{
	[Serializable]
	public class EffectFader
	{
		public LoopGroup Audio;

		private IInterpolator Interpolator;

		public float Value
		{
			get
			{
				return Interpolator.Value;
			}
		}

		public float TargetValue
		{
			set
			{
				Interpolator.TargetValue = value;
			}
		}

		public EffectFader (IInterpolator interpolator)
		{
			Interpolator = interpolator;
		}

		public void Update (float deltaTime)
		{
			Interpolator.Update (deltaTime);
		}
	}
}

