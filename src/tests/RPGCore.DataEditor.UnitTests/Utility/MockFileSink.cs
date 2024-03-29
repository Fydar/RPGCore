﻿using RPGCore.DataEditor.Files;
using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.DataEditor.UnitTests.Utility;

public class MockFileSink : IFileSaver
{
	public List<string> SaveHistory { get; }

	public MockFileSink()
	{
		SaveHistory = new List<string>();
	}

	/// <inheritdoc/>
	public void Save(EditorSession editorSession, TypeName type, IEditorValue save)
	{
		using var memoryStream = new MemoryStream();
		editorSession.Serializer.SerializeValue(save, type, memoryStream);

		memoryStream.Seek(0, SeekOrigin.Begin);
		using var reader = new StreamReader(memoryStream);

		string result = reader.ReadToEnd();
		SaveHistory.Add(result);
		Console.WriteLine(result);
	}
}
