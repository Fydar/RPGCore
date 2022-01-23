using System;

namespace RPGCore.DataEditor.Manifest.Source.RuntimeSource;

public interface IRuntimeTypeConverter
{
	SchemaType Convert(Type type);
}
