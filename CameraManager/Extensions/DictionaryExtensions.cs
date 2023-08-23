using System.Collections.Generic;

namespace CameraManager;

static class DictionaryExtensions
{
	public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvPair, out TKey key, out TValue value)
	{
		key = kvPair.Key;
		value = kvPair.Value;
	}

	public static TValue? FirstOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
	{
		return new List<TValue>(dictionary.Values)[0] ?? default;
	}
}
