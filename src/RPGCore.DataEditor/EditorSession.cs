using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;

namespace RPGCore.DataEditor
{
	public class EditorSession
	{
		public BehaviourManifest Manifest;
		public EditorObject Root;
		public JObject Instance;
		public JsonSerializer JsonSerializer;

		public Action OnChanged;

		private readonly List<object> features;

		public EditorSession(BehaviourManifest manifest, object instance, JsonSerializer jsonSerializer)
		{
			Manifest = manifest;
			JsonSerializer = jsonSerializer;
			features = new List<object>();

			var rootJson = JObject.FromObject(instance, JsonSerializer);
			string type = instance.GetType().Name;
			var typeInformation = Manifest.GetTypeInformation(type);

			Instance = rootJson;
			Root = new EditorObject(this, typeInformation, rootJson);
		}

		public EditorSession(BehaviourManifest manifest, JObject instance, string type, JsonSerializer jsonSerializer)
		{
			Manifest = manifest;
			JsonSerializer = jsonSerializer;
			Instance = instance;
			features = new List<object>();

			var typeInformation = Manifest.GetTypeInformation(type);

			Root = new EditorObject(this, typeInformation, instance);
		}

		internal IEditorValue CreateValue(TypeInformation typeInformation, FieldWrapper currentWrapper, JToken token)
		{
			if (currentWrapper == null)
			{
				if ((typeInformation.Fields?.Count ?? 0) == 0)
				{
					return new EditorValue(this, typeInformation, token);
				}
				else
				{
					return new EditorObject(this, typeInformation, token);
				}
			}
			else
			{
				if (currentWrapper.Type == FieldWrapperType.Dictionary)
				{
					return new EditorDictionary(this, typeInformation, token);
				}
				else if (currentWrapper.Type == FieldWrapperType.List)
				{
					return new EditorList(this, typeInformation, token);
				}
				else
				{
					throw new InvalidOperationException("Unsupported wrapper type");
				}
			}
		}

		public T GetFeature<T>()
			where T : class
		{
			var getFeatureType = typeof(T);
			foreach (object feature in features)
			{
				var featureType = feature.GetType();
				if (getFeatureType.IsAssignableFrom(featureType))
				{
					return (T)feature;
				}
			}
			return null;
		}

		public T GetOrCreateFeature<T>()
			where T : class, new()
		{
			var feature = GetFeature<T>();

			if (feature == null)
			{
				feature = new T();
				features.Add(feature);
			}
			return feature;
		}

		internal void InvokeOnChanged()
		{
			OnChanged?.Invoke();
		}
	}
}
