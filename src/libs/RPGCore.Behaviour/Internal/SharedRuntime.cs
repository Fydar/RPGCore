namespace RPGCore.Behaviour.Internal;

internal static class SharedRuntime<TRuntime>
	where TRuntime : NodeRuntime, new()
{
	public static TRuntime Instance { get; } = new TRuntime();
}
