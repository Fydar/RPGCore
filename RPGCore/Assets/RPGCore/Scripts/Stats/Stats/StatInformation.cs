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

		public override string RenderValue (float value)
		{
			if (MinValue.Enabled)
				value = Mathf.Max (value, MinValue.Value);

			if (MaxValue.Enabled)
				value = Mathf.Min (value, MaxValue.Value);

			return base.RenderValue (value);
		}
	}
}
