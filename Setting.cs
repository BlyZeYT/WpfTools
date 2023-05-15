namespace WpfTools;

public sealed class Setting
{
    public string Key { get; }
    public object Value { get; }

    public Setting(string key, object value) => (Key, Value) = (key, value);
}