namespace RPGCore.Events;

public interface IHandlerUsedCallback
{
	void OnUse(IReadOnlyEventField field);
}
