using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Wordki.Helpers;

namespace Controls.Calendar.CalendarState {
  public class DayState : CalendarState {

    private List<CalendarModel> DayLables;

    public DayState(Calendar pCalendar)
      : base(pCalendar) {
      Type = CalendarStateType.Day;
      ColumnCount = 7;
      RowCount = 7;
      DayLables = new List<CalendarModel>();
    }

    protected override void InitGridContent() {
      Calendar.GridMain.Children.Clear();
      Calendar.GridMain.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Auto);
      for (int i = 0; i < ColumnCount; i++) {
        Label label = new Label();
        label.Content = GetDayName(i);
        label.Style = Calendar.DaysStyle;
        Grid.SetColumn(label, i);
        Grid.SetRow(label, 0);
        Calendar.GridMain.Children.Add(label);
      }
      PrepareDayLables();
      for (int j = 1; j < RowCount; j++) {
        for (int i = 0; i < ColumnCount; i++) {
          CalendarModel dateModel = DayLables[(j - 1) * RowCount + i];
          Button number = new Button();
          number.Content = dateModel.Day;
          number.Style = dateModel.ActualMonth == 0 ? Calendar.CurrentNumbersStyle : Calendar.OtherNumbersStyle;
          number.Command = new BuilderCommand(DayClick);
          number.CommandParameter = dateModel;
          Grid.SetColumn(number, i);
          Grid.SetRow(number, j);
          Calendar.GridMain.Children.Add(number);
        }
      }
    }

    protected override void InitTypeLabel() {
      Calendar.ButtonCenter.Content = String.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Calendar.SelectedDate.Month), Calendar.SelectedDate.Year);
    }

    protected override void NextButton(object obj) {
      Calendar.SelectedDate = new DateTime(Calendar.SelectedDate.Year, Calendar.SelectedDate.Month, 1).AddMonths(1);
    }

    protected override void PreviousButton(object obj) {
      Calendar.SelectedDate = new DateTime(Calendar.SelectedDate.Year, Calendar.SelectedDate.Month, 1).AddMonths(-1);
    }

    private void DayClick(object obj) {
      CalendarModel day = obj as CalendarModel;
      if (day == null) {
        return;
      }
      Calendar.SelectedDate = new DateTime(day.Year, day.Month, day.Day);
    }

    private void PrepareDayLables() {
      DateTime selectedDateTime = Calendar.SelectedDate;
      DayLables.Clear();
      int yearBefore = selectedDateTime.Year;
      int monthBefore = selectedDateTime.Month - 1;
      if (selectedDateTime.Month == 1) {
        yearBefore--;
        monthBefore = 12;
      }
      int daysInMonthBefore = DateTime.DaysInMonth(yearBefore, monthBefore);
      int lDayBeginningSelectedMonth = (int)(new DateTime(selectedDateTime.Year, selectedDateTime.Month, 1)).DayOfWeek;
      lDayBeginningSelectedMonth -= (Calendar.SundayFirst ? 0 : 1);
      if(lDayBeginningSelectedMonth < 0) {
        lDayBeginningSelectedMonth = 6;
      } else if (lDayBeginningSelectedMonth == 0) {
        lDayBeginningSelectedMonth = 7;
      }

      for (int i = 1; i <= lDayBeginningSelectedMonth; i++) {
        DayLables.Insert(0, new CalendarModel {
          ActualMonth = -1,
          Checked = false,
          Day = daysInMonthBefore--,
          Month = monthBefore,
          Year = yearBefore,
        });
      }

      //days in current month
      int lDaysInSelectedMonth = DateTime.DaysInMonth(selectedDateTime.Year, selectedDateTime.Month);
      for (int i = 1; i <= lDaysInSelectedMonth; i++) {
        DayLables.Add(new CalendarModel {
          ActualMonth = 0,
          Checked = false,
          Day = i,
          Month = selectedDateTime.Month,
          Year = selectedDateTime.Year,
        });
      }

      //days in next month
      int lYearAfter = selectedDateTime.Year;
      int lMonthAfter = selectedDateTime.Month + 1;
      if (selectedDateTime.Month == 12) {
        lYearAfter++;
        lMonthAfter = 1;
      }

      for (int i = 1; DayLables.Count < ColumnCount * (RowCount - 1); i++) {
        DayLables.Add(new CalendarModel {
          ActualMonth = 1,
          Checked = false,
          Day = i,
          Month = lMonthAfter,
          Year = lYearAfter,
        });
      }
    }

    private string GetDayName(int i) {
      if (!Calendar.SundayFirst) i++;
      if (i > CultureInfo.CurrentCulture.DateTimeFormat.ShortestDayNames.Length - 1) i = 0;
      return CultureInfo.CurrentCulture.DateTimeFormat.ShortestDayNames[i];
    }




    
  }
}
