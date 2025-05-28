Imports NTSInformatica.CLN__STD
Imports System.Data.OleDb

Public Class CLDHHCNMG
  Inherits CLD__BASE

#Region "Funzioni e Routines"
  Public Overridable Function GetData(ByRef dsOut As DataSet) As Boolean
    Try
      Dim ds As New DataSet
      ds = dsOut.Copy

      '--------------------------------------------------
      '--- Azzero/Creo datatable CHIAMATE
      '--------------------------------------------------
      If ds.Tables.Contains("CHIAMATE") Then
        ds.Tables("CHIAMATE").Clear()
      Else
        ds.Tables.Add("CHIAMATE")
        With ds.Tables("CHIAMATE")
          .Columns.Add("ch_codchia", GetType(Integer))
          .Columns.Add("ch_deschia", GetType(String))
          .Columns.Add("ch_urlchia", GetType(String))
        End With
      End If

      '--------------------------------------------------
      '--- Legge le CHIAMATE.da registro di Business
      '--- e le carica nel DataSet
      '--------------------------------------------------
      Dim strProf As String = GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_CNChiamate", " ", " ", " ").Trim
      If strProf.Trim.Length > 0 Then
        Dim srXml As System.IO.StringReader = New System.IO.StringReader(strProf)
        ds.ReadXml(srXml)
      End If

      If dsOut.Tables.Contains("CHIAMATE") Then
        dsOut.Tables("CHIAMATE").Clear()
      End If
      ds.Tables("CHIAMATE").DefaultView.Sort = "ch_codchia ASC"
      Dim dt As New DataTable
      dt = dsOut.Tables("CHIAMATE")
      dt = ds.Tables("CHIAMATE").DefaultView.ToTable.Copy
      dsOut.Tables.Add(dt)

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

#Region "Funzioni e Routines per invio MAGAZZINO"
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
    ByVal dsOrder As DataSet,
    ByRef strLista As String
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
    Dim strOperazione As String = ""

    Try
      'Crea il DataTable di righe raggruppato
      Dim oDttgr As New CLEGROUPBY
      Dim strSelect As String = "ec_codart, SUM(ec_quant) AS ec_quant"
      Dim strWhere As String = "ec_quant > 0 AND ec_hhcheckmag  = 'S' "
      Dim strGroupBy As String = "ec_codart"
      oDttgr.NTSGroupBy(dsOrder.Tables("CORPO"), dttCorpoGr, strSelect, strWhere, strGroupBy)
      If dttCorpoGr.Rows.Count = 0 Then
        Return False
      End If

      '-------------------------------------------------
      '--- Creo Codice Lista
      '-------------------------------------------------
      With dsOrder.Tables("TESTA").Rows(0)
        strTipork = NTSCStr(!et_tipork)
        nAnno = NTSCInt(!et_anno)
        strSerie = NTSCStr(!et_serie)
        lNumdoc = NTSCInt(!et_numdoc)

        strLista = strTipork.ToString + nAnno.ToString.PadLeft(4, "0"c) + strSerie.ToString.ToUpper.PadRight(3) + lNumdoc.ToString.PadLeft(8, "0"c)
      End With

      '-------------------------------------------------
      '--- Accoda testata Lista di Prelievo
      '-------------------------------------------------
      'Ciclo di accodamento righe
      For Each drCorpo As DataRow In dttCorpoGr.Rows
        strCodart = NTSCStr(drCorpo!ec_codart)
        InserisciDatiArticolo(strDitta, strCodart)
        nRiga += 1
        nQuant = NTSCInt(drCorpo!ec_quant)
        strOperazione = "-"
        If strTipork = "M" Then strOperazione = "+"

        strSQL =
        "INSERT INTO tabhhlistaprelievo" +
        " (codditt, tb_lista, tb_riga, tb_tipork, tb_anno, tb_serie, tb_numero, tb_codart, tb_operazione, tb_quantita, tb_stato, tb_data, tb_esito)" +
        " VALUES (" + CStrSQL(strDitta) + ", " + CStrSQL(strLista) + ", " + nRiga.ToString + ", " + CStrSQL(strTipork) + ", " + nAnno.ToString +
        ", " + CStrSQL(strSerie) + ", " + lNumdoc.ToString + ", " + CStrSQL(strCodart) + ", " + CStrSQL(strOperazione) +
        ", " + nQuant.ToString + ", '', " + CDataSQL(Now.ToShortDateString) + ", '')"
        Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
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

#End Region

