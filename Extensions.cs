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

    public static void Times(this int value, Action action)
    {
        for (var i = 0; i < value; i++)
            action();
    }

    public static BigInteger Sum(this IEnumerable<BigInteger> value) => value.Aggregate(new BigInteger(0), (acc, item) => acc + item);

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
}
