using System.Collections.Generic;

namespace Controls.Calendar.CalendarState {
  public class CalendarStateFactory {

    private Dictionary<CalendarStateType, CalendarState> States = new Dictionary<CalendarStateType, CalendarState>();

    public CalendarState GetState(Calendar pCalendar, CalendarStateType pType) {
      CalendarState state = null;
      if (States.ContainsKey(pType)) {
        state = States[pType];
        state.Init();
        return state;
      }
      switch (pType) {
        case CalendarStateType.Day:
          state = new DayState(pCalendar);
          break;
        case CalendarStateType.Month:
          state = new MonthState(pCalendar);
          break;
        case CalendarStateType.Year:
          state = new YearState(pCalendar);
          break;
      }
      if (state != null)
        state.Init();
      States.Add(pType, state);
      return state;
    }

    public CalendarState GetState(Calendar pCalendar) {
      return GetState(pCalendar, GetNextState(pCalendar));
    }

    public CalendarStateType GetNextState(Calendar pCalendar) {
      switch (pCalendar.State.Type) {
        case CalendarStateType.Day:
          return CalendarStateType.Month;
        case CalendarStateType.Month:
          return CalendarStateType.Year;
        case CalendarStateType.Year:
          return CalendarStateType.Day;
      }
      return CalendarStateType.Day;
    }
  }
}
