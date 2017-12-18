using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Controls.Chart.Value {
  public abstract class ValueShape : Shape {

    public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
      "Position", typeof(Point), typeof(ValueShape), new UIPropertyMetadata(default(Point)));

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

    protected abstract void DrawGeometry(StreamGeometryContext context);
  }
}
