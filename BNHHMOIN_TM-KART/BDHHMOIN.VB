Imports NTSInformatica.CLN__STD
Imports System.Data.Common
Imports NTSInformatica
Imports System.IO

Public Class CLDHHMOIN
  Inherits CLD__BASE

  Public Overridable Function LeggiTesta(ByVal strDitta As String, ByVal strSel As String, ByVal dtOpz As DataTable, ByRef dsOut As DataSet) As Boolean

    Dim strSQL As String = ""

    Try
      If dsOut IsNot Nothing Then
        If dsOut.Tables("TESTA") IsNot Nothing Then dsOut.Tables.Remove("TESTA")
      End If

      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT DISTINCT " & CStrSQL(strSel) & " AS xx_sel, tabhhtestaelaborati.*, " &
                " testmag.tm_datdoc, testmag.tm_conto, anagra.an_descr1 AS xx_descrConto " &
                " FROM tabhhtestaelaborati " &
                  " LEFT OUTER JOIN tabhhcorpoelaborati ON " &
                    " tabhhtestaelaborati.codditt = tabhhcorpoelaborati.codditt AND " &
                    " tabhhtestaelaborati.id = tabhhcorpoelaborati.id " &
                  " LEFT OUTER JOIN testmag ON " &
                    " tabhhtestaelaborati.codditt = testmag.codditt AND " &
                    " tabhhtestaelaborati.busTipork = testmag.tm_tipork AND " &
                    " tabhhtestaelaborati.busAnno = testmag.tm_anno AND " &
                    " tabhhtestaelaborati.busSerie = testmag.tm_serie AND " &
                    " tabhhtestaelaborati.busNumero = testmag.tm_numdoc " &
                  " LEFT OUTER JOIN anagra ON " &
                    " testmag.codditt = anagra.codditt AND " &
                    " testmag.tm_conto = anagra.an_conto " &
                " WHERE tabhhtestaelaborati.codditt = " & CStrSQL(strDitta) &
                " AND ISNULL(tabhhtestaelaborati.importato,'') <> 'S' " &
                " AND ISNULL(tabhhtestaelaborati.cancellato,'') <> 'S' " &
                " AND ISNULL(tabhhtestaelaborati.busTipork,'') <> 'W' "

      If Not dtOpz Is Nothing Then
        If NTSCStr(dtOpz.Rows(0) !Tipork) <> "-1" And NTSCStr(dtOpz.Rows(0) !Tipork) <> "" Then
          strSQL &= " AND tabhhtestaelaborati.busTipork = " & CStrSQL(dtOpz.Rows(0) !Tipork)
        End If

        If NTSCInt(dtOpz.Rows(0) !Anno) <> 0 Then
          strSQL &= " AND tabhhtestaelaborati.busAnno = " & NTSCInt(dtOpz.Rows(0) !Anno)
        End If

        If NTSCStr(dtOpz.Rows(0) !Serie) <> "" Then
          strSQL &= " AND tabhhtestaelaborati.busSerie = " & CStrSQL(dtOpz.Rows(0) !Serie)
        End If

        If NTSCInt(dtOpz.Rows(0) !Numero) <> 0 Then
          strSQL &= " AND tabhhtestaelaborati.busNumero = " & NTSCInt(dtOpz.Rows(0) !Numero)
        End If

        If NTSCStr(dtOpz.Rows(0) !DataDaDoc) <> "" Then
          strSQL &= " AND testmag.tm_datdoc >= " & CDataOraSQL(NTSCStr(dtOpz.Rows(0) !DataDaDoc))
        End If

        If NTSCStr(dtOpz.Rows(0) !DataADoc) <> "" Then
          strSQL &= " AND testmag.tm_datdoc <= " & CDataOraSQL(NTSCStr(dtOpz.Rows(0) !DataADoc))
        End If

        If NTSCStr(dtOpz.Rows(0) !DataDaJob) <> "" Then
          strSQL &= " AND CAST(tabhhtestaelaborati.jobDataOra AS DATE) >= " & CDataOraSQL(NTSCStr(dtOpz.Rows(0) !DataDaJob))
        End If

        If NTSCStr(dtOpz.Rows(0) !DataAJob) <> "" Then
          strSQL &= " AND CAST(tabhhtestaelaborati.jobDataOra AS DATE) <= " & CDataOraSQL(NTSCStr(dtOpz.Rows(0) !DataAJob))
        End If

        If NTSCInt(dtOpz.Rows(0) !Conto) <> 0 Then
          strSQL &= " AND testmag.tm_conto = " & NTSCInt(dtOpz.Rows(0) !Conto)
        End If
      End If

      If NTSCStr(dtOpz.Rows(0) !Articolo) <> "" Then
        strSQL &= " AND tabhhcorpoelaborati.articleNumber = " & CStrSQL(dtOpz.Rows(0) !Articolo)
      End If

      strSQL &= " UNION ALL "

      strSQL &= "SELECT DISTINCT " & CStrSQL(strSel) & " AS xx_sel, tabhhtestaelaborati.*, " &
                " testprb.tm_datdoc, testprb.tm_conto, anagra.an_descr1 AS xx_descrConto " &
                " FROM tabhhtestaelaborati " &
                  " LEFT OUTER JOIN tabhhcorpoelaborati ON " &
                    " tabhhtestaelaborati.codditt = tabhhcorpoelaborati.codditt AND " &
                    " tabhhtestaelaborati.id = tabhhcorpoelaborati.id " &
                  " LEFT OUTER JOIN testprb ON " &
                    " tabhhtestaelaborati.codditt = testprb.codditt AND " &
                    " tabhhtestaelaborati.busTipork = testprb.tm_tipork AND " &
                    " tabhhtestaelaborati.busAnno = testprb.tm_anno AND " &
                    " tabhhtestaelaborati.busSerie = testprb.tm_serie AND " &
                    " tabhhtestaelaborati.busNumero = testprb.tm_numdoc " &
                  " LEFT OUTER JOIN anagra ON " &
                    " testprb.codditt = anagra.codditt AND " &
                    " testprb.tm_conto = anagra.an_conto " &
                " WHERE tabhhtestaelaborati.codditt = " & CStrSQL(strDitta) &
                " AND ISNULL(tabhhtestaelaborati.importato,'') <> 'S' " &
                " AND ISNULL(tabhhtestaelaborati.cancellato,'') <> 'S' " &
                " AND ISNULL(tabhhtestaelaborati.busTipork,'') = 'W' "

      If Not dtOpz Is Nothing Then
        If NTSCStr(dtOpz.Rows(0) !Tipork) <> "-1" And NTSCStr(dtOpz.Rows(0) !Tipork) <> "" Then
          strSQL &= " AND tabhhtestaelaborati.busTipork = " & CStrSQL(dtOpz.Rows(0) !Tipork)
        End If

        If NTSCInt(dtOpz.Rows(0) !Anno) <> 0 Then
          strSQL &= " AND tabhhtestaelaborati.busAnno = " & NTSCInt(dtOpz.Rows(0) !Anno)
        End If

        If NTSCStr(dtOpz.Rows(0) !Serie) <> "" Then
          strSQL &= " AND tabhhtestaelaborati.busSerie = " & CStrSQL(dtOpz.Rows(0) !Serie)
        End If

        If NTSCInt(dtOpz.Rows(0) !Numero) <> 0 Then
          strSQL &= " AND tabhhtestaelaborati.busNumero = " & NTSCInt(dtOpz.Rows(0) !Numero)
        End If

        If NTSCStr(dtOpz.Rows(0) !DataDaDoc) <> "" Then
          strSQL &= " AND testprb.tm_datdoc >= " & CDataOraSQL(NTSCStr(dtOpz.Rows(0) !DataDaDoc))
        End If

        If NTSCStr(dtOpz.Rows(0) !DataADoc) <> "" Then
          strSQL &= " AND testprb.tm_datdoc <= " & CDataOraSQL(NTSCStr(dtOpz.Rows(0) !DataADoc))
        End If

        If NTSCStr(dtOpz.Rows(0) !DataDaJob) <> "" Then
          strSQL &= " AND CAST(tabhhtestaelaborati.jobDataOra AS DATE) >= " & CDataOraSQL(NTSCStr(dtOpz.Rows(0) !DataDaJob))
        End If

        If NTSCStr(dtOpz.Rows(0) !DataAJob) <> "" Then
          strSQL &= " AND CAST(tabhhtestaelaborati.jobDataOra AS DATE) <= " & CDataOraSQL(NTSCStr(dtOpz.Rows(0) !DataAJob))
        End If

        If NTSCInt(dtOpz.Rows(0) !Conto) <> 0 Then
          strSQL &= " AND testprb.tm_conto = " & NTSCInt(dtOpz.Rows(0) !Conto)
        End If

        If NTSCStr(dtOpz.Rows(0) !Articolo) <> "" Then
          strSQL &= " AND tabhhcorpoelaborati.articleNumber = " & CStrSQL(dtOpz.Rows(0) !Articolo)
        End If

        If NTSCStr(dtOpz.Rows(0) !Tipork) = "-1" Or NTSCStr(dtOpz.Rows(0) !Tipork) = "1" Then
          strSQL &= "UNION ALL "
          strSQL &= " SELECT " & CStrSQL(strSel) & " AS xx_sel, tabhhmovimenti.codditt, tabhhmovimenti.id, CAST(tabhhmovimenti.id AS VARCHAR(50)) AS jobNumber,  " &
                  " '1' As busTipork, tabhhmovimenti.anno AS busAnno, '' AS busSerie, 0 AS busNumero, 0 AS jobPriority, tabhhmovimenti.status AS jobStatus, " &
                  " tabhhmovimenti.u02SpecialField AS jobDate, tabhhmovimenti.u03SpecialField As jobTime, tabhhmovimenti.jobDataOra, " &
                  " tabhhmovimenti.importato, tabhhmovimenti.cancellato As cancellato, " &
                  " NULL AS tm_datdoc, 0 AS tm_conto, '' AS an_descr1 " &
                  " FROM tabhhmovimenti " &
                  " WHERE tabhhmovimenti.codditt = " & CStrSQL(strDitta) &
                  " AND ISNULL(tabhhmovimenti.importato,'') <> 'S' " &
                  " AND ISNULL(tabhhmovimenti.cancellato,'') <> 'S' " &
                  " AND ISNULL(tabhhmovimenti.operation,'') <> 'm' " &
                  " AND ISNULL(tabhhmovimenti.operation,'') <> 'r' " &
                  " AND ISNULL(tabhhmovimenti.operation,'') <> 'n' " &
                  " AND ISNULL(tabhhmovimenti.operation,'') <> 'f' " &
                  " AND ISNULL(tabhhmovimenti.operation,'') <> 'a' " &
                  " AND ISNULL(tabhhmovimenti.operation,'') <> 'z' " &
                  " AND ISNULL(tabhhmovimenti.operation,'') <> 's' " &
                  " AND ISNULL(tabhhmovimenti.operation,'') <> 'd' "

          '" AND ISNULL(tabhhmovimenti.operation,'') <> 'p' " &
          '" AND ISNULL(tabhhmovimenti.operation,'') <> 'c' " & 

        End If
      End If

      OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "TESTA", dsOut)

      '--------------------------------------------------------------------------------------------------------------
      Return True
      '--------------------------------------------------------------------------------------------------------------
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return False
    End Try
  End Function

  Public Overridable Function LeggiCorpo(ByVal strDitta As String, ByVal nID As Integer, ByVal strTipo As String, ByRef dsOut As DataSet) As Boolean

    Dim strSQL As String = ""

    Try
      If dsOut IsNot Nothing Then
        If dsOut.Tables("CORPO") IsNot Nothing Then dsOut.Tables.Remove("CORPO")
      End If

      If strTipo <> "1" Then
        '--------------------------------------------------------------------------------------------------------------
        strSQL = "Select tabhhcorpoelaborati.*, artico.* " &
                " FROM tabhhcorpoelaborati" &
                  " LEFT OUTER Join artico ON " &
                    " tabhhcorpoelaborati.codditt = artico.codditt AND " &
                    " tabhhcorpoelaborati.articleNumber = artico.ar_codart " &
                " WHERE tabhhcorpoelaborati.codditt = " & CStrSQL(strDitta) &
                " AND tabhhcorpoelaborati.id = " & NTSCInt(nID)
      Else
        '--------------------------------------------------------------------------------------------------------------
        strSQL = "Select 1 AS idriga, tabhhmovimenti.id AS jobNumber, tabhhmovimenti.*, 0 AS nominalQuantity, tabhhmovimenti.status AS positionStatus, artico.* " &
                " FROM tabhhmovimenti" &
                  " LEFT OUTER Join artico ON " &
                    " tabhhmovimenti.codditt = artico.codditt AND " &
                    " tabhhmovimenti.articleNumber = artico.ar_codart " &
                " WHERE tabhhmovimenti.codditt = " & CStrSQL(strDitta) &
                " AND tabhhmovimenti.id = " & NTSCInt(nID)

      End If

      OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "CORPO", dsOut)

      '--------------------------------------------------------------------------------------------------------------
      Return True
      '--------------------------------------------------------------------------------------------------------------
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return False
    End Try
  End Function

  Public Overridable Function CancellaRiga(strDitta As String, nID As Integer, ByVal strTipo As String) As Boolean
    Dim strSQL As String = ""
    Dim lResult As Integer = 0
    Try

      If strTipo <> "1" Then
        strSQL = "UPDATE tabhhtestaelaborati SET cancellato = 'S' " &
                " WHERE codditt = " & CStrSQL(strDitta) &
                " AND id = " & NTSCInt(nID)
      Else
        strSQL = "UPDATE tabhhmovimenti SET cancellato = 'S' " &
                " WHERE codditt = " & CStrSQL(strDitta) &
                " AND id = " & NTSCInt(nID)
      End If
      lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      If lResult < 0 Then Return False

      Return True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Function

  Public Overridable Function SalvaGiacenze(dtGiac As DataTable) As Boolean
    Dim strSQL As String = ""
    Dim lResult As Integer = 0
    Dim bReturn As Boolean = False
    Try

      For Each drGiac As DataRow In dtGiac.Rows
        strSQL = " INSERT INTO tabhhgiacenzeinte " & GetQueryInsertField(dtGiac, "") &
                        " VALUES " & GetQueryInsertValue(dtGiac, drGiac, "")
        lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
        If lResult < 0 Then Return False
      Next

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
    Return False
  End Function

  Public Overridable Function getIDGiac(dData As Date) As Long
    Dim strSQL As String = ""
    Dim dt As New DataTable
    Dim lReturn As Long = 0
    Try
      strSQL = "SELECT MAX(tb_id) AS ID FROM tabhhgiacenzeinte " &
                " WHERE CAST(tb_dataora AS DATE) = " & CDataSQL(dData)
      dt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dt.Rows.Count <> 0 Then
        lReturn = NTSCLng(dt.Rows(0) !ID) + 1
      End If

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return lReturn
  End Function

  Public Overridable Function getDataTableGiacenze() As DataTable
    Dim strSQL As String = ""
    Dim dt As New DataTable
    Try
      strSQL = "SELECT TOP 1 * FROM tabhhgiacenzeinte "
      dt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
      dt.Rows.Clear()
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return dt
  End Function

  Public Overridable Function AggiornaJob(strDitta As String, nID As Integer, ByVal strTipo As String) As Boolean
    Dim strSQL As String = ""
    Dim lResult As Integer = 0
    Try

      If strTipo <> "1" Then
        strSQL = "UPDATE tabhhtestaelaborati SET importato = 'S' " &
                " WHERE codditt = " & CStrSQL(strDitta) &
                " AND id = " & NTSCInt(nID)
      Else
        strSQL = "UPDATE tabhhmovimenti SET importato = 'S' " &
                " WHERE codditt = " & CStrSQL(strDitta) &
                " AND id = " & NTSCInt(nID)
      End If
      lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      If lResult < 0 Then Return False

      Return True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Function

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

  Public Overridable Function getGiacenzaBus(strDitta As String, strCodArt As String, nMagaz As Integer) As Decimal
    Dim strSQL As String = ""
    Dim dt As New DataTable
    Try

      strSQL = "SELECT * FROM artpro " &
                " WHERE codditt = " & CStrSQL(strDitta) &
                " AND ap_codart = " & CStrSQL(strCodArt) &
                " AND ap_magaz = " & NTSCInt(nMagaz)
      dt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dt.Rows.Count = 0 Then Return 0

      Return NTSCDec(dt.Rows(0) !ap_esist)
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Function
End Class
