using System;
using System.Windows;
using System.Windows.Controls;

namespace Controls.LoginItem {
  /// <summary>
  /// Interaction logic for LoginItem.xaml
  /// </summary>
  public partial class LoginItem : UserControl {

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
      "Label", typeof(string), typeof(LoginItem), new PropertyMetadata("asdfasdf", PropertyChangedCallback));

    private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      var control = (LoginItem)dependencyObject;
      if (control == null) {
        return;
      }
      control.Label = (string)dependencyPropertyChangedEventArgs.NewValue;
    }

    public string Label {
      get { return (string)GetValue(LabelProperty); }
      set { SetValue(LabelProperty, value); }
    }

    public LoginItem() {
      InitializeComponent();
    }


  }
}
