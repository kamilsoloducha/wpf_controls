using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Controls.Chart.Value;

namespace Controls.Chart.Drawer {

  public class DotPlotDrawer : PlotDrawer {

    public override void Draw() {
      FindMaxMinValue();
      foreach (Shape lPoint in Markers) {
        Canvas.Children.Remove(lPoint);
      }
      Markers.Clear();
      foreach (KeyValuePair<int, double> lPoint in Values) {
        Point lPosition = FindChartPlot(lPoint);
        Console.WriteLine("New Point - {0}", lPosition);
        ValuePoint lNewValuePoint = new ValuePoint() {
          Position = lPosition,
          Stroke = Brushes.Blue,
          Fill = Brushes.Blue,
        };
        Markers.Add(lNewValuePoint);
        Canvas.Children.Add(lNewValuePoint);
      }
    }
  }
}