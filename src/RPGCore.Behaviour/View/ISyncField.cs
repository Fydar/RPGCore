using RPGCore.View;

namespace RPGCore.Behaviour
{
	public interface ISyncField
	{
		object AddSyncHandler(ViewDispatcher viewDispatcher, EntityRef root, string path);
		void Apply(ViewPacket packet);
	}
}
