Public Class MainForm
  Private currentDate As Date = Date.Today

  Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    LoadCalendar(currentDate.Year, currentDate.Month)
  End Sub

  Private Sub LoadCalendar(year As Integer, month As Integer)

    CalendarPanel.Controls.Clear()
    CalendarPanel.ColumnCount = 7
    CalendarPanel.RowCount = 6
    CalendarPanel.ColumnStyles.Clear()
    CalendarPanel.RowStyles.Clear()

    For i As Integer = 0 To 6
      CalendarPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100 / 7))
    Next
    For i As Integer = 0 To 5
      CalendarPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100 / 6))
    Next

    Dim firstDayOfMonth As New DateTime(year, month, 1)
    Dim daysInMonth As Integer = DateTime.DaysInMonth(year, month)
    Dim startDayOfWeek As Integer = CInt(firstDayOfMonth.DayOfWeek)
    If startDayOfWeek = 0 Then startDayOfWeek = 7 ' Domenica -> fine settimana

    Dim dayCounter As Integer = 1

    For row As Integer = 0 To 5
      For col As Integer = 0 To 6
        ' Calcolo della cella di inizio mese
        Dim cellIndex As Integer = row * 7 + col

        If cellIndex >= startDayOfWeek - 1 AndAlso dayCounter <= daysInMonth Then
          Dim btn As New Button()
          btn.Text = dayCounter.ToString()
          btn.Dock = DockStyle.Fill
          btn.Tag = New DateTime(year, month, dayCounter)

          AddHandler btn.Click, AddressOf DayButton_Click
          CalendarPanel.Controls.Add(btn, col, row)

          dayCounter += 1
        Else
          ' Celle vuote all'inizio/fine
          Dim emptyLabel As New Label()
          emptyLabel.Text = ""
          CalendarPanel.Controls.Add(emptyLabel, col, row)
        End If
      Next
    Next
  End Sub

  Private Sub DayButton_Click(sender As Object, e As EventArgs)
    Dim btn As Button = CType(sender, Button)
    Dim day As Date = CType(btn.Tag, Date)

    Dim f As New TimbratureForm(day)
    f.ShowDialog()
    LoadCalendar(day.Year, day.Month)
  End Sub
End Class
