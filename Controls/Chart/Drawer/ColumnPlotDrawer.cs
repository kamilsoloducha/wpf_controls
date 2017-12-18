using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Controls.Chart.Value;

namespace Controls.Chart.Drawer {

  public class ColumnPlotDrawer : PlotDrawer {

    public override void Draw() {
      FindMaxMinValue();
      foreach (Shape lColumn in Markers) {
        Canvas.Children.Remove(lColumn);
      }
      Markers.Clear();
      double lWidth = FindChartPlot(Values[0]).X;
      foreach (KeyValuePair<int, double> lPoint in Values) {
        Point lPosition = FindChartPlot(lPoint);
        Console.WriteLine("New Point - {0}", lPosition);
        ValueShape lColumn = new ValueColumn {
          ColumnWidth = lWidth - 10,
          Position = lPosition,
          Stroke = Brushes.Blue,
          Fill = Brushes.DodgerBlue,
        };
        Markers.Add(lColumn);
        Canvas.Children.Add(lColumn);
      }
    }
  }
}