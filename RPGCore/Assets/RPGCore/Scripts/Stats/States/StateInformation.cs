using System;
using UnityEngine;
using RPGCore.Utility;

namespace RPGCore.Stats
{

	[Serializable]
	public class ToggleFloatField : TogglableField<float>
	{
		public ToggleFloatField (bool enabled, float value) : base (enabled, value) { }
	}

	[CreateAssetMenu (menuName = "RPGCore/State/Info")]
	public class StateInformation : AttributeInformation
	{

	}
}