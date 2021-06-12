namespace RPGCore.Documentation.SyntaxHighlighting.Internal
{
	/// <summary>
	/// A token representing a classified span of text.
	/// </summary>
	public readonly struct LexerToken
	{
		/// <summary>
		/// The starting index of the classified span.
		/// </summary>
		public int StartIndex { get; }

		/// <summary>
		/// The length of the classified span.
		/// </summary>
		public int Length { get; }

		/// <summary>
		/// The index of the classifier used to classify the span.
		/// </summary>
		public int Classifier { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="LexerToken"/> struct.
		/// </summary>
		/// <param name="startIndex">The starting index of the classified span.</param>
		/// <param name="length">The length of the classified span.</param>
		/// <param name="classifier">The index of the classifier used to classify the span.</param>
		public LexerToken(int startIndex, int length, int classifier)
		{
			StartIndex = startIndex;
			Length = length;
			Classifier = classifier;
		}
	}
}
