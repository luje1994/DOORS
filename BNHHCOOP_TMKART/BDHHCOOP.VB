Imports NTSInformatica.CLN__STD
Imports System.Data.Common
Imports NTSInformatica
Imports System.IO

Public Class CLDHHCOOP
  Inherits CLD__BASE

  Public Overridable Function SalvaOPC(strDitta As String,
                                       strMacchina As String,
                                       lCommessa As Long,
                                       strCodArt As String,
                                       nQtaDaFare As Integer,
                                       lOrdine As Long,
                                       dData As DateTime,
                                       strStato As String) As Boolean

    Dim bReturn As Boolean = False
    Dim strSQL As String = ""
    Dim lResult As Integer = 0

    Try

      If recordExist("tabhhmactesta", "codditt = " & CStrSQL(strDitta), "tb_macchina = " & CStrSQL(strMacchina), "tb_commessa = " & lCommessa, "tb_codart = " & CStrSQL(strCodArt)) Then
        'strSQL = "UPDATE tabhhmactesta SET tb_quant = " & nQtaFatta & ", tb_quapre = " & nQtaDaFare & ", tb_tempo = " & lTempo & ", tb_numero = " & lOrdine &
        '          " WHERE codditt = " & CStrSQL(strDitta) &
        '          " AND tb_macchina = " & CStrSQL(strMacchina) &
        '          " AND tb_commessa = " & lCommessa &
        '          " AND tb_codart = " & CStrSQL(strCodArt)
        strSQL = "UPDATE tabhhmactesta SET tb_quapre = " & nQtaDaFare & ",tb_numero = " & lOrdine & ", " &
                  "tb_datart = " & CDataOraSQL(dData) & ", tb_stato = " & CStrSQL(strStato) &
                  " WHERE codditt = " & CStrSQL(strDitta) &
                  " AND tb_macchina = " & CStrSQL(strMacchina) &
                  " AND tb_commessa = " & lCommessa &
                  " AND tb_codart = " & CStrSQL(strCodArt)
      Else
        'strSQL = "INSERT INTO tabhhmactesta (codditt, tb_macchina, tb_ordine, tb_commessa, tb_codart, tb_quant, tb_quapre, tb_tempo, tb_numero) " &
        '          " VALUES ( " & CStrSQL(strDitta) & ", " & CStrSQL(strMacchina) & ", '', " & lCommessa & ", " & CStrSQL(strCodArt) & ", " &
        '                         nQtaFatta & ", " & nQtaDaFare & ", " & lTempo & ", " & lOrdine & " )"
        strSQL = "INSERT INTO tabhhmactesta (codditt, tb_macchina, tb_commessa, tb_codart, tb_quant, tb_quapre, tb_tempo, tb_numero, tb_datart, tb_stato) " &
                  " VALUES ( " & CStrSQL(strDitta) & ", " & CStrSQL(strMacchina) & ", " & lCommessa & ", " & CStrSQL(strCodArt) & ", " &
                                 "0, " & nQtaDaFare & ", 0, " & lOrdine & ", " & CDataOraSQL(dData) & ", " & CStrSQL(strStato) & " )"
      End If

      lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      If lResult < 0 Then Return False

      bReturn = True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return bReturn
  End Function

  Public Overridable Function AggiornaQta(nID As Integer, nOrdine As Long) As Boolean
    Dim bReturn As Boolean = False
    Dim strSQL As String = ""
    Dim lResult As Integer = 0

    Try

      strSQL = "UPDATE tabhhout SET tb_quant += 1 " &
                " WHERE tb_id = " & NTSCInt(nID) &
                " AND tb_ordine = " & NTSCLng(nOrdine)
      lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      If lResult < 0 Then Return False

      Return bReturn
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
    Return bReturn
  End Function

  Public Overridable Function InsertOPC(strDitta As String,
                                        strMacchina As String,
                                        lCommessa As Long,
                                        strCodArt As String) As Boolean

    Dim bReturn As Boolean = False
    Dim strSQL As String = ""
    Dim lResult As Integer = 0

    Try

      If Not recordExist("tabhhmactesta", "codditt = " & CStrSQL(strDitta), "tb_macchina = " & CStrSQL(strMacchina), "tb_commessa = " & lCommessa, "tb_codart = " & CStrSQL(strCodArt)) Then
        strSQL = "INSERT INTO tabhhmactesta (codditt, tb_macchina,  tb_commessa, tb_codart, tb_quant, tb_quapre, tb_tempo, tb_numero) " &
                  " VALUES ( " & CStrSQL(strDitta) & ", " & CStrSQL(strMacchina) & ", " & lCommessa & ", " & CStrSQL(strCodArt) & ",0,0,0,0 )"

        Dim dsComme As New DataSet
        If ApriComme(strDitta, strMacchina, lCommessa, strCodArt, dsComme) Then
          For Each drComme As DataRow In dsComme.Tables("COMME").Rows
            strSQL &= "INSERT INTO tabhhmaccorpo (codditt, tb_macchina,  tb_commessa, tb_codart, tb_ordine, tb_tipork, tb_anno, tb_serie, tb_numord, tb_riga, tb_fase, tb_quant, tb_data, tb_flevas) " &
                  " VALUES ( " & CStrSQL(strDitta) & ", " & CStrSQL(strMacchina) & ", " & lCommessa & ", " & CStrSQL(drComme!tc_codart) & ", " & CStrSQL(drComme!tc_ordine) & ", " &
                  CStrSQL(drComme!tc_tipork) & ", " & NTSCInt(drComme!tc_anno) & ", " & CStrSQL(drComme!tc_serie) & ", " & NTSCInt(drComme!tc_numord) & ", " &
                  NTSCInt(drComme!tc_riga) & ", " & NTSCInt(drComme!tc_fase) & ",0, NULL, 'N')"

          Next
        End If

        lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
        If lResult < 0 Then Return False
      End If

      bReturn = True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return bReturn
  End Function

  Public Overridable Function ApriComme(strDitta As String,
                                        strMacchina As String,
                                        lCommessa As Long,
                                        strCodart As String,
                                        ByRef dsOut As DataSet) As Boolean

    Dim bReturn As Boolean = False
    Dim strSQL As String = ""

    Try
      strSQL = "SELECT tabhhcomme.* " &
                " FROM tabhhcomme " &
                  " WHERE tabhhcomme.codditt = " & CStrSQL(strDitta) &
                  " AND tc_macchina = " & CStrSQL(strMacchina) &
                  " AND instId = " & lCommessa

      If Trim(strCodart) <> "" Then
        strSQL &= " AND tc_codart = " & CStrSQL(strCodart)
      End If

      dsOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "COMME")

      bReturn = True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return bReturn
  End Function

  Public Overridable Function ApriCorpo(strDitta As String,
                                        strMacchina As String,
                                        lCommessa As Long,
                                        strCodart As String,
                                        ByRef dsOut As DataSet) As Boolean

    Dim bReturn As Boolean = False
    Dim strSQL As String = ""

    Try
      strSQL = "SELECT tabhhmaccorpo.*, (tabhhcommemac.tc_quant) AS xx_quares " &
                " FROM tabhhmaccorpo " &
                    " LEFT OUTER JOIN movord ON " &
                      " tabhhmaccorpo.codditt = movord.codditt AND " &
                      " tabhhmaccorpo.tb_tipork = movord.mo_tipork AND " &
                      " tabhhmaccorpo.tb_anno = movord.mo_anno AND " &
                      " tabhhmaccorpo.tb_serie = movord.mo_serie AND " &
                      " tabhhmaccorpo.tb_numord = movord.mo_numord AND " &
                      " tabhhmaccorpo.tb_riga = movord.mo_riga " &
                    " LEFT OUTER JOIN tabhhcomme ON " &
                      " tabhhmaccorpo.codditt = tabhhcomme.codditt AND " &
                      " tabhhmaccorpo.tb_macchina = tabhhcomme.tc_macchina AND " &
                      " tabhhmaccorpo.tb_commessa = tabhhcomme.instId AND " &
                      " tabhhmaccorpo.tb_ordine = tabhhcomme.tc_ordine " &
                  " WHERE tabhhmaccorpo.codditt = " & CStrSQL(strDitta) &
                  " AND tb_macchina = " & CStrSQL(strMacchina) &
                  " AND tb_commessa = " & lCommessa &
                  " AND tb_codart = " & CStrSQL(strCodart) &
                  " ORDER BY movord.mo_datcons "

      dsOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "CORPO")

      bReturn = True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return bReturn
  End Function

  Public Overridable Function UpadteOPC(strDitta As String,
                                        strMacchina As String,
                                        lCommessa As Long,
                                        strCodArt As String,
                                        strCampo As String,
                                        strOperatore As String,
                                        strValore As String) As Boolean

    Dim bReturn As Boolean = False
    Dim strSQL As String = ""
    Dim lResult As Integer = 0

    Try
      strSQL = "UPDATE tabhhmactesta SET " & strCampo & " " & strOperatore & " " & strValore &
                  " WHERE codditt = " & CStrSQL(strDitta) &
                  " AND tb_macchina = " & CStrSQL(strMacchina) &
                  " AND tb_commessa = " & lCommessa &
                  " AND tb_codart = " & CStrSQL(strCodArt)

      lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      If lResult < 0 Then Return False

      bReturn = True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return bReturn
  End Function

  Public Overridable Function UpadteOPCCorpo(strDitta As String,
                                             strMacchina As String,
                                             lCommessa As Long,
                                             strCodArt As String,
                                             nQta As Integer,
                                             dData As Date) As Boolean

    Dim bReturn As Boolean = False
    Dim strSQL As String = ""
    Dim lResult As Integer = 0
    Dim dsCorpo As New DataSet
    Dim strOrdine As String = ""

    Try

      strSQL = "UPDATE tabhhmaccorpo SET tb_quant = " & nQta & ", tb_data = " & CDataOraSQL(dData) &
                  " WHERE codditt = " & CStrSQL(strDitta) &
                  " AND tb_macchina = " & CStrSQL(strMacchina) &
                  " AND tb_commessa = " & lCommessa &
                  " AND tb_codart = " & CStrSQL(strCodArt)

      lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      If lResult < 0 Then Return False

      'Dim nQuaTot As Integer = NTSCInt(GetOPC(strDitta, strMacchina, lCommessa, strCodArt, "tb_quant"))

      'Dim nQuaProd As Integer = nQta - nQuaTot

      'If ApriCorpo(strDitta, strMacchina, lCommessa, strCodArt, dsCorpo) Then

      '  For Each drCorpo As DataRow In dsCorpo.Tables("CORPO").Rows

      '    strOrdine = NTSCStr(drCorpo!tb_ordine)
      '    If nQuaProd = 0 Then Return True

      '    If NTSCStr(drCorpo!tb_flevas) <> "S" Then
      '      Dim nQuaRes As Integer = NTSCInt(drCorpo!xx_quares) - NTSCInt(drCorpo!tb_quant)
      '      If nQuaRes = 0 Then Continue For

      '      If nQuaProd <= nQuaRes Then
      '        strSQL = "UPDATE tabhhmaccorpo SET tb_quant += " & nQuaProd & ", tb_data = " & CDataOraSQL(dData) &
      '            " WHERE codditt = " & CStrSQL(strDitta) &
      '            " AND tb_macchina = " & CStrSQL(strMacchina) &
      '            " AND tb_commessa = " & lCommessa &
      '            " AND tb_codart = " & CStrSQL(strCodArt) &
      '            " AND tb_ordine = " & CStrSQL(drCorpo!tb_ordine)

      '        lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      '        If lResult < 0 Then Return False

      '        nQuaProd = 0
      '      Else

      '        strSQL = "UPDATE tabhhmaccorpo SET tb_quant += " & nQuaRes & ", tb_data = " & CDataOraSQL(dData) &
      '            " WHERE codditt = " & CStrSQL(strDitta) &
      '            " AND tb_macchina = " & CStrSQL(strMacchina) &
      '            " AND tb_commessa = " & lCommessa &
      '            " AND tb_codart = " & CStrSQL(strCodArt) &
      '            " AND tb_ordine = " & CStrSQL(drCorpo!tb_ordine)

      '        lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      '        If lResult < 0 Then Return False

      '        nQuaProd -= nQuaRes

      '      End If

      '    End If

      '  Next

      '  If nQuaProd > 0 Then
      '    strSQL = "UPDATE tabhhmaccorpo SET tb_quant += " & nQuaProd & ", tb_data = " & CDataOraSQL(dData) & ", " &
      '                                     " tb_flevas = CASE WHEN tb_flevas = 'S' THEN 'C' ELSE tb_flevas END " &
      '            " WHERE codditt = " & CStrSQL(strDitta) &
      '            " AND tb_macchina = " & CStrSQL(strMacchina) &
      '            " AND tb_commessa = " & lCommessa &
      '            " AND tb_codart = " & CStrSQL(strCodArt) &
      '            " AND tb_ordine = " & CStrSQL(strOrdine)

      '    lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      '    If lResult < 0 Then Return False
      '  End If

      'End If

      bReturn = True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return bReturn
  End Function

  Public Overridable Function UpadteQtaDaComme(strDitta As String,
                                               strMacchina As String,
                                               lCommessa As Long) As Boolean

    Dim bReturn As Boolean = False
    Dim strSQL As String = ""
    Dim lResult As Integer = 0
    Dim nQuant As Integer = 0

    Try

      Dim dsComme As New DataSet
      If ApriComme(strDitta, strMacchina, lCommessa, "", dsComme) Then
        For Each drComme As DataRow In dsComme.Tables("COMME").Rows
          nQuant += NTSCInt(drComme!tc_quant)
          strSQL = "UPDATE tabhhmaccorpo SET tb_quant = " & NTSCInt(drComme!tc_quant) & " FROM tabhhmaccorpo " &
                  " WHERE codditt = " & CStrSQL(strDitta) &
                  " AND tb_macchina = " & CStrSQL(strMacchina) &
                  " AND tb_commessa = " & lCommessa &
                  " AND tb_codart = " & CStrSQL(drComme!tc_codart) &
                  " AND tb_ordine = " & CStrSQL(drComme!tc_ordine)

          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False
        Next
      End If


      strSQL = "UPDATE tabhhmactesta SET tb_quant = " & NTSCStr(nQuant) & " FROM tabhhmactesta " &
                  " WHERE codditt = " & CStrSQL(strDitta) &
                  " AND tb_macchina = " & CStrSQL(strMacchina) &
                  " AND tb_commessa = " & lCommessa &
                  " AND tb_codart = '' "

      lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      If lResult < 0 Then Return False

      bReturn = True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return bReturn
  End Function

  Public Overridable Function GetOPC(strDitta As String,
                                     strMacchina As String,
                                     lCommessa As Long,
                                     strCodArt As String,
                                     strCampo As String) As String

    Dim sReturn As String = ""
    Dim strSQL As String = ""

    Try

      strSQL = "SELECT TOP 1 " & strCampo & " FROM tabhhmactesta " &
                " WHERE codditt = " & CStrSQL(strDitta) &
                " AND tb_macchina = " & CStrSQL(strMacchina) &
                " AND tb_commessa = " & lCommessa &
                " AND tb_codart = " & CStrSQL(strCodArt)

      Dim dttTmp As New DataTable
      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dttTmp.Rows.Count > 0 Then
        sReturn = NTSCStr(dttTmp.Rows(0)(strCampo))
      End If

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return sReturn
  End Function

  'Public Overridable Function UpadteQta(strDitta As String,
  '                                     strMacchina As String,
  '                                     lCommessa As Long,
  '                                     strCodArt As String,
  '                                     nQtaFatta As Integer) As Boolean

  '  Dim bReturn As Boolean = False
  '  Dim strSQL As String = ""
  '  Dim lResult As Integer = 0

  '  Try

  '    If recordExist("tabhhmactesta", "codditt = " & CStrSQL(strDitta), "tb_macchina = " & CStrSQL(strMacchina), "tb_commessa = " & lCommessa, "tb_codart = " & CStrSQL(strCodArt)) Then
  '      strSQL = "UPDATE tabhhmac SET tb_quant += " & nQtaFatta &
  '                " WHERE codditt = " & CStrSQL(strDitta) &
  '                " AND tb_macchina = " & CStrSQL(strMacchina) &
  '                " AND tb_commessa = " & lCommessa &
  '                " AND tb_codart = " & CStrSQL(strCodArt)

  '      lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
  '      If lResult < 0 Then Return False

  '    End If

  '    bReturn = True
  '  Catch ex As Exception
  '    CLN__STD.GestErr(ex, Me, "")
  '  End Try

  '  Return bReturn
  'End Function

  Public Overridable Function recordExist(ByVal tabella As String, ByVal ParamArray CampiChiave() As String) As Boolean
    Dim strSQL As String = ""
    Dim strWHERE As String = ""
    Dim dt As New DataTable
    Try

      If Not CampiChiave Is Nothing Then
        For Each str As String In CampiChiave
          If strWHERE = "" Then
            strWHERE &= " WHERE "
          Else
            strWHERE &= " AND "
          End If
          strWHERE &= str
        Next
      End If

      'If strWHERE = "" Then Return False

      strSQL = " SELECT * FROM " & tabella & strWHERE

      dt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dt.Rows.Count > 0 Then
        Return True
      End If

      Return False
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return False
    End Try
  End Function

  Public Overridable Function GetArticolo(strDitta As String, strMacchina As String, lCommessa As Long) As String
    Dim sReturn As String = ""
    Dim strSQL As String = ""
    Dim dttTmp As New DataTable

    Try

      strSQL = "SELECT TOP 1 * FROM tabhhcomme " &
                " WHERE codditt = " & CStrSQL(strDitta) &
                " AND tc_macchina = " & CStrSQL(strMacchina) &
                " AND instId = " & lCommessa

      dttTmp = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dttTmp.Rows.Count > 0 Then
        sReturn = NTSCStr(dttTmp.Rows(0) !tc_codart)
      End If

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return sReturn
  End Function

End Class
