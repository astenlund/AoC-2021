using Terminal.Gui;

namespace AoC_2021;

public class DigitKeyHandler
{
    private readonly Key[] DigitKeys =
    {
        Key.D1,
        Key.D2,
        Key.D3,
        Key.D4,
        Key.D5,
        Key.D6,
        Key.D7,
        Key.D8,
        Key.D9,
        Key.D0
    };

    private readonly List<Key> KeyPressHistory = new();
    private readonly TimeSpan Timeout;

    private DateTime LastKeyPress = DateTime.MinValue;

    public DigitKeyHandler(TimeSpan timeout) => Timeout = timeout;

    public int? Handle(Key key)
    {
        if (!DigitKeys.Contains(key))
            return null;

        if (DateTime.UtcNow > LastKeyPress + Timeout)
            KeyPressHistory.Clear();

        LastKeyPress = DateTime.UtcNow;
        KeyPressHistory.Add(key);

        return KeyPressHistory.Select(k => (int)(k - Key.D0)).Concat();
    }
}
