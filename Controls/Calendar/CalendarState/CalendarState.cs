using System.Windows;
using System.Windows.Controls;
using Wordki.Helpers;

namespace Controls.Calendar.CalendarState {
  public abstract class CalendarState {

    public CalendarStateType Type { get; set; }
    public BuilderCommand NextButtonCommand { get; set; }
    public BuilderCommand PreviousButtonCommand { get; set; }
    public BuilderCommand CenterButtonCommand { get; set; }

    protected Calendar Calendar { get; set; }
    protected int ColumnCount { get; set; }
    protected int RowCount { get; set; }

    protected CalendarState(Calendar pCalendar) {
      Calendar = pCalendar;
    }


    public void Init() {
      InitGrid();
      InitGridContent();
      InitTypeLabel();
      Calendar.ButtonNext.Command = new BuilderCommand(NextButton);
      Calendar.ButtonPrevious.Command = new BuilderCommand(PreviousButton);
    }

    protected abstract void InitGridContent();
    protected abstract void InitTypeLabel();
    protected abstract void NextButton(object obj);
    protected abstract void PreviousButton(object obj);

    private void InitGrid() {
      GridLength size = new GridLength(1, GridUnitType.Star);
      Calendar.GridMain.ColumnDefinitions.Clear();
      for (int i = 0; i < ColumnCount; i++) {
        Calendar.GridMain.ColumnDefinitions.Add(new ColumnDefinition() { Width = size });
      }
      Calendar.GridMain.RowDefinitions.Clear();
      for (int i = 0; i < RowCount; i++) {
        Calendar.GridMain.RowDefinitions.Add(new RowDefinition() { Height = size });
      }
    }

  }
}
