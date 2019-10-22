using Newtonsoft.Json.Linq;
using RPGCore.Behaviour.Manifest;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Behaviour.Editor
{
	public class EditorField : IEnumerable<EditorField>
	{
		private interface IQueuedItem
		{
			void SetValue (EditorField field);
		}

		private class QueuedItem<T> : IQueuedItem
		{
			public T Value
			{
				get => ValueInternal;
				set
				{
					ValueInternal = value;
					IsDirty = true;
				}
			}

			private T ValueInternal;
			private bool IsDirty;

			public QueuedItem (T value)
			{
				Value = value;
			}

			public void SetValue (EditorField field)
			{
				if (!IsDirty)
				{
					return;
				}

				var replace = JToken.FromObject (Value);

				if (JToken.DeepEquals(replace, field.Json))
				{
					return;
				}

				field.Json.Replace (replace);
				field.Json = replace;
				IsDirty = false;
			}
		}

		public string Name;
		public FieldInformation Field;
		public TypeInformation Type;
		public JToken Json;

		public EditorSession Session;
		public Dictionary<string, EditorField> Children;
		public Dictionary<object, object> ViewBag = new Dictionary<object, object> ();

		private IQueuedItem Queued;

		public void SetValue<T> (T value)
		{
			if (Queued == null)
			{
				Queued = new QueuedItem<T> (value);
			}
			else
			{
				((QueuedItem<T>)Queued).Value = value;
			}
		}

		public T GetValue<T> ()
		{
			if (Queued != null)
			{
				return ((QueuedItem<T>)Queued).Value;
			}
			else
			{
				return Json.ToObject<T> ();
			}
		}

		public void ApplyModifiedProperties ()
		{
			if (Queued == null)
			{
				return;
			}

			Queued.SetValue (this);
		}

		public EditorField (EditorSession session, JToken json, string name, FieldInformation info)
		{
			Session = session;
			Name = name;
			Field = info;
			Json = json;

			Type = session.GetTypeInformation (info.Type);

			if (Json.Type == JTokenType.Object
				&& Field?.Format != FieldFormat.Dictionary)
			{
				PopulateMissing ((JObject)Json, Type);
			}

			Children = new Dictionary<string, EditorField> ();
			if (Field.Format == FieldFormat.Dictionary)
			{
				if (Json.Type != JTokenType.Null)
				{
					foreach (var property in ((JObject)Json).Properties ())
					{
						Children.Add (property.Name, new EditorField (Session, property.Value, property.Name, Field.ValueFormat));
					}
				}
			}
			else
			{
				if (Type.Fields != null)
				{
					foreach (var field in Type.Fields)
					{
						var property = Json[field.Key];

						if (field.Value.Type == "JObject")
						{
							var type = Json["Type"];


							Children.Add (field.Key, new EditorField (Session, property, field.Key, new FieldInformation ()
							{
								Type = type.ToObject<string> (),
								Format = FieldFormat.Object
							}));
						}
						else
						{
							Children.Add (field.Key, new EditorField (Session, property, field.Key, field.Value));
						}
					}
				}
			}
		}

		public IEnumerator<EditorField> GetEnumerator ()
		{
			return ((IEnumerable<EditorField>)Children.Values).GetEnumerator ();
		}

		public EditorField this[string key] => Children[key];

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator ();
		}

		public static void PopulateMissing (JObject serialized, TypeInformation information)
		{
			// Remove any additional fields.
			foreach (var item in serialized.Children<JProperty> ().ToList ())
			{
				if (!information.Fields.Keys.Contains (item.Name))
				{
					item.Remove ();
				}
			}

			// Populate missing fields with default values.
			foreach (var field in information.Fields)
			{
				if (!serialized.ContainsKey (field.Key))
				{
					serialized.Add (field.Key, field.Value.DefaultValue);
				}
			}
		}
	}

	public class EditorSession
	{
		public BehaviourManifest Manifest;
		public JObject Json;
		public TypeInformation Type;

		public EditorField Root;

		public EditorSession (BehaviourManifest manifest, object instance)
		{
			Manifest = manifest;
			Root = new EditorField (this, JObject.FromObject (instance), "root", new FieldInformation ()
			{
				Type = instance.GetType ().FullName
			});
		}

		public EditorSession (BehaviourManifest manifest, JObject instance, string type)
		{
			Manifest = manifest;
			Root = new EditorField (this, instance, "root", new FieldInformation ()
			{
				Type = type
			});
		}

		public static TypeInformation GetTypeInformation (BehaviourManifest manifest, string type)
		{
			if (manifest.Types.JsonTypes.TryGetValue (type, out var jsonType))
			{
				return jsonType;
			}
			if (manifest.Types.ObjectTypes.TryGetValue (type, out var objectType))
			{
				return objectType;
			}
			if (manifest.Nodes.Nodes.TryGetValue (type, out var nodeType))
			{
				return nodeType;
			}
			return null;
		}

		public TypeInformation GetTypeInformation (string type)
		{
			return GetTypeInformation (Manifest, type);
		}
	}
}
