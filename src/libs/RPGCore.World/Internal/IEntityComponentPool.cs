namespace RPGCore.World
{
	internal interface IEntityComponentPool
	{
		void SetCapacity(int capacity);
		void Reset(int index);
		void CopyData(int srcIdx, int dstIdx);
		object GetObject(int index);
	}
}
