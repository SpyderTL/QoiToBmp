namespace QoiToBmp
{
	internal static class BinaryFileReader
	{
		internal static Stream? Stream;
		internal static BinaryReader? Reader;

		internal static void Open(string source)
		{
			Stream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read);
			Reader = new BinaryReader(Stream);
		}

		internal static void Close()
		{
			Reader?.Close();
			Stream?.Close();

			Reader = null;
			Stream = null;
		}
	}
}