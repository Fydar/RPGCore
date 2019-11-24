using RPGCore.Behaviour;

namespace RPGCore.View
{
	public struct EntityRef
	{
		public LocalId EntityId;

		public override string ToString()
		{
			return EntityId.ToString ();
		}
	}
}
