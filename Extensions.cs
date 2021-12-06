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
}
