using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Controls.TransitionLabel {

  [TemplatePart(Name = "PART_PaintArea", Type = typeof(Shape)),
   TemplatePart(Name = "PART_MainContent", Type = typeof(ContentPresenter))]
  public class TranslateContentControl : ContentControl {

    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
      "Direction", typeof(TranslateDirection), typeof(TranslateContentControl), new PropertyMetadata(default(TranslateDirection)));

    public TranslateDirection Direction {
      get { return (TranslateDirection)GetValue(DirectionProperty); }
      set { SetValue(DirectionProperty, value); }
    }

    #region Generated static constructor
    static TranslateContentControl() {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(TranslateContentControl), new FrameworkPropertyMetadata(typeof(TranslateContentControl)));
    }
    #endregion

    private Shape ScreenShoot { get; set; }
    private ContentPresenter MainContent { get; set; }

    public override void OnApplyTemplate() {
      ScreenShoot = Template.FindName("PART_PaintArea", this) as Shape;
      MainContent = Template.FindName("PART_MainContent", this) as ContentPresenter;
      base.OnApplyTemplate();
    }

    protected override void OnContentChanged(object oldContent, object newContent) {
      if (ScreenShoot != null && MainContent != null) {
        ScreenShoot.Fill = CreateBrushFromVisual(MainContent);
        BeginAnimateContentReplacement();
      }
      base.OnContentChanged(oldContent, newContent);
    }

    private void BeginAnimateContentReplacement() {
      var newContentTransform = new TranslateTransform();
      var oldContentTransform = new TranslateTransform();
      ScreenShoot.RenderTransform = oldContentTransform;
      MainContent.RenderTransform = newContentTransform;
      ScreenShoot.Visibility = Visibility.Visible;

      if (Direction == TranslateDirection.FromLeft) {
        newContentTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(ActualWidth, 0));
        oldContentTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(0, -ActualWidth, (s, e) => ScreenShoot.Visibility = Visibility.Hidden));
      } else {
        newContentTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(-ActualWidth, 0));
        oldContentTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(0, ActualWidth, (s, e) => ScreenShoot.Visibility = Visibility.Hidden));
      }
    }

    private AnimationTimeline CreateAnimation(double from, double to, EventHandler whenDone = null) {
      IEasingFunction ease = new BackEase { Amplitude = 0.5, EasingMode = EasingMode.EaseOut };
      var duration = new Duration(TimeSpan.FromSeconds(0.2));
      var anim = new DoubleAnimation(from, to, duration) { EasingFunction = ease };
      if (whenDone != null)
        anim.Completed += whenDone;
      anim.Freeze();
      return anim;
    }

    private Brush CreateBrushFromVisual(Visual v) {
      if (v == null)
        throw new ArgumentNullException("v");
      var target = new RenderTargetBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Pbgra32);
      target.Render(v);
      var brush = new ImageBrush(target);
      brush.Freeze();
      return brush;
    }

    public enum TranslateDirection {
      FromLeft,
      FromRight,
    }
  }
}
