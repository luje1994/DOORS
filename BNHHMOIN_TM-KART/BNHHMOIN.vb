Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System.IO
Imports System.ComponentModel

Public Class FRMHHMOIN

  Public oCleMoin As CLEHHMOIN
  Public oCallParams As CLE__CLDP

  Public dsMoin As DataSet
  Public dcMoin As BindingSource = New BindingSource

  Public dtOpz As New DataTable

#Region "Dichiarazione Controlli"
  Private components As System.ComponentModel.IContainer
  Public WithEvents NtsBarManager1 As NTSInformatica.NTSBarManager
  Public WithEvents tlbMain As NTSInformatica.NTSBar
  Public WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Public WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Public WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Public WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Public WithEvents tlbGuida As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbEsci As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbRicaricaGriglia As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbCambioDitta As NTSInformatica.NTSBarButtonItem
#End Region

  Private Moduli_P As Integer = bsModAll
  Private ModuliExt_P As Integer = bsModAll
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

#Region "Inizializzazione"

  Public Overloads Function Init(ByRef Menu As CLE__MENU, ByRef Param As CLE__CLDP, Optional ByVal Ditta As String = "", Optional ByRef SharedControls As CLE__EVNT = Nothing) As Boolean
    '----------------------------------------------------------------------------------------------------------------
    oMenu = Menu
    oApp = oMenu.App
    oCallParams = Param
    '----------------------------------------------------------------------------------------------------------------
    If Ditta <> "" Then DittaCorrente = Ditta Else DittaCorrente = oApp.Ditta
    '----------------------------------------------------------------------------------------------------------------
    Me.GctlTipoDoc = ""
    '----------------------------------------------------------------------------------------------------------------
    InitializeComponent()
    '----------------------------------------------------------------------------------------------------------------
    Dim strErr As String = ""
    Dim oTmp As Object = Nothing
    If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNHHMOIN", "BEHHMOIN", oTmp, strErr, False, "", "") = False Then
      oApp.MsgBoxErr(oApp.Tr(Me, 128423763770716000, "ERRORE in fase di creazione Entity:" & vbCrLf & strErr))
      Return False
    End If
    oCleMoin = CType(oTmp, CLEHHMOIN)

    '--------------------------------------------------
    '--- Aggiunge gestore eventi
    '--------------------------------------------------
    AddHandler oCleMoin.RemoteEvent, AddressOf GestisciEventiEntity

    If oCleMoin.Init(oApp, NTSScript, oMenu.oCleComm, "", False, "", "") = False Then Return False

    'If Not oCleMaav.CaricaConnessioniDB() Then
    '  'oApp.MsgBoxErr(oApp.Tr(Me, 128423763770716000, "ERRORE Non è stato configurato correttamente la connessione di TRUMPF." & vbCrLf & "Chiudo il programma"))
    '  MsgBox("ERRORE Non è stato configurato correttamente la connessione di TRUMPF." & vbCrLf & "Chiudo il programma", MsgBoxStyle.Critical, "Bussines Cube")
    '  Return False
    'End If

    Return True
    '----------------------------------------------------------------------------------------------------------------
  End Function

  Public Overridable Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMHHMOIN))
    Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
    Me.tlbMain = New NTSInformatica.NTSBar()
    Me.tlbElabora = New NTSInformatica.NTSBarButtonItem()
    Me.tlbScarica = New NTSInformatica.NTSBarButtonItem()
    Me.tlbCerca = New NTSInformatica.NTSBarButtonItem()
    Me.tlbCancellaRiga = New NTSInformatica.NTSBarButtonItem()
    Me.tlbStrumenti = New NTSInformatica.NTSBarSubItem()
    Me.tlbSeleziona = New NTSInformatica.NTSBarButtonItem()
    Me.tlbDeseleziona = New NTSInformatica.NTSBarButtonItem()
    Me.tlbFileRettificaGiacenze = New NTSInformatica.NTSBarMenuItem()
    Me.tlbConfrontaGiacenze = New NTSInformatica.NTSBarMenuItem()
    Me.tlbEsci = New NTSInformatica.NTSBarButtonItem()
    Me.tlbZoom = New NTSInformatica.NTSBarButtonItem()
    Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
    Me.tlbGuida = New NTSInformatica.NTSBarButtonItem()
    Me.tlbRicaricaGriglia = New NTSInformatica.NTSBarButtonItem()
    Me.tlbCambioDitta = New NTSInformatica.NTSBarButtonItem()
    Me.tlbConverti = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarButtonItem1 = New NTSInformatica.NTSBarButtonItem()
    Me.tlbLavorati = New NTSInformatica.NTSBarButtonItem()
    Me.fmFiltri = New NTSInformatica.NTSGroupBox()
    Me.edDataADoc = New NTSInformatica.NTSTextBoxData()
    Me.lbDataDaDataADoc = New NTSInformatica.NTSLabel()
    Me.edDataDaDoc = New NTSInformatica.NTSTextBoxData()
    Me.edArticolo = New NTSInformatica.NTSTextBoxStr()
    Me.lbNomeArticolo = New NTSInformatica.NTSLabel()
    Me.lbArticolo = New NTSInformatica.NTSLabel()
    Me.lbNomeConto = New NTSInformatica.NTSLabel()
    Me.edConto = New NTSInformatica.NTSTextBoxNum()
    Me.lbConto = New NTSInformatica.NTSLabel()
    Me.edSerie = New NTSInformatica.NTSTextBoxStr()
    Me.lbAnnoSerieNum = New NTSInformatica.NTSLabel()
    Me.edNumero = New NTSInformatica.NTSTextBoxNum()
    Me.edAnno = New NTSInformatica.NTSTextBoxNum()
    Me.cbTipork = New NTSInformatica.NTSComboBox()
    Me.lbTipork = New NTSInformatica.NTSLabel()
    Me.edDataAJob = New NTSInformatica.NTSTextBoxData()
    Me.lbDataDaDataAJob = New NTSInformatica.NTSLabel()
    Me.edDataDaJob = New NTSInformatica.NTSTextBoxData()
    Me.pbElabora1 = New NTSInformatica.NTSProgressBar()
    Me.NtsPanel3 = New NTSInformatica.NTSPanel()
    Me.pbElabora = New NTSInformatica.NTSProgressBar()
    Me.lbElabora = New NTSInformatica.NTSLabel()
    Me.NtsGridView1 = New NTSInformatica.NTSGridView()
    Me.NtsGridView2 = New NTSInformatica.NTSGridView()
    Me.NtsGridView3 = New NTSInformatica.NTSGridView()
    Me.NtsGridView4 = New NTSInformatica.NTSGridView()
    Me.NtsGridView5 = New NTSInformatica.NTSGridView()
    Me.fmCorpo = New NTSInformatica.NTSGroupBox()
    Me.grCorpo = New NTSInformatica.NTSGrid()
    Me.grvCorpo = New NTSInformatica.NTSGridView()
    Me.idriga = New NTSInformatica.NTSGridColumn()
    Me.articleNumber = New NTSInformatica.NTSGridColumn()
    Me.ar_descr = New NTSInformatica.NTSGridColumn()
    Me.ar_unmis = New NTSInformatica.NTSGridColumn()
    Me.operation = New NTSInformatica.NTSGridColumn()
    Me.nominalQuantity = New NTSInformatica.NTSGridColumn()
    Me.actualQuantity = New NTSInformatica.NTSGridColumn()
    Me.containerSize = New NTSInformatica.NTSGridColumn()
    Me.positionStatus = New NTSInformatica.NTSGridColumn()
    Me.NtsSplitter1 = New NTSInformatica.NTSSplitter()
    Me.fmTesta = New NTSInformatica.NTSGroupBox()
    Me.grTesta = New NTSInformatica.NTSGrid()
    Me.grvTesta = New NTSInformatica.NTSGridView()
    Me.xx_sel = New NTSInformatica.NTSGridColumn()
    Me.id = New NTSInformatica.NTSGridColumn()
    Me.jobNumber = New NTSInformatica.NTSGridColumn()
    Me.busTipork = New NTSInformatica.NTSGridColumn()
    Me.busAnno = New NTSInformatica.NTSGridColumn()
    Me.busSerie = New NTSInformatica.NTSGridColumn()
    Me.busNumero = New NTSInformatica.NTSGridColumn()
    Me.tm_datdoc = New NTSInformatica.NTSGridColumn()
    Me.tm_conto = New NTSInformatica.NTSGridColumn()
    Me.xx_descrConto = New NTSInformatica.NTSGridColumn()
    Me.jobPriority = New NTSInformatica.NTSGridColumn()
    Me.jobDataOra = New NTSInformatica.NTSGridColumn()
    Me.jobStatus = New NTSInformatica.NTSGridColumn()
    Me.timerElabora = New System.Windows.Forms.Timer()
    Me.timerGiacenze = New System.Windows.Forms.Timer()
    Me.taskIcon = New System.Windows.Forms.NotifyIcon()
    Me.cmTask = New System.Windows.Forms.ContextMenuStrip()
    Me.cmTaskChiudi = New System.Windows.Forms.ToolStripMenuItem()
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fmFiltri, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmFiltri.SuspendLayout()
    CType(Me.edDataADoc.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edDataDaDoc.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edArticolo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edConto.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edSerie.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edNumero.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edAnno.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.cbTipork.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edDataAJob.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edDataDaJob.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pbElabora1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.NtsPanel3.SuspendLayout()
    CType(Me.pbElabora.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsGridView4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsGridView5, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fmCorpo, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmCorpo.SuspendLayout()
    CType(Me.grCorpo, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grvCorpo, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fmTesta, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmTesta.SuspendLayout()
    CType(Me.grTesta, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grvTesta, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.cmTask.SuspendLayout()
    Me.SuspendLayout()
    '
    'NtsBarManager1
    '
    Me.NtsBarManager1.AllowCustomization = False
    Me.NtsBarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.tlbMain})
    Me.NtsBarManager1.DockControls.Add(Me.barDockControlTop)
    Me.NtsBarManager1.DockControls.Add(Me.barDockControlBottom)
    Me.NtsBarManager1.DockControls.Add(Me.barDockControlLeft)
    Me.NtsBarManager1.DockControls.Add(Me.barDockControlRight)
    Me.NtsBarManager1.Form = Me
    Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbGuida, Me.tlbEsci, Me.tlbRicaricaGriglia, Me.tlbCambioDitta, Me.tlbConverti, Me.NtsBarButtonItem1, Me.tlbStrumenti, Me.tlbSeleziona, Me.tlbDeseleziona, Me.tlbZoom, Me.tlbLavorati, Me.tlbScarica, Me.tlbCerca, Me.tlbCancellaRiga, Me.tlbElabora, Me.tlbFileRettificaGiacenze, Me.tlbConfrontaGiacenze})
    Me.NtsBarManager1.MaxItemId = 51
    '
    'tlbMain
    '
    Me.tlbMain.BarName = "tlbMain"
    Me.tlbMain.DockCol = 0
    Me.tlbMain.DockRow = 0
    Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
    Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbElabora, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbScarica, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCerca, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCancellaRiga, True), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, Me.tlbStrumenti, "", True, True, True, 0, Nothing, DevExpress.XtraBars.BarItemPaintStyle.CaptionInMenu), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbZoom)})
    Me.tlbMain.OptionsBar.AllowQuickCustomization = False
    Me.tlbMain.OptionsBar.DisableClose = True
    Me.tlbMain.OptionsBar.DrawDragBorder = False
    Me.tlbMain.OptionsBar.UseWholeRow = True
    Me.tlbMain.Text = "tlbMain"
    '
    'tlbElabora
    '
    Me.tlbElabora.Caption = "Elabora"
    Me.tlbElabora.Id = 48
    Me.tlbElabora.Name = "tlbElabora"
    Me.tlbElabora.Visible = True
    '
    'tlbScarica
    '
    Me.tlbScarica.Caption = "Scarica"
    Me.tlbScarica.Id = 44
    Me.tlbScarica.Name = "tlbScarica"
    Me.tlbScarica.Visible = True
    '
    'tlbCerca
    '
    Me.tlbCerca.Caption = "Cerca"
    Me.tlbCerca.Id = 46
    Me.tlbCerca.Name = "tlbCerca"
    Me.tlbCerca.Visible = True
    '
    'tlbCancellaRiga
    '
    Me.tlbCancellaRiga.Caption = "Cancella Riga"
    Me.tlbCancellaRiga.Id = 47
    Me.tlbCancellaRiga.Name = "tlbCancellaRiga"
    Me.tlbCancellaRiga.Visible = True
    '
    'tlbStrumenti
    '
    Me.tlbStrumenti.Caption = "Strumenti"
    Me.tlbStrumenti.Glyph = CType(resources.GetObject("tlbStrumenti.Glyph"), System.Drawing.Image)
    Me.tlbStrumenti.Id = 37
    Me.tlbStrumenti.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbSeleziona), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbDeseleziona), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbFileRettificaGiacenze), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbConfrontaGiacenze)})
    Me.tlbStrumenti.Name = "tlbStrumenti"
    Me.tlbStrumenti.Visible = True
    '
    'tlbSeleziona
    '
    Me.tlbSeleziona.Caption = "Seleziona"
    Me.tlbSeleziona.Glyph = CType(resources.GetObject("tlbSeleziona.Glyph"), System.Drawing.Image)
    Me.tlbSeleziona.Id = 38
    Me.tlbSeleziona.Name = "tlbSeleziona"
    Me.tlbSeleziona.Visible = True
    '
    'tlbDeseleziona
    '
    Me.tlbDeseleziona.Caption = "Deseleziona"
    Me.tlbDeseleziona.Glyph = CType(resources.GetObject("tlbDeseleziona.Glyph"), System.Drawing.Image)
    Me.tlbDeseleziona.Id = 39
    Me.tlbDeseleziona.Name = "tlbDeseleziona"
    Me.tlbDeseleziona.Visible = True
    '
    'tlbFileRettificaGiacenze
    '
    Me.tlbFileRettificaGiacenze.Caption = "File Rettifica Giacenze"
    Me.tlbFileRettificaGiacenze.Id = 49
    Me.tlbFileRettificaGiacenze.Name = "tlbFileRettificaGiacenze"
    Me.tlbFileRettificaGiacenze.NTSIsCheckBox = False
    Me.tlbFileRettificaGiacenze.Visible = True
    '
    'tlbConfrontaGiacenze
    '
    Me.tlbConfrontaGiacenze.Caption = "Confronta Giacenze"
    Me.tlbConfrontaGiacenze.Id = 50
    Me.tlbConfrontaGiacenze.Name = "tlbConfrontaGiacenze"
    Me.tlbConfrontaGiacenze.NTSIsCheckBox = False
    Me.tlbConfrontaGiacenze.Visible = True
    '
    'tlbEsci
    '
    Me.tlbEsci.Caption = "Esci"
    Me.tlbEsci.Glyph = CType(resources.GetObject("tlbEsci.Glyph"), System.Drawing.Image)
    Me.tlbEsci.Id = 19
    Me.tlbEsci.Name = "tlbEsci"
    Me.tlbEsci.Visible = True
    '
    'tlbZoom
    '
    Me.tlbZoom.Caption = "Zoom"
    Me.tlbZoom.Id = 40
    Me.tlbZoom.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
    Me.tlbZoom.Name = "tlbZoom"
    Me.tlbZoom.Visible = True
    '
    'barDockControlTop
    '
    Me.barDockControlTop.CausesValidation = False
    Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlTop.Size = New System.Drawing.Size(684, 35)
    '
    'barDockControlBottom
    '
    Me.barDockControlBottom.CausesValidation = False
    Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.barDockControlBottom.Location = New System.Drawing.Point(0, 476)
    Me.barDockControlBottom.Size = New System.Drawing.Size(684, 0)
    '
    'barDockControlLeft
    '
    Me.barDockControlLeft.CausesValidation = False
    Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
    Me.barDockControlLeft.Location = New System.Drawing.Point(0, 35)
    Me.barDockControlLeft.Size = New System.Drawing.Size(0, 441)
    '
    'barDockControlRight
    '
    Me.barDockControlRight.CausesValidation = False
    Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
    Me.barDockControlRight.Location = New System.Drawing.Point(684, 35)
    Me.barDockControlRight.Size = New System.Drawing.Size(0, 441)
    '
    'tlbGuida
    '
    Me.tlbGuida.Caption = "Guida"
    Me.tlbGuida.Glyph = CType(resources.GetObject("tlbGuida.Glyph"), System.Drawing.Image)
    Me.tlbGuida.Id = 18
    Me.tlbGuida.Name = "tlbGuida"
    Me.tlbGuida.Visible = True
    '
    'tlbRicaricaGriglia
    '
    Me.tlbRicaricaGriglia.Caption = "Ricarica Griglia"
    Me.tlbRicaricaGriglia.Glyph = CType(resources.GetObject("tlbRicaricaGriglia.Glyph"), System.Drawing.Image)
    Me.tlbRicaricaGriglia.Id = 20
    Me.tlbRicaricaGriglia.Name = "tlbRicaricaGriglia"
    Me.tlbRicaricaGriglia.Visible = True
    '
    'tlbCambioDitta
    '
    Me.tlbCambioDitta.Caption = "Cambio Ditta"
    Me.tlbCambioDitta.Id = 25
    Me.tlbCambioDitta.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D))
    Me.tlbCambioDitta.Name = "tlbCambioDitta"
    Me.tlbCambioDitta.Visible = True
    '
    'tlbConverti
    '
    Me.tlbConverti.Caption = "Coverti da ""Definitivi"" in ""Provvisori"""
    Me.tlbConverti.Id = 26
    Me.tlbConverti.ItemShortcut = New DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
                Or System.Windows.Forms.Keys.F7))
    Me.tlbConverti.Name = "tlbConverti"
    Me.tlbConverti.NTSIsCheckBox = False
    Me.tlbConverti.Visible = True
    '
    'NtsBarButtonItem1
    '
    Me.NtsBarButtonItem1.Caption = "Seleziona DDT"
    Me.NtsBarButtonItem1.Id = 27
    Me.NtsBarButtonItem1.Name = "NtsBarButtonItem1"
    Me.NtsBarButtonItem1.Visible = True
    '
    'tlbLavorati
    '
    Me.tlbLavorati.Caption = "Lavorati"
    Me.tlbLavorati.Glyph = Global.NTSInformatica.My.Resources.Resources.Doc_3
    Me.tlbLavorati.Id = 41
    Me.tlbLavorati.Name = "tlbLavorati"
    Me.tlbLavorati.Visible = True
    '
    'fmFiltri
    '
    Me.fmFiltri.AllowDrop = True
    Me.fmFiltri.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmFiltri.Appearance.Options.UseBackColor = True
    Me.fmFiltri.Controls.Add(Me.edDataADoc)
    Me.fmFiltri.Controls.Add(Me.lbDataDaDataADoc)
    Me.fmFiltri.Controls.Add(Me.edDataDaDoc)
    Me.fmFiltri.Controls.Add(Me.edArticolo)
    Me.fmFiltri.Controls.Add(Me.lbNomeArticolo)
    Me.fmFiltri.Controls.Add(Me.lbArticolo)
    Me.fmFiltri.Controls.Add(Me.lbNomeConto)
    Me.fmFiltri.Controls.Add(Me.edConto)
    Me.fmFiltri.Controls.Add(Me.lbConto)
    Me.fmFiltri.Controls.Add(Me.edSerie)
    Me.fmFiltri.Controls.Add(Me.lbAnnoSerieNum)
    Me.fmFiltri.Controls.Add(Me.edNumero)
    Me.fmFiltri.Controls.Add(Me.edAnno)
    Me.fmFiltri.Controls.Add(Me.cbTipork)
    Me.fmFiltri.Controls.Add(Me.lbTipork)
    Me.fmFiltri.Controls.Add(Me.edDataAJob)
    Me.fmFiltri.Controls.Add(Me.lbDataDaDataAJob)
    Me.fmFiltri.Controls.Add(Me.edDataDaJob)
    Me.fmFiltri.Dock = System.Windows.Forms.DockStyle.Top
    Me.fmFiltri.Location = New System.Drawing.Point(0, 35)
    Me.fmFiltri.Name = "fmFiltri"
    Me.fmFiltri.Size = New System.Drawing.Size(684, 168)
    Me.fmFiltri.Text = "FILTRI"
    '
    'edDataADoc
    '
    Me.edDataADoc.EditValue = ""
    Me.edDataADoc.Location = New System.Drawing.Point(268, 72)
    Me.edDataADoc.Name = "edDataADoc"
    Me.edDataADoc.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edDataADoc.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edDataADoc.Properties.AutoHeight = False
    Me.edDataADoc.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edDataADoc.Properties.MaxLength = 65536
    Me.edDataADoc.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edDataADoc.Size = New System.Drawing.Size(112, 20)
    '
    'lbDataDaDataADoc
    '
    Me.lbDataDaDataADoc.BackColor = System.Drawing.Color.Transparent
    Me.lbDataDaDataADoc.Location = New System.Drawing.Point(4, 72)
    Me.lbDataDaDataADoc.Name = "lbDataDaDataADoc"
    Me.lbDataDaDataADoc.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbDataDaDataADoc.Size = New System.Drawing.Size(140, 20)
    Me.lbDataDaDataADoc.Text = "dalla Data alla Data Doc."
    Me.lbDataDaDataADoc.UseMnemonic = False
    '
    'edDataDaDoc
    '
    Me.edDataDaDoc.EditValue = ""
    Me.edDataDaDoc.Location = New System.Drawing.Point(148, 72)
    Me.edDataDaDoc.Name = "edDataDaDoc"
    Me.edDataDaDoc.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edDataDaDoc.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edDataDaDoc.Properties.AutoHeight = False
    Me.edDataDaDoc.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edDataDaDoc.Properties.MaxLength = 65536
    Me.edDataDaDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edDataDaDoc.Size = New System.Drawing.Size(112, 20)
    '
    'edArticolo
    '
    Me.edArticolo.EditValue = ""
    Me.edArticolo.Location = New System.Drawing.Point(148, 144)
    Me.edArticolo.Name = "edArticolo"
    Me.edArticolo.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edArticolo.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edArticolo.Properties.AutoHeight = False
    Me.edArticolo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edArticolo.Properties.MaxLength = 65536
    Me.edArticolo.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edArticolo.Size = New System.Drawing.Size(124, 20)
    '
    'lbNomeArticolo
    '
    Me.lbNomeArticolo.AutoEllipsis = True
    Me.lbNomeArticolo.BackColor = System.Drawing.Color.Transparent
    Me.lbNomeArticolo.Location = New System.Drawing.Point(276, 144)
    Me.lbNomeArticolo.Name = "lbNomeArticolo"
    Me.lbNomeArticolo.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.lbNomeArticolo.Size = New System.Drawing.Size(284, 20)
    Me.lbNomeArticolo.UseMnemonic = False
    '
    'lbArticolo
    '
    Me.lbArticolo.BackColor = System.Drawing.Color.Transparent
    Me.lbArticolo.Location = New System.Drawing.Point(4, 144)
    Me.lbArticolo.Name = "lbArticolo"
    Me.lbArticolo.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbArticolo.Size = New System.Drawing.Size(140, 20)
    Me.lbArticolo.Text = "Articolo"
    Me.lbArticolo.UseMnemonic = False
    '
    'lbNomeConto
    '
    Me.lbNomeConto.AutoEllipsis = True
    Me.lbNomeConto.BackColor = System.Drawing.Color.Transparent
    Me.lbNomeConto.Location = New System.Drawing.Point(248, 120)
    Me.lbNomeConto.Name = "lbNomeConto"
    Me.lbNomeConto.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.lbNomeConto.Size = New System.Drawing.Size(312, 20)
    Me.lbNomeConto.UseMnemonic = False
    '
    'edConto
    '
    Me.edConto.EditValue = "0"
    Me.edConto.Location = New System.Drawing.Point(148, 120)
    Me.edConto.Name = "edConto"
    Me.edConto.Properties.Appearance.Options.UseTextOptions = True
    Me.edConto.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edConto.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edConto.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edConto.Properties.AutoHeight = False
    Me.edConto.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edConto.Properties.MaxLength = 65536
    Me.edConto.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edConto.Size = New System.Drawing.Size(96, 20)
    '
    'lbConto
    '
    Me.lbConto.BackColor = System.Drawing.Color.Transparent
    Me.lbConto.Location = New System.Drawing.Point(4, 120)
    Me.lbConto.Name = "lbConto"
    Me.lbConto.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbConto.Size = New System.Drawing.Size(140, 20)
    Me.lbConto.Text = "Conto"
    Me.lbConto.UseMnemonic = False
    '
    'edSerie
    '
    Me.edSerie.EditValue = ""
    Me.edSerie.Location = New System.Drawing.Point(228, 48)
    Me.edSerie.Name = "edSerie"
    Me.edSerie.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edSerie.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edSerie.Properties.AutoHeight = False
    Me.edSerie.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edSerie.Properties.MaxLength = 65536
    Me.edSerie.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edSerie.Size = New System.Drawing.Size(60, 20)
    '
    'lbAnnoSerieNum
    '
    Me.lbAnnoSerieNum.BackColor = System.Drawing.Color.Transparent
    Me.lbAnnoSerieNum.Location = New System.Drawing.Point(4, 48)
    Me.lbAnnoSerieNum.Name = "lbAnnoSerieNum"
    Me.lbAnnoSerieNum.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbAnnoSerieNum.Size = New System.Drawing.Size(140, 20)
    Me.lbAnnoSerieNum.Text = "Anno/Serie/Num Doc."
    Me.lbAnnoSerieNum.UseMnemonic = False
    '
    'edNumero
    '
    Me.edNumero.EditValue = "0"
    Me.edNumero.Location = New System.Drawing.Point(292, 48)
    Me.edNumero.Name = "edNumero"
    Me.edNumero.Properties.Appearance.Options.UseTextOptions = True
    Me.edNumero.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edNumero.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edNumero.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edNumero.Properties.AutoHeight = False
    Me.edNumero.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edNumero.Properties.MaxLength = 65536
    Me.edNumero.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edNumero.Size = New System.Drawing.Size(88, 20)
    '
    'edAnno
    '
    Me.edAnno.EditValue = "0"
    Me.edAnno.Location = New System.Drawing.Point(148, 48)
    Me.edAnno.Name = "edAnno"
    Me.edAnno.Properties.Appearance.Options.UseTextOptions = True
    Me.edAnno.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edAnno.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edAnno.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edAnno.Properties.AutoHeight = False
    Me.edAnno.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edAnno.Properties.MaxLength = 65536
    Me.edAnno.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edAnno.Size = New System.Drawing.Size(76, 20)
    '
    'cbTipork
    '
    Me.cbTipork.EditValue = ""
    Me.cbTipork.Location = New System.Drawing.Point(148, 24)
    Me.cbTipork.Name = "cbTipork"
    Me.cbTipork.Properties.AutoHeight = False
    Me.cbTipork.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.cbTipork.Properties.DropDownRows = 30
    Me.cbTipork.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
    Me.cbTipork.Size = New System.Drawing.Size(232, 20)
    '
    'lbTipork
    '
    Me.lbTipork.BackColor = System.Drawing.Color.Transparent
    Me.lbTipork.Location = New System.Drawing.Point(4, 24)
    Me.lbTipork.Name = "lbTipork"
    Me.lbTipork.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTipork.Size = New System.Drawing.Size(140, 20)
    Me.lbTipork.Text = "Tipo Doc."
    Me.lbTipork.UseMnemonic = False
    '
    'edDataAJob
    '
    Me.edDataAJob.EditValue = ""
    Me.edDataAJob.Location = New System.Drawing.Point(268, 96)
    Me.edDataAJob.Name = "edDataAJob"
    Me.edDataAJob.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edDataAJob.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edDataAJob.Properties.AutoHeight = False
    Me.edDataAJob.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edDataAJob.Properties.MaxLength = 65536
    Me.edDataAJob.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edDataAJob.Size = New System.Drawing.Size(112, 20)
    '
    'lbDataDaDataAJob
    '
    Me.lbDataDaDataAJob.BackColor = System.Drawing.Color.Transparent
    Me.lbDataDaDataAJob.Location = New System.Drawing.Point(4, 96)
    Me.lbDataDaDataAJob.Name = "lbDataDaDataAJob"
    Me.lbDataDaDataAJob.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbDataDaDataAJob.Size = New System.Drawing.Size(140, 20)
    Me.lbDataDaDataAJob.Text = "dalla Data alla Data Job"
    Me.lbDataDaDataAJob.UseMnemonic = False
    '
    'edDataDaJob
    '
    Me.edDataDaJob.EditValue = ""
    Me.edDataDaJob.Location = New System.Drawing.Point(148, 96)
    Me.edDataDaJob.Name = "edDataDaJob"
    Me.edDataDaJob.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edDataDaJob.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edDataDaJob.Properties.AutoHeight = False
    Me.edDataDaJob.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edDataDaJob.Properties.MaxLength = 65536
    Me.edDataDaJob.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edDataDaJob.Size = New System.Drawing.Size(112, 20)
    '
    'pbElabora1
    '
    Me.pbElabora1.Cursor = System.Windows.Forms.Cursors.Default
    Me.pbElabora1.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.pbElabora1.Location = New System.Drawing.Point(0, 5)
    Me.pbElabora1.Name = "pbElabora1"
    Me.pbElabora1.Properties.ShowTitle = True
    Me.pbElabora1.Size = New System.Drawing.Size(678, 20)
    Me.pbElabora1.TabIndex = 520
    '
    'NtsPanel3
    '
    Me.NtsPanel3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.NtsPanel3.Controls.Add(Me.pbElabora)
    Me.NtsPanel3.Controls.Add(Me.lbElabora)
    Me.NtsPanel3.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.NtsPanel3.Location = New System.Drawing.Point(0, 432)
    Me.NtsPanel3.Name = "NtsPanel3"
    Me.NtsPanel3.Size = New System.Drawing.Size(684, 44)
    '
    'pbElabora
    '
    Me.pbElabora.Cursor = System.Windows.Forms.Cursors.Default
    Me.pbElabora.Dock = System.Windows.Forms.DockStyle.Top
    Me.pbElabora.Location = New System.Drawing.Point(0, 20)
    Me.pbElabora.Name = "pbElabora"
    Me.pbElabora.Properties.ShowTitle = True
    Me.pbElabora.Size = New System.Drawing.Size(684, 20)
    Me.pbElabora.TabIndex = 525
    '
    'lbElabora
    '
    Me.lbElabora.BackColor = System.Drawing.Color.Transparent
    Me.lbElabora.Dock = System.Windows.Forms.DockStyle.Top
    Me.lbElabora.Location = New System.Drawing.Point(0, 0)
    Me.lbElabora.Name = "lbElabora"
    Me.lbElabora.Size = New System.Drawing.Size(684, 20)
    Me.lbElabora.Text = "Nessuna operazione in corso"
    Me.lbElabora.UseMnemonic = False
    '
    'NtsGridView1
    '
    Me.NtsGridView1.Enabled = True
    Me.NtsGridView1.Name = "NtsGridView1"
    Me.NtsGridView1.OptionsCustomization.AllowRowSizing = True
    Me.NtsGridView1.OptionsNavigation.EnterMoveNextColumn = True
    Me.NtsGridView1.OptionsNavigation.UseTabKey = False
    Me.NtsGridView1.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.NtsGridView1.OptionsView.ColumnAutoWidth = False
    Me.NtsGridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.NtsGridView1.OptionsView.ShowGroupPanel = False
    '
    'NtsGridView2
    '
    Me.NtsGridView2.Enabled = True
    Me.NtsGridView2.Name = "NtsGridView2"
    Me.NtsGridView2.OptionsCustomization.AllowRowSizing = True
    Me.NtsGridView2.OptionsNavigation.EnterMoveNextColumn = True
    Me.NtsGridView2.OptionsNavigation.UseTabKey = False
    Me.NtsGridView2.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.NtsGridView2.OptionsView.ColumnAutoWidth = False
    Me.NtsGridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.NtsGridView2.OptionsView.ShowGroupPanel = False
    '
    'NtsGridView3
    '
    Me.NtsGridView3.Enabled = True
    Me.NtsGridView3.Name = "NtsGridView3"
    Me.NtsGridView3.OptionsCustomization.AllowRowSizing = True
    Me.NtsGridView3.OptionsNavigation.EnterMoveNextColumn = True
    Me.NtsGridView3.OptionsNavigation.UseTabKey = False
    Me.NtsGridView3.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.NtsGridView3.OptionsView.ColumnAutoWidth = False
    Me.NtsGridView3.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.NtsGridView3.OptionsView.ShowGroupPanel = False
    '
    'NtsGridView4
    '
    Me.NtsGridView4.Enabled = True
    Me.NtsGridView4.Name = "NtsGridView4"
    Me.NtsGridView4.OptionsCustomization.AllowRowSizing = True
    Me.NtsGridView4.OptionsNavigation.EnterMoveNextColumn = True
    Me.NtsGridView4.OptionsNavigation.UseTabKey = False
    Me.NtsGridView4.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.NtsGridView4.OptionsView.ColumnAutoWidth = False
    Me.NtsGridView4.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.NtsGridView4.OptionsView.ShowGroupPanel = False
    '
    'NtsGridView5
    '
    Me.NtsGridView5.Enabled = True
    Me.NtsGridView5.Name = "NtsGridView5"
    Me.NtsGridView5.OptionsCustomization.AllowRowSizing = True
    Me.NtsGridView5.OptionsNavigation.EnterMoveNextColumn = True
    Me.NtsGridView5.OptionsNavigation.UseTabKey = False
    Me.NtsGridView5.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.NtsGridView5.OptionsView.ColumnAutoWidth = False
    Me.NtsGridView5.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.NtsGridView5.OptionsView.ShowGroupPanel = False
    '
    'fmCorpo
    '
    Me.fmCorpo.AllowDrop = True
    Me.fmCorpo.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmCorpo.Appearance.Options.UseBackColor = True
    Me.fmCorpo.Controls.Add(Me.grCorpo)
    Me.fmCorpo.Dock = System.Windows.Forms.DockStyle.Fill
    Me.fmCorpo.Location = New System.Drawing.Point(0, 342)
    Me.fmCorpo.Name = "fmCorpo"
    Me.fmCorpo.Size = New System.Drawing.Size(684, 90)
    Me.fmCorpo.Text = "CORPO"
    '
    'grCorpo
    '
    Me.grCorpo.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grCorpo.Location = New System.Drawing.Point(2, 21)
    Me.grCorpo.MainView = Me.grvCorpo
    Me.grCorpo.MenuManager = Me.NtsBarManager1
    Me.grCorpo.Name = "grCorpo"
    Me.grCorpo.Size = New System.Drawing.Size(680, 67)
    Me.grCorpo.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvCorpo})
    '
    'grvCorpo
    '
    Me.grvCorpo.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.idriga, Me.articleNumber, Me.ar_descr, Me.ar_unmis, Me.operation, Me.nominalQuantity, Me.actualQuantity, Me.containerSize, Me.positionStatus})
    Me.grvCorpo.Enabled = True
    Me.grvCorpo.GridControl = Me.grCorpo
    Me.grvCorpo.Name = "grvCorpo"
    Me.grvCorpo.OptionsCustomization.AllowRowSizing = True
    Me.grvCorpo.OptionsNavigation.EnterMoveNextColumn = True
    Me.grvCorpo.OptionsNavigation.UseTabKey = False
    Me.grvCorpo.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.grvCorpo.OptionsView.ColumnAutoWidth = False
    Me.grvCorpo.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.grvCorpo.OptionsView.ShowGroupPanel = False
    '
    'idriga
    '
    Me.idriga.AppearanceCell.Options.UseBackColor = True
    Me.idriga.AppearanceCell.Options.UseTextOptions = True
    Me.idriga.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.idriga.Caption = "Riga"
    Me.idriga.Enabled = False
    Me.idriga.FieldName = "idriga"
    Me.idriga.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.idriga.Name = "idriga"
    Me.idriga.OptionsColumn.AllowEdit = False
    Me.idriga.OptionsColumn.ReadOnly = True
    Me.idriga.Visible = True
    Me.idriga.VisibleIndex = 0
    '
    'articleNumber
    '
    Me.articleNumber.AppearanceCell.Options.UseBackColor = True
    Me.articleNumber.AppearanceCell.Options.UseTextOptions = True
    Me.articleNumber.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.articleNumber.Caption = "Articolo"
    Me.articleNumber.Enabled = False
    Me.articleNumber.FieldName = "articleNumber"
    Me.articleNumber.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.articleNumber.Name = "articleNumber"
    Me.articleNumber.OptionsColumn.AllowEdit = False
    Me.articleNumber.OptionsColumn.ReadOnly = True
    Me.articleNumber.Visible = True
    Me.articleNumber.VisibleIndex = 1
    '
    'ar_descr
    '
    Me.ar_descr.AppearanceCell.Options.UseBackColor = True
    Me.ar_descr.AppearanceCell.Options.UseTextOptions = True
    Me.ar_descr.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.ar_descr.Caption = "Descrizione Articolo"
    Me.ar_descr.Enabled = False
    Me.ar_descr.FieldName = "ar_descr"
    Me.ar_descr.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.ar_descr.Name = "ar_descr"
    Me.ar_descr.OptionsColumn.AllowEdit = False
    Me.ar_descr.OptionsColumn.ReadOnly = True
    Me.ar_descr.Visible = True
    Me.ar_descr.VisibleIndex = 2
    '
    'ar_unmis
    '
    Me.ar_unmis.AppearanceCell.Options.UseBackColor = True
    Me.ar_unmis.AppearanceCell.Options.UseTextOptions = True
    Me.ar_unmis.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.ar_unmis.Caption = "U.M."
    Me.ar_unmis.Enabled = False
    Me.ar_unmis.FieldName = "ar_unmis"
    Me.ar_unmis.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.ar_unmis.Name = "ar_unmis"
    Me.ar_unmis.OptionsColumn.AllowEdit = False
    Me.ar_unmis.OptionsColumn.ReadOnly = True
    Me.ar_unmis.Visible = True
    Me.ar_unmis.VisibleIndex = 3
    '
    'operation
    '
    Me.operation.AppearanceCell.Options.UseBackColor = True
    Me.operation.AppearanceCell.Options.UseTextOptions = True
    Me.operation.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.operation.Caption = "Operaz."
    Me.operation.Enabled = False
    Me.operation.FieldName = "operation"
    Me.operation.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.operation.Name = "operation"
    Me.operation.OptionsColumn.AllowEdit = False
    Me.operation.OptionsColumn.ReadOnly = True
    Me.operation.Visible = True
    Me.operation.VisibleIndex = 4
    '
    'nominalQuantity
    '
    Me.nominalQuantity.AppearanceCell.Options.UseBackColor = True
    Me.nominalQuantity.AppearanceCell.Options.UseTextOptions = True
    Me.nominalQuantity.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.nominalQuantity.Caption = "Q.tà Inviata"
    Me.nominalQuantity.Enabled = False
    Me.nominalQuantity.FieldName = "nominalQuantity"
    Me.nominalQuantity.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.nominalQuantity.Name = "nominalQuantity"
    Me.nominalQuantity.OptionsColumn.AllowEdit = False
    Me.nominalQuantity.OptionsColumn.ReadOnly = True
    Me.nominalQuantity.Visible = True
    Me.nominalQuantity.VisibleIndex = 5
    '
    'actualQuantity
    '
    Me.actualQuantity.AppearanceCell.Options.UseBackColor = True
    Me.actualQuantity.AppearanceCell.Options.UseTextOptions = True
    Me.actualQuantity.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.actualQuantity.Caption = "Q.tà Elaborata"
    Me.actualQuantity.Enabled = False
    Me.actualQuantity.FieldName = "actualQuantity"
    Me.actualQuantity.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.actualQuantity.Name = "actualQuantity"
    Me.actualQuantity.OptionsColumn.AllowEdit = False
    Me.actualQuantity.OptionsColumn.ReadOnly = True
    Me.actualQuantity.Visible = True
    Me.actualQuantity.VisibleIndex = 6
    '
    'containerSize
    '
    Me.containerSize.AppearanceCell.Options.UseBackColor = True
    Me.containerSize.AppearanceCell.Options.UseTextOptions = True
    Me.containerSize.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.containerSize.Caption = "Container Size"
    Me.containerSize.Enabled = False
    Me.containerSize.FieldName = "containerSize"
    Me.containerSize.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.containerSize.Name = "containerSize"
    Me.containerSize.OptionsColumn.AllowEdit = False
    Me.containerSize.OptionsColumn.ReadOnly = True
    Me.containerSize.Visible = True
    Me.containerSize.VisibleIndex = 7
    '
    'positionStatus
    '
    Me.positionStatus.AppearanceCell.Options.UseBackColor = True
    Me.positionStatus.AppearanceCell.Options.UseTextOptions = True
    Me.positionStatus.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.positionStatus.Caption = "Position Status"
    Me.positionStatus.Enabled = False
    Me.positionStatus.FieldName = "positionStatus"
    Me.positionStatus.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.positionStatus.Name = "positionStatus"
    Me.positionStatus.OptionsColumn.AllowEdit = False
    Me.positionStatus.OptionsColumn.ReadOnly = True
    Me.positionStatus.Visible = True
    Me.positionStatus.VisibleIndex = 8
    '
    'NtsSplitter1
    '
    Me.NtsSplitter1.Dock = System.Windows.Forms.DockStyle.Top
    Me.NtsSplitter1.Location = New System.Drawing.Point(0, 339)
    Me.NtsSplitter1.Name = "NtsSplitter1"
    Me.NtsSplitter1.Size = New System.Drawing.Size(684, 3)
    Me.NtsSplitter1.TabIndex = 23
    Me.NtsSplitter1.TabStop = False
    '
    'fmTesta
    '
    Me.fmTesta.AllowDrop = True
    Me.fmTesta.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmTesta.Appearance.Options.UseBackColor = True
    Me.fmTesta.Controls.Add(Me.grTesta)
    Me.fmTesta.Dock = System.Windows.Forms.DockStyle.Top
    Me.fmTesta.Location = New System.Drawing.Point(0, 203)
    Me.fmTesta.Name = "fmTesta"
    Me.fmTesta.Size = New System.Drawing.Size(684, 136)
    Me.fmTesta.Text = "TESTA"
    '
    'grTesta
    '
    Me.grTesta.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grTesta.Location = New System.Drawing.Point(2, 21)
    Me.grTesta.MainView = Me.grvTesta
    Me.grTesta.MenuManager = Me.NtsBarManager1
    Me.grTesta.Name = "grTesta"
    Me.grTesta.Size = New System.Drawing.Size(680, 113)
    Me.grTesta.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvTesta})
    '
    'grvTesta
    '
    Me.grvTesta.Appearance.FocusedCell.BackColor = System.Drawing.Color.FloralWhite
    Me.grvTesta.Appearance.FocusedCell.Options.UseBackColor = True
    Me.grvTesta.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.xx_sel, Me.id, Me.jobNumber, Me.busTipork, Me.busAnno, Me.busSerie, Me.busNumero, Me.tm_datdoc, Me.tm_conto, Me.xx_descrConto, Me.jobPriority, Me.jobDataOra, Me.jobStatus})
    Me.grvTesta.Enabled = True
    Me.grvTesta.GridControl = Me.grTesta
    Me.grvTesta.Name = "grvTesta"
    Me.grvTesta.OptionsCustomization.AllowRowSizing = True
    Me.grvTesta.OptionsNavigation.EnterMoveNextColumn = True
    Me.grvTesta.OptionsNavigation.UseTabKey = False
    Me.grvTesta.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.grvTesta.OptionsView.ColumnAutoWidth = False
    Me.grvTesta.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.grvTesta.OptionsView.ShowGroupPanel = False
    '
    'xx_sel
    '
    Me.xx_sel.AppearanceCell.Options.UseBackColor = True
    Me.xx_sel.AppearanceCell.Options.UseTextOptions = True
    Me.xx_sel.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.xx_sel.Caption = "Sel."
    Me.xx_sel.Enabled = True
    Me.xx_sel.FieldName = "xx_sel"
    Me.xx_sel.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.xx_sel.Name = "xx_sel"
    Me.xx_sel.Visible = True
    Me.xx_sel.VisibleIndex = 0
    '
    'id
    '
    Me.id.AppearanceCell.Options.UseBackColor = True
    Me.id.AppearanceCell.Options.UseTextOptions = True
    Me.id.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.id.Caption = "ID"
    Me.id.Enabled = False
    Me.id.FieldName = "id"
    Me.id.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.id.Name = "id"
    Me.id.OptionsColumn.AllowEdit = False
    Me.id.OptionsColumn.ReadOnly = True
    Me.id.Visible = True
    Me.id.VisibleIndex = 1
    '
    'jobNumber
    '
    Me.jobNumber.AppearanceCell.Options.UseBackColor = True
    Me.jobNumber.AppearanceCell.Options.UseTextOptions = True
    Me.jobNumber.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.jobNumber.Caption = "Numero"
    Me.jobNumber.Enabled = False
    Me.jobNumber.FieldName = "jobNumber"
    Me.jobNumber.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.jobNumber.Name = "jobNumber"
    Me.jobNumber.OptionsColumn.AllowEdit = False
    Me.jobNumber.OptionsColumn.ReadOnly = True
    Me.jobNumber.Visible = True
    Me.jobNumber.VisibleIndex = 2
    '
    'busTipork
    '
    Me.busTipork.AppearanceCell.Options.UseBackColor = True
    Me.busTipork.AppearanceCell.Options.UseTextOptions = True
    Me.busTipork.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.busTipork.Caption = "Tipo Doc."
    Me.busTipork.Enabled = False
    Me.busTipork.FieldName = "busTipork"
    Me.busTipork.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.busTipork.Name = "busTipork"
    Me.busTipork.OptionsColumn.AllowEdit = False
    Me.busTipork.OptionsColumn.ReadOnly = True
    Me.busTipork.Visible = True
    Me.busTipork.VisibleIndex = 3
    '
    'busAnno
    '
    Me.busAnno.AppearanceCell.Options.UseBackColor = True
    Me.busAnno.AppearanceCell.Options.UseTextOptions = True
    Me.busAnno.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.busAnno.Caption = "Anno Doc."
    Me.busAnno.Enabled = False
    Me.busAnno.FieldName = "busAnno"
    Me.busAnno.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.busAnno.Name = "busAnno"
    Me.busAnno.OptionsColumn.AllowEdit = False
    Me.busAnno.OptionsColumn.ReadOnly = True
    Me.busAnno.Visible = True
    Me.busAnno.VisibleIndex = 4
    '
    'busSerie
    '
    Me.busSerie.AppearanceCell.Options.UseBackColor = True
    Me.busSerie.AppearanceCell.Options.UseTextOptions = True
    Me.busSerie.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.busSerie.Caption = "Serie Doc."
    Me.busSerie.Enabled = False
    Me.busSerie.FieldName = "busSerie"
    Me.busSerie.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.busSerie.Name = "busSerie"
    Me.busSerie.OptionsColumn.AllowEdit = False
    Me.busSerie.OptionsColumn.ReadOnly = True
    Me.busSerie.Visible = True
    Me.busSerie.VisibleIndex = 5
    '
    'busNumero
    '
    Me.busNumero.AppearanceCell.Options.UseBackColor = True
    Me.busNumero.AppearanceCell.Options.UseTextOptions = True
    Me.busNumero.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.busNumero.Caption = "Numero Doc."
    Me.busNumero.Enabled = False
    Me.busNumero.FieldName = "busNumero"
    Me.busNumero.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.busNumero.Name = "busNumero"
    Me.busNumero.OptionsColumn.AllowEdit = False
    Me.busNumero.OptionsColumn.ReadOnly = True
    Me.busNumero.Visible = True
    Me.busNumero.VisibleIndex = 6
    '
    'tm_datdoc
    '
    Me.tm_datdoc.AppearanceCell.Options.UseBackColor = True
    Me.tm_datdoc.AppearanceCell.Options.UseTextOptions = True
    Me.tm_datdoc.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tm_datdoc.Caption = "Data Doc."
    Me.tm_datdoc.Enabled = False
    Me.tm_datdoc.FieldName = "tm_datdoc"
    Me.tm_datdoc.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tm_datdoc.Name = "tm_datdoc"
    Me.tm_datdoc.OptionsColumn.AllowEdit = False
    Me.tm_datdoc.OptionsColumn.ReadOnly = True
    Me.tm_datdoc.Visible = True
    Me.tm_datdoc.VisibleIndex = 7
    '
    'tm_conto
    '
    Me.tm_conto.AppearanceCell.Options.UseBackColor = True
    Me.tm_conto.AppearanceCell.Options.UseTextOptions = True
    Me.tm_conto.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tm_conto.Caption = "Conto"
    Me.tm_conto.Enabled = False
    Me.tm_conto.FieldName = "tm_conto"
    Me.tm_conto.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tm_conto.Name = "tm_conto"
    Me.tm_conto.OptionsColumn.AllowEdit = False
    Me.tm_conto.OptionsColumn.ReadOnly = True
    Me.tm_conto.Visible = True
    Me.tm_conto.VisibleIndex = 8
    '
    'xx_descrConto
    '
    Me.xx_descrConto.AppearanceCell.Options.UseBackColor = True
    Me.xx_descrConto.AppearanceCell.Options.UseTextOptions = True
    Me.xx_descrConto.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.xx_descrConto.Caption = "Descrizione Conto"
    Me.xx_descrConto.Enabled = False
    Me.xx_descrConto.FieldName = "xx_descrConto"
    Me.xx_descrConto.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.xx_descrConto.Name = "xx_descrConto"
    Me.xx_descrConto.OptionsColumn.AllowEdit = False
    Me.xx_descrConto.OptionsColumn.ReadOnly = True
    Me.xx_descrConto.Visible = True
    Me.xx_descrConto.VisibleIndex = 9
    '
    'jobPriority
    '
    Me.jobPriority.AppearanceCell.Options.UseBackColor = True
    Me.jobPriority.AppearanceCell.Options.UseTextOptions = True
    Me.jobPriority.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.jobPriority.Caption = "Priorità"
    Me.jobPriority.Enabled = False
    Me.jobPriority.FieldName = "jobPriority"
    Me.jobPriority.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.jobPriority.Name = "jobPriority"
    Me.jobPriority.OptionsColumn.AllowEdit = False
    Me.jobPriority.OptionsColumn.ReadOnly = True
    Me.jobPriority.Visible = True
    Me.jobPriority.VisibleIndex = 10
    '
    'jobDataOra
    '
    Me.jobDataOra.AppearanceCell.Options.UseBackColor = True
    Me.jobDataOra.AppearanceCell.Options.UseTextOptions = True
    Me.jobDataOra.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.jobDataOra.Caption = "Data"
    Me.jobDataOra.Enabled = False
    Me.jobDataOra.FieldName = "jobDataOra"
    Me.jobDataOra.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.jobDataOra.Name = "jobDataOra"
    Me.jobDataOra.OptionsColumn.AllowEdit = False
    Me.jobDataOra.OptionsColumn.ReadOnly = True
    Me.jobDataOra.Visible = True
    Me.jobDataOra.VisibleIndex = 11
    '
    'jobStatus
    '
    Me.jobStatus.AppearanceCell.Options.UseBackColor = True
    Me.jobStatus.AppearanceCell.Options.UseTextOptions = True
    Me.jobStatus.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.jobStatus.Caption = "Stato"
    Me.jobStatus.Enabled = False
    Me.jobStatus.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.jobStatus.Name = "jobStatus"
    Me.jobStatus.OptionsColumn.AllowEdit = False
    Me.jobStatus.OptionsColumn.ReadOnly = True
    Me.jobStatus.Visible = True
    Me.jobStatus.VisibleIndex = 12
    '
    'timerElabora
    '
    Me.timerElabora.Interval = 1000
    '
    'timerGiacenze
    '
    Me.timerGiacenze.Interval = 1000
    '
    'taskIcon
    '
    Me.taskIcon.ContextMenuStrip = Me.cmTask
    Me.taskIcon.Text = "MONITOR INCARICO TECH"
    Me.taskIcon.Visible = True
    '
    'cmTask
    '
    Me.cmTask.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmTaskChiudi})
    Me.cmTask.Name = "cmTask"
    Me.cmTask.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
    Me.cmTask.Size = New System.Drawing.Size(110, 26)
    '
    'cmTaskChiudi
    '
    Me.cmTaskChiudi.Name = "cmTaskChiudi"
    Me.cmTaskChiudi.Size = New System.Drawing.Size(109, 22)
    Me.cmTaskChiudi.Text = "Chiudi"
    '
    'FRMHHMOIN
    '
    Me.ClientSize = New System.Drawing.Size(684, 476)
    Me.Controls.Add(Me.fmCorpo)
    Me.Controls.Add(Me.NtsSplitter1)
    Me.Controls.Add(Me.fmTesta)
    Me.Controls.Add(Me.NtsPanel3)
    Me.Controls.Add(Me.fmFiltri)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Name = "FRMHHMOIN"
    Me.Text = "MONITOR INCARICO TECH"
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fmFiltri, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmFiltri.ResumeLayout(False)
    CType(Me.edDataADoc.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edDataDaDoc.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edArticolo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edConto.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edSerie.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edNumero.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edAnno.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.cbTipork.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edDataAJob.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edDataDaJob.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pbElabora1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsPanel3, System.ComponentModel.ISupportInitialize).EndInit()
    Me.NtsPanel3.ResumeLayout(False)
    CType(Me.pbElabora.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsGridView1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsGridView2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsGridView3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsGridView4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsGridView5, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fmCorpo, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmCorpo.ResumeLayout(False)
    CType(Me.grCorpo, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grvCorpo, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fmTesta, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmTesta.ResumeLayout(False)
    CType(Me.grTesta, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grvTesta, System.ComponentModel.ISupportInitialize).EndInit()
    Me.cmTask.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

  Public Overridable Sub InitControls()
    '----------------------------------------------------------------------------------------------------------------
    InitControlsBeginEndInit(Me, False)
    '----------------------------------------------------------------------------------------------------------------
    Try
      '--------------------------------------------------------------------------------------------------------------
      Try
        tlbElabora.GlyphPath = (oApp.ChildImageDir & "\elabora.png")
        tlbCerca.GlyphPath = (oApp.ChildImageDir & "\zoom.png")
        tlbCancellaRiga.GlyphPath = (oApp.ChildImageDir & "\Recdelete.png")
        tlbScarica.GlyphPath = (oApp.ChildImageDir & "\import.png")
        tlbStrumenti.GlyphPath = (oApp.ChildImageDir & "\options.png")
        tlbSeleziona.GlyphPath = (oApp.ChildImageDir & "\Ckon.png")
        tlbDeseleziona.GlyphPath = (oApp.ChildImageDir & "\Ckoff.png")
        tlbFileRettificaGiacenze.GlyphPath = (oApp.ChildImageDir & "\Chiaveinglese.png")
        tlbConfrontaGiacenze.GlyphPath = (oApp.ChildImageDir & "\Chiaveinglese.png")
        tlbGuida.GlyphPath = (oApp.ChildImageDir & "\help.png")
        tlbEsci.GlyphPath = (oApp.ChildImageDir & "\exit.png")
      Catch ex As Exception
      End Try
      '--------------------------------------------------------------------------------------------------------------
      tlbMain.NTSSetToolTip()
      '--------------------------------------------------------------------------------------------------------------
      NTSScriptExec("InitControls", Me, Nothing)
      '-------------------------------------------------

      Dim dttTipo As New DataTable
      With dttTipo
        .Columns.Add("cod", GetType(String))
        .Columns.Add("val", GetType(String))
        .Rows.Add(New Object() {"-1", "Tutti"})
        .Rows.Add(New Object() {"1", "Movimenti"})
        .Rows.Add(New Object() {"A", "Fattura Imm. emessa"})
        .Rows.Add(New Object() {"B", "DDT emesso"})
        .Rows.Add(New Object() {"C", "Corrispettivo emesso"})
        .Rows.Add(New Object() {"E", "Nota di Addeb. emessa"})
        .Rows.Add(New Object() {"F", "Ric.Fiscale Emessa"})
        .Rows.Add(New Object() {"I", "Riemissione Ric.Fiscali"})
        .Rows.Add(New Object() {"J", "Nota Accr. ricevuta"})
        .Rows.Add(New Object() {"L", "Fattura Imm. ricevuta"})
        .Rows.Add(New Object() {"M", "DDT Ricevuto"})
        .Rows.Add(New Object() {"N", "Nota Accr. emessa"})
        .Rows.Add(New Object() {"S", "Fatt.Ric.Fisc. Emessa"})
        .Rows.Add(New Object() {"T", "Carico da produzione"})
        .Rows.Add(New Object() {"W", "Nota di prelievo"})
        .Rows.Add(New Object() {"Z", "Bolla di mov. interna"})
      End With
      cbTipork.DataSource = dttTipo
      cbTipork.DisplayMember = "val"
      cbTipork.ValueMember = "cod"
      cbTipork.SelectedIndex = 0

      Dim dttOperaz As New DataTable
      With dttOperaz
        .Columns.Add("cod", GetType(String))
        .Columns.Add("val", GetType(String))
        .Rows.Add(New Object() {"+", "ENTRATA"})
        .Rows.Add(New Object() {"-", "USCITA"})
        .Rows.Add(New Object() {"p", "ENTRATA"})
        .Rows.Add(New Object() {"c", "USCITA"})
      End With

      cbTipork.NTSSetParam(oApp.Tr(Me, 128230023234256120, lbTipork.Text))
      edAnno.NTSSetParam(oMenu, oApp.Tr(Me, 128230023234256121, "Anno Doc."), "0", 4, 0, 9999)
      edSerie.NTSSetParam(oMenu, oApp.Tr(Me, 128230023234256122, "Serie Doc."), CLN__STD.SerieMaxLen, True)
      edNumero.NTSSetParam(oMenu, oApp.Tr(Me, 128230023234256123, "Numero Doc."), "0", 8, 0, 99999999)
      edDataDaDoc.NTSSetParam(oMenu, oApp.Tr(Me, 128230023234256124, "dalla Data Doc"), True)
      edDataADoc.NTSSetParam(oMenu, oApp.Tr(Me, 128230023234256125, "alla Data Doc"), True)
      edDataDaJob.NTSSetParam(oMenu, oApp.Tr(Me, 128230023234256126, "dalla Data Job"), True)
      edDataAJob.NTSSetParam(oMenu, oApp.Tr(Me, 128230023234256127, "alla Data Job"), True)
      edConto.NTSSetParamTabe(oMenu, oApp.Tr(Me, 128230023234256128, lbConto.Text), tabanagrac)
      edArticolo.NTSSetParamTabe(oMenu, oApp.Tr(Me, 128230023234256129, lbArticolo.Text), tabartico, True)

      ConfiguraZoomContoHome()

      edSerie.NTSForzaVisZoom = True
      edNumero.NTSForzaVisZoom = True

      '-------------------------------------------------
      'Griglia Testa
      grvTesta.NTSSetParam(oMenu, oApp.Tr(Me, 128230023234256139, "Lista Testa"))
      grvTesta.NTSAllowInsert = False
      'grvTesta.NTSHideMenuSx()
      grvTesta.NTSColonnaMultiselezione = "xx_sel"

      xx_sel.NTSSetParamCHK(oMenu, oApp.Tr(Me, 128230023234256140, xx_sel.Caption), "S", "N")
      id.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256141, id.Caption), "0", 10, 0, 9999999999)
      jobNumber.NTSSetParamSTR(oMenu, oApp.Tr(Me, 128230023234256142, jobNumber.Caption), 50, True)
      busTipork.NTSSetParamCMB(oMenu, oApp.Tr(Me, 128230023234256143, busTipork.Caption), dttTipo, "val", "cod")
      busAnno.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256144, busAnno.Caption), "0", 4, 0, 9999)
      busSerie.NTSSetParamSTR(oMenu, oApp.Tr(Me, 128230023234256145, busSerie.Caption), 3, True)
      busNumero.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256146, busNumero.Caption), "0", 8, 0, 99999999)
      tm_datdoc.NTSSetParamDATA(oMenu, oApp.Tr(Me, 128230023234256147, tm_datdoc.Caption), True)
      tm_conto.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256148, tm_conto.Caption), "0", 10, 0, 9999999999)
      xx_descrConto.NTSSetParamSTR(oMenu, oApp.Tr(Me, 128230023234256149, xx_descrConto.Caption), 200, True)
      jobPriority.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256150, jobPriority.Caption), "0", 5, 0, 99999)
      jobDataOra.NTSSetParamSTR(oMenu, oApp.Tr(Me, 128230023234256151, jobDataOra.Caption), 20, True)
      jobStatus.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256152, jobStatus.Caption), "0", 5, 0, 99999)

      '-------------------------------------------------
      'Griglia Corpo
      grvCorpo.NTSSetParam(oMenu, oApp.Tr(Me, 128230023234256239, "Lista Corpo"))
      grvCorpo.NTSAllowInsert = False
      'grvCorpo.NTSHideMenuSx()

      idriga.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256240, idriga.Caption), "0", 5, 0, 99999)
      articleNumber.NTSSetParamSTR(oMenu, oApp.Tr(Me, 128230023234256241, articleNumber.Caption), 50, True)
      ar_descr.NTSSetParamSTR(oMenu, oApp.Tr(Me, 128230023234256242, ar_descr.Caption), 255, True)
      ar_unmis.NTSSetParamSTR(oMenu, oApp.Tr(Me, 128230023234256143, ar_unmis.Caption), 3, True)
      operation.NTSSetParamCMB(oMenu, oApp.Tr(Me, 128230023234256144, operation.Caption), dttOperaz, "val", "cod")
      nominalQuantity.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256145, nominalQuantity.Caption), "0.00", 10, 0, 9999999999)
      actualQuantity.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256146, actualQuantity.Caption), "0.00", 10, 0, 9999999999)
      containerSize.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256147, containerSize.Caption), "0", 5, 0, 99999)
      positionStatus.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234256148, positionStatus.Caption), "0", 5, 0, 99999)

      grvCorpo.AddColumnBackColor("backcolor_actualQuantity")

      'chiamo lo script per inizializzare i controlli caricati con source ext
      NTSScriptExec("InitControls", Me, Nothing)

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
    '----------------------------------------------------------------------------------------------------------------
    InitControlsBeginEndInit(Me, True)
    '----------------------------------------------------------------------------------------------------------------
  End Sub

  Public Overloads Overrides Sub GestisciEventiEntity(ByVal sender As Object, ByRef e As NTSEventArgs)
    '------------------------------------------------------------
    '--- Questa funzione riceve gli eventi dall'ENTITY: rimappata rispetto a quella standard di FRM__CHILD
    '--- prima eseguo quella standard
    '------------------------------------------------------------
    If Not IsMyThrowRemoteEvent() Then Return 'il messaggio non è per questa form ...
    MyBase.GestisciEventiEntity(sender, e)

    Try
      '------------------------------------------------------------
      '--- Adesso gestisco le specifiche
      '--- devo inserire delle funzioni qui sotto per fare in modo che al variare di dati nell'entity delle informazioni 
      '--- legate all'interfaccia grafica (ui) vengano allineate a quanto richiesto dall'entity
      '------------------------------------------------------------
      Select Case e.TipoEvento
        Case "PROGRESS_BAR_POS"
          pbElabora.Position = NTSCInt(e.Message)
          pbElabora.Refresh()
        Case "INFO_LABEL"
          lbElabora.Text = e.Message
          lbElabora.Refresh()
        Case "TIMER_ELABORA_INTERVAL"
          timerElabora.Interval = NTSCInt(e.Message)
        Case "TIMER_GIACENZA_INTERVAL"
          timerGiacenze.Interval = NTSCInt(e.Message)
      End Select

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Eventi Form"

  Public Overridable Sub FRMHHMOIN_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Try

      '--------------------------------------------------------------------------------------------------------------
      InitControls()
      '--------------------------------------------------------------------------------------------------------------
      GctlSetRoules()
      '--------------------------------------------------------------------------------------------------------------

      'Dim processo() As Process

      'processo = Process.GetProcessesByName("BNHHMOIN", My.Computer.Name)
      'If processo.Length > 1 Then
      '  taskIcon.Visible = False
      '  MsgBox("Attenzione! è già aperto un processo per questo programma", vbExclamation)
      '  'oApp.MsgBoxInfo("Attenzione! è già aperto un processo per questo programma")
      '  Me.Close()
      '  Return
      'End If

      oCleMoin.LeggiRegistro()

      '--------------------------------------------------
      '--- Legge dal recent la posizione dello splitter
      '--------------------------------------------------
      Dim nTopRecent As Integer = NTSCInt(oMenu.GetSettingBus("BSHHMOIN", "Recent", ".", "HH_spSplitterMOIN", "0", " ", "0"))
      If nTopRecent <= Me.Height - 100 AndAlso nTopRecent > 0 Then fmTesta.Height = nTopRecent
      Dim sTipork As String = oMenu.GetSettingBus("BSHHMOIN", "Recent", ".", "HH_tiporkMOIN", " ", " ", " ")
      If sTipork <> "" Then cbTipork.SelectedValue = sTipork
      If cbTipork.SelectedText = "" Then cbTipork.SelectedIndex = 0

      edAnno.Text = Date.Now.Year.ToString
      edSerie.Text = ""

      NTSFormClearDataBinding(Me)

      If Not oApp.Batch Then ApriTesta()

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub FRMHHMOIN_ActivatedFirst(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ActivatedFirst
    Try
      '--------------------------------------------------------------------------------------------------------------
      '--------------------------------------------------------------------------------------------------------------
      GctlApplicaDefaultValue()

      timerElabora.Stop()
      timerGiacenze.Stop()
      If oApp.Batch Then
        Me.Hide()
        'taskIcon.Icon = Me.Icon
        cmTaskChiudi.Image = New Bitmap(oApp.ChildImageDir & "\exit.png")
        'taskIcon.Visible = True
        If oCleMoin.IstanziaHHInte() Then
          If File.Exists(oApp.AvvioProgrammaParametri) Then
            Dim bGiacenze As Boolean = CBool(NTSCInt(File.ReadAllText(oApp.AvvioProgrammaParametri)))
            If bGiacenze Then
              oCleMoin.GestisciGiacenze()
            Else
              oCleMoin.GestisciElabora()
            End If
          End If
          Me.Close()
          Return
          'timerElabora.Start()
          'timerGiacenze.Start()
        End If
      End If

      '--------------------------------------------------------------------------------------------------------------
    Catch ex As Exception
      If oApp.Batch Then
        Me.Close()
      Else
        CLN__STD.GestErr(ex, Me, "")
      End If
    End Try
  End Sub

  Public Overridable Sub FRMHHMOIN_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    Try
      '--------------------------------------------------
      '--- Scrive nel recent la posizione dello splitter
      '--------------------------------------------------
      oMenu.SaveSettingBus("BSHHMOIN", "Recent", ".", "HH_spSplitterMOIN", fmTesta.Height.ToString, " ", True, True, False)
      oMenu.SaveSettingBus("BSHHMOIN", "Recent", ".", "HH_tiporkMOIN", cbTipork.SelectedValue, " ", True, True, False)

      taskIcon.Visible = False

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub
#End Region

#Region "Eventi Toolbar"

  Public Overridable Sub tlbElabora_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbElabora.ItemClick
    Try
      Elabora()
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub tlbScarica_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbScarica.ItemClick
    Try
      Scarica()
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub tlbCerca_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbCerca.ItemClick
    Try
      ApriTesta()
      ApriCorpo()
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub tlbCancellaRiga_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbCancellaRiga.ItemClick
    Try
      CancellaRiga()
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub tlbSeleziona_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbSeleziona.ItemClick
    Try
      If grvTesta.GetFocusedDataRow Is Nothing Then Return
      grvTesta.NTSGetCurrentDataRow.EndEdit()
      grvTesta.NTSSelezionaTutto()
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub tlbDeseleziona_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbDeseleziona.ItemClick
    Try
      If grvTesta.GetFocusedDataRow Is Nothing Then Return
      grvTesta.NTSGetCurrentDataRow.EndEdit()
      grvTesta.NTSDeselezionaTutto()
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub tlbFileRettificaGiacenze_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbFileRettificaGiacenze.ItemClick
    Try
      RettificaGiacenze()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbConfrontaGiacenze_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbConfrontaGiacenze.ItemClick
    Try
      ConfrontaGiacenze()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbEsci_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbEsci.ItemClick
    Try
      Me.Close()
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub tlbZoom_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbZoom.ItemClick
    'SendKeys.SendWait("{F5}")
    'se faccio la riga sopra va in un loop infinito....
    'devo vedere quale controllo ha il focus, quindi lanciare lo zoom direttamente sul controllo, senza forzare l'F5
    Dim ctrlTmp As Control = Nothing
    Dim oParam As New CLE__CLDP
    Dim dttOpens As New DataTable    'datatable contenente l'elenco dei documenti che si vogliono aprire (compilato da zoom documenti per selezionarne uno)

    Try


      ''------------------------------------
      ''zoom documenti 
      'If tsBoll.Visible = True Then Return 'esco se sono dentro ad un documento

      'oParam.nTipologia = 1   '= bsveboll
      'oParam.strTipo = cbTipoDoc.SelectedValue.ToString
      'oParam.strAlfpar = edSerieDoc.Text
      'oParam.nAnno = NTSCInt(edAnnoDoc.Text)
      'oParam.nCodStab = oCleBoll.dtoGestStab.nCodStab
      'NTSZOOM.strIn = ""
      'NTSZOOM.ZoomStrIn("ZOOMDOCUMENTI", DittaCorrente, oParam)
      'If NTSZOOM.strIn = "" Then Return

      'If Not oParam.oParam Is Nothing Then
      '  'la selezione documenti mi ha passato un elenco di documenti da aprire: memorizzo l'elenco
      '  dttOpens = CType(oParam.oParam, DataTable).Copy
      '  CType(oParam.oParam, DataTable).Clear()
      '  oParam.oParam = Nothing
      'End If
      'If dttOpens.Rows.Count > 1 Then
      '  GctlSetVisEnab(tlbApriDocPrec, False)
      '  GctlSetVisEnab(tlbApriDocSucc, False)

      '  cbTipoDoc.SelectedValue = dttOpens.Rows(0) !tm_tipork.ToString
      '  edAnnoDoc.Text = dttOpens.Rows(0) !tm_anno.ToString
      '  edSerieDoc.Text = dttOpens.Rows(0) !tm_serie.ToString
      '  edNumDoc.Text = dttOpens.Rows(0) !tm_numdoc.ToString
      'Else
      '  dttOpens.Clear()
      '  tlbApriDocPrec.Enabled = False
      '  tlbApriDocSucc.Enabled = False

      '  edSerieDoc.Text = oParam.strAlfpar
      '  edNumDoc.Text = oParam.lNumpar.ToString
      'End If

      'Apri()

      'NTSCallStandardZoom()
      '------------------------------------
      'zoom specifico per le destinazioni diverse (devo passare il conto cliente/fornitore)
      If edSerie.Focused Then
        '------------------------------------
        'zoom serie
        If NTSCInt(edAnno.Text) = 0 Then
          oApp.MsgBoxInfo(oApp.Tr(Me, 130512728416683717, "Indicare prima l'anno"))
          Return
        End If
        SetFastZoom(edSerie.Text, oParam)    'abilito la gestione dello zoom veloce
        oParam.strDescr = cbTipork.SelectedValue
        oParam.lContoCF = NTSCInt(edAnno.Text)
        NTSZOOM.strIn = ""
        NTSZOOM.ZoomStrIn("ZOOMSERIE", DittaCorrente, oParam)
        If oParam.strOut <> "" Then edSerie.NTSTextDB = oParam.strOut

        '  'zoom con F5 sul campo testo note per attivare lo zoom contestuale.
        'ElseIf grvInor.FocusedColumn.Equals(xx_partnumber) Then
        '  NTSZOOM.strIn = NTSCStr(grvInor.EditingValue)
        '  NTSZOOM.ZoomStrIn("ZOOMARTICO", DittaCorrente, oParam)
        '  If NTSZOOM.strIn <> NTSCStr(grvInor.EditingValue) Then grvInor.SetFocusedValue(NTSZOOM.strIn)
      ElseIf edNumero.Focused Or cbTipork.Focused Then
        '------------------------------------
        'zoom documenti 

        oParam.nTipologia = 1   '= bsveboll
        oParam.strTipo = cbTipork.SelectedValue.ToString
        oParam.strAlfpar = edSerie.Text
        oParam.nAnno = NTSCInt(edAnno.Text)
        oParam.nCodStab = oCleMoin.dtoGestStab.nCodStab
        NTSZOOM.strIn = ""
        NTSZOOM.ZoomStrIn("ZOOMDOCUMENTI", DittaCorrente, oParam)
        If NTSZOOM.strIn = "" Then Return

        If Not oParam.oParam Is Nothing Then
          'la selezione documenti mi ha passato un elenco di documenti da aprire: memorizzo l'elenco
          dttOpens = CType(oParam.oParam, DataTable).Copy
          CType(oParam.oParam, DataTable).Clear()
          oParam.oParam = Nothing
        End If
        'If dttOpens.Rows.Count > 1 Then
        ''GctlSetVisEnab(tlbApriDocPrec, False)
        ''GctlSetVisEnab(tlbApriDocSucc, False)

        'cbTipork.SelectedValue = dttOpens.Rows(0) !tm_tipork.ToString
        'edAnno.Text = dttOpens.Rows(0) !tm_anno.ToString
        'edSerie.Text = dttOpens.Rows(0) !tm_serie.ToString
        'edNumero.Text = dttOpens.Rows(0) !tm_numdoc.ToString
        'Else
        dttOpens.Clear()
        ''tlbApriDocPrec.Enabled = False
        ''tlbApriDocSucc.Enabled = False

        'edSerie.Text = oParam.strAlfpar
        edNumero.Text = oParam.lNumpar.ToString
        'End If

        'Apri()
      Else
        '------------------------------------
        'zoom standard di textbox e griglia
        NTSCallStandardZoom()
      End If

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Eventi Timer"

  Public Overridable Sub timerElabora_Tick(sender As Object, e As EventArgs) Handles timerElabora.Tick
    Try
      timerElabora.Stop()

      'oCleMoin.GestisciTimerElabora()

    Catch ex As Exception

    Finally
      timerElabora.Start()
    End Try
  End Sub

  Public Overridable Sub timerGiacenze_Tick(sender As Object, e As EventArgs) Handles timerGiacenze.Tick
    Try
      timerGiacenze.Stop()

      'oCleMoin.GestisciTimerGiacenze()

    Catch ex As Exception

    Finally
      timerGiacenze.Start()
    End Try
  End Sub

#End Region

#Region "Eventi Combo"

  Public Overridable Sub cbTipork_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbTipork.SelectedIndexChanged
    Try
      ConfiguraZoomContoHome()
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Eventi Controlli"

  Public Overridable Sub edConto_EditValueChanged(sender As Object, e As EventArgs) Handles edConto.EditValueChanged
    Try
      Dim strDescr As String = ""
      If oCleMoin Is Nothing Then Return

      oCleMoin.ValCodcieDb(edConto.Text.ToString(), "anagra", "N", strDescr, Nothing)
      lbNomeConto.Text = strDescr
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub edArticolo_EditValueChanged(sender As Object, e As EventArgs) Handles edArticolo.EditValueChanged
    Try
      Dim strDescr As String = ""
      If oCleMoin Is Nothing Then Return

      oCleMoin.ValCodcieDb(edArticolo.Text.ToString(), "artico", "S", strDescr, Nothing)
      lbNomeArticolo.Text = strDescr
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub cmTaskChiudi_Click(sender As Object, e As EventArgs) Handles cmTaskChiudi.Click
    Try
      Me.Close()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Eventi Griglia"

  Public Overridable Sub grvTesta_NTSFocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles grvTesta.NTSFocusedRowChanged
    Try
      ApriCorpo()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Procedure"

  Public Overridable Sub Scarica()
    Try
      If Not oApp.Batch Then
        If oApp.MsgBoxInfoYesNo_DefNo("Confermi lo scarico dei movimenti?") = DialogResult.No Then Return
      End If

      Me.Cursor = Cursors.WaitCursor
      oCleMoin.Scarica()
      ApriTesta()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try

  End Sub

  Public Overridable Sub ApriTesta()
    Try
      Me.Validate()
      Me.Cursor = Cursors.WaitCursor
      SettaOpzioni()
      oCleMoin.LeggiTesta("N", dtOpz)
      dcMoin = New BindingSource
      dsMoin = oCleMoin.dsShared
      If Not dsMoin.Tables("CORPO") Is Nothing Then dsMoin.Tables("CORPO").Clear()
      dcMoin.DataSource = dsMoin.Tables("TESTA")
      dsMoin.AcceptChanges()
      grTesta.DataSource = dcMoin
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try

  End Sub

  Public Overridable Sub ApriCorpo()
    Try
      Dim dr As DataRow = grvTesta.GetFocusedDataRow
      If dr Is Nothing Then Return

      Me.Cursor = Cursors.WaitCursor
      oCleMoin.LeggiCorpo(NTSCInt(dr!id), NTSCStr(dr!busTipork))
      dcMoin = New BindingSource
      dsMoin = oCleMoin.dsShared
      dcMoin.DataSource = dsMoin.Tables("CORPO")
      dsMoin.AcceptChanges()
      grCorpo.DataSource = dcMoin
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub Elabora()
    Try
      If Not oApp.Batch Then
        If oApp.MsgBoxInfoYesNo_DefNo("Confermi l'elaborazione delle righe selezionate?") = DialogResult.No Then Return
      End If

      Me.Cursor = Cursors.WaitCursor
      oCleMoin.Elabora()
      ApriTesta()
      ApriCorpo()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub RettificaGiacenze()
    Try
      If oApp.MsgBoxInfoYesNo_DefNo("Confermi la creazione del file di rettifica delle giacenze?") = DialogResult.No Then Return

      Me.Cursor = Cursors.WaitCursor
      oCleMoin.RettificaGiacenze()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub ConfrontaGiacenze()
    Dim frmCogi As FRMHHCOGI = Nothing
    Try
      Me.Cursor = Cursors.WaitCursor

      frmCogi = CType(NTSNewFormModal("FRMHHCOGI"), FRMHHCOGI)
      If Not frmCogi.Init(oMenu, oCallParams, DittaCorrente, Nothing) Then Return
      frmCogi.InitEntity(oCleMoin)
      frmCogi.ShowDialog(Me)

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub CancellaRiga()
    Try
      Dim dr As DataRow = grvTesta.GetFocusedDataRow
      If dr Is Nothing Then Return
      If oApp.MsgBoxInfoYesNo_DefNo("Confermi la cancellazione della riga corrente?") = DialogResult.No Then Return

      Me.Cursor = Cursors.WaitCursor
      If oCleMoin.CancellaRiga(NTSCInt(dr!id), NTSCStr(dr!busTipork)) Then
        dr.Delete()
        ApriCorpo()
      End If
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub SettaOpzioni()
    Try
      oCleMoin.setDtOpz(dtOpz)

      dtOpz.Rows.Add(cbTipork.SelectedValue, edAnno.Text, edSerie.Text, edNumero.Text, edDataDaDoc.Text, edDataADoc.Text, edDataDaJob.Text, edDataAJob.Text, edConto.Text, edArticolo.Text)

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub ConfiguraZoomContoHome()
    Try
      Select Case cbTipork.SelectedValue
        Case "A", "B", "C", "D", "N", "E", "W", "Z", "F", "I", "S"
          edConto.NTSSetParamZoom("ZOOMANAGRAC")
        Case "-1"
          edConto.NTSSetParamZoom("ZOOMANAGRA")
        Case Else
          edConto.NTSSetParamZoom("ZOOMANAGRAF")
      End Select
    Catch ex As Exception
      '--------------------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------------------
    End Try
  End Sub

#End Region

End Class