using System.Collections;

namespace AoC_2021;

public static class Extensions
{
    public static uint ToUInt(this BitArray value)
    {
        var result = new uint[1];
        value.CopyTo(result, 0);
        return result[0];
    }
}
