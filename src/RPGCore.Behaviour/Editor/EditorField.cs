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
				get => ValueInternal;
				set
				{
					ValueInternal = value;
					IsDirty = true;
				}
			}

			private T ValueInternal;
			private bool IsDirty;

			public QueuedItem(T value)
			{
				Value = value;
			}

			public void ApplyValue(EditorField field)
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

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private Dictionary<string, EditorField> Children;
		private readonly List<object> Features;
		private JToken Json;
		private IQueuedItem Queued;

		public int Count => Children.Count;

		public EditorField this[string key] => Children[key];

		public EditorField(EditorSession session, JToken json, string name, FieldInformation field)
		{
			Children = new Dictionary<string, EditorField> ();
			Features = new List<object> ();

			Session = session;
			Name = name;
			Field = field;
			Json = json;
			Type = session.Manifest.GetTypeInformation (field.Type);

			if (Type == null)
			{
				throw new InvalidOperationException ($"Cannot find type information for type \"{field.Type}\".");
			}

			UpdateChildren ();
		}

		private void UpdateChildren()
		{
			var newChildren = new Dictionary<string, EditorField> ();

			if (Field.Format == FieldFormat.Dictionary)
			{
				if (Json.Type != JTokenType.Null)
				{
					foreach (var property in ((JObject)Json).Properties ())
					{
						string key = property.Name;

						if (!Children.TryGetValue (key, out var field))
						{
							field = new EditorField (Session, property.Value, property.Name, Field.ValueFormat);
						}
						newChildren.Add (key, field);
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

						if (!Children.TryGetValue (childName, out var field))
						{
							field = new EditorField (Session, token, childName, Field.ValueFormat);
						}
						newChildren.Add (childName, field);

						index++;
					}
				}
			}
			else if (Field.Format == FieldFormat.Object && Json.Type == JTokenType.Object)
			{
				if (Type.Fields != null && Type.Fields.Count != 0)
				{
					PopulateMissing ((JObject)Json, Type);

					foreach (var childField in Type.Fields)
					{
						string key = childField.Key;
						var property = Json[key];

						if (!Children.TryGetValue (key, out var field))
						{
							if (childField.Value.Type == "JObject")
							{
								var type = Json["Type"];

								var genericField = new FieldInformation ()
								{
									Type = type.ToObject<string> (),
									Format = FieldFormat.Object,

									Attributes = childField.Value.Attributes,
									DefaultValue = childField.Value.DefaultValue,
									Description = childField.Value.Description,
									ValueFormat = childField.Value.ValueFormat
								};

								field = new EditorField (Session, property, key, genericField);
							}
							else
							{
								field = new EditorField (Session, property, key, childField.Value);
							}
						}
						newChildren.Add (key, field);
					}
				}
			}
			Children = newChildren;
		}

		public T GetFeature<T>()
			where T : class
		{
			var getFeatureType = typeof (T);
			foreach (object feature in Features)
			{
				var featureType = feature.GetType ();
				if (getFeatureType.IsAssignableFrom (featureType))
				{
					return (T)feature;
				}
			}
			return null;
		}

		public T GetOrCreateFeature<T>()
			where T : class, new()
		{
			var feature = GetFeature<T> ();

			if (feature == null)
			{
				feature = new T ();
				Features.Add (feature);
			}
			return feature;
		}

		public void SetValue<T>(T value)
		{
			if (Field.Format != FieldFormat.Object)
			{
				throw new InvalidOperationException ($"Cannot set the value of \"{Field.Format}\" typed fields.");
			}

			if (Queued == null)
			{
				Queued = new QueuedItem<T> (value);
			}
			else
			{
				((QueuedItem<T>)Queued).Value = value;
			}
		}

		public T GetValue<T>()
		{
			if (Field.Format != FieldFormat.Object)
			{
				throw new InvalidOperationException ($"Cannot get the value of \"{Field.Format}\" typed fields.");
			}

			if (Queued == null)
			{
				Queued = new QueuedItem<T> (Json.ToObject<T> (Session.JsonSerializer));
			}

			return ((QueuedItem<T>)Queued).Value;
		}

		public void ApplyModifiedProperties()
		{
			if (Field.Format != FieldFormat.Object)
			{
				throw new InvalidOperationException ($"Cannot apply modified properties of \"{Field.Format}\" typed fields.");
			}

			if (Queued == null)
			{
				return;
			}

			if (Json.Type == JTokenType.Null)
			{
				Queued.ApplyValue (this);

				UpdateChildren ();
			}
			else
			{
				Queued.ApplyValue (this);
			}
		}

		public override string ToString()
		{
			return $"{Field.Type} {Name} = {Json}";
		}

		public IEnumerator<EditorField> GetEnumerator()
		{
			return ((IEnumerable<EditorField>)Children.Values).GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator ();
		}

		private static void PopulateMissing(JObject serialized, TypeInformation information)
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
			if (information.Fields != null)
			{
				foreach (var field in information.Fields)
				{
					if (!serialized.ContainsKey (field.Key))
					{
						serialized.Add (field.Key, field.Value.DefaultValue);
					}
				}
			}
		}
	}
}
