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
        ReadConfig();

        ThrowIfKeyExists(setting.Key);

        _settings.Add(setting.Key, setting.Value);

        OverrideConfig();
    }

    public bool TryAddSetting(Setting setting)
    {
        ReadConfig();

        var added = _settings.TryAdd(setting.Key, setting.Value);

        OverrideConfig();

        return added;
    }

    public void RemoveSetting(string key)
    {
        ReadConfig();

        ThrowIfKeyNotExists(key);

        _settings.Remove(key);

        OverrideConfig();
    }

    public bool TryRemoveSetting(string key)
    {
        ReadConfig();

        var removed = _settings.Remove(key);

        OverrideConfig();

        return removed;
    }

    public bool ContainsSetting(string key)
    {
        ReadConfig();

        return _settings.ContainsKey(key);
    }

    public bool ContainsSetting(object value)
    {
        ReadConfig();

        return _settings.ContainsValue(value);
    }

    public bool ContainsSetting(Setting setting)
    {
        ReadConfig();

        return ContainsSetting(setting.Key);
    }

    public TValue GetSetting<TValue>(string key) where TValue : class
    {
        ReadConfig();

        return (TValue)_settings[key];
    }

    public TValue? GetSettingOrDefault<TValue>(string key, TValue defaultValue) where TValue : class
    {
        ReadConfig();

        return (TValue)_settings.GetValueOrDefault(key, defaultValue);
    }

    public bool TryGetSetting<TValue>(string key, out TValue value) where TValue : class
    {
        ReadConfig();

        var hasValue = _settings.TryGetValue(key, out var obj);

        value = (TValue)obj!;

        return hasValue;
    }

    public object this[string key] => GetSetting<object>(key);

    public void ResetSettings()
    {
        _settings.Clear();

        OverrideConfig();
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

    private void OverrideConfig()
    {
        Json.Write(_path, _settings);
        _shouldRead = true;
    }

    private Dictionary<string, object> ReadConfig()
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