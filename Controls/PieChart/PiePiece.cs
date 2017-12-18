using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Controls.Piechart {
  public class PiePiece : Shape {

    public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
      "InnerRadius", typeof(double), typeof(PiePiece), new UIPropertyMetadata(0.0));

    public double InnerRadius {
      get { return (double)GetValue(InnerRadiusProperty); }
      set { SetValue(InnerRadiusProperty, value); }
    }

    public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
      "Radius", typeof(double), typeof(PiePiece), new UIPropertyMetadata(0.0));

    public double Radius {
      get { return (double)GetValue(RadiusProperty); }
      set { SetValue(RadiusProperty, value); }
    }

    public static readonly DependencyProperty WedgeAngleProperty = DependencyProperty.Register(
      "WedgeAngle", typeof(double), typeof(PiePiece), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    public double WedgeAngle {
      get { return (double)GetValue(WedgeAngleProperty); }
      set { SetValue(WedgeAngleProperty, value); }
    }

    public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(
      "StartAngle", typeof(double), typeof(PiePiece), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    public double StartAngle {
      get { return (double)GetValue(StartAngleProperty); }
      set { SetValue(StartAngleProperty, value); }
    }

    public static readonly DependencyProperty CentreXProperty = DependencyProperty.Register(
      "CentreXProperty", typeof(double), typeof(PiePiece), new UIPropertyMetadata(0.0));

    public double CentreX {
      get { return (double)GetValue(CentreXProperty); }
      set { SetValue(CentreXProperty, value); }
    }

    public static readonly DependencyProperty CentreYProperty = DependencyProperty.Register(
      "CentreYProperty", typeof(double), typeof(PiePiece), new UIPropertyMetadata(0.0));

    public double CentreY {
      get { return (double)GetValue(CentreYProperty); }
      set { SetValue(CentreYProperty, value); }
    }

    public static readonly DependencyProperty PushOutProperty = DependencyProperty.Register(
      "PushOutProperty", typeof(double), typeof(PiePiece), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public double PushOut {
      get { return (double)GetValue(PushOutProperty); }
      set { SetValue(PushOutProperty, value); }
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
      Point innerArcStartPoint = ComputeCartesianCoordinate(StartAngle, InnerRadius + PushOut);
      innerArcStartPoint.Offset(CentreX, CentreY);

      Point innerArcEndPoint = ComputeCartesianCoordinate(StartAngle + WedgeAngle, InnerRadius + PushOut);
      innerArcEndPoint.Offset(CentreX, CentreY);

      Point innerArcMidPoint = ComputeCartesianCoordinate(StartAngle + WedgeAngle / 2, InnerRadius + PushOut);
      innerArcEndPoint.Offset(CentreX, CentreY);

      Point outerArcStartPoint = ComputeCartesianCoordinate(StartAngle, Radius + PushOut);
      outerArcStartPoint.Offset(CentreX, CentreY);

      Point outerArcEndPoint = ComputeCartesianCoordinate(StartAngle + WedgeAngle, Radius + PushOut);
      outerArcEndPoint.Offset(CentreX, CentreY);

      Point outerArcMidPoint = ComputeCartesianCoordinate(StartAngle + WedgeAngle / 2, Radius + PushOut);
      outerArcEndPoint.Offset(CentreX, CentreY);


      Size outerArcSize = new Size(Radius + PushOut, Radius + PushOut);
      Size innerArcSize = new Size(InnerRadius + PushOut, InnerRadius + PushOut);

      context.BeginFigure(innerArcStartPoint, true, true);
      context.LineTo(outerArcStartPoint, true, true);
      context.ArcTo(outerArcMidPoint, outerArcSize, 0, WedgeAngle / 2 > 180.0, SweepDirection.Clockwise, false, true);
      context.ArcTo(outerArcEndPoint, outerArcSize, 0, WedgeAngle / 2 > 180.0, SweepDirection.Clockwise, false, true);
      context.LineTo(innerArcEndPoint, true, true);
      context.ArcTo(innerArcMidPoint, innerArcSize, 0, WedgeAngle / 2 > 180.0, SweepDirection.Counterclockwise, false, true);
      context.ArcTo(innerArcStartPoint, innerArcSize, 0, WedgeAngle / 2 > 180.0, SweepDirection.Counterclockwise, false, true);
    }

    private static Point ComputeCartesianCoordinate(double angle, double radius) {
      // convert to radians
      double angleRad = (Math.PI / 180.0) * (angle - 90);

      double x = radius * Math.Cos(angleRad);
      double y = radius * Math.Sin(angleRad);

      return new Point(x, y);
    }
  }
}
