using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controls.Notification {
  /// <summary>
  /// Interaction logic for Toast.xaml
  /// </summary>
  public partial class Toast : UserControl {

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
      "CornerRadius", typeof(double), typeof(Toast), new FrameworkPropertyMetadata(default(double)));

    public double CornerRadius {
      get { return (double)GetValue(IconProperty); }
      set { SetValue(IconProperty, value); }
    }

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
      "Icon", typeof(ImageSource), typeof(Toast), new FrameworkPropertyMetadata(default(ImageSource)));

    public ImageSource Icon {
      get { return GetValue(IconProperty) as ImageSource; }
      set { SetValue(IconProperty, value); }
    }

    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
      "Message", typeof(string), typeof(Toast), new FrameworkPropertyMetadata(default(string)));

    public string Message {
      get { return GetValue(MessageProperty) as string; }
      set { SetValue(MessageProperty, value); }
    }

    public Toast() {
      InitializeComponent();
    }

    public Toast(ToastProperties pToastProperties) : this(){
      Message = pToastProperties.Message;
      Icon = pToastProperties.Icon;
    }
  }

  public enum ToastLevel {
    Info,
    Alert,
    Error,
  }
}