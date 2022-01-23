using System;

namespace RPGCore.Demo.BoardGame;

[Flags]
public enum BuildingOrientation : byte
{
	None = 0b_000,
	MirrorX = 0b_001,
	MirrorY = 0b_010,
	MirrorXandY = 0b_011,

	Rotate90 = 0b_100,
	Rotate90MirrorX = 0b_101,
	Rotate90MirrorY = 0b_110,
	Rotate90MirrorXandY = 0b_111,
}
