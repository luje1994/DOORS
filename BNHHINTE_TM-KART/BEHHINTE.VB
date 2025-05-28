Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System
Imports System.Net
Imports System.IO
Imports System.Xml
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class CLEHHINTE
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

#Region "Oggetto DAL"
  Public oCldInte As CLDHHINTE
#End Region

#Region "Oggetto Entity VEBOLL per AUTOSTORE"
  Public oCleBoll As CLEVEBOLL
#End Region

#Region "Variabili"
  'Variabili per validazione eventi before/after update
  Public bNonValidare As Boolean
  Public bInOnAddNewRow As Boolean
  Public bInUnload As Boolean

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
    If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHINTE"
    MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")
    oCldInte = CType(MyBase.ocldBase, CLDHHINTE)
    oCldInte.Init(oApp)

    '--------------------------------------------------
    '--- Valore funzione
    '--------------------------------------------------
    Return True

  End Function
#End Region

#Region "Funzioni e Routines di Gestione CHIAMATE"
  Public Overloads Function Apri(ByRef dsOut As DataSet) As Boolean
    Dim dReturn As Boolean = False
    Try
      '--------------------------------------------------
      '--- Tengo un puntatore al datatable che verrà usato nell'UI
      '--- quindi chiedo i dati al DAL
      '--------------------------------------------------
      dReturn = oCldInte.GetData(dsOut)

      '--------------------------------------------------
      '--- Esce se c'è stato un errore
      '--------------------------------------------------
      If dReturn = False Then Return False

      '--------------------------------------------------
      '--- Imposta valori di default
      '--------------------------------------------------
      SetDefaultValue(dsOut)

      If dsOut.Tables("TESTA").Rows.Count > 0 Then
        If NTSCStr(dsOut.Tables("TESTA").Rows(0) !tb_oragiac1) = "" Then
          dsOut.Tables("TESTA").Rows(0) !tb_oragiac1 = "00:00"
        End If
        If NTSCStr(dsOut.Tables("TESTA").Rows(0) !tb_oragiac2) = "" Then
          dsOut.Tables("TESTA").Rows(0) !tb_oragiac2 = "00:00"
        End If
        If NTSCStr(dsOut.Tables("TESTA").Rows(0) !tb_oragiac3) = "" Then
          dsOut.Tables("TESTA").Rows(0) !tb_oragiac3 = "00:00"
        End If
      End If

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

  Public Overloads Function Salva(ByVal bDelete As Boolean) As Boolean
    Dim bResult As Boolean = False
    Try
      ''--------------------------------------------------
      ''--- Controlli pre-salvataggio (solo se non è una delete)
      ''--------------------------------------------------
      If Not bDelete Then
        If Not TestPreSalva("TESTA") Then Return False
        If Not TestPreSalva("CORPO") Then Return False
      End If

      '--------------------------------------------------
      '--- Salva il dataset in formato XML nel registro Business
      '--------------------------------------------------
      'Scrive il dataset in XML
      Dim sbStrTesta As New System.Text.StringBuilder
      Dim swXmlTesta As System.IO.StringWriter = New System.IO.StringWriter(sbStrTesta)

      dsShared.Tables("TESTA").WriteXml(swXmlTesta, XmlWriteMode.IgnoreSchema)
      Dim strXmlTesta As String = sbStrTesta.ToString()
      'In bOperatore indicazione di connessione specifico per operatore
      oCldInte.SaveSettingBus("BSHHINTE", "OPZIONI", ".", "HH_INTESTA", strXmlTesta, " ", False, True, False)

      Dim sbStrCorpo As New System.Text.StringBuilder
      Dim swXmlCorpo As System.IO.StringWriter = New System.IO.StringWriter(sbStrCorpo)

      For Each dr As DataRow In dsShared.Tables("CORPO").Select("tb_tipork = ''")
        dr.Delete()
        dsShared.AcceptChanges()
      Next

      dsShared.Tables("CORPO").WriteXml(swXmlCorpo, XmlWriteMode.IgnoreSchema)
      Dim strXmlCorpo As String = sbStrCorpo.ToString()
      'In bOperatore indicazione di connessione specifico per operatore
      oCldInte.SaveSettingBus("BSHHINTE", "OPZIONI", ".", "HH_INCORPO", strXmlCorpo, " ", False, True, False)

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

  Public Overridable Function RipristinaInte(ByVal strTabella As String, ByVal nRow As Integer, ByVal strFilter As String) As Boolean
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

  Public Overrides Sub SetDefaultValue(ByRef ds As DataSet)
    Try
      '--------------------------------------------------
      '--- Imposto i valori di default TESTA
      '--------------------------------------------------
      With ds.Tables("TESTA")
        .Columns("tb_urlchia").DefaultValue = ""
        .Columns("tb_elaborazioneautom").DefaultValue = "N"
        .Columns("tb_elaborazionegiac1").DefaultValue = "N"
        .Columns("tb_oragiac1").DefaultValue = "00:00"
        .Columns("tb_elaborazionegiac2").DefaultValue = "N"
        .Columns("tb_oragiac2").DefaultValue = "00:00"
        .Columns("tb_elaborazionegiac3").DefaultValue = "N"
        .Columns("tb_oragiac3").DefaultValue = "00:00"
      End With

      With ds.Tables("CORPO")
        .Columns("tb_tipork").DefaultValue = ""
        .Columns("tb_serie").DefaultValue = ""
        .Columns("tb_operazione").DefaultValue = ""
        .Columns("tb_magazmacchina").DefaultValue = 0
        .Columns("xx_descrmagmacchina").DefaultValue = ""
        .Columns("tb_magazprincipale").DefaultValue = 0
        .Columns("xx_descrmagprincipale").DefaultValue = ""
      End With

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
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

      If NTSCStr(dsShared.Tables("CORPO").Rows(nRow) !tb_tipork) = "" Then
        ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 127791222113437500, "Attenzione! Il tipo non può essere vuoto")))
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.SETFOCUS, "grDoc.tb_tipork"))
        Return False
      End If

      Return True
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function ValCodcieDb(strCodice As String, strNomeTabella As String, strTipoCod As String, ByRef strDescampo As String, ByRef dttTable As DataTable) As Boolean
    Try
      Return oCldInte.ValCodiceDb(strCodice, strDittaCorrente, strNomeTabella, strTipoCod, strDescampo, dttTable)
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function getMagazzinoMacchina() As Integer
    Try
      Dim dsInte As New DataSet
      Apri(dsInte)
      If dsInte.Tables("TESTA").Rows.Count = 0 Then Return 0
      Return NTSCInt(dsInte.Tables("TESTA").Rows(0) !tb_magazzinoMacchina)
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function

  Public Overridable Function getMagazzinoPricipale() As Integer
    Try
      Dim dsInte As New DataSet
      Apri(dsInte)
      If dsInte.Tables("TESTA").Rows.Count = 0 Then Return 0
      Return NTSCInt(dsInte.Tables("TESTA").Rows(0) !tb_magazzinoPricipale)
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Function
#End Region

