using RPGCore.Stats;
using RPGCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class StatInstanceElement : TooltipElement, ITooltipTarget<StatInstance>
	{
		[SerializeField] private Text statDescription;
		[SerializeField] private ProgressBar progress;

		public void Render (StatInstance target)
		{
			statDescription.text = target.Info.Description;

			if (target.Info.MinValue.Enabled && target.Info.MaxValue.Enabled)
			{
				progress.gameObject.SetActive (true);

				progress.SetLabels (target.Info.RenderValue (target.Info.MinValue.Value),
					target.Info.RenderValue (target.Info.MaxValue.Value),
					target.Info.RenderValue (target.Value));

				progress.SetFill (Mathf.InverseLerp (target.Info.MinValue.Value,
					target.Info.MaxValue.Value, target.Value));
			}
			else
			{
				progress.gameObject.SetActive (false);
			}
		}
	}
}
