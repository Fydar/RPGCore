using UnityEngine;

namespace RPGCore.Stats
{
	[CreateAssetMenu (menuName = "RPGCore/Stat/Info")]
	public class StatInformation : AttributeInformation
	{
		[Header ("Render")]
		public string Abbreviation = "STAT";

#if ASSET_ICONS
		[AssetIcon]
#endif
		public Color32 StatColour = new Color32 (255, 255, 255, 255);

		[Header ("Maths")]
		public ToggleFloatField MinValue = new ToggleFloatField (true, 0.0f);
		public ToggleFloatField MaxValue = new ToggleFloatField (false, 1.0f);

		public override float Filter (float original)
		{
			original = base.Filter (original);

			if (MinValue.Enabled)
				original = Mathf.Max (original, MinValue.Value);

			if (MaxValue.Enabled)
				original = Mathf.Min (original, MaxValue.Value);

			return base.Filter (original);
		}
	}
}