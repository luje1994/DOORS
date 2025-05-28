Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System.IO
Imports System.Diagnostics
Imports System.ComponentModel
'Imports System.Management

Public Class FRMHHCOOP

  Public oCleCoop As CLEHHCOOP
  Public oCallParams As CLE__CLDP

  Public dsCoop As DataSet
  Public dcCoop As BindingSource = New BindingSource

  'Public dtOpz As New DataTable

  Public bCompleteConnect As Boolean = True
  Public bCompleteElabora As Boolean = True
  Public nTentativi As Integer = 0
  Public bStopTimerConnect As Boolean = False
  Public bStopTimerElabora As Boolean = False

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
    If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNHHCOOP", "BEHHCOOP", oTmp, strErr, False, "", "") = False Then
      oApp.MsgBoxErr(oApp.Tr(Me, 128423763770716000, "ERRORE in fase di creazione Entity:" & vbCrLf & strErr))
      Return False
    End If
    oCleCoop = CType(oTmp, CLEHHCOOP)

    '--------------------------------------------------
    '--- Aggiunge gestore eventi
    '--------------------------------------------------
    AddHandler oCleCoop.RemoteEvent, AddressOf GestisciEventiEntity

    If oCleCoop.Init(oApp, NTSScript, oMenu.oCleComm, "", False, "", "") = False Then Return False

    'If Not oCleMaav.CaricaConnessioniDB() Then
    '  'oApp.MsgBoxErr(oApp.Tr(Me, 128423763770716000, "ERRORE Non è stato configurato correttamente la connessione di TRUMPF." & vbCrLf & "Chiudo il programma"))
    '  MsgBox("ERRORE Non è stato configurato correttamente la connessione di TRUMPF." & vbCrLf & "Chiudo il programma", MsgBoxStyle.Critical, "Bussines Cube")
    '  Return False
    'End If

    Return True
    '----------------------------------------------------------------------------------------------------------------
  End Function

  Public Overridable Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMHHCOOP))
    Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
    Me.tlbMain = New NTSInformatica.NTSBar()
    Me.tlbCerca = New NTSInformatica.NTSBarButtonItem()
    Me.tlbImpostazioni = New NTSInformatica.NTSBarButtonItem()
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
    Me.tlbStrumenti = New NTSInformatica.NTSBarSubItem()
    Me.tlbSeleziona = New NTSInformatica.NTSBarButtonItem()
    Me.tlbDeseleziona = New NTSInformatica.NTSBarButtonItem()
    Me.tlbFileRettificaGiacenze = New NTSInformatica.NTSBarMenuItem()
    Me.tlbConfrontaGiacenze = New NTSInformatica.NTSBarMenuItem()
    Me.tlbLavorati = New NTSInformatica.NTSBarButtonItem()
    Me.tlbScarica = New NTSInformatica.NTSBarButtonItem()
    Me.tlbCancellaRiga = New NTSInformatica.NTSBarButtonItem()
    Me.tlbElabora = New NTSInformatica.NTSBarButtonItem()
    Me.fmConnettore = New NTSInformatica.NTSGroupBox()
    Me.grCoop = New NTSInformatica.NTSGrid()
    Me.grvCoop = New NTSInformatica.NTSGridView()
    Me.tb_codice = New NTSInformatica.NTSGridColumn()
    Me.tb_batch = New NTSInformatica.NTSGridColumn()
    Me.tb_startstop = New NTSInformatica.NTSGridColumn()
    Me.tb_stato = New NTSInformatica.NTSGridColumn()
    Me.RepositoryItemRadioGroup1 = New DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup()
    Me.RepositoryItemButtonEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
    Me.RepositoryItemToggleSwitch1 = New DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch()
    Me.RepositoryItemToggleSwitch2 = New DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch()
    Me.RepositoryItemToggleSwitch3 = New DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch()
    Me.RepositoryItemZoomTrackBar1 = New DevExpress.XtraEditors.Repository.RepositoryItemZoomTrackBar()
    Me.RepositoryItemTrackBar1 = New DevExpress.XtraEditors.Repository.RepositoryItemTrackBar()
    Me.RepositoryItemTokenEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTokenEdit()
    Me.RepositoryItemToggleSwitch4 = New DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch()
    Me.pbElabora1 = New NTSInformatica.NTSProgressBar()
    Me.NtsGridView1 = New NTSInformatica.NTSGridView()
    Me.NtsGridView2 = New NTSInformatica.NTSGridView()
    Me.NtsGridView3 = New NTSInformatica.NTSGridView()
    Me.NtsGridView4 = New NTSInformatica.NTSGridView()
    Me.NtsGridView5 = New NTSInformatica.NTSGridView()
    Me.timerElabora = New System.Windows.Forms.Timer()
    Me.timerConnect = New System.Windows.Forms.Timer()
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fmConnettore, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmConnettore.SuspendLayout()
    CType(Me.grCoop, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grvCoop, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemRadioGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemToggleSwitch1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemToggleSwitch2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemToggleSwitch3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemZoomTrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemTrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemTokenEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RepositoryItemToggleSwitch4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pbElabora1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsGridView4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsGridView5, System.ComponentModel.ISupportInitialize).BeginInit()
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
    Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbGuida, Me.tlbEsci, Me.tlbRicaricaGriglia, Me.tlbCambioDitta, Me.tlbConverti, Me.NtsBarButtonItem1, Me.tlbStrumenti, Me.tlbSeleziona, Me.tlbDeseleziona, Me.tlbZoom, Me.tlbLavorati, Me.tlbScarica, Me.tlbCancellaRiga, Me.tlbElabora, Me.tlbFileRettificaGiacenze, Me.tlbConfrontaGiacenze, Me.tlbCerca, Me.tlbImpostazioni})
    Me.NtsBarManager1.MaxItemId = 53
    '
    'tlbMain
    '
    Me.tlbMain.BarName = "tlbMain"
    Me.tlbMain.DockCol = 0
    Me.tlbMain.DockRow = 0
    Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
    Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCerca, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbImpostazioni, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbZoom)})
    Me.tlbMain.OptionsBar.AllowQuickCustomization = False
    Me.tlbMain.OptionsBar.DisableClose = True
    Me.tlbMain.OptionsBar.DrawDragBorder = False
    Me.tlbMain.OptionsBar.UseWholeRow = True
    Me.tlbMain.Text = "tlbMain"
    '
    'tlbCerca
    '
    Me.tlbCerca.Caption = "Cerca"
    Me.tlbCerca.Id = 51
    Me.tlbCerca.Name = "tlbCerca"
    Me.tlbCerca.Visible = True
    '
    'tlbImpostazioni
    '
    Me.tlbImpostazioni.Caption = "Impostazioni"
    Me.tlbImpostazioni.Id = 52
    Me.tlbImpostazioni.Name = "tlbImpostazioni"
    Me.tlbImpostazioni.Visible = True
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
    Me.barDockControlBottom.Location = New System.Drawing.Point(0, 511)
    Me.barDockControlBottom.Size = New System.Drawing.Size(684, 0)
    '
    'barDockControlLeft
    '
    Me.barDockControlLeft.CausesValidation = False
    Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
    Me.barDockControlLeft.Location = New System.Drawing.Point(0, 35)
    Me.barDockControlLeft.Size = New System.Drawing.Size(0, 476)
    '
    'barDockControlRight
    '
    Me.barDockControlRight.CausesValidation = False
    Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
    Me.barDockControlRight.Location = New System.Drawing.Point(684, 35)
    Me.barDockControlRight.Size = New System.Drawing.Size(0, 476)
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
    'tlbLavorati
    '
    Me.tlbLavorati.Caption = "Lavorati"
    Me.tlbLavorati.Glyph = Global.NTSInformatica.My.Resources.Resources.Doc_3
    Me.tlbLavorati.Id = 41
    Me.tlbLavorati.Name = "tlbLavorati"
    Me.tlbLavorati.Visible = True
    '
    'tlbScarica
    '
    Me.tlbScarica.Caption = "Scarica"
    Me.tlbScarica.Id = 44
    Me.tlbScarica.Name = "tlbScarica"
    Me.tlbScarica.Visible = True
    '
    'tlbCancellaRiga
    '
    Me.tlbCancellaRiga.Caption = "Cancella Riga"
    Me.tlbCancellaRiga.Id = 47
    Me.tlbCancellaRiga.Name = "tlbCancellaRiga"
    Me.tlbCancellaRiga.Visible = True
    '
    'tlbElabora
    '
    Me.tlbElabora.Caption = "Elabora"
    Me.tlbElabora.Id = 48
    Me.tlbElabora.Name = "tlbElabora"
    Me.tlbElabora.Visible = True
    '
    'fmConnettore
    '
    Me.fmConnettore.AllowDrop = True
    Me.fmConnettore.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmConnettore.Appearance.Options.UseBackColor = True
    Me.fmConnettore.Controls.Add(Me.grCoop)
    Me.fmConnettore.Dock = System.Windows.Forms.DockStyle.Fill
    Me.fmConnettore.Location = New System.Drawing.Point(0, 35)
    Me.fmConnettore.Name = "fmConnettore"
    Me.fmConnettore.Size = New System.Drawing.Size(684, 476)
    Me.fmConnettore.Text = "CONNETTORE"
    '
    'grCoop
    '
    Me.grCoop.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grCoop.Location = New System.Drawing.Point(2, 21)
    Me.grCoop.MainView = Me.grvCoop
    Me.grCoop.MenuManager = Me.NtsBarManager1
    Me.grCoop.Name = "grCoop"
    Me.grCoop.Size = New System.Drawing.Size(680, 453)
    Me.grCoop.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvCoop})
    '
    'grvCoop
    '
    Me.grvCoop.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.tb_codice, Me.tb_batch, Me.tb_stato, Me.tb_startstop})
    Me.grvCoop.Enabled = True
    Me.grvCoop.GridControl = Me.grCoop
    Me.grvCoop.Name = "grvCoop"
    Me.grvCoop.OptionsCustomization.AllowRowSizing = True
    Me.grvCoop.OptionsNavigation.EnterMoveNextColumn = True
    Me.grvCoop.OptionsNavigation.UseTabKey = False
    Me.grvCoop.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.grvCoop.OptionsView.ColumnAutoWidth = False
    Me.grvCoop.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.grvCoop.OptionsView.ShowGroupPanel = False
    '
    'tb_codice
    '
    Me.tb_codice.AppearanceCell.Options.UseBackColor = True
    Me.tb_codice.AppearanceCell.Options.UseTextOptions = True
    Me.tb_codice.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_codice.Caption = "Codice"
    Me.tb_codice.Enabled = False
    Me.tb_codice.FieldName = "tb_codice"
    Me.tb_codice.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_codice.Name = "tb_codice"
    Me.tb_codice.OptionsColumn.AllowEdit = False
    Me.tb_codice.OptionsColumn.ReadOnly = True
    Me.tb_codice.Visible = True
    Me.tb_codice.VisibleIndex = 0
    '
    'tb_batch
    '
    Me.tb_batch.AppearanceCell.Options.UseBackColor = True
    Me.tb_batch.AppearanceCell.Options.UseTextOptions = True
    Me.tb_batch.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_batch.Caption = "Batch"
    Me.tb_batch.Enabled = True
    Me.tb_batch.FieldName = "tb_batch"
    Me.tb_batch.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_batch.Name = "tb_batch"
    Me.tb_batch.Visible = True
    Me.tb_batch.VisibleIndex = 1
    '
    'tb_startstop
    '
    Me.tb_startstop.AppearanceCell.Options.UseBackColor = True
    Me.tb_startstop.AppearanceCell.Options.UseTextOptions = True
    Me.tb_startstop.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_startstop.Caption = "Start\Stop"
    Me.tb_startstop.Enabled = False
    Me.tb_startstop.FieldName = "tb_startstop"
    Me.tb_startstop.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_startstop.Name = "tb_startstop"
    Me.tb_startstop.OptionsColumn.AllowEdit = False
    Me.tb_startstop.OptionsColumn.ReadOnly = True
    Me.tb_startstop.Visible = True
    Me.tb_startstop.VisibleIndex = 3
    '
    'tb_stato
    '
    Me.tb_stato.AppearanceCell.Options.UseBackColor = True
    Me.tb_stato.AppearanceCell.Options.UseTextOptions = True
    Me.tb_stato.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_stato.Caption = "Stato"
    Me.tb_stato.Enabled = False
    Me.tb_stato.FieldName = "tb_stato"
    Me.tb_stato.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_stato.Name = "tb_stato"
    Me.tb_stato.OptionsColumn.AllowEdit = False
    Me.tb_stato.OptionsColumn.ReadOnly = True
    Me.tb_stato.Visible = True
    Me.tb_stato.VisibleIndex = 2
    '
    'RepositoryItemRadioGroup1
    '
    Me.RepositoryItemRadioGroup1.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(), New DevExpress.XtraEditors.Controls.RadioGroupItem()})
    Me.RepositoryItemRadioGroup1.Name = "RepositoryItemRadioGroup1"
    '
    'RepositoryItemButtonEdit1
    '
    Me.RepositoryItemButtonEdit1.AutoHeight = False
    Me.RepositoryItemButtonEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.RepositoryItemButtonEdit1.Name = "RepositoryItemButtonEdit1"
    '
    'RepositoryItemToggleSwitch1
    '
    Me.RepositoryItemToggleSwitch1.AutoHeight = False
    Me.RepositoryItemToggleSwitch1.Name = "RepositoryItemToggleSwitch1"
    Me.RepositoryItemToggleSwitch1.OffText = "Off"
    Me.RepositoryItemToggleSwitch1.OnText = "On"
    '
    'RepositoryItemToggleSwitch2
    '
    Me.RepositoryItemToggleSwitch2.AutoHeight = False
    Me.RepositoryItemToggleSwitch2.Name = "RepositoryItemToggleSwitch2"
    Me.RepositoryItemToggleSwitch2.OffText = "Off"
    Me.RepositoryItemToggleSwitch2.OnText = "On"
    '
    'RepositoryItemToggleSwitch3
    '
    Me.RepositoryItemToggleSwitch3.AutoHeight = False
    Me.RepositoryItemToggleSwitch3.Name = "RepositoryItemToggleSwitch3"
    Me.RepositoryItemToggleSwitch3.NullText = "False"
    Me.RepositoryItemToggleSwitch3.OffText = "False"
    Me.RepositoryItemToggleSwitch3.OnText = "True"
    '
    'RepositoryItemZoomTrackBar1
    '
    Me.RepositoryItemZoomTrackBar1.Middle = 5
    Me.RepositoryItemZoomTrackBar1.Name = "RepositoryItemZoomTrackBar1"
    Me.RepositoryItemZoomTrackBar1.ScrollThumbStyle = DevExpress.XtraEditors.Repository.ScrollThumbStyle.ArrowDownRight
    '
    'RepositoryItemTrackBar1
    '
    Me.RepositoryItemTrackBar1.LabelAppearance.Options.UseTextOptions = True
    Me.RepositoryItemTrackBar1.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    Me.RepositoryItemTrackBar1.Name = "RepositoryItemTrackBar1"
    '
    'RepositoryItemTokenEdit1
    '
    Me.RepositoryItemTokenEdit1.Name = "RepositoryItemTokenEdit1"
    '
    'RepositoryItemToggleSwitch4
    '
    Me.RepositoryItemToggleSwitch4.AutoHeight = False
    Me.RepositoryItemToggleSwitch4.Name = "RepositoryItemToggleSwitch4"
    Me.RepositoryItemToggleSwitch4.OffText = "Off"
    Me.RepositoryItemToggleSwitch4.OnText = "On"
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
    'timerElabora
    '
    Me.timerElabora.Interval = 1000
    '
    'timerConnect
    '
    '
    'FRMHHCOOP
    '
    Me.ClientSize = New System.Drawing.Size(684, 511)
    Me.Controls.Add(Me.fmConnettore)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Name = "FRMHHCOOP"
    Me.Text = "CONNETTORE OPC-UA"
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fmConnettore, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmConnettore.ResumeLayout(False)
    CType(Me.grCoop, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grvCoop, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemRadioGroup1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemToggleSwitch1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemToggleSwitch2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemToggleSwitch3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemZoomTrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemTrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemTokenEdit1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RepositoryItemToggleSwitch4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pbElabora1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsGridView1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsGridView2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsGridView3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsGridView4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsGridView5, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

  Public Overridable Sub InitControls()
    '----------------------------------------------------------------------------------------------------------------
    InitControlsBeginEndInit(Me, False)
    '----------------------------------------------------------------------------------------------------------------
    Try
      '--------------------------------------------------------------------------------------------------------------
      Try
        tlbCerca.GlyphPath = (oApp.ChildImageDir & "\Zoom.png")
        tlbImpostazioni.GlyphPath = (oApp.ChildImageDir & "\chiaveinglese.png")
        tlbGuida.GlyphPath = (oApp.ChildImageDir & "\help.png")
        tlbEsci.GlyphPath = (oApp.ChildImageDir & "\exit.png")
      Catch ex As Exception
      End Try
      '--------------------------------------------------------------------------------------------------------------
      tlbMain.NTSSetToolTip()
      '--------------------------------------------------------------------------------------------------------------
      NTSScriptExec("InitControls", Me, Nothing)
      '-------------------------------------------------

      '--------------------------------------------------
      '--- Griglia Macchine
      '--------------------------------------------------
      grvCoop.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964289, "Elenco Macchine"))
      grvCoop.NTSAllowInsert = False
      'grvCoop.NTSHideMenuSx()

      Dim dttStartStop As New DataTable
      dttStartStop.Columns.Add("cod")
      dttStartStop.Columns.Add("val")
      dttStartStop.Rows.Add(0, "STOP")
      dttStartStop.Rows.Add(1, "START")

      Dim dttStato As New DataTable
      dttStato.Columns.Add("cod")
      dttStato.Columns.Add("val")
      dttStato.Rows.Add(0, "DISCONNESSO")
      dttStato.Rows.Add(1, "CONNESSO")

      tb_codice.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130995879126964290, tb_codice.Caption), 255, True)
      tb_batch.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130995879126964291, tb_batch.Caption), 255, True)
      tb_stato.NTSSetParamCMB(oMenu, oApp.Tr(Me, 130995879126964292, tb_stato.Caption), dttStato, "val", "cod")
      tb_startstop.NTSSetParamCMB(oMenu, oApp.Tr(Me, 130995879126964293, tb_startstop.Caption), dttStartStop, "val", "cod")

      grvCoop.AddColumnBackColor("backcolor_tb_startstop")
      grvCoop.AddColumnBackColor("backcolor_tb_stato")

      tb_batch.NTSSetRichiesto()

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
        Case "INFO_LABEL"
        Case "TIMER_ELABORA_INTERVAL"
          timerElabora.Interval = NTSCInt(e.Message)
      End Select

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Eventi Form"

  Public Overridable Sub FRMHHCOOP_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Try

      '--------------------------------------------------------------------------------------------------------------
      InitControls()
      '--------------------------------------------------------------------------------------------------------------
      GctlSetRoules()
      '--------------------------------------------------------------------------------------------------------------

      oCleCoop.LeggiRegistro()

      NTSFormClearDataBinding(Me)

      'If Not oApp.Batch Then ApriTesta()

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub FRMHHCOOP_ActivatedFirst(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ActivatedFirst
    Try
      '--------------------------------------------------------------------------------------------------------------
      '--------------------------------------------------------------------------------------------------------------
      GctlApplicaDefaultValue()

      oCleCoop.strModalita = ""

      If Not oCleCoop.IstanziaHHIopc() Then Return

      'btnAvvia.Enabled = True
      timerConnect.Interval = 100
      timerConnect.Stop()
      timerElabora.Interval = 100
      timerElabora.Stop()
      If oApp.Batch Then

        Dim strBub As String = File.ReadAllText(oApp.AvvioProgrammaParametri)

        If UCase(strBub) = "AUTOMATICO" Then
          LanciaAtomatico()
          Me.Close()
        Else
          Dim strMacchina As String = strBub
          oCleCoop.oCleIopc.strMacchina = strMacchina
          Me.Hide()
          oCleCoop.ApriMacchine(dsCoop)
          VerificaMacchina(strMacchina)
          timerConnect.Start()
          timerElabora.Start()
        End If
      Else
        Apri()
        timerConnect.Start()
      End If

      'timerConnect.Start()
      'timerElabora.Start()

      '--------------------------------------------------------------------------------------------------------------
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub FRMHHCOOP_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    Try

      If Not oCleCoop.SalvaMacchine(dsCoop) Then
        oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! Salvataggio non andato a buon fine"))
        e.Cancel = True
        Return
      End If

      If oApp.Batch Then oCleCoop.ChiudiOPCUA()
      'taskIcon.Visible = False

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Eventi Toolbar"

  Public Overridable Sub tlbCerca_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbCerca.ItemClick
    Try
      Apri()
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub tlbImpostazioni_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbImpostazioni.ItemClick
    Dim strParam As String = ""
    Dim oCallParams As New CLE__CLDP

    Try
      oMenu.RunChild("BSHHIOPC", "CLSHHIOPC", "", DittaCorrente, "", "", oCallParams, strParam, True, True)
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub tlbEsci_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbEsci.ItemClick
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
      'zoom standard di textbox e griglia
      NTSCallStandardZoom()

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Eventi Timer"

  Public Overridable Sub timerConnect_Tick(sender As Object, e As EventArgs) Handles timerConnect.Tick
    Try
      timerConnect.Stop()

      If bStopTimerConnect Then
        timerConnect.Stop()
        Return
      End If

      timerConnect.Interval = 5000

      If oApp.Batch Then
        If bCompleteConnect Then
          Dim bgWorkerConnect As New BackgroundWorker
          AddHandler bgWorkerConnect.DoWork, AddressOf bgWorkerConnect_DoWork
          AddHandler bgWorkerConnect.RunWorkerCompleted, AddressOf bgWorkerConnect_RunWorkerCompleted
          bgWorkerConnect.RunWorkerAsync()
        End If
      Else
        Dim ds As New DataSet
        oCleCoop.ApriMacchine(ds)
        For Each drMacchine As DataRow In ds.Tables("MACCHINE").Rows
          If dsCoop.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(drMacchine!tb_codice)).Length > 0 Then
            If NTSCInt(drMacchine!tb_startstop) = 0 Then
              dsCoop.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(drMacchine!tb_codice))(0) !tb_stato = 0
            Else
              dsCoop.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(drMacchine!tb_codice))(0) !tb_stato = NTSCInt(drMacchine!tb_stato)
            End If
          End If
        Next
      End If

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      timerConnect.Start()
    End Try
  End Sub

  Public Overridable Sub timerElabora_Tick(sender As Object, e As EventArgs) Handles timerElabora.Tick
    Try
      timerElabora.Stop()

      If bStopTimerElabora Then
        timerElabora.Stop()
        Return
      End If

      oCleCoop.GestisciTimerElabora()

    Catch ex As Exception

    Finally
      timerElabora.Start()
    End Try
  End Sub