#Region "Funzioni e Routines per invio AUTOSTORE"
  ''' <summary>
  ''' Seleziona lo specifico skuId da inviare
  ''' </summary>
  Public Overridable Function GetSku _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strSkuId As String
    ) As DataRow

    Dim strSQL As String = ""
    Dim dttOut As New DataTable

    Try
      '--------------------------------------------------
      '--- Seleziona lo Sku da inviare dalla tabella HHAUSKU
      '--------------------------------------------------
      strSQL =
        "SELECT * FROM HHAUSKU" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND skuId = " + CStrSQL(strSkuId)
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
  ''' Seleziona tutti gli skuId da inviare
  ''' </summary>
  Public Overridable Function GetSku _
    (
    ByVal strDitta As String,
    ByVal strClientId As String
    ) As DataTable

    Dim strSQL As String = ""
    Dim dttOut As New DataTable

    Try
      '--------------------------------------------------
      '--- Seleziona lo Sku da inviare dalla tabella HHAUSKU
      '--------------------------------------------------
      strSQL =
        "SELECT * FROM HHAUSKU" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND ISNULL(BUSStatus,' ') = ' ' "
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
  ''' Seleziona le unità di misura associate allo skuId
  ''' </summary>
  Public Overridable Function GetSkuUnmis(ByVal strDitta As String, ByVal strCodart As String) As DataTable
    Try
      Dim dttUnmis As New DataTable
      Dim dttArt As New DataTable
      Dim strSQL As String = ""

      '--------------------------------------------------
      '--- Crea il Datatable delle unità di misura dell'articolo
      '--------------------------------------------------
      dttUnmis.Columns.Add("ar_unmis", GetType(String))
      dttUnmis.Columns.Add("ar_fattore", GetType(Decimal))

      '--------------------------------------------------
      '--- Seleziona le Unità di mIusra dell 'articolo
      '--------------------------------------------------
      strSQL =
        "SELECT ar_unmis, ar_confez2, ar_unmis2, ar_unmis5, ar_unmis6, ar_unmis7, ar_unmis8," +
        " 1," +
        " CASE WHEN ar_qtacon2 = 0 THEN 1 ELSE ar_qtacon2 END," +
        " CASE WHEN ar_conver5 = 0 THEN 1 ELSE CASE WHEN ar_umsegno5 = -1 THEN ROUND(1 / ar_conver5, 3) ELSE ar_conver5 END END," +
        " CASE WHEN ar_conver6 = 0 THEN 1 ELSE CASE WHEN ar_umsegno6 = -1 THEN ROUND(1 / ar_conver6, 3) ELSE ar_conver6 END END," +
        " CASE WHEN ar_conver7 = 0 THEN 1 ELSE CASE WHEN ar_umsegno7 = -1 THEN ROUND(1 / ar_conver7, 3) ELSE ar_conver7 END END," +
        " CASE WHEN ar_conver8 = 0 THEN 1 ELSE CASE WHEN ar_umsegno8 = -1 THEN ROUND(1 / ar_conver8, 3) ELSE ar_conver8 END END" +
        " FROM artico " +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND ar_codart = " + CStrSQL(strCodart)
      dttArt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      '--------------------------------------------------
      '--- Popola il Datatable delle unità di misura dell'articolo
      '--------------------------------------------------
      If dttArt.Rows.Count <> 0 Then
        For col As Integer = 0 To 6
          If NTSCStr(dttArt.Rows(0).ItemArray(col)) <> "" Then
            dttUnmis.Rows.Add(dttUnmis.NewRow)
            dttUnmis.Rows(dttUnmis.Rows.Count - 1) !ar_unmis = NTSCStr(dttArt.Rows(0).ItemArray(col))
            dttUnmis.Rows(dttUnmis.Rows.Count - 1) !ar_fattore = NTSCStr(dttArt.Rows(0).ItemArray(col + 7))
          End If
        Next
      End If

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return dttUnmis

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Aggiorna esito invio SkuId
  ''' </summary>
  Public Overridable Sub AggiornaEsitoSku _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strSkuId As String,
    ByVal strStato As String,
    ByVal strOperazione As String,
    ByVal strEsito As String
    )
    Dim strSQL As String = ""
    Try
      '--------------------------------------------------
      '--- Aggiorna esito invio SkuId
      '--------------------------------------------------
      strSQL =
        "UPDATE HHAUSKU" +
        " SET BUSStatus = " + CStrSQL(strStato) + "," +
        " BUSData = " + CDataOraSQL(Now) + "," +
        " BUSOperazione = " + CStrSQL(strOperazione) + "," +
        " BUSEsito = " + CStrSQL(strEsito) +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND skuId = " + CStrSQL(strSkuId)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Inserisce nelle tabelle di "frotiera" i dati dello SkuId da inviare
  ''' </summary>
  Public Overridable Function InserisciDatiSku _
    (
    ByVal strDitta As String,
    ByVal strClientid As String,
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
        "DELETE FROM HHAUSKU" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientid) +
        " AND skuId = " + CStrSQL(strCodart)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

      '-------------------------------------------------
      '--- Accoda articolo nella tabella per invio
      '-------------------------------------------------
      strSQL =
        "INSERT INTO HHAUSKU" +
        " (codditt, clientId, skuId, description, productCode, productGroupId, baseQuantityUnitId, handlingHint," +
        " batchMandatoryForHost, stocktakingExcluded, cycleCountingThreshold," +
        " BUSStatus, BUSData, BUSOperazione, BUSEsito)" +
        " VALUES (" + CStrSQL(strDitta) + ", " + CStrSQL(strClientid) + ", " + CStrSQL(strCodart) + ", " + CStrSQL(dttArticolo.Rows(0) !ar_descr) +
        ", ISNULL((SELECT TOP (1) bc_code FROM barcode WHERE codditt = " + CStrSQL(strDitta) + " AND bc_codart = " + CStrSQL(strCodart) + " ORDER BY bc_datagg DESC, bc_oragg DESC),'')" +
        ", '', " + CStrSQL(dttArticolo.Rows(0) !ar_unmis) + ", '', 'true', '', 0" +
        ", ' ', Null, Null, Null)"
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
  ''' Seleziona l'Advice da inviare
  ''' </summary>
  Public Overridable Function GetAdvice _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strAdviceId As String
    ) As DataRow

    Dim dttOut As New DataTable
    Dim strSQL As String = ""

    Try
      '--------------------------------------------------
      '--- Seleziona l'Advice da inviare dalla tabella HHAUADVICETEST
      '--------------------------------------------------
      strSQL =
        "SELECT * FROM HHAUADVICETEST" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND adviceId = " + CStrSQL(strAdviceId)
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
  ''' Seleziona le righe dell'Advice da inviare
  ''' </summary>
  Public Overridable Function GetAdviceMov _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strAdviceId As String
    ) As DataTable

    'Dichiarazione variabili
    Dim dttOut As New DataTable
    Dim strSQL As String = ""

    Try
      '--------------------------------------------------
      '--- Seleziona le righe dell'Advice da inviare dalla tabella HHAUADVICEMOV
      '--------------------------------------------------
      strSQL =
        "SELECT * FROM HHAUADVICEMOV " +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND adviceId = " + CStrSQL(strAdviceId)
      dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      '-------------------------------------------------
      '--- Valore funzione
      '-------------------------------------------------
      Return dttOut

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Aggiorna esito invio Advice
  ''' </summary>
  Public Overridable Sub AggiornaEsitoAdvice _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strAdviceId As String,
    ByVal strStato As String,
    ByVal strOperazione As String,
    ByVal strEsito As String
    )
    Dim strSQL As String = ""
    Try
      '--------------------------------------------------
      '--- Aggiorna esito invio SkuId
      '--------------------------------------------------
      strSQL =
        "UPDATE HHAUADVICETEST" +
        " SET BUSStatus = " + CStrSQL(strStato) + "," +
        " BUSData = " + CDataOraSQL(Now) + "," +
        " BUSOperazione = " + CStrSQL(strOperazione) + "," +
        " BUSEsito = " + CStrSQL(strEsito) +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND adviceId = " + CStrSQL(strAdviceId)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Inserisce nelle tabelle di "frotiera" i dati dell' Advice da inviare
  ''' </summary>
  Public Overridable Function InserisciDatiAdvice _
    (
    ByVal strDitta As String,
    ByVal strClientid As String,
    ByVal dsAdvice As DataSet,
    ByVal nMagazzinoAutostore As Integer,
    ByVal strBusTipo As String,
    ByRef strAdviceId As String
    ) As Boolean

    Dim strSQL As String = ""
    Dim strCodart As String = ""
    'Dim strAdviceId As String = ""
    Dim strSupplierId As String = ""
    Dim dttArticolo As New DataTable
    Dim strTipork As String = " "
    Dim nAnno As Integer = 0
    Dim strSerie As String = " "
    Dim lNumdoc As Integer = 0
    Dim strBUSDescrizione As String = ""
    Dim strBUSCliente As String = ""
    Dim strBUSNote As String = ""
    Dim strBUSSpedizione As String = ""
    Dim nProgressivo As Integer = 0

    'Datatable righe per selezioe GROUP BY
    Dim dttCorpoGr As New DataTable
    Dim nRiga As Integer = 0

    Try
      '-------------------------------------------------
      '--- Crea il DataTable di righe raggruppato
      '-------------------------------------------------
      Dim oDttgr As New CLEGROUPBY
      Dim strSelect As String = "ec_codart, ec_unmis, SUM(ec_quant) AS ec_quant"
      Dim strWhere As String = "ec_magaz = " + nMagazzinoAutostore.ToString + " AND ec_quant > 0"
      Dim strGroupBy As String = "ec_codart, ec_unmis"
      oDttgr.NTSGroupBy(dsAdvice.Tables("CORPO"), dttCorpoGr, strSelect, strWhere, strGroupBy)
      If dttCorpoGr.Rows.Count = 0 Then
        Return False
      End If

      '-------------------------------------------------
      '--- Creo AdviceId
      '-------------------------------------------------
      With dsAdvice.Tables("TESTA").Rows(0)
        strTipork = NTSCStr(!et_tipork)
        nAnno = NTSCInt(!et_anno)
        strSerie = NTSCStr(!et_serie)
        lNumdoc = NTSCInt(!et_numdoc)
        strBUSDescrizione = NTSCStr(!et_riferim.ToString).PadRight(50).Substring(0, 50).Trim
        strBUSCliente = NTSCStr(!xx_conto)
        strBUSNote = "CAR"
        strBUSSpedizione = IIf(NTSCInt(!et_vettor) = 0, "", NTSCStr(!xx_vettor)).ToString

        nProgressivo = getAdviceProgressivo(strDitta, strClientid, strTipork, nAnno, strSerie, lNumdoc)

        strAdviceId = strTipork.ToString + nAnno.ToString.PadLeft(4, "0"c) + strSerie.ToString.ToUpper.PadRight(3) + nProgressivo.ToString.PadLeft(2, "0"c) + lNumdoc.ToString.PadLeft(8, "0"c)
      End With

      'ADESSO L'ID VIENE GESTITO CON UN PROGRESSIVO E NON SARA' MAI UGUALE. QUINDI NON HO BISOGNO DI CANCELLARE
      ''-------------------------------------------------
      ''--- Elimina i record dell'advice
      ''-------------------------------------------------
      'strSQL =
      '  "DELETE FROM HHAUADVICEMOV" +
      '  " WHERE codditt = " + CStrSQL(strDitta) +
      '  " AND clientId = " + CStrSQL(strClientid) +
      '  " AND adviceId = " + CStrSQL(strAdviceId)
      'Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

      'strSQL =
      '  "DELETE FROM HHAUADVICETEST" +
      '  " WHERE codditt = " + CStrSQL(strDitta) +
      '  " AND clientId = " + CStrSQL(strClientid) +
      '  " AND adviceId = " + CStrSQL(strAdviceId)
      'Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

      'Imposta supplierId
      strSupplierId = dsAdvice.Tables("TESTA").Rows(0) !xx_conto.ToString.Replace(vbCrLf, "").PadLeft(50).Substring(0, 50)

      '-------------------------------------------------
      '--- Accoda testata Advice
      '-------------------------------------------------
      strSQL =
        "INSERT INTO HHAUADVICETEST" +
        " (codditt, clientId, adviceId, adviceType, referenceBarcode, purchaseOrderReference, supplierId," +
        " expectationDate, deliveryText, additionalHostData," +
        " BUSStatus, BUSData, BUSOperazione, BUSEsito," +
        " BUSTipork, BUSAnno, BUSSerie, BUSNumDoc, BUSProgressivo, BusTipo," +
        " BUSDescrizione, BUSCliente, BUSNote, BUSSpedizione)" +
        " VALUES (" + CStrSQL(strDitta) + ", " + CStrSQL(strClientid) + ", " + CStrSQL(strAdviceId) + ", 'SUPPLIER'" +
        ", Null, Null, " + CStrSQL(strSupplierId) + ", " + CDataSQL(Now.ToShortDateString) + ", Null, 'additionalHostData'" +
        ", Null, Null, Null, Null" +
        ", " + CStrSQL(strTipork) + ", " + nAnno.ToString + ", " + CStrSQL(strSerie) + ", " + lNumdoc.ToString + ", " + CStrSQL(nProgressivo.ToString) + ", " + CStrSQL(strBusTipo) +
        ", " + CStrSQL(strBUSDescrizione) + ", " + CStrSQL(strBUSCliente) + ", " + CStrSQL(strBUSNote) + ", " + CStrSQL(strBUSSpedizione) + ")"
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

      '-------------------------------------------------
      '--- Accoda righe Advice
      '-------------------------------------------------
      'Ciclo di accodamento righe
      For Each dtrCorpo As DataRow In dttCorpoGr.Rows
        'Per sicurezza reinvio lo Sku così se articolo aggiornato da procedure esterne (import listini o query)
        'lo aggiorno su Autostore
        strCodart = NTSCStr(dtrCorpo!ec_codart)
        InserisciDatiSku(strDitta, strClientid, strCodart)

        nRiga += 1

        'Accoda riga advice
        strSQL =
          "INSERT INTO HHAUADVICEMOV" +
          " (codditt, clientId, adviceId, adviceLineId, skuId, supplierSkuId, purchaseOrderLine, quantityTarget, receivingQuantityUnit," +
          " unlimitedOverdeliveryAllowed, overdeliveryTolerance, undeliveryTolerance, batch, hostStorageLocation, bestBeforeDate," +
          " deliveryText, HoldReasonld, dateOfReceipt, dateOfManufacture, additionalHostData)" +
          " VALUES (" + CStrSQL(strDitta) + ", " + CStrSQL(strClientid) + ", " + CStrSQL(strAdviceId) + ", " + CStrSQL(nRiga.ToString.PadLeft(4, "0"c)) +
          ", " + CStrSQL(strCodart) + ", Null, Null, " + CDblSQL(dtrCorpo!ec_quant) + ", " + CStrSQL(dtrCorpo!ec_unmis) +
          ", Null, Null, Null, 'BATCH', Null, " + CDataSQL(Now.ToShortDateString) +
          ", Null, Null, Null, Null, Null)"
        Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
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
  ''' Trova Progressivo Advice
  ''' </summary>
  Public Overridable Function getAdviceProgressivo _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strTipork As String,
    ByVal nAnno As Integer,
    ByVal strSerie As String,
    ByVal lNumdoc As Integer
    ) As Integer
    Dim strSQL As String = ""
    Dim dtt As New DataTable
    Try
      strSQL =
        "SELECT TOP 1 * FROM HHAUADVICETEST" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND BUSTipork = " + CStrSQL(strTipork) +
        " AND BUSAnno = " + nAnno.ToString +
        " AND BUSSerie = " + CStrSQL(strSerie) +
        " AND BUSNumDoc = " + lNumdoc.ToString +
        " ORDER BY BUSProgressivo DESC "
      dtt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dtt.Rows.Count <> 0 Then
        Return NTSCInt(dtt.Rows(0) !BUSProgressivo) + 1
      End If

      Return 0
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Function

  ''' <summary>
  ''' Seleziona l'Order da inviare
  ''' </summary>
  Public Overridable Function GetOrder _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strOrderId As String
    ) As DataRow

    'Dichiarazione variabili
    Dim dttOut As New DataTable
    Dim strSQL As String = ""

    Try
      '--------------------------------------------------
      '--- Seleziona l'Order da inviare dalla tabella HHAUORDERTEST
      '--------------------------------------------------
      strSQL =
        "SELECT * FROM HHAUORDERTEST" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND orderId = " + CStrSQL(strOrderId)
      dttOut = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dttOut.Rows.Count > 0 Then
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
  ''' Seleziona le righe dell'Order da inviare
  ''' </summary>
  Public Overridable Function GetOrderMov _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strOrderId As String
    ) As DataTable

    'Dichiarazione variabili
    Dim dttOut As New DataTable
    Dim strSQL As String = ""

    Try
      '--------------------------------------------------
      '--- Seleziona le righe dell'Order da inviare dalla tabella HHAUORDERMOV
      '--------------------------------------------------
      strSQL =
        "SELECT * FROM HHAUORDERMOV" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND orderId = " + CStrSQL(strOrderId)
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
  ''' Aggiorna esito invio Order
  ''' </summary>
  Public Overridable Sub AggiornaEsitoOrder _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strOrderId As String,
    ByVal strStato As String,
    ByVal strOperazione As String,
    ByVal strEsito As String
    )
    Dim strSQL As String = ""
    Try
      '--------------------------------------------------
      '--- Aggiorna esito invio SkuId
      '--------------------------------------------------
      strSQL =
        "UPDATE HHAUORDERTEST" +
        " SET BUSStatus = " + CStrSQL(strStato) + "," +
        " BUSData = " + CDataOraSQL(Now) + "," +
        " BUSOperazione = " + CStrSQL(strOperazione) + "," +
        " BUSEsito = " + CStrSQL(strEsito) +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND orderId = " + CStrSQL(strOrderId)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Inserisce nelle tabelle di "frotiera" i dati dell' Order da inviare
  ''' </summary>
  Public Overridable Function InserisciDatiOrder _
    (
    ByVal strDitta As String,
    ByVal strClientid As String,
    ByVal dsOrder As DataSet,
    ByVal nMagazPrelievoAutostore As Integer,
    ByVal strBusTipo As String,
    ByVal nPriorita As Integer,
    ByRef strOrderId As String
    ) As Boolean

    'Dichiarazione variabili
    Dim strSQL As String = ""
    Dim strCodart As String = ""
    'Dim strOrderId As String = ""
    Dim strCustomerId As String = ""
    Dim dttArticolo As New DataTable
    Dim strTipork As String = " "
    Dim nAnno As Integer = 0
    Dim strSerie As String = " "
    Dim lNumdoc As Integer = 0
    Dim strNumord As String = ""
    Dim strSerieord As String = ""
    Dim dttOrd As New DataTable
    Dim strBUSDescrizione As String = ""
    Dim strBUSCliente As String = ""
    Dim strBUSNote As String = ""
    Dim strBUSSpedizione As String = ""
    Dim nProgressivo As Integer = 0
    Dim strAdditionalHostData As String = ""
    Dim strUbicazione As String = ""
    Dim nCommessa As Integer = 0

    'Datatable righe per selezioe GROUP BY
    Dim dttCorpoGr As New DataTable
    Dim nRiga As Integer = 0

    Try
      'Crea il DataTable di righe raggruppato
      Dim oDttgr As New CLEGROUPBY
      Dim strSelect As String = "ec_codart, ec_unmis, SUM(ec_quant) AS ec_quant"
      Dim strWhere As String = "ec_quant > 0 AND ec_magaz = " + nMagazPrelievoAutostore.ToString
      Dim strGroupBy As String = "ec_codart, ec_unmis"
      oDttgr.NTSGroupBy(dsOrder.Tables("CORPO"), dttCorpoGr, strSelect, strWhere, strGroupBy)
      If dttCorpoGr.Rows.Count = 0 Then
        Return False
      End If

      '-------------------------------------------------
      '--- Creo OrderId
      '-------------------------------------------------
      With dsOrder.Tables("TESTA").Rows(0)
        strTipork = NTSCStr(!et_tipork)
        nAnno = NTSCInt(!et_anno)
        strSerie = NTSCStr(!et_serie)
        lNumdoc = NTSCInt(!et_numdoc)
        strBUSDescrizione = NTSCStr(!et_riferim.ToString).PadRight(50).Substring(0, 50).Trim
        'Verifico se è una NDP collegata a IDC/IC
        strNumord = ""
        strSerieord = ""

        'AAAAAAAAAAAAAAAAAAAAAAA
        'RINOMINARE SENZA underscore in tm_hhscamagautomatico???? 
        'o comunque con un nome geralizzato per MODULA e AUTOSTORE
        If!et_hh_sca_modula.ToString <> "S" Then
          'Cerco i riferimento dall'impegno evaso
          strSQL =
            "SELECT testord.td_numord, testord.td_serie, testord.td_datord, testord.td_datcons, movord.mo_datcons" +
            " FROM movprb" +
            " INNER JOIN movord" +
            " ON movord.codditt = movprb.codditt" +
            " AND movord.mo_tipork = movprb.mm_ortipo" +
            " AND movord.mo_anno = movprb.mm_oranno" +
            " AND movord.mo_serie = movprb.mm_orserie" +
            " AND movord.mo_numord = movprb.mm_ornum" +
            " AND movord.mo_riga = movprb.mm_orriga" +
            " INNER JOIN testord" +
            " ON testord.codditt = movord.codditt" +
            " AND testord.td_tipork = movord.mo_tipork" +
            " AND testord.td_anno = movord.mo_anno" +
            " AND testord.td_serie  = movord.mo_serie" +
            " AND testord.td_numord = movord.mo_numord" +
            " WHERE movprb.codditt = " + CStrSQL(!codditt) +
            " AND movprb.mm_tipork  = " + CStrSQL(!et_tipork) +
            " AND movprb.mm_anno  = " + NTSCStr(!et_anno) +
            " AND movprb.mm_serie  = " + CStrSQL(!et_serie) +
            " AND movprb.mm_numdoc  = " + NTSCStr(!et_numdoc)
          dttOrd = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)
          If dttOrd.Rows.Count > 0 Then
            strNumord = NTSCStr(dttOrd.Rows(0) !td_numord)
            strSerieord = NTSCStr(dttOrd.Rows(0) !td_serie)
          End If
          dttOrd.Clear()
        End If

        If strNumord.Trim <> "" Then
          strBUSCliente = NTSCStr("Ord. Nr: " + strNumord + IIf(strSerieord.Trim.Length = 0, "", "/" + strSerieord).ToString + " - " +!xx_conto.ToString).PadRight(50).Substring(0, 50).Trim
        Else
          strBUSCliente = NTSCStr("Prelievo Interno - " +!xx_conto.ToString).PadRight(50).Substring(0, 50).Trim
        End If

        strBUSNote = "PICK"
        strBUSSpedizione = IIf(NTSCInt(!et_vettor) = 0, "Vettore non inserito", NTSCStr(!xx_vettor)).ToString

        nProgressivo = getOrderProgressivo(strDitta, strClientid, strTipork, nAnno, strSerie, lNumdoc)

        strOrderId = strTipork.ToString + nAnno.ToString.PadLeft(4, "0"c) + strSerie.ToString.ToUpper.PadRight(3) + nProgressivo.ToString.PadLeft(2, "0"c) + lNumdoc.ToString.PadLeft(8, "0"c)
      End With

      'ADESSO L'ID VIENE GESTITO CON UN PROGRESSIVO E NON SARA' MAI UGUALE. QUINDI NON HO BISOGNO DI CANCELLARE
      ''-------------------------------------------------
      ''--- Elimina i record dell'order
      ''-------------------------------------------------
      'strSQL =
      '  "DELETE FROM HHAUORDERMOV" +
      '  " WHERE codditt = " + CStrSQL(strDitta) +
      '  " And clientId = " + CStrSQL(strClientid) +
      '  " And orderId = " + CStrSQL(strOrderId)
      'Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

      'strSQL =
      '  "DELETE FROM HHAUORDERTEST" +
      '  " WHERE codditt = " + CStrSQL(strDitta) +
      '  " And clientId = " + CStrSQL(strClientid) +
      '  " And orderId = " + CStrSQL(strOrderId)
      'Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

      'Imposta customerId
      strCustomerId = dsOrder.Tables("TESTA").Rows(0) !xx_conto.ToString.Replace(vbCrLf, "").PadLeft(100).Substring(0, 100).Trim

      'Imposta additionalHostData
      nCommessa = NTSCInt(dsOrder.Tables("TESTA").Rows(0) !et_commeca)
      strAdditionalHostData = nCommessa.ToString
      strUbicazione = getUbicazione(strDitta, nCommessa)
      If strUbicazione <> "" Then
        If strAdditionalHostData.Length + 3 < 50 Then
          strUbicazione = Mid(strUbicazione, 1, (50 - strAdditionalHostData.Length + 3))
          strAdditionalHostData += " / " + strUbicazione
        End If
      End If
      '-------------------------------------------------
      '--- Accoda testata Order
      '-------------------------------------------------
      strSQL =
        "INSERT INTO HHAUORDERTEST" +
        " (codditt, clientId, orderId, priority, additionalHostData, customerId, " +
        " earliestStartTime, latestStartTime, earliestStagingTime, latestStagingTime, " +
        " shippingTimeTarget, deliveryTime, customerOrderType, reason, " +
        " BUSStatus, BUSData, BUSOperazione, BUSEsito, " +
        " BUSTipork, BUSAnno, BUSSerie, BUSNumDoc, BUSProgressivo, BusTipo, " +
        " BUSDescrizione, BUSCliente, BUSNote, BUSSpedizione)" +
        " VALUES (" + CStrSQL(strDitta) + ", " + CStrSQL(strClientid) + ", " + CStrSQL(strOrderId) + ", " + CStrSQL(nPriorita.ToString) + ", " + CStrSQL(strAdditionalHostData) + ", " + CStrSQL(strCustomerId) +
        ", " + CDataSQL(Now.ToShortDateString) + ", " + CDataSQL(Now.ToShortDateString) + ", " + CDataSQL(Now.ToShortDateString) + ", " + CDataSQL(Now.ToShortDateString) +
        ", " + CDataSQL(Now.ToShortDateString) + ", " + CDataSQL(New Date(2099, 12, 31)) + ", 'shipping', Null" +
        ", Null, Null, Null, Null" +
        ", " + CStrSQL(strTipork) + ", " + nAnno.ToString + ", " + CStrSQL(strSerie) + ", " + lNumdoc.ToString + ", " + CStrSQL(nProgressivo.ToString) + ", " + CStrSQL(strBusTipo) +
        ", " + CStrSQL(strBUSDescrizione) + ", " + CStrSQL(strBUSCliente) + ", " + CStrSQL(strBUSNote) + ", " + CStrSQL(strBUSSpedizione) + ")"
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

      '-------------------------------------------------
      '--- Accoda righe Order
      '-------------------------------------------------
      'Ciclo di accodamento righe
      For Each dtrCorpo As DataRow In dttCorpoGr.Rows
        'Per sicurezza reinvio lo Sku cosi se articolo aggiornato da procedure esterne (import listini o query)
        'lo aggiorno du Autostore
        strCodart = NTSCStr(dtrCorpo!ec_codart)
        InserisciDatiSku(strDitta, strClientid, strCodart)

        nRiga += 1

        'Accoda riga order
        strSQL =
          "INSERT INTO HHAUORDERMOV" +
          " (codditt, clientId, orderId, orderLineId, hostOrderType" +
          ", criteriaUsed, locatioId, loadUnitId, finalDestinationLocation" +
          ", additionalHostData, holdOption, hostMoveReason, hostMoveType" +
          ", skuId, batch, hostStorageLocation, holdReasonId, fulfillmentrequirement" +
          ", quantityBaseTargetHost, quantityUnit, unlimitedOverdeliveryAllowed, overdeliveryTolerance, underdeliveryTolerance" +
          ", specialInventoryMark, specialInventoryReferenceId, bestBeforedate, minQuantityPerLU, roundingMethod, minimumRestQuantity)" +
          " VALUES (" + CStrSQL(strDitta) + ", " + CStrSQL(strClientid) + ", " + CStrSQL(strOrderId) + ", " + CStrSQL(nRiga.ToString.PadLeft(4, "0"c)) + ", Null" +
          ", 'INVENTORY_CRITERIA', Null, Null, Null" +
          ", Null, Null, Null, Null" +
          ", " + CStrSQL(strCodart) + ", 'BATCH', Null, Null, Null" +
          ", " + CDblSQL(dtrCorpo!ec_quant) + ", " + CStrSQL(dtrCorpo!ec_unmis) + ", 'false', '1', Null" +
          ", Null, Null, " + CDataSQL(Now.ToShortDateString) + ", Null, Null, Null)"
        Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
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
  ''' Trova Progressivo Order
  ''' </summary>
  Public Overridable Function getOrderProgressivo _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strTipork As String,
    ByVal nAnno As Integer,
    ByVal strSerie As String,
    ByVal lNumdoc As Integer
    ) As Integer
    Dim strSQL As String = ""
    Dim dtt As New DataTable
    Try
      strSQL =
        "SELECT TOP 1 * FROM HHAUORDERTEST" +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND BUSTipork = " + CStrSQL(strTipork) +
        " AND BUSAnno = " + nAnno.ToString +
        " AND BUSSerie = " + CStrSQL(strSerie) +
        " AND BUSNumDoc = " + lNumdoc.ToString +
        " ORDER BY BUSProgressivo DESC "
      dtt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dtt.Rows.Count <> 0 Then
        Return NTSCInt(dtt.Rows(0) !BUSProgressivo) + 1
      End If

      Return 0
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Function
#End Region

#Region "Funzioni e Routines per ricezione dati AUTOSTORE"
  ''' <summary>
  ''' Aggiorna Conferma Advice
  ''' </summary>
  Public Overridable Sub AggiornaConfermaAdvice _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal stradviceId As String,
    ByVal strConferma As String
    )
    Dim strSQL As String = ""
    Try
      '--------------------------------------------------
      '--- Aggiorna conferma e stato Advice 
      '--------------------------------------------------
      strSQL =
        "UPDATE HHAUADVICETEST" +
        " SET BUSFile = " + CStrSQL(strConferma) +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND adviceId = " + CStrSQL(stradviceId)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Aggiorna Conferma Advice Line
  ''' </summary>
  Public Overridable Sub AggiornaConfermaAdviceline _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strAdviceId As String,
    ByVal strAdviceLineId As String,
    ByVal nQtaConferma As Decimal,
    ByVal strBaia As String
    )
    Dim strSQL As String = ""
    Try
      '--------------------------------------------------
      '--- Aggiorna conferma e stato Advice Line
      '--------------------------------------------------
      strSQL =
        "UPDATE HHAUADVICEMOV" +
        " SET BUSQuantConfermata = " + CDblSQL(nQtaConferma) + ", " +
        " receivingLocationId = " + CStrSQL(strBaia) +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND adviceId = " + CStrSQL(strAdviceId) +
        " AND adviceLineId = " + CStrSQL(strAdviceLineId)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Aggiorna Conferma Order
  ''' </summary>
  Public Overridable Sub AggiornaConfermaOrder _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strorderId As String,
    ByVal strConferma As String
    )
    Dim strSQL As String = ""
    Try
      '--------------------------------------------------
      '--- Aggiorna conferma e stato Order 
      '--------------------------------------------------
      strSQL =
        "UPDATE HHAUORDERTEST" +
        " SET BUSFile = " + CStrSQL(strConferma) +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND orderId = " + CStrSQL(strorderId)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Aggiorna Conferma Order Line
  ''' </summary>
  Public Overridable Sub AggiornaConfermaOrderline _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strorderId As String,
    ByVal strorderLineId As String,
    ByVal nQtaConferma As Decimal
    )
    Dim strSQL As String = ""
    Try
      '--------------------------------------------------
      '--- Aggiorna conferma e stato Order Line
      '--------------------------------------------------
      strSQL =
        "UPDATE HHAUORDERMOV" +
        " SET BUSQuantConfermata = " + CDblSQL(nQtaConferma) +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND orderId = " + CStrSQL(strorderId) +
        " AND orderLineId = " + CStrSQL(strorderLineId)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Aggiorna stato Order 
  ''' </summary>
  Public Overridable Function AggiornaStatoOrder _
    (
    ByVal strDitta As String,
    ByVal strClientId As String,
    ByVal strorderId As String,
    ByVal strStato As String
    ) As Boolean
    Dim strSQL As String = ""
    Try
      '--------------------------------------------------
      '--- Aggiorna stato Order
      '--------------------------------------------------
      strSQL =
        "UPDATE HHAUORDERTEST" +
        " SET BUSStatus = " + CStrSQL(strStato) +
        " WHERE codditt = " + CStrSQL(strDitta) +
        " AND clientId = " + CStrSQL(strClientId) +
        " AND orderId = " + CStrSQL(strorderId)
      Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
      Return True
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Function getImage(strCodArt As String) As DataTable
    Try
      Dim strSQL As String = ""
      Dim dtt As New DataTable

      strSQL = "SELECT * FROM allole " &
               " WHERE ao_tipo = 'A' " &
               " AND ao_argom = 'PS' " &
               " AND SUBSTRING(ao_nomedoc,1,1) = '\' " &
               " AND ao_strcod = " & CStrSQL(strCodArt)
      dtt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      Return dtt
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function
#End Region

  'AAAAAAAAAAAAAAAAAAAAAA

