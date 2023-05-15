namespace WpfTools.Internals;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal sealed class NativeMonitorInfo
{
    public int Size = Marshal.SizeOf(typeof(NativeMonitorInfo));
    public NativeRectangle Monitor;
    public NativeRectangle Work;
    public int Flags;
}