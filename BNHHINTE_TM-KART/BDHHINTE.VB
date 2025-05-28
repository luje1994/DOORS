Imports NTSInformatica.CLN__STD
Imports System.Data.OleDb

Public Class CLDHHINTE
  Inherits CLD__BASE

#Region "Funzioni e Routines"
  Public Overridable Function GetData(ByRef dsOut As DataSet) As Boolean
    Try
      Dim ds As New DataSet
      ds = dsOut.Copy

      '--------------------------------------------------
      '--- Azzero/Creo datatable TESTA
      '--------------------------------------------------
      If ds.Tables.Contains("TESTA") Then
        ds.Tables("TESTA").Clear()
      Else
        ds.Tables.Add("TESTA")
        With ds.Tables("TESTA")
          .Columns.Add("tb_urlchia", GetType(String))
          .Columns.Add("tb_conto", GetType(Integer))
          .Columns.Add("tb_serie", GetType(String))
          .Columns.Add("tb_tipobf", GetType(Integer))
          .Columns.Add("tb_causalecarico", GetType(Integer))
          .Columns.Add("tb_causalescarico", GetType(Integer))
          .Columns.Add("tb_magazzinoMacchina", GetType(Integer))
          .Columns.Add("tb_magazzinoPrincipale", GetType(Integer))
          .Columns.Add("tb_elaborazioneautom", GetType(String))
          .Columns.Add("tb_secelaborazione", GetType(String))
          .Columns.Add("tb_elaborazionegiac1", GetType(String))
          .Columns.Add("tb_oragiac1", GetType(String))
          .Columns.Add("tb_elaborazionegiac2", GetType(String))
          .Columns.Add("tb_oragiac2", GetType(String))
          .Columns.Add("tb_elaborazionegiac3", GetType(String))
          .Columns.Add("tb_oragiac3", GetType(String))
        End With
      End If

      '--------------------------------------------------
      '--- Azzero/Creo datatable CORPO
      '--------------------------------------------------
      If ds.Tables.Contains("CORPO") Then
        ds.Tables("CORPO").Clear()
      Else
        ds.Tables.Add("CORPO")
        With ds.Tables("CORPO")
          .Columns.Add("tb_tipork", GetType(String))
          .Columns.Add("tb_serie", GetType(String))
          .Columns.Add("tb_operazione", GetType(String))
          .Columns.Add("tb_magazmacchina", GetType(Integer))
          .Columns.Add("xx_descrmagmacchina", GetType(String))
          .Columns.Add("tb_magazprincipale", GetType(Integer))
          .Columns.Add("xx_descrmagprincipale", GetType(String))
        End With
      End If

      '--------------------------------------------------
      '--- Legge le CHIAMATE.da registro di Business
      '--- e le carica nel DataSet
      '--------------------------------------------------
      Dim strTesta As String = GetSettingBus("BSHHINTE", "OPZIONI", ".", "HH_INTESTA", " ", " ", " ").Trim
      If strTesta.Trim.Length > 0 Then
        Dim srXmlTesta As System.IO.StringReader = New System.IO.StringReader(strTesta)
        ds.ReadXml(srXmlTesta)
      End If

      Dim strCorpo As String = GetSettingBus("BSHHINTE", "OPZIONI", ".", "HH_INCORPO", " ", " ", " ").Trim
      If strCorpo.Trim.Length > 0 Then
        Dim srXmlCorpo As System.IO.StringReader = New System.IO.StringReader(strCorpo)
        ds.ReadXml(srXmlCorpo)
      End If

      If dsOut.Tables.Contains("TESTA") Then
        dsOut.Tables("TESTA").Clear()
      End If
      ds.Tables("TESTA").DefaultView.Sort = "tb_urlchia ASC"
      Dim dtTesta As New DataTable
      dtTesta = dsOut.Tables("TESTA")
      dtTesta = ds.Tables("TESTA").DefaultView.ToTable.Copy

      If dtTesta.Rows.Count = 0 Then
        dtTesta.Rows.Add(dtTesta.NewRow)
      End If

      dsOut.Tables.Add(dtTesta)

      If dsOut.Tables.Contains("CORPO") Then
        dsOut.Tables("CORPO").Clear()
      End If
      ds.Tables("CORPO").DefaultView.Sort = "tb_tipork ASC, tb_serie ASC"
      Dim dtCorpo As New DataTable
      dtCorpo = dsOut.Tables("CORPO")
      dtCorpo = ds.Tables("CORPO").DefaultView.ToTable.Copy
      dsOut.Tables.Add(dtCorpo)
      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return True

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function
#End Region

