namespace RPGCore.Inventory.Slots
{
	public enum ItemTransactionType
	{
		/// <summary>
		/// <para>Represents no inventory action.</para>
		/// </summary>
		None,

		/// <summary>
		/// <para>Represents a tranaction where items where moved from one inventory to another.</para>
		/// </summary>
		Move,

		/// <summary>
		/// <para>Represents a tranaction where items where added to an inventory.</para>
		/// </summary>
		Add,
		
		/// <summary>
		/// <para>Represents a tranaction where items where destroyed from an inventory.</para>
		/// </summary>
		/// <remarks>
		/// <para>This may have been caused by consuming, crafting, or user/admin action.
		/// </remarks>
		Destroy,

		TransferIn,

		TransferOut
	}
}
