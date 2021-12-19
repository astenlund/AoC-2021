namespace AoC_2021.Solutions;

public class Day16 : DayBase
{
    internal Day16(string session, string? input = null) : base(session, input)
    {
    }

    private protected override ushort Day => 16;

    public override async Task<string> PartOne()
    {
        await Initialize();

        var bits = ConvertToBits(Input!);
        var versions = new List<long>();

        _ = Unwrap(bits, versions);

        var result = versions.Sum();

        return result.ToString();
    }

    public override async Task<string> PartTwo()
    {
        await Initialize();

        var bits = ConvertToBits(Input!);
        var versions = new List<long>();

        var (result, _) = Unwrap(bits, versions);

        return result.ToString();
    }

    private static (long values, int length) Unwrap(string bits, ICollection<long> versions)
    {
        if (bits.Length < 11 || bits.All(b => b == '0'))
            return (0L, 0);

        var version = Convert.ToInt32(bits[..3], 2);
        var packetType = (PacketType)Convert.ToInt32(bits[3..6], 2);

        versions.Add(version);

        if (packetType == PacketType.Literal)
        {
            var literalBits = bits[6..];
            var (value, length) = ParseLiteralValue(literalBits);

            return (value, 6 + length);
        }

        switch (bits[6])
        {
            case '0':
            {
                var subPacketLength = Convert.ToInt32(bits[7..22], 2);
                var subPacketBits = bits[22..(22 + subPacketLength)];
                var lengthTotal = 0;
                var values = new List<long>();

                while (lengthTotal < subPacketLength)
                {
                    var (value, length) = Unwrap(subPacketBits, versions);
                    subPacketBits = subPacketBits[length..];
                    lengthTotal += length;
                    values.Add(value);
                }

                var result = PerformOperation(packetType, values);

                return (result, 22 + subPacketLength);
            }
            case '1':
            {
                var numSubPackets = Convert.ToInt32(bits[7..18], 2);
                var subPacketBits = bits[18..];
                var lengthTotal = 0;
                var values = new List<long>();

                for (var i = 0; i < numSubPackets; i++)
                {
                    var (value, length) = Unwrap(subPacketBits, versions);
                    subPacketBits = subPacketBits[length..];
                    lengthTotal += length;
                    values.Add(value);
                }

                var result = PerformOperation(packetType, values);

                return (result, 18 + lengthTotal);
            }
        }

        return (0L, 0);
    }

    private static long PerformOperation(PacketType packetType, IList<long> values) => packetType switch
    {
        PacketType.Sum => values.Sum(),
        PacketType.Product => values.Product(),
        PacketType.Minimum => values.Min(),
        PacketType.Maximum => values.Max(),
        PacketType.GreaterThan => values[0] > values[1] ? 1L : 0L,
        PacketType.LessThan => values[0] < values[1] ? 1L : 0L,
        PacketType.EqualTo => values[0] == values[1] ? 1L : 0L,
        PacketType.Literal => values[0],
        _ => 0L
    };

    private static (long value, int length) ParseLiteralValue(string bits)
    {
        var literalValueBits = "";
        var prefix = 1;
        var length = 0;

        for (var i = 0; prefix == 1; i += 5)
        {
            prefix = bits[i] - '0';
            literalValueBits += bits[(i + 1)..(i + 5)].Concat();
            length += 5;
        }

        var value = Convert.ToInt64(literalValueBits, 2);

        return (value, length);
    }

    private static string ConvertToBits(string hexString) => string.Concat(hexString.ToUpper().Select(c => c switch
    {
        '0' => "0000",
        '1' => "0001",
        '2' => "0010",
        '3' => "0011",
        '4' => "0100",
        '5' => "0101",
        '6' => "0110",
        '7' => "0111",
        '8' => "1000",
        '9' => "1001",
        'A' => "1010",
        'B' => "1011",
        'C' => "1100",
        'D' => "1101",
        'E' => "1110",
        'F' => "1111",
        _ => ""
    }));

    private enum PacketType
    {
        Sum         = 0b000,
        Product     = 0b001,
        Minimum     = 0b010,
        Maximum     = 0b011,
        Literal     = 0b100,
        GreaterThan = 0b101,
        LessThan    = 0b110,
        EqualTo     = 0b111
    }
}
