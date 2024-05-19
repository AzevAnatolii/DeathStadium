internal static class NameChecker
{
	public static bool IsNameValid(string name)
	{
		return name.Length is > 3 and < 10;
	}
}