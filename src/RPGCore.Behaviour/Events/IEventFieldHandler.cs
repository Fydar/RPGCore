namespace RPGCore.Behaviour
{
	public interface IEventFieldHandler
	{
		void OnBeforeChanged();

		void OnAfterChanged();
	}
}
