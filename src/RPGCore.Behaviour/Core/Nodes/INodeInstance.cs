namespace RPGCore.Behaviour
{
	public interface INodeInstance
	{
		void OnInputChanged ();
		void Setup (IGraphInstance graph, Actor target);
		void Remove ();
	}
}
