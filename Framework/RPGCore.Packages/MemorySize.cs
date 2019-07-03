using System;
using System.IO;

namespace RPGCore.Packages
{
	/// <summary>
	/// A simple structure that formats for bytes. 
	/// </summary>
	public struct MemorySize : IEquatable<MemorySize>
	{
		public const ulong Denomination = 1024;

		private const ulong KilobyteSize = Denomination;
		private const ulong MegabyteSize = KilobyteSize * Denomination;
		private const ulong GigabyteSize = MegabyteSize * Denomination;
		private const ulong TerrabyteSize = GigabyteSize * Denomination;

		private const string ByteSuffix = "b";
		private const string KilobyteSuffix = "kB";
		private const string MegabyteSuffix = "MB";
		private const string GigabyteSuffix = "MB";

		public ulong Bytes { get; set; }

		public float Kilobytes
		{
			get
			{
				return ((float)Bytes) / KilobyteSize;
			}
			set
			{
				Bytes = (ulong)(value * KilobyteSize);
			}
		}

		public float Megabytes
		{
			get
			{
				return ((float)Bytes) / KilobyteSize;
			}
			set
			{
				Bytes = (ulong)(value * KilobyteSize);
			}
		}

		public float Gigabytes
		{
			get
			{
				return ((float)Bytes) / GigabyteSize;
			}
			set
			{
				Bytes = (ulong)(value * GigabyteSize);
			}
		}

		public float Terrabytes
		{
			get
			{
				return ((float)Bytes) / TerrabyteSize;
			}
			set
			{
				Bytes = (ulong)(value * TerrabyteSize);
			}
		}

		public MemorySize (ulong bytes)
		{
			Bytes = bytes;
		}

		public static MemorySize SizeOf (string data)
		{
			return new MemorySize ((ulong)data.Length * 2);
		}

		public static MemorySize SizeOf (byte[] data)
		{
			return new MemorySize ((ulong)data.Length);
		}

		public static MemorySize SizeOf (FileInfo file)
		{
			return new MemorySize ((ulong)file.Length);
		}

		public override string ToString ()
		{
			if (Gigabytes < 1.0f)
			{
				if (Megabytes < 1.0f)
				{
					if (Kilobytes < 1.0f)
					{
						return $"{Bytes.ToString ()} {ByteSuffix}";
					}
					return $"{Kilobytes.ToString ()} {KilobyteSuffix}";
				}
				return $"{Megabytes.ToString ()} {MegabyteSuffix}";
			}
			return $"{Gigabytes.ToString ()} {GigabyteSuffix}";
		}

		public override bool Equals (object obj) => obj is MemorySize size && Equals (size);

		public bool Equals (MemorySize other) => Bytes == other.Bytes && Kilobytes == other.Kilobytes && Megabytes == other.Megabytes && Gigabytes == other.Gigabytes && Terrabytes == other.Terrabytes;

		public override int GetHashCode ()
		{
			int hashCode = 263278701;
			hashCode = hashCode * -1521134295 + Bytes.GetHashCode ();
			hashCode = hashCode * -1521134295 + Kilobytes.GetHashCode ();
			hashCode = hashCode * -1521134295 + Megabytes.GetHashCode ();
			hashCode = hashCode * -1521134295 + Gigabytes.GetHashCode ();
			hashCode = hashCode * -1521134295 + Terrabytes.GetHashCode ();
			return hashCode;
		}

		public static bool operator == (MemorySize left, MemorySize right)
		{
			return left.Equals (right);
		}

		public static bool operator != (MemorySize left, MemorySize right)
		{
			return !(left == right);
		}
	}
}
