namespace WpfTools;

using System;

public sealed class EnabledSystemButtonsChangedEventArgs : EventArgs
{
    public SystemButton OldValue { get; }
    public SystemButton NewValue { get; }

    internal EnabledSystemButtonsChangedEventArgs(SystemButton oldValue, SystemButton newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}