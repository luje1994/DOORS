Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports NTSInformatica.CLN__STD
Imports System.IO
Imports System.Xml
Imports Opc.Ua
Imports Doors.Opc.Ua

Public Class CLEHHCOOP
  Inherits CLE__BASE

  Public oCldCoop As CLDHHCOOP
  Public dttAnaz As New DataTable
  Public strMacchina As String = ""
  Public strMsg As String = ""

#Region "Oggetto connessione DBAVEX"
  Public oCleAvex As CLEDBAVEX
#End Region

#Region "Oggetto connessione HHIOPC"
  Public oCleIopc As CLEHHIOPC
#End Region

#Region "Oggetto connessione OPCUA"
  Public client As UaClient
  Public subscribe As Opc.Ua.Client.Subscription
#End Region

#Region "Variabili Registro"
  Public strNomeServer As String = ""
#End Region

#Region "Variabili"
  Public strModalita As String
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

    If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHCOOP"
    MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")

    oCldCoop = CType(MyBase.ocldBase, CLDHHCOOP)
    oCldCoop.Init(oApp)
    Dim strErr As String = ""
    Dim oTmp As Object = Nothing

    Return True

  End Function

  Public Overridable Function IstanziaDBAvex() As Boolean
    Try
      '-------------------------------------------------
      '--- Inizializzo il componente
      '-------------------------------------------------
      If oCleAvex IsNot Nothing Then Return True

      Dim strErr As String = ""
      Dim oTmp As Object = Nothing
      If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BEHHCOOP", "BEDBAVEX", oTmp, strErr, False, "", "") = False Then
        Throw New NTSException(oApp.Tr(Me, 128607611686875000, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
        Return False
      End If
      oCleAvex = CType(oTmp, CLEDBAVEX)

      AddHandler oCleAvex.RemoteEvent, AddressOf GestisciEventiEntityDBAvex
      If oCleAvex.Init(oApp, oScript, oCleComm, "", False, "", "") = False Then Return False

      Return True

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Function

  Public Overridable Function IstanziaHHIopc() As Boolean
    Try
      '-------------------------------------------------
      '--- Inizializzo il componente
      '-------------------------------------------------
      If oCleIopc IsNot Nothing Then Return True

      Dim strErr As String = ""
      Dim oTmp As Object = Nothing
      If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BEHHCOOP", "BEHHIOPC", oTmp, strErr, False, "", "") = False Then
        Throw New NTSException(oApp.Tr(Me, 128607611686875000, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
        Return False
      End If
      oCleIopc = CType(oTmp, CLEHHIOPC)

      AddHandler oCleIopc.RemoteEvent, AddressOf GestisciEventiEntityHHIopc
      If oCleIopc.Init(oApp, oScript, oCleComm, "", False, "", "") = False Then Return False

      Return True

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Function

  Public Overridable Sub GestisciEventiEntityDBAvex(ByVal sender As Object, ByRef e As NTSEventArgs)
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

  Public Overridable Sub GestisciEventiEntityHHIopc(ByVal sender As Object, ByRef e As NTSEventArgs)
    Try
      Select Case UCase(e.TipoEvento)
        Case "SET_FOCUS"
        Case "MONITOR_OPCUA"
          Dim strNodo As String = e.Message
          If Trim(strNodo) <> "" Then
            GestisciNodo(strNodo)
          End If
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

  Public Overridable Function ApriMacchine(ByRef dsOut As DataSet) As Boolean
    Try

      dsOut = New DataSet
      If Not oCleIopc.Apri(dsOut) Then Return False
      '--------------------------------------------------
      '--- Imposta Dataset condiviso
      '--------------------------------------------------
      dsShared = dsOut

      '--------------------------------------------------
      '--- Creo gli eventi per la gestione dei datatable dentro l'entity
      '--------------------------------------------------
      If Not dsShared Is Nothing Then
        'Eventi
        RemoveHandler dsShared.Tables("TESTA").ColumnChanging, AddressOf BeforeColUpdateTesta
        RemoveHandler dsShared.Tables("TESTA").ColumnChanging, AddressOf AfterColUpdateTesta
        RemoveHandler dsShared.Tables("CORPO").ColumnChanging, AddressOf BeforeColUpdateCorpo
        RemoveHandler dsShared.Tables("CORPO").ColumnChanging, AddressOf AfterColUpdateCorpo
        RemoveHandler dsShared.Tables("MACCHINE").ColumnChanging, AddressOf BeforeColUpdateMacchine
        RemoveHandler dsShared.Tables("MACCHINE").ColumnChanging, AddressOf AfterColUpdateMacchine
      End If
      AddHandler dsShared.Tables("TESTA").ColumnChanging, AddressOf BeforeColUpdateTesta
      AddHandler dsShared.Tables("TESTA").ColumnChanged, AddressOf AfterColUpdateTesta
      AddHandler dsShared.Tables("CORPO").ColumnChanging, AddressOf BeforeColUpdateCorpo
      AddHandler dsShared.Tables("CORPO").ColumnChanged, AddressOf AfterColUpdateCorpo
      AddHandler dsShared.Tables("MACCHINE").ColumnChanging, AddressOf BeforeColUpdateMacchine
      AddHandler dsShared.Tables("MACCHINE").ColumnChanged, AddressOf AfterColUpdateMacchine

      Return True
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function SalvaMacchine(ByVal dsOut As DataSet) As Boolean
    Try
      If Not oCleIopc.SalvaMacchine(dsOut) Then Return False
      Return True
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function ConnettiOPCUA() As Boolean
    Dim bReturn As Boolean = False
    Try
      If dsShared Is Nothing Then Return False
      If dsShared.Tables("TESTA") Is Nothing Then Return False
      If dsShared.Tables("TESTA").Rows.Count = 0 Then Return False

      If oCleIopc.ConnessioneOPCUA(client) Then Return True
      bReturn = oCleIopc.ConnettiOPCUA(client, subscribe)

      'If bReturn Then
      '  Dim strNodoVal As String = ""
      '  If oCleIopc.TrovaNodo(".Start_ID1", strNodoVal) Then
      '    oCleIopc.ScriviNodo(client, strNodoVal, "True")
      '  End If
      'End If

      Return bReturn
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function LeggiDatiDitta(ByVal strDitta As String) As Boolean
    Dim dttTmp As New DataTable

    Try
      '--------------------------------------------------------------------------------------------------------------
      oCldCoop.ValCodiceDb(strDitta, strDitta, "TABANAZ", "S", "", dttAnaz)
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
      strNomeServer = NTSCStr(oCldCoop.GetSettingBusDitt(strDittaCorrente, "BSHHCOOP", "OPZIONI", ".", "HHNomeServer", " ", " ", " "))

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub GestisciTimerElabora()
    Try
      Dim dsIopc As New DataSet

      If Not oCleIopc.Apri(dsIopc) Then Return

      If dsIopc Is Nothing Then Return

      If dsIopc.Tables("TESTA") Is Nothing Then Return

      If dsIopc.Tables("TESTA").Rows.Count = 0 Then Return

      Dim drTesta As DataRow = dsIopc.Tables("TESTA").Rows(0)

      strModalita = NTSCStr(drTesta!tb_modalita)

      If NTSCStr(drTesta!tb_elaborazioneautom) <> "S" Then
        ThrowRemoteEvent(New NTSEventArgs("TIMER_ELABORA_INTERVAL", NTSCStr(1000)))
        Return
      End If

      Dim nInterval As Integer = NTSCInt(drTesta!tb_secelaborazione) * 1000

      ThrowRemoteEvent(New NTSEventArgs("TIMER_ELABORA_INTERVAL", NTSCStr(nInterval)))

      'Dim dtOpz As New DataTable
      'setDtOpz(dtOpz)
      'dtOpz.Rows.Add("-1", 0, "", 0, "", "", "", "", 0, "")

      'LeggiTesta("S", dtOpz)

      If oCleIopc.ConnessioneOPCUA(client) Then
        Elabora()
      End If

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

  Public Overridable Function Elabora() As Boolean
    Try
      Dim dsIopc As New DataSet
      Dim dtProd As New DataTable
      Dim bError As Boolean = False

      'IstanziaHHIopc()

      'If Not LogStart("BNHHCOOP", "Elabora OPCUA" & vbNewLine, False) Then Return False

      If Not oCleIopc.Apri(dsIopc) Then
        If Not oApp.Batch Then ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, "Errore nell'apertura delle impostazioni OPCUA"))
        Return False
      End If

      'MsgBox("PRE GEST MONITOR")

      oCleIopc.GestisciMonitorOPC(client, subscribe)

      ''MsgBox("GEST MONITOR")

      '      For Each dr As DataRow In dsShared.Tables("CORPO").Rows
      '        If Not oCleIopc.ConnessioneOPCUA(client) Then GoTo Uscita

      '        'oCldCoop.ApriProdotti(dtProd)

      '        ''MsgBox("1")

      '        'For Each drProd As DataRow In dtProd.Rows
      '        '  'MsgBox("2")
      '        '  Dim strProdotto As String = NTSCStr(drProd!tb_prodotto)
      '        '  'MsgBox("PRODOTTO: " & strProdotto)
      '        '  Dim nIDNodo As Integer = NTSCInt(drProd!tb_id)
      '        '  'MsgBox("ID NODO: " & nIDNodo.ToString())
      '        '  Dim strNodo As String = ""
      '        '  If oCleIopc.TrovaNodo(".ID" & NTSCStr(nIDNodo) & "String", strNodo) Then
      '        '    Dim strValore As String = NTSCStr(oCleIopc.LeggiNodo(client, strNodo))
      '        '    'MsgBox("ID NODO: " & nIDNodo.ToString())
      '        '    'MsgBox("NODO: " & strNodo)
      '        '    'MsgBox("PRE VALORE: " & strValore)
      '        '    If UCase(strValore) <> UCase(strProdotto) Then
      '        '      'MsgBox("VALORE: " & strValore)
      '        '      'MsgBox("PRODOTTO: " & strProdotto)
      '        '      oCleIopc.ScriviNodo(client, strNodo, strProdotto)
      '        '    End If
      '        '  End If
      '        'Next

      '      Next

      'Uscita:

      'LogStop()

      'If Not oApp.Batch Then oApp.MsgBoxInfo(oApp.Tr(Me, 129877042710458543, "Elaborazione Terminata"))

      ''ThrowRemoteEvent(New NTSEventArgs("INFO_LABEL", oApp.Tr(Me, 128768973837496061, "Nessuna operazione In corso")))

      '''Imposta label e ProgressBar di elaborazione
      ''If Not oApp.Batch Then ThrowRemoteEvent(New NTSEventArgs("PROGRESS_BAR_POS", "0"))

      'If nCountLog > 0 And Not oApp.Batch Then
      '  oApp.MsgBoxInfo(oApp.Tr(Me, 129877042710458543, "Ci sono stati dei problemi con l'elborazione "))
      '  System.Diagnostics.Process.Start("notepad", LogFileName)
      '  Return False
      'End If

      Return True
    Catch ex As Exception
      LogStop()
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Sub GestisciNodo(strNodo As String)
    Dim strNodoVal As String = ""
    Dim strValoreVal As String = ""
    Dim strValore As String = ""
    Dim strProgramma As String = ""
    Dim strCodart As String = ""
    Dim lCommessa As Long = 0
    Dim nAnno As Integer = 0
    Dim nMese As Integer = 0
    Dim nGiorno As Integer = 0
    Dim nOra As Integer = 0
    Dim nMinuti As Integer = 0
    Dim nSecondi As Integer = 0

    'Dim nStato As Integer = 0
    Try
      'BNHHCOOP /B C:\Bus\Asc\CTX1250.bub TMKART
      If oCleIopc.dsShared.Tables("CORPO").Select("tb_nodo = " & CStrSQL(strNodo)).Length > 0 Then

        Dim dtCorpo As DataTable = oCleIopc.dsShared.Tables("CORPO").Copy()
        Dim dr As DataRow = dtCorpo.Select("tb_nodo = " & CStrSQL(strNodo))(0)
        'System.IO.File.AppendAllText("C:\TEMP\Nodo.txt", "Data: " & Date.Now.ToString & " Nodo: " & strNodo & vbCrLf)
        If Not dr Is Nothing Then
          'If Not LogStart("BNHHCOOP", "Connettore OPC-UA" & vbNewLine, False) Then Return

          strValore = NTSCStr(dr!tb_valore)
          'strValoreVal = NTSCStr(dr!tb_valore)
          'System.IO.File.AppendAllText("C:\TEMP\Nodo.txt", "Macchina: " & oCleIopc.strMacchina & " Data: " & Date.Now.ToString & " Nodo: " & strNodo & " Valore: " & strValoreVal & vbCrLf)

          If UCase(strModalita) = "DMG_DMU" Then
            If dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.PROGRAMMA)).Length > 0 And
               dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.STATO)).Length > 0 And
               dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.QTAFATTA)).Length > 0 And
               dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.SECONDI)).Length > 0 Then

              Dim strNodoProg As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.PROGRAMMA))(0) !tb_nodo)
              strProgramma = NTSCStr(oCleIopc.LeggiNodo(client, strNodoProg))

              'strProgramma = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.PROGRAMMA))(0) !tb_valore)
              If Trim(strProgramma) <> "" Then
                Dim strPath As String = strProgramma
                strPath = Replace(strPath, "/", "\")
                strPath = Replace(strPath, ":", "")
                Dim f As New FileInfo(strPath)
                'Dim strFile As String = f.Name
                'If InStr(strFile, "_") > 0 Then
                '  strCodart = Mid(strFile, 1, InStr(strFile, "_") - 1)
                'End If
                Dim strDir As String = Path.GetFileName(f.DirectoryName)
                If InStr(strDir, "_") > 0 Then
                  lCommessa = NTSCLng(Mid(strDir, 1, InStr(strDir, "_") - 1))
                  strCodart = oCldCoop.GetArticolo(strDittaCorrente, oCleIopc.strMacchina, lCommessa)
                End If
              End If

              If lCommessa > 0 And Trim(strCodart) <> "" Then
                If Not oCldCoop.recordExist("tabhhmactesta", "codditt = " & CStrSQL(strDittaCorrente),
                                                      "tb_macchina = " & CStrSQL(oCleIopc.strMacchina),
                                                      "tb_commessa = " & lCommessa,
                                                      "tb_codart = " & CStrSQL(strCodart)) Then

                  oCldCoop.InsertOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart)
                End If
              End If

              'nStato = NTSCInt(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.STATO))(0) !tb_valore)

              Select Case NTSCStr(dr!tb_tipo)
                'Case oCleIopc.PROGRAMMA
                '  Try
                '    Dim strNodoQta As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.QTAFATTA))(0) !tb_nodo)
                '    Dim nQta As Integer = NTSCInt(oCleIopc.LeggiNodo(client, strNodoQta))
                '  Catch ex As Exception
                '    oCldCoop.SaveSettingBus("BSHHCOOP", "Recent", ".", "HHReset" & oCleIopc.strMacchina, "-1", " ", True, True, False)
                '  End Try
                Case oCleIopc.QTAFATTA
                  Dim nQta As Integer = NTSCInt(strValore)
                  Dim nQtaFatta As Integer = 0
                  'Dim nQtaFatta As Integer = nQta
                  If nQta > 0 Then

                    Dim nQtaAtt As Integer = NTSCInt(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quatt"))
                    If nQta <= nQtaAtt Then
                      Dim nQtaPrec As Integer = NTSCInt(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quant"))
                      oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quapre", "=", NTSCStr(nQtaPrec))
                    End If
                    Dim nQtaIniz As Integer = NTSCInt(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quapre"))
                    nQtaFatta = nQta + nQtaIniz
                    If nQtaFatta > 0 Then
                      oCldCoop.UpadteOPCCorpo(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, nQtaFatta, Now)
                      oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_datqua", "=", CDataOraSQL(Now))
                      oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quant", "=", NTSCStr(nQtaFatta))
                    End If
                    oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quatt", "=", NTSCStr(nQta))

                    'If CBool(NTSCInt(oCldCoop.GetSettingBus("BSHHCOOP", "Recent", ".", "HHReset" & oCleIopc.strMacchina, "0", " ", "0"))) Then
                    '  nQtaFatta = NTSCInt(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quant"))
                    '  oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quapre", "=", NTSCStr(nQtaFatta))
                    '  oCldCoop.SaveSettingBus("BSHHCOOP", "Recent", ".", "HHReset" & oCleIopc.strMacchina, "0", " ", True, True, False)
                    'End If
                    'Dim nQtaIniz As Integer = NTSCInt(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quapre"))
                    'nQtaFatta = nQta + nQtaIniz
                    'If nQtaFatta > 0 Then
                    '  oCldCoop.UpadteOPCCorpo(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, nQtaFatta, Now)
                    '  oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_datqua", "=", CDataOraSQL(Now))
                    '  oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quant", "=", NTSCStr(nQtaFatta))
                    'End If
                    ''Else
                    ''  oCldCoop.SaveSettingBus("BSHHCOOP", "Recent", ".", "HHReset" & oCleIopc.strMacchina, "-1", " ", True, True, False)
                    ''  'nQtaFatta = NTSCInt(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quant"))
                    ''  'oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quapre", "=", NTSCStr(nQtaFatta))
                  End If
                Case oCleIopc.SECONDI
                  If dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.STATO)).Length > 0 Then
                    Dim strNodoStato As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.STATO))(0) !tb_nodo)
                    Dim nStato As Integer = NTSCInt(oCleIopc.LeggiNodo(client, strNodoStato))
                    If nStato = 1 Then
                      Dim nTempo As Long = NTSCLng(strValore)
                      If nTempo > 0 Then
                        oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_sec", "=", NTSCStr(nTempo))
                      End If
                    End If
                  End If
                Case oCleIopc.STATO
                  oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_datsta", "=", CDataOraSQL(Now))
                  oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_stato", "=", CStrSQL(strValore))
                  If NTSCInt(strValore) = 1 Then
                    'If dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.QTAFATTA)).Length > 0 Then
                    '  Dim strNodoQta As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.QTAFATTA))(0) !tb_nodo)
                    '  Dim nQta As Integer = NTSCInt(oCleIopc.LeggiNodo(client, strNodoQta))
                    '  oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quapre", "=", NTSCStr(nQta))
                    'End If
                  ElseIf NTSCInt(strValore) = 3 Then
                    If dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.SECONDI)).Length > 0 Then
                      Dim lSec As Long = NTSCLng(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_sec"))
                      oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_tempo", "+=", NTSCStr(lSec))
                      oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_sec", "=", "0")
                    End If
                  End If
              End Select

            End If
          ElseIf UCase(strModalita) = "DMG_CTX" Then

            If dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.PROGRAMMA)).Length > 0 And
               dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.QTAFATTA)).Length > 0 And
               dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.SECONDI)).Length > 0 Then

              Dim strNodoProg As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.PROGRAMMA))(0) !tb_nodo)
              strProgramma = NTSCStr(oCleIopc.LeggiNodo(client, strNodoProg))

              If Trim(strProgramma) <> "" Then
                Dim strPath As String = strProgramma
                strPath = Replace(strPath, "/", "\")
                strPath = Replace(strPath, ":", "")
                Dim f As New FileInfo(strPath)
                Dim strDir As String = Path.GetFileName(f.DirectoryName)
                If InStr(strDir, "_") > 0 Then
                  lCommessa = NTSCLng(Mid(strDir, 1, InStr(strDir, "_") - 1))
                  strCodart = oCldCoop.GetArticolo(strDittaCorrente, oCleIopc.strMacchina, lCommessa)
                End If
              End If

              If lCommessa > 0 And Trim(strCodart) <> "" Then
                If Not oCldCoop.recordExist("tabhhmactesta", "codditt = " & CStrSQL(strDittaCorrente),
                                                      "tb_macchina = " & CStrSQL(oCleIopc.strMacchina),
                                                      "tb_commessa = " & lCommessa,
                                                      "tb_codart = " & CStrSQL(strCodart)) Then

                  oCldCoop.InsertOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart)
                End If
              End If

              Select Case NTSCStr(dr!tb_tipo)
                Case oCleIopc.QTAFATTA
                  Dim nQta As Integer = NTSCInt(strValore)
                  If nQta > 0 Then
                    oCldCoop.UpadteOPCCorpo(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, nQta, Now)
                    oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_datqua", "=", CDataOraSQL(Now))
                    oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_quant", "=", NTSCStr(nQta))
                  End If
                Case oCleIopc.SECONDI
                  'Dim lSec As Long = NTSCLng(strValore)
                  Dim lSec As Long = NTSCLng(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_sec"))
                  Dim lTempo As Long = NTSCLng(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_tempo"))
                  If NTSCLng(strValore) < lSec Then
                    oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_secprec", "=", NTSCStr(lTempo))
                  End If
                  Dim lSecPrec As Long = NTSCLng(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_secprec"))
                  lSec = NTSCLng(strValore) + lSecPrec
                  oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_tempo", "=", NTSCStr(lSec))
                  oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_sec", "=", NTSCStr(NTSCLng(strValore)))
                Case oCleIopc.STATO
                  oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_datsta", "=", CDataOraSQL(Now))
                  oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_stato", "=", CStrSQL(strValore))
              End Select

            End If

          ElseIf UCase(strModalita) = "IFP" Then

            If dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.COMMESSA)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.FLAGSTART)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.FLAGSTOP)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.ANNOSTART)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.MESESTART)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.GIORNOSTART)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.ORASTART)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.MINUTOSTART)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.SECONDOSTART)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.ANNOSTOP)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.MESESTOP)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.GIORNOSTOP)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.ORASTOP)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.MINUTOSTOP)).Length > 0 And
             dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.SECONDOSTOP)).Length > 0 Then

              Dim strNodoComm As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.COMMESSA))(0) !tb_nodo)
              lCommessa = NTSCLng(oCleIopc.LeggiNodo(client, strNodoComm))
              strCodart = ""

              If lCommessa > 0 Then
                If Not oCldCoop.recordExist("tabhhmactesta", "codditt = " & CStrSQL(strDittaCorrente),
                                                      "tb_macchina = " & CStrSQL(oCleIopc.strMacchina),
                                                      "tb_commessa = " & lCommessa,
                                                      "tb_codart = " & CStrSQL(strCodart)) Then

                  oCldCoop.InsertOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart)
                End If
              End If

              Select Case NTSCStr(dr!tb_tipo)
                'Case oCleIopc.FLAGSTART
                '  If CBool(strValore) Then
                '    Dim strNodoAnno As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.ANNOSTART))(0) !tb_nodo)
                '    nAnno = NTSCInt(oCleIopc.LeggiNodo(client, strNodoAnno))
                '    Dim strNodoMese As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.MESESTART))(0) !tb_nodo)
                '    nMese = NTSCInt(oCleIopc.LeggiNodo(client, strNodoMese))
                '    Dim strNodoGiorno As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.GIORNOSTART))(0) !tb_nodo)
                '    nGiorno = NTSCInt(oCleIopc.LeggiNodo(client, strNodoGiorno))
                '    Dim strNodoOra As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.ORASTART))(0) !tb_nodo)
                '    nOra = NTSCInt(oCleIopc.LeggiNodo(client, strNodoOra))
                '    Dim strNodoMinuti As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.MINUTOSTART))(0) !tb_nodo)
                '    nMinuti = NTSCInt(oCleIopc.LeggiNodo(client, strNodoMinuti))
                '    Dim strNodoSecondi As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.SECONDOSTART))(0) !tb_nodo)
                '    nSecondi = NTSCInt(oCleIopc.LeggiNodo(client, strNodoSecondi))
                '    Dim dStart As New Date(nAnno, nMese, nGiorno, nOra, nMinuti, nSecondi)
                '    oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_datini", "=", CDataOraSQL(dStart))
                '  End If
                Case oCleIopc.FLAGSTOP
                  If CBool(strValore) Then

                    Dim strNodoAnnoStart As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.ANNOSTART))(0) !tb_nodo)
                    nAnno = NTSCInt(oCleIopc.LeggiNodo(client, strNodoAnnoStart))
                    Dim strNodoMeseStart As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.MESESTART))(0) !tb_nodo)
                    nMese = NTSCInt(oCleIopc.LeggiNodo(client, strNodoMeseStart))
                    Dim strNodoGiornoStart As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.GIORNOSTART))(0) !tb_nodo)
                    nGiorno = NTSCInt(oCleIopc.LeggiNodo(client, strNodoGiornoStart))
                    Dim strNodoOraStart As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.ORASTART))(0) !tb_nodo)
                    nOra = NTSCInt(oCleIopc.LeggiNodo(client, strNodoOraStart))
                    Dim strNodoMinutiStart As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.MINUTOSTART))(0) !tb_nodo)
                    nMinuti = NTSCInt(oCleIopc.LeggiNodo(client, strNodoMinutiStart))
                    Dim strNodoSecondiStart As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.SECONDOSTART))(0) !tb_nodo)
                    nSecondi = NTSCInt(oCleIopc.LeggiNodo(client, strNodoSecondiStart))
                    Dim dStart As New Date(nAnno, nMese, nGiorno, nOra, nMinuti, nSecondi)
                    'oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_datini", "=", CDataOraSQL(dStart))

                    Dim strNodoAnnoStop As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.ANNOSTOP))(0) !tb_nodo)
                    nAnno = NTSCInt(oCleIopc.LeggiNodo(client, strNodoAnnoStop))
                    Dim strNodoMeseStop As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.MESESTOP))(0) !tb_nodo)
                    nMese = NTSCInt(oCleIopc.LeggiNodo(client, strNodoMeseStop))
                    Dim strNodoGiornoStop As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.GIORNOSTOP))(0) !tb_nodo)
                    nGiorno = NTSCInt(oCleIopc.LeggiNodo(client, strNodoGiornoStop))
                    Dim strNodoOraStop As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.ORASTOP))(0) !tb_nodo)
                    nOra = NTSCInt(oCleIopc.LeggiNodo(client, strNodoOraStop))
                    Dim strNodoMinutiStop As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.MINUTOSTOP))(0) !tb_nodo)
                    nMinuti = NTSCInt(oCleIopc.LeggiNodo(client, strNodoMinutiStop))
                    Dim strNodoSecondiStop As String = NTSCStr(dtCorpo.Select("tb_tipo = " & CStrSQL(oCleIopc.SECONDOSTOP))(0) !tb_nodo)
                    nSecondi = NTSCInt(oCleIopc.LeggiNodo(client, strNodoSecondiStop))
                    Dim dStop As New Date(nAnno, nMese, nGiorno, nOra, nMinuti, nSecondi)
                    'Dim dStart As Date = NTSCDate(oCldCoop.GetOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_datini"))

                    Dim nTempo As Long = 0
                    nTempo = DateDiff(DateInterval.Second, dStart, dStop)

                    oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_datfin", "=", CDataOraSQL(dStop))
                    oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_datini", "=", CDataOraSQL(dStart))

                    oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_tempo", "=", NTSCStr(nTempo))
                    oCldCoop.UpadteOPC(strDittaCorrente, oCleIopc.strMacchina, lCommessa, strCodart, "tb_sec", "=", NTSCStr(nTempo))
                    oCldCoop.UpadteQtaDaComme(strDittaCorrente, oCleIopc.strMacchina, lCommessa)

                  End If
              End Select


            End If

          End If

        End If

      End If

    Catch ex As Exception
      If Not LogStart("BNHHCOOP", "Connettore OPC-UA" & vbNewLine, True) Then Return
      LogWrite("MACCHINA: " & oCleIopc.strMacchina, True)
      LogWrite("NODO: " & strNodo, True)
      LogWrite("VALORE: " & strValore, True)
      LogWrite("ERRORE: " & ex.Message, True)
      LogStop()
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
      'Finally
      '  LogStop()
    End Try
  End Sub

  Public Overridable Sub ChiudiOPCUA()
    Try
      oCleIopc.ChiudiOPCUA(client, subscribe)
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub CambiaStato(nStato As Integer)
    Try
      If dsShared.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(oCleIopc.strMacchina)).Length > 0 Then
        Dim drMacchine As DataRow = dsShared.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(oCleIopc.strMacchina))(0)
        If nStato <> NTSCInt(drMacchine!tb_stato) Then
          drMacchine!tb_stato = nStato
          oCleIopc.SalvaMacchine(dsShared)
        End If
      End If
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Function ValCodcieDb(strCodice As String, strNomeTabella As String, strTipoCod As String, ByRef strDescampo As String, ByRef dttTable As DataTable) As Boolean
    Try
      Return oCldCoop.ValCodiceDb(strCodice, strDittaCorrente, strNomeTabella, strTipoCod, strDescampo, dttTable)
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

  Public Overridable Sub BeforeColUpdateMacchine(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
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
      Dim strFunction As String = "BeforeColUpdateMacchine_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub AfterColUpdateMacchine(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)

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
      Dim strFunction As String = "AfterColUpdateMacchine_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub AfterColUpdateMacchine_tb_startstop(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)

    Try

      If oApp.Batch Then Return

      If e.ProposedValue = 1 Then
        e.Row!backcolor_tb_startstop = Color.LimeGreen.ToArgb
      Else
        e.Row!backcolor_tb_startstop = Color.Crimson.ToArgb
      End If

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub AfterColUpdateMacchine_tb_stato(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)

    Try

      If oApp.Batch Then Return

      If e.ProposedValue = 1 Then
        e.Row!backcolor_tb_stato = Color.LimeGreen.ToArgb
      Else
        e.Row!backcolor_tb_stato = Color.Crimson.ToArgb
      End If

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

End Class