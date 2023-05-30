namespace WpfTools;

public sealed record Setting : Setting<object>
{
    public Setting(string key, object value) : base(key, value) { }
}

public record Setting<T> where T : notnull
{
    public string Key { get; }
    public T Value { get; }

    public Setting(string key, T value) => (Key, Value) = (key, value);
}