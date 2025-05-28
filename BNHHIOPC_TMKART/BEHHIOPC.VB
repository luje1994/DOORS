Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System
Imports System.Net
Imports System.IO
Imports System.Xml
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Opc.Ua
Imports Doors.Opc.Ua
Imports System.Linq

Public Class CLEHHIOPC
  Inherits CLE__BASE

#Region "Moduli"
  Private Moduli_P As Integer = CLN__STD.bsModAll
  Private ModuliExt_P As Integer = CLN__STD.bsModExtAll
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

#Region "Oggetto OPC"
  Public client As UaClient
  Public subscribe As Opc.Ua.Client.Subscription
#End Region

#Region "Oggetto DAL"
  Public oCldIopc As CLDHHIOPC
#End Region

#Region "Oggetto Entity VEBOLL"
  Public oCleBoll As CLEVEBOLL
#End Region

#Region "Variabili"
  'Variabili per validazione eventi before/after update
  Public bNonValidare As Boolean
  Public bInOnAddNewRow As Boolean
  Public bInUnload As Boolean

  Public bNew As Boolean = False

  Public strMacchina As String = ""
#End Region

#Region "Variabili ReadOnly"
  Public ReadOnly ARTICOLO As String = "ARTICOLO"
  Public ReadOnly COMMESSA As String = "COMMESSA"
  Public ReadOnly ORDINE As String = "ORDINE"
  Public ReadOnly RICETTA As String = "RICETTA"
  Public ReadOnly TEMPO As String = "TEMPO"
  Public ReadOnly QTADAFARE As String = "QTADAFARE"
  Public ReadOnly QTAFATTA As String = "QTAFATTA"
  Public ReadOnly SECONDI As String = "SECONDI"
  Public ReadOnly DATA As String = "DATA"
  Public ReadOnly ORA As String = "ORA"
  Public ReadOnly DATAORA As String = "DATAORA"
  Public ReadOnly STATO As String = "STATO"
  Public ReadOnly PROGRAMMA As String = "PROGRAMMA"
  Public ReadOnly DATASTART As String = "DATASTART"
  Public ReadOnly DATASTOP As String = "DATASTOP"
  Public ReadOnly FLAGSTART As String = "FLAGSTART"
  Public ReadOnly FLAGSTOP As String = "FLAGSTOP"
  Public ReadOnly ANNOSTART As String = "ANNOSTART"
  Public ReadOnly MESESTART As String = "MESESTART"
  Public ReadOnly GIORNOSTART As String = "GIORNOSTART"
  Public ReadOnly ORASTART As String = "ORASTART"
  Public ReadOnly MINUTOSTART As String = "MINUTOSTART"
  Public ReadOnly SECONDOSTART As String = "SECONDOSTART"
  Public ReadOnly ANNOSTOP As String = "ANNOSTOP"
  Public ReadOnly MESESTOP As String = "MESESTOP"
  Public ReadOnly GIORNOSTOP As String = "GIORNOSTOP"
  Public ReadOnly ORASTOP As String = "ORASTOP"
  Public ReadOnly MINUTOSTOP As String = "MINUTOSTOP"
  Public ReadOnly SECONDOSTOP As String = "SECONDOSTOP"
  Public ReadOnly STINGA1 As String = "STINGA1"
  Public ReadOnly STINGA2 As String = "STINGA2"
  Public ReadOnly STINGA3 As String = "STINGA3"
  Public ReadOnly STINGA4 As String = "STINGA4"
  Public ReadOnly STINGA5 As String = "STINGA5"
  Public ReadOnly STINGA6 As String = "STINGA6"
  Public ReadOnly STINGA7 As String = "STINGA7"
  Public ReadOnly STINGA8 As String = "STINGA8"
  Public ReadOnly FLAG1 As String = "FLAG1"
  Public ReadOnly FLAG2 As String = "FLAG2"
  Public ReadOnly FLAG3 As String = "FLAG3"
  Public ReadOnly FLAG4 As String = "FLAG4"
  Public ReadOnly FLAG5 As String = "FLAG5"
  Public ReadOnly FLAG6 As String = "FLAG6"
  Public ReadOnly FLAG7 As String = "FLAG7"
  Public ReadOnly FLAG8 As String = "FLAG8"
#End Region

#Region "Funzioni e Routines di Inizializzazione"
  Public Overrides Function Init _
    (
    ByRef App As CLE__APP,
    ByRef oScriptEngine As INT__SCRIPT,
    ByRef oCleLbmenu As Object,
    ByVal strTabella As String,
    ByVal bFiller1 As Boolean,
    ByVal strFiller1 As String,
    ByVal strFiller2 As String
    ) As Boolean

    '--------------------------------------------------
    '--- Inzializza DAL
    '--------------------------------------------------
    If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHIOPC"
    MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")
    oCldIopc = CType(MyBase.ocldBase, CLDHHIOPC)
    oCldIopc.Init(oApp)
    '--------------------------------------------------
    '--- Valore funzione
    '--------------------------------------------------
    Return True

  End Function
#End Region

