Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System
Imports System.Net
Imports System.IO
Imports System.Xml
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class CLEHHCNMG
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
  Public oCldCnmg As CLDHHCNMG
#End Region

#Region "Oggetto Entity VEBOLL per AUTOSTORE"
  Public oCleBoll As CLEVEBOLL
#End Region

#Region "Variabili"
  'Variabili per validazione eventi before/after update
  Public bNonValidare As Boolean
  Public bInOnAddNewRow As Boolean
  Public bInUnload As Boolean

  'Variabili per gestione tabelle CONNESSIONI.
  Public lCodconn As Integer          'Codice chiamata corrente
  Public strDesconn As String = ""    'Descrizione chiamata corrente
  Public strUrlconn As String = ""    'strURL chiamata corrente
  Public lCodconnSel As Integer       'Codice chiamata selezionato
  Public lCodconnNew As Integer       'Codice chiamata nuovo per duplicazione

  'Costanti per connessione a magazzino
  Private Const INVIOARTICOLI As Integer = 1
  Private Const LISTAPRELIEVO As Integer = 10
  Private Const CANCELLALISTAPRELIEVO As Integer = 11
  Private Const ORDINIESEGUITI As Integer = 20
  Private Const SKUINSERTUPDATE As Integer = 31
  Private Const SKUDELETE As Integer = 32
  Private Const ADVICEINSERTUPDATE As Integer = 40
  Private Const ADVICEDELETE As Integer = 41
  Private Const ORDERINSERTUPDATE As Integer = 50
  Private Const ORDERDELETE As Integer = 51
  Private Const STOCKTACKINGREQUEST As Integer = 60
  Private Const CONFIRM As Integer = 70
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
    If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHCNMG"
    MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")
    oCldCnmg = CType(MyBase.ocldBase, CLDHHCNMG)
    oCldCnmg.Init(oApp)

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
      'strDittaCorrente = strDitta
      dReturn = oCldCnmg.GetData(dsOut)

      '--------------------------------------------------
      '--- Esce se c'è stato un errore
      '--------------------------------------------------
      If dReturn = False Then Return False

      '--------------------------------------------------
      '--- Imposta valori di default
      '--------------------------------------------------
      SetDefaultValue(dsOut)

      '--------------------------------------------------
      '--- Imposta Dataset condiviso
      '--------------------------------------------------
      dsShared = dsOut

      '--------------------------------------------------
      '--- Creo gli eventi per la gestione dei datatable dentro l'entity
      '--------------------------------------------------
      If Not dsShared Is Nothing Then
        'Eventi
        RemoveHandler dsShared.Tables("CHIAMATE").ColumnChanging, AddressOf BeforeColUpdate
        RemoveHandler dsShared.Tables("CHIAMATE").ColumnChanging, AddressOf AfterColUpdate
      End If

      AddHandler dsShared.Tables("CHIAMATE").ColumnChanging, AddressOf BeforeColUpdate
      AddHandler dsShared.Tables("CHIAMATE").ColumnChanged, AddressOf AfterColUpdate

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

  Public Overridable Function Esiste(ByVal lCodconn As Integer) As Boolean
    Try
      '--------------------------------------------------
      '--- Verifica se esiste una connessione con il codice specificato
      '--------------------------------------------------
      If dsShared.Tables("CHIAMATE").Select("ch_codchia = " & lCodconn.ToString).Length > 0 Then
        ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 127791761089687542, "Codice connessione '" & lCodconn.ToString & "' già esistente.")))
        Return True
      End If

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return False

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return False
    End Try
  End Function

  Public Overridable Function Duplica() As Boolean
    Dim dtrP() As DataRow
    Dim dttCopy As New DataTable
    Dim strColname As String
    Try
      '--------------------------------------------------
      '--- Conferma modifiche a dsShared 
      '--------------------------------------------------
      dsShared.AcceptChanges()

      '--------------------------------------------------
      '--- Seleziona la connessione da duplicare
      '--------------------------------------------------
      If dsShared.Tables("CHIAMATE").Rows.Count > 0 Then
        dtrP = dsShared.Tables("CHIAMATE").Select("ch_codchia = " & lCodconn.ToString)
        If dtrP.Length = 0 Then
          Return False
        End If
      Else
        Return False
      End If

      '--------------------------------------------------
      '--- Duplica connessione
      '--------------------------------------------------
      dsShared.Tables("CHIAMATE").Rows.Add()
      With dsShared.Tables("CHIAMATE")
        'Imposta codice connessione
        .Rows(.Rows.Count - 1)("ch_codchia") = lCodconnNew

        'Imposta rimanenti campi
        For i As Integer = 0 To .Columns.Count - 1
          strColname = .Columns(i).ColumnName
          If strColname <> "ch_codchia" AndAlso strColname <> "ts" Then
            .Rows(.Rows.Count - 1)(strColname) = dtrP(0)(strColname)
          End If
        Next

      End With

      '--------------------------------------------------
      '--- Salva la nuova connessione creata
      '--------------------------------------------------
      lCodconn = lCodconnNew
      lCodconnSel = lCodconnNew
      If Not Salva(False) Then
        'Annullo le modifiche
        dsShared.Tables("CHIAMATE").RejectChanges()
        dsShared.AcceptChanges()
        Return False
      End If
      dsShared.AcceptChanges()

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

  Public Overrides Sub Nuovo()
    Dim i As Integer = 0
    Dim l As Integer = 0
    Try
      '--------------------------------------------------
      '--- Inserisco una nuova riga
      '--------------------------------------------------
      dsShared.Tables("CHIAMATE").Rows.Add(dsShared.Tables("CHIAMATE").NewRow)

      '--------------------------------------------------
      '--- Segnala variazioni su dataset
      '--------------------------------------------------
      bHasChanges = True

      '--------------------------------------------------
      '--- Propongo il codice
      '--------------------------------------------------
      Dim dtrN As DataRow
      dtrN = dsShared.Tables("CHIAMATE").Rows(dsShared.Tables("CHIAMATE").Rows.Count - 1)
      If dsShared.Tables("CHIAMATE").Rows.Count > 1 Then
        If dsShared.Tables("CHIAMATE").Select("ch_codchia <> 0", "ch_codchia DESC").Length = 0 Then
          dtrN("ch_codchia") = 1
          Return
        End If
        i = NTSCInt(dsShared.Tables("CHIAMATE").Select("ch_codchia <> 0", "ch_codchia DESC")(0) !ch_codchia) + 1
        'se i = il valore massimo consentito per il campo, propongo il primo buco libero partendo da 1
        If i > 999 Then
          l = 0
          For Each dtrT As DataRow In dsShared.Tables("CHIAMATE").Select("ch_codchia <> 0", "ch_codchia ASC")
            If NTSCInt(dtrT!ch_codchia) > l + 1 Then
              i = l + 1
              Exit For
            End If
            l = NTSCInt(dtrT!ch_codchia)
          Next
        End If
        dtrN("ch_codchia") = i
      Else
        dtrN("ch_codchia") = 1
      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overloads Function Salva(ByVal bDelete As Boolean) As Boolean
    Dim bResult As Boolean = False
    Try
      '--------------------------------------------------
      '--- Controlli pre-salvataggio (solo se non è una delete)
      '--------------------------------------------------
      If Not bDelete Then
        If Not TestPreSalva("CHIAMATE") Then Return False
      End If

      '--------------------------------------------------
      '--- Salva il dataset in formato XML nel registro Business
      '--------------------------------------------------
      'Scrive il dataset in XML
      Dim sbStr As New System.Text.StringBuilder
      Dim swXml As System.IO.StringWriter = New System.IO.StringWriter(sbStr)
      dsShared.Tables("CHIAMATE").WriteXml(swXml, XmlWriteMode.IgnoreSchema)
      Dim strXml As String = sbStr.ToString
      'In bOperatore indicazione di connessione specifico per operatore
      oCldCnmg.SaveSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_CNChiamate", strXml, " ", False, True, False)

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

  Public Overrides Function Ripristina(ByVal nRow As Integer, ByVal strFilter As String) As Boolean
    'non gestire l'eccezione in ripristino di una griglia: darebbe errore per un problema del framework
    Try
      '--------------------------------------------------
      '--- Annulla modifiche 
      '--------------------------------------------------
      dsShared.Tables("CHIAMATE").Select(strFilter)(nRow).RejectChanges()

      '--------------------------------------------------
      '--- Reimposta la variabile lCodprof per connessione corrente
      '--------------------------------------------------
      Dim nRiga As Integer = dsShared.Tables("CHIAMATE").Rows.Count - 1
      If nRiga = -1 Then
        lCodconn = 0
      Else
        lCodconn = NTSCInt(dsShared.Tables("CHIAMATE").Rows(nRiga) !ch_codchia)
      End If

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
      '--- Imposto i valori di default CHIAMATE
      '--------------------------------------------------
      With ds.Tables("CHIAMATE")
        .Columns("ch_codchia").DefaultValue = 0
        .Columns("ch_deschia").DefaultValue = " "
        .Columns("ch_urlchia").DefaultValue = " "
      End With

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Overloads Function TestPreSalva(ByVal strTabella As String) As Boolean
    Dim dtrTmp As DataRow() = dsShared.Tables(strTabella).Select(Nothing, Nothing, DataViewRowState.Added Or DataViewRowState.ModifiedCurrent)
    Dim dttTmp As New DataTable

    Try
      '--------------------------------------------------
      '--- Esce se non ci sono CHIAMATE in gestione 
      '--------------------------------------------------
      If lCodconn = 0 Then Return True

      '--------------------------------------------------
      '--- Cicla sulle righe da validare
      '--------------------------------------------------
      'For i As Integer = 0 To (dtrTmp.Length - 1)

      'Next

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
      dttTmp.Clear()
      dttTmp.Dispose()
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
      httpWebRequest.ContentType = "application/xml"
      httpWebRequest.Method = "POST"
      'L'impostazione di:
      'TransferEncoding = "utf-8" e SendChunked = True senza la quale non è possibile impostare TransferEncoding
      'consente l'invio di caratteri "strani"
      'Es.Termostato NC x resistenze 0-60°C
      'Senza le due righe seguenti la chiamata viene rifiutata
      httpWebRequest.SendChunked = True
      httpWebRequest.TransferEncoding = "utf-8"
      httpWebRequest.ContentLength = strXMLData.Length

      '-------------------------------------------------
      '--- Invio dati XML
      '-------------------------------------------------
      Using streamWriter As StreamWriter = New StreamWriter(httpWebRequest.GetRequestStream(), Text.Encoding.GetEncoding("utf-8"))
        streamWriter.Write(strXMLData)
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
  Public Overridable Function putHttp(HttpWebRequest As HttpWebRequest, ByRef strErrore As String) As Boolean
    'Dichiarazione variabili
    Dim strMsg As String = ""
    Dim webResponse As System.Net.WebResponse = Nothing
    Dim strResponse As String = ""
    Dim objJson As JObject

    Try
      '--------------------------------------------------
      '--- Recupera risposta chiamata
      '--------------------------------------------------
      strErrore = ""
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
          objJson = JObject.Parse(strResponse)

          'Verifca se success = true 
          If UCase(CType(objJson.SelectToken("messageText"), String)) <> "OK" Then
            strErrore = CType(objJson.SelectToken("messageText"), String)
            Return False
          End If
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
    ByRef strErrore As String
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
      drArt = oCldCnmg.GetArticoli(strDittaCorrente, strCodArt)
      If dRArt Is Nothing Then Return False

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
      strErrore = ""
      bEsito = putHttp(httpWebRequest, strErrore)
      If bEsito Then
        strStato = "I"
      Else
        strStato = "E"
      End If
      oCldCnmg.AggiornaEsitoArticoli(strDittaCorrente, strCodArt, strStato, IIf(bEsito, "OK", strErrore).ToString)

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
    ByRef strErrore As String
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
      dtArt = oCldCnmg.GetArticoli(strDittaCorrente)
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
        strErrore = ""
        bEsito = putHttp(httpWebRequest, strErrore)
        If bEsito Then
          strStato = "I"
        Else
          strStato = "E"
        End If
        oCldCnmg.AggiornaEsitoArticoli(strDittaCorrente, strCodArt, strStato, IIf(bEsito, "OK", strErrore).ToString)
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
      strXMLData &= "         <xsd:articleNumber> " & NTSCStr(drArt!tb_codart) & "</xsd:articleNumber>" & vbCrLf
      strXMLData &= "         <xsd:articleName> " & NTSCStr(drArt!tb_descr) & "</xsd:articleName>" & vbCrLf
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
  ''' Chiamata al servizio REST di connessione MAGAZZINO per inserimento Lista di Prelievo
  ''' </summary>
  Public Overridable Function postListaDiPrelievo _
    (
    ByVal strURL As String,
    ByVal strLista As String,
    ByRef strErrore As String
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
      dtLista = oCldCnmg.GetListadiPrelievo(strDittaCorrente, strLista)
      If dtLista Is Nothing Then Return False
      If dtLista.Rows.Count = 0 Then Return False

      '--------------------------------------------------
      '--- Crea l'XML per invio lista di prelievo
      '--------------------------------------------------
      strXMLData = creaXMLListaDiPrelievo(dtLista)

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostXML(strXMLData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strErrore = ""
      bEsito = putHttp(httpWebRequest, strErrore)
      If bEsito Then
        strStato = "I"
      Else
        strStato = "E"
      End If
      'oCldCnmg.AggiornaEsitoOrder(strDittaCorrente, strClientId, strLista, strStato, "Insert/Update", IIf(bEsito, "OK", strErrore).ToString)

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
 <!--1 or more repetitions:-->
 <xsd:job>
 <xsd:jobNumber>MATTEO</xsd:jobNumber>
 
 <xsd:JobPosition>
 <xsd:articleNumber>ARTICOLO456</xsd:articleNumber>
 <xsd:operation>+</xsd:operation>
 <xsd:nominalQuantity>11</xsd:nominalQuantity>
 </xsd:JobPosition>xsd:JobPosition> 
 <xsd:JobPosition>
 
 <xsd:articleNumber>ARTICOLO2</xsd:articleNumber>
 <xsd:operation>+</xsd:operation>
 <xsd:nominalQuantity>10</xsd:nominalQuantity>
 
 </xsd:JobPosition>
 <xsd:JobPosition>
 <xsd:articleNumber>ARTICOLO5</xsd:articleNumber>
 <xsd:operation>+</xsd:operation>
 <xsd:nominalQuantity>10</xsd:nominalQuantity>
 </xsd:JobPosition>xsd:JobPosition> 
 </xsd:job>
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
#End Region

#Region "Gestione SKU - Articoli"
  ''' <summary>
  ''' Compone il Json per delete SKU
  ''' </summary>
  Public Overridable Function creaJsonSkuDelete _
    (
    ByVal strClientId As String,
    ByVal strSkuId As String
    ) As String

    Dim strJsonData As String
    Dim clsSku As New SkuDelete.skuDelete

    Try
      '--------------------------------------------------
      '--- Crea il Json per delete SKU
      '--------------------------------------------------
      clsSku = New SkuDelete.skuDelete
      With clsSku
        .clientId = NTSCStr(strClientId)
        .skuId = NTSCStr(strSkuId)
      End With
      strJsonData = JsonConvert.SerializeObject(clsSku, Newtonsoft.Json.Formatting.Indented)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strJsonData

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Compone il Json per inser/update SKU
  ''' </summary>
  Public Overridable Function creaJsonSkuInsertUpdate(ByVal dtrSku As DataRow) As String
    Try
      Dim strJsonData As String
      Dim dtArt As New DataTable
      Dim dttUnmis As New DataTable
      Dim clsSku As New skuInsertUpdate.skuInsertUpdate

      '--------------------------------------------------
      '--- Crea il Json per insert/update SKU
      '--------------------------------------------------
      clsSku = New skuInsertUpdate.skuInsertUpdate
      With clsSku
        .clientId = NTSCStr(dtrSku!clientId)
        .skuId = NTSCStr(dtrSku!skuId)
        .description = NTSCStr(dtrSku!description)
        .productCode = NTSCStr(dtrSku!productCode)
        .productGroupId = NTSCStr(dtrSku!productGroupId)
        .baseQuantityUnitId = NTSCStr(dtrSku!baseQuantityUnitId)
        .handlingHint = NTSCStr(dtrSku!handlingHint)
        .batchMandatoryForHost = cb(dtrSku!batchMandatoryForHost)
        '.stocktakingExcluded = cb(dtrOrder!stocktakingExcluded)
        .cycleCountingThreshold = NTSCInt(dtrSku!cycleCountingThreshold)

        'leggi Unità di misura da Artico
        dttUnmis = oCldCnmg.GetSkuUnmis(strDittaCorrente, .skuId)

        Dim clsSkuQuantityUnits As New skuInsertUpdate.skuInsertUpdate.SkuQuantityUnits
        Dim clsSkuQuantityUnitList As New List(Of skuInsertUpdate.SkuQuantityUnit)
        Dim clsSkuQuantityUnit As New skuInsertUpdate.SkuQuantityUnit
        For Each drr As DataRow In dttUnmis.Rows
          clsSkuQuantityUnit = New skuInsertUpdate.SkuQuantityUnit
          With clsSkuQuantityUnit
            .quantityUnitId = NTSCStr(drr!ar_unmis)
            '.unitOfMeasure = cb(dtrOrder!unitOfMeasure)
            .totalWeight = 0
            .netWeight = 0
            '.totalVolume = 1
            .totalVolume = NTSCDec(0.25)
            .length = 0
            .width = 0
            .height = 0
            '.hostWeightUnitId = 0
            '.hostVolumeUnitId = 0
            '.hostLengthUnitId = 0
            .defaultPickQuantityUnit = False
            .factorToBaseQU = NTSCDec(drr!ar_fattore)

            ''crea Json Image List
            'Dim clsSkuImageLists As New SkuQuantityUnit.cSkuImageList
            'Dim clsSkuImageListsList As New List(Of SkuImage)
            'Dim clsSkuImage As New SkuImage
            'With clsSkuImage
            '    .imageName = ""
            '    .skuImageSource = ""
            '    .defaultImage = False
            'End With
            'clsSkuImageListsList.Add(clsSkuImage)
            'clsSkuImageLists.skuImage = clsSkuImageListsList
            '.skuImageList = clsSkuImageLists
          End With

          clsSkuQuantityUnitList.Add(clsSkuQuantityUnit)
        Next

        clsSkuQuantityUnits.availableQuantityUnit = clsSkuQuantityUnitList
        .availableQuantityUnits = clsSkuQuantityUnits
      End With
      strJsonData = JsonConvert.SerializeObject(clsSku, Newtonsoft.Json.Formatting.Indented)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strJsonData

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Compone il Json per inser/update image SKU
  ''' </summary>
  Public Overridable Function creaJsonSkuInsertUpdateImage(ByVal dtrSku As DataRow) As String
    Try
      Dim strJsonData As String
      Dim dtArt As New DataTable
      Dim dttUnmis As New DataTable
      Dim dttImage As New DataTable
      Dim bImageDefault As Boolean = True
      Dim clsSku As New skuInsertUpdateImage.skuInsertUpdate

      '--------------------------------------------------
      '--- Crea il Json per insert/update SKU
      '--------------------------------------------------
      clsSku = New skuInsertUpdateImage.skuInsertUpdate
      With clsSku
        .clientId = NTSCStr(dtrSku!clientId)
        .skuId = NTSCStr(dtrSku!skuId)
        .description = NTSCStr(dtrSku!description)
        .productCode = NTSCStr(dtrSku!productCode)
        .productGroupId = NTSCStr(dtrSku!productGroupId)
        .baseQuantityUnitId = NTSCStr(dtrSku!baseQuantityUnitId)
        .handlingHint = NTSCStr(dtrSku!handlingHint)
        .batchMandatoryForHost = cb(dtrSku!batchMandatoryForHost)
        '.stocktakingExcluded = cb(dtrOrder!stocktakingExcluded)
        .cycleCountingThreshold = NTSCInt(dtrSku!cycleCountingThreshold)

        'leggi Unità di misura da Artico
        dttUnmis = oCldCnmg.GetSkuUnmis(strDittaCorrente, .skuId)

        Dim clsSkuQuantityUnits As New skuInsertUpdateImage.skuInsertUpdate.SkuQuantityUnits
        Dim clsSkuQuantityUnitList As New List(Of skuInsertUpdateImage.SkuQuantityUnit)
        Dim clsSkuQuantityUnit As New skuInsertUpdateImage.SkuQuantityUnit
        For Each drr As DataRow In dttUnmis.Rows
          clsSkuQuantityUnit = New skuInsertUpdateImage.SkuQuantityUnit
          With clsSkuQuantityUnit
            .quantityUnitId = NTSCStr(drr!ar_unmis)
            '.unitOfMeasure = cb(dtrOrder!unitOfMeasure)
            .totalWeight = 0
            .netWeight = 0
            '.totalVolume = 1
            .totalVolume = NTSCDec(0.25)
            .length = 0
            .width = 0
            .height = 0
            '.hostWeightUnitId = 0
            '.hostVolumeUnitId = 0
            '.hostLengthUnitId = 0
            .defaultPickQuantityUnit = False
            .factorToBaseQU = NTSCDec(drr!ar_fattore)

            'leggi Immagini Artico
            dttImage = oCldCnmg.getImage(NTSCStr(dtrSku!skuId))

            'crea Json Image List
            Dim clsSkuImageLists As New skuInsertUpdateImage.SkuQuantityUnit.cSkuImageList
            Dim clsSkuImageListsList As New List(Of skuInsertUpdateImage.SkuImage)
            Dim clsSkuImage As New skuInsertUpdateImage.SkuImage
            For Each drrImage As DataRow In dttImage.Rows
              clsSkuImage = New skuInsertUpdateImage.SkuImage
              With clsSkuImage
                .imageName = NTSCStr(drrImage!ao_nomedoc)
                .skuImageSource = "EXTERNAL_URL"
                If bImageDefault Then
                  .defaultImage = True
                Else
                  .defaultImage = False
                End If
                bImageDefault = False
              End With
              clsSkuImageListsList.Add(clsSkuImage)
            Next
            clsSkuImageLists.skuImage = clsSkuImageListsList
            .skuImageList = clsSkuImageLists
          End With

          clsSkuQuantityUnitList.Add(clsSkuQuantityUnit)
        Next

        clsSkuQuantityUnits.availableQuantityUnit = clsSkuQuantityUnitList
        .availableQuantityUnits = clsSkuQuantityUnits
      End With
      strJsonData = JsonConvert.SerializeObject(clsSku, Newtonsoft.Json.Formatting.Indented)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strJsonData

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione AUTOSTORE per inserimento / modifica articoli
  ''' </summary>
  Public Overridable Function postSkuInsertUpdate _
    (
    ByVal strClientId As String,
    ByVal strURL As String,
    ByVal strSkuId As String,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dtrSku As DataRow = Nothing
    Dim strJsonData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Datarow dell'articolo da inviare
      '--------------------------------------------------
      dtrSku = oCldCnmg.GetSku(strDittaCorrente, strClientId, strSkuId)
      If dtrSku Is Nothing Then
        Return False
      End If

      '--------------------------------------------------
      '--- Crea datatable per le immagini articolo
      '--------------------------------------------------
      Dim dtt As DataTable = oCldCnmg.getImage(strSkuId)

      '--------------------------------------------------
      '--- Crea il Json per invio articolo 
      '--------------------------------------------------
      If dtt.Rows.Count = 0 Then
        strJsonData = creaJsonSkuInsertUpdate(dtrSku)
      Else
        strJsonData = creaJsonSkuInsertUpdateImage(dtrSku)
      End If

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostJSON(strJsonData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strErrore = ""
      bEsito = putHttp(httpWebRequest, strErrore)
      If bEsito Then
        strStato = "I"
      Else
        strStato = "E"
      End If
      oCldCnmg.AggiornaEsitoSku(strDittaCorrente, strClientId, strSkuId, strStato, "Insert/Update", IIf(bEsito, "OK", strErrore).ToString)

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
  ''' Chiamata al servizio REST di connessione AUTOSTORE per inserimento / modifica "massivo" articoli
  ''' </summary>
  Public Overridable Function postSkuInsertUpdate _
    (
    ByVal strClientId As String,
    ByVal strURL As String,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dttSku As New DataTable
    Dim strJsonData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Datatable di tutti gli articoli da inviare
      '--------------------------------------------------
      dttSku = oCldCnmg.GetSku(strDittaCorrente, strClientId)

      '--------------------------------------------------
      '--- Legge URL invio articolo
      '--------------------------------------------------
      For Each dtrSku As DataRow In dttSku.Rows
        'Crea il Json per invio articolo 
        strJsonData = creaJsonSkuInsertUpdate(dtrSku)

        'Esegue la chiamata al URL
        Dim a As String = strJsonData
        httpWebRequest = PostJSON(strJsonData, strURL)

        'Recupera esito chiamata
        strErrore = ""
        bEsito = putHttp(httpWebRequest, strErrore)
        If bEsito Then
          strStato = "I"
        Else
          strStato = "E"
        End If
        oCldCnmg.AggiornaEsitoSku(strDittaCorrente, strClientId, dtrSku!skuId.ToString, strStato, "Insert/Update", IIf(bEsito, "OK", strErrore).ToString)

        'Valore funzione
        If Not bEsito Then Return False
      Next

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

  ''' <summary>
  ''' chiamata al servizio REST di connessione AUTOSTORE per cancellazione
  ''' </summary>
  Public Overridable Function postSkuDelete _
    (
    ByVal strClientId As String,
    ByVal strURL As String,
    ByVal strSkuId As String,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dtrSku As DataRow = Nothing
    Dim strJsonData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Json per cancellazione articolo 
      '--------------------------------------------------
      strJsonData = creaJsonSkuDelete(strClientId, strSkuId)

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostJSON(strJsonData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strErrore = ""
      bEsito = putHttp(httpWebRequest, strErrore)
      If bEsito Then
        strStato = "D"
      Else
        strStato = "E"
      End If

      '--------------------------------------------------
      '--- Aggiorna esito chiamata
      '--------------------------------------------------
      oCldCnmg.AggiornaEsitoSku(strDittaCorrente, strClientId, strSkuId, strStato, "Delete", IIf(bEsito, "OK", strErrore).ToString)

      '--------------------------------------------------
      '--- Aggiorna esito chiamata
      '--------------------------------------------------
      Return bEsito

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function
#End Region

#Region "Gestione STOCKTACKING - Articoli"
  ''' <summary>
  ''' Compone il Json per inser/update STOCKTACKING
  ''' </summary>
  Public Overridable Function creaJsonStocktackingRequest(ByVal dtrOrder As DataRow) As String
    'Dichiarazione variabili
    Dim strJsonData As String
    Dim dttCorpo As New DataTable
    Dim clsOrder As New StocktakingRequest.stocktakingRequest

    Try
      clsOrder = New StocktakingRequest.stocktakingRequest
      With clsOrder

        .clientId = NTSCStr(dtrOrder!clientId)
        .orderId = NTSCStr(dtrOrder!orderId)

        'leggi Corpo order (HHAUORDERMOV)
        dttCorpo = oCldCnmg.GetOrderMov(strDittaCorrente, .clientId, .orderId)

        Dim clsOrderLines As New StocktakingRequest.stocktakingRequest.cOrderLines
        Dim clsOrderLinesList As New List(Of StocktakingRequest.orderLine)
        Dim clsOrderLine As New StocktakingRequest.orderLine

        For Each dtrCorpo As DataRow In dttCorpo.Rows
          clsOrderLine = New StocktakingRequest.orderLine

          With clsOrderLine
            .orderLineId = NTSCStr(dtrCorpo!orderLineId)
            .criteriaUsed = NTSCStr(dtrCorpo!criteriaUsed)

            Dim cInventoryCriteria As New StocktakingRequest.orderLine.clsInventoryCriteria

            With cInventoryCriteria
              .clientId = NTSCStr(dtrOrder!clientId)
              .skuId = NTSCStr(dtrCorpo!skuId)
            End With

            .inventoryCriteria = cInventoryCriteria

          End With
          clsOrderLinesList.Add(clsOrderLine)
        Next

        clsOrderLines.orderLine = clsOrderLinesList
        .orderLines = clsOrderLines

      End With

      strJsonData = JsonConvert.SerializeObject(clsOrder, Newtonsoft.Json.Formatting.Indented)

      Return strJsonData

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione AUTOSTORE per inserimento / modifica articoli
  ''' </summary>
  Public Overridable Function postStocktackingRequest _
    (
    ByVal strClientId As String,
    ByVal strURL As String,
    ByVal strURLSku As String,
    ByVal strOrderId As String,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dtrOrder As DataRow = Nothing
    Dim strJsonData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Datarow dell'order da inviare
      '--------------------------------------------------
      dtrOrder = oCldCnmg.GetOrder(strDittaCorrente, strClientId, strOrderId)
      If dtrOrder Is Nothing Then
        Return False
      End If

      '--------------------------------------------------
      '--- Crea il Json per invio advice 
      '--------------------------------------------------
      strJsonData = creaJsonStocktackingRequest(dtrOrder)

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostJSON(strJsonData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strErrore = ""
      bEsito = putHttp(httpWebRequest, strErrore)
      If bEsito Then
        strStato = "I"
      Else
        strStato = "E"
      End If
      'AAAAAAAAAAAAAAAAAAAAAA
      'DA IMPLEMENTARE
      oCldCnmg.AggiornaEsitoOrder(strDittaCorrente, strClientId, strOrderId, strStato, "Insert/Update", IIf(bEsito, "OK", strErrore).ToString)

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

#End Region

#Region "Gestione ADVICE - versamenti"
  ''' <summary>
  ''' Compone il Json per delete ADVICE
  ''' </summary>
  Public Overridable Function creaJsonAdviceDelete _
    (
    ByVal strClientId As String,
    ByVal strAdviceId As String
    ) As String

    Dim strJsonData As String
    Dim clsAdvice As New AdviceDelete.adviceDelete

    Try
      '--------------------------------------------------
      '--- Crea il Json per delete SKU
      '--------------------------------------------------
      clsAdvice = New AdviceDelete.adviceDelete
      With clsAdvice
        .clientId = NTSCStr(strClientId)
        .adviceId = NTSCStr(strAdviceId)
      End With
      strJsonData = JsonConvert.SerializeObject(clsAdvice, Newtonsoft.Json.Formatting.Indented)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strJsonData

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Compone il Json per insert / update ADVICE
  ''' </summary>
  Public Overridable Function creaJsonAdviceInsertUpdate(ByVal dtrAdvice As DataRow) As String
    'DIchiarazione variabili
    Dim strJsonData As String
    Dim dttCorpo As New DataTable
    Dim clsAdvice As New AdviceInsertUpdate.adviceInsertUpdate

    Try
      '--------------------------------------------------
      '--- Compone il Json per inser/update ADVICE
      '--------------------------------------------------
      clsAdvice = New AdviceInsertUpdate.adviceInsertUpdate
      With clsAdvice

        .clientId = NTSCStr(dtrAdvice!clientId)
        .adviceId = NTSCStr(dtrAdvice!adviceId)
        .referenceBarcode = NTSCStr(dtrAdvice!referenceBarcode)
        .supplierId = NTSCStr(dtrAdvice!supplierID)
        .additionalHostData = NTSCStr(dtrAdvice!additionalHostData)

        'leggi Corpo Advice (HHAUADVICEMOV)
        dttCorpo = oCldCnmg.GetAdviceMov(strDittaCorrente, .clientId, .adviceId)

        Dim clsAdviceLines As New AdviceInsertUpdate.adviceInsertUpdate.cAdviceLines
        Dim clsAdviceLinesList As New List(Of AdviceInsertUpdate.adviceLine)
        Dim clsAdviceLine As New AdviceInsertUpdate.adviceLine

        For Each dtrCorpo As DataRow In dttCorpo.Rows
          clsAdviceLine = New AdviceInsertUpdate.adviceLine
          With clsAdviceLine

            .adviceLineId = NTSCStr(dtrCorpo!adviceLineId)
            .skuId = NTSCStr(dtrCorpo!skuId)
            .quantityTarget = NTSCInt(dtrCorpo!quantityTarget)
            .receivingQuantityUnit = NTSCStr(dtrCorpo!receivingQuantityUnit)
            .batch = NTSCStr(dtrCorpo!batch)
            .bestBeforeDate = NTSCDate(dtrCorpo!bestBeforeDate)

          End With
          clsAdviceLinesList.Add(clsAdviceLine)

        Next
        clsAdviceLines.adviceLine = clsAdviceLinesList
        .adviceLines = clsAdviceLines

      End With

      strJsonData = JsonConvert.SerializeObject(clsAdvice, Newtonsoft.Json.Formatting.Indented)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return strJsonData

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione AUTOSTORE per inserimento / modifica advice
  ''' </summary>
  Public Overridable Function postAdviceInsertUpdate _
    (
    ByVal strClientId As String,
    ByVal strURL As String,
    ByVal strURLSku As String,
    ByVal strAdviceId As String,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dtrAdvice As DataRow = Nothing
    Dim strJsonData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Datarow dell'advice da inviare
      '--------------------------------------------------
      dtrAdvice = oCldCnmg.GetAdvice(strDittaCorrente, strClientId, strAdviceId)
      If dtrAdvice Is Nothing Then
        Return False
      End If

      '--------------------------------------------------
      '--- Crea il Json per invio advice 
      '--------------------------------------------------
      strJsonData = creaJsonAdviceInsertUpdate(dtrAdvice)

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostJSON(strJsonData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strErrore = ""
      bEsito = putHttp(httpWebRequest, strErrore)
      If bEsito Then
        strStato = "I"
      Else
        strStato = "E"
      End If
      oCldCnmg.AggiornaEsitoAdvice(strDittaCorrente, strClientId, strAdviceId, strStato, "Insert/Update", IIf(bEsito, "OK", strErrore).ToString)

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
  ''' Chiamata al servizio REST di connessione AUTOSTORE per cancellazione advice
  ''' </summary>
  Public Overridable Function postAdviceDelete _
    (
    ByVal strClientId As String,
    ByVal strURL As String,
    ByVal strAdviceId As String,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim strJsonData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Json per invio advice 
      '--------------------------------------------------
      strJsonData = creaJsonAdviceDelete(strClientId, strAdviceId)

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostJSON(strJsonData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strErrore = ""
      bEsito = putHttp(httpWebRequest, strErrore)
      If bEsito Then
        strStato = "D"
      Else
        strStato = "E"
      End If

      '--------------------------------------------------
      '--- Aggiorna esito chiamata
      '--------------------------------------------------
      oCldCnmg.AggiornaEsitoAdvice(strDittaCorrente, strClientId, strAdviceId, strStato, "Delete", IIf(bEsito, "OK", strErrore).ToString)

      '--------------------------------------------------
      '--- Aggiorna esito chiamata
      '--------------------------------------------------
      Return bEsito

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function
#End Region

#Region "Gestione ORDER - prelievi"
  ''' <summary>
  ''' Compone il Json per insert / update ORDER
  ''' </summary>
  Public Overridable Function creaJsonOrderInsertUpdate(ByVal dtrOrder As DataRow) As String
    'Dichiarazione variabili
    Dim strJsonData As String
    Dim dttCorpo As New DataTable
    Dim clsOrder As New OrderInsertUpdate.orderInsertUpdate

    Try
      clsOrder = New OrderInsertUpdate.orderInsertUpdate
      With clsOrder

        .clientId = NTSCStr(dtrOrder!clientId)
        .orderId = NTSCStr(dtrOrder!orderId)
        .priority = NTSCInt(dtrOrder!priority)
        .additionalHostData = NTSCStr(dtrOrder!additionalHostData)
        '.customerId = NTSCStr(dtrOrder!customerId)
        .customerOrderType = NTSCStr(dtrOrder!customerOrderType)
        .defaultDestinationId = "As.SHIP.0.0.ASSHIPNP01"
        .earliestStartTime = NTSCDate(dtrOrder!earliestStartTime)
        .latestStartTime = NTSCDate(New Date(2099, 12, 31))
        .earliestStagingTime = NTSCDate(dtrOrder!earliestStagingTime)
        .latestStagingTime = NTSCDate(New Date(2099, 12, 31))
        .deliveryTime = NTSCDate(dtrOrder!deliveryTime)
        .shippingCondition = "WEB"

        'leggi Corpo order (HHAUORDERMOV)
        dttCorpo = oCldCnmg.GetOrderMov(strDittaCorrente, .clientId, .orderId)

        Dim clsOrderLines As New OrderInsertUpdate.orderInsertUpdate.cOrderLines
        Dim clsOrderLinesList As New List(Of OrderInsertUpdate.orderLine)
        Dim clsOrderLine As New OrderInsertUpdate.orderLine

        For Each dtrCorpo As DataRow In dttCorpo.Rows
          clsOrderLine = New OrderInsertUpdate.orderLine

          With clsOrderLine
            .orderLineId = NTSCStr(dtrCorpo!orderLineId)
            .criteriaUsed = NTSCStr(dtrCorpo!criteriaUsed)

            Dim cInventoryCriteria As New OrderInsertUpdate.orderLine.clsInventoryCriteria

            With cInventoryCriteria
              .skuId = NTSCStr(dtrCorpo!skuId)
              .batch = NTSCStr(dtrCorpo!batch)
              .quantityBaseTargetHost = NTSCInt(dtrCorpo!quantityBaseTargetHost)
              .quantityUnit = NTSCStr(dtrCorpo!quantityUnit)
              .unlimitedOverdeliveryAllowed = cb(dtrCorpo!unlimitedOverdeliveryAllowed)
              .underdeliveryTolerance = NTSCInt(dtrCorpo!underdeliveryTolerance)
            End With

            .inventoryCriteria = cInventoryCriteria

          End With
          clsOrderLinesList.Add(clsOrderLine)
        Next

        clsOrderLines.orderLine = clsOrderLinesList
        .orderLines = clsOrderLines

      End With

      strJsonData = JsonConvert.SerializeObject(clsOrder, Newtonsoft.Json.Formatting.Indented)

      Return strJsonData

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Compone il Json per delete ORDER
  ''' </summary>
  Public Overridable Function creaJsonOrderDelete(ByVal strClientId As String, ByVal strOrderId As String) As String
    Try
      Dim strJsonData As String
      Dim dtCorpo As New DataTable
      Dim clsOrder As New OrderDelete.orderDelete

      clsOrder = New OrderDelete.orderDelete
      With clsOrder
        .clientId = NTSCStr(strClientId)
        .orderId = NTSCStr(strOrderId)
      End With

      strJsonData = JsonConvert.SerializeObject(clsOrder, Newtonsoft.Json.Formatting.Indented)

      Return strJsonData

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return ""
    End Try
  End Function

  ''' <summary>
  ''' Chiamata al servizio REST di connessione AUTOSTORE per inserimento / modifica order
  ''' </summary>
  Public Overridable Function postOrderInsertUpdate _
    (
    ByVal strClientId As String,
    ByVal strURL As String,
    ByVal strURLSku As String,
    ByVal strOrderId As String,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dtrOrder As DataRow = Nothing
    Dim strJsonData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Datarow dell'order da inviare
      '--------------------------------------------------
      dtrOrder = oCldCnmg.GetOrder(strDittaCorrente, strClientId, strOrderId)
      If dtrOrder Is Nothing Then
        Return False
      End If

      '--------------------------------------------------
      '--- Crea il Json per invio advice 
      '--------------------------------------------------
      strJsonData = creaJsonOrderInsertUpdate(dtrOrder)

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostJSON(strJsonData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strErrore = ""
      bEsito = putHttp(httpWebRequest, strErrore)
      If bEsito Then
        strStato = "I"
      Else
        strStato = "E"
      End If
      oCldCnmg.AggiornaEsitoOrder(strDittaCorrente, strClientId, strOrderId, strStato, "Insert/Update", IIf(bEsito, "OK", strErrore).ToString)

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
  ''' Chiamata al servizio REST di connessione AUTOSTORE per delete order
  ''' </summary>
  Public Overridable Function postOrderDelete _
    (
    ByVal strClientId As String,
    ByVal strURL As String,
    ByVal strOrderId As String,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim strJsonData As String
    Dim httpWebRequest As HttpWebRequest = Nothing
    Dim bEsito As Boolean = False
    Dim strStato As String = ""

    Try
      '--------------------------------------------------
      '--- Crea il Json per invio advice 
      '--------------------------------------------------
      strJsonData = creaJsonOrderDelete(strClientId, strOrderId)

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = PostJSON(strJsonData, strURL)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strErrore = ""
      bEsito = putHttp(httpWebRequest, strErrore)
      If bEsito Then
        strStato = "D"
      Else
        strStato = "E"
      End If

      '--------------------------------------------------
      '--- Aggiorna esito chiamata
      '--------------------------------------------------
      oCldCnmg.AggiornaEsitoOrder(strDittaCorrente, strClientId, strOrderId, strStato, "Delete", IIf(bEsito, "OK", strErrore).ToString)

      '--------------------------------------------------
      '--- Aggiorna esito chiamata
      '--------------------------------------------------
      Return bEsito

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
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

    Dim dsCnmg As New DataSet
    Dim strSQL As String = ""
    Dim dtrChiamata As DataRow = Nothing
    Dim strURL As String = ""
    Dim strCodart As String = ""

    Try

      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(dsCnmg) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Magazzino"
        Return False
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(INVIOARTICOLI)).Length = 0 Then
        strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento articoli Magazzino non configurata"
        Return False
      Else
        dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(INVIOARTICOLI))(0)
      End If

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      strURL = NTSCStr(dtrChiamata!ch_urlchia)

      '-------------------------------------------------
      '--- Inserisce i dati nella tabella 
      '-------------------------------------------------
      For Each dtrArtico As DataRow In dttArtico.Rows
        strCodart = NTSCStr(dtrArtico!ar_codart)

        If Not oCldCnmg.InserisciDatiArticolo(strDitta, strCodart) Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Errore In selezione dati articolo " + CStrSQL(strCodart) + " per IncaricoTech"
          Return False
        Else
          If Not postAnagrafiche(strURL, strCodart, strErrore) Then Return False
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
  ''' Invia la lista di prelievo al magazzino automatico MAGAZZINO
  ''' </summary>
  Public Overridable Function InviaListaDiPrelievo _
    (
    ByVal strDitta As String,
    ByVal bDelete As Boolean,
    ByVal dsDoc As DataSet,
    ByRef strErrore As String
    ) As Boolean

    Dim dsCnmg As New DataSet
    Dim strSQL As String = ""
    Dim dtrChiamata As DataRow = Nothing
    Dim dtrChiamataArt As DataRow = Nothing
    Dim strURL As String = ""
    Dim strURLArt As String = ""
    Dim strLista As String = ""

    Try
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(dsCnmg) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Magazzino"
        Return False
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If Not bDelete Then
        'Inserisci Lista di Prelievo
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(LISTAPRELIEVO)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento lista di prelievo Incarico Tech non configurata"
          Return False
        Else
          dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(LISTAPRELIEVO))(0)
        End If

        'Inserisci Articoli
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(INVIOARTICOLI)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento articoli Incarico Tech non configurata"
          Return False
        Else
          dtrChiamataArt = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(INVIOARTICOLI))(0)
        End If
      Else
        'Cancella Lista di Prelievo
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(CANCELLALISTAPRELIEVO)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Cancellazione lista di perlievo Incarico Tech non configurata"
          Return False
        Else
          dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(CANCELLALISTAPRELIEVO))(0)
        End If
      End If

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      'Articoli
      strURLArt = NTSCStr(dtrChiamataArt!ch_urlchia)
      'Lista Prelievo
      strURL = NTSCStr(dtrChiamata!ch_urlchia)

      '-------------------------------------------------
      '--- Inserisce i dati nella tabella 
      '-------------------------------------------------
      If Not oCldCnmg.InserisciDatiListaDiPrelievo(strDitta, dsDoc, strLista) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In selezione dati lista di prelievo " + dsDoc.Tables("TESTA").Rows(0) !et_numdoc.ToString + IIf(dsDoc.Tables("TESTA").Rows(0) !et_serie.ToString.Trim.Length = 0, "", "/" + dsDoc.Tables("TESTA").Rows(0) !et_serie.ToString).ToString + " per magazzino Incarico Tech"
        Return False
      Else
        'Invia Lista di Prelievo
        If Not bDelete Then
          'Invia tutti gli articoli della Lista di Prelievo
          If Not postAnagrafiche(strURLArt, strErrore) Then
            Return False
          End If

          'Invia Lista di Prelievo
          If Not postListaDiPrelievo(strURL, strLista, strErrore) Then
            Return False
          End If
        Else
          'If postOrderDelete(strClientid, strURL, strLista, strErrore) Then
          '  Return False
          'End If
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
  ''' Invia gli sku (articoli) al magazzino automatico AUTOSTORE
  ''' </summary>
  Public Overridable Function InviaSku _
    (
    ByVal strDitta As String,
    ByVal bDelete As Boolean,
    ByVal dttArtico As DataTable,
    ByRef strErrore As String
    ) As Boolean

    Dim dsCnmg As New DataSet
    Dim strSQL As String = ""
    Dim dtrChiamata As DataRow = Nothing
    Dim strClientid As String = ""
    Dim strURL As String = ""
    Dim strCodart As String = ""

    Try
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(strDitta, dsCnmg) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Autostore"
        Return False
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If Not bDelete Then
        'skuInsertUpdate
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(SKUINSERTUPDATE)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento/Modifica articoli Autostore non configurata"
          Return False
        Else
          dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(SKUINSERTUPDATE))(0)
        End If
      Else
        'skuDelete
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(SKUDELETE)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Cancellazione articoli Autostore non configurata"
          Return False
        Else
          dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(SKUDELETE))(0)
        End If
      End If

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      strURL = NTSCStr(dtrChiamata!ch_urlchia)
      strClientid = NTSCStr(dtrChiamata!ch_clientid)

      '-------------------------------------------------
      '--- Inserisce i dati nella tabella 
      '-------------------------------------------------
      For Each dtrArtico As DataRow In dttArtico.Rows
        strCodart = NTSCStr(dtrArtico!ar_codart)

        If Not oCldCnmg.InserisciDatiSku(strDitta, strClientid, strCodart) Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Errore In selezione dati articolo " + CStrSQL(strCodart) + " per Autostore"
          Return False
        Else
          If Not bDelete Then
            If Not postSkuInsertUpdate(strClientid, strURL, strCodart, strErrore) Then
              Return False
            End If
          Else
            If Not postSkuDelete(strClientid, strURL, strCodart, strErrore) Then
              Return False
            End If
          End If
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
  ''' Invia gli advice (depositi) al magazzino automatico AUTOSTORE
  ''' </summary>
  Public Overridable Function InviaAdvice _
    (
    ByVal strDitta As String,
    ByVal bDelete As Boolean,
    ByVal dsDoc As DataSet,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dsCnmg As New DataSet
    Dim strSQL As String = ""
    Dim dtrChiamata As DataRow = Nothing
    Dim dtrChiamataSku As DataRow = Nothing
    Dim strClientid As String = ""
    Dim strURL As String = ""
    Dim strURLSku As String = ""
    Dim strAdviceId As String = ""
    Dim nMagazzinoAutostore As Integer = 0

    'Variabili per stampa report Advice
    Dim strNomeRpt As String = "BSHHAUTO_ADVICE.RPT"
    Dim strTitleRpt As String = "Ricevimento Merce"
    Dim strKey As String = "Reports1"
    'Dim nPjob As Object
    Dim nRis As Integer = 0
    Dim strCrpe As String = ""
    'Dim i As Integer

    Try
      '-------------------------------------------------
      '--- Leggo magazzino carico ricevimento merce WMS autostore
      '-------------------------------------------------
      nMagazzinoAutostore = NTSCInt(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_MagazzinoAutostore", "1004", "", "1004"))

      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(strDitta, dsCnmg) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Autostore"
        Return False
      End If

      'GRAZIANO 5-11-2022 da sistemare

      '-------------------------------------------------
      '--- Avvio la stampa del rscontro
      '-------------------------------------------------
      'Filtro report
      With dsDoc.Tables("TESTA").Rows(0)
        strCrpe =
          "{testmag.codditt} = " & ConvStrRpt(strDittaCorrente) &
          " And {testmag.tm_tipork} = " & ConvStrRpt(NTSCStr(!et_tipork)) &
          " And {testmag.tm_anno} = " & NTSCInt(!et_anno) &
          " And {testmag.tm_serie} = " & ConvStrRpt(NTSCStr(!et_serie)) &
          " And {testmag.tm_numdoc} In [" & NTSCInt(!et_numdoc) & "]"
      End With

      ''Imposta oggetti per istanziare motore di stampa
      'Dim oMenu As Object = CType(oCleLbmenu, CLE__MENU)
      'Dim oCldMenu As New CLD__MENU
      'oCldMenu.Init(oApp)
      'Dim oCldBase As New CLD__BASE
      'oCldBase.Init(oApp)
      'Dim oCrpe As New CLE__CRPE
      'oCrpe.Init(oApp, oCldBase, "", oMenu)

      ''Preparo il motore di stampa
      'nPjob = Nothing
      'nPjob = oCrpe.ReportPEInit(oApp.Ditta, Nothing, "BSHHAUTO", strKey, " ", 0, 0, strNomeRpt, False, strTitleRpt, False)
      'If nPjob Is Nothing Then Return False

      ''Lancio tutti gli eventuali reports (gestisce già il multireport)
      'For i = 1 To UBound(CType(nPjob, Array), 2)
      '  nRis = oCrpe.PESetSelectionFormula(NTSCInt(CType(nPjob, Array).GetValue(0, i)), strCrpe)
      '  nRis = oCrpe.ReportPEVai(NTSCInt(CType(nPjob, Array).GetValue(0, i)))
      'Next

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If Not bDelete Then
        'AdviceInsertUpdate
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(ADVICEINSERTUPDATE)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento/Modifica advice Autostore non configurata"
          Return False
        Else
          dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(ADVICEINSERTUPDATE))(0)
        End If

        'skuInsertUpdate
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(SKUINSERTUPDATE)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento/Modifica articoli Autostore non configurata"
          Return False
        Else
          dtrChiamataSku = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(SKUINSERTUPDATE))(0)
        End If
      Else
        'AdviceDelete
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(ADVICEDELETE)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Cancellazione advice Autostore non configurata"
          Return False
        Else
          dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(ADVICEDELETE))(0)
        End If
      End If

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      'Sku
      strURLSku = NTSCStr(dtrChiamataSku!ch_urlchia)
      'Advice
      strURL = NTSCStr(dtrChiamata!ch_urlchia)
      strClientid = NTSCStr(dtrChiamata!ch_clientid)

      '-------------------------------------------------
      '--- Inserisce i dati nella tabella 
      '-------------------------------------------------
      If Not oCldCnmg.InserisciDatiAdvice(strDitta, strClientid, dsDoc, nMagazzinoAutostore, "AD", strAdviceId) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In selezione dati BMI " + dsDoc.Tables("TESTA").Rows(0) !et_numdoc.ToString + IIf(dsDoc.Tables("TESTA").Rows(0) !et_serie.ToString.Trim.Length = 0, "", "/" + dsDoc.Tables("TESTA").Rows(0) !et_serie.ToString).ToString + " per Autostore"
        Return False
      Else
        'Invia Advice
        If Not bDelete Then
          'Invia tutti gli articoli del'advice
          If Not postSkuInsertUpdate(strClientid, strURLSku, strErrore) Then
            Return False
          End If

          'Invia advice
          If Not postAdviceInsertUpdate(strClientid, strURL, strURLSku, strAdviceId, strErrore) Then
            Return False
          End If
        Else
          If postAdviceDelete(strClientid, strURL, strAdviceId, strErrore) Then
            Return False
          End If
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
  ''' Invia gli orders (prelievi) al magazzino automatico AUTOSTORE
  ''' </summary>
  Public Overridable Function InviaOrder _
    (
    ByVal strDitta As String,
    ByVal bDelete As Boolean,
    ByVal strBusTipo As String,
    ByVal nMagazPrelievoAutostore As Integer,
    ByVal dsDoc As DataSet,
    ByVal nPriorita As Integer,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dsCnmg As New DataSet
    Dim strSQL As String = ""
    Dim dtrChiamata As DataRow = Nothing
    Dim dtrChiamataSku As DataRow = Nothing
    Dim strClientid As String = ""
    Dim strURL As String = ""
    Dim strURLSku As String = ""
    Dim strOrderId As String = ""

    Try
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(strDitta, dsCnmg) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Autostore"
        Return False
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If Not bDelete Then
        'OrderInsertUpdate
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(ORDERINSERTUPDATE)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento/Modifica order Autostore non configurata"
          Return False
        Else
          dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(ORDERINSERTUPDATE))(0)
        End If

        'skuInsertUpdate
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(SKUINSERTUPDATE)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Inserimento/Modifica articoli Autostore non configurata"
          Return False
        Else
          dtrChiamataSku = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(SKUINSERTUPDATE))(0)
        End If
      Else
        'OrderDelete
        If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(ADVICEDELETE)).Length = 0 Then
          strErrore =
            "ATTENZIONE!" + vbCrLf +
            "Chiamata di Cancellazione advice Autostore non configurata"
          Return False
        Else
          dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(ADVICEDELETE))(0)
        End If
      End If

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      'Sku
      strURLSku = NTSCStr(dtrChiamataSku!ch_urlchia)
      'Advice
      strURL = NTSCStr(dtrChiamata!ch_urlchia)
      strClientid = NTSCStr(dtrChiamata!ch_clientid)

      '-------------------------------------------------
      '--- Inserisce i dati nella tabella 
      '-------------------------------------------------
      If Not oCldCnmg.InserisciDatiOrder(strDitta, strClientid, dsDoc, nMagazPrelievoAutostore, strBusTipo, nPriorita, strOrderId) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In selezione dati NDP " + dsDoc.Tables("TESTA").Rows(0) !et_numdoc.ToString + IIf(dsDoc.Tables("TESTA").Rows(0) !et_serie.ToString.Trim.Length = 0, "", "/" + dsDoc.Tables("TESTA").Rows(0) !et_serie.ToString).ToString + " per Autostore"
        Return False
      Else
        'Invia Order
        If Not bDelete Then
          'Invia tutti gli articoli del'advice
          If Not postSkuInsertUpdate(strClientid, strURLSku, strErrore) Then
            Return False
          End If

          'Invia Order
          If Not postOrderInsertUpdate(strClientid, strURL, strURLSku, strOrderId, strErrore) Then
            Return False
          End If
        Else
          If postOrderDelete(strClientid, strURL, strOrderId, strErrore) Then
            Return False
          End If
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
  ''' Invia richieste stocktaking (interrogazione giacenze) al magazzino automatico AUTOSTORE
  ''' </summary>
  Public Overridable Function InviaStocktackingRequest _
    (
    ByVal strDitta As String,
    ByVal strBusTipo As String,
    ByVal nMagazPrelievoAutostore As Integer,
    ByVal dsDoc As DataSet,
    ByVal nPriorita As Integer,
    ByRef strErrore As String
    ) As Boolean

    'Dichiarazione variabili
    Dim dsCnmg As New DataSet
    Dim strSQL As String = ""
    Dim dtrChiamata As DataRow = Nothing
    Dim dtrChiamataSku As DataRow = Nothing
    Dim strClientid As String = ""
    Dim strURL As String = ""
    Dim strURLSku As String = ""
    Dim strOrderId As String = ""
    Dim nMagazzinoPrincipale As Integer = 0

    Try
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(strDitta, dsCnmg) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Autostore"
        Return False
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      'OrderInsertUpdate
      If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(STOCKTACKINGREQUEST)).Length = 0 Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Chiamata di Inserimento/Modifica stocktacking Autostore non configurata"
        Return False
      Else
        dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(STOCKTACKINGREQUEST))(0)
      End If

      'skuInsertUpdate
      If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(SKUINSERTUPDATE)).Length = 0 Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Chiamata di Inserimento/Modifica articoli Autostore non configurata"
        Return False
      Else
        dtrChiamataSku = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(SKUINSERTUPDATE))(0)
      End If

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      'Sku
      strURLSku = NTSCStr(dtrChiamataSku!ch_urlchia)
      'StockTacking
      strURL = NTSCStr(dtrChiamata!ch_urlchia)
      strClientid = NTSCStr(dtrChiamata!ch_clientid)

      '-------------------------------------------------
      '--- Inserisce i dati nella tabella 
      '-------------------------------------------------
      'AAAAAAAAAAAAAAAAAAAA
      'DA RIVEDERE
      If Not oCldCnmg.InserisciDatiOrder(strDitta, strClientid, dsDoc, nMagazPrelievoAutostore, strBusTipo, nPriorita, strOrderId) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In selezione dati NDP " + dsDoc.Tables("TESTA").Rows(0) !et_numdoc.ToString + IIf(dsDoc.Tables("TESTA").Rows(0) !et_serie.ToString.Trim.Length = 0, "", "/" + dsDoc.Tables("TESTA").Rows(0) !et_serie.ToString).ToString + " per Autostore"
        Return False
      Else
        'Invia Order
        'Invia tutti gli articoli dell'Order
        If Not postSkuInsertUpdate(strClientid, strURLSku, strErrore) Then
          Return False
        End If

        'Invia stocktackingrequest
        If Not postStocktackingRequest(strClientid, strURL, strURLSku, strOrderId, strErrore) Then
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

#Region "Funzioni e Routines per ricezione Conferme AUTOSTORE"
  ''' <summary>
  ''' Importazione delle conferme dal server di AUTOSTORE
  ''' </summary>
  Public Overridable Function ImportaConferme _
    (
    ByVal strDitta As String,
    ByRef strErrore As String
    ) As DataSet

    Dim dsCnmg As New DataSet
    Dim strSQL As String = ""
    Dim dtrChiamata As DataRow = Nothing
    Dim strClientid As String = ""
    Dim strFile As String = ""
    Dim strCodart As String = ""
    Dim dsOut As New DataSet

    Try
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(strDitta, dsCnmg) Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Errore In apertura chiamate di collegamento Autostore"
        Return Nothing
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(CONFIRM)).Length = 0 Then
        strErrore =
          "ATTENZIONE!" + vbCrLf +
          "Chiamata di ricevzione Conferme articoli Autostore non configurata"
        Return Nothing
      Else
        dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(CONFIRM))(0)
      End If

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      strFile = NTSCStr(dtrChiamata!ch_urlchia)
      strClientid = NTSCStr(dtrChiamata!ch_clientid)


      dsOut = GetConferme(strClientid, strFile, strErrore)

      '-------------------------------------------------
      '--- Valore funzione
      '-------------------------------------------------
      Return dsOut

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Crea il datatset delle conferme Advice e Orders
  ''' </summary>
  Public Overridable Function GetConferme _
    (
    ByVal strClientId As String,
    ByVal strFile As String,
    ByRef strErrore As String
    ) As DataSet

    'Dim bEsito As Boolean = False
    Dim strStato As String = ""
    Dim dsOut As New DataSet
    Dim dttAdvice As New DataTable
    Dim dttOrder As New DataTable
    Dim dttFile As New DataTable
    Dim dr As DataRow = Nothing

    Try
      '--------------------------------------------------
      '--- Verifica che esita la cartella delle riosposte
      '--------------------------------------------------
      If Not Directory.Exists(strFile) Then
        strErrore = "Cartella " + strFile + " non non trovata "
        Return Nothing
      End If

      '--------------------------------------------------
      '--- Crea il datatabvle della ricezione risposte
      '--------------------------------------------------
      dttFile.Columns.Add("ID")
      dttFile.Columns.Add("Tipo")
      dttFile.Columns.Add("File")
      dttFile.Columns.Add("NomeFile")

      '--------------------------------------------------
      '--- Ciclo di lettura adviceconfirm
      '--------------------------------------------------
      For Each filename As String In Directory.GetFiles(strFile, "*adviceconfirm*")
        If filename.ToUpper.EndsWith(".TODO") Then
          Dim f As New FileInfo(filename)
          dr = GetFileadvice(strClientId, filename, strErrore)
          If Not dr Is Nothing Then
            If dttAdvice.Rows.Count = 0 Then
              dttAdvice = dr.Table.Copy
            Else
              dttAdvice.ImportRow(dr)
            End If
            dttFile.Rows.Add(NTSCStr(dr!adviceId), "AD", filename, f.Name)
          End If
          'GetFileadvice(strClientId, filename, strErrore)
        End If
      Next
      dttAdvice.TableName = "ADVICE"

      '--------------------------------------------------
      '--- Ciclo di lettura orderconfirm
      '--------------------------------------------------
      For Each filename As String In Directory.GetFiles(strFile, "*orderconfirm*")
        If filename.ToUpper.EndsWith(".TODO") Then
          Dim f As New FileInfo(filename)
          dr = GetFileorder(strClientId, filename, strErrore)
          If Not dr Is Nothing Then
            If dttOrder.Rows.Count = 0 Then
              dttOrder = dr.Table.Copy
            Else
              dttOrder.ImportRow(dr)
            End If
            dttFile.Rows.Add(NTSCStr(dr!orderId), "Or", filename, f.Name)
          End If
          'GetFileorder(strClientId, filename, strErrore)
        End If
      Next
      dttOrder.TableName = "ORDER"
      dttFile.TableName = "FILE"

      '--------------------------------------------------
      '--- Imposta Dataset
      '--------------------------------------------------
      dsOut.Tables.Add(dttAdvice)
      dsOut.Tables.Add(dttOrder)
      dsOut.Tables.Add(dttFile)

      '--------------------------------------------------
      '--- Valore funzione
      '--------------------------------------------------
      Return dsOut

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Importazione delle conferme advice dal server di AUTOSTORE
  ''' </summary>
  Public Overridable Function GetFileadvice _
    (
    ByVal strClientId As String,
    ByVal strFile As String,
    ByRef strErrore As String
    ) As DataRow

    'Dim bEsito As Boolean = False
    Dim strStatoCorpo As String = ""
    Dim strStatoTesta As String = ""
    Dim xmldoc As New XmlDocument()
    Dim strAdviceId As String = ""
    Dim strAdviceLineId As String = ""
    Dim adviceLinesConfirm As XmlNode
    Dim adviceLineConfirm As XmlNodeList
    Dim dQuantInviata As Decimal = 0
    Dim dQuantConfermata As Decimal = 0
    Dim strTextFile As String = ""
    Dim strBaia As String = ""

    Try
      '--------------------------------------------------
      '--- Legge il file di advicecofirm
      '--------------------------------------------------
      xmldoc.Load(strFile)
      adviceLinesConfirm = xmldoc.DocumentElement.SelectNodes("adviceLinesConfirm")(0)
      adviceLineConfirm = adviceLinesConfirm.SelectNodes("adviceLineConfirm")
      strAdviceId = xmldoc.DocumentElement.SelectSingleNode("adviceId").InnerText
      strTextFile = File.ReadAllText(strFile)

      '--------------------------------------------------
      '--- Verfica se esiste l'advice nella tabelle Business
      '--------------------------------------------------
      If oCldCnmg.GetAdvice(strDittaCorrente, strClientId, strAdviceId) Is Nothing Then
        Return Nothing
      End If

      '--------------------------------------------------
      '--- Ciclo di riscontro quantità advice
      '--------------------------------------------------
      For Each node As XmlNode In adviceLinesConfirm
        strAdviceLineId = NTSCStr(node.SelectSingleNode("adviceLineId").InnerText).PadLeft(4, "0"c)
        dQuantInviata = NTSCDec(node.SelectSingleNode("quantityTarget").InnerText)
        dQuantConfermata = NTSCDec(node.SelectSingleNode("quantityCurrent").InnerText)
        'GRAZIANO 14/09/2022
        'Se la riga è stata dichiarata con quantityCurrent = 0 il tag receivingLocationId non è presente
        'quindi prima verifico altrmenti da errore
        If Not node.SelectSingleNode("receivingLocationId") Is Nothing Then
          strBaia = NTSCStr(node.SelectSingleNode("receivingLocationId").InnerText)
        Else
          strBaia = ""
        End If
        'If dQuantInviata = dQuantConfermata Then
        '  strStatoCorpo = "C"
        'ElseIf dQuantInviata > dQuantConfermata Then
        '  strStatoCorpo = "P"
        'ElseIf dQuantInviata < dQuantConfermata Then
        '  strStatoCorpo = "P"
        'ElseIf dQuantConfermata = 0 Then
        '  strStatoCorpo = "N"
        'End If

        'Aggiorna informazioni riga su Business
        oCldCnmg.AggiornaConfermaAdviceline(strDittaCorrente, strClientId, strAdviceId, strAdviceLineId, dQuantConfermata, strBaia)
      Next

      '--------------------------------------------------
      '--- Aggiorna informazioni su tabelle Business
      '--------------------------------------------------
      oCldCnmg.AggiornaConfermaAdvice(strDittaCorrente, strClientId, strAdviceId, strTextFile)

      '--------------------------------------------------
      '--- Restituisce il datarow dell'advice con le informazioni aggiorate 
      '--------------------------------------------------
      Return oCldCnmg.GetAdvice(strDittaCorrente, strClientId, strAdviceId)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Importazione delle conferme order dal server di AUTOSTORE
  ''' </summary>
  Public Overridable Function GetFileorder _
    (
    ByVal strClientId As String,
    ByVal strFile As String,
    ByRef strErrore As String
    ) As DataRow

    'Dim bEsito As Boolean = False
    Dim strStatoCorpo As String = ""
    Dim strStatoTesta As String = ""
    Dim xmldoc As New XmlDocument()
    Dim orderLinesConfirm As XmlNode
    Dim orderLineConfirm As XmlNodeList
    Dim inventoryCriteriaConfirm As XmlNode
    Dim strOrderId As String = ""
    Dim strOrderLineId As String = ""
    Dim dQuantInviata As Decimal = 0
    Dim dQuantConfermata As Decimal = 0
    Dim strTextFile As String = ""

    Try
      '--------------------------------------------------
      '--- Legge il file di orderconfirm
      '--------------------------------------------------
      xmldoc.Load(strFile)
      strOrderId = xmldoc.DocumentElement.SelectSingleNode("orderId").InnerText
      orderLinesConfirm = xmldoc.DocumentElement.SelectNodes("orderLinesConfirm")(0)
      orderLineConfirm = orderLinesConfirm.SelectNodes("orderLineConfirm")
      strTextFile = File.ReadAllText(strFile)

      '--------------------------------------------------
      '--- Verfica se esiste l'order nella tabelle Business
      '--------------------------------------------------
      If oCldCnmg.GetOrder(strDittaCorrente, strClientId, strOrderId) Is Nothing Then
        Return Nothing
      End If

      '--------------------------------------------------
      '--- Ciclo di riscontro quantità order
      '--------------------------------------------------
      For Each node As XmlNode In orderLinesConfirm
        strOrderLineId = NTSCStr(node.SelectSingleNode("orderLineId").InnerText).PadLeft(4, "0"c)
        inventoryCriteriaConfirm = node.SelectNodes("inventoryCriteriaConfirm")(0)
        dQuantInviata = NTSCDec(inventoryCriteriaConfirm.SelectSingleNode("quantityBaseTargetHost").InnerText)
        dQuantConfermata = NTSCDec(inventoryCriteriaConfirm.SelectSingleNode("quantityBaseCurrent").InnerText)

        'If dQuantInviata = dQuantConfermata Then
        '  strStatoCorpo = "C"
        'ElseIf dQuantConfermata = 0 Then
        '  strStatoCorpo = "N"
        'ElseIf dQuantInviata > dQuantConfermata Then
        '  strStatoCorpo = "P"
        'ElseIf dQuantInviata < dQuantConfermata Then
        '  strStatoCorpo = "P"
        'End If

        'Aggiorna informazioni riga su Business
        oCldCnmg.AggiornaConfermaOrderline(strDittaCorrente, strClientId, strOrderId, strOrderLineId, dQuantConfermata)
      Next

      '--------------------------------------------------
      '--- Aggiorna informazioni su tabelle Business
      '--------------------------------------------------
      oCldCnmg.AggiornaConfermaOrder(strDittaCorrente, strClientId, strOrderId, strTextFile)

      '--------------------------------------------------
      '--- Restituisce il datarow dell'order con le informazioni aggiorate 
      '--------------------------------------------------
      Return oCldCnmg.GetOrder(strDittaCorrente, strClientId, strOrderId)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  Public Overridable Sub moveFile _
    (
    ByVal drFile As DataRow,
    ByVal strDir As String
    )

    Dim strFile As String
    Dim strNomeFile As String

    Try
      If Not Directory.Exists(strDir + "\STORICO") Then
        MkDir(strDir + "\STORICO")
      End If

      strFile = NTSCStr(drFile!File)
      strNomeFile = NTSCStr(drFile!NomeFile)

      File.Copy(strFile, strDir + "\STORICO\" + strNomeFile, True)
      File.Delete(strFile)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Sposta File AUTOSTORE
  ''' </summary>
  Public Overridable Sub spostaFile _
    (
    ByVal strDitta As String,
    ByRef drFile As DataRow
    )

    Dim dsCnmg As New DataSet
    Dim strSQL As String = ""
    Dim dtrChiamata As DataRow = Nothing
    Dim strClientid As String = ""
    Dim strDir As String = ""
    Dim strCodart As String = ""
    Dim dsOut As New DataSet

    Try
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(strDitta, dsCnmg) Then
        'strErrore =
        '  "ATTENZIONE!" + vbCrLf +
        '  "Errore In apertura chiamate di collegamento Autostore"
        Return
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------

      'skuInsertUpdate
      If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(CONFIRM)).Length = 0 Then
        'strErrore =
        '    "ATTENZIONE!" + vbCrLf +
        '    "Chiamata di Inserimento/Modifica articoli Autostore non configurata"
        Return
      Else
        dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(CONFIRM))(0)
      End If

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      strDir = NTSCStr(dtrChiamata!ch_urlchia)
      strClientid = NTSCStr(dtrChiamata!ch_clientid)

      moveFile(drFile, strDir)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  ''' <summary>
  ''' Importazione dello stato dal server di AUTOSTORE
  ''' </summary>
  Public Overridable Function ImportaStato _
    (
    ByVal strDitta As String
    ) As DataSet

    Dim dsCnmg As New DataSet
    Dim strSQL As String = ""
    Dim dtrChiamata As DataRow = Nothing
    Dim strClientid As String = ""
    Dim strFile As String = ""
    Dim strCodart As String = ""
    Dim dsOut As New DataSet

    Try
      '-------------------------------------------------
      '--- Apre il Dataset delle chiamate configurate
      '-------------------------------------------------
      If Not Apri(strDitta, dsCnmg) Then
        Return Nothing
      End If

      '-------------------------------------------------
      '--- Esce se non ci sono chiamate configurate
      '-------------------------------------------------
      If dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(CONFIRM)).Length = 0 Then
        Return Nothing
      Else
        dtrChiamata = dsCnmg.Tables("CHIAMATE").Select("ch_codchia = " & NTSCInt(CONFIRM))(0)
      End If

      '-------------------------------------------------
      '--- Imposta dati per chiamata
      '-------------------------------------------------
      strFile = NTSCStr(dtrChiamata!ch_urlchia)
      strClientid = NTSCStr(dtrChiamata!ch_clientid)


      dsOut = GetStato(strClientid, strFile)

      '-------------------------------------------------
      '--- Valore funzione
      '-------------------------------------------------
      Return dsOut

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Crea il datatset dello stato Advice e Orders
  ''' </summary>
  Public Overridable Function GetStato _
    (
    ByVal strClientId As String,
    ByVal strFile As String
    ) As DataSet

    'Dim bEsito As Boolean = False
    Dim strStato As String = ""
    Dim dsOut As New DataSet
    Dim dttOrder As New DataTable
    Dim dttFile As New DataTable
    Dim dr As DataRow = Nothing
    Dim strErrore As String = ""

    Try
      '--------------------------------------------------
      '--- Verifica che esita la cartella delle riosposte
      '--------------------------------------------------
      If Not Directory.Exists(strFile) Then
        Return Nothing
      End If

      '--------------------------------------------------
      '--- Crea il datatabvle della ricezione risposte
      '--------------------------------------------------
      dttFile.Columns.Add("ID")
      dttFile.Columns.Add("Tipo")
      dttFile.Columns.Add("File")
      dttFile.Columns.Add("NomeFile")

      '--------------------------------------------------
      '--- Ciclo di lettura pickcomplete
      '--------------------------------------------------
      For Each filename As String In Directory.GetFiles(strFile, "*pickcomplete*")
        If filename.ToUpper.EndsWith(".TODO") Then
          Dim f As New FileInfo(filename)
          dr = GetFilepick(strClientId, filename)
          If Not dr Is Nothing Then
            If dttOrder.Rows.Count = 0 Then
              dttOrder = dr.Table.Copy
            Else
              dttOrder.ImportRow(dr)
            End If
            dttFile.Rows.Add(NTSCStr(dr!orderId), "Or", filename, f.Name)
          End If
        End If
      Next
      dttOrder.TableName = "ORDER"
      dttFile.TableName = "FILE"

      '--------------------------------------------------
      '--- Imposta Dataset
      '--------------------------------------------------
      dsOut.Tables.Add(dttOrder)
      dsOut.Tables.Add(dttFile)
      Return dsOut

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Importazione del pickcomplete order dal server di AUTOSTORE
  ''' </summary>
  Public Overridable Function GetFilepick _
    (
    ByVal strClientId As String,
    ByVal strFile As String
    ) As DataRow

    'Dim bEsito As Boolean = False
    Dim strStatoCorpo As String = ""
    Dim strStatoTesta As String = ""
    Dim xmldoc As New XmlDocument()
    Dim strOrderId As String = ""

    Try
      '--------------------------------------------------
      '--- Legge il file di pickcomplete
      '--------------------------------------------------
      xmldoc.Load(strFile)
      strOrderId = xmldoc.DocumentElement.SelectSingleNode("orderId").InnerText

      '--------------------------------------------------
      '--- Restituisce il datarow dell'order con le informazioni aggiorate 
      '--------------------------------------------------
      Return oCldCnmg.GetOrder(strDittaCorrente, strClientId, strOrderId)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      Return Nothing
    End Try
  End Function

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
    Try
      Return oCldCnmg.AggiornaStatoOrder(strDitta, strClientId, strorderId, strStato)
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

#End Region

  'AAAAAAAAAAAAAAAAAAAAAAAAAA

#Region "Funzioni e Routines di WMS AUTOSTORE"
  ''' <summary>
  ''' CUSTOM
  ''' Personalizzazione per WMS
  ''' Verifico la presenza di matricola e in caso la gestisco
  ''' </summary>
  Public Overridable Function AggiornaMatricola(ByVal dtrCurr As DataRow) As Boolean
    Try
      '--------------------------------------------------
      '--- Verifico la presenza di matricola e in caso la gestisco
      '--------------------------------------------------
      If NTSCStr(dtrCurr!ec_matric).Trim() <> "" Then
        oCleBoll.dttMOVMATR.Rows.Add()
        With oCleBoll.dttMOVMATR.Rows(oCleBoll.dttMOVMATR.Rows.Count - 1)
          !codditt = oCleBoll.dttEC.Rows(oCleBoll.dttEC.Rows.Count - 1) !codditt
          !mma_tipork = oCleBoll.dttEC.Rows(oCleBoll.dttEC.Rows.Count - 1) !ec_tipork
          !mma_serie = oCleBoll.dttEC.Rows(oCleBoll.dttEC.Rows.Count - 1) !ec_serie
          !mma_numdoc = oCleBoll.dttEC.Rows(oCleBoll.dttEC.Rows.Count - 1) !ec_numdoc
          !mma_anno = oCleBoll.dttEC.Rows(oCleBoll.dttEC.Rows.Count - 1) !ec_anno
          !mma_riga = oCleBoll.dttEC.Rows(oCleBoll.dttEC.Rows.Count - 1) !ec_riga
          !mma_rigaa = 1
          !mma_quant = oCleBoll.dttEC.Rows(oCleBoll.dttEC.Rows.Count - 1) !ec_quant
          !mma_matric = dtrCurr!ec_matric
          !mma_note = ""
        End With
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

  ''' <summary>
  ''' CUSTOM
  ''' Personalizzazione per WMS
  ''' Genera la BMI di trasferimento giacenze articoli da magazzino principale a magazzino AUTOSTORE
  ''' </summary>
  Public Overridable Function GeneraAdviceAutostore _
    (
    ByVal strDitta As String,
    ByVal nTipoRaggruppamentoRicm As Integer,
    ByVal dsDocRic As DataSet
    ) As Boolean

    'Datatable articoli AUTOSTORE
    Dim dttArt As New DataTable
    Dim dtrArt() As DataRow = Nothing
    'Variabili per inizializzazione BOLL Autostore
    Dim strErr As String = ""
    Dim oTmp As Object = Nothing
    'Variabili per generazione BMI Autostore 
    Dim strSelectRigheBMI As String = ""
    Dim strPrefissoLog As String = "AUTOSTORE".PadRight(12)
    Dim strMsg As String = ""
    Dim nTipoBF As Integer = 0
    Dim nCauCarVal As Integer = 0
    Dim nCauCarNoVal As Integer = 0
    Dim nCauSca As Integer = 0
    Dim nCriterioValorizz As Integer = 0
    Dim lConto As Integer = 0
    Dim strSerie As String = ""
    Dim strModello As String = ""
    Dim nMagazCaricoAutostore As Integer
    Dim strUbicazCaricoAutostore As String
    Dim lAnnoTco As Integer = 0
    Dim lStagTco As Integer = 0
    Dim bCausaleDoppia As Boolean
    Dim lNuma As Integer = 0
    Dim lNumaNew As Integer = 0
    Dim strTipoProg As String = ""
    Dim dttNuma As New DataTable
    Dim strRiferim As String = ""

    'Dataset BMI Autostore
    Dim dsBoll As New DataSet

    Try
      '--------------------------------------------------
      '--- Generazione Advice carico AUTOSTORE
      '--------------------------------------------------
      'Verifico se il documento contiene articoli AUTOSTORE
      dttArt.Clear()
      If Not oCldCnmg.VerificaAdviceAutostore(strDittaCorrente, dsDocRic, nTipoRaggruppamentoRicm, dttArt) Then
        LogWrite(oApp.Tr(Me, 128843579324644000, strPrefissoLog + " : ERRORE verifica articoli da elaborare"), False)
        Return False
      Else
        If dttArt.Rows.Count = 0 Then
          LogWrite(oApp.Tr(Me, 128843579324644000, strPrefissoLog + " : Nessun articolo da elaborare"), False)
          Return True
        Else
          'Inizializzo BEVEBOLL per creazione BMI Autostore
          LogWrite(oApp.Tr(Me, 128843579324644000, strPrefissoLog + " : Inizializzo Boll Autostore "), False)

          If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BEHHCNMG", "BEVEBOLL", oTmp, strErr, False, "", "") = False Then
            Throw New NTSException(oApp.Tr(Me, 128607611686875000, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
            Return False
          End If
          oCleBoll = CType(oTmp, CLEVEBOLL)

          AddHandler oCleBoll.RemoteEvent, AddressOf GestisciEventiEntityBoll
          If oCleBoll.Init(oApp, oScript, oCleComm, "", False, "", "") = False Then Return False
          If Not oCleBoll.InitExt() Then Return False
          oCleBoll.strProgChiamante = "BEHHCNMG"
          oCleBoll.bModuloCRM = False
          oCleBoll.bIsCRMUser = False

          'Carico le impostazioni di registro di BSHHCNMG
          nTipoBF = NTSCInt(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_TipoBollaFattura", "0", "", "0"))
          nCauCarVal = NTSCInt(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_CausaleCarVal", "0", "", "0"))
          nCauCarNoVal = NTSCInt(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_CausaleCarNoVal", "0", "", "0"))
          nCauSca = NTSCInt(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_CausaleScarico", "0", "", "0"))
          nCriterioValorizz = NTSCInt(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_CriterioValorizz", "0", "", "0"))
          lConto = NTSCInt(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_Conto", "0", "", "0"))
          strSerie = NTSCStr(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_Serie", " ", "", " "))
          strModello = NTSCStr(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_Modello", " ", "", " "))
          'Carico le impostazioni di magazzino BNHHAUTO
          'AAAAAAAAAAAAAAAAAAAAA VERIFCARE
          'CREARE BNHHAUTO
          nMagazCaricoAutostore = NTSCInt(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_MagazzinoCaricoWMSRic", "1004", "", "1004"))
          strUbicazCaricoAutostore = NTSCStr(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_UbicazioneCaricoWMSRic", "XX----------", "", "XX----------"))

          'Verifico che i dati siano stati impostati a registro
          If nTipoBF = 0 Then strMsg += "BSHHCNMG\OPZIONI\HH_TipoBollaFattura;" + vbCrLf
          If nCauCarNoVal = 0 Then strMsg += "BSHHCNMG\OPZIONI\HH_CausaleCarNoVal;" + vbCrLf
          If nCauSca = 0 Then strMsg += "BSHHCNMG\OPZIONI\HH_CausaleScarico;" + vbCrLf
          If lConto = 0 Then strMsg += "BSHHCNMG\OPZIONI\HH_Conto;"
          If oApp.oGvar.bModelliDoc AndAlso String.IsNullOrWhiteSpace(strModello) Then strMsg += vbCrLf + "BSHHCNMG\OPZIONI\HH_Modello;"
          If strMsg.Trim.Length > 0 Then
            strMsg =
              "Non tutti i valori di registro sono stati impostati correttamente impossibile continuare." + vbCrLf +
              "Valori mancanti:" + vbCrLf + strMsg
            ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 128820343370420029, strMsg)))
            Return False
          End If

          'Se le causali sono collegate allora genera una riga unica
          Dim dttCar, dttScar As New DataTable
          oCldCnmg.ValCodiceDb(nCauCarNoVal.ToString, strDittaCorrente, "TABCAUM", "N", , dttCar)
          oCldCnmg.ValCodiceDb(nCauSca.ToString, strDittaCorrente, "TABCAUM", "N", , dttScar)

          If dttCar.Rows.Count = 0 Then
            strMsg =
              "Il codice |" + nCauCarNoVal.ToString + "| non esiste nella tabella delle causali di magazzino impossibile continuare"
            ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 132452607880778052, strMsg)))
            Return False
          End If
          If dttScar.Rows.Count = 0 Then
            strMsg =
              "Il codice |" + nCauSca.ToString + "| non esiste nella tabella delle causali di magazzino impossibile continuare"
            ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 132452607880778053, strMsg)))
            Return False
          End If

          bCausaleDoppia = NTSCInt(dttCar.Rows(0) !tb_causec) = nCauSca AndAlso NTSCInt(dttScar.Rows(0) !tb_causec) = nCauCarNoVal

          'Carico la tabella delle personalizzazioni vendite
          Dim dttPeve As DataTable = New DataTable
          ocldBase.ValCodiceDb("1", oCleBoll.strDittaCorrente, "TABPEVE", "N", "", dttPeve)
          lAnnoTco = NTSCInt(dttPeve.Rows(0) !tb_annotco)
          lStagTco = NTSCInt(dttPeve.Rows(0) !tb_codstag)

          'Creo il documento su veboll
          If Not oCleBoll.ApriDoc(strDittaCorrente, True, "Z", Now.Year, strSerie, 0, dsBoll) Then
            LogWrite(oApp.Tr(Me, 128843579324644000, "- Errore in inizializzazione BMI Autostore"), False)
            Return False
          End If
          oCleBoll.bInApriDocSilent = True

          'Determino il numero del nuovo documento
          'In oCleBoll.NuovoDocumento se numero lNuma gia esitente da errore 
          strTipoProg = oCleBoll.TipoNumaDaTipoRk("Z")
          lNuma = oCldCnmg.LegNuma(strDittaCorrente, strTipoProg, strSerie, Now.Year, True)
          Do While oCleBoll.oCldBoll.EsisteDoc(strDittaCorrente, "Z", Now.Year, strSerie, lNuma, dttNuma)
            lNuma += 1 'oCldBoll.LegNuma(strDittaCorrente, "Z", strSerie, Now.Year, True)
          Loop

          'Inizializza il nuovo documento
          oCleBoll.ResetVar()
          oCleBoll.bControllaCommessaChiusa = False
          oCleBoll.bDisabilitaCheckDateAnteriori = True
          oCleBoll.bDisabilitaCheckAnnoData = True
          oCleBoll.strVisNoteConto = "N"
          oCleBoll.bDisabilitaMsgPrezzoZero = True
          oCleBoll.bCreaFilePick = False
          'Creo il nuovo documento
          If Not oCleBoll.NuovoDocumento(strDittaCorrente, "Z", Now.Year, strSerie, lNuma, strModello) Then
            LogWrite(oApp.Tr(Me, 128843579324644000, "- Errore in creazione BMI Autostore"), False)
            Return False
          End If
          oCleBoll.bInNuovoDocSilent = True
          oCleBoll.bDocEmesso = False        'Altrimenti da il messaggio di documento emesso a fornitore se (come deve essere) il conto è NS stabilimento
          oCleBoll.strTestEsist = "N"        'Evito che venga segnalato che l'articolo sta andando sotto esistenza. Sono in un picking, se l'articolo l'ho prelevato vuol dire che c'è.
          oCleBoll.bConsentiCreazDocumCliFornBloccoFisso = True

          'Azzero per sicurezza le righe
          oCleBoll.dttEC.Clear()

          'Preparo la testata
          With oCleBoll.dttET.Rows(0)
            !et_conto = lConto
            If Not oApp.oGvar.bModelliDoc Then!et_tipobf = nTipoBF
            !et_causale = nCauSca
            !et_magaz = dsDocRic.Tables("TESTA").Rows(0) !et_magaz
            !et_codstag = lStagTco
            !et_annotco = lAnnoTco
            !et_datdoc = Now
            !et_ultagg = Now
            !et_opnome = oApp.User.Nome
            'Mi salvo i riferimento del DDT ricevuto che ha generato la BMI Autostore
            strRiferim =
              "DDT : " + NTSCStr(dsDocRic.Tables("TESTA").Rows(0) !et_numdoc) + IIf(NTSCStr(dsDocRic.Tables("TESTA").Rows(0) !et_serie).Trim.Length = 0, "", NTSCStr(dsDocRic.Tables("TESTA").Rows(0) !et_serie)).ToString +
              " Forn. : " + NTSCStr(dsDocRic.Tables("TESTA").Rows(0) !xx_conto)
            If strRiferim.Length > 50 Then
              strRiferim = strRiferim.Substring(0, 50)
            End If
            !et_riferim = strRiferim.Trim
          End With

          'Righe
          For Each dtrCorpo As DataRow In dsDocRic.Tables("CORPO").Rows
            'Verifico se articolo per Autostore
            dtrArt = dttArt.Select("mm_riga = " + CStrSQL(dtrCorpo!ec_riga))
            If dtrArt.Length > 0 Then
              'Riga di scarico
              oCleBoll.dttEC.Rows.Add()
              With oCleBoll.dttEC.Rows(oCleBoll.dttEC.Rows.Count - 1)
                !codditt = "."
                !codditt = strDittaCorrente
                !ec_magaz = dtrCorpo!ec_magaz
                !ec_causale = nCauSca
                !ec_causale2 = 0
                !ec_matric = dtrCorpo!ec_matric
                !ec_codart = dtrCorpo!ec_codart
                !ec_fase = dtrCorpo!ec_fase
                !ec_unmis = dtrCorpo!ec_unmis
                !ec_colli = dtrCorpo!ec_colli
                !ec_quant = dtrCorpo!ec_quant
                !ec_commeca = dtrCorpo!ec_commeca
                !ec_lotto = dtrCorpo!ec_lotto
                !ec_ultagg = Now
                !ec_tctaglia = dtrCorpo!ec_tctaglia
                !ec_tcindtagl = dtrCorpo!ec_tcindtagl
                'Salvo i riferimenti del documento originario nei campi personalizzati
                !ec_hh_tipo = dtrCorpo!ec_tipork
                !ec_hh_anno = dtrCorpo!ec_anno
                !ec_hh_serie = dtrCorpo!ec_serie
                !ec_hh_num = dtrCorpo!ec_numdoc
                !ec_hh_riga = dtrCorpo!ec_riga
                !ec_hh_magriga = dtrCorpo!ec_magaz
                !ec_hh_commeca_ic = dtrCorpo!ec_commeca

                'Se causale doppia iomposta i relativi campi
                If bCausaleDoppia Then
                  !ec_magaz2 = nMagazCaricoAutostore
                  !ec_causale2 = nCauCarNoVal
                  !ec_commeca2 = dtrCorpo!ec_commeca
                  If NTSCStr(dtrArt(0) !ar_gesubic) = "S" Then!ec_ubicaz2 = strUbicazCaricoAutostore.PadRight(12, "-"c)
                End If

                If NTSCStr(dtrArt(0) !ar_gesubic) = "S" Then!ec_ubicaz = NTSCStr(dtrCorpo!ec_ubicaz).PadRight(12, "-"c)
              End With

              If Not AggiornaMatricola(dtrCorpo) Then
                LogWrite(oApp.Tr(Me, 128843579324644000, "- Riga Saltata. Errore in creazione matricola per l'articolo " + dtrCorpo!ec_codart.ToString), False)
                Return False
              End If

              'If Not AggiornaTaglia(dtrCorpo) Then Return False

              If Not bCausaleDoppia Then
                'Riga di carico
                oCleBoll.dttEC.Rows.Add()
                With oCleBoll.dttEC.Rows(oCleBoll.dttEC.Rows.Count - 1)
                  !codditt = "."
                  !codditt = strDittaCorrente
                  !ec_magaz = nMagazCaricoAutostore
                  !ec_causale = nCauCarNoVal
                  !ec_causale2 = 0
                  !ec_matric = dtrCorpo!ec_matric
                  !ec_codart = dtrCorpo!ec_codart
                  !ec_fase = dtrCorpo!ec_fase
                  !ec_unmis = dtrCorpo!ec_unmis
                  !ec_colli = dtrCorpo!ec_colli
                  !ec_quant = dtrCorpo!ec_quant
                  !ec_commeca = dtrCorpo!ec_commeca
                  !ec_lotto = dtrCorpo!ec_lotto
                  !ec_ultagg = Now
                  !ec_tctaglia = dtrCorpo!ec_tctaglia
                  !ec_tcindtagl = dtrCorpo!ec_tcindtagl

                  'If bGeneraPallet Then
                  '  dtrCorpo!xx_tipork = !ec_tipork
                  '  dtrCorpo!xx_anno = !ec_anno
                  '  dtrCorpo!xx_serie = !ec_serie
                  '  dtrCorpo!xx_numdoc = !ec_numdoc
                  '  dtrCorpo!xx_riga = !ec_riga
                  'End If

                  'Salvo i riferimenti del documento originario nei relativi campi personalizzati
                  !ec_hh_tipo = dtrCorpo!ec_tipork
                  !ec_hh_serie = dtrCorpo!ec_serie
                  !ec_hh_num = dtrCorpo!ec_numdoc
                  !ec_hh_anno = dtrCorpo!ec_anno
                  !ec_hh_riga = dtrCorpo!ec_riga
                  !ec_hh_magriga = dtrCorpo!ec_magaz
                  !ec_hh_commeca_ic = dtrCorpo!ec_commeca

                  If NTSCStr(dtrArt(0) !ar_gesubic) = "S" Then!ec_ubicaz = strUbicazCaricoAutostore.PadRight(12, "-"c)
                End With

                ' Il ciclo va fatto su MOVMATR del documento originario e non su movpic.
                ' quindi qui va messo un ciclo for.
                If Not AggiornaMatricola(dtrCorpo) Then
                  LogWrite(oApp.Tr(Me, 128843579324644000, "- Riga Saltata. Errore in creazione matricola per l'articolo : " + dtrCorpo!ec_codart.ToString), False)
                  Return False
                End If

                'If Not AggiornaTaglia(dtrCorpo) Then Return False
              End If
            End If
          Next

          'Controllo comunque se ci sono righe perché per assurdo potrei non aver trovato nessuna riga in dttArt.....
          If oCleBoll.dttEC.Rows.Count > 0 Then
            'Ricalcola i valori di riga
            For Each dtrEc As DataRow In oCleBoll.dttEC.Rows
              oCleBoll.SettaValoriRiga(dtrEc)
            Next

            'Prima di salvare il documento controllo se il numero è già stato usato
            'Nel caso cambio progressivo
Imposta_Numero_Doc:
            lNuma = NTSCInt(oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_numdoc)
            lNumaNew = lNuma
            Do While oCleBoll.oCldBoll.EsisteDoc _
              (
              strDittaCorrente,
              NTSCStr(oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_tipork),
              NTSCInt(oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_anno),
              NTSCStr(oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_serie),
              lNumaNew, dttNuma
              )

              'Determino nuovo numero documento e aggiorno la numerazione così nessuno la potrà riprendere
              lNumaNew = oCldCnmg.AggNuma _
                (
                strDittaCorrente, strTipoProg,
                NTSCStr(oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_serie),
                NTSCInt(oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_anno),
                NTSCInt(oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_numdoc),
                True, False, ""
                )
            Loop

            'Se cambiato aggiorno numero documento
            If lNumaNew <> lNuma Then
              oCleBoll.CambiaNumdoc _
                (
                oCleBoll.dsShared,
                oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_tipork.ToString,
                lNumaNew,
                NTSCStr(oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_serie),
                NTSCInt(oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_anno), False, String.Empty
                )
              'Segnala progressivo numerazione cambiato
              oCleBoll.bProgrCambiato = True
            End If

            'Salvo il documento
            oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_aspetto = ""
            If Not oCleBoll.SalvaDocumento("N") Then
              'Controllo cambio numero documento
              If NTSCStr(oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_aspetto) = "#CAMBIONUMDOC#" Then
                oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_aspetto = ""
                LogWrite(oApp.Tr(Me, 128843579324644000, "CAMBIO NUMERO DOCUMENTO " & oCleBoll.dsShared.Tables("TESTA").Rows(0) !et_numdoc.ToString), False)
                GoTo Imposta_Numero_Doc
              Else
                ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_ERROR, oApp.Tr(Me, 129175453922343750, "BMI Autostore non salvata")))
                LogWrite(oApp.Tr(Me, 128843579324644000, "- BMI Autostore non salvata"), False)
              End If
              'Valore funzione
              Return False
            End If

            With oCleBoll.dsShared
              strMsg =
                "Creata BMI Autostore " + NTSCStr(.Tables("TESTA").Rows(0) !et_numdoc) + IIf(NTSCStr(.Tables("TESTA").Rows(0) !et_serie).Trim.Length = 0, "", "/" + NTSCStr(.Tables("TESTA").Rows(0) !et_serie)).ToString
            End With
            ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.MSG_INFO, oApp.Tr(Me, 129175453922343750, strMsg)))
            LogWrite(oApp.Tr(Me, 129175453922343750, "- " + strMsg), False)

          End If
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
    End Try
  End Function

  ''' <summary>
  ''' CUSTOM
  ''' Personalizzazione per WMS
  ''' Genera Order per Picking merce da magazzino AUTOSTORE
  ''' </summary>
  Public Overridable Function GeneraOrderAutostore _
    (
    ByVal strDitta As String,
    ByVal strBusTipo As String,
    ByVal dsDocOrd As DataSet,
    ByVal nPriorita As Integer,
    ByRef strErrore As String
   ) As Boolean

    Dim nMagazPrelievoAutostore As Integer

    Try
      '--------------------------------------------------
      '--- Generazione Order prelievo AUTOSTORE
      '--------------------------------------------------
      'Legge magazzino per Prelievo Merce
      nMagazPrelievoAutostore = NTSCInt(oCldCnmg.GetSettingBus("BSHHCNMG", "OPZIONI", ".", "HH_MagazzinoPrelievoWMS", "1003", "", "1003"))

      'Verifico se il documento contiene articoli AUTOSTORE
      If Not InviaOrder(strDittaCorrente, False, strBusTipo, nMagazPrelievoAutostore, dsDocOrd, nPriorita, strErrore) Then
        Return False
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
  'AAAAAAAAAAAAAAAAAAAAAAAAA

#Region "Eventi AfterColUpdate e BeforeColUpdate"
  Public Overrides Sub AfterColUpdate(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Try
      '--------------------------------------------------
      '--- Esce se non abilitata Validazione
      '--------------------------------------------------
      If lCodconn = 0 Then Return
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
      Dim strFunction As String = "AfterColUpdate_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overrides Sub BeforeColUpdate(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Dim strErr As String = ""
    Try
      '--------------------------------------------------
      '--- Esce se non abilitata Validazione
      '--------------------------------------------------
      If lCodconn = 0 Then Return
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
      Dim strFunction As String = "BeforeColUpdate_" & e.Column.ColumnName.ToLower
      Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
      If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub BeforeColUpdate_ch_codchia(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Dim dtrTmp() As DataRow
    Dim strErrore As String = ""

    Try
      '--------------------------------------------------
      '--- Verifica codice 0
      '--------------------------------------------------
      If e.ProposedValue.ToString = "0" Then
        ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 127791222113437520, "Codice connessione 0 non consentito:   " & vbNewLine & "Inserire un nuovo codice connessione")))
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.SETFOCUS, "edch_codchia"))
        Return
      End If

      '--------------------------------------------------
      '--- Verifica codice già esistente
      '--------------------------------------------------
      If dsShared.Tables("CHIAMATE").Rows.Count > 1 Then
        dtrTmp = dsShared.Tables("CHIAMATE").Select("ch_codchia = " & e.ProposedValue.ToString())
        If dtrTmp.Length > 0 Then
          ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 127791222113437500, "Codice chiamata già esistente:" & vbNewLine & "Inserire un nuovo codice chiamata")))
          ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.SETFOCUS, "edch_codchia"))
          e.ProposedValue = e.Row(e.Column.ColumnName)
          Return
        End If
      End If

      '--------------------------------------------------
      '--- Imposta codice connessione
      '--------------------------------------------------
      lCodconn = NTSCInt(e.ProposedValue)

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub BeforeColUpdate_ch_deschia(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
    Try
      '--------------------------------------------------
      '--- Verifica descrizione chiamata
      '--------------------------------------------------
      If e.ProposedValue.ToString.Trim.Length.Equals(0) Then
        ThrowRemoteEvent(New NTSEventArgs("", oApp.Tr(Me, 128571303239218321, "Attenzione!" & vbNewLine & "Descrizione connessione non specificata, verificare impostazione connessione")))
        ThrowRemoteEvent(New NTSEventArgs(CLN__STD.ThMsg.SETFOCUS, "edch_deschia"))
      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub
#End Region

End Class