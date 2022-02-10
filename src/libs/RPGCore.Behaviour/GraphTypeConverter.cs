namespace RPGCore.Behaviour;

public abstract class GraphTypeConverter<TFrom, TTo>
{
	public abstract TTo Convert(TFrom from);
}
