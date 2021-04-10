namespace RPGCore.DataEditor.Manifest
{
	public struct SocketInformation
	{
		public string Description { get; set; }
		public string Type { get; set; }
		/*
		public static SocketInformation Construct(FieldInfo field, InputMap socket)
		{
			var socketInformation = new SocketInformation
			{
				Type = socket.ConnectionType.Name
			};

			return socketInformation;
		}

		public static SocketInformation Construct(FieldInfo field, OutputMap socket)
		{
			var socketInformation = new SocketInformation
			{
				Type = socket.ConnectionType.Name
			};

			return socketInformation;
		}*/
	}
}
