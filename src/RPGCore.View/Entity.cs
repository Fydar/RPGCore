using Newtonsoft.Json;

namespace RPGCore.View
{
	public abstract class Entity
	{
		[JsonIgnore]
		public EntityRef Id;
	}
}