#End Region

#Region "Eventi Backgroudworker"

  Public Overridable Sub bgWorkerConnect_DoWork(sender As Object, e As DoWorkEventArgs)
    Try
      bCompleteConnect = False

      'oCleCoop.ApriMacchine(dsCoop)

      If dsCoop Is Nothing Then Return
      If dsCoop.Tables("TESTA") Is Nothing Then Return
      If dsCoop.Tables("TESTA").Rows.Count = 0 Then Return

      If bStopTimerConnect Then Return

      If oCleCoop.ConnettiOPCUA() Then
        'labStato.BackColor = Color.Green
        'labStato.Text = "CONNESSO"
        'If oApp.Batch Then cmTaskStato.Image = My.Resources.PVerde_20
        nTentativi = 0
        oCleCoop.CambiaStato(1)
      Else
        If Not oCleCoop.LogStart("BNHHCOOP", "Connessione OPC-UA" & vbNewLine, True) Then Return
        oCleCoop.LogWrite("Errore di connessione per la macchina " & oCleCoop.oCleIopc.strMacchina, True)
        oCleCoop.LogStop()
        'labStato.BackColor = Color.Red
        'labStato.Text = "DISCONNESSO"
        'If oApp.Batch Then cmTaskStato.Image = My.Resources.PRosso_20
        oCleCoop.CambiaStato(0)
        If nTentativi = 3 Then
          bStopTimerConnect = True
          bStopTimerElabora = True
          LanciaRestart(oCleCoop.oCleIopc.strMacchina)
        End If
        nTentativi += 1
      End If

    Catch ex As Exception
    End Try
  End Sub

  Public Overridable Sub bgWorkerConnect_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
    Try

    Catch ex As Exception
    Finally
      bCompleteConnect = True
    End Try
  End Sub

