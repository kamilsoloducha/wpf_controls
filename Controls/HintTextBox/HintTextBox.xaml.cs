using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Controls.HintTextBox {
  /// <summary>
  /// Interaction logic for UserControl1.xaml
  /// </summary>
  public partial class HintTextBox : UserControl {

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
      "Text", typeof(string), typeof(HintTextBox), new PropertyMetadata(default(string)));

    public string Text {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty HiddenTextProperty = DependencyProperty.Register(
      "HiddenText", typeof(string), typeof(HintTextBox), new PropertyMetadata(default(string)));

    public string HiddenText {
      get { return (string)GetValue(HiddenTextProperty); }
      set { SetValue(HiddenTextProperty, value); }
    }

    public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
      "Hint", typeof(string), typeof(HintTextBox), new PropertyMetadata(default(string)));

    public string Hint {
      get { return (string)GetValue(HintProperty); }
      set { SetValue(HintProperty, value); }
    }

    public static readonly DependencyProperty HintForegroundProperty = DependencyProperty.Register(
      "HintForeground", typeof(Brush), typeof(HintTextBox), new PropertyMetadata(Brushes.LightGray));

    public Brush HintForeground {
      get { return (Brush)GetValue(HintForegroundProperty); }
      set { SetValue(HintForegroundProperty, value); }
    }

    public static readonly DependencyProperty IsPasswordProperty = DependencyProperty.Register(
      "IsPassword", typeof(bool), typeof(HintTextBox), new PropertyMetadata(false));

    public bool IsPassword {
      get { return (bool)GetValue(IsPasswordProperty); }
      set { SetValue(IsPasswordProperty, value); }
    }

    public HintTextBox() {
      InitializeComponent();
      TextBox.LostFocus += TextBox_LostFocus;
      TextBox.GotFocus += TextBox_GotFocus;
      TextBox.Loaded += TextBoxOnLoaded;
      TextBox.TextChanged += TextBoxOnTextChanged;
    }

    private void TextBoxOnLoaded(object sender, RoutedEventArgs routedEventArgs) {
      if (TextBox.Text != null && !TextBox.Text.Equals("")) {
        HideHint();
      }
    }

    protected override void OnInitialized(EventArgs e) {
      base.OnInitialized(e);
      if (TextBox.Text != null && !TextBox.Text.Equals("")) {
        HideHint();
      }
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
      if (TextBox.Text == null || TextBox.Text.Equals("")) {
        ShowHint();
      }
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
      HideHint();
    }

    private void TextBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs) {
      TextBox textBox = sender as TextBox;
      if (textBox == null) {
        return;
      }
      if (!textBox.IsFocused && textBox.Text.Equals("")) {
        ShowHint();
        return;
      }
      HideHint();
    }

    #region Animations

    private void ShowHint() {
      TextBlock.BeginAnimation(TextBlock.FontSizeProperty, new DoubleAnimation(FontSize, new Duration(TimeSpan.FromMilliseconds(200))));
      TextBlock.BeginAnimation(MarginProperty, new ThicknessAnimation(new Thickness(TextBlock.Margin.Left, 0, 0, 0), new Duration(TimeSpan.FromMilliseconds(200))));
    }

    private void HideHint() {
      TextBlock.BeginAnimation(TextBlock.FontSizeProperty, new DoubleAnimation(CalculateSmallFont(), new Duration(TimeSpan.FromMilliseconds(200))));
      TextBlock.BeginAnimation(MarginProperty, new ThicknessAnimation(new Thickness(TextBlock.Margin.Left, -ActualHeight, 0, 0), new Duration(TimeSpan.FromMilliseconds(200))));
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
