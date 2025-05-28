Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports NTSInformatica.CLN__STD
Imports System.IO
Imports System.Xml

Public Class CLEHHMOIN
  Inherits CLE__BASE

  Public oCldMoin As CLDHHMOIN
  Public dttAnaz As New DataTable
  Public strMacchina As String = ""
  Public strMsg As String = ""

#Region "Oggetto connessione VEBOLL"
  Public oCleBoll As CLEVEBOLL
#End Region

#Region "Oggetto connessione HHINTE"
  Public oCleInte As CLEHHINTE
#End Region

#Region "Oggetto Aggiornamento Quantità"
  Public listLav As New Dictionary(Of String, String)
#End Region

#Region "Variabili Reistro"
  'Public nConto As Integer = 0
  Public nTipobf As Integer = 0
  Public pathDef As String = ""
#End Region

#Region "Variabili"
  Public dtMacchineInvio As New DataTable
  Public dtMacchineRicevi As New DataTable
#End Region


#Region "Moduli"
  Private Moduli_P As Integer = bsModAll
  Private ModuliExt_P As Integer = 0
  Private ModuliSup_P As Integer = 0
  Private ModuliSupExt_P As Integer = 0
  Private ModuliPtn_P As Integer = 0
  Private ModuliPtnExt_P As Integer = 0

  Public ReadOnly Property Moduli() As Integer
    Get
      Return Moduli_P
    End Get
  End Property
  Public ReadOnly Property ModuliExt() As Integer
    Get
      Return ModuliExt_P
    End Get
  End Property
  Public ReadOnly Property ModuliSup() As Integer
    Get
      Return ModuliSup_P
    End Get
  End Property
  Public ReadOnly Property ModuliSupExt() As Integer
    Get
      Return ModuliSupExt_P
    End Get
  End Property
  Public ReadOnly Property ModuliPtn() As Integer
    Get
      Return ModuliPtn_P
    End Get
  End Property
  Public ReadOnly Property ModuliPtnExt() As Integer
    Get
      Return ModuliPtnExt_P
    End Get
  End Property
#End Region

