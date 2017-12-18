using System;
using System.Windows;
using System.Windows.Controls;
using Controls.Calendar.CalendarState;

namespace Controls.Calendar {
  /// <summary>
  /// Interaction logic for Calendar.xaml
  /// </summary>
  public partial class Calendar : UserControl {

    #region Dependencies

    public static readonly DependencyProperty SundayFirstProperty = DependencyProperty.Register(
      "SundayFirst", typeof(bool), typeof(Calendar), new UIPropertyMetadata(true, SandayFirstChangedCallback));

    private static void SandayFirstChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
    }

    public bool SundayFirst {
      get { return (bool)GetValue(SundayFirstProperty); }
      set { SetValue(SundayFirstProperty, value); }
    }

    public static readonly DependencyProperty DaysStyleProperty = DependencyProperty.Register(
      "DaysStyle", typeof(Style), typeof(Calendar), new UIPropertyMetadata(default(Style)));

    public Style DaysStyle {
      get { return GetValue(DaysStyleProperty) as Style; }
      set { SetValue(DaysStyleProperty, value); }
    }

    public static readonly DependencyProperty CurrentNumbersStyleProperty = DependencyProperty.Register(
      "CurrentNumbersStyle", typeof(Style), typeof(Calendar), new UIPropertyMetadata(default(Style)));

    public Style CurrentNumbersStyle {
      get { return GetValue(CurrentNumbersStyleProperty) as Style; }
      set { SetValue(CurrentNumbersStyleProperty, value); }
    }

    public static readonly DependencyProperty OtherNumbersStyleProperty = DependencyProperty.Register(
      "OtherNumbersStyle", typeof(Style), typeof(Calendar), new UIPropertyMetadata(default(Style)));

    public Style OtherNumbersStyle {
      get { return GetValue(OtherNumbersStyleProperty) as Style; }
      set { SetValue(OtherNumbersStyleProperty, value); }
    }

    public static readonly DependencyProperty TitleButtonStyleProperty = DependencyProperty.Register(
      "TitleButtonStyle", typeof(Style), typeof(Calendar), new UIPropertyMetadata(default(Style)));

    public Style TitleButtonStyle {
      get { return GetValue(TitleButtonStyleProperty) as Style; }
      set { SetValue(TitleButtonStyleProperty, value); }
    }

    public static readonly DependencyProperty LeftButtonStyleProperty = DependencyProperty.Register(
      "LeftButtonStyle", typeof(Style), typeof(Calendar), new UIPropertyMetadata(default(Style)));

    public Style LeftButtonStyle {
      get { return GetValue(LeftButtonStyleProperty) as Style; }
      set { SetValue(LeftButtonStyleProperty, value); }
    }

    public static readonly DependencyProperty RightButtonStyleProperty = DependencyProperty.Register(
      "RightButtonStyle", typeof(Style), typeof(Calendar), new UIPropertyMetadata(default(Style)));

    public Style RightButtonStyle {
      get { return GetValue(RightButtonStyleProperty) as Style; }
      set { SetValue(RightButtonStyleProperty, value); }
    }

    public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(
      "SelectedDate", typeof(DateTime), typeof(Calendar), new UIPropertyMetadata(DateTime.Now, SelectedDateChangedCallback));

    private static void SelectedDateChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
      
    }

    public DateTime SelectedDate {
      get { return (DateTime)GetValue(SelectedDateProperty); }
      set {
        if (SelectedDate.Day == value.Day && SelectedDate.Month == value.Month && SelectedDate.Year == value.Year) {
          return;
        }
        SetValue(SelectedDateProperty, value);
        if (SelectedDateChanged != null) {
          SelectedDateChanged.Invoke(this, new EventArgs());
        }
        State = StateFactory.GetState(this, State.Type);
        Console.WriteLine(value);
      }
    }

    #endregion

    public CalendarState.CalendarState State { get; set; }
    public EventHandler SelectedDateChanged;
    public CalendarStateFactory StateFactory = new CalendarStateFactory();
    

    public Calendar() {
      InitializeComponent();
    }

    public override void EndInit() {
      base.EndInit();
      State = StateFactory.GetState(this, CalendarStateType.Day);
    }

    public override void BeginInit() {
      DaysStyle = Resources["DaysStyle"] as Style;
      CurrentNumbersStyle = Resources["CurrentNumbersStyle"] as Style;
      OtherNumbersStyle = Resources["OtherNumberStyle"] as Style;
      base.BeginInit();
    }

    private void ButtonType_Click(object sender, RoutedEventArgs e) {
      //State = StateFactory.GetState(this);
    }
  }
}
