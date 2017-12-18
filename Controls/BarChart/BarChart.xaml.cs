using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Controls.BarChart {
  /// <summary>
  /// Interaction logic for UserControl1.xaml
  /// </summary>
  public partial class BarChart : UserControl {

    public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
      "Values", typeof(ObservableCollection<double>), typeof(BarChart), new FrameworkPropertyMetadata(
        default(ObservableCollection<double>), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    public ObservableCollection<double> Values {
      get { return (ObservableCollection<double>)GetValue(ValuesProperty); }
      set { SetValue(ValuesProperty, value); }
    }

    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
      "MaxValue", typeof(double), typeof(BarChart), new PropertyMetadata(default(double)));

    public double MaxValue {
      get { return (double)GetValue(MaxValueProperty); }
      set { SetValue(MaxValueProperty, value); }
    }

    public static readonly DependencyProperty ColorsProperty = DependencyProperty.Register(
      "Colors", typeof(Color[]), typeof(BarChart), new PropertyMetadata(default(Color[])));

    public Color[] Colors {
      get { return (Color[])GetValue(ColorsProperty); }
      set { SetValue(ColorsProperty, value); }
    }

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
      "SelectedItem", typeof(int), typeof(BarChart), new PropertyMetadata(-1));

    public int SelectedItem {
      get { return (int)GetValue(SelectedItemProperty); }
      set { SetValue(SelectedItemProperty, value); }
    }

    public static readonly DependencyProperty MarkerHeightProperty = DependencyProperty.Register(
      "MarkerHeight", typeof(double), typeof(BarChart), new PropertyMetadata(2.0));

    public double MarkerHeight {
      get { return (double)GetValue(MarkerHeightProperty); }
      set { SetValue(MarkerHeightProperty, value); }
    }

    private readonly List<Rectangle> _rectangleList = new List<Rectangle>();
    private Rectangle _selectedRectangle;

    public BarChart() {
      DependencyPropertyDescriptor.FromProperty(ValuesProperty, typeof(BarChart))
        .AddValueChanged(this, (sender, args) => {
          CreateBarPieces();
          Values.CollectionChanged += ValuesOnCollectionChanged;
        });
      DependencyPropertyDescriptor.FromProperty(SelectedItemProperty, typeof(BarChart))
        .AddValueChanged(this, (sender, args) => {
          if (SelectedItem == -1) {
            if (_selectedRectangle != null)
              UnselectItem(_selectedRectangle);
            _selectedRectangle = null;
          } else {
            if (_selectedRectangle != null)
              UnselectItem(_selectedRectangle);
            _selectedRectangle = _rectangleList[SelectedItem];
            SelectItem(_selectedRectangle);
          }
        });

      InitializeComponent();
      Loaded += (sender, args) => CreateBarPieces();
      SizeChanged += (sender, args) => CreateBarPieces();
    }

    private void ValuesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) {
      double lValuesSum = 0;
      for (int i = 0; i < _rectangleList.Count - 1; i++) {
        Rectangle lRectangle = _rectangleList[i];
        lValuesSum += Values[i];
        double lWidth = Values[i] / MaxValue * ActualWidth;
        lRectangle.BeginAnimation(WidthProperty, new DoubleAnimation(lWidth, new Duration(TimeSpan.FromMilliseconds(200))));
      }
      if (lValuesSum < MaxValue) {
        _rectangleList.Last().BeginAnimation(WidthProperty, new DoubleAnimation((MaxValue - lValuesSum) / MaxValue * ActualWidth, new Duration(TimeSpan.FromMilliseconds(200))));
      }
    }

    private void CreateBarPieces() {
      if (Colors == null) return;
      MainPanel.Children.Clear();
      _rectangleList.Clear();
      double lHeight = Height - MarkerHeight;
      double lValuesSum = 0;
      if (Values != null) {
        for (int i = 0; i < Values.Count; i++) {
          double lValue = Values[i];
          lValuesSum += lValue;
          Rectangle lRectangle = new Rectangle();
          lRectangle.Height = lHeight;
          lRectangle.Width = lValue / MaxValue * ActualWidth;
          lRectangle.Fill = new SolidColorBrush(Colors[i % Colors.Length]);
          lRectangle.MouseUp += RectangleOnMouseUp;
          lRectangle.VerticalAlignment = VerticalAlignment.Bottom;
          MainPanel.Children.Add(lRectangle);
          _rectangleList.Add(lRectangle);
        }
      }

      Rectangle lGrayRectangle = new Rectangle();
      lGrayRectangle.Height = lHeight;
      lGrayRectangle.Width = (MaxValue - lValuesSum) / MaxValue * ActualWidth;
      lGrayRectangle.Fill = Brushes.LightGray;
      lGrayRectangle.VerticalAlignment = VerticalAlignment.Bottom;
      MainPanel.Children.Add(lGrayRectangle);
      _rectangleList.Add(lGrayRectangle);
    }

    private void RectangleOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs) {
      Rectangle lClicked = sender as Rectangle;
      if (lClicked == null) return;
      int pClickedIndex = _rectangleList.IndexOf(lClicked);
      if (pClickedIndex == SelectedItem) {
        UnselectItem(lClicked);
        SelectedItem = -1;
      } else {
        if (SelectedItem >= 0)
          UnselectItem(_rectangleList[SelectedItem]);
        SelectItem(lClicked);
        SelectedItem = pClickedIndex;
      }
    }

    private void SelectItem(Rectangle pItem) {
      pItem.BeginAnimation(HeightProperty, new DoubleAnimation(Height, new Duration(TimeSpan.FromMilliseconds(200))));
      _selectedRectangle = pItem;
    }

    private void UnselectItem(Rectangle pItem) {
      pItem.BeginAnimation(HeightProperty, new DoubleAnimation(Height - MarkerHeight, new Duration(TimeSpan.FromMilliseconds(200))));
    }
  }
}
