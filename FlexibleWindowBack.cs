namespace WpfTools;

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using WpfTools.Internals;

public partial class FlexibleWindow : Window
{
    private int sizingEdge;

    private WindowMaster _window;

    public FlexibleWindow()
    {
        _window = new WindowMaster(this);
        sizingEdge = 0;
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        _window.ManageSystemButtons(EnabledSystemButtons);

        if (_window.GetMonitorInfo(out var monitorInfo))
        {
            var width = monitorInfo.Monitor.Right - monitorInfo.Monitor.Left;
            var height = monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top;

            if (!double.IsNaN(RelativeWindowSize))
            {
                Width = width * RelativeWindowSize;
                Height = height * RelativeWindowSize;
            }

            if (!double.IsNaN(RelativeMinWindowSize))
            {
                MinWidth = width * RelativeMinWindowSize;
                MinHeight = height * RelativeMinWindowSize;
            }

            if (!double.IsNaN(RelativeMaxWindowSize))
            {
                MaxWidth = width * RelativeMaxWindowSize;
                MaxHeight = height * RelativeMaxWindowSize;
            }
        }

        if (!AspectRatio.IsEmpty) CalcAspectRatio();

        AspectRatioChanged += OnAspectRatioChanged;
        RelativeWindowSizeChanged += OnRelativeWindowSizeChanged;
        RelativeMinWindowSizeChanged += OnRelativeMinWindowSizeChanged;
        RelativeMaxWindowSizeChanged += OnRelativeMaxWindowSizeChanged;
        EnabledSystemButtonsChanged += OnEnabledSystemButtonsChanged;
    }

    private nint DragHook(nint hwnd, int msg, nint wParam, nint lParam, ref bool handeled)
    {
        switch (msg)
        {
            case WindowMaster.WM_SIZING: sizingEdge = wParam.ToInt32(); break;

            case WindowMaster.WM_WINDOWPOSCHANGING:
                var position = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS))!;

                if (position.cx == Width && position.cy == Height) return nint.Zero;

                switch (sizingEdge)
                {
                    case WindowMaster.WMSZ_TOP or WindowMaster.WMSZ_BOTTOM or WindowMaster.WMSZ_TOPRIGHT:
                        position.cx = (int)(position.cy * AspectRatio.Width / AspectRatio.Height);
                        break;

                    case WindowMaster.WMSZ_LEFT or WindowMaster.WMSZ_RIGHT or WindowMaster.WMSZ_BOTTOMRIGHT or WindowMaster.WMSZ_BOTTOMLEFT:
                        position.cy = (int)(position.cx * AspectRatio.Height / AspectRatio.Width);
                        break;

                    case WindowMaster.WMSZ_TOPLEFT:
                        var width = (int)(position.cy * AspectRatio.Width / AspectRatio.Height);
                        position.x -= width - position.cx;
                        position.cx = width;

                        var height = (int)(position.cx * AspectRatio.Height / AspectRatio.Width);
                        position.y -= height - position.cy;
                        position.cy = height;
                        break;
                }

                Marshal.StructureToPtr(position, lParam, true);
                break;
        }

        return nint.Zero;
    }

    private void CalcAspectRatio()
    {
        var source = HwndSource.FromHwnd(_window.GetHandle());

        source?.RemoveHook(DragHook);

        if (AspectRatio.IsEmpty)
        {
            CalcRelativeWindowSize();
            CalcRelativeMinWindowSize();
            CalcRelativeMaxWindowSize();
        }
        else
        {
            source?.AddHook(DragHook);

            if (Width is double.NaN)
            {
                MinWidth = MinHeight * (AspectRatio.Width / AspectRatio.Height);
                Width = Height * (AspectRatio.Width / AspectRatio.Height);
                MaxWidth = MaxHeight * (AspectRatio.Width / AspectRatio.Height);
            }
            else
            {
                MinHeight = MinWidth / (AspectRatio.Width / AspectRatio.Height);
                Height = Width / (AspectRatio.Width / AspectRatio.Height);
                MaxHeight = MaxWidth / (AspectRatio.Width / AspectRatio.Height);
            }
        }
    }

    private void CalcRelativeWindowSize()
    {
        if (double.IsNaN(RelativeWindowSize)) return;

        if (!_window.GetMonitorInfo(out var monitorInfo)) return;

        Width = (monitorInfo.Monitor.Right - monitorInfo.Monitor.Left) * RelativeWindowSize;
        Height = (monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top) * RelativeWindowSize;
    }

    private void CalcRelativeMinWindowSize()
    {
        if (double.IsNaN(RelativeMinWindowSize)) return;

        if (!_window.GetMonitorInfo(out var monitorInfo)) return;

        MinWidth = (monitorInfo.Monitor.Right - monitorInfo.Monitor.Left) * RelativeMinWindowSize;
        MinHeight = (monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top) * RelativeMinWindowSize;
    }

    private void CalcRelativeMaxWindowSize()
    {
        if (double.IsNaN(RelativeMaxWindowSize)) return;

        if (!_window.GetMonitorInfo(out var monitorInfo)) return;

        MaxWidth = (monitorInfo.Monitor.Right - monitorInfo.Monitor.Left) * RelativeMaxWindowSize;
        MaxHeight = (monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top) * RelativeMaxWindowSize;
    }

    private void UpdateSystemButtons() => _window.ManageSystemButtons(EnabledSystemButtons);
}