#Region "Funzioni e Routines di Gestione OPCUA"
  Public Overloads Function Apri(ByRef dsOut As DataSet) As Boolean
    Dim dReturn As Boolean = False
    Dim ds As New DataSet
    Try
      '--------------------------------------------------
      '--- Tengo un puntatore al datatable che verrà usato nell'UI
      '--- quindi chiedo i dati al DAL
      '--------------------------------------------------
      dReturn = oCldIopc.GetData(strDittaCorrente, strMacchina, dsOut)
      dsOut.AcceptChanges()
      'dReturn = oCldIopc.GetData(ds)

      '--------------------------------------------------
      '--- Esce se c'è stato un errore
      '--------------------------------------------------
      If dReturn = False Then Return False

      '--------------------------------------------------
      '--- Imposta valori di default
      '--------------------------------------------------
      'SetDefaultValue(ds)
      SetDefaultValue(dsOut)

      'dsOut = ds.Copy
      'dsOut.Tables("TESTA").Rows.Clear()
      'dsOut.Tables("CORPO").Rows.Clear()

      ''Dim dt As New DataTable
      ''dt = ds.Tables("TESTA").Copy
      ''dt.TableName = "MACCHINE"

      ''dsOut.Tables.Add(dt.Copy)

      'For Each drTesta As DataRow In ds.Tables("TESTA").Select("tb_codice = " & CStrSQL(strMacchina))
      '  dsOut.Tables("TESTA").ImportRow(drTesta)
      'Next

      'For Each drCorpo As DataRow In ds.Tables("CORPO").Select("tb_codice = " & CStrSQL(strMacchina))
      '  dsOut.Tables("CORPO").ImportRow(drCorpo)
      'Next

      '--------------------------------------------------
      '--- Imposta Dataset condiviso
      '--------------------------------------------------
      dsShared = dsOut

      '--------------------------------------------------
      '--- Creo gli eventi per la gestione dei datatable dentro l'entity
      '--------------------------------------------------
      If Not dsShared Is Nothing Then
        'Eventi
        RemoveHandler dsShared.Tables("TESTA").ColumnChanging, AddressOf BeforeColUpdateTESTA
        RemoveHandler dsShared.Tables("TESTA").ColumnChanging, AddressOf AfterColUpdateTESTA
        RemoveHandler dsShared.Tables("CORPO").ColumnChanging, AddressOf BeforeColUpdateCORPO
        RemoveHandler dsShared.Tables("CORPO").ColumnChanging, AddressOf AfterColUpdateCORPO
      End If
      AddHandler dsShared.Tables("TESTA").ColumnChanging, AddressOf BeforeColUpdateTESTA
      AddHandler dsShared.Tables("TESTA").ColumnChanged, AddressOf AfterColUpdateTESTA
      AddHandler dsShared.Tables("CORPO").ColumnChanging, AddressOf BeforeColUpdateCORPO
      AddHandler dsShared.Tables("CORPO").ColumnChanged, AddressOf AfterColUpdateCORPO

      '--------------------------------------------------
      '--- Imposta flag variazione su dataset CONNESSIONI.
      '--------------------------------------------------
      bHasChanges = False

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return dReturn

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overloads Function ApriMacchina(ByVal strMacchina As String, ByRef dsOut As DataSet) As Boolean
    Dim dReturn As Boolean = False
    Try
      '--------------------------------------------------
      '--- Tengo un puntatore al datatable che verrà usato nell'UI
      '--- quindi chiedo i dati al DAL
      '--------------------------------------------------
      'dReturn = oCldIopc.GetData(dsOut)
      dReturn = oCldIopc.GetDataMacchine(strDittaCorrente, strMacchina, dsOut)
      dsOut.AcceptChanges()

      '--------------------------------------------------
      '--- Esce se c'è stato un errore
      '--------------------------------------------------
      If dReturn = False Then Return False

      '--------------------------------------------------
      '--- Imposta valori di default
      '--------------------------------------------------
      'SetDefaultValue(dsOut)

      'If dsOut.Tables("TESTA").Rows.Count > 0 Then
      '  If NTSCStr(dsOut.Tables("TESTA").Rows(0) !tb_oragiac1) = "" Then
      '    dsOut.Tables("TESTA").Rows(0) !tb_oragiac1 = "00:00"
      '  End If
      '  If NTSCStr(dsOut.Tables("TESTA").Rows(0) !tb_oragiac2) = "" Then
      '    dsOut.Tables("TESTA").Rows(0) !tb_oragiac2 = "00:00"
      '  End If
      '  If NTSCStr(dsOut.Tables("TESTA").Rows(0) !tb_oragiac3) = "" Then
      '    dsOut.Tables("TESTA").Rows(0) !tb_oragiac3 = "00:00"
      '  End If
      'End If

      '--------------------------------------------------
      '--- Imposta Dataset condiviso
      '--------------------------------------------------
      dsShared = dsOut

      ''--------------------------------------------------
      ''--- Creo gli eventi per la gestione dei datatable dentro l'entity
      ''--------------------------------------------------
      'If Not dsShared Is Nothing Then
      '  'Eventi
      '  RemoveHandler dsShared.Tables("TESTA").ColumnChanging, AddressOf BeforeColUpdateTESTA
      '  RemoveHandler dsShared.Tables("TESTA").ColumnChanging, AddressOf AfterColUpdateTESTA
      '  RemoveHandler dsShared.Tables("CORPO").ColumnChanging, AddressOf BeforeColUpdateCORPO
      '  RemoveHandler dsShared.Tables("CORPO").ColumnChanging, AddressOf AfterColUpdateCORPO
      'End If
      'AddHandler dsShared.Tables("TESTA").ColumnChanging, AddressOf BeforeColUpdateTESTA
      'AddHandler dsShared.Tables("TESTA").ColumnChanged, AddressOf AfterColUpdateTESTA
      'AddHandler dsShared.Tables("CORPO").ColumnChanging, AddressOf BeforeColUpdateCORPO
      'AddHandler dsShared.Tables("CORPO").ColumnChanged, AddressOf AfterColUpdateCORPO

      '--------------------------------------------------
      '--- Imposta flag variazione su dataset CONNESSIONI.
      '--------------------------------------------------
      'bHasChanges = False

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return dReturn

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Function ApriModalita(ByRef dsOut As DataSet) As Boolean
    Dim bReturn As Boolean = False
    Try
      '--------------------------------------
      'Tengo un puntatore al datatable che verrà usato nell'UI, quindi chiedo i dati al DAL
      bReturn = oCldIopc.ApriModalita(strDittaCorrente, dsShared)
      If bReturn = False Then Return False

      dsOut = dsShared

      '--------------------------------------------------------------
      'Imposto i valori di default della tabella: con la riga che segue prendo prima i valori dal database
      oCldIopc.SetTableDefaultValueFromDB("MODA", dsOut)

      SetDefaultValueModalita(dsOut)

      AddHandler dsShared.Tables("MODA").ColumnChanging, AddressOf BeforeColUpdateMODA
      AddHandler dsShared.Tables("MODA").ColumnChanged, AddressOf AfterColUpdateMODA

      bHasChanges = False

      Return bReturn
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overloads Function Salva(ByVal bDelete As Boolean) As Boolean
    Dim bResult As Boolean = False
    'Dim ds As New DataSet
    Try
      '--------------------------------------------------
      '--- Controlli pre-salvataggio (solo se non è una delete)
      '--------------------------------------------------
      If Not bDelete Then
        If Not TestPreSalva("TESTA") Then Return False
        If Not TestPreSalva("CORPO") Then Return False
      End If

      'If Not oCldIopc.GetData(ds) Then Return False

      'SetDefaultValue(ds)

      'If bDelete Then

      '  If ds.Tables("TESTA").Select("tb_codice = " & CStrSQL(strMacchina)).Length > 0 Then
      '    ds.Tables("TESTA").Select("tb_codice = " & CStrSQL(strMacchina))(0).Delete()
      '    For Each dr As DataRow In ds.Tables("CORPO").Select("tb_codice = " & CStrSQL(strMacchina))
      '      dr.Delete()
      '    Next
      '  End If

      '  If ds.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(strMacchina)).Length > 0 Then
      '    ds.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(strMacchina))(0).Delete()
      '  End If

      'Else
      '  If dsShared.Tables("TESTA").Select("tb_codice = " & CStrSQL(strMacchina)).Length > 0 Then

      '    Dim drTesta As DataRow = dsShared.Tables("TESTA").Select("tb_codice = " & CStrSQL(strMacchina))(0)
      '    If ds.Tables("TESTA").Select("tb_codice = " & CStrSQL(strMacchina)).Length = 0 Then
      '      ds.Tables("TESTA").ImportRow(drTesta)
      '    Else
      '      ds.Tables("TESTA").Select("tb_codice = " & CStrSQL(strMacchina))(0).Delete()
      '      ds.Tables("TESTA").ImportRow(drTesta)
      '    End If

      '    For Each dr As DataRow In ds.Tables("CORPO").Select("tb_codice = " & CStrSQL(strMacchina))
      '      dr.Delete()
      '    Next

      '    For Each dr As DataRow In dsShared.Tables("CORPO").Select("tb_codice = " & CStrSQL(strMacchina))
      '      If Trim(NTSCStr(dr!tb_nodo)) = "" Then Continue For
      '      ds.Tables("CORPO").ImportRow(dr)
      '    Next

      '  End If

      '  If ds.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(strMacchina)).Length = 0 Then
      '    ds.Tables("MACCHINE").Rows.Add(ds.Tables("MACCHINE").NewRow)
      '  End If

      'End If

      oCldIopc.SetData(dsShared, bDelete)

      'dsShared.AcceptChanges()

      '--------------------------------------------------
      '--- Segnala Dataset non modificato
      '--------------------------------------------------
      bHasChanges = False

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

  Public Overloads Function SalvaMacchine(ByVal dsOut As DataSet) As Boolean
    Dim bResult As Boolean = False
    Try
      '--------------------------------------------------

      'dsOut.AcceptChanges()

      oCldIopc.SetDataMacchina(dsOut, False)

      dsOut.AcceptChanges()
      '--------------------------------------------------
      '--- Segnala Dataset non modificato
      '--------------------------------------------------
      'bHasChanges = False

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

  Public Overridable Function SalvaModalita(bDelete As Boolean) As Boolean
    Dim bReturn As Boolean = False
    Try
      If Not bDelete Then
        If Not TestPreSalvaModalita() Then Return False
      End If

      bReturn = oCldIopc.SalvaModalita(dsShared.Tables("MODA"), bDelete)

      dsShared.AcceptChanges()

      If bReturn Then bHasChanges = False

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try

    Return bReturn
  End Function

  Public Overloads Function TestPreSalvaModalita() As Boolean
    Dim bReturn As Boolean = False
    Try

      For Each dr As DataRow In dsShared.Tables("MODA").Select("", "", DataViewRowState.Added)
        If Trim(NTSCStr(dr!tb_codice)) = "" Then
          ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 128768973837496061, "Il Codice non può essere vuoto o spazio")))
          Return False
        End If
      Next

      bReturn = True
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try

    Return bReturn
  End Function

  Public Overridable Function RipristinaIopc(ByVal strTabella As String, ByVal nRow As Integer, ByVal strFilter As String) As Boolean
    Try
      '--------------------------------------------------
      '--- Annulla modifiche 
      '--------------------------------------------------
      dsShared.Tables(strTabella).Select(strFilter)(nRow).RejectChanges()
      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return True
    Catch ex As Exception
    End Try
  End Function

  Public Overridable Function RipristinaIopc(ByVal dsIopc As DataSet, ByVal strTabella As String, ByVal nRow As Integer, ByVal strFilter As String) As Boolean
    Try
      '--------------------------------------------------
      '--- Annulla modifiche 
      '--------------------------------------------------
      dsIopc.Tables(strTabella).Select(strFilter)(nRow).RejectChanges()
      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return True
    Catch ex As Exception
    End Try
  End Function

  Public Overrides Sub SetDefaultValue(ByRef ds As DataSet)
    Try
      '--------------------------------------------------
      '--- Imposto i valori di default TESTA
      '--------------------------------------------------
      With ds.Tables("TESTA")
        .Columns("codditt").DefaultValue = strDittaCorrente
        .Columns("tb_codice").DefaultValue = strMacchina
        .Columns("tb_indirizzo").DefaultValue = ""
        .Columns("tb_utente").DefaultValue = ""
        .Columns("tb_password").DefaultValue = ""
        .Columns("tb_elaborazioneautom").DefaultValue = "N"
        .Columns("tb_secelaborazione").DefaultValue = 0
      End With

      With ds.Tables("CORPO")
        .Columns("codditt").DefaultValue = strDittaCorrente
        .Columns("tb_codice").DefaultValue = strMacchina
        .Columns("tb_nodo").DefaultValue = ""
        .Columns("tb_tiponodo").DefaultValue = ""
        .Columns("tb_ordinamento").DefaultValue = 0
      End With

      With ds.Tables("MACCHINE")
        .Columns("codditt").DefaultValue = strDittaCorrente
        .Columns("tb_codice").DefaultValue = strMacchina
        .Columns("tb_batch").DefaultValue = "BNHHCOOP"
        .Columns("tb_stato").DefaultValue = 0
        .Columns("tb_startstop").DefaultValue = 0
        .Columns("tb_idprocess").DefaultValue = 0
      End With

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overloads Sub SetDefaultValueModalita(ByRef ds As DataSet)
    Try

      '--------------------------------------------------
      '--- Imposto i valori di default MODA
      '--------------------------------------------------
      'Imposto i valori di default diversi da quelli impostati nel database
      ds.Tables("MODA").Columns("codditt").DefaultValue = strDittaCorrente
      ds.Tables("MODA").Columns("tb_lettura").DefaultValue = "N"
      ds.Tables("MODA").Columns("tb_scrittura").DefaultValue = "N"

    Catch ex As Exception
      '--------------------------------------------------------------

      CLN__STD.GestErr(ex, Me, "")

      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Overloads Function TestPreSalva(ByVal strTabella As String) As Boolean
    'Dim dtrTmp As DataRow() = dsShared.Tables(strTabella).Select(Nothing, Nothing, DataViewRowState.Added Or DataViewRowState.ModifiedCurrent)
    'Dim dttTmp As New DataTable

    Try
      ''--------------------------------------------------
      ''--- Esce se non ci sono CHIAMATE in gestione 
      ''--------------------------------------------------
      'If lCodconn = 0 Then Return True

      ''--------------------------------------------------
      ''--- Cicla sulle righe da validare
      ''--------------------------------------------------
      ''For i As Integer = 0 To (dtrTmp.Length - 1)

      ''Next

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return True

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return False
    Finally
      'dttTmp.Clear()
      'dttTmp.Dispose()
    End Try
  End Function

  Public Overridable Function RecordSalva(ByVal nRow As Integer, ByVal bDelete As Boolean, ByRef dtrDeleted As DataRow) As Boolean
    Try

      If bDelete Then
        dtrDeleted = dsShared.Tables("CORPO").Rows(nRow)
        dtrDeleted.Delete()
        dsShared.AcceptChanges()
        Return True
      End If

      If NTSCStr(dsShared.Tables("CORPO").Rows(nRow) !tb_nodo) = "" Then
        ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 127791222113437500, "Attenzione! Il tipo non può essere vuoto")))
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.SETFOCUS, "grNodi.tb_nodo"))
        Return False
      End If
      dsShared.Tables("CORPO").AcceptChanges()

      Return True
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function ValCodcieDb(strCodice As String, strNomeTabella As String, strTipoCod As String, ByRef strDescampo As String, ByRef dttTable As DataTable) As Boolean
    Try
      Return oCldIopc.ValCodiceDb(strCodice, strDittaCorrente, strNomeTabella, strTipoCod, strDescampo, dttTable)
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function ConnessioneOPCUA(ByVal clientOPC As UaClient) As Boolean
    Dim bReturn As Boolean = False
    Try
      If Not clientOPC Is Nothing Then
        If Not clientOPC.Session Is Nothing Then
          If clientOPC.Status = Doors.Opc.Common.OpcStatus.Connected And clientOPC.Session.Connected Then
            bReturn = True
          End If
        End If
      End If
      Return bReturn
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function ConnettiOPCUA(ByRef clientOPC As UaClient, ByRef subscribeOPC As Opc.Ua.Client.Subscription) As Boolean
    Dim bReturn As Boolean = False
    Try
      Dim options As New UaClientOptions
      Dim dtConfig As New DataTable

      'ApriConnessione(treeConnessioni.SelectedNode.Name, dtConfig)

      If Not subscribeOPC Is Nothing Then subscribeOPC.Dispose()

      If Not clientOPC Is Nothing Then
        If Not clientOPC.Session Is Nothing Then
          clientOPC.Session.Dispose()
        End If
      End If

      dtConfig = dsShared.Tables("TESTA")

      Try
        Dim serverUrl As New Uri(NTSCStr(dtConfig.Rows(0) !tb_indirizzo))

        'clientOPC = New UaClient(serverUrl)
        If Trim(NTSCStr(dtConfig.Rows(0) !tb_utente)) <> "" Then
          options.UserIdentity = New Opc.Ua.UserIdentity(NTSCStr(dtConfig.Rows(0) !tb_utente), NTSCStr(dtConfig.Rows(0) !tb_password))
          'options.UseMessageSecurity = False
          clientOPC = New UaClient(New Uri(NTSCStr(dtConfig.Rows(0) !tb_indirizzo)), options)
        Else
          'options.UseMessageSecurity = False
          clientOPC = New UaClient(New Uri(NTSCStr(dtConfig.Rows(0) !tb_indirizzo)), options)
        End If

        'client.ObjRoot = Opc.Ua.ObjectIds.RootFolder
        clientOPC.Connect()

        If Not subscribeOPC Is Nothing Then
          subscribeOPC.Dispose()
          subscribeOPC = New Opc.Ua.Client.Subscription
        End If
        subscribeOPC = clientOPC.CreateSubscription()

        clientOPC.Session.OperationTimeout = 2000
        'client.Session.SessionTimeout = 2000

        AddHandler clientOPC.ServerConnectionLost, AddressOf client_ServerConnectionLost
        'AddHandler client.Session.SessionClosing, AddressOf client_SessionClosing

        'AddHandler client.ServerConnectionLost, AddressOf client_ServerConnectionLost
        ''AddHandler client.Session.SessionClosing, AddressOf client_SessionClosing

        ''bConncet = True
        'strNodoConnect = cs(dtConfig.Rows(0) !cn_nome)

        'WriteLog(cs(dtConfig.Rows(0) !cn_nome) & " Connesso! ", Type.OK)

        bReturn = True
      Catch ex As Exception
        'WriteLog("Errore in Connetti Client OpcUa: " & ex.Message, Type.ERRORE)
        'listLog.Items.Add("Errore in Connetti Client OpcUa: " & ex.Message)
        'MsgBox("Errore in Connetti Client OpcUa: " & ex.Message)
      End Try
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
      'MsgBox("Errore in ConnettiOPCUA: " & ex.Message)
    End Try
    Return bReturn
  End Function

  Public Overridable Sub client_ServerConnectionLost(sender As Object, e As EventArgs)
    Try
      Dim clientOPC As UaClient = CType(sender, UaClient)

      clientOPC.Session.Dispose()
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub LeggiNodi(ByVal clientOPC As UaClient, fullPath As String, ByRef dtNodes As DataTable)
    Try
      'Dim fullPath As String = ""
      'Dim obj As NTSTreeView = tree

      'Dim root As TreeNode = nodeOut

      'If Not root Is Nothing Then
      '  root.Nodes.Clear()
      '  fullPath = root.FullPath
      '  obj = CType(root, NTSTreeView)
      'End If

      dtNodes = New DataTable

      dtNodes.Columns.Add("tb_idnodo", GetType(String))
      dtNodes.Columns.Add("tb_nodo", GetType(String))

      Dim nodes As IEnumerable(Of UaNode) = clientOPC.ExploreFolder(fullPath)

      For Each node As UaNode In nodes
        dtNodes.Rows.Add(node.NodeId, node.Name)
        'obj.Nodes.Add(node.NodeId, node.Name)
        'Dim nodeadd As TreeNode = obj.Nodes.Item(node.NodeId)
        'nodeadd.Nodes.Add("")
        'nodeadd.Collapse()
      Next

      'If Not root Is Nothing Then obj.ExpandAll()

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
      'MsgBox("Errore in LeggiNodi: " & ex.Message)
    End Try
  End Sub

  Public Overridable Sub AggiungiRiga(ByVal clientOPC As UaClient, strNodo As String)
    Try
      If clientOPC Is Nothing Then Return
      If clientOPC.Status <> Doors.Opc.Common.OpcStatus.Connected Then Return

      Dim drNodo As DataRow = dsShared.Tables("CORPO").NewRow

      drNodo!tb_nodo = strNodo
      If Trim(NTSCStr(drNodo!tb_nodo)) = "" Then Return

      Dim val As Object = clientOPC.Read(Of Object)(strNodo).Value

      If Not val Is Nothing Then
        drNodo!tb_tiponodo = val.GetType.FullName
        If val.GetType.FullName = "Opc.Ua.ExtensionObject" Or val.GetType.FullName = "Opc.Ua.ExtensionObject[]" Then
          'drNodi!valore = "Doppio Click"
        Else
          'drNodi!valore = NTSCStr(val.ToString())
        End If
      End If
      dsShared.Tables("CORPO").Rows.Add(drNodo)
      dsShared.Tables("CORPO").AcceptChanges()

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub GestisciMonitorOPC(ByVal clientOPC As UaClient, ByVal subscribeOPC As Opc.Ua.Client.Subscription)
    Try
      Try
        If Not ConnessioneOPCUA(clientOPC) Then Return

        If dsShared.Tables("CORPO").Rows.Count > 0 Then
          For Each drNodi As DataRow In dsShared.Tables("CORPO").Rows
            Application.DoEvents()
            Try
              If clientOPC.FindNode(NTSCStr(drNodi!tb_nodo)) Is Nothing Then Continue For
            Catch ex As Exception
              Continue For
            End Try
            Dim o As Object
            Try
              o = subscribeOPC.MonitoredItems.Single(Function(n) n.DisplayName = NTSCStr(drNodi!tb_nodo))
            Catch ex As Exception
              clientOPC.MonitorOPC(Of Object)(NTSCStr(drNodi!tb_nodo), subscribeOPC, Sub(value, unsubscribe, nodo)
                                                                                       'If nodoEdit = cs(drNodi!nd_nodo) Then Return
                                                                                       If Not value.Value Is Nothing Then
                                                                                         'x = node.Name & ": " & value.Value.ToString
                                                                                         'x = value.Value.GetType.Name

                                                                                         'For Each dr As DataRow In dsShared.Tables("CORPO").Select("tb_nodo = " & CStrSQL(nodo))
                                                                                         '  If dr Is Nothing Then Continue For
                                                                                         '  'If dr.RowState = DataRowState.Added Then Continue For
                                                                                         '  If value.Value.GetType.FullName = "Opc.Ua.ExtensionObject" Then
                                                                                         '    dr!valore = "Doppio Click"
                                                                                         '  Else
                                                                                         '    dr!valore = NTSCStr(value.Value.ToString)
                                                                                         '  End If
                                                                                         'Next

                                                                                         If dsShared.Tables("CORPO").Select("tb_nodo = " & CStrSQL(nodo)).Length > 0 Then
                                                                                           Dim dr As DataRow = dsShared.Tables("CORPO").Select("tb_nodo = " & CStrSQL(nodo))(0)
                                                                                           If Not dr Is Nothing Then
                                                                                             'If dr.RowState = DataRowState.Added Then Continue For
                                                                                             If value.Value.GetType.FullName = "Opc.Ua.ExtensionObject" Then
                                                                                               'dr!tb_valore = "Doppio Click"
                                                                                               dr!tb_valore = ""
                                                                                             Else
                                                                                               dr!tb_valore = NTSCStr(value.Value.ToString)
                                                                                             End If

                                                                                             ThrowRemoteEvent(New NTSEventArgs("MONITOR_OPCUA", nodo))
                                                                                           End If
                                                                                         End If

                                                                                       End If
                                                                                     End Sub)
            End Try
            Try
              If Not ConnessioneOPCUA(clientOPC) Then Continue For
              Dim val As Object = clientOPC.Read(Of Object)(NTSCStr(drNodi!tb_nodo)).Value

              If Not val Is Nothing Then
                drNodi!tb_tiponodo = val.GetType.FullName
                If val.GetType.FullName = "Opc.Ua.ExtensionObject" Or val.GetType.FullName = "Opc.Ua.ExtensionObject[]" Then
                  drNodi!tb_valore = "Doppio Click"
                Else
                  drNodi!tb_valore = NTSCStr(val.ToString())
                End If
              End If
            Catch ex As Exception
            End Try

          Next

          'dsShared.Tables("CORPO").AcceptChanges()
          If Not subscribeOPC.Created Then subscribeOPC.Create()
          subscribeOPC.ApplyChanges()
        End If
      Catch ex As Exception
      End Try
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub ChiudiOPCUA(ByRef clientOPC As UaClient, ByRef subscribeOPC As Opc.Ua.Client.Subscription)
    Try
      If Not subscribeOPC Is Nothing Then subscribeOPC.Dispose()

      If Not clientOPC Is Nothing Then
        If Not clientOPC.Session Is Nothing Then
          clientOPC.Session.CloseSession(Nothing, True)
          clientOPC.Session.Dispose()
        End If
        clientOPC.Dispose()
        clientOPC = Nothing
      End If
    Catch ex As Exception
      ''--------------------------------------------------
      'CLN__STD.GestErr(ex, Me, "")
      ''--------------------------------------------------
    End Try
  End Sub

  Public Overridable Function LeggiNodo(ByVal clientOPC As UaClient, ByVal strNodo As String) As Object
    Dim oReturn As Object = ""
    Try
      If Not ConnessioneOPCUA(clientOPC) Then Return ""
      oReturn = clientOPC.Read(Of Object)(strNodo).Value
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "Nodo: " & strNodo)
      '--------------------------------------------------
    End Try
    Return oReturn
  End Function

  Public Overridable Function ScriviNodo(ByVal clientOPC As UaClient, ByVal strNodo As String, ByVal oValore As Object) As Boolean
    Dim bReturn As Boolean = False
    Try
      Try
        If Not ConnessioneOPCUA(clientOPC) Then Return False
        'clientOPC.Write(Of Object)(strNodo, oValore)
        clientOPC.Write(strNodo, oValore)
        bReturn = True
      Catch ex As Doors.Opc.Common.OpcException
        bReturn = False
      End Try
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
    Return bReturn
  End Function

  Public Overridable Function TrovaNodo(ByVal strNodoCerca As String, ByRef strNodo As String) As Boolean
    Dim bReturn As Boolean = False
    Try
      strNodo = ""

      If dsShared.Tables("CORPO").Select("tb_nodo LIKE '%" & strNodoCerca & "%'").Length > 0 Then
        strNodo = NTSCStr(dsShared.Tables("CORPO").Select("tb_nodo LIKE '%" & strNodoCerca & "%'")(0) !tb_nodo)
      End If

      If Trim(strNodo) = "" Then Return False

      bReturn = True
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
    Return bReturn
  End Function

  Public Overridable Function TrovaNodo(ByVal dtCorpo As DataTable, ByVal strNodoCerca As String, ByRef strNodo As String) As Boolean
    Dim bReturn As Boolean = False
    Try
      strNodo = ""

      If dtCorpo.Select("tb_nodo LIKE '%" & strNodoCerca & "%'").Length > 0 Then
        strNodo = NTSCStr(dtCorpo.Select("tb_nodo LIKE '%" & strNodoCerca & "%'")(0) !tb_nodo)
      End If

      If Trim(strNodo) = "" Then Return False

      bReturn = True
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
    Return bReturn
  End Function

  Public Overridable Function TrovaValore(ByVal strNodoCerca As String, ByRef strValore As String) As Boolean
    Dim bReturn As Boolean = False
    Try
      strValore = ""

      If dsShared.Tables("CORPO").Select("tb_nodo LIKE '%" & strNodoCerca & "%'").Length > 0 Then
        strValore = NTSCStr(dsShared.Tables("CORPO").Select("tb_nodo LIKE '%" & strNodoCerca & "%'")(0) !tb_valore)
        Return True
      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
    Return bReturn
  End Function

  Public Overridable Function TrovaValore(ByVal dtCorpo As DataTable, ByVal strNodoCerca As String, ByRef strValore As String) As Boolean
    Dim bReturn As Boolean = False
    Try
      strValore = ""

      If dtCorpo.Select("tb_nodo LIKE '%" & strNodoCerca & "%'").Length > 0 Then
        strValore = NTSCStr(dtCorpo.Select("tb_nodo LIKE '%" & strNodoCerca & "%'")(0) !tb_valore)
        Return True
      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
    Return bReturn
  End Function

  Public Overridable Function cOpc(strNodo As String, oValore As Object) As Object
    Dim oReturn As Object = oValore
    Try

      'If dsShared.Tables("CORPO").Select("tb_nodo = " & CStrSQL(strNodo)).Length = 0 Then Return oReturn

      'Dim strTipo As String = NTSCStr(dsShared.Tables("CORPO").Select("tb_nodo = " & CStrSQL(strNodo))(0) !tb_tiponodo)

      'Select Case strTipo
      '  Case GetType(Int16).Name
      '    oReturn = Int16.Parse(oValore.ToString())
      '  Case GetType(Int32).Name
      '    oReturn = Int32.Parse(oValore.ToString())
      '  Case GetType(Int64).Name
      '    oReturn = Int64.Parse(oValore.ToString())
      '  Case GetType(UInt16).FullName
      '    oReturn = UInt16.Parse(oValore.ToString())
      '  Case GetType(UInt32).FullName
      '    oReturn = UInt32.Parse(oValore.ToString())
      '  Case GetType(UInt64).FullName
      '    oReturn = UInt64.Parse(oValore.ToString())
      '  Case GetType(Integer).FullName
      '    oReturn = Integer.Parse(oValore.ToString())
      '  Case GetType(Decimal).Name
      '    oReturn = Decimal.Parse(oValore.ToString())
      '  Case GetType(Double).Name
      '    oReturn = Double.Parse(oValore.ToString())
      '  Case GetType(Single).Name, "System.Float"
      '    oReturn = Single.Parse(oValore.ToString())
      '  Case GetType(Boolean).Name
      '    oReturn = Boolean.Parse(oValore.ToString())
      '  Case GetType(String).Name
      '    oReturn = oValore.ToString
      '  Case Else
      '    oReturn = oValore.ToString
      'End Select

      oReturn = cOpc(dsShared.Tables("CORPO"), strNodo, oValore)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
    Return oReturn
  End Function

  Public Overridable Function cOpc(dtCotpo As DataTable, strNodo As String, oValore As Object) As Object
    Dim oReturn As Object = oValore
    Try

      If dtCotpo.Select("tb_nodo = " & CStrSQL(strNodo)).Length = 0 Then Return oReturn

      Dim strTipo As String = NTSCStr(dtCotpo.Select("tb_nodo = " & CStrSQL(strNodo))(0) !tb_tiponodo)

      Select Case strTipo
        Case GetType(Int16).FullName
          oReturn = Int16.Parse(oValore.ToString())
        Case GetType(Int32).FullName
          oReturn = Int32.Parse(oValore.ToString())
        Case GetType(Int64).FullName
          oReturn = Int64.Parse(oValore.ToString())
        Case GetType(UInt16).FullName
          oReturn = UInt16.Parse(oValore.ToString())
        Case GetType(UInt32).FullName
          oReturn = UInt32.Parse(oValore.ToString())
        Case GetType(UInt64).FullName
          oReturn = UInt64.Parse(oValore.ToString())
        Case GetType(Integer).FullName
          oReturn = Integer.Parse(oValore.ToString())
        Case GetType(Decimal).FullName
          oReturn = Decimal.Parse(oValore.ToString())
        Case GetType(Double).FullName
          oReturn = Double.Parse(oValore.ToString())
        Case GetType(Single).FullName, "System.Float"
          oReturn = Single.Parse(oValore.ToString())
        Case GetType(Boolean).FullName
          oReturn = Boolean.Parse(CBool(oValore).ToString())
        Case GetType(String).FullName
          oReturn = oValore.ToString
        Case Else
          oReturn = oValore.ToString
      End Select

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
    Return oReturn
  End Function
