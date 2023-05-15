namespace WpfTools.Internals;

using System.Runtime.InteropServices;
using System;

[Serializable, StructLayout(LayoutKind.Sequential)]
internal struct NativeRectangle
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    internal NativeRectangle(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
}
