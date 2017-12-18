using System;
using System.Windows.Controls;
using System.Windows.Media;
using Wordki.Helpers;

namespace Controls.Calendar.CalendarState {
  public class YearState : CalendarState {

    public YearState(Calendar pCalendar)
      : base(pCalendar) {
      Type = CalendarStateType.Year;
      ColumnCount = 4;
      RowCount = 3;
    }

    protected override void InitGridContent() {
      Calendar.GridMain.Children.Clear();
      int beginYear = GetBeginYear();
      for (int j = 0; j < RowCount; j++) {
        for (int i = 0; i < ColumnCount; i++) {
          Button year = new Button();
          year.Foreground = Brushes.Black;
          year.Style = Calendar.CurrentNumbersStyle;
          year.Background = Calendar.SelectedDate.Year == beginYear ? Brushes.DarkGray : Brushes.DimGray;
          year.Content = beginYear;
          year.Command = new BuilderCommand(YearClick);
          year.CommandParameter = beginYear++;
          Grid.SetColumn(year, i);
          Grid.SetRow(year, j);
          Calendar.GridMain.Children.Add(year);
        }
      }
    }

    protected override void InitTypeLabel() {
      Calendar.ButtonCenter.Content = String.Format("{0} - {1}", GetBeginYear(), GetBeginYear() + 11);
    }

    protected override void NextButton(object obj) {
      Calendar.SelectedDate = new DateTime(Calendar.SelectedDate.Year, Calendar.SelectedDate.Month, 1).AddYears(10);
    }

    protected override void PreviousButton(object obj) {
      Calendar.SelectedDate = new DateTime(Calendar.SelectedDate.Year, Calendar.SelectedDate.Month, 1).AddYears(-10);
    }

    private void YearClick(object obj) {
      Calendar.SelectedDate = new DateTime((int)obj, Calendar.SelectedDate.Month, Calendar.SelectedDate.Day);
      Calendar.State = Calendar.StateFactory.GetState(Calendar, CalendarStateType.Month);
    }

    private int GetBeginYear() {
      return Calendar.SelectedDate.Year - Calendar.SelectedDate.Year % 10 - 1;
    }
  }
}
