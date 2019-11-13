﻿using RPGCore.UI;
using System;
using UnityEngine;

namespace RPGCore
{
	[Serializable] public class BuffIndicatorPool : UIPool<BuffIndicator> { }

	public class BuffsBar : MonoBehaviour
	{
		public RPGCharacter TargetCharacter;

		[Space]
		public Transform BuffsHolder;
		public Transform DebuffsHolder;

		[Space]
		public BuffIndicatorPool Indicator;

		private void Start()
		{
			Indicator.Flush();
			Subscribe(TargetCharacter);
		}

		private void Subscribe(RPGCharacter character)
		{
			character.Buffs.OnAddBuff += CreateIndicator;
		}

		private void CreateIndicator(Buff buff)
		{
			if (buff.buffTemplate.Type == BuffType.None)
			{
				return;
			}

			Transform parent;
			if (buff.buffTemplate.Type == BuffType.Debuff)
			{
				parent = DebuffsHolder;
			}
			else
			{
				parent = BuffsHolder;
			}

			var indicator = Indicator.Grab(parent);
			indicator.transform.localScale = Vector3.one;

			indicator.Setup(this, buff);
		}
	}
}

