Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System.IO
Imports System.ComponentModel

Public Class FRMHHCOGI

  Public oCleMoin As CLEHHMOIN
  Public oCallParams As CLE__CLDP

  Public dsCogi As DataSet
  Public dcCogi As BindingSource = New BindingSource

#Region "Dichiarazione Controlli"
  Private components As System.ComponentModel.IContainer
  Public WithEvents NtsBarManager1 As NTSInformatica.NTSBarManager
  Public WithEvents tlbMain As NTSInformatica.NTSBar
  Public WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Public WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Public WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Public WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Public WithEvents tlbElabora As NTSInformatica.NTSBarButtonItem
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
    If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNHHCOGI", "BEHHMOIN", oTmp, strErr, False, "", "") = False Then
      oApp.MsgBoxErr(oApp.Tr(Me, 128423763770716000, "ERRORE in fase di creazione Entity:" & vbCrLf & strErr))
      Return False
    End If
    oCleMoin = CType(oTmp, CLEHHMOIN)

    '--------------------------------------------------
    '--- Aggiunge gestore eventi
    '--------------------------------------------------
    AddHandler oCleMoin.RemoteEvent, AddressOf GestisciEventiEntity

    If oCleMoin.Init(oApp, NTSScript, oMenu.oCleComm, "", False, "", "") = False Then Return False

    Return True
    '----------------------------------------------------------------------------------------------------------------
  End Function

  Public Overridable Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMHHCOGI))
    Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
    Me.tlbMain = New NTSInformatica.NTSBar()
    Me.tlbCerca = New NTSInformatica.NTSBarButtonItem()
    Me.tlbEsci = New NTSInformatica.NTSBarButtonItem()
    Me.tlbZoom = New NTSInformatica.NTSBarButtonItem()
    Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
    Me.tlbElabora = New NTSInformatica.NTSBarButtonItem()
    Me.tlbGuida = New NTSInformatica.NTSBarButtonItem()
    Me.tlbRicaricaGriglia = New NTSInformatica.NTSBarButtonItem()
    Me.tlbCambioDitta = New NTSInformatica.NTSBarButtonItem()
    Me.tlbConverti = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarButtonItem1 = New NTSInformatica.NTSBarButtonItem()
    Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem()
    Me.tlbCaricaSuFTP = New DevExpress.XtraBars.BarButtonItem()
    Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem()
    Me.NtsBarButtonItem5 = New NTSInformatica.NTSBarButtonItem()
    Me.tlbSeleziona = New NTSInformatica.NTSBarButtonItem()
    Me.tlbDeseleziona = New NTSInformatica.NTSBarButtonItem()
    Me.NtsBarButtonItem2 = New NTSInformatica.NTSBarButtonItem()
    Me.NtsBarButtonItem3 = New NTSInformatica.NTSBarButtonItem()
    Me.NtsBarButtonItem4 = New NTSInformatica.NTSBarButtonItem()
    Me.frmFiltri = New NTSInformatica.NTSGroupBox()
    Me.edArticolo = New NTSInformatica.NTSTextBoxStr()
    Me.lbNomeArticolo = New NTSInformatica.NTSLabel()
    Me.lbArticolo = New NTSInformatica.NTSLabel()
    Me.frmArticoli = New NTSInformatica.NTSGroupBox()
    Me.grCogi = New NTSInformatica.NTSGrid()
    Me.grvCogi = New NTSInformatica.NTSGridView()
    Me.tb_articolo = New NTSInformatica.NTSGridColumn()
    Me.tb_descrart = New NTSInformatica.NTSGridColumn()
    Me.tb_quantmacchina = New NTSInformatica.NTSGridColumn()
    Me.tb_quantbus = New NTSInformatica.NTSGridColumn()
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.frmFiltri, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.frmFiltri.SuspendLayout()
    CType(Me.edArticolo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.frmArticoli, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.frmArticoli.SuspendLayout()
    CType(Me.grCogi, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grvCogi, System.ComponentModel.ISupportInitialize).BeginInit()
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
    Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbElabora, Me.tlbGuida, Me.tlbEsci, Me.tlbRicaricaGriglia, Me.tlbCambioDitta, Me.tlbConverti, Me.NtsBarButtonItem1, Me.BarButtonItem1, Me.tlbCaricaSuFTP, Me.BarButtonItem2, Me.tlbCerca, Me.NtsBarButtonItem5, Me.tlbSeleziona, Me.tlbDeseleziona, Me.tlbZoom})
    Me.NtsBarManager1.MaxItemId = 41
    '
    'tlbMain
    '
    Me.tlbMain.BarName = "tlbMain"
    Me.tlbMain.DockCol = 0
    Me.tlbMain.DockRow = 0
    Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
    Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCerca, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbZoom)})
    Me.tlbMain.OptionsBar.AllowQuickCustomization = False
    Me.tlbMain.OptionsBar.DisableClose = True
    Me.tlbMain.OptionsBar.DrawDragBorder = False
    Me.tlbMain.OptionsBar.UseWholeRow = True
    Me.tlbMain.Text = "tlbMain"
    '
    'tlbCerca
    '
    Me.tlbCerca.Caption = "Cerca"
    Me.tlbCerca.Glyph = CType(resources.GetObject("tlbCerca.Glyph"), System.Drawing.Image)
    Me.tlbCerca.Id = 35
    Me.tlbCerca.Name = "tlbCerca"
    Me.tlbCerca.Visible = True
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
    'tlbElabora
    '
    Me.tlbElabora.Caption = "Elabora"
    Me.tlbElabora.Glyph = CType(resources.GetObject("tlbElabora.Glyph"), System.Drawing.Image)
    Me.tlbElabora.Id = 4
    Me.tlbElabora.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F7)
    Me.tlbElabora.Name = "tlbElabora"
    Me.tlbElabora.Visible = True
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
    'BarButtonItem1
    '
    Me.BarButtonItem1.Caption = "BarButtonItem1"
    Me.BarButtonItem1.Id = 30
    Me.BarButtonItem1.Name = "BarButtonItem1"
    '
    'tlbCaricaSuFTP
    '
    Me.tlbCaricaSuFTP.Caption = "Carica su FTP"
    Me.tlbCaricaSuFTP.Glyph = CType(resources.GetObject("tlbCaricaSuFTP.Glyph"), System.Drawing.Image)
    Me.tlbCaricaSuFTP.Id = 31
    Me.tlbCaricaSuFTP.Name = "tlbCaricaSuFTP"
    Me.tlbCaricaSuFTP.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '
    'BarButtonItem2
    '
    Me.BarButtonItem2.Caption = "cerca"
    Me.BarButtonItem2.Id = 33
    Me.BarButtonItem2.Name = "BarButtonItem2"
    '
    'NtsBarButtonItem5
    '
    Me.NtsBarButtonItem5.Caption = "Strumenti"
    Me.NtsBarButtonItem5.Id = 36
    Me.NtsBarButtonItem5.Name = "NtsBarButtonItem5"
    Me.NtsBarButtonItem5.Visible = True
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
    'NtsBarButtonItem2
    '
    Me.NtsBarButtonItem2.Caption = "Esci"
    Me.NtsBarButtonItem2.Glyph = CType(resources.GetObject("NtsBarButtonItem2.Glyph"), System.Drawing.Image)
    Me.NtsBarButtonItem2.Id = 19
    Me.NtsBarButtonItem2.Name = "NtsBarButtonItem2"
    Me.NtsBarButtonItem2.Visible = True
    '
    'NtsBarButtonItem3
    '
    Me.NtsBarButtonItem3.Caption = "Esci"
    Me.NtsBarButtonItem3.Glyph = CType(resources.GetObject("NtsBarButtonItem3.Glyph"), System.Drawing.Image)
    Me.NtsBarButtonItem3.Id = 19
    Me.NtsBarButtonItem3.Name = "NtsBarButtonItem3"
    Me.NtsBarButtonItem3.Visible = True
    '
    'NtsBarButtonItem4
    '
    Me.NtsBarButtonItem4.Caption = "Elabora"
    Me.NtsBarButtonItem4.Glyph = CType(resources.GetObject("NtsBarButtonItem4.Glyph"), System.Drawing.Image)
    Me.NtsBarButtonItem4.Id = 4
    Me.NtsBarButtonItem4.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F7)
    Me.NtsBarButtonItem4.Name = "NtsBarButtonItem4"
    Me.NtsBarButtonItem4.Visible = True
    '
    'frmFiltri
    '
    Me.frmFiltri.AllowDrop = True
    Me.frmFiltri.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.frmFiltri.Appearance.Options.UseBackColor = True
    Me.frmFiltri.Controls.Add(Me.edArticolo)
    Me.frmFiltri.Controls.Add(Me.lbNomeArticolo)
    Me.frmFiltri.Controls.Add(Me.lbArticolo)
    Me.frmFiltri.Dock = System.Windows.Forms.DockStyle.Top
    Me.frmFiltri.Location = New System.Drawing.Point(0, 35)
    Me.frmFiltri.Name = "frmFiltri"
    Me.frmFiltri.Size = New System.Drawing.Size(684, 56)
    Me.frmFiltri.Text = "FILTRI"
    '
    'edArticolo
    '
    Me.edArticolo.EditValue = ""
    Me.edArticolo.Location = New System.Drawing.Point(88, 24)
    Me.edArticolo.Name = "edArticolo"
    Me.edArticolo.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edArticolo.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edArticolo.Properties.AutoHeight = False
    Me.edArticolo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edArticolo.Properties.MaxLength = 65536
    Me.edArticolo.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edArticolo.Size = New System.Drawing.Size(148, 20)
    '
    'lbNomeArticolo
    '
    Me.lbNomeArticolo.AutoEllipsis = True
    Me.lbNomeArticolo.BackColor = System.Drawing.Color.Transparent
    Me.lbNomeArticolo.Location = New System.Drawing.Point(244, 24)
    Me.lbNomeArticolo.Name = "lbNomeArticolo"
    Me.lbNomeArticolo.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.lbNomeArticolo.Size = New System.Drawing.Size(372, 20)
    Me.lbNomeArticolo.UseMnemonic = False
    '
    'lbArticolo
    '
    Me.lbArticolo.BackColor = System.Drawing.Color.Transparent
    Me.lbArticolo.Location = New System.Drawing.Point(4, 24)
    Me.lbArticolo.Name = "lbArticolo"
    Me.lbArticolo.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbArticolo.Size = New System.Drawing.Size(80, 20)
    Me.lbArticolo.Text = "Articolo"
    Me.lbArticolo.UseMnemonic = False
    '
    'frmArticoli
    '
    Me.frmArticoli.AllowDrop = True
    Me.frmArticoli.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.frmArticoli.Appearance.Options.UseBackColor = True
    Me.frmArticoli.Controls.Add(Me.grCogi)
    Me.frmArticoli.Dock = System.Windows.Forms.DockStyle.Fill
    Me.frmArticoli.Location = New System.Drawing.Point(0, 91)
    Me.frmArticoli.Name = "frmArticoli"
    Me.frmArticoli.Size = New System.Drawing.Size(684, 385)
    Me.frmArticoli.Text = "ARTICOLI"
    '
    'grCogi
    '
    Me.grCogi.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grCogi.Location = New System.Drawing.Point(2, 21)
    Me.grCogi.MainView = Me.grvCogi
    Me.grCogi.MenuManager = Me.NtsBarManager1
    Me.grCogi.Name = "grCogi"
    Me.grCogi.Size = New System.Drawing.Size(680, 362)
    Me.grCogi.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvCogi})
    '
    'grvCogi
    '
    Me.grvCogi.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.tb_articolo, Me.tb_descrart, Me.tb_quantmacchina, Me.tb_quantbus})
    Me.grvCogi.Enabled = True
    Me.grvCogi.GridControl = Me.grCogi
    Me.grvCogi.Name = "grvCogi"
    Me.grvCogi.OptionsCustomization.AllowRowSizing = True
    Me.grvCogi.OptionsNavigation.EnterMoveNextColumn = True
    Me.grvCogi.OptionsNavigation.UseTabKey = False
    Me.grvCogi.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.grvCogi.OptionsView.ColumnAutoWidth = False
    Me.grvCogi.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.grvCogi.OptionsView.ShowGroupPanel = False
    '
    'tb_articolo
    '
    Me.tb_articolo.AppearanceCell.Options.UseBackColor = True
    Me.tb_articolo.AppearanceCell.Options.UseTextOptions = True
    Me.tb_articolo.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_articolo.Caption = "Articolo"
    Me.tb_articolo.Enabled = False
    Me.tb_articolo.FieldName = "tb_articolo"
    Me.tb_articolo.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_articolo.Name = "tb_articolo"
    Me.tb_articolo.OptionsColumn.AllowEdit = False
    Me.tb_articolo.OptionsColumn.ReadOnly = True
    Me.tb_articolo.Visible = True
    Me.tb_articolo.VisibleIndex = 0
    '
    'tb_descrart
    '
    Me.tb_descrart.AppearanceCell.Options.UseBackColor = True
    Me.tb_descrart.AppearanceCell.Options.UseTextOptions = True
    Me.tb_descrart.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_descrart.Caption = "Descrizione"
    Me.tb_descrart.Enabled = False
    Me.tb_descrart.FieldName = "tb_descrart"
    Me.tb_descrart.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_descrart.Name = "tb_descrart"
    Me.tb_descrart.OptionsColumn.AllowEdit = False
    Me.tb_descrart.OptionsColumn.ReadOnly = True
    Me.tb_descrart.Visible = True
    Me.tb_descrart.VisibleIndex = 1
    '
    'tb_quantmacchina
    '
    Me.tb_quantmacchina.AppearanceCell.Options.UseBackColor = True
    Me.tb_quantmacchina.AppearanceCell.Options.UseTextOptions = True
    Me.tb_quantmacchina.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_quantmacchina.Caption = "Q.tà Macchina"
    Me.tb_quantmacchina.Enabled = False
    Me.tb_quantmacchina.FieldName = "tb_quantmacchina"
    Me.tb_quantmacchina.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_quantmacchina.Name = "tb_quantmacchina"
    Me.tb_quantmacchina.OptionsColumn.AllowEdit = False
    Me.tb_quantmacchina.OptionsColumn.ReadOnly = True
    Me.tb_quantmacchina.Visible = True
    Me.tb_quantmacchina.VisibleIndex = 2
    '
    'tb_quantbus
    '
    Me.tb_quantbus.AppearanceCell.Options.UseBackColor = True
    Me.tb_quantbus.AppearanceCell.Options.UseTextOptions = True
    Me.tb_quantbus.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_quantbus.Caption = "Q.tà Bus."
    Me.tb_quantbus.Enabled = False
    Me.tb_quantbus.FieldName = "tb_quantbus"
    Me.tb_quantbus.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_quantbus.Name = "tb_quantbus"
    Me.tb_quantbus.OptionsColumn.AllowEdit = False
    Me.tb_quantbus.OptionsColumn.ReadOnly = True
    Me.tb_quantbus.Visible = True
    Me.tb_quantbus.VisibleIndex = 3
    '
    'FRMHHCOGI
    '
    Me.ClientSize = New System.Drawing.Size(684, 476)
    Me.Controls.Add(Me.frmArticoli)
    Me.Controls.Add(Me.frmFiltri)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Name = "FRMHHCOGI"
    Me.Text = "CONFRONTA GIACENZE"
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.frmFiltri, System.ComponentModel.ISupportInitialize).EndInit()
    Me.frmFiltri.ResumeLayout(False)
    CType(Me.edArticolo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.frmArticoli, System.ComponentModel.ISupportInitialize).EndInit()
    Me.frmArticoli.ResumeLayout(False)
    CType(Me.grCogi, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grvCogi, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

  Public Overridable Sub InitControls()
    '----------------------------------------------------------------------------------------------------------------
    InitControlsBeginEndInit(Me, False)
    '----------------------------------------------------------------------------------------------------------------
    Try
      '--------------------------------------------------------------------------------------------------------------
      Try
        tlbCerca.GlyphPath = (oApp.ChildImageDir & "\zoom.png")
        tlbEsci.GlyphPath = (oApp.ChildImageDir & "\exit.png")
        tlbGuida.GlyphPath = (oApp.ChildImageDir & "\help.png")
      Catch ex As Exception
      End Try
      '--------------------------------------------------------------------------------------------------------------
      tlbMain.NTSSetToolTip()
      '--------------------------------------------------------------------------------------------------------------
      NTSScriptExec("InitControls", Me, Nothing)

      '--------------------------------------------------------------------------------------------------------------

      edArticolo.NTSSetParamTabe(oMenu, oApp.Tr(Me, 128230023234255739, lbArticolo.Text), tabartico, True)

      'Griglia Cogi

      grvCogi.NTSSetParam(oMenu, oApp.Tr(Me, 128230023234255739, "Lista Articoli"))
      grvCogi.NTSAllowInsert = False
      'grvImdoTesta.NTSHideMenuSx()

      tb_articolo.NTSSetParamSTR(oMenu, oApp.Tr(Me, 128230023234255765, tb_articolo.Caption), 50, True)
      tb_descrart.NTSSetParamSTR(oMenu, oApp.Tr(Me, 128230023234255766, tb_descrart.Caption), 200, True)
      tb_quantmacchina.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234255767, tb_quantmacchina.Caption), "0.00", 11, 0, 99999999999)
      tb_quantbus.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128230023234255768, tb_quantbus.Caption), "0.00", 11, 0, 99999999999)

      grvCogi.AddColumnBackColor("backcolor_tb_quantbus")

      '-------------------------------------------------
      'chiamo lo script per inizializzare i controlli caricati con source ext
      NTSScriptExec("InitControls", Me, Nothing)

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
    '----------------------------------------------------------------------------------------------------------------
    InitControlsBeginEndInit(Me, True)
    '----------------------------------------------------------------------------------------------------------------
  End Sub

  Public Overridable Sub InitEntity(ByRef cleMoin As CLEHHMOIN)
    Try
      oCleMoin = cleMoin
      AddHandler oCleMoin.RemoteEvent, AddressOf GestisciEventiEntity
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
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

      End Select

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Eventi Form"

  Public Overridable Sub FRMHHCOGI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Try
      '--------------------------------------------------------------------------------------------------------------
      InitControls()
      '--------------------------------------------------------------------------------------------------------------
      GctlSetRoules()
      '--------------------------------------------------------------------------------------------------------------

      'oCleMaav.LeggiRegistro()

      'Apri()

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub

  Public Overridable Sub FRMHHCOGI_ActivatedFirst(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ActivatedFirst
    Try
      '--------------------------------------------------------------------------------------------------------------
      '--------------------------------------------------------------------------------------------------------------
      GctlApplicaDefaultValue()
      '--------------------------------------------------------------------------------------------------------------
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
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
    Try
      NTSCallStandardZoom()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Eventi Controlli"

  Public Overridable Sub edArticolo_EditValueChanged(sender As Object, e As EventArgs) Handles edArticolo.EditValueChanged
    Try
      If oCleMoin Is Nothing Then Return

      Dim strDescr As String = ""
      oCleMoin.ValCodcieDb(edArticolo.Text, "artico", "S", strDescr, Nothing)
      lbNomeArticolo.Text = strDescr
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

#End Region

#Region "Procedure"

  Public Overridable Sub Apri()
    Try
      Me.Cursor = Cursors.WaitCursor
      dsCogi = Nothing
      dsCogi = New DataSet
      oCleMoin.LeggiConfrontoGiacenze(edArticolo.Text, dsCogi)
      dcCogi = New BindingSource
      dcCogi.DataSource = dsCogi.Tables("GIAC")
      dsCogi.AcceptChanges()
      grCogi.DataSource = dcCogi
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try

  End Sub

#End Region

End Class