using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Controls.Helpers;

namespace Controls.ExtendedPanel {
  /// <summary>
  /// Interaction logic for ExtendedPanel.xaml
  /// </summary>
  public partial class ExtendedPanel : UserControl {

    public static readonly DependencyProperty AdditionalContentProperty = DependencyProperty.Register(
      "AdditionalContent", typeof(object), typeof(ExtendedPanel), new PropertyMetadata(default(object)));

    public object AdditionalContent {
      get { return GetValue(AdditionalContentProperty); }
      set { SetValue(AdditionalContentProperty, value); }
    }

    //--------------------------------------------------------

    public static readonly DependencyProperty IsExtendedProperty = DependencyProperty.Register(
      "IsExtended", typeof(bool), typeof(ExtendedPanel), new PropertyMetadata(true, IsExtendedChangedCallback));

    private static void IsExtendedChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      ExtendedPanel panel = dependencyObject as ExtendedPanel;
      if (panel == null) { return; }
      bool newValue = (bool)dependencyPropertyChangedEventArgs.NewValue;
      panel.AnimatePanel(newValue);
    }

    public bool IsExtended {
      get { return (bool)GetValue(IsExtendedProperty); }
      set { SetValue(IsExtendedProperty, value); }
    }

    //--------------------------------------------------------

    private double _contentHeight;

    public ExtendedPanel() {
      InitializeComponent();
    }

    private void OnExtendClick(object sender, RoutedEventArgs e) {
      IsExtended = !IsExtended;
    }
    
    private void AnimatePanel(bool pIsExtended) {
      GridLengthAnimation animation = new GridLengthAnimation {
        Duration = new Duration(TimeSpan.FromMilliseconds(500)),
        From = new GridLength(Grid.RowDefinitions[1].ActualHeight),
        FillBehavior = FillBehavior.Stop,
        AccelerationRatio = 0.5,
        DecelerationRatio = 0.5
      };
      if (!pIsExtended) {
        _contentHeight = ContentPresenter.ActualHeight;
        animation.To = new GridLength(0);
        animation.Completed += (sender, args) => {
          Grid.RowDefinitions[1].Height = new GridLength(0);

        };
      } else {
        animation.To = new GridLength(_contentHeight);
        animation.Completed += (sender, args) => {
          Grid.RowDefinitions[1].Height = GridLength.Auto;
        };
      }
      Grid.RowDefinitions[1].BeginAnimation(RowDefinition.HeightProperty, animation);
    }
  }
}
