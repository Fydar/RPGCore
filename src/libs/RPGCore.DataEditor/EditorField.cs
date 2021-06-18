using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable hard-typed container for a value.
	/// </summary>
	public class EditorField : IEditorToken
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		private readonly List<object> features;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session => Parent.Session;

		/// <summary>
		/// The <see cref="EditorObject"/> that this <see cref="EditorField"/> belongs to.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorObject Parent { get; }

		/// <summary>
		/// A <see cref="SchemaField"/> used to drive the behaviour of this <see cref="EditorField"/>.
		/// </summary>
		public SchemaField? Schema
		{
			get
			{
				var parentType = Session.ResolveType(Parent.Type);

				if (parentType?.Fields == null)
				{
					return null;
				}

				foreach (var field in parentType.Fields)
				{
					if (field.Name == Name)
					{
						return field;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// The value contained within this <see cref="EditorField"/>.
		/// </summary>
		public IEditorValue Value { get; set; }

		/// <summary>
		/// The name of this field.
		/// </summary>
		public string Name { get; } = string.Empty;

		/// <inheritdoc/>
		public event PropertyChangedEventHandler PropertyChanged;

		internal EditorField(EditorObject parent, string name)
		{
			Parent = parent;
			Name = name;

			Value = new EditorNull(Session);
			features = new List<object>();
			comments = new List<string>();

			PropertyChanged = delegate { };
		}

		public T? GetFeature<T>()
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

		/// <inheritdoc/>
		public override string ToString()
		{
			if (Schema == null)
			{
				return "unknown";
			}
			else if (Value is EditorNull)
			{
				return $"{Schema.Type} {Schema.Name} = null";
			}
			else if (Value is EditorScalarValue editorScalarValue)
			{
				return $"{Schema.Type} {Schema.Name} = {editorScalarValue.Value}";
			}
			else if (Value is EditorDictionary)
			{
				return $"{Schema.Type} {Schema.Name} = {{ ... }}";
			}
			else if (Value is EditorList editorList)
			{
				return $"{Schema.Type} {Schema.Name} = [{editorList.Elements.Count}]";
			}
			else
			{
				return $"{Schema.Type} {Schema.Name}";
			}
		}
	}
}
