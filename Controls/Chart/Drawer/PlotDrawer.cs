using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Controls.Chart.Value;

namespace Controls.Chart.Drawer {
  public abstract class PlotDrawer {

    public Canvas Canvas { get; set; }
    public ObservableCollection<KeyValuePair<int, double>> Values { get; set; }
    public List<ValueShape> Markers { get; set; }
    public double MaxValue { get; set; }
    public double MinValue { get; set; }

    protected PlotDrawer() {
      Markers = new List<ValueShape>();
    }

    public abstract void Draw();

    protected Point FindChartPlot(KeyValuePair<int, double> pValue) {
      double x = (Canvas.ActualWidth - 20) * pValue.Key / Values.Count + 1;
      double y = Canvas.ActualHeight - (Canvas.ActualHeight - 20) * pValue.Value / MaxValue;
      return new Point(x, y);
    }

    protected void FindMaxMinValue() {
      if (Values == null) { return; }
      MaxValue = double.MinValue;
      MinValue = double.MaxValue;

      foreach (KeyValuePair<int, double> item in Values) {
        if (item.Value > MaxValue) MaxValue = item.Value;
        if (item.Value < MinValue) MinValue = item.Value;
      }
      Console.WriteLine("Max: {0}, Min: {1}", MaxValue, MinValue);
    }
  }
}
