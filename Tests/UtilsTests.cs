using System.Collections.Generic;
using NUnit.Framework;
using VkServer.Utils;

namespace Tests;

public class UtilsTests
{
	[Test]
	public void GetLetterOccurrences_TestOnRussianText()
	{
		var text = new[] { "Привет, мир!", "Мы - лучше всех!" };
		var expected = new SortedDictionary<char, int>
		{
			{ 'в', 2 },
			{ 'е', 3 },
			{ 'и', 2 },
			{ 'л', 1 },
			{ 'м', 2 },
			{ 'п', 1 },
			{ 'р', 2 },
			{ 'с', 1 },
			{ 'т', 1 },
			{ 'у', 1 },
			{ 'х', 1 },
			{ 'ч', 1 },
			{ 'ш', 1 },
			{ 'ы', 1 }
		};

		Assert.AreEqual(expected, text.GetLetterOccurrences());
	}
}