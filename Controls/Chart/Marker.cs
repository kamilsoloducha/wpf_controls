using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
namespace Controls.Chart {
  public class Marker : Shape {

    public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
      "Position", typeof(Point), typeof(Marker), new UIPropertyMetadata(default(Point)));

    public Point Position {
      get { return (Point)GetValue(PositionProperty); }
      set { SetValue(PositionProperty, value); }
    }

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
      context.BeginFigure(new Point(Position.X, 0), false, false);
      context.LineTo(new Point(Position.X, (double)Parent.GetValue(ActualHeightProperty)), true, true);

      context.BeginFigure(new Point(0, Position.Y), false, false);
      context.LineTo(new Point((double)Parent.GetValue(ActualWidthProperty), Position.Y), true, true);
    }

  }
}
