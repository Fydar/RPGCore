namespace RPGCore.Audio
{
	public interface IInterpolator
	{
		float Value { get; set; }
		float TargetValue { set; }
		bool Sleeping { get; }

		void Update (float deltaTime);
	}
}