using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Controls.Notification;
using System.Windows.Media;
using System.Collections.Generic;
using Controls.TransitionLabel;

namespace Test {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged {

    public List<string> LoginItems { get; set; }

    private Color _color;

    public Color Color {
      get { return _color; }
      set {
        if (_color == value) return;
        _color = value;
        OnPropertyChanged();
        Console.WriteLine("Zmiana");
      }
    }

    public List<Color> colors = new List<Color>();

    ObservableCollection<double> values = new ObservableCollection<double> { 40, 60, 60, 60, 60, 60 };
    public MainWindow() {
      InitializeComponent();
      Chart.Values = values;
      Chart.MaxValue = 360;

      colors.Add(Colors.Beige);
      colors.Add(Colors.Aquamarine);
      colors.Add(Colors.DarkGoldenrod);
      colors.Add(Colors.Crimson);
      colors.Add(Colors.Coral);
      //Chart.Colors = colors;

      //BarChart.Values = values;
      //BarChart.MaxValue = maxValue;
      //BarChart.Colors = colors;
      //Chart.Values = new ObservableCollection<KeyValuePair<int, double>>();
      //Chart.Values.Add(new KeyValuePair<int, double>(1, 1));
      //Chart.Values.Add(new KeyValuePair<int, double>(2, 3));
      //Chart.Values.Add(new KeyValuePair<int, double>(3, 6));
      //Chart.Values.Add(new KeyValuePair<int, double>(4, 7));
      //Chart.Values.Add(new KeyValuePair<int, double>(5, 3));
      //Chart.Values.Add(new KeyValuePair<int, double>(6, 6));
      //Chart.Values.Add(new KeyValuePair<int, double>(7, 7));
      Toaster.Instance = ToastList;
      DataContext = this;
      InitLabelItem();
    }

    Random lRandeom = new Random();

    private void Button_Click(object sender, RoutedEventArgs e) {
      TranslateContentControl.Direction = TranslateContentControl.Direction == TranslateContentControl.TranslateDirection.FromLeft
        ? TranslateContentControl.TranslateDirection.FromRight
        : TranslateContentControl.TranslateDirection.FromLeft;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e) {
      ToastList.ToastPropertieses.Add(new ToastProperties { Message = lRandeom.Next().ToString() });
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string name = "") {
      if (PropertyChanged != null) {
        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
      }
    }

    private void ValueDescriptionDodajClick(object sender, RoutedEventArgs e) {
      ValueDescription.Value += "a";
    }

    private void ValueDescriptionZmniejszClick(object sender, RoutedEventArgs e) {
      ValueDescription.Value = ValueDescription.Value.Substring(0, ValueDescription.Value.Length - 1);
    }

    private void InitLabelItem() {
      LoginItems = new List<string>(new []{"jeden","dwa", "trzy"});
    }

    private void LoginItemListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
      Console.WriteLine("zmiana");
    }
  }
}
