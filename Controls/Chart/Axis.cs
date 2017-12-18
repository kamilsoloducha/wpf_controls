using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Controls.Chart {
  public class Axis : Shape {

    protected override Geometry DefiningGeometry {
      get {
        StreamGeometry geometry = new StreamGeometry();
        geometry.FillRule = FillRule.EvenOdd;
        using (StreamGeometryContext context = geometry.Open()) {
          DrawGeometry(context);
        }
        geometry.Freeze();
        return geometry;
      }
    }

    private void DrawGeometry(StreamGeometryContext context) {
      double lActualHeight = (double)Parent.GetValue(ActualHeightProperty);
      double lActualWidth = (double)Parent.GetValue(ActualWidthProperty);

      context.BeginFigure(new Point(0, lActualHeight), false, false);
      context.LineTo(new Point(lActualWidth, lActualHeight), true, true);

      context.BeginFigure(new Point(0, lActualHeight), false, false);
      context.LineTo(new Point(0, 0), true, true);

      //context.BeginFigure(new Point(lActualWidth, lActualHeight), false, false);
      //context.LineTo(new Point(lActualWidth - 5, lActualHeight - 5), true, true);

      //context.BeginFigure(new Point(lActualWidth, lActualHeight), false, false);
      //context.LineTo(new Point(lActualWidth - 5, lActualHeight + 5), true, true);

      //context.BeginFigure(new Point(0, 0), false, false);
      //context.LineTo(new Point(5, 5), true, true);

      //context.BeginFigure(new Point(0, 0), false, false);
      //context.LineTo(new Point(-5, 5), true, true);
    }

  }
}
