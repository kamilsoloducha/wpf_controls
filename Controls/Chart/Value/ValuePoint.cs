using System.Windows;
using System.Windows.Media;

namespace Controls.Chart.Value {
  public class ValuePoint : ValueShape {

    protected override void DrawGeometry(StreamGeometryContext context) {

      context.BeginFigure(new Point(Position.X + 5, Position.Y), true, true);
      context.LineTo(new Point(Position.X, Position.Y + 5), true, true);
      context.LineTo(new Point(Position.X - 5, Position.Y), true, true);
      context.LineTo(new Point(Position.X, Position.Y - 5), true, true);
    }
  }
}