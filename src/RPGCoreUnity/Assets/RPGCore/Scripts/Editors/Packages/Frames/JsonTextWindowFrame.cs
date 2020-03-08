using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Packages;
using System.IO;
using System.Text;

namespace RPGCore.Unity.Editors
{
	public class JsonTextWindowFrame : RawTextWindowFrame
	{
		private const string KeyColor = "#0451a5";
		private const string StringValueColor = "#a31515";
		private const string NumericValueColor = "#098658";

		// private const string BoolValueColor = "#0000ff";

		protected override string[] BuildLines(IResource resource)
		{
			var serializer = new JsonSerializer();
			JObject jobject;

			using (var stream = resource.LoadStream())
			using (var reader = new StreamReader(stream))
			using (var jsonReader = new JsonTextReader(reader))
			{
				jobject = serializer.Deserialize<JObject>(jsonReader);
			}

			string allLines = jobject.ToString(Formatting.Indented);
			string[] builtLines = HighlightSyntax(allLines);

			return builtLines;
		}

		private static string[] HighlightSyntax(string allLines)
		{
			string[] builtLines = allLines.Split(new char[] { '\n' }, System.StringSplitOptions.None);

			var sb = new StringBuilder();

			for (int i = 0; i < builtLines.Length; i++)
			{
				string oldLine = builtLines[i];

				int quoteIndex = 0;
				bool isValue = false;
				bool isToken = false;
				sb.Clear();

				foreach (char c in oldLine)
				{
					if (c == '"')
					{
						if (quoteIndex == 0)
						{
							sb.Append("<color=" + KeyColor + ">");
							sb.Append(c);
						}
						else if (quoteIndex == 1)
						{
							sb.Append(c);
							sb.Append("</color>");
						}
						else if (quoteIndex == 2)
						{
							sb.Append("<color=" + StringValueColor + ">");
							sb.Append(c);
						}
						else if (quoteIndex == 3)
						{
							sb.Append(c);
							sb.Append("</color>");
						}
						quoteIndex++;
						continue;
					}
					else if (c == ':')
					{
						isValue = true;
					}
					else if (isToken && c == ',')
					{
						sb.Append("</color>");
						isToken = false;
					}
					else if (quoteIndex == 2
						&& isValue
						&& !isToken
						&& c != '{'
						&& c != '}'
						&& c != '['
						&& c != ']'
						&& c != ','
						&& c != '.'
						&& !char.IsWhiteSpace(c))
					{
						isToken = true;

						sb.Append("<color=" + NumericValueColor + ">");
					}

					sb.Append(c);
				}

				if (isToken)
				{
					sb.Append("</color>");
				}

				builtLines[i] = sb.ToString();
			}

			return builtLines;
		}

		public JsonTextWindowFrame(IResource resource)
			: base(resource)
		{
		}

		public JsonTextWindowFrame(string text)
			: base(HighlightSyntax(text))
		{
		}
	}
}
