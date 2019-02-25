using System.Reflection;

namespace RPGCore.Behaviour.Manifest
{
	public struct SocketInformation
	{
		public string Name;
		public string Description;
		public string Type;

		public static SocketInformation Construct (FieldInfo field, InputMap socket)
		{
			var socketInformation = new SocketInformation
			{
				Type = socket.ConnectionType.Name,
				Name = field.Name
			};

			return socketInformation;
		}

		public static SocketInformation Construct (FieldInfo field, OutputMap socket)
		{
			var socketInformation = new SocketInformation
			{
				Type = socket.ConnectionType.Name,
				Name = field.Name
			};

			return socketInformation;
		}
	}
}