#End Region

#Region "Griglia"

  Public Overridable Sub grvCoop_NTSBeforeRowUpdate(sender As Object, e As DevExpress.XtraGrid.Views.Base.RowAllowEventArgs) Handles grvCoop.NTSBeforeRowUpdate
    Try
      oCleCoop.SalvaMacchine(dsCoop)
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub grvCoop_NTSFocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles grvCoop.NTSFocusedRowChanged
    Try
      If grvCoop Is Nothing Then Return

      If grvCoop.FocusedRowHandle = NTSGrid.AutoFilterRowHandle Then Return

      If NTSCInt(grvCoop.GetFocusedDataRow!tb_startstop) = 1 Then
        'GctlSetVisEnab(tb_batch, False)
        tb_batch.Enabled = False
      Else
        'GctlSetVisEnab(tb_batch, True)
        tb_batch.Enabled = True
      End If

      'If grvCoop.FocusedRowHandle = NTSGridView.NESSUNA_RIGA Then Return
      'If Not grvCoop.FocusedRowHandle = NTSGridView.NUOVA_RIGA Then
      '  Select Case grvCoop.GetDataRow(grvCoop.FocusedRowHandle).RowState
      '    Case DataRowState.Added
      '      If Not tb_batch.Enabled Then
      '        GctlSetVisEnab(tb_batch, False)
      '        '        GctlSetVisEnab(tb_tipo, False)
      '      End If
      '    Case Else
      '      tb_batch.Enabled = False
      '      '      tb_tipo.Enabled = False
      '  End Select
      'Else
      '  If Not tb_batch.Enabled Then
      '    GctlSetVisEnab(tb_batch, False)
      '    '    GctlSetVisEnab(tb_tipo, False)
      '  End If
      '  grvCoop.NTSMoveFirstColunn()
      'End If
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub grvCoop_DoubleClick(sender As Object, e As EventArgs) Handles grvCoop.DoubleClick
    Try

      If grvCoop.GetFocusedDataRow Is Nothing Then Return
      Dim drMacchine As DataRow = grvCoop.GetFocusedDataRow

      If Not grvCoop.FocusedColumn.Equals(CType(tb_startstop, DevExpress.XtraGrid.Columns.GridColumn)) Then Return

      'MsgBox(Environment.MachineName)
      If UCase(oCleCoop.strNomeServer) <> UCase(Environment.MachineName) Then
        If Not oApp.Batch Then oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! non è possibile far partire o arrestare i processi su una macchina diversa dal server presente in Opzioni di Registro HHNomeServer"))
        Return
      End If

      'endprocess("192.168.56.1", "EXCEL.exe")
      ''endprocess("10.10.1.94", "EXCEL.exe")
      ''endprocess("10.10.1.127", "EXCEL.exe")

      ''Dim strProcesso As String = ""
      ''Dim strNomeMacchina As String = ""
      'Dim processes As System.Diagnostics.Process()

      ''Dim pp As Process
      ''Try
      ''  File.WriteAllText("\\PORT-CARBINI\Condivisa\TEST\" & NTSCStr(drMacchine!tb_codice) & ".txt", NTSCStr(drMacchine!tb_codice))
      ''  pp = Process.Start("\\PORT-CARBINI\Condivisa\TEST\" & NTSCStr(drMacchine!tb_codice) & ".txt")
      ''Catch ex As Exception
      ''End Try

      ''Try
      ''  '56.1
      ''  processes = System.Diagnostics.Process.GetProcessesByName("notepad", "192.168.56.1")

      'processes = System.Diagnostics.Process.GetProcesses("192.168.56.1")
      ''  processes = System.Diagnostics.Process.GetProcesses("PORT-CARBINI")

      ''  For Each p As System.Diagnostics.Process In processes
      ''    strProcesso = p.ProcessName
      ''    strNomeMacchina = p.MachineName
      ''  Next
      ''Catch ex As Exception
      ''End Try

      'processes = System.Diagnostics.Process.GetProcesses("PORT-CARBINI")

      'Return

      If NTSCInt(drMacchine!tb_startstop) = 1 Then

        If NTSCInt(drMacchine!tb_idprocess) > 0 Then
          Try
            Process.GetProcessById(NTSCInt(drMacchine!tb_idprocess)).Kill()
          Catch ex As Exception
          End Try
        End If

        drMacchine!tb_startstop = 0
        drMacchine!tb_idprocess = 0
        drMacchine!tb_stato = 0
        drMacchine!backcolor_tb_startstop = Color.Crimson.ToArgb
        drMacchine!backcolor_tb_stato = Color.Crimson.ToArgb

        tb_batch.Enabled = True

      Else

        If NTSCStr(drMacchine!tb_batch) = "" Then
          oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! il batch non può essere vuoto"))
          Return
        End If

        Dim dll As String = oApp.NetDir & "\" & NTSCStr(drMacchine!tb_batch) & ".dll"

        If Not File.Exists(dll) Then
          oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! il batch non esiste come DLL nelle cartelle di Business"))
          Return
        End If

        If Not Directory.Exists(oApp.AscDir) Then
          oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! Directory Asc non inesistente " & oApp.AscDir))
          Return
        End If

        File.WriteAllText(oApp.AscDir & "\" & NTSCStr(drMacchine!tb_codice) & ".bub", NTSCStr(drMacchine!tb_codice))

        Dim strStartUpParams As String = ""
        Dim strUtente As String = oApp.User.Nome
        Dim strPassword As String = oApp.User.Pwd
        If strUtente = "" Then strUtente = "."
        If strPassword = "" Then strPassword = "."

        strStartUpParams = strUtente & " " & strPassword & " " & oApp.Ditta & " " & oApp.Profilo

        Dim p As Process = Process.Start(oApp.NetDir & "\buscube.exe", strStartUpParams & " " & NTSCStr(drMacchine!tb_batch) & " /B " & oApp.AscDir & "\" & NTSCStr(drMacchine!tb_codice) & ".bub " & NTSCStr(oCleCoop.strDittaCorrente))

        drMacchine!tb_startstop = 1
        drMacchine!tb_idprocess = p.Id
        drMacchine!backcolor_tb_startstop = Color.LimeGreen.ToArgb

        tb_batch.Enabled = False

      End If

      oCleCoop.SalvaMacchine(dsCoop)

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Funzioni"

  Public Overridable Sub Apri()
    Try

      oCleCoop.oCleIopc.strMacchina = ""
      oCleCoop.ApriMacchine(dsCoop)

      dcCoop = New BindingSource
      dcCoop.DataSource = dsCoop.Tables("MACCHINE")
      dsCoop.Tables("MACCHINE").AcceptChanges()

      grCoop.DataSource = dcCoop

      If oApp.Batch Then Return

      dsCoop.Tables("MACCHINE").Columns.Add("backcolor_tb_startstop", GetType(Integer))
      dsCoop.Tables("MACCHINE").Columns.Add("backcolor_tb_stato", GetType(Integer))

      For Each drMacchine As DataRow In dsCoop.Tables("MACCHINE").Rows
        If NTSCInt(drMacchine!tb_startstop) = 1 Then

          If NTSCInt(drMacchine!tb_idprocess) > 0 Then
            Try
              Process.GetProcessById(NTSCInt(drMacchine!tb_idprocess))
            Catch ex As Exception
              drMacchine!tb_startstop = 0
              drMacchine!tb_idprocess = 0
              drMacchine!tb_stato = 0
              drMacchine!backcolor_tb_startstop = Color.Crimson.ToArgb
              drMacchine!backcolor_tb_stato = Color.Crimson.ToArgb
              oCleCoop.SalvaMacchine(dsCoop)
              Continue For
            End Try
          End If

          drMacchine!backcolor_tb_startstop = Color.LimeGreen.ToArgb

        Else
          drMacchine!tb_stato = 0
          drMacchine!backcolor_tb_startstop = Color.Crimson.ToArgb
          drMacchine!backcolor_tb_stato = Color.Crimson.ToArgb
        End If
      Next

      For Each drMacchine As DataRow In dsCoop.Tables("MACCHINE").Rows
        If NTSCInt(drMacchine!tb_stato) = 1 Then
          drMacchine!backcolor_tb_stato = Color.LimeGreen.ToArgb
        Else
          drMacchine!backcolor_tb_stato = Color.Crimson.ToArgb
        End If
      Next

      If Not grvCoop.GetFocusedDataRow Is Nothing Then
        If NTSCInt(grvCoop.GetFocusedDataRow!tb_startstop) = 1 Then
          tb_batch.Enabled = False
        Else
          tb_batch.Enabled = True
        End If
      End If

      dsCoop.Tables("MACCHINE").AcceptChanges()

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub LanciaAtomatico()
    Try

      If Not oApp.Batch Then Return

      If UCase(oCleCoop.strNomeServer) <> UCase(Environment.MachineName) Then Return

      oCleCoop.oCleIopc.strMacchina = ""
      oCleCoop.ApriMacchine(dsCoop)

      For Each drMacchine As DataRow In dsCoop.Tables("MACCHINE").Rows
        If NTSCInt(drMacchine!tb_startstop) = 1 Then

          If NTSCInt(drMacchine!tb_idprocess) > 0 Then
            Try
              Process.GetProcessById(NTSCInt(drMacchine!tb_idprocess))
            Catch ex As Exception

              If NTSCStr(drMacchine!tb_batch) = "" Then
                'oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! il batch non può essere vuoto"))
                Continue For
              End If

              Dim dll As String = oApp.NetDir & "\" & NTSCStr(drMacchine!tb_batch) & ".dll"

              If Not File.Exists(dll) Then
                'oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! il batch non esiste come DLL nelle cartelle di Business"))
                Continue For
              End If

              If Not Directory.Exists(oApp.AscDir) Then
                'oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! Directory Asc non inesistente " & oApp.AscDir))
                Continue For
              End If

              File.WriteAllText(oApp.AscDir & "\" & NTSCStr(drMacchine!tb_codice) & ".bub", NTSCStr(drMacchine!tb_codice))

              Dim strStartUpParams As String = ""
              Dim strUtente As String = oApp.User.Nome
              Dim strPassword As String = oApp.User.Pwd
              If strUtente = "" Then strUtente = "."
              If strPassword = "" Then strPassword = "."

              strStartUpParams = strUtente & " " & strPassword & " " & oApp.Ditta & " " & oApp.Profilo

              Dim p As Process = Process.Start(oApp.NetDir & "\buscube.exe", strStartUpParams & " " & NTSCStr(drMacchine!tb_batch) & " /B " & oApp.AscDir & "\" & NTSCStr(drMacchine!tb_codice) & ".bub " & NTSCStr(oCleCoop.strDittaCorrente))

              drMacchine!tb_startstop = 1
              drMacchine!tb_idprocess = p.Id

              oCleCoop.SalvaMacchine(dsCoop)
              dsCoop.Tables("MACCHINE").AcceptChanges()
              Continue For
            End Try
          End If
        Else
          Try
            Process.GetProcessById(NTSCInt(drMacchine!tb_idprocess))
            Process.GetProcessById(NTSCInt(drMacchine!tb_idprocess)).Kill()
          Catch ex As Exception
          End Try
          drMacchine!tb_stato = 0
          drMacchine!tb_idprocess = 0
          drMacchine!tb_stato = 0
          oCleCoop.SalvaMacchine(dsCoop)
          dsCoop.Tables("MACCHINE").AcceptChanges()
        End If
      Next

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub LanciaRestart(strMacchina As String)
    Try

      If Not oApp.Batch Then Return

      If UCase(oCleCoop.strNomeServer) <> UCase(Environment.MachineName) Then Return

      'oCleCoop.oCleIopc.strMacchina = ""
      oCleCoop.ApriMacchine(dsCoop)

      For Each drMacchine As DataRow In dsCoop.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(strMacchina))

        If NTSCStr(drMacchine!tb_batch) = "" Then
          'oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! il batch non può essere vuoto"))
          Continue For
        End If

        Dim dll As String = oApp.NetDir & "\" & NTSCStr(drMacchine!tb_batch) & ".dll"

        If Not File.Exists(dll) Then
          'oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! il batch non esiste come DLL nelle cartelle di Business"))
          Continue For
        End If

        If Not Directory.Exists(oApp.AscDir) Then
          'oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Attenzione! Directory Asc non inesistente " & oApp.AscDir))
          Continue For
        End If

        File.WriteAllText(oApp.AscDir & "\" & NTSCStr(drMacchine!tb_codice) & ".bub", NTSCStr(drMacchine!tb_codice))

        Dim strStartUpParams As String = ""
        Dim strUtente As String = oApp.User.Nome
        Dim strPassword As String = oApp.User.Pwd
        If strUtente = "" Then strUtente = "."
        If strPassword = "" Then strPassword = "."

        strStartUpParams = strUtente & " " & strPassword & " " & oApp.Ditta & " " & oApp.Profilo

        Dim p As Process = Process.Start(oApp.NetDir & "\buscube.exe", strStartUpParams & " " & NTSCStr(drMacchine!tb_batch) & " /B " & oApp.AscDir & "\" & NTSCStr(drMacchine!tb_codice) & ".bub " & NTSCStr(oCleCoop.strDittaCorrente))

        drMacchine!tb_startstop = 1
        drMacchine!tb_restart = p.Id
        'drMacchine!tb_idprocess = p.Id

        oCleCoop.SalvaMacchine(dsCoop)
        dsCoop.Tables("MACCHINE").AcceptChanges()

      Next

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub VerificaMacchina(strMacchina As String)
    Try

      If dsCoop.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(strMacchina)).Length > 0 Then
        Dim drMacchina As DataRow = dsCoop.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(strMacchina))(0)
        If NTSCInt(drMacchina!tb_restart) > 0 Then

          Dim nId As Integer = NTSCInt(drMacchina!tb_restart)

          drMacchina!tb_restart = 0

          If NTSCInt(drMacchina!tb_idprocess) > 0 Then
            Try
              Process.GetProcessById(NTSCInt(drMacchina!tb_idprocess))
              Process.GetProcessById(NTSCInt(drMacchina!tb_idprocess)).Kill()
            Catch ex As Exception
            End Try
          End If

          drMacchina!tb_idprocess = nId

          oCleCoop.SalvaMacchine(dsCoop)
          dsCoop.AcceptChanges()
        End If
      End If

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

End Class