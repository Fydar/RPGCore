using RPGCore.DataEditor.Manifest;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace RPGCore.DataEditor
{
	/// <inheritdoc/>
	public class JsonEditorSerializer : IEditorSerializer
	{
		/// <inheritdoc/>
		public IEditorValue DeserializeValue(EditorSession session, SchemaQualifiedType type, ReadOnlySpan<byte> output)
		{
			var reader = new Utf8JsonReader(output, true, new JsonReaderState(new JsonReaderOptions()
			{
				CommentHandling = JsonCommentHandling.Allow,
				AllowTrailingCommas = true,
			}));

			if (!reader.Read())
			{
				throw new InvalidOperationException("Unable to read value as the data is empty.");
			}

			return ReadValue(session, type, ref reader);
		}

		private IEditorValue ReadValue(EditorSession session, SchemaQualifiedType type, ref Utf8JsonReader reader)
		{
			var typeInfo = session.ResolveType(type);

			if (type.Identifier == "[Array]")
			{
				return ReadArray(session, type, ref reader);
			}
			else if (type.Identifier == "[Dictionary]")
			{
				return ReadDictionary(session, type, ref reader);
			}
			else if (typeInfo.Fields != null && typeInfo.Fields.Count > 0)
			{
				return ReadObject(session, type, ref reader);
			}
			else
			{
				return ReadScalarValue(session, type, ref reader);
			}
		}

		private IEditorValue ReadObject(EditorSession session, SchemaQualifiedType type, ref Utf8JsonReader reader)
		{
			var tokenType = reader.TokenType;
			if (tokenType == JsonTokenType.Null)
			{
				return new EditorNull(session);
			}
			else if (tokenType != JsonTokenType.StartObject)
			{
				throw new InvalidDataException("Unexpected token.");
			}

			var editorObject = new EditorObject(session, type);

			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.EndObject:
						return editorObject;

					case JsonTokenType.PropertyName:
						string? propertyName = reader.GetString();
						var field = editorObject.Fields.FirstOrDefault(f => f.Name == propertyName);

						if (!reader.Read())
						{
							throw new InvalidOperationException("Unable to read value as the data is empty.");
						}

						var fieldValue = ReadValue(session, field.Schema.Type, ref reader);
						field.Value = fieldValue;
						break;

					case JsonTokenType.Comment:

						break;

					default:
						throw new ArgumentException();
				}
			}

			return new EditorNull(session);
		}

		private IEditorValue ReadArray(EditorSession session, SchemaQualifiedType type, ref Utf8JsonReader reader)
		{
			var tokenType = reader.TokenType;
			if (tokenType == JsonTokenType.Null)
			{
				return new EditorNull(session);
			}
			else if (tokenType != JsonTokenType.StartArray)
			{
				throw new InvalidDataException($"Unexpected token {tokenType}.");
			}

			var elementType = type.TemplateTypes[0];
			var editorList = new EditorList(session, type);

			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.EndArray:
						return editorList;

					case JsonTokenType.Comment:
						break;

					default:
						var elementValue = ReadValue(session, elementType, ref reader);
						editorList.Elements.Add(elementValue);
						break;
				}
			}

			throw new InvalidOperationException("Invalid syntax");
		}

		private IEditorValue ReadDictionary(EditorSession session, SchemaQualifiedType type, ref Utf8JsonReader reader)
		{
			var tokenType = reader.TokenType;
			if (tokenType == JsonTokenType.Null)
			{
				return new EditorNull(session);
			}
			else if (tokenType != JsonTokenType.StartObject)
			{
				throw new InvalidDataException($"Unexpected token {tokenType}.");
			}

			var keyType = type.TemplateTypes[0];
			var valueType = type.TemplateTypes[1];
			var editorDictionary = new EditorDictionary(session, keyType, valueType);

			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonTokenType.EndObject:
						return editorDictionary;

					case JsonTokenType.Comment:
						break;

					case JsonTokenType.PropertyName:
						var keyValue = ReadScalarValue(session, keyType, ref reader);

						if (!reader.Read())
						{
							throw new InvalidOperationException("Unable to read value as the data is empty.");
						}

						var elementValue = ReadValue(session, valueType, ref reader);
						editorDictionary.KeyValuePairs.Add(new EditorKeyValuePair(editorDictionary, keyValue, elementValue));
						break;

					default:
						throw new InvalidOperationException("Invalid syntax");
				}
			}

			throw new InvalidOperationException("Invalid syntax");
		}

		private IEditorValue ReadScalarValue(EditorSession session, SchemaQualifiedType type, ref Utf8JsonReader reader)
		{
			if (reader.TokenType == JsonTokenType.Null)
			{
				if (type.IsNullable)
				{
					return new EditorNull(session);
				}
				else
				{
					throw new InvalidOperationException($"Unable to assign null value to type {type}.");
				}
			}
			else if (type.Identifier == "string")
			{
				return new EditorScalarValue(session, type)
				{
					Value = reader.GetString()
				};
			}
			else if (type.Identifier == "int")
			{
				if (reader.TryGetInt32(out int value))
				{
					return new EditorScalarValue(session, type)
					{
						Value = value
					};
				}
			}
			else if (type.Identifier == "long")
			{
				if (reader.TryGetInt64(out long value))
				{
					return new EditorScalarValue(session, type)
					{
						Value = value
					};
				}
			}

			throw new InvalidOperationException($"unknown type {type.Identifier}");
		}

		/// <inheritdoc/>
		public void SerializeValue(IEditorValue value, Stream output)
		{
			using var writer = new Utf8JsonWriter(output, new JsonWriterOptions()
			{
				Indented = true
			});
			SerializeValue(value, writer);
		}

		private void SerializeValue(IEditorValue value, Utf8JsonWriter writer)
		{
			switch (value)
			{
				case EditorScalarValue editorScalar:
				{
					switch (editorScalar.Value)
					{
						case string stringValue:
						{
							writer.WriteStringValue(stringValue);
							break;
						}
						case long numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case ulong numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case int numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case uint numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case short numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case ushort numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case byte numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case sbyte numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case decimal numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case double numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case float numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
					}
					break;
				}
				case EditorObject editorObject:
				{
					writer.WriteStartObject();

					foreach (var field in editorObject.Fields)
					{
						writer.WritePropertyName(field.Name);
						SerializeValue(field.Value, writer);
					}

					writer.WriteEndObject();
					break;
				}
				case EditorDictionary editorDictionary:
				{
					writer.WriteStartObject();

					foreach (var field in editorDictionary.KeyValuePairs)
					{
						var editorScalar = field.Key as EditorScalarValue;
						switch (editorScalar?.Value)
						{
							case string stringValue:
							{
								break;
							}
						}

						string? key = editorScalar?.Value as string;
						writer.WritePropertyName(key!);
						SerializeValue(field.Value, writer);
					}

					writer.WriteEndObject();
					break;
				}
				case EditorList editorList:
				{
					writer.WriteStartArray();

					foreach (var element in editorList.Elements)
					{
						SerializeValue(element, writer);
					}

					writer.WriteEndArray();
					break;
				}
				case EditorNull:
				{
					writer.WriteNullValue();
					break;
				}
				case null:
					throw new ArgumentNullException("Cannot serialize \"null\" editor value.");
				default:
					throw new ArgumentException($"Cannot serialize editor value of type \"{value.GetType().Name}\".");
			}
		}
	}
}
