using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Controls.AnimatedProgressBar {
  /// <summary>
  /// Interaction logic for AnimatedProgressBar.xaml
  /// </summary>
  public partial class AnimatedProgressBar : UserControl {

    #region

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
      "Text", typeof(string), typeof(AnimatedProgressBar), new PropertyMetadata(default(string)));

    public string Text {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }

    //------------------------------------------------------

    public static readonly DependencyProperty BarForegroundProperty = DependencyProperty.Register(
      "BarForeground", typeof(Brush), typeof(AnimatedProgressBar), new PropertyMetadata(Brushes.DimGray));

    public Brush BarForeground {
      get { return (Brush)GetValue(BarForegroundProperty); }
      set { SetValue(BarForegroundProperty, value); }
    }

    //------------------------------------------------------

    public static readonly DependencyProperty BarBackgroundProperty = DependencyProperty.Register(
      "BarBackground", typeof(Brush), typeof(AnimatedProgressBar), new PropertyMetadata(Brushes.DarkGray));

    public Brush BarBackground {
      get { return (Brush)GetValue(BarBackgroundProperty); }
      set { SetValue(BarBackgroundProperty, value); }
    }

    //------------------------------------------------------

    public static readonly DependencyProperty TextStyleProperty = DependencyProperty.Register(
      "TextStyle", typeof (Style), typeof (AnimatedProgressBar), new PropertyMetadata(default(Style), TextStylePropertyChanged));

    private static void TextStylePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      AnimatedProgressBar animatedProgressBar = dependencyObject as AnimatedProgressBar;
      if (animatedProgressBar == null) {
        return;
      }
      animatedProgressBar.TextBlock.Style = dependencyPropertyChangedEventArgs.NewValue as Style;
      animatedProgressBar.TextBlock.InvalidateVisual();
    }

    public Style TextStyle {
      get { return (Style) GetValue(TextStyleProperty); }
      set { SetValue(TextStyleProperty, value); }
    }

    //------------------------------------------------------

    #endregion

    private Storyboard sb { get; set; }

    public AnimatedProgressBar() {
      InitializeComponent();
      sb = new Storyboard();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
      base.OnRenderSizeChanged(sizeInfo);
      StartAnimation();
    }

    private void StartAnimation() {
      sb.Pause();
      sb.Children.Clear();
      int time = 1500;
      DoubleAnimation transalteAnimation = new DoubleAnimation(0, Canvas.ActualWidth, new Duration(TimeSpan.FromMilliseconds(time))) {
        EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseIn },
        RepeatBehavior = RepeatBehavior.Forever,
      };
      Storyboard.SetTarget(transalteAnimation, BarRectangle);
      Storyboard.SetTargetProperty(transalteAnimation, new PropertyPath("(Canvas.Left)"));
      sb.Children.Add(transalteAnimation);

      DoubleAnimation widthAnimation = new DoubleAnimation(0, Canvas.ActualWidth / 2, new Duration(TimeSpan.FromMilliseconds(time / 2))) {
        AutoReverse = true,
        EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut },
        RepeatBehavior = RepeatBehavior.Forever,
      };
      Storyboard.SetTarget(widthAnimation, BarRectangle);
      Storyboard.SetTargetProperty(widthAnimation, new PropertyPath("Width"));
      sb.Children.Add(widthAnimation);
      sb.Begin();
    }
  }
}
