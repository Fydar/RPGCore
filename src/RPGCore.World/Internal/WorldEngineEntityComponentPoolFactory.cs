using System;

namespace RPGCore.World
{
	internal class WorldEngineEntityComponentPoolFactory
	{
		internal Type FeatureType { get; set; }
		internal int FeatureIdentifier { get; set; }
		internal Func<int, IEntityComponentPool> ConstructEntityComponentPool { get; set; }

		public WorldEngineEntityComponentPoolFactory(
			Type featureType,
			int featureIdentifier,
			Func<int, IEntityComponentPool> constructEntityComponentPool)
		{
			FeatureType = featureType;
			FeatureIdentifier = featureIdentifier;
			ConstructEntityComponentPool = constructEntityComponentPool;
		}
	}
}
