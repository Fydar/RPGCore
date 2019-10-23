namespace RPGCore.Behaviour
{
	public interface INodeInstance
	{
		Node Node { get; }

		void OnInputChanged ();
		void Setup (IGraphInstance graph, Actor target);
		void Remove ();
	}
}
