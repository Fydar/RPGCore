using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour.Manifest;

namespace RPGCore.Behaviour.Editor
{
	public class EditorSession
	{
		public BehaviourManifest Manifest;
		public EditorField Root;
		public JObject Instance;
		public JsonSerializer JsonSerializer;

		public EditorSession (BehaviourManifest manifest, object instance, JsonSerializer jsonSerializer)
		{
			Manifest = manifest;
			JsonSerializer = jsonSerializer;

			var rootJson = JObject.FromObject (instance, JsonSerializer);
			string type = instance.GetType ().FullName;
			Instance = rootJson;

			var rootField = new FieldInformation ()
			{
				Type = type
			};
			Root = new EditorField (this, rootJson, "root", rootField);
		}

		public EditorSession (BehaviourManifest manifest, JObject instance, string type, JsonSerializer jsonSerializer)
		{
			Manifest = manifest;
			JsonSerializer = jsonSerializer;
			Instance = instance;

			var rootField = new FieldInformation ()
			{
				Type = type
			};
			Root = new EditorField (this, instance, "root", rootField);
		}
	}
}
