namespace RPGCore.Behaviour
{
	public interface IInputNode { }

	public interface IInputNode<T> : IInputNode
	{
		void SetTarget (IBehaviourContext context, T target);
	}
}
