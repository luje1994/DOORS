Public Class TimbratureForm

  Private workDate As Date

  Public Sub New(d As Date)
    InitializeComponent()
    workDate = d
    LabelDate.Text = d.ToLongDateString()
  End Sub

  Private Sub TimbratureForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    LoadTimbrature()
  End Sub

  Private Sub LoadTimbrature()
    ListBoxEntries.Items.Clear()
    Dim entries = DBHelper.GetEntriesByDate(workDate)
    For Each pair In entries
      ListBoxEntries.Items.Add($"{pair.Item1:HH:mm} - {pair.Item2:HH:mm}")
    Next
  End Sub

  Private Sub ButtonAdd_Click(sender As Object, e As EventArgs) Handles ButtonAdd.Click
    Dim entry As DateTime = DateTimePickerEntry.Value
    Dim exitT As DateTime = DateTimePickerExit.Value
    If exitT <= entry Then
      MessageBox.Show("Orario non valido")
      Return
    End If

    DBHelper.AddEntry(workDate, entry, exitT)
    LoadTimbrature()
  End Sub
End Class
