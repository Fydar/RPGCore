namespace RPGCore.Behaviour
{
	public interface INodeInstance
	{
		Node NodeBase { get; }

		void OnInputChanged();
		void Setup();
		void Remove();
	}
}
