namespace WpfTools;

using System;

[Flags]
public enum SystemButton
{
    None = 0,
    Minimize = 2,
    Maximize = 4,
    Close = 8,
    All = Minimize | Maximize | Close
}