#Region "Inizializzazione"

  Public Overrides Function Init(ByRef App As CLE__APP,
                                ByRef oScriptEngine As INT__SCRIPT, ByRef oCleLbmenu As Object, ByVal strTabella As String,
                                ByVal bFiller1 As Boolean, ByVal strFiller1 As String,
                                ByVal strFiller2 As String) As Boolean

    If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHMOIN"
    MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")

    oCldMoin = CType(MyBase.ocldBase, CLDHHMOIN)
    oCldMoin.Init(oApp)
    Dim strErr As String = ""
    Dim oTmp As Object = Nothing

    Return True

  End Function

  Public Overridable Function IstanziaVEBoll() As Boolean
    Try
      '-------------------------------------------------
      '--- Inizializzo il componente
      '-------------------------------------------------
      If oCleBoll IsNot Nothing Then Return True

      Dim strErr As String = ""
      Dim oTmp As Object = Nothing
      If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BEHHMOIN", "BEVEBOLL", oTmp, strErr, False, "", "") = False Then
        Throw New NTSException(oApp.Tr(Me, 128607611686875000, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
        Return False
      End If
      oCleBoll = CType(oTmp, CLEVEBOLL)

      AddHandler oCleBoll.RemoteEvent, AddressOf GestisciEventiEntityVEBoll
      If oCleBoll.Init(oApp, oScript, oCleComm, "", False, "", "") = False Then Return False
      If Not oCleBoll.InitExt() Then Return False
      oCleBoll.strProgChiamante = "BEHHMOIN"
      oCleBoll.bModuloCRM = False
      oCleBoll.bIsCRMUser = False

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Function

  Public Overridable Function IstanziaHHInte() As Boolean
    Try
      '-------------------------------------------------
      '--- Inizializzo il componente
      '-------------------------------------------------
      If oCleInte IsNot Nothing Then Return True

      Dim strErr As String = ""
      Dim oTmp As Object = Nothing
      If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BEHHMOIN", "BEHHINTE", oTmp, strErr, False, "", "") = False Then
        Throw New NTSException(oApp.Tr(Me, 128607611686875000, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
        Return False
      End If
      oCleInte = CType(oTmp, CLEHHINTE)

      AddHandler oCleInte.RemoteEvent, AddressOf GestisciEventiEntityHHInte
      If oCleInte.Init(oApp, oScript, oCleComm, "", False, "", "") = False Then Return False

      Return True

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Function

  Public Overridable Sub GestisciEventiEntityVEBoll(ByVal sender As Object, ByRef e As NTSEventArgs)
    Try
      Select Case UCase(e.TipoEvento)
        Case "SET_FOCUS"
        Case CLN__STD.ThMsg.MSG_YESNO, CLN__STD.ThMsg.MSG_NOYES, CLN__STD.ThMsg.MSG_NOYESCANC, CLN__STD.ThMsg.MSG_YESNOCANC
          e.RetValue = CLN__STD.ThMsg.RETVALUE_YES
        Case Else
          If Trim(e.Message) <> "" Then
            LogWrite(e.Message, True)
          End If
      End Select
      'ThrowRemoteEvent(e)
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub GestisciEventiEntityHHInte(ByVal sender As Object, ByRef e As NTSEventArgs)
    Try
      Select Case UCase(e.TipoEvento)
        Case "SET_FOCUS"
        Case Else
          If Trim(e.Message) <> "" Then
            LogWrite(e.Message, True)
          End If
      End Select
      'ThrowRemoteEvent(e)
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

  Public Overridable Function LeggiDatiDitta(ByVal strDitta As String) As Boolean
    Dim dttTmp As New DataTable

    Try
      '--------------------------------------------------------------------------------------------------------------
      oCldMoin.ValCodiceDb(strDitta, strDitta, "TABANAZ", "S", "", dttAnaz)
      strDittaCorrente = strDitta
      '--------------------------------------------------------------------------------------------------------------
      Return True
      '--------------------------------------------------------------------------------------------------------------
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return False
    End Try
  End Function

  Public Overridable Sub LeggiRegistro()

    Try
      '---------------------------------
      'leggo le opzioni di registro globali
      pathDef = NTSCStr(oCldMoin.GetSettingBusDitt(strDittaCorrente, "BSHHGEMA", "OPZIONI", ".", "PathDef", "0", " ", "0"))

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub LeggiTesta(ByVal strSel As String, ByVal dtOpz As DataTable)
    Try
      'lancio la procedura di caricamento Testa in griglia.
      oCldMoin.LeggiTesta(strDittaCorrente, strSel, dtOpz, dsShared)
      AddHandler dsShared.Tables("TESTA").ColumnChanging, AddressOf BeforeColUpdateTesta
      AddHandler dsShared.Tables("TESTA").ColumnChanged, AddressOf AfterColUpdateTesta
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub LeggiCorpo(nID As Integer, strTipo As String)
    Try
      'lancio la procedura di caricamento Corpo in griglia.
      oCldMoin.LeggiCorpo(strDittaCorrente, nID, strTipo, dsShared)

      If strTipo <> "1" Then
        dsShared.Tables("CORPO").Columns.Add("backcolor_actualQuantity")
        For Each drCorpo As DataRow In dsShared.Tables("CORPO").Rows
          If NTSCDec(drCorpo!actualQuantity) <> NTSCDec(drCorpo!nominalQuantity) Then
            drCorpo!backcolor_actualQuantity = Color.Red.ToArgb()
          Else
            drCorpo!backcolor_actualQuantity = Color.Green.ToArgb()
          End If
        Next
      End If

      AddHandler dsShared.Tables("CORPO").ColumnChanging, AddressOf BeforeColUpdateCorpo
      AddHandler dsShared.Tables("CORPO").ColumnChanged, AddressOf AfterColUpdateCorpo
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub GestisciTimerElabora()
    Try
      Dim dsInte As New DataSet

      oCleInte.Apri(dsInte)

      If dsInte Is Nothing Then Return

      If dsInte.Tables("TESTA") Is Nothing Then Return

      If dsInte.Tables("TESTA").Rows.Count = 0 Then Return

      Dim drTesta As DataRow = dsInte.Tables("TESTA").Rows(0)

      If NTSCStr(drTesta!tb_elaborazioneautom) <> "S" Then
        ThrowRemoteEvent(New NTSEventArgs("TIMER_ELABORA_INTERVAL", NTSCStr(1000)))
        Return
      End If

      Dim nInterval As Integer = NTSCInt(drTesta!tb_secelaborazione) * 1000

      ThrowRemoteEvent(New NTSEventArgs("TIMER_ELABORA_INTERVAL", NTSCStr(nInterval)))

      Scarica()

      Dim dtOpz As New DataTable
      setDtOpz(dtOpz)
      dtOpz.Rows.Add("-1", 0, "", 0, "", "", "", "", 0, "")

      LeggiTesta("S", dtOpz)

      Elabora()

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub GestisciElabora()
    Try

      'File.AppendAllText("F:\BUS\ASC\LogHHMOIN.txt", "Pre Elabora" & vbCrLf)

      Try
        Scarica()
      Catch ex As Exception

      End Try

      Dim dtOpz As New DataTable
      setDtOpz(dtOpz)
      dtOpz.Rows.Add("-1", 0, "", 0, "", "", "", "", 0, "")

      LeggiTesta("S", dtOpz)

      Elabora()

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub GestisciTimerGiacenze()
    Try
      Dim dsInte As New DataSet
      Dim strErrore As String = ""
      Dim dsGiac As New DataSet
      Dim strOra1 As String = ""
      Dim strOra2 As String = ""
      Dim strOra3 As String = ""
      Dim strOra As String = DateTime.Now.ToShortTimeString()

      oCleInte.Apri(dsInte)

      If dsInte Is Nothing Then Return

      If dsInte.Tables("TESTA") Is Nothing Then Return

      If dsInte.Tables("TESTA").Rows.Count = 0 Then Return

      Dim drTesta As DataRow = dsInte.Tables("TESTA").Rows(0)

      ThrowRemoteEvent(New NTSEventArgs("TIMER_GIACENZA_INTERVAL", NTSCStr(1000)))

      If NTSCStr(drTesta!tb_elaborazionegiac1) = "S" Then
        strOra1 = NTSCStr(drTesta!tb_oragiac1)
      End If

      If NTSCStr(drTesta!tb_elaborazionegiac2) = "S" Then
        strOra2 = NTSCStr(drTesta!tb_oragiac2)
      End If

      If NTSCStr(drTesta!tb_elaborazionegiac3) = "S" Then
        strOra3 = NTSCStr(drTesta!tb_oragiac3)
      End If

      If strOra1 = strOra Or strOra2 = strOra Or strOra3 = strOra Then
        oCleInte.LeggiGiacenze(strDittaCorrente, strErrore, dsGiac)
        Dim dtGiac As DataTable = oCldMoin.getDataTableGiacenze()
        dtGiac.Columns("tb_dataora").DefaultValue = DateTime.Now().ToString()
        Dim strData As String = DateTime.Now().ToShortDateString()
        Dim nID As Long = oCldMoin.getIDGiac(NTSCDate(strData))
        dtGiac.Columns("tb_id").DefaultValue = nID
        dtGiac.Columns.Remove("id")
        For Each drGiac As DataRow In dsGiac.Tables("article").Rows
          dtGiac.ImportRow(drGiac)
        Next
        oCldMoin.SalvaGiacenze(dtGiac)
        ThrowRemoteEvent(New NTSEventArgs("TIMER_GIACENZA_INTERVAL", NTSCStr(60000)))
      End If

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub GestisciGiacenze()
    Try
      Dim strErrore As String = ""
      Dim dsGiac As New DataSet

      oCleInte.LeggiGiacenze(strDittaCorrente, strErrore, dsGiac)
      Dim dtGiac As DataTable = oCldMoin.getDataTableGiacenze()
      dtGiac.Columns("tb_dataora").DefaultValue = DateTime.Now().ToString()
      Dim strData As String = DateTime.Now().ToShortDateString()
      Dim nID As Long = oCldMoin.getIDGiac(NTSCDate(strData))
      dtGiac.Columns("tb_id").DefaultValue = nID
      dtGiac.Columns.Remove("id")
      For Each drGiac As DataRow In dsGiac.Tables("article").Rows
        dtGiac.ImportRow(drGiac)
      Next
      oCldMoin.SalvaGiacenze(dtGiac)

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub setDtOpz(ByRef dtOpz As DataTable)
    Try
      dtOpz = New DataTable
      dtOpz.Columns.Add("Tipork", GetType(String))
      dtOpz.Columns.Add("Anno", GetType(Integer))
      dtOpz.Columns.Add("Serie", GetType(String))
      dtOpz.Columns.Add("Numero", GetType(Integer))
      dtOpz.Columns.Add("DataDaDoc", GetType(String))
      dtOpz.Columns.Add("DataADoc", GetType(String))
      dtOpz.Columns.Add("DataDaJob", GetType(String))
      dtOpz.Columns.Add("DataAJob", GetType(String))
      dtOpz.Columns.Add("Conto", GetType(Integer))
      dtOpz.Columns.Add("Articolo", GetType(String))
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Function formatoData(strValore As String) As String
    Dim sReturn As String = ""
    Try
      If strValore = "" Then Return ""
      Try
        Dim sData As DateTime = NTSCDate(strValore)
        Return sData.Day.ToString("00") & sData.Month.ToString("00") & Mid(sData.Year.ToString(), 3, 2)
      Catch ex As Exception
        Return ""
      End Try
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
    Return sReturn
  End Function

  Public Overridable Function CancellaRiga(nID As Integer, strTipo As String) As Boolean
    Try
      Return oCldMoin.CancellaRiga(strDittaCorrente, nID, strTipo)
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Function

  Public Overridable Function Scarica() As Boolean
    Try
      Dim strErrore As String = ""
      IstanziaHHInte()
      oCleInte.LeggiOrdiniElaborati(strDittaCorrente, strErrore)
      If strErrore <> "" Then
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, strErrore))
        strErrore = ""
      End If
      oCleInte.LeggiOperazioniManuali(strDittaCorrente, strErrore)
      If strErrore <> "" Then
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, strErrore))
      End If
      Return True
    Catch ex As Exception
      'CLN__STD.GestErr(ex, Me, "")
      Return False
    End Try
  End Function

  Public Overridable Function Elabora() As Boolean
    Try
      Dim nProgress As Integer = 0
      Dim nDocumento As Integer = 0
      Dim nDocumenti As Integer = 0
      Dim strTipork As String = "Z"
      Dim nAnno As Integer = NTSCInt(Date.Now.Year)
      Dim lNum As Integer = 0
      Dim strSerie As String = ""
      Dim dsBoll As New DataSet
      Dim dsInte As New DataSet
      Dim nCausaleCarcio As Integer = 0
      Dim nCausaleScarcio As Integer = 0
      Dim nMagazMacchina As Integer = 0
      Dim nMagazPrincipale As Integer = 0
      Dim strOperazione As String = ""
      Dim listID As New List(Of Integer)
      Dim bError As Boolean = False

      dsShared.AcceptChanges()

      If dsShared.Tables("TESTA").Select("xx_sel = 'S'").Length = 0 Then
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_INFO, oApp.Tr(Me, 128768973837496061, "Attenzione! Non hai selezionato nessuna riga")))
        Return False
      End If

      IstanziaVEBoll()
      IstanziaHHInte()

      If Not LogStart("BNHHMOIN", "Crea Documenti" & vbNewLine, False) Then Return False

      If Not oCleInte.Apri(dsInte) Then
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, "Errore nell'apertura dei settaggi"))
        Return False
      End If

      strSerie = NTSCStr(dsInte.Tables("TESTA").Rows(0) !tb_serie)
      If strSerie = "" Then strSerie = " "

      nCausaleCarcio = NTSCInt(dsInte.Tables("TESTA").Rows(0) !tb_causalecarico)
      nCausaleScarcio = NTSCInt(dsInte.Tables("TESTA").Rows(0) !tb_causalescarico)

      If nCausaleCarcio = 0 Then
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, "Non è stata parametrizzata una causale di carico nelle opzioni"))
        'LogWrite("Non è stata parametrizzata una causale di carico nelle opzioni", True)
        Return False
      End If

      If nCausaleScarcio = 0 Then
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, "Non è stata parametrizzata una causale di scarico nelle opzioni"))
        'LogWrite("Non è stata parametrizzata una causale di scarico nelle opzioni", True)
        Return False
      End If

      'lNum = oCleBoll.LegNuma(strTipork, strSerie, nAnno)
      'If lNum = 0 Then
      '  'oApp.MsgBoxInfo(oApp.Tr(Me, 129877042710458543, "Errore nella numerazione"))
      '  ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, "Errore nella numerazione"))
      '  'LogWrite("Errore nella numerazione", True)
      '  Return False
      'End If

      'If Not oCleBoll.ApriDoc(strDittaCorrente, True, strTipork, nAnno, strSerie, lNum, dsBoll) Then
      '  'oApp.MsgBoxInfo(oApp.Tr(Me, 129877042710458543, "Errore nell'apertura di un nuovo documento"))
      '  ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, "Errore nell'apertura di un nuovo documento"))
      '  'LogWrite("Errore nell'apertura di un nuovo documento", True)
      '  Return False
      'End If

      'If Not oCleBoll.NuovoDocumento(strDittaCorrente, strTipork, nAnno, strSerie, lNum, "") Then
      '  'oApp.MsgBoxInfo(oApp.Tr(Me, 129877042710458543, "Errore nella creazione di un nuovo documento"))
      '  ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, "Errore nella creazione di un nuovo documento"))
      '  'LogWrite("Errore nella creazione di un nuovo documento", True)
      '  Return False
      'End If

      'With dsBoll.Tables("TESTA").Rows(0)
      '  !codditt = strDittaCorrente
      '  !et_conto = NTSCInt(dsInte.Tables("TESTA").Rows(0) !tb_conto)
      '  !et_tipobf = NTSCInt(dsInte.Tables("TESTA").Rows(0) !tb_tipoBF)
      '  !et_riferim = "MOVIMENTI INCARICO TECH"
      'End With

      nDocumenti = dsShared.Tables("TESTA").Select("xx_sel = 'S'").Length

      listID.Clear()

      For Each dr As DataRow In dsShared.Tables("TESTA").Select("xx_sel = 'S'")
        nDocumento += 1
        If nDocumento.Equals(nDocumenti) Then
          nProgress = 100
        Else
          nProgress = NTSCInt((nDocumento * 100) / nDocumenti)
        End If

        If Not oApp.Batch Then ThrowRemoteEvent(New NTSEventArgs("PROGRESS_BAR_POS", nProgress.ToString))
        ThrowRemoteEvent(New NTSEventArgs("INFO_LABEL", oApp.Tr(Me, 128768973837496061, "Elaborazione Documento " & NTSCStr(dr!jobNumber) & " in corso...")))

        nAnno = NTSCInt(NTSCDate(dr!jobDataOra).Year)

        lNum = oCleBoll.LegNuma(strTipork, strSerie, nAnno)
        If lNum = 0 Then
          'oApp.MsgBoxInfo(oApp.Tr(Me, 129877042710458543, "Errore nella numerazione"))
          ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, "Errore nella numerazione"))
          'LogWrite("Errore nella numerazione", True)
          Return False
        End If

        If Not oCleBoll.ApriDoc(strDittaCorrente, True, strTipork, nAnno, strSerie, lNum, dsBoll) Then
          'oApp.MsgBoxInfo(oApp.Tr(Me, 129877042710458543, "Errore nell'apertura di un nuovo documento"))
          ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, "Errore nell'apertura di un nuovo documento"))
          'LogWrite("Errore nell'apertura di un nuovo documento", True)
          Return False
        End If

        If Not oCleBoll.NuovoDocumento(strDittaCorrente, strTipork, nAnno, strSerie, lNum, "") Then
          'oApp.MsgBoxInfo(oApp.Tr(Me, 129877042710458543, "Errore nella creazione di un nuovo documento"))
          ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, "Errore nella creazione di un nuovo documento"))
          'LogWrite("Errore nella creazione di un nuovo documento", True)
          Return False
        End If

        If NTSCStr(dr!busTipork) <> "1" Then
          If dsInte.Tables("CORPO").Select("tb_tipork = " & CStrSQL(dr!busTipork) & " AND tb_serie = " & CStrSQL(dr!busSerie)).Length = 0 Then
            If dsInte.Tables("CORPO").Select("tb_tipork = " & CStrSQL(dr!busTipork) & " AND tb_serie = ''").Length = 0 Then
              LogWrite("Non è stato parametrizzato il tipo " & NTSCStr(dr!busTipork) & " nelle opzioni della Macchina", True)
              Continue For
            Else
              nMagazMacchina = NTSCInt(dsInte.Tables("CORPO").Select("tb_tipork = " & CStrSQL(dr!busTipork) & " AND tb_serie = ''")(0) !tb_magazMacchina)
              nMagazPrincipale = NTSCInt(dsInte.Tables("CORPO").Select("tb_tipork = " & CStrSQL(dr!busTipork) & " AND tb_serie = ''")(0) !tb_magazPrincipale)
              strOperazione = NTSCStr(dsInte.Tables("CORPO").Select("tb_tipork = " & CStrSQL(dr!busTipork) & " AND tb_serie = ''")(0) !tb_operazione)
            End If
          Else
            nMagazMacchina = NTSCInt(dsInte.Tables("CORPO").Select("tb_tipork = " & CStrSQL(dr!busTipork) & " AND tb_serie = " & CStrSQL(dr!busSerie))(0) !tb_magazMacchina)
            nMagazPrincipale = NTSCInt(dsInte.Tables("CORPO").Select("tb_tipork = " & CStrSQL(dr!busTipork) & " AND tb_serie = " & CStrSQL(dr!busSerie))(0) !tb_magazPrincipale)
            strOperazione = NTSCStr(dsInte.Tables("CORPO").Select("tb_tipork = " & CStrSQL(dr!busTipork) & " AND tb_serie = " & CStrSQL(dr!busSerie))(0) !tb_operazione)
          End If
        Else
          nMagazMacchina = NTSCInt(dsInte.Tables("TESTA").Rows(0) !tb_magazzinoMacchina)
          nMagazPrincipale = NTSCInt(dsInte.Tables("TESTA").Rows(0) !tb_magazzinoPrincipale)
          strOperazione = "+"
        End If

        If nMagazMacchina = 0 Then
          LogWrite("Non è stata parametrizzata un Magazzino Macchina nelle opzioni per il tipo Doc. " & NTSCStr(dr!busTipork), True)
          Continue For
        End If

        If nMagazPrincipale = 0 Then
          LogWrite("Non è stata parametrizzata un Magazzino Principale nelle opzioni per il tipo Doc. " & NTSCStr(dr!busTipork), True)
          Continue For
        End If

        With dsBoll.Tables("TESTA").Rows(0)
          !codditt = strDittaCorrente
          !et_conto = NTSCInt(dsInte.Tables("TESTA").Rows(0) !tb_conto)
          !et_tipobf = NTSCInt(dsInte.Tables("TESTA").Rows(0) !tb_tipoBF)
          !et_datdoc = NTSCDate(dr!jobDataOra)
          '!et_riferim = "MOVIMENTI INCARICO TECH"
          Select Case strOperazione
            Case "+"
              !et_riferim = "VERSAMENTO " & NTSCStr(dr!jobNumber)
            Case "-"
              !et_riferim = "PRELIEVO " & NTSCStr(dr!jobNumber)
          End Select
        End With

        Dim dsCorpo As New DataSet
        oCldMoin.LeggiCorpo(strDittaCorrente, NTSCInt(dr!id), NTSCStr(dr!busTipork), dsCorpo)

        bError = False
        For Each drCorpo As DataRow In dsCorpo.Tables("CORPO").Rows
          If NTSCStr(dr!busTipork) = "1" Then
            strOperazione = NTSCStr(drCorpo!OPERATION)
            Select Case strOperazione
              Case "+", "p"
                dsBoll.Tables("TESTA").Rows(0) !et_riferim = "VERSAMENTO " & NTSCStr(dr!jobNumber)
              Case "-", "c"
                dsBoll.Tables("TESTA").Rows(0) !et_riferim = "PRELIEVO " & NTSCStr(dr!jobNumber)
            End Select
          End If

          If Not AggiungiRigaDocumento(drCorpo, dsBoll, nCausaleCarcio, nCausaleScarcio, nMagazMacchina, nMagazPrincipale) Then
            bError = True
            Exit For
          End If
        Next

        Dim oClfBoll As CLFVEBOLL = CType(oCleBoll, CLFVEBOLL)

        oClfBoll.bMoin = True
        'If Not bError Then listID.Add(NTSCInt(dr!id))
        If dsBoll.Tables("CORPO").Rows.Count <> 0 Then
          If Not oCleBoll.SalvaDocumento("N") Then
            LogWrite("Errore nel salvataggio del documento " & NTSCStr(dr!jobNumber), True)
          Else
            If Not bError Then
              oCldMoin.AggiornaJob(strDittaCorrente, NTSCInt(dr!id), NTSCStr(dr!busTipork))
            End If
          End If
        End If

      Next

      'If Not oCleBoll.SalvaDocumento("N") Then
      '  LogWrite("Errore nel salvataggio del documento", True)
      'Else
      '  For Each nID As Integer In listID
      '    oCldMoin.AggiornaJob(strDittaCorrente, nID)
      '  Next
      'End If

      LogStop()
      oApp.MsgBoxInfo(oApp.Tr(Me, 129877042710458543, "Elaborazione Terminata"))

      ThrowRemoteEvent(New NTSEventArgs("INFO_LABEL", oApp.Tr(Me, 128768973837496061, "Nessuna operazione In corso")))

      'Imposta label e ProgressBar di elaborazione
      If Not oApp.Batch Then
        ThrowRemoteEvent(New NTSEventArgs("PROGRESS_BAR_POS", "0"))
      End If

      If nCountLog > 0 And Not oApp.Batch Then
        oApp.MsgBoxInfo(oApp.Tr(Me, 129877042710458543, "Ci sono stati dei problemi con l'elborazione "))
        System.Diagnostics.Process.Start("notepad", LogFileName)
        Return False
      End If

      Return True
    Catch ex As Exception
      LogStop()
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function RettificaGiacenze() As Boolean
    Try
      Dim dsGiac As New DataSet
      Dim strErrore As String = ""
      Dim strTesto As String = ""

      IstanziaHHInte()
      oCleInte.LeggiGiacenze(strDittaCorrente, strErrore, dsGiac)

      If dsGiac Is Nothing Then Return False
      If dsGiac.Tables("article") Is Nothing Then Return False

      For Each dr As DataRow In dsGiac.Tables("article").Rows
        If NTSCDec(dr!inventoryAtStorageLocation) <> 0 Then
          strTesto &= NTSCStr(dr!articleNumber) & ";" & NTSCStr(dr!inventoryAtStorageLocation) & vbCrLf
        End If
      Next

      If strTesto <> "" Then
        File.WriteAllText(oApp.DesktopDir & "\RETTIFICA_GIACENZE.csv", strTesto)
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_INFO, "File Rettifica Giacenze Creato"))
        System.Diagnostics.Process.Start("Excel", oApp.DesktopDir & "\RETTIFICA_GIACENZE.csv")
      End If

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function getMagazzinoMacchina() As Integer
    Try
      IstanziaHHInte()
      Return oCleInte.getMagazzinoMacchina()
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Sub LeggiConfrontoGiacenze(ByVal strFCodArt As String, ByRef dsOut As DataSet)
    Try
      Dim strCodArt As String = ""
      Dim nMagaz As Integer = 0
      Dim strErrore As String = ""
      Dim dsGiac As New DataSet
      Dim strDescr As String = ""
      Dim strSelect As String = ""

      IstanziaHHInte()
      nMagaz = oCleInte.getMagazzinoMacchina
      oCleInte.LeggiGiacenze(strDittaCorrente, strErrore, dsGiac)

      dsOut.Tables.Add("GIAC")
      dsOut.Tables("GIAC").Columns.Add("tb_articolo", GetType(String))
      dsOut.Tables("GIAC").Columns.Add("tb_descrart", GetType(String))
      dsOut.Tables("GIAC").Columns.Add("tb_quantmacchina", GetType(Decimal))
      dsOut.Tables("GIAC").Columns.Add("tb_quantbus", GetType(Decimal))
      dsOut.Tables("GIAC").Columns.Add("backcolor_tb_quantbus")

      If strFCodArt <> "" Then
        strSelect = "articleNumber = " & CStrSQL(strFCodArt)
      End If

      For Each drGiac As DataRow In dsGiac.Tables("article").Select(strSelect)
        strCodArt = NTSCStr(drGiac!articleNumber)
        strDescr = ""
        oCldMoin.ValCodiceDb(strCodArt, strDittaCorrente, "artico", "S", strDescr)
        Dim drOut As DataRow = dsOut.Tables("GIAC").NewRow
        drOut!tb_articolo = strCodArt
        drOut!tb_descrart = strDescr
        drOut!tb_quantmacchina = NTSCDec(drGiac!inventoryAtStorageLocation)
        drOut!tb_quantbus = oCldMoin.getGiacenzaBus(strDittaCorrente, strCodArt, nMagaz)
        If NTSCDec(drOut!tb_quantmacchina) <> NTSCDec(drOut!tb_quantbus) Then
          drOut!backcolor_tb_quantbus = Color.Red.ToArgb
        Else
          drOut!backcolor_tb_quantbus = Color.Green.ToArgb
        End If
        dsOut.Tables("GIAC").Rows.Add(drOut)
      Next
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Function AggiungiRigaDocumento(drJob As DataRow, dsBoll As DataSet, nCausaleCarico As Integer, nCausaleScarico As Integer, nMagazMaccina As Integer, nMagazPrincipale As Integer) As Boolean
    Try
      Dim strOrdine As String = ""
      Dim qtaCarico As Decimal = 0
      Dim qtaScarico As Decimal = 0
      Dim strOperazione As String = NTSCStr(drJob!operation)
      Dim nMagazCarico As Integer = 0
      Dim nMagazScarico As Integer = 0
      Dim strNote As String = ""
      Dim nFase As Integer = 0
      Dim nRiga As Integer = 0

      Select Case strOperazione
        Case "+", "p"
          nMagazScarico = nMagazPrincipale
          nMagazCarico = nMagazMaccina
          'qtaScarico = NTSCDec(drJob!nominalQuantity)
          qtaScarico = NTSCDec(drJob!actualQuantity)
          qtaCarico = NTSCDec(drJob!actualQuantity)
          strNote = "VERSAMENTO " & NTSCStr(drJob!jobNumber)
        Case "-", "c"
          nMagazScarico = nMagazMaccina
          nMagazCarico = nMagazPrincipale
          qtaScarico = NTSCDec(drJob!actualQuantity)
          qtaCarico = NTSCDec(drJob!actualQuantity)
          strNote = "PRELIEVO " & NTSCStr(drJob!jobNumber)
      End Select

      Dim dtArt As New DataTable
      oCldMoin.ValCodiceDb(NTSCStr(drJob!articleNumber), strDittaCorrente, "artico", "S",, dtArt)

      If dtArt.Rows.Count > 0 Then
        If NTSCStr(dtArt.Rows(0) !ar_gesfasi) = "S" Then
          nFase = NTSCInt(dtArt.Rows(0) !ar_ultfase)
        End If
      End If

      'Riga di carico
      If Not oCleBoll.AggiungiRigaCorpo(False, NTSCStr(drJob!articleNumber), nFase, 0, nCausaleScarico, nMagazScarico) Then
        LogWrite("Articolo " & NTSCStr(drJob!articleNumber) & " inesistente in archivio", True)
        Return False
      End If

      If Trim(NTSCStr(dsBoll.Tables("CORPO").Rows(dsBoll.Tables("CORPO").Rows.Count - 1) !ec_codart)) = "" Then
        LogWrite("Articolo " & NTSCStr(drJob!articleNumber) & " inesistente in archivio", True)
        dsBoll.Tables("CORPO").Rows(dsBoll.Tables("CORPO").Rows.Count - 1).Delete()
        Return False
      End If

      With dsBoll.Tables("CORPO").Rows(dsBoll.Tables("CORPO").Rows.Count - 1)
        nRiga = NTSCInt(!ec_riga)
        !ec_colli = qtaScarico
        !ec_quant = qtaScarico
        !ec_note = strNote
      End With

      If Not oCleBoll.RecordSalva(dsBoll.Tables("CORPO").Rows.Count - 1, False, Nothing) Then
        Return False
      End If

      AggiornaRigheKit(nRiga, nCausaleScarico, nMagazScarico, strNote, dsBoll)

      'Riga di scarcio
      If Not oCleBoll.AggiungiRigaCorpo(False, NTSCStr(drJob!articleNumber), nFase, 0, nCausaleCarico, nMagazCarico) Then
        LogWrite("Articolo " & NTSCStr(drJob!articleNumber) & " inesistente in archivio", True)
        Return False
      End If

      If Trim(NTSCStr(dsBoll.Tables("CORPO").Rows(dsBoll.Tables("CORPO").Rows.Count - 1) !ec_codart)) = "" Then
        LogWrite("Articolo " & NTSCStr(drJob!articleNumber) & " inesistente in archivio", True)
        dsBoll.Tables("CORPO").Rows(dsBoll.Tables("CORPO").Rows.Count - 1).Delete()
        Return False
      End If

      With dsBoll.Tables("CORPO").Rows(dsBoll.Tables("CORPO").Rows.Count - 1)
        nRiga = NTSCInt(!ec_riga)
        !ec_colli = qtaCarico
        !ec_quant = qtaCarico
        !ec_note = strNote
      End With

      If Not oCleBoll.RecordSalva(dsBoll.Tables("CORPO").Rows.Count - 1, False, Nothing) Then
        Return False
      End If

      AggiornaRigheKit(nRiga, nCausaleCarico, nMagazCarico, strNote, dsBoll)

      Return True
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return False
    End Try
  End Function

  Public Overridable Function AggiornaRigheKit(nRigaPadre As Integer, nCausale As Integer, nMagaz As Integer, strNote As String, dsBoll As DataSet) As Boolean
    Try

      For Each drCorpo As DataRow In dsBoll.Tables("CORPO").Select("ec_flkit = 'B'")
        If NTSCInt(drCorpo!ec_ktriga) = nRigaPadre Then
          drCorpo!ec_causale = nCausale
          drCorpo!ec_magaz = nMagaz
          drCorpo!ec_note = strNote
        End If
      Next

      Return True
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return False
    End Try
  End Function

  Public Overridable Function ValCodcieDb(strCodice As String, strNomeTabella As String, strTipoCod As String, ByRef strDescampo As String, ByRef dttTable As DataTable) As Boolean
    Try
      Return oCldMoin.ValCodiceDb(strCodice, strDittaCorrente, strNomeTabella, strTipoCod, strDescampo, dttTable)
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Sub BeforeColUpdateTesta(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Dim strErr As String = ""
    Try
      'memorizzo il valore corrente di cella per testarlo nella AfterColUpdate
      'solo se il dato è uguale a quello precedentemente contenuto nella cella
      If ValoriUguali(e.ProposedValue.ToString, e.Row(e.Column.ColumnName).ToString) Then
        strPrevCelValue += e.Column.ColumnName.ToUpper + ";"
        Return
      End If

      '-------------------------------------------------------------
      'controllo che in una cella short non venga inserito un numero troppo grande
      If Not CheckCellaShort(e, strErr) Then Throw New NTSException(strErr)
      '-------------------------------------------------------------
      'cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
      Dim strFunction As String = "BeforeColUpdateTesta_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub AfterColUpdateTesta(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)

    Try
      'non valido la colonna se il dato non è cambiato
      If strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";") > -1 Then
        strPrevCelValue = strPrevCelValue.Remove(strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";"), e.Column.ColumnName.ToUpper.Length + 1)
        Return
      End If

      bHasChanges = True

      'comunico che una cella è cambiata, per fare in modo che se il dato è contenuto in una griglia 
      'vengano fatte le routine di validazione del caso
      ThrowRemoteEvent(New NTSEventArgs("GRIAGG", e.Column.Table.TableName & "§" & e.Column.ColumnName))

      e.Row.EndEdit()

      '-------------------------------------------------------------
      'cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
      Dim strFunction As String = "AfterColUpdateTesta_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------------------

      CLN__STD.GestErr(ex, Me, "")

      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub BeforeColUpdateCorpo(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Dim strErr As String = ""
    Try
      'memorizzo il valore corrente di cella per testarlo nella AfterColUpdate
      'solo se il dato è uguale a quello precedentemente contenuto nella cella
      If ValoriUguali(e.ProposedValue.ToString, e.Row(e.Column.ColumnName).ToString) Then
        strPrevCelValue += e.Column.ColumnName.ToUpper + ";"
        Return
      End If

      '-------------------------------------------------------------
      'controllo che in una cella short non venga inserito un numero troppo grande
      If Not CheckCellaShort(e, strErr) Then Throw New NTSException(strErr)
      '-------------------------------------------------------------
      'cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
      Dim strFunction As String = "BeforeColUpdateCorpo_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub AfterColUpdateCorpo(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)

    Try
      'non valido la colonna se il dato non è cambiato
      If strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";") > -1 Then
        strPrevCelValue = strPrevCelValue.Remove(strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";"), e.Column.ColumnName.ToUpper.Length + 1)
        Return
      End If

      bHasChanges = True

      'comunico che una cella è cambiata, per fare in modo che se il dato è contenuto in una griglia 
      'vengano fatte le routine di validazione del caso
      ThrowRemoteEvent(New NTSEventArgs("GRIAGG", e.Column.Table.TableName & "§" & e.Column.ColumnName))

      e.Row.EndEdit()

      '-------------------------------------------------------------
      'cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
      Dim strFunction As String = "AfterColUpdateCorpo_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

End Class