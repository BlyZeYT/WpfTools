namespace WpfTools;

using System;

public sealed class RelativeWindowSizeChangedEventArgs : EventArgs
{
    public double OldValue { get; }
    public double NewValue { get; }

    internal RelativeWindowSizeChangedEventArgs(double oldValue, double newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}