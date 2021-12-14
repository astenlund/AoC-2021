namespace AoC_2021;

public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
{
    private readonly TValue Default;

    public DefaultDictionary(TValue defaultValue) => Default = defaultValue;

    public new TValue this[TKey key]
    {
        get
        {
            if (!ContainsKey(key))
                Add(key, Default);

            return base[key];
        }

        set => base[key] = value;
    }
}
