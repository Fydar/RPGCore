namespace RPGCore.Behaviour
{
	public interface IInputNode<T> : INodeInstance
	{
		void OnReceiveInput(T input);
	}
}
