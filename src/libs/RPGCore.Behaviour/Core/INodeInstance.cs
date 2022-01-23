namespace RPGCore.Behaviour;

public interface INodeInstance
{
	IGraphInstance Graph { get; }
	NodeTemplate Template { get; }

	void OnInputChanged();

	void Setup();

	void Remove();
}