#Region "Funzioni e Routines per invio INCARICOTECH"
  ''' <summary>
  ''' Seleziona gli Articoli da inviare
  ''' </summary>
  Public Overridable Function GetArticoli _
    (
    ByVal strDitta As String,
    ByVal strCodArt As String
    ) As DataRow

    Dim strSQL As String = ""
    Dim dttOut As New DataTable

    Try
      '--------------------------------------------------
      '--- Seleziona lo Sku da inviare dalla tabella HHAUSKU
      '--------------------------------------------------
      strSQL =
        "SELECT * FROM tabhhinvioarticoli" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND tb_codart = " + CStrSQL(strCodArt)
      dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dttOut.Rows.Count <> 0 Then
        Return dttOut.Rows(0)
      End If

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return Nothing

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Seleziona gli Articoli da inviare
  ''' </summary>
  Public Overridable Function GetArticoli _
    (
    ByVal strDitta As String
    ) As DataTable

    Dim strSQL As String = ""
    Dim dttOut As New DataTable

    Try
      '--------------------------------------------------
      '--- Seleziona lo Sku da inviare dalla tabella HHAUSKU
      '--------------------------------------------------
      strSQL =
        "SELECT * FROM tabhhinvioarticoli" +
        " WHERE codditt = " + CStrSQL(strDitta)
      dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return dttOut

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Inserisce nelle tabelle di "frotiera" i dati del' Articolo da inviare
  ''' </summary>
  Public Overridable Function InserisciDatiArticolo _
    (
    ByVal strDitta As String,
    ByVal strCodart As String
    ) As Boolean

    Dim strSQL As String = ""
    Dim dttArticolo As New DataTable
    Try
      '-------------------------------------------------
      '--- Legge articolo
      '-------------------------------------------------
      ValCodiceDb(strCodart, strDitta, "ARTICO", "S",, dttArticolo)

      '-------------------------------------------------
      '--- Elimina il record dell'articolo
      '-------------------------------------------------
      strSQL =
        "DELETE FROM tabhhinvioarticoli" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND tb_codart = " + CStrSQL(strCodart)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

      '-------------------------------------------------
      '--- Accoda articolo nella tabella per invio
      '-------------------------------------------------
      strSQL =
        "INSERT INTO tabhhinvioarticoli" +
        " (codditt, tb_codart, tb_descr, tb_stato, tb_data, tb_esito)" +
        " VALUES (" + CStrSQL(strDitta) + ", " + CStrSQL(strCodart) + ", " + CStrSQL(dttArticolo.Rows(0) !ar_descr) +
        ", 'A', GETDATE(), '')"
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

      '-------------------------------------------------
      '--- Valore funzione
      '-------------------------------------------------
      Return True

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Function

  ''' <summary>
  ''' Aggiorna esito invio Articoli
  ''' </summary>
  Public Overridable Sub AggiornaEsitoArticoli _
    (
    ByVal strDitta As String,
    ByVal strCodArt As String,
    ByVal strStato As String,
    ByVal strEsito As String
    )
    Dim strSQL As String = ""
    Try
      '--------------------------------------------------
      '--- Aggiorna esito invio Articoli
      '--------------------------------------------------
      strSQL =
        "UPDATE tabhhinvioarticoli" +
        " SET tb_stato = " + CStrSQL(strStato) + "," +
        " tb_data = " + CDataOraSQL(Now) + "," +
        " tb_esito = " + CStrSQL(strEsito) +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND tb_codart = " + CStrSQL(strCodArt)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Inserisce nelle tabelle di "frotiera" i dati della lista di prelievo da inviare
  ''' </summary>
  Public Overridable Function InserisciDatiListaDiPrelievo _
    (
    ByVal strDitta As String,
    ByVal dsDoc As DataSet,
    ByVal dtCorpo As DataTable,
    ByVal strLista As String,
    ByVal strOperazione As String
    ) As Boolean

    'Dichiarazione variabili
    Dim strSQL As String = ""
    Dim strCodart As String = ""
    'Dim strOrderId As String = ""
    Dim strTipork As String = " "
    Dim nAnno As Integer = 0
    Dim strSerie As String = " "
    Dim lNumdoc As Integer = 0
    Dim strNumord As String = ""
    Dim strSerieord As String = ""
    Dim dttOrd As New DataTable

    'Datatable righe per selezioe GROUP BY
    Dim dttCorpoGr As New DataTable
    Dim nRiga As Integer = 0
    Dim nQuant As Integer = 0

    Try
      'Crea il DataTable di righe raggruppato
      Dim oDttgr As New CLEGROUPBY
      Dim strSelect As String = "ec_codart, SUM(ec_quant) AS ec_quant"
      Dim strWhere As String = "ec_quant > 0 AND ec_hhcheckmag  = 'S' "
      Dim strGroupBy As String = "ec_codart"
      oDttgr.NTSGroupBy(dsDoc.Tables("CORPO"), dttCorpoGr, strSelect, strWhere, strGroupBy)
      If dttCorpoGr.Rows.Count = 0 Then
        Return False
      End If

      With dsDoc.Tables("TESTA").Rows(0)
        strTipork = NTSCStr(!et_tipork)
        nAnno = NTSCInt(!et_anno)
        strSerie = NTSCStr(!et_serie)
        lNumdoc = NTSCInt(!et_numdoc)
      End With

      '-------------------------------------------------
      '--- Accoda testata Lista di Prelievo
      '-------------------------------------------------
      'Ciclo di accodamento righe
      For Each drCorpo As DataRow In dttCorpoGr.Rows
        strCodart = NTSCStr(drCorpo!ec_codart)
        'InserisciDatiArticolo(strDitta, strCodart)
        nRiga += 1
        nQuant = NTSCInt(drCorpo!ec_quant)

        strSQL = "SELECT * FROM tabhhlistaprelievo " &
                  " WHERE codditt = " & CStrSQL(strDitta) &
                  " AND tb_lista = " & CStrSQL(strLista) &
                  " AND tb_riga = " & nRiga

        Dim dt As New DataTable
        dt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

        If dt.Rows.Count = 0 Then
          strSQL =
            "INSERT INTO tabhhlistaprelievo" +
            " (codditt, tb_lista, tb_riga, tb_tipork, tb_anno, tb_serie, tb_numero, tb_codart, tb_operazione, tb_quantita, tb_stato, tb_data, tb_esito)" +
            " VALUES (" + CStrSQL(strDitta) + ", " + CStrSQL(strLista) + ", " + nRiga.ToString + ", " + CStrSQL(strTipork) + ", " + nAnno.ToString +
            ", " + CStrSQL(strSerie) + ", " + lNumdoc.ToString + ", " + CStrSQL(strCodart) + ", " + CStrSQL(strOperazione) +
            ", " + nQuant.ToString + ", '', " + CDataSQL(Now.ToShortDateString) + ", '')"
          Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
        Else
          strSQL =
            "UPDATE tabhhlistaprelievo SET codditt = " + CStrSQL(strDitta) + ", tb_lista = " + CStrSQL(strLista) + " , tb_riga = " + nRiga.ToString +
                ", tb_tipork = " + CStrSQL(strTipork) + ", tb_anno = " + nAnno.ToString + ", tb_serie = " + CStrSQL(strSerie) + ", tb_numero = " + lNumdoc.ToString +
                ", tb_codart = " + CStrSQL(strCodart) + ", tb_operazione = " + CStrSQL(strOperazione) + ", tb_quantita = " + nQuant.ToString + ", tb_stato = ''" +
                ", tb_data = " + CDataSQL(Now.ToShortDateString) + ", tb_esito = ''" +
                " WHERE codditt = " + CStrSQL(strDitta) +
                " AND tb_lista = " + CStrSQL(strLista) +
                " AND tb_riga = " + nRiga.ToString
          Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
        End If

      Next

      '-------------------------------------------------
      '--- Valore funzione
      '-------------------------------------------------
      Return True

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Function

  ''' <summary>
  ''' Seleziona la lista di prelievo da inviare
  ''' </summary>
  Public Overridable Function GetListadiPrelievo _
    (
    ByVal strDitta As String,
    ByVal strLista As String
    ) As DataTable

    'Dichiarazione variabili
    Dim dttOut As New DataTable
    Dim strSQL As String = ""

    Try
      '--------------------------------------------------
      '--- Seleziona la lista di prelievo da inviare dalla tabella tabhhlistaprelievo
      '--------------------------------------------------
      strSQL =
        "SELECT * FROM tabhhlistaprelievo" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND tb_lista = " + CStrSQL(strLista)
      dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return dttOut

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Aggiorna esito invio Lista di Prelievo
  ''' </summary>
  Public Overridable Sub AggiornaEsitoListaDiPrelievo _
    (
    ByVal strDitta As String,
    ByVal strLista As String,
    ByVal strStato As String,
    ByVal strEsito As String
    )
    Dim strSQL As String = ""
    Try
      '--------------------------------------------------
      '--- Aggiorna esito invio Articoli
      '--------------------------------------------------
      strSQL =
        "UPDATE tabhhlistaprelievo" +
        " SET tb_stato = " + CStrSQL(strStato) + "," +
        " tb_data = " + CDataOraSQL(Now) + "," +
        " tb_esito = " + CStrSQL(strEsito) +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND tb_lista = " + CStrSQL(strLista)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Function getOrdiniElaborati(ByRef dsOut As DataSet) As Boolean
    Dim strSQL As String = ""
    Try
      strSQL =
        "SELECT TOP 1 * FROM tabhhtestaelaborati "

      If Not dsOut Is Nothing Then
        If Not dsOut.Tables("TESTA") Is Nothing Then dsOut.Tables.Remove("TESTA")
      End If
      OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "TESTA", dsOut)
      dsOut.Tables("TESTA").Rows.Clear()

      strSQL =
        "SELECT TOP 1 * FROM tabhhcorpoelaborati "

      If Not dsOut Is Nothing Then
        If Not dsOut.Tables("CORPO") Is Nothing Then dsOut.Tables.Remove("CORPO")
      End If
      OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "CORPO", dsOut)
      dsOut.Tables("CORPO").Rows.Clear()

      Return True
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Function getOperazioniManuali(ByRef dsOut As DataSet) As Boolean
    Dim strSQL As String = ""
    Try
      strSQL =
        "SELECT TOP 1 * FROM tabhhmovimenti "

      If Not dsOut Is Nothing Then
        If Not dsOut.Tables("MOV") Is Nothing Then dsOut.Tables.Remove("MOV")
      End If
      OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "MOV", dsOut)
      dsOut.Tables("MOV").Rows.Clear()

      Return True
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Function getOrderID(ByVal strDitta As String) As Integer
    Dim strSQL As String = ""
    Dim dt As New DataTable

    Try
      strSQL =
        "SELECT TOP 1 id FROM tabhhtestaelaborati WHERE codditt = " & CStrSQL(strDitta) & "ORDER BY id DESC "
      dt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dt.Rows.Count = 0 Then Return 1

      Return NTSCInt(dt.Rows(0) !id) + 1
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Function getOperationID(ByVal strDitta As String) As Integer
    Dim strSQL As String = ""
    Dim dt As New DataTable

    Try
      strSQL =
        "SELECT TOP 1 id FROM tabhhmovimenti WHERE codditt = " & CStrSQL(strDitta) & "ORDER BY id DESC "
      dt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dt.Rows.Count = 0 Then Return 1

      Return NTSCInt(dt.Rows(0) !id) + 1
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Function SalvaOrdiniElaborati(dsOrdine As DataSet) As Boolean
    Dim strSQL As String = ""
    Dim dt As New DataTable
    Dim lResult As Integer = 0

    Try
      For Each drTesta As DataRow In dsOrdine.Tables("TESTA").Rows
        strSQL =
        "INSERT INTO tabhhtestaelaborati " & GetQueryInsertField(dsOrdine.Tables("TESTA"), "") &
                                " VALUES " & GetQueryInsertValue(dsOrdine.Tables("TESTA"), drTesta, "")
        lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
        If lResult < 0 Then Return False
      Next

      For Each drCorpo As DataRow In dsOrdine.Tables("CORPO").Rows
        strSQL =
        "INSERT INTO tabhhcorpoelaborati " & GetQueryInsertField(dsOrdine.Tables("CORPO"), "") &
                                " VALUES " & GetQueryInsertValue(dsOrdine.Tables("CORPO"), drCorpo, "")
        lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
        If lResult < 0 Then Return False
      Next

      Return True
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Function SalvaOperazioniManuali(drMov As DataRow) As Boolean
    Dim strSQL As String = ""
    Dim dt As New DataTable
    Dim lResult As Integer = 0

    Try
      strSQL =
      "INSERT INTO tabhhmovimenti " & GetQueryInsertField(drMov.Table, "") &
                         " VALUES " & GetQueryInsertValue(drMov.Table, drMov, "")
      lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      If lResult < 0 Then Return False

      Return True
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

#End Region

End Class

