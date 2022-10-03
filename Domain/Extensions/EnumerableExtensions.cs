namespace Domain.Extensions;

public static class EnumerableExtensions
{
	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
	{
		Random rng = new Random();
		T[] elements = source.ToArray();
		for (int i = elements.Length - 1; i > 0; i--)
		{
			int swapIndex = rng.Next(i + 1);
			T tmp = elements[i];
			elements[i] = elements[swapIndex];
			elements[swapIndex] = tmp;
		}
		foreach (T element in elements)
		{
			yield return element;
		}
	}
}
