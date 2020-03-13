using Newtonsoft.Json.Linq;
using RPGCore.Behaviour.Manifest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPGCore.Behaviour.Editor
{
	public class EditorField : IEnumerable<EditorField>
	{
		private interface IQueuedItem
		{
			void ApplyValue(EditorField field);
		}

		private class QueuedItem<T> : IQueuedItem
		{
			public T Value
			{
				get => valueInternal;
				set
				{
					valueInternal = value;
					isDirty = true;
				}
			}

			private T valueInternal;
			private bool isDirty;

			public QueuedItem(T value)
			{
				Value = value;
			}

			public void ApplyValue(EditorField field)
			{
				if (!isDirty)
				{
					return;
				}

				var replace = JToken.FromObject(Value);

				if (JToken.DeepEquals(replace, field.json))
				{
					return;
				}

				field.json.Replace(replace);
				field.json = replace;
				isDirty = false;
			}
		}

		public string Name;
		public FieldInformation Field;
		public TypeInformation Type;
		public EditorSession Session;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Dictionary<string, EditorField> children;

		private readonly List<object> features;
		private JToken json;
		private IQueuedItem queued;

		public int Count => children.Count;

		public EditorField this[string key] => children[key];

		public EditorField(EditorSession session, JToken json, string name, FieldInformation field)
		{
			children = new Dictionary<string, EditorField>();
			features = new List<object>();

			Session = session;
			Name = name;
			Field = field;
			this.json = json;
			Type = session.Manifest.GetTypeInformation(field.Type);

			UpdateChildren();
		}

		private void UpdateChildren()
		{
			var newChildren = new Dictionary<string, EditorField>();

			if (Field.Format == FieldFormat.Dictionary)
			{
				if (json.Type != JTokenType.Null)
				{
					foreach (var property in ((JObject)json).Properties())
					{
						string key = property.Name;

						if (!children.TryGetValue(key, out var field))
						{
							field = new EditorField(Session, property.Value, property.Name, Field.ValueFormat);
						}
						newChildren.Add(key, field);
					}
				}
			}
			else if (Field.Format == FieldFormat.List)
			{
				if (json.Type != JTokenType.Null)
				{
					var children = ((JArray)json).Children();
					int index = 0;
					foreach (var token in children)
					{
						string childName = $"[{index}]";

						if (!this.children.TryGetValue(childName, out var field))
						{
							field = new EditorField(Session, token, childName, Field.ValueFormat);
						}
						newChildren.Add(childName, field);

						index++;
					}
				}
			}
			else if (Field.Format == FieldFormat.Object
				&& json != null
				&& json.Type == JTokenType.Object)
			{
				if (Type?.Fields != null && Type.Fields.Count != 0)
				{
					PopulateMissing((JObject)json, Type);

					foreach (var childField in Type.Fields)
					{
						string key = childField.Key;
						var property = json[key];

						if (!children.TryGetValue(key, out var field))
						{
							if (childField.Value.Type == "JObject")
							{
								var type = json["Type"];

								var genericField = new FieldInformation()
								{
									Type = type.ToObject<string>(),
									Format = FieldFormat.Object,

									Attributes = childField.Value.Attributes,
									DefaultValue = childField.Value.DefaultValue,
									Description = childField.Value.Description,
									ValueFormat = childField.Value.ValueFormat
								};

								field = new EditorField(Session, property, key, genericField);
							}
							else
							{
								field = new EditorField(Session, property, key, childField.Value);
							}
						}
						newChildren.Add(key, field);
					}
				}
			}
			children = newChildren;
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

		public bool Remove(string key)
		{
			if (Field.Format != FieldFormat.Dictionary)
			{
				throw new InvalidOperationException($"Cannot remove elements of \"{Field.Format}\" typed fields.");
			}

			if (json.Type == JTokenType.Null)
			{
				return false;
			}

			var jsonObject = (JObject)json;

			bool removed = jsonObject.Remove(key);

			if (!removed)
			{
				return false;
			}

			children.Remove(key);

			return true;
		}

		public void Add(string key)
		{
			var duplicate = Type.DefaultValue?.DeepClone() ?? JValue.CreateNull();

			AddInternal(key, duplicate);
		}

		private void AddInternal(string key, JToken value)
		{
			if (Field.Format != FieldFormat.Dictionary)
			{
				throw new InvalidOperationException($"Cannot remove elements of \"{Field.Format}\" typed fields.");
			}

			JObject jsonObject;
			if (json.Type == JTokenType.Null)
			{
				jsonObject = new JObject();
				json.Replace(jsonObject);
				json = jsonObject;
			}
			else
			{
				jsonObject = (JObject)json;
			}

			jsonObject.Add(
				key,
				value
			);

			if (!children.TryGetValue(key, out var field))
			{
				field = new EditorField(Session, value, key, Field.ValueFormat);
			}
			children.Add(key, field);
		}

		public void SetArraySize(int size)
		{
			if (Field.Format != FieldFormat.List)
			{
				throw new InvalidOperationException($"Cannot set the value of \"{Field.Format}\" typed fields.");
			}

			JArray jsonArray;
			if (json.Type == JTokenType.Null)
			{
				jsonArray = new JArray();
				json.Replace(jsonArray);
				json = jsonArray;
			}
			else
			{
				jsonArray = (JArray)json;
			}

			while (jsonArray.Count > size)
			{
				int removeIndex = jsonArray.Count - 1;
				string childName = $"[{removeIndex}]";

				jsonArray.RemoveAt(removeIndex);

				children.Remove(childName);
			}

			while (jsonArray.Count < size)
			{
				int addIndex = jsonArray.Count;
				string childName = $"[{addIndex}]";

				var duplicate = Type.DefaultValue?.DeepClone() ?? JValue.CreateNull();
				jsonArray.Add(
					duplicate
				);

				if (!children.TryGetValue(childName, out var field))
				{
					field = new EditorField(Session, duplicate, childName, Field.ValueFormat);
				}
				children.Add(childName, field);
			}
		}

		public void SetValue<T>(T value)
		{
			if (Field.Format != FieldFormat.Object)
			{
				throw new InvalidOperationException($"Cannot set the value of \"{Field.Format}\" typed fields.");
			}

			if (queued == null)
			{
				queued = new QueuedItem<T>(value);
			}
			else
			{
				((QueuedItem<T>)queued).Value = value;
			}
		}

		public T GetValue<T>()
		{
			if (Field.Format != FieldFormat.Object)
			{
				throw new InvalidOperationException($"Cannot get the value of \"{Field.Format}\" typed fields.");
			}

			if (queued == null)
			{
				queued = new QueuedItem<T>(json.ToObject<T>(Session.JsonSerializer));
			}

			return ((QueuedItem<T>)queued).Value;
		}

		public void ApplyModifiedProperties()
		{
			if (Field.Format != FieldFormat.Object)
			{
				throw new InvalidOperationException($"Cannot apply modified properties of \"{Field.Format}\" typed fields.");
			}

			if (queued == null)
			{
				return;
			}

			if (json.Type == JTokenType.Null)
			{
				queued.ApplyValue(this);

			}
			else
			{
				queued.ApplyValue(this);
			}
			children = new Dictionary<string, EditorField>();
			UpdateChildren();
		}

		public override string ToString()
		{
			return $"{Field.Type} {Name} = {json}";
		}

		public IEnumerator<EditorField> GetEnumerator()
		{
			return ((IEnumerable<EditorField>)children.Values).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator();
		}

		private static void PopulateMissing(JObject serialized, TypeInformation information)
		{
			// Remove any additional fields.
			foreach (var item in serialized.Children<JProperty>().ToList())
			{
				if (!information.Fields.Keys.Contains(item.Name))
				{
					item.Remove();
				}
			}

			// Populate missing fields with default values.
			if (information.Fields != null)
			{
				foreach (var field in information.Fields)
				{
					if (!serialized.ContainsKey(field.Key))
					{
						serialized.Add(field.Key, field.Value.DefaultValue);
					}
				}
			}
		}
	}
}
