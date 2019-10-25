using Newtonsoft.Json.Linq;
using RPGCore.Behaviour.Manifest;
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
			void ApplyValue (EditorField field);
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

			public void ApplyValue (EditorField field)
			{
				if (!IsDirty)
				{
					return;
				}

				var replace = JToken.FromObject (Value);

				if (JToken.DeepEquals (replace, field.Json))
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

		public EditorSession Session;
		public Dictionary<object, object> ViewBag = new Dictionary<object, object> ();

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private readonly Dictionary<string, EditorField> Children;
		private JToken Json;
		private IQueuedItem Queued;

		public int Count => Children.Count;

		public EditorField this[string key] => Children[key];

		public EditorField (EditorSession session, JToken json, string name, FieldInformation info)
		{
			Session = session;
			Name = name;
			Field = info;
			Json = json;

			Type = session.Manifest.GetTypeInformation (info.Type);

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
			else if (Field.Format == FieldFormat.List)
			{
				if (Json.Type != JTokenType.Null)
				{
					var children = ((JArray)Json).Children ();
					int index = 0;
					foreach (var token in children)
					{
						string childName = $"[{index}]";
						Children.Add (childName, new EditorField (Session, token, childName, Field.ValueFormat));
						index++;
					}
				}
			}
			else if (Type.Fields != null)
			{
				foreach (var field in Type.Fields)
				{
					var property = Json[field.Key];

					if (field.Value.Type == "JObject")
					{
						var type = Json["Type"];

						var genericField = new FieldInformation ()
						{
							Type = type.ToObject<string> (),
							Format = FieldFormat.Object,

							Attributes = field.Value.Attributes,
							DefaultValue = field.Value.DefaultValue,
							Description = field.Value.Description,
							ValueFormat = field.Value.ValueFormat
						};

						Children.Add (field.Key, new EditorField (Session, property, field.Key, genericField));
					}
					else
					{
						Children.Add (field.Key, new EditorField (Session, property, field.Key, field.Value));
					}
				}
			}
		}

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

			Queued.ApplyValue (this);
		}

		public override string ToString ()
		{
			return $"{Field.Type} {Name} = {Json}";
		}

		public IEnumerator<EditorField> GetEnumerator ()
		{
			return ((IEnumerable<EditorField>)Children.Values).GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator ();
		}

		private static void PopulateMissing (JObject serialized, TypeInformation information)
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
		public EditorField Root;
		public JObject Instance;

		public EditorSession (BehaviourManifest manifest, object instance)
		{
			Manifest = manifest;

			var rootJson = JObject.FromObject (instance);
			string type = instance.GetType ().FullName;
			Instance = rootJson;

			var rootField = new FieldInformation ()
			{
				Type = type
			};
			Root = new EditorField (this, rootJson, "root", rootField);
		}

		public EditorSession (BehaviourManifest manifest, JObject instance, string type)
		{
			Manifest = manifest;
			Instance = instance;

			var rootField = new FieldInformation ()
			{
				Type = type
			};
			Root = new EditorField (this, instance, "root", rootField);
		}
	}
}
