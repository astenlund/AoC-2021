namespace AoC_2021;

public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
{
    private readonly Func<TValue> Init;

    public DefaultDictionary(Func<TValue> init) => Init = init;

    public new TValue this[TKey key]
    {
        get
        {
            if (!ContainsKey(key))
                Add(key, Init());

            return base[key];
        }

        set => base[key] = value;
    }
}
