namespace WpfTools.Internals;

using System.Runtime.InteropServices;
using System.Windows.Interop;

internal sealed class WindowMaster
{
    public const int WM_SIZING = 0x0214;
    public const int WM_WINDOWPOSCHANGING = 0x0046;

    public const int WMSZ_LEFT = 1;
    public const int WMSZ_RIGHT = 2;
    public const int WMSZ_TOP = 3;
    public const int WMSZ_TOPLEFT = 4;
    public const int WMSZ_TOPRIGHT = 5;
    public const int WMSZ_BOTTOM = 6;
    public const int WMSZ_BOTTOMLEFT = 7;
    public const int WMSZ_BOTTOMRIGHT = 8;

    private const nint MONITOR_DEFAULTONNEAREST = 0x00000002;

    private const int GWL_STYLE = -16;
    private const int WS_MAXIMIZEBOX = 0x00010000;
    private const int WS_MINIMIZEBOX = 0x00020000;
    private const int WS_SYSMENU = 0x00080000;

    private const uint MF_BYCOMMAND = 0x00000000;
    private const uint SC_CLOSE = 0x0000F060;

    private FlexibleWindow _window;
    private WindowInteropHelper _helper;

    public WindowMaster(FlexibleWindow window)
    {
        _window = window;
        _helper = new WindowInteropHelper(_window);
    }

    [DllImport("user32.dll")]
    private static extern nint MonitorFromWindow(nint handle, nint flags);

    [DllImport("user32.dll")]
    private static extern bool GetMonitorInfo(nint hMonitor, NativeMonitorInfo lpmi);

    [DllImport("user32.dll")]
    private static extern nint GetSystemMenu(nint hWnd, bool bRevert);

    [DllImport("user32.dll")]
    private static extern int GetMenuItemCount(nint hMenu);

    [DllImport("user32.dll")]
    private static extern uint RemoveMenu(nint hMenu, uint nPosition, uint wFlags);

    [DllImport("user32.dll")]
    private static extern bool DrawMenuBar(nint hWnd);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private extern static int GetWindowLongPtr(nint hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    private extern static int SetWindowLongPtr(nint hWnd, int nIndex, int dwNewLong);

    public nint GetHandle() => _helper.EnsureHandle();

    public bool GetMonitorInfo(out NativeMonitorInfo monitorInfo)
    {
        monitorInfo = new NativeMonitorInfo();

        var monitor = MonitorFromWindow(GetHandle(), MONITOR_DEFAULTONNEAREST);

        if (monitor == nint.Zero) return false;

        GetMonitorInfo(monitor, monitorInfo);

        return true;
    }

    public void ManageSystemButtons(SystemButton systemButtons)
    {
        if (systemButtons is SystemButton.None)
        {
            HideSysButtons();

            return;
        }

        if (systemButtons is SystemButton.All)
        {
            EnableMinimizeButton();
            EnableMaximizeButton();
            EnableCloseButton();

            ShowSysButtons();

            return;
        }

        if (systemButtons.HasFlag(SystemButton.Minimize))
        {
            EnableMinimizeButton();

            if (systemButtons.HasFlag(SystemButton.Maximize)) EnableMaximizeButton();
            else DisableMaximizeButton();

            if (systemButtons.HasFlag(SystemButton.Close)) EnableCloseButton();
            else DisableCloseButton();
        }

        if (systemButtons.HasFlag(SystemButton.Maximize))
        {
            EnableMaximizeButton();

            if (systemButtons.HasFlag(SystemButton.Minimize)) EnableMinimizeButton();
            else DisableMinimizeButton();

            if (systemButtons.HasFlag(SystemButton.Close)) EnableCloseButton();
            else DisableCloseButton();
        }

        if (systemButtons.HasFlag(SystemButton.Close))
        {
            EnableCloseButton();

            if (systemButtons.HasFlag(SystemButton.Minimize)) EnableMinimizeButton();
            else DisableMinimizeButton();

            if (systemButtons.HasFlag(SystemButton.Maximize)) EnableMaximizeButton();
            else DisableMaximizeButton();
        }
    }

    private void DisableCloseButton()
    {
        var hMenu = GetSystemMenu(GetHandle(), false);
        _ = RemoveMenu(hMenu, SC_CLOSE, MF_BYCOMMAND);
        DrawMenuBar(GetHandle());
    }

    private void EnableCloseButton()
    {
        _ = GetSystemMenu(GetHandle(), true);
        DrawMenuBar(GetHandle());
    }

    private void DisableMaximizeButton()
    {
        var windowStyle = GetWindowLongPtr(GetHandle(), GWL_STYLE);
        _ = SetWindowLongPtr(GetHandle(), GWL_STYLE, windowStyle & ~WS_MAXIMIZEBOX);
        DrawMenuBar(GetHandle());
    }

    private void EnableMaximizeButton()
    {
        var windowStyle = GetWindowLongPtr(GetHandle(), GWL_STYLE);
        _ = SetWindowLongPtr(GetHandle(), GWL_STYLE, windowStyle | WS_MAXIMIZEBOX);
        DrawMenuBar(GetHandle());
    }

    private void DisableMinimizeButton()
    {
        var windowStyle = GetWindowLongPtr(GetHandle(), GWL_STYLE);
        _ = SetWindowLongPtr(GetHandle(), GWL_STYLE, windowStyle & ~WS_MINIMIZEBOX);
        DrawMenuBar(GetHandle());
    }

    private void EnableMinimizeButton()
    {
        var windowStyle = GetWindowLongPtr(GetHandle(), GWL_STYLE);
        _ = SetWindowLongPtr(GetHandle(), GWL_STYLE, windowStyle | WS_MINIMIZEBOX);
        DrawMenuBar(GetHandle());
    }

    private void HideSysButtons()
    {
        var windowStyle = GetWindowLongPtr(GetHandle(), GWL_STYLE);
        _ = SetWindowLongPtr(GetHandle(), GWL_STYLE, windowStyle & ~WS_SYSMENU);
        DrawMenuBar(GetHandle());
    }

    private void ShowSysButtons()
    {
        var windowStyle = GetWindowLongPtr(GetHandle(), GWL_STYLE);
        _ = SetWindowLongPtr(GetHandle(), GWL_STYLE, windowStyle | WS_SYSMENU);
        DrawMenuBar(GetHandle());
    }
}