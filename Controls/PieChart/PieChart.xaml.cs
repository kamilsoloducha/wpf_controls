using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Controls.Helpers;
using Controls.Piechart;
using System.Collections.Generic;

namespace Controls.PieChart {
  /// <summary>
  /// Interaction logic for UserControl1.xaml
  /// </summary>
  public partial class PieChart : UserControl {

    public delegate string TextChangeDelegate(PiePiece pPiePiece);
    public TextChangeDelegate TextChange { get; set; }

    #region Dependencies

    /// <summary>
    /// Values of chart
    /// </summary>
    public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
      "Values", typeof(ObservableCollection<double>), typeof(PieChart), new UIPropertyMetadata(new ObservableCollection<double>(), ValueChangedCallback));

    private static void ValueChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      if (!(dependencyObject is PieChart)) {
        return;
      }
      PieChart chart = dependencyObject as PieChart;
      if (dependencyPropertyChangedEventArgs.NewValue == null) {
        foreach (PiePiece piece in chart.PieList) {
          chart.Canvas.Children.Remove(piece);
        }
        chart.PieList.Clear();
        return;
      }
      if (!(dependencyPropertyChangedEventArgs.NewValue is ObservableCollection<double>)) {
        return;
      }
      ObservableCollection<double> newCollection = dependencyPropertyChangedEventArgs.NewValue as ObservableCollection<double>;
      int newCount = newCollection.Count - chart.PieList.Count;
      for (int i = 0; i < newCount; i++) {
        PiePiece lastPiece = chart.PieList.LastOrDefault();
        PiePiece newPiece = chart.AddPiePiece();
        if (lastPiece != null) {
          double newStartAngle = lastPiece.StartAngle + lastPiece.WedgeAngle;
          newPiece.StartAngle = newStartAngle;
        }
      }
      chart.AnimateChart();
    }

    public ObservableCollection<double> Values {
      get { return GetValue(ValuesProperty) as ObservableCollection<double>; }
      set { SetValue(ValuesProperty, value); }
    }

    /// <summary>
    /// Colors of part of chart
    /// </summary>
    public static readonly DependencyProperty ColorsProperty = DependencyProperty.Register(
      "Colors", typeof(Color[]), typeof(PieChart), new UIPropertyMetadata(new[] { System.Windows.Media.Colors.Red, System.Windows.Media.Colors.Green, System.Windows.Media.Colors.Blue }));

    public Color[] Colors {
      get { return GetValue(ColorsProperty) as Color[]; }
      set { SetValue(ColorsProperty, value); }
    }

    /// <summary>
    /// Maximum value of chart
    /// </summary>
    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
      "MaxValue", typeof(double), typeof(PieChart), new UIPropertyMetadata(100d, MaxValueChangedCallback));

    private static void MaxValueChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      if (!(dependencyObject is PieChart)) {
        return;
      }
      PieChart chart = dependencyObject as PieChart;
      chart.AnimateChart();
    }

    public double MaxValue {
      get { return (double)GetValue(MaxValueProperty); }
      set { SetValue(MaxValueProperty, value); }
    }

    /// <summary>
    /// Indicate selected piece of chart
    /// </summary>
    public static readonly DependencyProperty SelectedPieceProperty = DependencyProperty.Register(
      "SelectedPiece", typeof(int), typeof(PieChart), new UIPropertyMetadata(-1, SelectedPieceChangedCallback));

    private static void SelectedPieceChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      if (!(dependencyObject is PieChart)) {
        return;
      }
      PieChart chart = dependencyObject as PieChart;
      int newValue = (int)dependencyPropertyChangedEventArgs.NewValue;
      int oldValue = (int)dependencyPropertyChangedEventArgs.OldValue;
      if (newValue < 0 || newValue >= chart.PieList.Count) {
        foreach (PiePiece piece in chart.PieList.Where(x => x.PushOut > 0)) {
          chart.MakePieInvisible(piece);
        }
        chart.SetCenterLabel("");
      } else {
        if (oldValue >= 0) {
          chart.MakePieInvisible(chart.PieList[oldValue]);
        }
        chart.MakePieVisible(chart.PieList[newValue]);
        chart.SetCenterLabel(chart.TextChange(chart.PieList[newValue]));
      }
    }

    public int SelectedPiece {
      get { return (int)GetValue(SelectedPieceProperty); }
      set {

        SetValue(SelectedPieceProperty, value);
      }
    }

    /// <summary>
    /// The way in which will be text displayed
    /// </summary>
    public static readonly DependencyProperty DescriptionTextProperty = DependencyProperty.Register(
      "DescriptionText", typeof(DescriptionTextType), typeof(PieChart), new UIPropertyMetadata(DescriptionTextType.Angle));

    public DescriptionTextType DescriptionText {
      get { return (DescriptionTextType)GetValue(DescriptionTextProperty); }
      set { SetValue(DescriptionTextProperty, value); }
    }

    /// <summary>
    /// Color behind the chart ring
    /// </summary>
    public static readonly DependencyProperty RingBackgroundProperty = DependencyProperty.Register(
      "RingBackground", typeof(Brush), typeof(PieChart), new UIPropertyMetadata(Brushes.LightGray, RingBackgroundChangedCallback));

    private static void RingBackgroundChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      if (!(dependencyObject is PieChart)) {
        return;
      }
      PieChart chart = dependencyObject as PieChart;
      if (!(dependencyPropertyChangedEventArgs.NewValue is Brush)) {
        return;
      }
      chart.BackgroundPiece.Fill = (Brush)dependencyPropertyChangedEventArgs.NewValue;
    }

    public Brush RingBackground {
      get { return (Brush)GetValue(RingBackgroundProperty); }
      set { SetValue(RingBackgroundProperty, value); }
    }

    /// <summary>
    /// Is chart animated
    /// </summary>
    public static readonly DependencyProperty IsAnimateProperty = DependencyProperty.Register(
      "IsAnimate", typeof(bool), typeof(PieChart), new UIPropertyMetadata(default(bool)));

    public bool IsAnimate {
      get { return (bool)GetValue(IsAnimateProperty); }
      set { SetValue(IsAnimateProperty, value); }
    }

    public static readonly DependencyProperty RingWidthFactorProperty = DependencyProperty.Register(
      "RingWidthFactor", typeof(double), typeof(PieChart), new PropertyMetadata(default(double)));

    public double RingWidthFactor {
      get { return (double)GetValue(RingWidthFactorProperty); }
      set {
        if (value > 1) value = 1;
        if (value < 0) value = 0;
        SetValue(RingWidthFactorProperty, value);
      }
    }

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
      "Title", typeof(string), typeof(PieChart), new PropertyMetadata(default(string)));

    public string Title {
      get { return (string)GetValue(TitleProperty); }
      set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty ValueFontSizeProperty = DependencyProperty.Register(
      "ValueFontSize", typeof(int), typeof(PieChart), new PropertyMetadata(15));

    public int ValueFontSize {
      get { return (int)GetValue(ValueFontSizeProperty); }
      set { SetValue(ValueFontSizeProperty, value); }
    }

    #endregion

    #region Properties

    public EasingFunctionBase EasingFunction { get; set; }
    public double StrokeThickness { get; set; }

    #endregion

    private readonly IList<PiePiece> PieList = new List<PiePiece>();
    private readonly PiePiece BackgroundPiece = new PiePiece {
      StartAngle = 0,
      WedgeAngle = 360,
    };

    public PieChart() {
      DependencyPropertyDescriptor.FromProperty(ValuesProperty, typeof(PieChart)).AddValueChanged(this, (sender, args) => {
        if (Values != null)
          Values.CollectionChanged += (o, eventArgs) => AnimateChart();
      });
      InitializeComponent();
      TextChange = CreateDescriptionText;
      BackgroundPiece.Fill = Background;
      Canvas.Children.Add(BackgroundPiece);
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
      base.OnRenderSizeChanged(sizeInfo);
      RefreshView();
    }

    private void RefreshView() {
      double size = Math.Min(ActualHeight - Grid.RowDefinitions[0].ActualHeight - Padding.Bottom - Padding.Top, ActualWidth - Padding.Left - Padding.Right);
      if (size < 0) {
        return;
      }
      Canvas.Width = size;
      Canvas.Height = size;
      Canvas.InvalidateVisual();
      BackgroundPiece.Radius = size / 2;
      BackgroundPiece.InnerRadius = BackgroundPiece.Radius * RingWidthFactor;
      BackgroundPiece.InvalidateVisual();
      Canvas.SetLeft(BackgroundPiece, size / 2);
      Canvas.SetTop(BackgroundPiece, size / 2);
      foreach (PiePiece item in PieList) {
        item.Radius = size / 2;
        item.InnerRadius = item.Radius * RingWidthFactor;
        item.InvalidateVisual();
        Canvas.SetLeft(item, size / 2);
        Canvas.SetTop(item, size / 2);
      }
      RefreshValueText();
    }

    private PiePiece AddPiePiece() {
      PiePiece newPiece = new PiePiece();
      newPiece.MouseDown += LPiepieceOnMouseUp;
      PiePiece lastPiece = PieList.LastOrDefault();
      if (lastPiece != null) {
        newPiece.StartAngle = lastPiece.StartAngle + lastPiece.WedgeAngle;
      }
      PieList.Add(newPiece);
      Canvas.SetTop(newPiece, Canvas.ActualHeight / 2);
      Canvas.SetLeft(newPiece, Canvas.ActualWidth / 2);
      Canvas.Children.Add(newPiece);
      return newPiece;
    }

    private void RemovePiePiece(PiePiece pPiePiece) {
      PieList.Remove(pPiePiece);
      Canvas.Children.Remove(pPiePiece);
    }

    private void AnimateChart() {
      double lStartAngle = StrokeThickness;
      int i = 0;
      for (; i < Values.Count; i++) {
        if (PieList.Count - 1 < i) AddPiePiece();
        PiePiece piePiece = PieList[i];
        piePiece.Fill = new SolidColorBrush(Colors[i % Colors.Count()]);
        piePiece.Radius = BackgroundPiece.Radius;
        piePiece.InnerRadius = piePiece.Radius * RingWidthFactor;
        double lWedgeAngle = ConvertValueToAngle(Values[i]);
        lWedgeAngle = ((lWedgeAngle - StrokeThickness) < 0) ? 0 : lWedgeAngle - StrokeThickness;
        AnimateAngle(piePiece, PiePiece.StartAngleProperty, lStartAngle);
        AnimateAngle(piePiece, PiePiece.WedgeAngleProperty, lWedgeAngle);
        lStartAngle += lWedgeAngle;
        lStartAngle += StrokeThickness;
      }
      for (; i < PieList.Count; i++) {
        PiePiece piePiece = PieList[i];
        AnimateAngle(piePiece, PiePiece.StartAngleProperty, lStartAngle, (sender, args) => RemovePiePiece(piePiece));
        AnimateAngle(piePiece, PiePiece.WedgeAngleProperty, 0, (sender, args) => RemovePiePiece(piePiece));
      }
      RefreshDescriptonText();
    }

    private void AnimateAngle(PiePiece pPiePiece, DependencyProperty pDependencyProperty, double pAngle, EventHandler pEventHandler = null) {
      double oldAngle = (double)pPiePiece.GetValue(pDependencyProperty);
      DoubleAnimation animation = new DoubleAnimation(oldAngle, pAngle, new Duration(TimeSpan.FromMilliseconds(IsAnimate ? 500 : 0)), FillBehavior.HoldEnd);
      animation.EasingFunction = EasingFunction;
      if (pEventHandler != null) {
        animation.Completed += pEventHandler;
      }
      pPiePiece.BeginAnimation(pDependencyProperty, animation);
    }

    private double ConvertValueToAngle(double pValue) {
      double lMaxValue = 100;
      if (MaxValue > 0) {
        lMaxValue = MaxValue;
      }
      if (pValue > lMaxValue)
        pValue = lMaxValue;
      return 360 * pValue / lMaxValue;
    }

    private void RefreshDescriptonText() {
      if (SelectedPiece >= 0 && SelectedPiece < PieList.Count) {
        SetCenterLabel(CreateDescriptionText(PieList[SelectedPiece]));
      } else {
        SetCenterLabel("");
      }
    }

    private void RefreshValueText() {
      TextSizeCalculator.Calculate(CenterLabel, BackgroundPiece.InnerRadius * 2 * 0.75);
    }

    private string CreateDescriptionText(PiePiece pClickedPiePiece) {
      double lValue = Values[PieList.IndexOf(pClickedPiePiece)];
      switch (DescriptionText) {
        case DescriptionTextType.None:
          return "";
        case DescriptionTextType.Percentage:
          return String.Format("{0:0.0}%", lValue * 100 / MaxValue);
        case DescriptionTextType.Value:
          return String.Format("{0}/{1}", (int)lValue, (int)MaxValue);
        case DescriptionTextType.Angle:
          return pClickedPiePiece.WedgeAngle.ToString();
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void MakePieVisible(PiePiece pClickedPiePiece) {
      PiePiece lLastClickedPiePiece = PieList.FirstOrDefault(x => x.PushOut > 0.0);
      if (lLastClickedPiePiece != null) {
        MakePieInvisible(lLastClickedPiePiece);
      }
      DoubleAnimation lShow = new DoubleAnimation(BackgroundPiece.Radius * 0.04, new Duration(TimeSpan.FromMilliseconds(IsAnimate ? 200 : 0)));
      pClickedPiePiece.BeginAnimation(PiePiece.PushOutProperty, lShow);
    }

    private void MakePieInvisible(PiePiece pClickedPiePiece) {
      DoubleAnimation lHide = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(IsAnimate ? 200 : 0)));
      pClickedPiePiece.BeginAnimation(PiePiece.PushOutProperty, lHide);
    }

    private void LPiepieceOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs) {
      PiePiece lPiepiece = sender as PiePiece;
      if (lPiepiece == null) {
        return;
      }
      if (lPiepiece.PushOut > 0) {
        MakePieInvisible(lPiepiece);
        SetCenterLabel("");
      } else {
        SetCenterLabel(TextChange(lPiepiece));
        MakePieVisible(lPiepiece);
      }
    }

    private void SetCenterLabel(string pText) {
      CenterLabel.Text = pText;
      RefreshValueText();
    }

    public enum DescriptionTextType {
      None,
      Percentage,
      Value,
      Angle,
    }

    private void PieChart_OnLoaded(object sender, RoutedEventArgs e) {
      RefreshView();
    }
  }
}