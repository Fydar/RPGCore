using Newtonsoft.Json.Linq;
using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	public class EditorField
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] public EditorSession Session { get; }

		public FieldInformation Field { get; }
		public TypeInformation FieldType { get; }

		private readonly JProperty json;
		private readonly List<object> features;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly IEditorValue valueInternal;

		public IEditorValue Value => valueInternal;

		public EditorField(EditorSession session, FieldInformation field, JProperty json)
		{
			Session = session;
			Field = field;

			if (field.Type != "JObject")
			{
				FieldType = session.Manifest.GetTypeInformation(field.Type);
				if (FieldType == null)
				{
					throw new InvalidOperationException($"Failed to find type for {field.Type}");
				}
			}
			else
			{
				string typeName = json.Parent["Type"].ToString();
				FieldType = session.Manifest.GetTypeInformation(typeName);
				if (FieldType == null)
				{
					throw new InvalidOperationException($"Failed to find type for {typeName}");
				}
			}

			this.json = json;
			features = new List<object>();

			valueInternal = session.CreateValue(FieldType, field.Wrapper, json.Value);
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

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{Field.Type}";
		}
	}
}
