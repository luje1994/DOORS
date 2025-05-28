
Imports System.Data.SqlClient

Public Class DBHelper
  Private Shared connectionString As String = "Server=PC-ANDOLFI;Database=CalendarioLAV;UID=sa;pwd=sqladmin;LANGUAGE=us_english;APP=Business;Encrypt=True;TrustServerCertificate=True;"

  Public Shared Function GetEntriesByDate(workDate As Date) As List(Of Tuple(Of DateTime, DateTime))
        Dim entries As New List(Of Tuple(Of DateTime, DateTime))()
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT EntryTime, ExitTime FROM WorkEntries WHERE WorkDate = @Date", conn)
            cmd.Parameters.AddWithValue("@Date", workDate.Date)
            Dim reader = cmd.ExecuteReader()
            While reader.Read()
                entries.Add(Tuple.Create(reader.GetDateTime(0), reader.GetDateTime(1)))
            End While
        End Using
        Return entries
    End Function

    Public Shared Sub AddEntry(workDate As Date, entry As DateTime, exitT As DateTime)
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim cmd As New SqlCommand("INSERT INTO WorkEntries (WorkDate, EntryTime, ExitTime) VALUES (@Date, @Entry, @Exit)", conn)
            cmd.Parameters.AddWithValue("@Date", workDate.Date)
            cmd.Parameters.AddWithValue("@Entry", entry)
            cmd.Parameters.AddWithValue("@Exit", exitT)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Shared Function GetTotalHoursForDate(workDate As Date) As Double
        Dim total As Double = 0
        Dim entries = GetEntriesByDate(workDate)
        For Each pair In entries
            total += pair.Item2.Subtract(pair.Item1).TotalHours
        Next
        Return total
    End Function
End Class
