using System.Drawing;

namespace QoiToBmp
{
	public static class QoiFile
	{
		public static char[] Signature = Array.Empty<char>();
		public static uint Width;
		public static uint Height;
		public static byte Channels;
		public static byte Colorspace;
		public static QoiBlock[] Blocks = Array.Empty<QoiBlock>();
	}

	public struct QoiBlock
	{
		public QoiBlockType Type;
		public byte Red;
		public byte Green;
		public byte Blue;
		public byte Alpha;
		public byte Index;
		public byte Run;
	}

	public enum QoiBlockType : byte
	{
		Index = 0x00,
		Rgb = 0xFE,
		Rgba = 0xFF,
		Diff = 0x40,
		Luma = 0x80,
		Run = 0xC0,
	}
}