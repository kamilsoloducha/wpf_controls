using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Controls.HintPasswordBox {
  /// <summary>
  /// Interaction logic for HintPasswordBox.xaml
  /// </summary>
  public partial class HintPasswordBox : UserControl {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
      "Text", typeof(string), typeof(HintPasswordBox), new PropertyMetadata(default(string)));

    public string Text {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
      "Hint", typeof(string), typeof(HintPasswordBox), new PropertyMetadata(default(string)));

    public string Hint {
      get { return (string)GetValue(HintProperty); }
      set { SetValue(HintProperty, value); }
    }

    public static readonly DependencyProperty HintForegroundProperty = DependencyProperty.Register(
      "HintForeground", typeof(Brush), typeof(HintPasswordBox), new PropertyMetadata(Brushes.LightGray));

    public Brush HintForeground {
      get { return (Brush)GetValue(HintForegroundProperty); }
      set { SetValue(HintForegroundProperty, value); }
    }

    public HintPasswordBox() {
      InitializeComponent();
      //Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(30));
      DependencyPropertyDescriptor.FromProperty(TextProperty, typeof(HintPasswordBox)).AddValueChanged(this, OnTextChanged);
      TextBox.LostFocus += TextBox_LostFocus;
      TextBox.GotFocus += TextBox_GotFocus;
      TextBox.SizeChanged += TextBoxOnSizeChanged;
    }

    private void OnTextChanged(object sender, EventArgs eventArgs) {
      if (Text != null && !Text.Equals("")) {
        HideHint();
      }
    }

    private void TextBoxOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs) {
      if (Text != null && !Text.Equals("")) {
        HideHint();
      }
    }

    protected override void OnInitialized(EventArgs e) {
      base.OnInitialized(e);
      if (Text != null && !Text.Equals("")) {
        HideHint();
      }
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
      if (Text == null || Text.Equals("")) {
        ShowHint();
      }
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
      HideHint();
    }

    #region Animations

    private void ShowHint() {
      TextBlock.BeginAnimation(TextBlock.FontSizeProperty, new DoubleAnimation(FontSize, new Duration(TimeSpan.FromMilliseconds(200))));
      TextBlock.BeginAnimation(MarginProperty, new ThicknessAnimation(new Thickness(TextBlock.Margin.Left, 0, 0, 0), new Duration(TimeSpan.FromMilliseconds(200))));
    }

    private void HideHint() {
        TextBlock.BeginAnimation(TextBlock.FontSizeProperty, new DoubleAnimation(CalculateSmallFont(), new Duration(TimeSpan.FromMilliseconds(200))));
        TextBlock.BeginAnimation(MarginProperty, new ThicknessAnimation(new Thickness(TextBlock.Margin.Left, -TextBox.ActualHeight, 0, 0), new Duration(TimeSpan.FromMilliseconds(200))));
    }

    private double CalculateSmallFont() {
      double lNewFont = FontSize / 2;
      if (lNewFont < 10)
        return 10;
      return lNewFont;
    }
    #endregion
  }
}
