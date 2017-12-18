using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;
using Wordki.Helpers;

namespace Controls.Calendar.CalendarState {
  public class MonthState : CalendarState {
    public MonthState(Calendar pCalendar)
      : base(pCalendar) {
      Type = CalendarStateType.Month;
      ColumnCount = 4;
      RowCount = 3;
    }


    protected override void InitGridContent() {
      Calendar.GridMain.Children.Clear();
      int countr = 1;
      for (int j = 0; j < RowCount; j++) {
        for (int i = 0; i < ColumnCount; i++) {
          Button month = new Button();
          month.Foreground = Brushes.Black;
          month.Style = Calendar.CurrentNumbersStyle;
          month.Background = Calendar.SelectedDate.Month == countr ? Brushes.DarkGray : Brushes.DimGray;
          month.Content = new DateTime(2000, countr++, 1).ToString("MMM", CultureInfo.CurrentCulture);
          month.Command = new BuilderCommand(MonthClick);
          month.CommandParameter = j * RowCount + i + 1;
          Grid.SetColumn(month, i);
          Grid.SetRow(month, j);
          Calendar.GridMain.Children.Add(month);

        }
      }
    }

    protected override void InitTypeLabel() {
      Calendar.ButtonCenter.Content = Calendar.SelectedDate.Year;
    }

    protected override void NextButton(object obj) {
      Calendar.SelectedDate = new DateTime(Calendar.SelectedDate.Year, Calendar.SelectedDate.Month, 1).AddYears(1);
    }

    protected override void PreviousButton(object obj) {
      Calendar.SelectedDate = new DateTime(Calendar.SelectedDate.Year, Calendar.SelectedDate.Month, 1).AddYears(-1);
    }

    private void MonthClick(object obj) {
      Calendar.SelectedDate = new DateTime(Calendar.SelectedDate.Year, (int)obj, Calendar.SelectedDate.Day);
      Calendar.State = Calendar.StateFactory.GetState(Calendar, CalendarStateType.Day);
    }

    
  }
}
