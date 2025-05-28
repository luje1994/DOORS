Public Class WING
  Private Sub WING_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
    '*************************************************************
    'carico l'oggetto MENU e lo avvio passandogli la riga di comando
    Try
      Dim assem As System.Reflection.Assembly
      Dim tpChild As Type
      Dim frmMenu As Object

      assem = System.Reflection.Assembly.LoadFrom("C:\PROGETTI-DOORS\WING\Bin\MENU.DLL")
      tpChild = assem.GetType("CLS_STD.MENU", True, True)
      frmMenu = Activator.CreateInstance(tpChild)

      If Not frmMenu.InitMenu("", "C:\PROGETTI-DOORS\WING\Bin\", Me, "") Then
        Me.Close()
        Return
      End If

      Me.BringToFront()

      'visualizzo il menu (non modale)
      frmMenu.show()
      frmMenu.BringToFront

    Catch ex As Exception
      'If bBatch = False Then
      '  MsgBox("Error starting Bus.net: " & ex.Message, MsgBoxStyle.Critical, "Messaggio di Business CUBE")
      'Else
      '  WriteMsgBoxToLog("Error starting Bus.net: " & ex.ToString, True)
      'End If
      Me.Close()
      Return
    End Try
  End Sub
End Class
