using Newtonsoft.Json.Linq;
using RPGCore.DataEditor.Manifest;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	public class EditorValue : IEditorValue
	{
		private interface IQueuedItem
		{
			void ApplyValue(EditorValue field);
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

			public void ApplyValue(EditorValue valueContainer)
			{
				if (!isDirty)
				{
					return;
				}

				var replace = JToken.FromObject(Value);

				if (JToken.DeepEquals(replace, valueContainer.json))
				{
					return;
				}

				valueContainer.json.Replace(replace);
				valueContainer.json = replace;
				isDirty = false;
			}
		}


		[DebuggerBrowsable(DebuggerBrowsableState.Never)] public EditorSession Session { get; }

		public TypeInformation Type { get; }

		private JToken json;
		private IQueuedItem queued;

		public EditorValue(EditorSession session, TypeInformation type, JToken json)
		{
			Session = session;
			this.json = json;
			Type = type;
		}

		public void SetValue<T>(T value)
		{
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
			if (queued == null)
			{
				T loaded;
				try
				{
					loaded = json.ToObject<T>(Session.JsonSerializer);
				}
				catch
				{
					loaded = default;
				}
				queued = new QueuedItem<T>(loaded);
			}

			return ((QueuedItem<T>)queued).Value;
		}

		public void ApplyModifiedProperties()
		{
			if (queued == null)
			{
				return;
			}
			queued.ApplyValue(this);

			Session.InvokeOnChanged();
		}

		public override string ToString()
		{
			return json.ToString();
		}
	}
}
