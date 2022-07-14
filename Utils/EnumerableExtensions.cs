namespace VkServer.Utils;

public static class EnumerableExtensions
{
	public static SortedDictionary<char, int> GetLetterOccurrences(this IEnumerable<string> source)
	{
		var result = new SortedDictionary<char, int>();

		foreach (var text in source)
		{
			foreach (var ch in text.Where(char.IsLetter).Select(char.ToLower))
			{
				if (!result.ContainsKey(ch))
					result.Add(ch, 0);

				result[ch] += 1;
			}
		}

		return result;
	}
}