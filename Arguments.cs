internal class Arguments
{
	internal static bool Valid;
	internal static string Source = string.Empty;
	internal static string Destination = string.Empty;

	internal static void Parse(string[] arguments)
	{
		Valid = false;

		if(arguments == null ||
			arguments.Length < 1)
			return;

		Source = arguments[0];

		if (arguments.Length > 1)
			Destination = arguments[1];
		else
			Destination = Path.Combine(Path.GetDirectoryName(Source) ?? string.Empty, Path.GetFileNameWithoutExtension(Source) + ".bmp");

		Valid = true;
	}
}