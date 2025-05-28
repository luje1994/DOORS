Imports CLS_STD.STD
'Imports DOORS.STD
'Imports NTSInformatica.CLN__STD

Public Class CLIENTI

  Public ocle As ENTITY

  Public Function Init(oParam As PARAM) As Boolean
    Dim oTmp As Object = Nothing
    Dim strErrore As String = ""

    oPar = oParam

    Dim s As String = "dsdf"
    's = NTSCStr(s)
    'cs("SDSD")

    If Not IstanziaDLL(oPar.strPath, "CLIENTI", "ENTITY", oTmp, strErrore) Then
      MsgBox("Errore per istanziare la DLL: " & strErrore)
      Return False
    End If

    ocle = CType(oTmp, ENTITY)
    ocle.Init(oPar)

    Return True
  End Function
  Private Sub CLIENTI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    'MsgBox(oPar.strAzienda)
    'MsgBox(oPar.strPath)
    'MsgBox(oPar.strFormChiamante)
  End Sub
End Class
