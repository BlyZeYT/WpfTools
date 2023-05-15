namespace WpfTools;

using System;
using System.Windows;

public sealed class AspectRatioChangedEventArgs : EventArgs
{
    public Size OldAspectRatio { get; }
    public Size NewAspectRatio { get; }

    internal AspectRatioChangedEventArgs(Size oldAspectRatio, Size newAspectRatio)
    {
        OldAspectRatio = oldAspectRatio;
        NewAspectRatio = newAspectRatio;
    }
}