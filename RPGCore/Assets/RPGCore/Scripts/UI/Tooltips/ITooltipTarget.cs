namespace RPGCore.Tooltips
{
	public interface ITooltipTarget<T>
	{
		void Render (T target);
	}
}