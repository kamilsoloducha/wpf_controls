using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
namespace Controls.Chart.Value {
  public class ValuesLine : ValueShape {

    public List<Point> Values { get; set; } 

    protected override void DrawGeometry(StreamGeometryContext context) {
      context.BeginFigure(Values[0], false, false);
      List<Point> lPoints = new List<Point>();
      PolyLineSegment line = new PolyLineSegment();
      lPoints.AddRange(Values.GetRange(1, Values.Count - 1));
      context.PolyLineTo(lPoints, true, true);
    }

  }
}
