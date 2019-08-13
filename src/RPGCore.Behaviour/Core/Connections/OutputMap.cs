﻿using System;

namespace RPGCore.Behaviour
{
	public struct OutputMap
	{
		public int ConnectionId;
		public Type ConnectionType;

		public OutputMap (OutputSocket source, Type connectionType)
		{
			ConnectionType = connectionType;
			ConnectionId = source.Id;
		}

		public override string ToString ()
		{
			if (ConnectionId == -1)
			{
				return $"Unmapped Output of type {ConnectionType}";
			}
			else
			{
				return $"Output {ConnectionId} of type {ConnectionType}";
			}
		}
	}
}
