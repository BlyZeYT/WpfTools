namespace WpfTools.Internals;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct WINDOWPOS
{
    public nint hwnd;
    public nint hwndInsertAfter;
    public int x;
    public int y;
    public int cx;
    public int cy;
    public int flags;
}
