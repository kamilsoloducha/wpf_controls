using System.Windows;
using System.Windows.Media;

namespace Controls.Chart.Value {
  public  class ValueColumn : ValueShape {

    public double ColumnWidth { get; set; }

    protected override void DrawGeometry(StreamGeometryContext context) {
      double lHeight = (double)Parent.GetValue(ActualHeightProperty);
      context.BeginFigure(Position, true, true);
      context.LineTo(new Point(Position.X, lHeight), true, true);
      context.LineTo(new Point(Position.X - ColumnWidth, lHeight), true, true);
      context.LineTo(new Point(Position.X - ColumnWidth, Position.Y), true, true);
    }
  }
}
