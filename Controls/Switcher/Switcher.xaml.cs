using Controls.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Controls.Switcher {
  /// <summary>
  /// Interaction logic for Switcher.xaml
  /// </summary>
  public partial class Switcher : UserControl {

    #region Dependencies

    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
      "IsChecked", typeof(bool), typeof(Switcher), new PropertyMetadata(false, IsCheckedChangedCallback));

    private static void IsCheckedChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      if (!(dependencyObject is Switcher)) {
        return;
      }
      Switcher switcher = dependencyObject as Switcher;
      switcher.ChangeChecked();
    }

    public bool IsChecked {
      get { return (bool)GetValue(IsCheckedProperty); }
      set { SetValue(IsCheckedProperty, value); }
    }

    //---------------------------------------------------------------------------------------

    public static readonly DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register(
      "CheckedBackground", typeof(Brush), typeof(Switcher), new PropertyMetadata(Brushes.ForestGreen, CheckedBackgroundPropertyChanged));

    private static void CheckedBackgroundPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      if (!(dependencyObject is Switcher)) {
        return;
      }
      Switcher switcher = dependencyObject as Switcher;
      switcher.RefreshRectangleFill();
    }

    public Brush CheckedBackground {
      get { return (Brush)GetValue(CheckedBackgroundProperty); }
      set { SetValue(CheckedBackgroundProperty, value); }
    }

    //---------------------------------------------------------------------------------------

    public static readonly DependencyProperty UncheckedBackgroundProperty = DependencyProperty.Register(
      "UncheckedBackground", typeof(Brush), typeof(Switcher), new PropertyMetadata(Brushes.DarkGray, UncheckedBackgroundChangedCallback));

    private static void UncheckedBackgroundChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      if (!(dependencyObject is Switcher)) {
        return;
      }
      Switcher switcher = dependencyObject as Switcher;
      switcher.RefreshRectangleFill();
    }

    public Brush UncheckedBackground {
      get { return (Brush)GetValue(UncheckedBackgroundProperty); }
      set { SetValue(UncheckedBackgroundProperty, value); }
    }

    //---------------------------------------------------------------------------------------

    public static readonly DependencyProperty ThumbBrushProperty = DependencyProperty.Register(
      "ThumbBrush", typeof(Brush), typeof(Switcher), new PropertyMetadata(Brushes.White, ThumbBrushChangedCallback));

    private static void ThumbBrushChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      if (!(dependencyObject is Switcher)) {
        return;
      }
      Switcher switcher = dependencyObject as Switcher;
      switcher.Ellipse.Fill = dependencyPropertyChangedEventArgs.NewValue as Brush;
    }

    public Brush ThumbBrush {
      get { return (Brush)GetValue(ThumbBrushProperty); }
      set { SetValue(ThumbBrushProperty, value); }
    }

    //---------------------------------------------------------------------------------------

    #endregion

    private Ellipse Ellipse { get; set; }
    private Rectangle Rectangle { get; set; }

    private bool IsClicked { get; set; }
    private double ClickXPosition { get; set; }
    private double ClickEllipsePostion { get; set; }

    private const double MarginFactor = 0.95;
    private double EllipseMargin { get; set; }

    public Switcher() {
      InitializeComponent();

      Ellipse = new Ellipse();
      Rectangle = new Rectangle {
        Fill = GetBackgroundColor(IsChecked)
      };
      Canvas.Children.Add(Rectangle);
      Canvas.Children.Add(Ellipse);
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
      base.OnRenderSizeChanged(sizeInfo);
      RefreshView();
    }

    private void RefreshView() {
      Canvas.Width = ActualWidth;
      Canvas.Height = ActualHeight;

      EllipseMargin = ActualHeight * (1 - MarginFactor);

      Ellipse.Width = ActualHeight - EllipseMargin * 2;
      Ellipse.Height = ActualHeight - EllipseMargin * 2;
      Ellipse.Fill = ThumbBrush;
      Canvas.SetTop(Ellipse, EllipseMargin);
      Canvas.SetLeft(Ellipse, GetEllipseMargin(IsChecked));

      Rectangle.Width = ActualWidth;
      Rectangle.Height = ActualHeight;
      Rectangle.RadiusX = ActualHeight / 2;
      Rectangle.RadiusY = ActualHeight / 2;
    }

    private void ChangeChecked() {
      var sb = new Storyboard();

      DoubleAnimation doubleAnimation = new DoubleAnimation(Canvas.GetLeft(Ellipse), GetEllipseMargin(IsChecked), new Duration(TimeSpan.FromMilliseconds(200)), FillBehavior.Stop);
      Storyboard.SetTarget(doubleAnimation, Ellipse);
      Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
      sb.Children.Add(doubleAnimation);
      sb.Completed += (sender, args) => Canvas.SetLeft(Ellipse, GetEllipseMargin(IsChecked));
      sb.Begin();

      var animation = new BrushAnimation {
        From = GetBackgroundColor(!IsChecked),
        To = GetBackgroundColor(IsChecked),
        Duration = new Duration(TimeSpan.FromMilliseconds(200)),
        FillBehavior = FillBehavior.Stop,
      };
      Storyboard.SetTarget(animation, Rectangle);
      Storyboard.SetTargetProperty(animation, new PropertyPath("Fill"));
      sb = new Storyboard();
      sb.Children.Add(animation);
      sb.Completed += (sender, args) => { Rectangle.Fill = GetBackgroundColor(IsChecked); };
      sb.Begin();
    }

    private void Switcher_OnMouseDown(object sender, MouseButtonEventArgs e) {
      ClickXPosition = e.GetPosition(this).X;
      ClickEllipsePostion = Canvas.GetLeft(Ellipse);
      IsClicked = true;
      Mouse.Capture(this);
    }

    private void Switcher_OnMouseUp(object sender, MouseButtonEventArgs e) {
      IsClicked = false;
      ReleaseMouseCapture();
      IsChecked = !IsChecked;
    }

    private void Switcher_OnMouseMove(object sender, MouseEventArgs e) {
      if (!IsClicked) {
        return;
      }
      double xPoint = e.GetPosition(this).X;
      double newLeft = ClickEllipsePostion + (xPoint - ClickXPosition);
      if (newLeft < EllipseMargin) {
        newLeft = EllipseMargin;
      } else if (newLeft > ActualWidth - Ellipse.ActualWidth - EllipseMargin) {
        newLeft = ActualWidth - Ellipse.ActualWidth - EllipseMargin;
      }
      Canvas.SetLeft(Ellipse, newLeft);
    }

    private void Switcher_OnLoaded(object sender, RoutedEventArgs e) {
    }

    private Brush GetBackgroundColor(bool pIsChecked) {
      return pIsChecked ? CheckedBackground : UncheckedBackground;
    }

    private double GetEllipseMargin(bool pIsChecked) {
      return pIsChecked ? ActualWidth - Ellipse.Width - EllipseMargin : EllipseMargin;
    }

    private void RefreshRectangleFill() {
      Rectangle.Fill = GetBackgroundColor(IsChecked);
    }

  }
}