#Region "Funzioni e Routines di elaborazione CHIAMATE"
  ''' <summary>
  ''' Chiamata al servizio di connessione MAGAZZINO
  ''' </summary>
  Public Overridable Function PostXML(ByVal strXMLData As String, ByVal strURL As String) As HttpWebRequest
    'Dichiarazione oggetto HttpWebRequest da restituire
    Dim objhttpWebRequest As HttpWebRequest = Nothing
    Try
      '-------------------------------------------------
      '--- Impostazione HttpWebRequest
      '-------------------------------------------------
      Dim httpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
      httpWebRequest.ContentType = "text/html"
      httpWebRequest.Method = "POST"
      'httpWebRequest.Host = "192.168.100.124"
      ''L'impostazione di:
      ''TransferEncoding = "utf-8" e SendChunked = True senza la quale non è possibile impostare TransferEncoding
      ''consente l'invio di caratteri "strani"
      ''Es.Termostato NC x resistenze 0-60°C
      ''Senza le due righe seguenti la chiamata viene rifiutata


      Dim XMLByte() As Byte
      Dim DataStream As Stream
      XMLByte = System.Text.Encoding.UTF8.GetBytes(strXMLData)

      httpWebRequest.ContentLength = XMLByte.Length

      '-------------------------------------------------
      '--- Invio dati JSON
      '-------------------------------------------------
      DataStream = httpWebRequest.GetRequestStream()
      DataStream.Write(XMLByte, 0, XMLByte.Length)
      DataStream.Close()

      'httpWebRequest.SendChunked = True
      'httpWebRequest.TransferEncoding = "utf-8"
      'httpWebRequest.ContentLength = strXMLData.Length

      ''-------------------------------------------------
      ''--- Invio dati XML
      ''-------------------------------------------------
      'Using streamWriter As StreamWriter = New StreamWriter(httpWebRequest.GetRequestStream(), Text.Encoding.GetEncoding("utf-8"))
      '  streamWriter.Write(strXMLData)
      '  streamWriter.Flush()
      '  streamWriter.Close()
      'End Using

      '-------------------------------------------------
      '--- Impostazione oggetto HttpWebRequest da restituire
      '-------------------------------------------------
      objhttpWebRequest = httpWebRequest

      '-------------------------------------------------
      '--- Valore funzione
      '-------------------------------------------------
      Return objhttpWebRequest

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione AUTOSTORE
  ''' </summary>
  Public Overridable Function PostJSON(ByVal strJsonData As String, ByVal strURL As String) As HttpWebRequest
    'Dichiarazione oggetto HttpWebRequest da restituire
    Dim objhttpWebRequest As HttpWebRequest = Nothing
    Try
      '-------------------------------------------------
      '--- Impostazione HttpWebRequest
      '-------------------------------------------------

      Dim httpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
      httpWebRequest.ContentType = "application/json"
      httpWebRequest.Method = "POST"
      'L'impostazione di:
      'TransferEncoding = "utf-8" e SendChunked = True senza la quale non è possibile impostare TransferEncoding
      'consente l'invio di caratteri "strani"
      'Es.Termostato NC x resistenze 0-60°C
      'Senza le due righe seguenti la chiamata viene rifiutata
      httpWebRequest.SendChunked = True
      httpWebRequest.TransferEncoding = "utf-8"
      httpWebRequest.ContentLength = strJsonData.Length

      '-------------------------------------------------
      '--- Invio dati JSON
      '-------------------------------------------------
      Using streamWriter As StreamWriter = New StreamWriter(httpWebRequest.GetRequestStream(), Text.Encoding.GetEncoding("utf-8"))
        streamWriter.Write(strJsonData)
        streamWriter.Flush()
        streamWriter.Close()
      End Using

      '-------------------------------------------------
      '--- Impostazione oggetto HttpWebRequest da restituire
      '-------------------------------------------------
      objhttpWebRequest = httpWebRequest

      '-------------------------------------------------
      '--- Valore funzione
      '-------------------------------------------------
      Return objhttpWebRequest

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Recupero esito chiamata al servizio di connessione MAGAZZINO
  ''' </summary>
  Public Overridable Function postHttp(HttpWebRequest As HttpWebRequest, ByRef strEsito As String) As Boolean
    'Dichiarazione variabili
    Dim strMsg As String = ""
    Dim webResponse As System.Net.WebResponse = Nothing
    Dim strResponse As String = ""
    'Dim objXML As JObject

    Try
      '--------------------------------------------------
      '--- Recupera risposta chiamata
      '--------------------------------------------------
      strEsito = ""
      If HttpWebRequest Is Nothing Then
        Return False
      Else
        Try
          Try
            webResponse = HttpWebRequest.GetResponse()
          Catch e As WebException
            If (e.Status = WebExceptionStatus.ProtocolError) Then
              strResponse = New System.IO.StreamReader(e.Response.GetResponseStream()).ReadToEnd()
            End If
          End Try
          If strResponse.Length = 0 Then strResponse = New System.IO.StreamReader(webResponse.GetResponseStream()).ReadToEnd()
        Catch e As Exception
        End Try
        'Recupera risposta
        'strResponse = GetResponse(httpWebRequest)
        If strResponse.Trim.Length > 0 Then
          'objXML = JObject.Parse(strResponse)
          strEsito = strResponse
        End If
      End If

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return True

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return False
    End Try
  End Function
#End Region

#Region "Gestione Magazzino Incarico Tech"
  ''' <summary>
  ''' Chiamata al servizio REST di connessione MAGAZZINO per inserimento articoli
  ''' </summary>
  Public Overridable Function postAnagrafiche _
    (
    ByVal strURL As String,
    ByVal strCodArt As String,
    ByRef strEsito As String
    ) As Boolean

    'Dichiarazione variabili
    Dim drArt As DataRow = Nothing
    Dim strXMLData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Datarow dell'articolo da inviare
      '--------------------------------------------------
      drArt = oCldInte.GetArticoli(strDittaCorrente, strCodArt)
      If drArt Is Nothing Then Return False

      '--------------------------------------------------
      '--- Crea XML per invio articolo 
      '--------------------------------------------------
      strXMLData = creaXMLAnagrafiche(drArt)

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostXML(strXMLData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strEsito = ""
      bEsito = postHttp(httpWebRequest, strEsito)
      If bEsito Then
        strStato = "I"
      Else
        strStato = "E"
      End If
      oCldInte.AggiornaEsitoArticoli(strDittaCorrente, strCodArt, strStato, strEsito)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return bEsito
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione MAGAZZINO per inserimento articoli
  ''' </summary>
  Public Overridable Function postAnagrafiche _
    (
    ByVal strURL As String,
    ByRef strEsito As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dtArt As New DataTable
    Dim strXMLData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""
    Dim strCodArt As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Datarow dell'articolo da inviare
      '--------------------------------------------------
      dtArt = oCldInte.GetArticoli(strDittaCorrente)
      If dtArt Is Nothing Then Return False
      If dtArt.Rows.Count = 0 Then Return False

      For Each drArt As DataRow In dtArt.Rows
        strCodArt = NTSCStr(drArt!tb_codart)

        '--------------------------------------------------
        '--- Crea XML per invio articolo 
        '--------------------------------------------------
        strXMLData = creaXMLAnagrafiche(drArt)

        '--------------------------------------------------
        '--- Esegue la chiamata al URL
        '--------------------------------------------------
        httpWebRequest = PostXML(strXMLData, strURL)

        '--------------------------------------------------
        '--- Recupera esito chiamata
        '--------------------------------------------------
        strEsito = ""
        bEsito = postHttp(httpWebRequest, strEsito)
        If bEsito Then
          strStato = "I"
        Else
          strStato = "E"
        End If
        oCldInte.AggiornaEsitoArticoli(strDittaCorrente, strCodArt, strStato, strEsito)
      Next

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return bEsito
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  ''' <summary>
  ''' Compone il XML per insert Articoli
  ''' </summary>
  Public Overridable Function creaXMLAnagrafiche(ByVal drArt As DataRow) As String
    Dim strXMLData As String = ""

    Try
      If drArt Is Nothing Then Return ""

      strXMLData = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
xmlns:main=""http://main.jws.com.hanel.de"" xmlns:xsd=""http://main.jws.com.hanel.de/xsd"">
  <soapenv:Header/>
  <soapenv:Body>
    <main:sendAPDReqV01>
      <main:param>" & vbCrLf

      ''--------------------------------------------------
      '--- Crea XML per insert Articoli
      '--------------------------------------------------

      strXMLData &= "       <xsd:articlePoolDataRecord>" & vbCrLf
      strXMLData &= "         <xsd:articleNumber>" & NTSCStr(drArt!tb_codart) & "</xsd:articleNumber>" & vbCrLf
      strXMLData &= "         <xsd:articleName>" & NTSCStr(drArt!tb_descr) & "</xsd:articleName>" & vbCrLf
      strXMLData &= "       </xsd:articlePoolDataRecord>" & vbCrLf

      strXMLData &= "      </main:param>
    </main:sendAPDReqV01>
  </soapenv:Body>
</soapenv:Envelope>"
      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strXMLData
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione INCARICOTECH per inserimento Lista di Prelievo
  ''' </summary>
  Public Overridable Function postListaDiPrelievo _
    (
    ByVal strURL As String,
    ByVal strLista As String,
    ByRef strEsito As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dtLista As New DataTable
    Dim strXMLData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Datarow della lista di prelievo da inviare
      '--------------------------------------------------
      dtLista = oCldInte.GetListadiPrelievo(strDittaCorrente, strLista)
      If dtLista Is Nothing Then Return False
      If dtLista.Rows.Count = 0 Then Return False

      '--------------------------------------------------
      '--- Crea l'XML per invio lista di prelievo
      '--------------------------------------------------
      strXMLData = creaXMLListaDiPrelievo(dtLista)

      If Not Directory.Exists(Application.StartupPath & "\LogChiamate") Then
        MkDir(Application.StartupPath & "\LogChiamate")
      End If

      Dim strTesto As String = " Inizio " & DateTime.Now.ToShortDateString & " " & DateTime.Now.ToLongTimeString

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostXML(strXMLData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strEsito = ""
      bEsito = postHttp(httpWebRequest, strEsito)
      If bEsito Then
        strStato = "I"
      Else
        strStato = "E"
      End If

      strTesto &= " - Fine " & DateTime.Now.ToShortDateString & " " & DateTime.Now.ToLongTimeString

      File.AppendAllText(Application.StartupPath & "\LogChiamate\LogInvioLista.txt", strTesto & vbCrLf)

      '--------------------------------------------------
      '--- Aggiorna esito chiamata
      '--------------------------------------------------
      oCldInte.AggiornaEsitoListaDiPrelievo(strDittaCorrente, strLista, strStato, strEsito)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return bEsito

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  ''' <summary>
  ''' Compone XML per insert Lista di Prelievo
  ''' </summary>
  Public Overridable Function creaXMLListaDiPrelievo(ByVal dtLista As DataTable) As String
    Dim strXMLData As String = ""

    Try
      If dtLista Is Nothing Then Return ""
      If dtLista.Rows.Count = 0 Then Return ""

      strXMLData = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
xmlns:main=""http://main.jws.com.hanel.de"" xmlns:xsd=""http://main.jws.com.hanel.de/xsd"">
 <soapenv:Header/>
 <soapenv:Body>
 <main:sendJobsReqV01>
 <main:param>
 <xsd:job>" & vbCrLf
      Dim dtTesta As DataTable = dtLista.DefaultView.ToTable(True, "tb_lista")
      For Each drTesta As DataRow In dtTesta.Rows
        strXMLData &= " <xsd:jobNumber>" & NTSCStr(drTesta!tb_lista) & "</xsd:jobNumber>" & vbCrLf
        For Each drCorpo As DataRow In dtLista.Select("tb_lista = " & CStrSQL(drTesta!tb_lista))
          strXMLData &= " <xsd:JobPosition>
 <xsd:articleNumber>" & NTSCStr(drCorpo!tb_codart) & "</xsd:articleNumber>
 <xsd:operation>" & NTSCStr(drCorpo!tb_operazione) & "</xsd:operation>
 <xsd:nominalQuantity>" & NTSCInt(drCorpo!tb_quantita).ToString & "</xsd:nominalQuantity>
 </xsd:JobPosition>xsd:JobPosition>" & vbCrLf
        Next
      Next
      strXMLData &= " </xsd:job>
 </main:param>
 </main:sendJobsReqV01>
 </soapenv:Body>
</soapenv:Envelope>"

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strXMLData
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione INCARICOTECH per prelievo Ordini Elaborati
  ''' </summary>
  Public Overridable Function postOrdiniElaborati _
    (
    ByVal strURL As String,
    ByRef strEsito As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dtLista As New DataTable
    Dim strXMLData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea l'XML per la ricezione Ordini Elaborati
      '--------------------------------------------------
      strXMLData = creaXMLOrdiniElaborati()

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostXML(strXMLData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strEsito = ""
      bEsito = postHttp(httpWebRequest, strEsito)
      'If bEsito Then
      '  strStato = "I"
      'Else
      '  strStato = "E"
      'End If

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return bEsito

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione INCARICOTECH per prelievo Operazioni Manuali
  ''' </summary>
  Public Overridable Function postOperazioniManuali _
    (
    ByVal strURL As String,
    ByRef strEsito As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dtLista As New DataTable
    Dim strXMLData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea l'XML per ila ricezione Operazioni Manuali
      '--------------------------------------------------
      strXMLData = creaXMLOperazioniManuali()

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostXML(strXMLData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strEsito = ""
      bEsito = postHttp(httpWebRequest, strEsito)
      'If bEsito Then
      '  strStato = "I"
      'Else
      '  strStato = "E"
      'End If

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return bEsito

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  ''' <summary>
  ''' Compone XML per get Ordini Elaborati
  ''' </summary>
  Public Overridable Function creaXMLOrdiniElaborati() As String
    Dim strXMLData As String = ""

    Try
      strXMLData = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
xmlns:main = ""http://main.jws.com.hanel.de"" xmlns:xsd = ""http://main.jws.com.hanel.de/xsd"">
 <soapenv:Header/>
 <soapenv:Body>
 <main:readAllJobsReqV01>
 <main:param>
 <xsd:mode>1</xsd:mode>
 </main:param>
 </main:readAllJobsReqV01>
 </soapenv:Body>
</soapenv:Envelope>"

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strXMLData
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Compone XML per get Operazioni Manuali
  ''' </summary>
  Public Overridable Function creaXMLOperazioniManuali() As String
    Dim strXMLData As String = ""

    Try
      strXMLData = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
       xmlns:main=""http://main.jws.com.hanel.de"" xmlns:xsd=""http://main.jws.com.hanel.de/xsd"">
   <soapenv:Header/>
   <soapenv:Body>
      <main:readOpjournalReqV01>
         <main:param>
            <xsd:modus>1</xsd:modus>
         </main:param>
      </main:readOpjournalReqV01>
   </soapenv:Body>
</soapenv:Envelope>"

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strXMLData
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione INCARICOTECH per la richiesta delle giacenze
  ''' </summary>
  Public Overridable Function postGiacenze _
    (
    ByVal strURL As String,
    ByRef strEsito As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dtLista As New DataTable
    Dim strXMLData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea l'XML per invio lista di prelievo
      '--------------------------------------------------
      strXMLData = creaXMLGiacenze()

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostXML(strXMLData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strEsito = ""
      bEsito = postHttp(httpWebRequest, strEsito)
      'If bEsito Then
      '  strStato = "I"
      'Else
      '  strStato = "E"
      'End If

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return bEsito

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  ''' <summary>
  ''' Compone XML per get richiesta Giacenze
  ''' </summary>
  Public Overridable Function creaXMLGiacenze() As String
    Dim strXMLData As String = ""

    Try
      strXMLData = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
xmlns:main=""http://main.jws.com.hanel.de"">
 <soapenv:Header/>
 <soapenv:Body>
 <main:readAllAMDReqV01/>
 </soapenv:Body>
</soapenv:Envelope>"

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strXMLData
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione INCARICO TECH per delete lista di prelievo
  ''' </summary>
  Public Overridable Function postCancellaLista _
    (
    ByVal strURL As String,
    ByVal strLista As String,
    ByRef strEsito As String
    ) As Boolean

    'Dichiarazione variabili
    Dim strXMLData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Json per invio advice 
      '--------------------------------------------------
      strXMLData = creaXMLCancellaLista(strLista)

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostXML(strXMLData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strEsito = ""
      bEsito = postHttp(httpWebRequest, strEsito)
      If bEsito Then
        strStato = "D"
      Else
        strStato = "E"
      End If

      '--------------------------------------------------
      '--- Aggiorna esito chiamata
      '--------------------------------------------------
      oCldInte.AggiornaEsitoListaDiPrelievo(strDittaCorrente, strLista, strStato, strEsito)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return bEsito

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  ''' <summary>
  ''' Compone XML per delete Lista di Prelievo
  ''' </summary>
  Public Overridable Function creaXMLCancellaLista(ByVal strLista As String) As String
    Dim strXMLData As String = ""

    Try
      If strLista = "" Then Return ""

      strXMLData = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
xmlns:main=""http://main.jws.com.hanel.de"" xmlns:xsd=""http://main.jws.com.hanel.de/xsd"">
 <soapenv:Header/>
 <soapenv:Body>
 <main:deleteJobReqV01>
 <main:param>
 <xsd:jobNumber>" & strLista & "</xsd:jobNumber>
 </main:param>
 </main:deleteJobReqV01>
 </soapenv:Body>
</soapenv:Envelope>"

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strXMLData
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function
#End Region

#Region "Funzioni e Routine per invio MAGAZZINO"
  ''' <summary>
  ''' Invia gli articoli al magazzino automatico MAGAZZINO
  ''' </summary>
  Public Overridable Function InviaArticoli _
    (
    ByVal strDitta As String,
    ByVal dttArtico As DataTable,
    ByRef strErrore As String
    ) As Boolean

    Dim dsInte As New DataSet
    Dim strSQL As String = ""
    Dim drTesta As DataRow = Nothing
    Dim strURL As String = ""
    Dim strCodart As String = ""
    Dim strEsito As String = ""

    Try
      dttArtico.AcceptChanges()
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(dsInte) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Magazzino"
        Return False
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If dsInte Is Nothing Then
        strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento lista di prelievo Incarico Tech non configurata"
        Return False
      End If
      drTesta = dsInte.Tables("TESTA").Rows(0)

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      strURL = NTSCStr(drTesta!tb_urlchia)

      If NTSCStr(dttArtico.Rows(0) !ar_hhcheckmag) <> "S" Then Return False

      '-------------------------------------------------
      '--- Inserisce i dati nella tabella 
      '-------------------------------------------------
      For Each dtrArtico As DataRow In dttArtico.Rows
        strCodart = NTSCStr(dtrArtico!ar_codart)

        If Not oCldInte.InserisciDatiArticolo(strDitta, strCodart) Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Errore In selezione dati articolo " + CStrSQL(strCodart) + " per IncaricoTech"
          Return False
        Else
          If Not postAnagrafiche(strURL, strCodart, strEsito) Then Return False
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
  ''' Invia la lista di prelievo al magazzino automatico INCARICOTECH
  ''' </summary>
  Public Overridable Function InviaListaDiPrelievo _
    (
    ByVal strDitta As String,
    ByVal bDelete As Boolean,
    ByVal dsDoc As DataSet,
    ByRef strErrore As String
    ) As Boolean

    Dim dsInte As New DataSet
    Dim strSQL As String = ""
    Dim drTesta As DataRow = Nothing
    Dim dtCorpo As New DataTable
    Dim strURL As String = ""
    Dim strLista As String = ""
    Dim strOperazione As String = ""
    Dim strEsito As String = ""

    Try
      dsDoc.AcceptChanges()
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(dsInte) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Magazzino"
        Return False
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If dsInte Is Nothing Then
        strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento lista di prelievo Incarico Tech non configurata"
        Return False
      End If
      drTesta = dsInte.Tables("TESTA").Rows(0)
      dtCorpo = dsInte.Tables("CORPO")

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      'Lista Prelievo
      strURL = NTSCStr(drTesta!tb_urlchia)
      If strURL = "" Then
        strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento lista di prelievo Incarico Tech non configurata"
        Return False
      End If

      '-------------------------------------------------
      '--- Creo Codice Lista
      '-------------------------------------------------
      With dsDoc.Tables("TESTA").Rows(0)
        strLista = NTSCStr(!et_tipork) + NTSCInt(!et_anno).ToString.PadLeft(4, "0"c) + NTSCStr(!et_serie).ToUpper.PadRight(3) + NTSCInt(!et_numdoc).ToString.PadLeft(8, "0"c)
        If dtCorpo.Select("tb_tipork = " & CStrSQL(NTSCStr(!et_tipork)) & " AND tb_serie = " & CStrSQL(NTSCStr(!et_serie))).Length > 0 Then
          strOperazione = NTSCStr(dtCorpo.Select("tb_tipork = " & CStrSQL(NTSCStr(!et_tipork)) & " AND tb_serie = " & CStrSQL(NTSCStr(!et_serie)))(0) !tb_operazione)
        ElseIf dtCorpo.Select("tb_tipork = " & CStrSQL(NTSCStr(!et_tipork)) & " AND tb_serie = ''").Length > 0 Then
          strOperazione = NTSCStr(dtCorpo.Select("tb_tipork = " & CStrSQL(NTSCStr(!et_tipork)) & " AND tb_serie = ''")(0) !tb_operazione)
        Else
          Return False
        End If
      End With

      If strOperazione <> "+" And strOperazione <> "-" Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore nei parametri per la gestione Magazzino, non è stato indicato un'operazione valida (+/-)"
        Return False
      End If

      If Not bDelete Then
        '-------------------------------------------------
        '--- Inserisce i dati nella tabella 
        '-------------------------------------------------
        If Not oCldInte.InserisciDatiListaDiPrelievo(strDitta, dsDoc, dtCorpo, strLista, strOperazione) Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Errore In selezione dati lista di prelievo " + dsDoc.Tables("TESTA").Rows(0) !et_numdoc.ToString + IIf(dsDoc.Tables("TESTA").Rows(0) !et_serie.ToString.Trim.Length = 0, "", "/" + dsDoc.Tables("TESTA").Rows(0) !et_serie.ToString).ToString + " per magazzino Incarico Tech"
          Return False
        Else
          ''Invia Lista di Prelievo
          ''Invia tutti gli articoli della Lista di Prelievo
          'If Not postAnagrafiche(strURL, strEsito) Then
          '  Return False
          'End If

          'Invia Lista di Prelievo
          If Not postListaDiPrelievo(strURL, strLista, strEsito) Then
            Return False
          End If
        End If
      Else
        If postCancellaLista(strURL, strLista, strEsito) Then
          Return False
        End If
      End If


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
  ''' Leggi gli ordini elaborati al magazzino automatico INCARICOTECH
  ''' </summary>
  Public Overridable Function LeggiOrdiniElaborati _
    (
    ByVal strDitta As String,
    ByRef strErrore As String
    ) As Boolean

    Dim dsInte As New DataSet
    Dim strSQL As String = ""
    Dim drTesta As DataRow = Nothing
    Dim strURL As String = ""
    Dim strOperazione As String = ""
    Dim strRitorno As String = ""
    Dim nID As Integer = 0
    Dim nRiga As Integer = 0
    Dim strTipork As String = ""
    Dim nAnno As Integer = 0
    Dim strSerie As String = ""
    Dim lNum As Integer = 0

    Try
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(dsInte) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Magazzino"
        Return False
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If dsInte Is Nothing Then
        strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Ricezione ordini elaborati Incarico Tech non configurata"
        Return False
      End If
      drTesta = dsInte.Tables("TESTA").Rows(0)

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      'Ordini Elaborati
      strURL = NTSCStr(drTesta!tb_urlchia)
      If strURL = "" Then
        strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Ricezione ordini elaborati Incarico Tech non configurata"
        Return False
      End If

      'Leggi Ordini Elaborati
      If Not postOrdiniElaborati(strURL, strRitorno) Then
        Return False
      End If

      'strRitorno = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/enve lope/"">
      ' <soapenv:Body>
      ' <ns2:readAllJobsResV01 xmlns:ns2=""http://main.jws.com.hanel.de"">
      ' <ns2:return>
      ' <job xmlns=""http://main.jws.com.hanel.de/xsd"">
      ' <jobNumber>M2023   00000008</jobNumber>
      ' <jobPriority>100</jobPriority>
      ' <jobStatus>3</jobStatus>
      ' <jobDate>110220</jobDate>
      ' <jobTime>1601</jobTime>
      ' <JobPosition>
      ' <articleNumber>0438</articleNumber>
      ' <operation>+</operation>
      ' <nominalQuantity>10</nominalQuantity>
      ' <actualQuantity>10</actualQuantity>
      ' <containerSize>101</containerSize>
      ' <positionStatus>1</positionStatus>
      ' </JobPosition>
      ' </job>
      ' <job xmlns=""http://main.jws.com.hanel.de/xsd"">
      ' <jobNumber>M2023   00000009</jobNumber>
      ' <jobPriority>100</jobPriority>
      ' <jobStatus>3</jobStatus>
      ' <jobDate>110220</jobDate>
      ' <jobTime>1601</jobTime>
      ' <JobPosition>
      ' <articleNumber>0438</articleNumber>
      ' <operation>+</operation>
      ' <nominalQuantity>11</nominalQuantity>
      ' <actualQuantity>11</actualQuantity>
      ' <containerSize>101</containerSize>
      ' <positionStatus>1</positionStatus>
      ' </JobPosition>
      ' <JobPosition>
      ' <articleNumber>0438</articleNumber>
      ' <operation>+</operation>
      ' <nominalQuantity>5</nominalQuantity>
      ' <actualQuantity>5</actualQuantity>
      ' <containerSize>101</containerSize>
      ' <positionStatus>1</positionStatus>
      ' </JobPosition>
      ' </job>
      ' <job xmlns=""http://main.jws.com.hanel.de/xsd"">
      ' <jobNumber>W2023   00000002</jobNumber>
      ' <jobPriority>100</jobPriority>
      ' <jobStatus>3</jobStatus>
      ' <jobDate>110220</jobDate>
      ' <jobTime>1601</jobTime>
      ' <JobPosition>
      ' <articleNumber>0438</articleNumber>
      ' <operation>-</operation>
      ' <nominalQuantity>2</nominalQuantity>
      ' <actualQuantity>2</actualQuantity>
      ' <containerSize>101</containerSize>
      ' <positionStatus>1</positionStatus>
      ' </JobPosition>
      ' </job>
      ' <ns1:returnValue xmlns:ns1=""http://main.jws.com.hanel.de/xsd"">0</ns1:returnValue>
      ' </ns2:return>
      ' </ns2:readAllJobsResV01>
      ' </soapenv:Body>
      '</soapenv:Envelope>"

      'strRitorno = "<?xml version='1.0' encoding='UTF-8'?>
      '<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
      '    <soapenv:Body>
      '        <ns2:readAllJobsResV01 xmlns:ns2=""http://main.jws.com.hanel.de"">
      '            <ns2:return>
      '                <ns1:returnValue xmlns:ns1=""http://main.jws.com.hanel.de/xsd"">0</ns1:returnValue>
      '                <ns1:returnErrorNumber xmlns:ns1=""http://main.jws.com.hanel.de/xsd"">1</ns1:returnErrorNumber>
      '                <ns1:returnErrorMessage xmlns:ns1=""http://main.jws.com.hanel.de/xsd"">No Job available</ns1:returnErrorMessage>
      '            </ns2:return>
      '        </ns2:readAllJobsResV01>
      '    </soapenv:Body>
      '</soapenv:Envelope>"

      Dim ds As New DataSet
      Dim dsOrdine As New DataSet

      If strRitorno.Trim.Length > 0 Then
        Dim srXmlTesta As System.IO.StringReader = New System.IO.StringReader(strRitorno)
        ds.ReadXml(srXmlTesta)

        If ds.Tables("Job") Is Nothing Then Return False
        If ds.Tables("JobPosition") Is Nothing Then Return False

        'If dsOrdine Is Nothing Then Return False
        'If dsOrdine.Tables("TESTA") Is Nothing Then Return False
        'If dsOrdine.Tables("CORPO") Is Nothing Then Return False

        For Each drT As DataRow In ds.Tables("Job").Rows
          oCldInte.getOrdiniElaborati(dsOrdine)
          If dsOrdine Is Nothing Then Return False
          If dsOrdine.Tables("TESTA") Is Nothing Then Return False
          If dsOrdine.Tables("CORPO") Is Nothing Then Return False
          nID = oCldInte.getOrderID(strDittaCorrente)
          nRiga = 0
          Dim drOrdineTesta As DataRow = dsOrdine.Tables("TESTA").NewRow
          With drOrdineTesta
            !codditt = strDittaCorrente
            !id = nID
            !jobNumber = NTSCStr(drT!jobNumber)
            ScomponiNumero(NTSCStr(drT!jobNumber), strTipork, nAnno, strSerie, lNum)
            !busTipork = strTipork
            !busAnno = nAnno
            !busSerie = strSerie
            !busNumero = lNum
            !jobPriority = NTSCInt(drT!jobPriority)
            !jobStatus = NTSCInt(drT!jobStatus)
            !jobDate = NTSCStr(drT!jobDate)
            !jobTime = NTSCStr(drT!jobTime)
            !jobDataOra = FormatoDataOra(NTSCStr(drT!jobDate), NTSCStr(drT!jobTime))
            !importato = "N"
            !cancellato = "N"
            For Each drC As DataRow In ds.Tables("JobPosition").Select("job_id = " & NTSCInt(drT!job_id))
              nRiga += 1
              Dim drOrdineCorpo As DataRow = dsOrdine.Tables("CORPO").NewRow
              With drOrdineCorpo
                !codditt = strDittaCorrente
                !id = nID
                !idriga = nRiga
                !jobNumber = NTSCStr(drT!jobNumber)
                !articleNumber = NTSCStr(drC!articleNumber)
                !operation = NTSCStr(drC!operation)
                !nominalQuantity = NTSCInt(drC!nominalQuantity)
                !actualQuantity = NTSCInt(drC!actualQuantity)
                !containerSize = NTSCInt(drC!containerSize)
                !positionStatus = NTSCInt(drC!positionStatus)
              End With
              dsOrdine.Tables("CORPO").Rows.Add(drOrdineCorpo)
            Next
          End With
          dsOrdine.Tables("TESTA").Rows.Add(drOrdineTesta)
          oCldInte.SalvaOrdiniElaborati(dsOrdine)
        Next

      End If

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
  ''' Leggi le operazioni manuali dal magazzino automatico INCARICOTECH
  ''' </summary>
  Public Overridable Function LeggiOperazioniManuali _
    (
    ByVal strDitta As String,
    ByRef strErrore As String
    ) As Boolean

    Dim dsInte As New DataSet
    Dim strSQL As String = ""
    Dim drTesta As DataRow = Nothing
    Dim strURL As String = ""
    Dim strOperazione As String = ""
    Dim strRitorno As String = ""
    Dim nID As Integer = 0
    Dim nRiga As Integer = 0
    Dim strTipork As String = ""
    Dim nAnno As Integer = 0
    Dim strSerie As String = ""
    Dim lNum As Integer = 0

    Try
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(dsInte) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Magazzino"
        Return False
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If dsInte Is Nothing Then
        strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Ricezione opearzioni manuali Incarico Tech non configurata"
        Return False
      End If
      drTesta = dsInte.Tables("TESTA").Rows(0)

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      'Ordini Elaborati
      strURL = NTSCStr(drTesta!tb_urlchia)
      If strURL = "" Then
        strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Ricezione ordini elaborati Incarico Tech non configurata"
        Return False
      End If

      'Leggi Operazioni Manuali
      If Not postOperazioniManuali(strURL, strRitorno) Then
        Return False
      End If

      'strRitorno = "<?xml version='1.0' encoding='UTF-8'?>
      '<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
      '    <soapenv:Body>
      '        <ns2:readOpjournalResV01 xmlns:ns2=""http://main.jws.com.hanel.de"">
      '            <ns2:return>
      '                <opjournalPosition xmlns=""http://main.jws.com.hanel.de/xsd"">
      '                    <articleNumber>13052</articleNumber>
      '                    <operation>-</operation>
      '                    <actualQuantity>1</actualQuantity>
      '                    <containerSize>206</containerSize>
      '                    <status>2</status>
      '                    <u02SpecialField>290423</u02SpecialField>
      '                    <u03SpecialField>1136</u03SpecialField>
      '                    <u06SpecialField>9999</u06SpecialField>
      '                </opjournalPosition>
      '                <opjournalPosition xmlns=""http://main.jws.com.hanel.de/xsd"">
      '                    <articleNumber>20085.1</articleNumber>
      '                    <operation>+</operation>
      '                    <actualQuantity>47</actualQuantity>
      '                    <containerSize>304</containerSize>
      '                    <status>2</status>
      '                    <u02SpecialField>020523</u02SpecialField>
      '                    <u03SpecialField>0750</u03SpecialField>
      '                    <u06SpecialField>9999</u06SpecialField>
      '                </opjournalPosition>
      '                <opjournalPosition xmlns=""http://main.jws.com.hanel.de/xsd"">
      '                    <articleNumber>03029</articleNumber>
      '                    <operation>-</operation>
      '                    <actualQuantity>40</actualQuantity>
      '                    <containerSize>304</containerSize>
      '                    <status>2</status>
      '                    <u02SpecialField>020523</u02SpecialField>
      '                    <u03SpecialField>0752</u03SpecialField>
      '                    <u06SpecialField>9999</u06SpecialField>
      '                </opjournalPosition>               
      '                <opjournalPosition xmlns=""http://main.jws.com.hanel.de/xsd"">
      '                    <articleNumber>33102</articleNumber>
      '                    <operation>-</operation>
      '                    <actualQuantity>12</actualQuantity>
      '                    <containerSize>205</containerSize>
      '                    <status>2</status>
      '                    <u02SpecialField>020523</u02SpecialField>
      '                    <u03SpecialField>0818</u03SpecialField>
      '                    <u06SpecialField>9999</u06SpecialField>
      '                </opjournalPosition>
      '                <opjournalPosition xmlns=""http://main.jws.com.hanel.de/xsd"">
      '                    <articleNumber>28107</articleNumber>
      '                    <operation>-</operation>
      '                    <actualQuantity>12</actualQuantity>
      '                    <containerSize>304</containerSize>
      '                    <status>2</status>
      '                    <u02SpecialField>020523</u02SpecialField>
      '                    <u03SpecialField>0818</u03SpecialField>
      '                    <u06SpecialField>9999</u06SpecialField>
      '                </opjournalPosition>
      '              <ns1:returnValue xmlns:ns1=""http://main.jws.com.hanel.de/xsd"">0</ns1:returnValue>
      '            </ns2:return>
      '        </ns2:readOpjournalResV01>
      '    </soapenv:Body>
      '</soapenv:Envelope>"

      Dim ds As New DataSet
      Dim dsMov As New DataSet

      If strRitorno.Trim.Length > 0 Then
        Dim srXmlTesta As System.IO.StringReader = New System.IO.StringReader(strRitorno)
        ds.ReadXml(srXmlTesta)

        If ds.Tables("opjournalPosition") Is Nothing Then Return False

        oCldInte.getOperazioniManuali(dsMov)
        If dsMov Is Nothing Then Return False
        If dsMov.Tables("MOV") Is Nothing Then Return False
        nRiga = 0
        For Each drC As DataRow In ds.Tables("opjournalPosition").Rows
          nID = oCldInte.getOperationID(strDittaCorrente)
          nRiga += 1
          Dim drMov As DataRow = dsMov.Tables("MOV").NewRow
          With drMov
            !codditt = strDittaCorrente
            !id = nID
            !articleNumber = NTSCStr(drC!articleNumber)
            !operation = NTSCStr(drC!operation)
            !actualQuantity = NTSCInt(drC!actualQuantity)
            !containerSize = NTSCInt(drC!containerSize)
            !status = NTSCInt(drC!status)
            !u02SpecialField = NTSCStr(drC!u02SpecialField)
            !u03SpecialField = NTSCStr(drC!u03SpecialField)
            !u06SpecialField = NTSCStr(drC!u06SpecialField)
            Try
              !u07SpecialField = NTSCStr(drC!u07SpecialField)
            Catch ex As Exception
            End Try
            Dim edate As String = NTSCStr(drC!u02SpecialField)
            Dim expenddt As Date = Date.ParseExact(edate, "ddMMyy", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            Dim ts As New TimeSpan(NTSCInt(Mid(NTSCStr(drC!u03SpecialField), 1, 2)), NTSCInt(Mid(NTSCStr(drC!u03SpecialField), 3, 2)), 0)
            expenddt = expenddt.Add(ts)
            !jobDataOra = expenddt.ToString
            !anno = NTSCInt(expenddt.Year)
            !importato = "N"
            !cancellato = "N"
          End With
          dsMov.Tables("MOV").Rows.Add(drMov)
          oCldInte.SalvaOperazioniManuali(drMov)
        Next

      End If

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
  ''' Leggi le giacenze dal magazzino automatico INCARICOTECH
  ''' </summary>
  Public Overridable Function LeggiGiacenze _
    (
    ByVal strDitta As String,
    ByRef strErrore As String,
    ByRef dsOut As DataSet
    ) As Boolean

    Dim dsInte As New DataSet
    Dim strSQL As String = ""
    Dim drTesta As DataRow = Nothing
    Dim strURL As String = ""
    Dim strOperazione As String = ""
    Dim strRitorno As String = ""
    Dim nID As Integer = 0
    Dim nRiga As Integer = 0
    Dim strTipork As String = ""
    Dim nAnno As Integer = 0
    Dim strSerie As String = ""
    Dim lNum As Integer = 0

    Try
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(dsInte) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Magazzino"
        Return False
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If dsInte Is Nothing Then
        strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Ricezione ordini elaborati Incarico Tech non configurata"
        Return False
      End If
      drTesta = dsInte.Tables("TESTA").Rows(0)

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      'Lista Prelievo
      strURL = NTSCStr(drTesta!tb_urlchia)
      If strURL = "" Then
        strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Ricezione ordini elaborati Incarico Tech non configurata"
      End If

      'Leggi Giacenze
      If Not postGiacenze(strURL, strRitorno) Then
        Return False
      End If

      'strRitorno = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
      ' <soapenv:Body>
      ' <ns2:readAllAMDResV01 xmlns:ns2=""http://main.jws.com.hanel.de"">
      ' <ns2:return>
      ' <article xmlns=""http://main.jws.com.hanel.de/xsd"">
      ' <articleNumber>?</articleNumber>
      ' <articleName/>
      ' <liftNumber>1</liftNumber>
      ' <shelfNumber>7</shelfNumber>
      ' <compartmentNumber>1</compartmentNumber>
      ' <compartmentDepthNumber>1</compartmentDepthNumber>
      ' <containerSize>101</containerSize>
      ' <fifo>1</fifo>
      ' <inventoryAtStorageLocation>10</inventoryAtStorageLocation>
      ' <minimumInventory>0</minimumInventory>
      ' </article>
      ' <article xmlns=""http://main.jws.com.hanel.de/xsd"">
      ' <articleNumber>ARTICOLO1</articleNumber>
      ' <articleName>PLUTO23</articleName>
      ' <liftNumber>1</liftNumber>
      ' <shelfNumber>7</shelfNumber>
      ' <compartmentNumber>2</compartmentNumber>
      ' <compartmentDepthNumber>1</compartmentDepthNumber>
      ' <containerSize>101</containerSize>
      ' <fifo>1</fifo>
      ' <inventoryAtStorageLocation>10</inventoryAtStorageLocation>
      ' <minimumInventory>0</minimumInventory>
      ' </article>
      ' <article xmlns=""http://main.jws.com.hanel.de/xsd"">
      ' <articleNumber>ARTICOLO3</articleNumber>
      ' <articleName>PLUTO23</articleName>
      ' <liftNumber>0</liftNumber>
      ' <shelfNumber>0</shelfNumber>
      ' <compartmentNumber>0</compartmentNumber>
      ' <compartmentDepthNumber>0</compartmentDepthNumber>
      ' <containerSize>101</containerSize>
      ' <fifo>1</fifo>
      ' <inventoryAtStorageLocation>0</inventoryAtStorageLocation>
      ' <minimumInventory>0</minimumInventory>
      ' </article>
      ' <article xmlns=""http://main.jws.com.hanel.de/xsd"">
      ' <articleNumber>0438</articleNumber>
      ' <articleName>PLUTO23</articleName>
      ' <liftNumber>0</liftNumber>
      ' <shelfNumber>0</shelfNumber>
      ' <compartmentNumber>0</compartmentNumber>
      ' <compartmentDepthNumber>0</compartmentDepthNumber>
      ' <containerSize>101</containerSize>
      ' <fifo>1</fifo>
      ' <inventoryAtStorageLocation>20</inventoryAtStorageLocation>
      ' <minimumInventory>0</minimumInventory>
      ' </article>
      ' <article xmlns=""http://main.jws.com.hanel.de/xsd"" >
      ' <articleNumber>PIPPO</articleNumber>
      ' <articleName>PLUTO</articleName>
      ' <liftNumber>0</liftNumber>
      ' <shelfNumber>0</shelfNumber>
      ' <compartmentNumber>0</compartmentNumber>
      ' <compartmentDepthNumber>0</compartmentDepthNumber>
      ' <containerSize>101</containerSize>
      ' <fifo>1</fifo>
      ' <inventoryAtStorageLocation>0</inventoryAtStorageLocation>
      ' <minimumInventory>0</minimumInventory>
      ' </article>
      ' <ns1:returnValue xmlns:ns1=""http://main.jws.com.hanel.de/xsd"" >0</ns1:returnValue>
      ' </ns2:return>
      ' </ns2:readAllAMDResV01>
      ' </soapenv:Body>
      '</soapenv:Envelope>"

      If strRitorno.Trim.Length > 0 Then
        Dim srXml As System.IO.StringReader = New System.IO.StringReader(strRitorno)
        dsOut.ReadXml(srXml)
      End If

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

  Public Overridable Function FormatoDataOra(strData As String, strOra As String) As DateTime
    Try
      Dim nGiorno As Integer = NTSCInt(Mid(strData, 1, 2))
      Dim nMese As Integer = NTSCInt(Mid(strData, 3, 2))
      Dim nAnno As Integer = NTSCInt(Mid(strData, 5, 2)) + 2000
      Dim nOra As Integer = NTSCInt(Mid(strOra, 1, 2))
      Dim nMinuti As Integer = NTSCInt(Mid(strOra, 3, 2))

      Return New DateTime(nAnno, nMese, nGiorno, nOra, nMinuti, 0)
    Catch ex As Exception
      Return New DateTime(1900, 1, 1)
      ''-------------------------------------------------
      'CLN__STD.GestErr(ex, Me, "")
      ''-------------------------------------------------
    End Try
  End Function

  Public Overridable Sub ScomponiNumero(ByVal strNumero As String, ByRef strTipork As String, ByRef nAnno As Integer, ByRef strSerie As String, ByRef lNum As Integer)
    Try
      strTipork = ""
      nAnno = 0
      strSerie = ""
      lNum = 0
      If strNumero.Length <> 16 Then Return
      strTipork = Mid(strNumero, 1, 1)
      nAnno = NTSCInt(Mid(strNumero, 2, 4))
      strSerie = Trim(Mid(strNumero, 6, 3))
      If strSerie = "" Then strSerie = " "
      lNum = NTSCInt(Mid(strNumero, 9, 8))
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Funzione di conversione in valore Boolean
  ''' </summary>
  Public Overridable Function cb(ByVal valore As Object) As Boolean
    Try
      If valore Is Nothing Then Return False
      If IsDBNull(valore) Then Return False

      Return CBool(valore)
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function
#End Region

#Region "Funzioni e Routines Incarico Tech"
  ''' <summary>
  ''' CUSTOM
  ''' Personalizzazione per WMS
  ''' Gestore eventi entity VEBOLL AUTOSTORE
  ''' </summary>
  Public Overridable Sub GestisciEventiEntityBoll(ByVal sender As Object, ByRef e As NTSEventArgs)
    Try
      'gli eventuali messaggi dati da BEVEBOLL tramite la ThrowRemoteEvent li passo a BNRMRICM
      If e.Message <> "" Then
        LogWrite(oApp.Tr(Me, 128843579324644000, "Messaggio BollAutostore : " & e.Message.ToString), False)
      End If

      '------------------------------------------------------------------------------------------
      ' SISCO LILIANA 14/03/2022                                        Inizio
      '   Intercetto messaggi inseriti in BFVEBOLL per proseguire con il salvataggio
      '------------------------------------------------------------------------------------------
      'AAAAAAAAAAAAAAAAAAAAA
      'SECONDO MME QUESTII SONO DA ELIMINARE PER AUTOSTORE
      'IMPORTANTE GESTIE LA MANCATA GENERAZIONE DEL MESSAGGIO DI VEBOLL QUANDO CHIAMATO in SilentMode!!!!!!
      If e.Message Like "*Il riferimento*" And
          e.Message Like "*è già presente nei seguenti documenti*" And
          e.Message Like "*Continuare con il salvataggio*" Then
        e.RetValue = CLN__STD.ThMsg.RETVALUE_YES
        Return
      End If

      'AAAAAAAAAAAAAAAAAAAAA
      'SECONDO MME QUESTII SONO DA ELIMINARE PER AUTOSTORE
      'IMPORTANTE GESTIE LA MANCATA GENERAZIONE DEL MESSAGGIO DI VEBOLL QUANDO CHIAMATO in SilentMode!!!!!!
      'ELIMNARE LA PERSONALIZZAIONE DA VEBOLL IN QUANTO FATTA PER ELEKTRICA E NON PIù UTILIZZATA!!!!!!
      If e.Message Like "*Magazzino*" And
          e.Message Like "*non corrisponde*" And
          e.Message Like "*associato alla commessa*" And
          e.Message Like "*Proseguire con il salvataggio*" Then
        e.RetValue = CLN__STD.ThMsg.RETVALUE_YES
        Return
      End If
      '------------------------------------------------------------------------------------------
      ' SISCO LILIANA 14/03/2022                                        Fine
      '------------------------------------------------------------------------------------------

      '--------------------------------------------------------------------------------------
      ' SISCO LILIANA. 22/04/2022  Intercettato messaggio errore. 
      '       Se dovuto a documento già essitente allora riprovo con altro numero
      '--------------------------------------------------------------------------------------
      If e.Message Like "*Esiste già un documento di tipo*" And
         e.Message Like "*con le stesse caratteristiche di quello che si desidera creare*" Then
        'AAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        'bhhRileggiNum = True
      End If
      '--------------------------------------------------------------------------------------
      ' SISCO LILIANA. 22/04/2022  Intercettato messaggio errore.         Fine
      '--------------------------------------------------------------------------------------


      ThrowRemoteEvent(e)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub
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

  Public Overridable Sub BeforeColUpdateCORPO_tb_tipork(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Dim dtrTmp() As DataRow
    Dim strErrore As String = ""

    Try
      '--------------------------------------------------
      '--- Verifica codice già esistente
      '--------------------------------------------------
      If dsShared.Tables("CORPO").Rows.Count > 1 Then
        dtrTmp = dsShared.Tables("CORPO").Select("tb_tipork = " & CStrSQL(e.ProposedValue.ToString()) & " AND tb_serie = " & CStrSQL(e.Row!tb_serie))
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

  Public Overridable Sub BeforeColUpdateCORPO_tb_serie(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Dim dtrTmp() As DataRow
    Dim strErrore As String = ""

    Try
      '--------------------------------------------------
      '--- Verifica riga già esistente
      '--------------------------------------------------
      If dsShared.Tables("CORPO").Rows.Count > 1 Then
        dtrTmp = dsShared.Tables("CORPO").Select("tb_tipork = " & CStrSQL(e.Row!tb_tipork) & " AND tb_serie = " & CStrSQL(e.ProposedValue.ToString()))
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

  Public Overridable Sub BeforeColUpdateCORPO_tb_magazmacchina(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Try
      Dim dt As New DataTable
      Dim strDescr As String = ""

      oCldInte.ValCodiceDb(e.ProposedValue.ToString(), strDittaCorrente, "tabmaga", "N", strDescr, dt)
      If dt.Rows.Count = 0 Then
        ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 128571303239218321, "Attenzione! Codice magazzino inesistente")))
        e.ProposedValue = e.Row(e.Column.ColumnName)
      Else
        e.Row!xx_descrmagmacchina = strDescr
      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub BeforeColUpdateCORPO_tb_magazprincipale(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Try
      Dim dt As New DataTable
      Dim strDescr As String = ""

      oCldInte.ValCodiceDb(e.ProposedValue.ToString(), strDittaCorrente, "tabmaga", "N", strDescr, dt)
      If dt.Rows.Count = 0 Then
        ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 128571303239218321, "Attenzione! Codice magazzino inesistente")))
        e.ProposedValue = e.Row(e.Column.ColumnName)
      Else
        e.Row!xx_descrmagprincipale = strDescr
      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub
#End Region

End Class