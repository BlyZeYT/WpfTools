namespace WpfTools;

using System;

public sealed class AppSettingsException : Exception
{
    internal AppSettingsException(string message) : base(message) { }
}