using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Controls.Notification {
  /// <summary>
  /// Interaction logic for ToastList.xaml
  /// </summary>
  public partial class ToastList : UserControl {

    public static readonly DependencyProperty ToastPropertiesesProperty = DependencyProperty.Register(
      "ToastPropertieses", typeof(ObservableCollection<ToastProperties>), typeof(ToastList), new PropertyMetadata(new ObservableCollection<ToastProperties>()));

    public ObservableCollection<ToastProperties> ToastPropertieses {
      get { return (ObservableCollection<ToastProperties>)GetValue(ToastPropertiesesProperty); }
    }

    public static readonly DependencyProperty ToastBackgroundProperty = DependencyProperty.Register(
      "ToastBackground", typeof(Brush), typeof(ToastList), new PropertyMetadata(default(Brush)));

    public Brush ToastBackground {
      get { return (Brush)GetValue(ToastBackgroundProperty); }
      set { SetValue(ToastBackgroundProperty, value); }
    }

    public static readonly DependencyProperty ToastForegroundProperty = DependencyProperty.Register(
      "ToastForeground", typeof(Brush), typeof(ToastList), new PropertyMetadata(default(Brush)));

    public Brush ToastForeground {
      get { return (Brush)GetValue(ToastForegroundProperty); }
      set { SetValue(ToastForegroundProperty, value); }
    }

    public static readonly DependencyProperty ToastFontSizeProperty = DependencyProperty.Register(
      "ToastFontSize", typeof(double), typeof(ToastList), new PropertyMetadata(12.0d));

    public double ToastFontSize {
      get { return (double)GetValue(ToastFontSizeProperty); }
      set { SetValue(ToastFontSizeProperty, value); }
    }

    public ToastList() {
      InitializeComponent();
      ToastPropertieses.CollectionChanged += ToastPropertiesesOnCollectionChanged;
    }

    private void ToastPropertiesesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) {
      switch (notifyCollectionChangedEventArgs.Action) {
        case NotifyCollectionChangedAction.Add:
          AddVisualToast(notifyCollectionChangedEventArgs.NewItems[0] as ToastProperties);
          break;
      }
    }

    private void AddVisualToast(ToastProperties lNewItem) {
      Toast lNewToast = new Toast(lNewItem);
      lNewToast.Background = ToastBackground;
      lNewToast.FontSize = ToastFontSize;
      lNewToast.Foreground = ToastForeground;
      lNewToast.MouseEnter += NewToastOnMouseEnter;
      lNewToast.MouseLeave += NewToastOnMouseLeave;
      lNewToast.MouseDown += NewToastOnMouseDown;
      lNewToast.Loaded += LNewToastOnLoaded;
      lNewToast.Opacity = 0;
      lNewToast.Margin = new Thickness(10);
      StartDisappearAnimation(lNewToast);
      ItemsControl.Items.Insert(0, lNewToast);
    }

    private void LNewToastOnLoaded(object sender, RoutedEventArgs routedEventArgs) {
      StartAppearAnimation(sender as Control);
    }

    private void NewToastOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs) {
      Toast lToast = sender as Toast;
      if (lToast == null) return;
      ItemsControl.Items.Remove(lToast);
    }

    private void NewToastOnMouseLeave(object sender, MouseEventArgs mouseEventArgs) {
      Toast lToast = sender as Toast;
      if (lToast == null) return;
      StartDisappearAnimation(lToast);
    }

    private void NewToastOnMouseEnter(object sender, MouseEventArgs mouseEventArgs) {
      Toast lToast = sender as Toast;
      if (lToast == null) return;
      lToast.BeginAnimation(OpacityProperty, null);
      lToast.BeginAnimation(OpacityProperty, new DoubleAnimation(1.0, new Duration(TimeSpan.FromMilliseconds(100))));
    }

    private void StartAppearAnimation(Control pControl) {
      TimeSpan heightAnimationDuration = TimeSpan.FromMilliseconds(200);
      DoubleAnimation heightAnimation = new DoubleAnimation(0, pControl.ActualHeight, new Duration(heightAnimationDuration));
      heightAnimation.Completed += (sender, args) => { pControl.Opacity = 1; };
      pControl.BeginAnimation(HeightProperty, heightAnimation);
      TranslateTransform translateTransform = new TranslateTransform();
      DoubleAnimation marginAnimation = new DoubleAnimation(-pControl.ActualWidth, 0, new Duration(TimeSpan.FromMilliseconds(200))) {
        BeginTime = heightAnimationDuration,
      };
      pControl.RenderTransform = translateTransform;
      translateTransform.BeginAnimation(TranslateTransform.XProperty, marginAnimation);
      
    }

    private void StartDisappearAnimation(Control lControl) {
      DoubleAnimation lAnimation = new DoubleAnimation(0.0, new Duration(TimeSpan.FromMilliseconds(1000)));
      lAnimation.BeginTime = TimeSpan.FromSeconds(4);
      lAnimation.Completed += (sender, args) => {
        if (lControl.Opacity < 0.01) {
          ItemsControl.Items.Remove(lControl);
        }
      };
      lControl.BeginAnimation(OpacityProperty, lAnimation);
    }
  }
}
