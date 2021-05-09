using RPGCore.DataEditor.Manifest;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// A builder for <see cref="EditorFile"/>.
	/// </summary>
	public class EditorFileBuilder
	{
		private readonly EditorSession editorSession;

		private IFileLoader? fileLoader;
		private IFileSaver? fileSaver;
		private TypeName type;

		internal EditorFileBuilder(EditorSession editorSession)
		{
			this.editorSession = editorSession;
			type = TypeName.Unknown;
		}

		/// <summary>
		/// Specifies the source that the constructed <see cref="EditorFile"/> should load from.
		/// </summary>
		/// <param name="fileLoader">The <see cref="IFileLoader"/> that the constructed <see cref="EditorFile"/> should load from.</param>
		/// <returns>Returns the current instance of the <see cref="EditorFileBuilder"/>.</returns>
		public EditorFileBuilder LoadFrom(IFileLoader fileLoader)
		{
			this.fileLoader = fileLoader;
			return this;
		}

		/// <summary>
		/// Specifies the destination that the constructed <see cref="EditorFile"/> should save to.
		/// </summary>
		/// <param name="fileSaver">The <see cref="IFileSaver"/> that the constructed <see cref="EditorFile"/> should save to.</param>
		/// <returns>Returns the current instance of the <see cref="EditorFileBuilder"/>.</returns>
		public EditorFileBuilder SaveTo(IFileSaver fileSaver)
		{
			this.fileSaver = fileSaver;
			return this;
		}

		/// <summary>
		/// Specifies the source and destination that the constructed <see cref="EditorFile"/> should load from and save to.
		/// </summary>
		/// <param name="loaderAndSaver">The <see cref="IFileLoader"/> and <see cref="IFileSaver"/> that the constructed <see cref="EditorFile"/> should load from and save to.</param>
		/// <returns>Returns the current instance of the <see cref="EditorFileBuilder"/>.</returns>
		public EditorFileBuilder LoadFromAndSaveTo<T>(T loaderAndSaver)
			where T : IFileLoader, IFileSaver
		{
			fileSaver = loaderAndSaver;
			fileLoader = loaderAndSaver;
			return this;
		}

		/// <summary>
		/// Specifies the base <see cref="TypeName"/> that the file should use.
		/// </summary>
		/// <param name="type">The base <see cref="TypeName"/> that the file editor should use.</param>
		/// <returns>Returns the current instance of the <see cref="EditorFileBuilder"/>.</returns>
		public EditorFileBuilder WithType(TypeName type)
		{
			this.type = type;
			return this;
		}

		/// <summary>
		/// Starts editing a file using configuration provided by this <see cref="EditorFileBuilder"/>.
		/// </summary>
		/// <returns>A new <see cref="EditorFile"/> from the current state of this <see cref="EditorFileBuilder"/>.</returns>
		public EditorFile Build()
		{
			var loader = fileLoader;

			if (loader == null)
			{
				loader = new TypeDefaultLoader();
			}

			return new EditorFile(editorSession, fileLoader, fileSaver, type);
		}
	}
}