#End Region

#Region "Eventi AfterColUpdate e BeforeColUpdate"
  Public Overridable Sub AfterColUpdateTESTA(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Try
      '--------------------------------------------------
      '--- Esce se non abilitata Validazione
      '--------------------------------------------------
      'If lCodconn = 0 Then Return
      If bNonValidare Then Return

      '--------------------------------------------------
      '--- Non valido la colonna se il dato non è cambiato
      '--------------------------------------------------
      If strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";") > -1 Then
        strPrevCelValue = strPrevCelValue.Remove(strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";"), e.Column.ColumnName.ToUpper.Length + 1)
        Return
      End If

      bHasChanges = True

      '--------------------------------------------------
      '--- Comunico che una cella è cambiata, per fare in modo che se il dato è contenuto in una griglia 
      '--- vengano fatte le routine di validazione del caso
      '--------------------------------------------------
      ThrowRemoteEvent(New NTSEventArgs("GRIAGG", e.Column.Table.TableName & "§" & e.Column.ColumnName))

      e.Row.EndEdit()
      e.Row.EndEdit()

      '--------------------------------------------------
      '--- Cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
      '--------------------------------------------------
      Dim strFunction As String = "AfterColUpdateTESTA_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub BeforeColUpdateTESTA(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Dim strErr As String = ""
    Try
      '--------------------------------------------------
      '--- Esce se non abilitata Validazione
      '--------------------------------------------------
      'If lCodconn = 0 Then Return
      If bNonValidare Then Return

      '--------------------------------------------------
      '--- Memorizzo il valore corrente di cella per testarlo nella AfterColUpdate
      '--- solo se il dato è uguale a quello precedentemente contenuto nella cella
      '--------------------------------------------------
      If ValoriUguali(e.ProposedValue.ToString, e.Row(e.Column.ColumnName).ToString) Then
        strPrevCelValue += e.Column.ColumnName.ToUpper + ";"
        Return
      End If

      '--------------------------------------------------
      '--- Controllo che in una cella short non venga inserito un numero troppo grande
      '--------------------------------------------------
      If Not CheckCellaShort(e, strErr) Then Throw New NTSException(strErr)

      '--------------------------------------------------
      '--- Cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
      '--------------------------------------------------
      Dim strFunction As String = "BeforeColUpdateTESTA_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub AfterColUpdateCORPO(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Try
      '--------------------------------------------------
      '--- Esce se non abilitata Validazione
      '--------------------------------------------------
      'If lCodconn = 0 Then Return
      If bNonValidare Then Return

      '--------------------------------------------------
      '--- Non valido la colonna se il dato non è cambiato
      '--------------------------------------------------
      If strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";") > -1 Then
        strPrevCelValue = strPrevCelValue.Remove(strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";"), e.Column.ColumnName.ToUpper.Length + 1)
        Return
      End If

      bHasChanges = True

      '--------------------------------------------------
      '--- Comunico che una cella è cambiata, per fare in modo che se il dato è contenuto in una griglia 
      '--- vengano fatte le routine di validazione del caso
      '--------------------------------------------------
      ThrowRemoteEvent(New NTSEventArgs("GRIAGG", e.Column.Table.TableName & "§" & e.Column.ColumnName))

      e.Row.EndEdit()
      e.Row.EndEdit()

      '--------------------------------------------------
      '--- Cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
      '--------------------------------------------------
      Dim strFunction As String = "AfterColUpdateCORPO_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub BeforeColUpdateCORPO(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Dim strErr As String = ""
    Try
      '--------------------------------------------------
      '--- Esce se non abilitata Validazione
      '--------------------------------------------------
      'If lCodconn = 0 Then Return
      If bNonValidare Then Return

      '--------------------------------------------------
      '--- Memorizzo il valore corrente di cella per testarlo nella AfterColUpdate
      '--- solo se il dato è uguale a quello precedentemente contenuto nella cella
      '--------------------------------------------------
      If ValoriUguali(e.ProposedValue.ToString, e.Row(e.Column.ColumnName).ToString) Then
        strPrevCelValue += e.Column.ColumnName.ToUpper + ";"
        Return
      End If

      '--------------------------------------------------
      '--- Controllo che in una cella short non venga inserito un numero troppo grande
      '--------------------------------------------------
      If Not CheckCellaShort(e, strErr) Then Throw New NTSException(strErr)

      '--------------------------------------------------
      '--- Cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
      '--------------------------------------------------
      Dim strFunction As String = "BeforeColUpdateCORPO_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub BeforeColUpdateCORPO_tb_nodo(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Dim dtrTmp() As DataRow
    Dim strErrore As String = ""

    Try
      '--------------------------------------------------
      '--- Verifica codice già esistente
      '--------------------------------------------------
      If dsShared.Tables("CORPO").Rows.Count > 1 Then
        dtrTmp = dsShared.Tables("CORPO").Select("tb_nodo = " & CStrSQL(e.ProposedValue.ToString()))
        If dtrTmp.Length > 0 Then
          ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 127791222113437500, "Attenzione! Riga già esistente")))
          e.ProposedValue = e.Row(e.Column.ColumnName)
          Return
        End If
      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub BeforeColUpdateMODA(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
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
      Dim strFunction As String = "BeforeColUpdateMODA_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub BeforeColUpdateMODA_tb_codice(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Dim dtrTmp() As DataRow = Nothing
    Dim strErrore As String = ""
    Try

      If Trim(NTSCStr(e.ProposedValue.ToString())) = "" Then
        e.ProposedValue = e.Row(e.Column.ColumnName)
        ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 128768973837496061, "Il Codice non può essere vuoto o spazio")))
        Return
      End If

      If dsShared.Tables("MODA").Rows.Count > 1 Then
        dtrTmp = dsShared.Tables("MODA").Select("tb_codice = " & CStrSQL(e.ProposedValue))
        If dtrTmp.Length > 0 Then
          e.ProposedValue = e.Row(e.Column.ColumnName)
          ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 128182575973437500, "Codice già presente!")))
          Return
        End If
      End If

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub AfterColUpdateMODA(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)

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
      Dim strFunction As String = "AfterColUpdateMODA_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub
#End Region

End Class