#Region "Funzioni e Routines di generazione Depositi/Prelievi AUTOSTORE"
  ''' <summary>
  ''' CUSTOM
  ''' Personalizzazione per invio dati ad AUTOSTORE
  ''' Verifica se il documento contiene righe gestite su magazzino Autostore
  ''' </summary>
  Public Overridable Function VerificaAdviceAutostore _
    (
    ByVal strDitta As String,
    ByVal dsDocRic As DataSet,
    ByVal nTipoRaggruppamentoRicm As Integer,
    ByRef dttArticoli As DataTable
    ) As Boolean

    'Dichiarazione variabili
    Dim strTipork As String = ""
    Dim nAnno As Integer = 0
    Dim strSerie As String = ""
    Dim lNumdoc As Integer = 0
    Dim strSQL As String = ""

    Try
      '--------------------------------------------------
      '--- Verifico se il documento contiene articoli AUTOSTORE 
      '--- su magazzini di carico merce che prevedono il trasferimento a MA
      '--- di commesse per cui non deve essere fatto lo scarico immediato
      '--------------------------------------------------
      With dsDocRic.Tables("TESTA").Rows(0)
        strTipork = NTSCStr(!et_tipork)
        nAnno = NTSCInt(!et_anno)
        strSerie = NTSCStr(!et_serie)
        lNumdoc = NTSCInt(!et_numdoc)
      End With

      strSQL =
        "SELECT movmag.mm_riga, movmag.mm_codart, movmag.mm_commeca, artico.ar_gesubic," +
        " ISNULL(commess.co_hhflscaricoimmediatowms, 'N') AS co_hhflscaricoimmediatowms," +
        " ISNULL(commess.co_stato, '4') AS co_stato, " +
        " ISNULL((" +
        " SELECT COUNT(M.codditt)" +
        " FROM movprb AS M" +
        " INNER JOIN commess AS C" +
        " ON M.codditt = C.codditt" +
        " AND M.mm_commeca = C.co_comme" +
        " INNER JOIN HHCOTipologie AS T" +
        " ON C.codditt = T.codditt" +
        " AND C.co_hhtipologia = T.tp_tipologia" +
        " WHERE C.co_stato <> '5'" +
        " AND T.tp_flinlavorazione = 'S'" +
        " AND C.co_comme = commess.co_comme" +
        " ),0) AS co_hhflinproduzionewms" +
        " FROM movmag" +
        " INNER JOIN artico" +
        " ON movmag.codditt = artico.codditt" +
        " AND movmag.mm_codart = artico.ar_codart" +
        " INNER JOIN tabmaga" +
        " ON movmag.codditt = tabmaga.codditt" +
        " AND movmag.mm_magaz = tabmaga.tb_codmaga" +
        " LEFT JOIN commess" +
        " ON movmag.codditt = commess.codditt" +
        " AND movmag.mm_commeca = commess.co_comme" +
        " WHERE movmag.codditt = " + CStrSQL(strDitta) +
        " AND movmag.mm_tipork = " + CStrSQL(strTipork) +
        " AND movmag.mm_anno = " + nAnno.ToString +
        " AND movmag.mm_serie = " + CStrSQL(strSerie) +
        " AND movmag.mm_numdoc = " + lNumdoc.ToString +
        " AND artico.ar_hhflmagautomatico = 'A'" +
        " AND tabmaga.tb_hhfltrasferimentoma = 'S'" +
        " AND ISNULL(commess.co_hhflscaricoimmediatowms, 'N') = 'N'"
      dttArticoli = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      '-------------------------------------------------
      '--- In base al tipo di raggruppamento righe RICM 
      '--- elimina le righe che non devono rientrare nella BMI
      '-------------------------------------------------
      '0 = Nessun ragruppamento
      '1 = Commesse Chiuse
      '2 = Commesse Lavori in corso
      '3 = Commesse Chiuse e Lavori in corso
      '4 = Commesse Scarico Immediato (Scarichi anticipati / Materiale rotto)
      '5 = Commesse Chiuse e Commesse scarico Immediato
      '6 = Commesse Lavori in Corso e Commesse scarico Immediato
      '7 = Commesse chiuse Commesse Lavori in corso e Commesse scarico Immediato
      nTipoRaggruppamentoRicm = NTSCInt(GetSettingBus("BSRMRICM", "OPZIONI", ".", "HH_TipoRaggruppamentoRicm", "5", ".", "5"))

      Dim strSelect As String = ""
      Select Case nTipoRaggruppamentoRicm
        Case 1
          strSelect = "co_stato = '5'"
        Case 2
          strSelect = "co_hhflinproduzionewms > 0"
        Case 3
          strSelect = "co_stato = '5' OR co_hhflinproduzionewms > 0"
        Case 4
          strSelect = "co_hhflscaricoimmediatowms = 'S'"
        Case 5
          strSelect = "co_stato = '5' OR co_hhflscaricoimmediatowms = 'S'"
        Case 6
          strSelect = "co_hhflinproduzionewms > 0 OR co_hhflscaricoimmediatowms = 'S'"
        Case 7
          strSelect = "co_stato = '5' OR co_hhflinproduzionewms > 0 OR co_hhflscaricoimmediatowms = 'S'"
      End Select
      If strSelect.Trim.Length > 0 Then
        For Each dtrArticolo As DataRow In dttArticoli.Select(strSelect)
          dtrArticolo.Delete()
        Next
        dttArticoli.AcceptChanges()
      End If

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

  Public Overridable Function getUbicazione(strDitta As String, nCommessa As Integer) As String
    Try
      Dim strSQL As String = ""
      Dim dtt As New DataTable

      strSQL = "SELECT co_hhnoteubicazione FROM commess " +
                " WHERE codditt = " + CStrSQL(strDitta) +
                " AND co_comme = " + nCommessa.ToString

      dtt = OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI)

      If dtt.Rows.Count <> 0 Then
        Return NTSCStr(dtt.Rows(0) !co_hhnoteubicazione)
      End If

      Return ""
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function
#End Region

End Class

