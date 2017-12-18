using System;
using System.Windows;
using System.Windows.Controls;
using Controls.Helpers;

namespace Controls.ValueDescription {

  public partial class ValueDescription : UserControl {

    #region Dependecies

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
      "Description", typeof(string), typeof(ValueDescription), new PropertyMetadata(default(string)));

    public string Description {
      get { return (string)GetValue(DescriptionProperty); }
      set {
        RefreshView(Value);
        SetValue(DescriptionProperty, value);
      }
    }

    public static readonly DependencyProperty ValueFontSizeProperty = DependencyProperty.Register(
      "ValueFontSize", typeof(double), typeof(ValueDescription), new PropertyMetadata(12d));

    public double ValueFontSize {
      get { return (double)GetValue(ValueFontSizeProperty); }
      set { SetValue(ValueFontSizeProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
      "Value", typeof(string), typeof(ValueDescription), new PropertyMetadata(default(string), ValueChangedCallback));

    private static void ValueChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      if (!(dependencyObject is ValueDescription)) {
        return;
      }
      var valueDescription = dependencyObject as ValueDescription;
      if (!(dependencyPropertyChangedEventArgs.NewValue is string)) {
        return;
      }
      var newValue = dependencyPropertyChangedEventArgs.NewValue as string;
      if (string.IsNullOrEmpty(newValue)) { 
        return;
      }
      var oldValue = dependencyPropertyChangedEventArgs.OldValue as string;
      if (oldValue == null || newValue.Length != oldValue.Length) {
        valueDescription.RefreshValueText(newValue);
      }
    }

    public string Value {
      get { return (string)GetValue(ValueProperty); }
      set { SetValue(ValueProperty, value); }
    }

    #endregion

    public ValueDescription() {
      InitializeComponent();
      Border.CornerRadius = new CornerRadius(double.MaxValue);
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
      base.OnRenderSizeChanged(sizeInfo);
      RefreshView(Value);
    }

    private void RefreshView(string newText) {
      double size = Math.Min(ActualHeight - Grid.RowDefinitions[0].ActualHeight - Padding.Bottom - Padding.Top - Margin.Bottom - Margin.Top,
        ActualWidth - Padding.Left - Padding.Right - Margin.Left - Margin.Right);
      if (size <= 0)
        return;
      Border.Width = size - Border.Margin.Left - Border.Margin.Right;
      Border.Height = size - Border.Margin.Top - Border.Margin.Bottom;
      RefreshValueText(newText);
    }

    private void RefreshValueText(string newText) {
      if (Border == null || Border.Width <= 0) {
        return;
      }
      TextSizeCalculator.CalculateNewText(ValueText, Border.Width * 0.75, newText);
    }

    private void ValueDescription_OnLoaded(object sender, RoutedEventArgs e) {
      RefreshView(Value);
    }
  }
}
