using Controls.Chart.Drawer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Controls.Chart.Value;

namespace Controls.Chart {
  /// <summary>
  /// Interaction logic for Chart.xaml
  /// </summary>
  public partial class Chart : UserControl {

    #region DependencyProperties

    /// <summary>
    /// MarkerVisibility DependencyProperty
    /// </summary>
    public static readonly DependencyProperty MarkerVisivilityProperty =
    DependencyProperty.Register("MarkerVisibility",
    typeof(Visibility), typeof(Chart),
    new FrameworkPropertyMetadata(default(Visibility), (o, args) => {
      if (!(o is Chart)) { return; }
      Chart chart = o as Chart;
      chart.Marker.Visibility = chart.MarkerVisibility;
    }));

    /// <summary>
    /// MarkerColor DependencyProperty
    /// </summary>
    public static readonly DependencyProperty MarkerColorProperty =
    DependencyProperty.Register("MarkerColor",
    typeof(Color), typeof(Chart),
    new FrameworkPropertyMetadata(Colors.Black, (o, args) => {
      if (!(o is Chart)) { return; }
      Chart chart = o as Chart;
      chart.Marker.Stroke = new SolidColorBrush(chart.MarkerColor);
    }));

    /// <summary>
    /// AxisColor DependencyProperty
    /// </summary>
    public static readonly DependencyProperty AxisColorProperty =
    DependencyProperty.Register("AxisColor",
    typeof(Color), typeof(Chart),
    new FrameworkPropertyMetadata(Colors.Black, (o, args) => {
      if (!(o is Chart)) { return; }
      Chart chart = o as Chart;
      chart.Axises.Stroke = new SolidColorBrush(chart.AxisColor);
    }));

    /// <summary>
    /// AxisLabel X DependencyProperty
    /// </summary>
    public static readonly DependencyProperty AxisLabelXProperty =
    DependencyProperty.Register("AxisLabelX",
    typeof(string), typeof(Chart),
    new FrameworkPropertyMetadata(default(string)));

    /// <summary>
    /// AxisLabel Y DependencyProperty
    /// </summary>
    public static readonly DependencyProperty AxisLabelYProperty =
    DependencyProperty.Register("AxisLabelY",
    typeof(string), typeof(Chart),
    new FrameworkPropertyMetadata(default(string)));

    /// <summary>
    /// AxisLabel Y DependencyProperty
    /// </summary>
    public static readonly DependencyProperty ValuesProperty =
    DependencyProperty.Register("Values",
    typeof(ObservableCollection<KeyValuePair<int, double>>), typeof(Chart),
    new FrameworkPropertyMetadata(default(ObservableCollection<KeyValuePair<int, double>>), (o, args) => {
      if (!(o is Chart)) { return; }
      Chart chart = o as Chart;
      chart.Drawer.Values = chart.Values;
    }));

    #endregion

    #region Properties

    public Visibility MarkerVisibility {
      get { return (Visibility)GetValue(MarkerVisivilityProperty); }
      set { SetValue(MarkerVisivilityProperty, value); }
    }

    public Color MarkerColor {
      get { return (Color)GetValue(MarkerColorProperty); }
      set { SetValue(MarkerColorProperty, value); }
    }

    public Color AxisColor {
      get { return (Color)GetValue(AxisColorProperty); }
      set { SetValue(AxisColorProperty, value); }
    }

    public string AxisLabelX {
      get { return (string)GetValue(AxisLabelXProperty); }
      set { SetValue(AxisLabelXProperty, value); }
    }

    public string AxisLabelY {
      get { return (string)GetValue(AxisLabelYProperty); }
      set { SetValue(AxisLabelYProperty, value); }
    }

    public ObservableCollection<KeyValuePair<int, double>> Values {
      get { return (ObservableCollection<KeyValuePair<int, double>>)GetValue(ValuesProperty); }
      set { SetValue(ValuesProperty, value); }
    }

    private Marker Marker { get; set; }
    private Axis Axises { get; set; }
    private List<ValuePoint> Points { get; set; }
    private PlotDrawer Drawer { get; set; }
    #endregion

    #region Constructors

    public Chart() {
      InitializeComponent();
      Marker = new Marker();
      Marker.Stroke = new SolidColorBrush(MarkerColor);
      //MyCanvas.Children.Add(Marker);

      Axises = new Axis();
      Axises.Stroke = new SolidColorBrush(AxisColor);
      //MyCanvas.Children.Add(Axises);

      DependencyPropertyDescriptor.FromProperty(ValuesProperty, typeof(Chart)).AddValueChanged(this, (sender, args) => {
        Values.CollectionChanged += ValuesOnCollectionChanged;
      });
      Points = new List<ValuePoint>();

      Drawer = new DotPlotDrawer {
        Canvas = MyCanvas,
        Values = Values
      };
    }

    #endregion

    #region Events

    private void Canvas_OnMouseMove(object sender, MouseEventArgs e) {
      if (Marker.Visibility != Visibility) { return; }
      Point lCursorLocation = e.GetPosition(e.Device.Target);
      Console.WriteLine("Move - {0}", lCursorLocation);
      Canvas lCanvas = sender as Canvas;
      if (lCanvas == null) { return; }
      if (lCursorLocation.X > lCanvas.ActualWidth || lCursorLocation.Y > lCanvas.ActualHeight) { return; }
      Marker.Position = lCursorLocation;
      Marker.InvalidateVisual();
    }

    private void Canvas_OnMouseLeave(object sender, MouseEventArgs e) {
      Console.WriteLine("Leave - {0}", e.GetPosition(e.Device.Target));
      Marker.Visibility = Visibility.Hidden;
    }

    private void Canvas_OnMouseEnter(object sender, MouseEventArgs e) {
      Console.WriteLine("Enter - {0}", e.GetPosition(e.Device.Target));
      Marker.Visibility = Visibility.Visible;
    }

    private void ValuesOnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
      Drawer.Draw();
    }
    #endregion

    #region Overrides

    protected override void OnRender(DrawingContext drawingContext) {
      base.OnRender(drawingContext);
      Axises.Height = ActualHeight;
      Axises.Width = ActualWidth;
      Axises.InvalidateVisual();
      MyCanvas.InvalidateVisual();
      MyCanvas.Children.Add(new Rectangle { Width = ActualWidth, Height = ActualHeight, Fill = new SolidColorBrush(Colors.Transparent) });
      Drawer.Draw();
      Console.WriteLine("OnRender");
    }

    #endregion

    #region Methods

    #endregion
  }
}
