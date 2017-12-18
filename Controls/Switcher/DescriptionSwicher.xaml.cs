using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controls.Switcher {
  /// <summary>
  /// Interaction logic for DescriptionSwicher.xaml
  /// </summary>
  public partial class DescriptionSwicher : UserControl {

    #region Dependencies

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
      "Text", typeof(string), typeof(DescriptionSwicher), new PropertyMetadata(default(string)));

    public string Text {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }


    //----------------------------------------------------------------------------------------------------

    public static readonly DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register(
      "CheckedBackground", typeof(Brush), typeof(DescriptionSwicher), new PropertyMetadata(Brushes.ForestGreen));

    public Brush CheckedBackground {
      get { return Switcher.CheckedBackground; }
      set { Switcher.CheckedBackground = value; }
    }

    //----------------------------------------------------------------------------------------------------

    public static readonly DependencyProperty UncheckedBackgroundProperty = DependencyProperty.Register(
      "UncheckedBackground", typeof(Brush), typeof(DescriptionSwicher), new PropertyMetadata(Brushes.DarkGray));

    public Brush UncheckedBackground {
      get { return Switcher.UncheckedBackground; }
      set { Switcher.UncheckedBackground = value; }
    }

    //----------------------------------------------------------------------------------------------------

    public static readonly DependencyProperty ThumbBrushProperty = DependencyProperty.Register(
      "ThumbBrush", typeof(Brush), typeof(DescriptionSwicher), new PropertyMetadata(Brushes.White));

    public Brush ThumbBrush {
      get { return Switcher.ThumbBrush; }
      set { Switcher.ThumbBrush = value; }
    }

    //----------------------------------------------------------------------------------------------------

    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
      "IsChecked", typeof(bool), typeof(DescriptionSwicher), new PropertyMetadata(false));

    public bool IsChecked {
      get { return Switcher.IsChecked; }
      set { Switcher.IsChecked = value; }
    }

    //----------------------------------------------------------------------------------------------------

    #endregion

    public DescriptionSwicher() {
      InitializeComponent();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
      base.OnRenderSizeChanged(sizeInfo);
      RefreshView();
    }

    private void RefreshView() {
      double height = TextBlock.FontSize;
      Switcher.Height = 2 * height;
      Switcher.Width = 2 * Switcher.Height;
    }
  }
}
