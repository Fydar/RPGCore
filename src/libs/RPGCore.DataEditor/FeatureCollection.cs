using System.Collections.Generic;

namespace RPGCore.DataEditor
{
	public abstract class FeatureCollection
	{
		internal readonly List<IEditorFeature> features;

		internal FeatureCollection()
		{
			features = new List<IEditorFeature>();
		}

		public T? GetFeature<T>()
			where T : class, IEditorFeature
		{
			var getFeatureType = typeof(T);
			foreach (var feature in features)
			{
				var featureType = feature.GetType();
				if (getFeatureType.IsAssignableFrom(featureType))
				{
					return (T)feature;
				}
			}
			return null;
		}
	}

	public class FeatureCollection<TToken> : FeatureCollection
		where TToken : IEditorToken
	{
		private readonly TToken token;

		internal FeatureCollection(TToken token)
		{
			this.token = token;
		}

		public T GetOrCreateFeature<T>()
			where T : class, IEditorFeature, new()
		{
			var feature = GetFeature<T>();

			if (feature == null)
			{
				feature = new T();
				feature.AttachToToken(token);
				features.Add(feature);
			}
			return feature;
		}
	}
}
