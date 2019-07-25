namespace RPGCore.Behaviour
{
	public interface INodeInstance
	{
		void OnInputChanged ();
		void Setup (IGraphInstance graph, Node parent, Actor target);
		void Remove ();
	}
}
