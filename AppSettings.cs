namespace WpfTools;

using System;
using System.Collections.Generic;
using System.IO;
using WpfTools.Internals;

public sealed class AppSettings : IAppSettings
{
    private readonly string _path = Path.Combine(Environment.CurrentDirectory, "appsettings.config.json");

    private Dictionary<string, object> _settings = new();
    private bool _shouldRead = true;

    public IReadOnlyDictionary<string, object> Settings => _settings.AsReadOnly();

    public void AddSetting(Setting setting)
    {
        ReadConfig();

        ThrowIfKeyExists(setting.Key);

        _settings.Add(setting.Key, setting.Value);

        OverrideConfig();
    }

    public void AddSetting<TValue>(Setting<TValue> setting) where TValue : notnull
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

    public bool TryAddSetting<TValue>(Setting<TValue> setting) where TValue : notnull
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

    public bool ContainsSetting(Setting setting) => ContainsSetting(setting.Key);

    public bool ContainsSetting<TValue>(Setting<TValue> setting) where TValue : notnull
        => ContainsSetting(setting.Key);

    public TValue GetSetting<TValue>(string key) where TValue : notnull
    {
        ReadConfig();

        return (TValue)_settings[key];
    }

    public TValue GetSettingOrDefault<TValue>(string key, TValue defaultValue) where TValue : notnull
    {
        ReadConfig();

        return (TValue)_settings.GetValueOrDefault(key, defaultValue);
    }

    public bool TryGetSetting<TValue>(string key, out TValue value) where TValue : notnull
    {
        try
        {
            value = GetSetting<TValue>(key);
            return true;
        }
        catch (Exception)
        {
            value = default!;
            return false;
        }
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

    private void ReadConfig()
    {
        if (!_shouldRead) return;

        _shouldRead = false;
        _settings = Json.Read<Dictionary<string, object>>(_path) ?? new();
    }
}

public interface IAppSettings
{
    public IReadOnlyDictionary<string, object> Settings { get; }

    public void AddSetting(Setting setting);

    public void AddSetting<TValue>(Setting<TValue> setting) where TValue : notnull;

    public bool TryAddSetting(Setting setting);

    public bool TryAddSetting<TValue>(Setting<TValue> setting) where TValue : notnull;

    public void RemoveSetting(string key);

    public bool TryRemoveSetting(string key);

    public bool ContainsSetting(string key);

    public bool ContainsSetting(object value);

    public bool ContainsSetting(Setting setting);

    public bool ContainsSetting<TValue>(Setting<TValue> setting) where TValue : notnull;

    public TValue GetSetting<TValue>(string key) where TValue : notnull;

    public TValue GetSettingOrDefault<TValue>(string key, TValue defaultValue) where TValue : notnull;

    public bool TryGetSetting<TValue>(string key, out TValue value) where TValue : notnull;

    public object this[string key] { get; }

    public void ResetSettings();
}