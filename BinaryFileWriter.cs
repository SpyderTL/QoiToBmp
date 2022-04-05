namespace QoiToBmp
{
	internal static class BinaryFileWriter
	{
		internal static Stream? Stream;
		internal static BinaryWriter? Writer;

		internal static void Open(string path)
		{
			Stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			Writer = new BinaryWriter(Stream);
		}

		internal static void Close()
		{
			Writer?.Flush();

			Writer?.Close();
			Stream?.Close();

			Writer?.Dispose();
			Stream?.Dispose();

			Writer = null;
			Stream = null;
		}
	}
}