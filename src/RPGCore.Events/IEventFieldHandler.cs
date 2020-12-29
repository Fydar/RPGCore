namespace RPGCore.Events
{
	public interface IEventFieldHandler
	{
		void OnBeforeChanged();

		void OnAfterChanged();
	}
}
