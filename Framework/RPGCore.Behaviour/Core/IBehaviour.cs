namespace RPGCore.Behaviour
{
	public interface IBehaviour
	{
		INodeInstance GetNode<T> ();
		void Remove ();
	}
}
