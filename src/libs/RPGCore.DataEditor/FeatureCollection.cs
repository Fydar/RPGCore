using System.Collections.Generic;

namespace RPGCore.DataEditor
{
	public abstract class FeatureCollection
	{
		internal readonly List<IEditorFeature> features;

		private readonly IEditorToken token;

		internal FeatureCollection(IEditorToken token)
		{
			this.token = token;
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

	public class FeatureCollection<TToken> : FeatureCollection
		where TToken : IEditorToken
	{
		public FeatureCollection(IEditorToken token) : base(token)
		{
		}
	}
}
