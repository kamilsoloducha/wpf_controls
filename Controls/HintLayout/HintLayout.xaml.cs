using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Controls.HintLayout {
  /// <summary>
  /// Interaction logic for HintLayout.xaml
  /// </summary>
  public partial class HintLayout : UserControl {

    public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
      "Hint", typeof(string), typeof(HintLayout), new PropertyMetadata(default(string)));

    public string Hint {
      get { return (string)GetValue(HintProperty); }
      set { SetValue(HintProperty, value); }
    }

    public static readonly DependencyProperty HintForegroundProperty = DependencyProperty.Register(
      "HintForeground", typeof(Brush), typeof(HintLayout), new PropertyMetadata(Brushes.LightGray));

    public Brush HintForeground {
      get { return (Brush)GetValue(HintForegroundProperty); }
      set { SetValue(HintForegroundProperty, value); }
    }

    public static readonly DependencyProperty AdditionalContentProperty = DependencyProperty.Register(
      "AdditionalContent", typeof(FrameworkElement), typeof(HintLayout), new PropertyMetadata(default(FrameworkElement)));



    public object AdditionalContent {
      get { return GetValue(AdditionalContentProperty); }
      set { SetValue(AdditionalContentProperty, value); }
    }

    public HintLayout() {
      InitializeComponent();

    }
    private void ShowHint() {
      TextBlock.BeginAnimation(TextBlock.FontSizeProperty, new DoubleAnimation(FontSize, new Duration(TimeSpan.FromMilliseconds(200))));
      TextBlock.BeginAnimation(MarginProperty, new ThicknessAnimation(new Thickness(TextBlock.Margin.Left, 0, 0, 0), new Duration(TimeSpan.FromMilliseconds(200))));
    }

    private void HideHint() {
      TextBlock.BeginAnimation(TextBlock.FontSizeProperty, new DoubleAnimation(9, new Duration(TimeSpan.FromMilliseconds(200))));
      TextBlock.BeginAnimation(MarginProperty, new ThicknessAnimation(new Thickness(TextBlock.Margin.Left, -ActualHeight, 0, 0), new Duration(TimeSpan.FromMilliseconds(200))));
    }
  }
}
