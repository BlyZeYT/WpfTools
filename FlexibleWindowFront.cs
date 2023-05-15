namespace WpfTools;

using System;
using System.Windows;

public partial class FlexibleWindow : Window
{
    protected virtual void OnAspectRatioChanged(object? sender, AspectRatioChangedEventArgs e) => CalcAspectRatio();

    public event EventHandler<AspectRatioChangedEventArgs>? AspectRatioChanged;

    public static readonly DependencyProperty AspectRatioProperty =
        DependencyProperty.Register(
            nameof(AspectRatio),
            typeof(Size),
            typeof(FlexibleWindow),
            new PropertyMetadata(Size.Empty, AspectRatioChangedCallback),
            AspectRatioValidation);

    public Size AspectRatio
    {
        get { return (Size)GetValue(AspectRatioProperty); }
        set { SetValue(AspectRatioProperty, value); }
    }

    private static void AspectRatioChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var window = (FlexibleWindow)d;

        window.AspectRatioChanged?.Invoke(window, new AspectRatioChangedEventArgs((Size)e.OldValue, (Size)e.NewValue));
    }

    private static bool AspectRatioValidation(object value)
    {
        var aspectRatio = (Size)value;

        return aspectRatio.IsEmpty || aspectRatio.Width >= 0 || aspectRatio.Height >= 0;
    }


    protected virtual void OnRelativeWindowSizeChanged(object? sender, RelativeWindowSizeChangedEventArgs e) => CalcRelativeWindowSize();

    public event EventHandler<RelativeWindowSizeChangedEventArgs>? RelativeWindowSizeChanged;

    public static readonly DependencyProperty RelativeWindowSizeProperty =
        DependencyProperty.Register(
            nameof(RelativeWindowSize),
            typeof(double),
            typeof(FlexibleWindow),
            new PropertyMetadata(double.NaN, RelativeWindowSizeCallback),
            RelativeWindowSizeValidation);

    public double RelativeWindowSize
    {
        get { return (double)GetValue(RelativeWindowSizeProperty); }
        set { SetValue(RelativeWindowSizeProperty, value); }
    }

    private static void RelativeWindowSizeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var window = (FlexibleWindow)d;

        window.RelativeWindowSizeChanged?.Invoke(window, new RelativeWindowSizeChangedEventArgs((double)e.OldValue, (double)e.NewValue));
    }

    private static bool RelativeWindowSizeValidation(object value)
    {
        var relativeWindowSize = (double)value;

        return double.IsNaN(relativeWindowSize) || relativeWindowSize is > 0 and < 1;
    }


    protected virtual void OnRelativeMinWindowSizeChanged(object? sender, RelativeWindowSizeChangedEventArgs e) => CalcRelativeMinWindowSize();

    public event EventHandler<RelativeWindowSizeChangedEventArgs>? RelativeMinWindowSizeChanged;

    public static readonly DependencyProperty RelativeMinWindowSizeProperty =
        DependencyProperty.Register(
            nameof(RelativeMinWindowSize),
            typeof(double),
            typeof(FlexibleWindow),
            new PropertyMetadata(double.NaN, RelativeMinWindowSizeCallback),
            RelativeMinWindowSizeValidation);

    public double RelativeMinWindowSize
    {
        get { return (double)GetValue(RelativeMinWindowSizeProperty); }
        set { SetValue(RelativeMinWindowSizeProperty, value); }
    }

    private static void RelativeMinWindowSizeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var window = (FlexibleWindow)d;

        window.RelativeMinWindowSizeChanged?.Invoke(window, new RelativeWindowSizeChangedEventArgs((double)e.OldValue, (double)e.NewValue));
    }

    private static bool RelativeMinWindowSizeValidation(object value)
    {
        var relativeMinWindowSize = (double)value;

        return double.IsNaN(relativeMinWindowSize) || relativeMinWindowSize is > 0 and < 1;
    }


    protected virtual void OnRelativeMaxWindowSizeChanged(object? sender, RelativeWindowSizeChangedEventArgs e) => CalcRelativeMaxWindowSize();

    public event EventHandler<RelativeWindowSizeChangedEventArgs>? RelativeMaxWindowSizeChanged;

    public static readonly DependencyProperty RelativeMaxWindowSizeProperty =
        DependencyProperty.Register(
            nameof(RelativeMaxWindowSize),
            typeof(double),
            typeof(FlexibleWindow),
            new PropertyMetadata(double.NaN, RelativeMaxWindowSizeCallback),
            RelativeMaxWindowSizeValidation);

    public double RelativeMaxWindowSize
    {
        get { return (double)GetValue(RelativeMaxWindowSizeProperty); }
        set { SetValue(RelativeMaxWindowSizeProperty, value); }
    }

    private static void RelativeMaxWindowSizeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var window = (FlexibleWindow)d;

        window.RelativeMaxWindowSizeChanged?.Invoke(window, new RelativeWindowSizeChangedEventArgs((double)e.OldValue, (double)e.NewValue));
    }

    private static bool RelativeMaxWindowSizeValidation(object value)
    {
        var relativeMaxWindowSize = (double)value;

        return double.IsNaN(relativeMaxWindowSize) || relativeMaxWindowSize is > 0 and < 1;
    }


    protected virtual void OnEnabledSystemButtonsChanged(object? sender, EnabledSystemButtonsChangedEventArgs e) => UpdateSystemButtons();

    public event EventHandler<EnabledSystemButtonsChangedEventArgs>? EnabledSystemButtonsChanged;

    public static readonly DependencyProperty EnabledSystemButtonsProperty =
        DependencyProperty.Register(
            nameof(EnabledSystemButtons),
            typeof(SystemButton),
            typeof(FlexibleWindow),
            new PropertyMetadata(SystemButton.All, EnabledSystemButtonsCallback));

    public SystemButton EnabledSystemButtons
    {
        get { return (SystemButton)GetValue(EnabledSystemButtonsProperty); }
        set { SetValue(EnabledSystemButtonsProperty, value); }
    }

    private static void EnabledSystemButtonsCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var window = (FlexibleWindow)d;

        window.EnabledSystemButtonsChanged?.Invoke(window, new EnabledSystemButtonsChangedEventArgs((SystemButton)e.OldValue, (SystemButton)e.NewValue));
    }
}