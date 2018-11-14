namespace RPGCore.Inventories
{
	public class ClingyCondition : ItemCondition
	{
		public ClingyCondition ()
		{

		}

		public override bool IsValid (ItemSurrogate item)
		{
			return item != null;
		}
	}
}