using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// A root data structure for data editing.
	/// </summary>
	public class EditorFile
	{
		private readonly List<object> features;

		/// <summary>
		/// An <see cref="Session"/> that provides editor configuration and state.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		/// <summary>
		/// An <see cref="IEditorValue"/> representing the root of the <see cref="EditorField"/>.
		/// </summary>
		public IEditorValue Root { get; set; }

		internal EditorFile(EditorSession session, SchemaQualifiedType type)
		{
			Session = session;
			features = new List<object>();

			Root = Session.CreateInstatedValue(type);
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
	}
}
