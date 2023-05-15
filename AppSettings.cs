namespace WpfTools;

using System;
using System.Collections.Generic;
using System.IO;
using WpfTools.Internals;

public sealed class AppSettings : IAppSettings
{
    private readonly string _path = Path.Combine(Environment.CurrentDirectory, "appsettings.config.json");
    private readonly Dictionary<string, object> _settings = new();

    private bool _shouldRead = true;

    public IReadOnlyDictionary<string, object> Settings => _settings.AsReadOnly();

    public void AddSetting(Setting setting)
    {
        Read();

        ThrowIfKeyExists(setting.Key);

        _settings.Add(setting.Key, setting.Value);

        Update();
    }

    public bool TryAddSetting(Setting setting)
    {
        Read();

        var added = _settings.TryAdd(setting.Key, setting.Value);

        Update();

        return added;
    }

    public void RemoveSetting(string key)
    {
        Read();

        ThrowIfKeyNotExists(key);

        _settings.Remove(key);

        Update();
    }

    public bool TryRemoveSetting(string key)
    {
        Read();

        var removed = _settings.Remove(key);

        Update();

        return removed;
    }

    public bool ContainsSetting(string key)
    {
        Read();

        return _settings.ContainsKey(key);
    }

    public bool ContainsSetting(object value)
    {
        Read();

        return _settings.ContainsValue(value);
    }

    public bool ContainsSetting(Setting setting)
    {
        Read();

        return ContainsSetting(setting.Key);
    }

    public TValue GetSetting<TValue>(string key) where TValue : class
    {
        Read();

        return (TValue)_settings[key];
    }

    public TValue? GetSettingOrDefault<TValue>(string key, TValue defaultValue) where TValue : class
    {
        Read();

        return (TValue)_settings.GetValueOrDefault(key, defaultValue);
    }

    public bool TryGetSetting<TValue>(string key, out TValue value) where TValue : class
    {
        Read();

        var hasValue = _settings.TryGetValue(key, out var obj);

        value = (TValue)obj!;

        return hasValue;
    }

    public object this[string key] => GetSetting<object>(key);

    public void ResetSettings()
    {
        _settings.Clear();

        Update();
    }

    private void ThrowIfKeyExists(string key)
    {
        if (_settings.ContainsKey(key))
            throw new AppSettingsException($"Key: {key} already exists");
    }

    private void ThrowIfKeyNotExists(string key)
    {
        if (!_settings.ContainsKey(key))
            throw new AppSettingsException($"Key: {key} does not exist");
    }

    private void Update() => Json.Write(_path, _settings);

    private Dictionary<string, object> Read()
    {
        if (!_shouldRead) return _settings;

        _shouldRead = false;

        return Json.Read<Dictionary<string, object>>(_path) ?? new();
    }
}

public interface IAppSettings
{
    public IReadOnlyDictionary<string, object> Settings { get; }

    public void AddSetting(Setting setting);

    public bool TryAddSetting(Setting setting);

    public void RemoveSetting(string key);

    public bool TryRemoveSetting(string key);

    public bool ContainsSetting(string key);

    public bool ContainsSetting(object value);

    public bool ContainsSetting(Setting setting);

    public TValue GetSetting<TValue>(string key) where TValue : class;

    public TValue? GetSettingOrDefault<TValue>(string key, TValue defaultValue) where TValue : class;

    public bool TryGetSetting<TValue>(string key, out TValue value) where TValue : class;

    public object this[string key] { get; }

    public void ResetSettings();
}