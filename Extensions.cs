using System.Collections;
using System.Numerics;

namespace AoC_2021;

public static class Extensions
{
    public static uint ToUInt(this BitArray value)
    {
        var result = new uint[1];
        value.CopyTo(result, 0);
        return result[0];
    }

    public static bool IsLower(this string value) => value.All(char.IsLower);

    public static bool IsUpper(this string value) => value.All(char.IsUpper);

    public static void Times(this int value, Action action)
    {
        for (var i = 0; i < value; i++)
            action();
    }

    public static IEnumerable<T> Times<T>(this int value, Func<T> func)
    {
        for (var i = 0; i < value; i++)
            yield return func();
    }

    public static IEnumerable<(T first, T second)> Permute<T>(this IEnumerable<T> value)
    {
        var arr = value.ToArray();

        foreach (var u in arr)
        foreach (var v in arr)
            yield return (u, v);
    }

    public static BigInteger Sum(this IEnumerable<BigInteger> value) => value.Aggregate(new BigInteger(0), (acc, item) => acc + item);

    public static int Sum(this int[,] value) => value.Cast<int>().Sum();

    public static double GetMedian(this IEnumerable<int> value)
    {
        var arr = value.OrderBy(x => x).ToArray();
        var length = arr.Length;
        return length % 2 == 0
            ? (arr[length / 2 - 1] + arr[length / 2]) / 2
            : arr[(length - 1) / 2];
    }

    public static string Concat(this IEnumerable<char> value) => string.Concat(value);

    public static int Concat(this IEnumerable<int> value) => value.Aggregate(0, (acc, val) => acc * 10 + val);

    public static int Product(this IEnumerable<int> value) => value.Aggregate(1, (acc, val) => acc * val);

    public static long Product(this IEnumerable<long> value) => value.Aggregate(1L, (acc, val) => acc * val);

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> value) where TKey : notnull =>
        value.ToDictionary(pair => pair.Key, pair => pair.Value);
}
