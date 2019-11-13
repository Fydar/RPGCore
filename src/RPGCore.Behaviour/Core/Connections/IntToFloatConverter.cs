namespace RPGCore.Behaviour
{
	public class IntToFloatConverter : ConnectionTypeConverter<int, float>
	{
		protected override float Convert(int original) => original;
	}
}
