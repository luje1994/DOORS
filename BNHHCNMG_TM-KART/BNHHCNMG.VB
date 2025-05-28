Imports System.Data
Imports System.IO
Imports DevExpress.XtraGrid.Views.Base
Imports NTSInformatica.CLN__STD

Public Class FRMHHCNMG

#Region "Variabili"
  Public oCallParams As CLE__CLDP
  Public ocleCnmg As CLEHHCNMG

  Public dsCnmg As New DataSet
  Public dcCnmg As New BindingSource

  Public strProgramma As String = "BSHHCNMG"

  'Flag per load form
  Public bInLoad As Boolean = False
#End Region

#Region "Dichiarazione Controlli"
  Private components As System.ComponentModel.IContainer
  Public WithEvents NtsBarManager1 As NTSInformatica.NTSBarManager
  Public WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
  Public WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
  Public WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
  Public WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
  Public WithEvents tlbMain As NTSInformatica.NTSBar
  Public WithEvents tlbNuovo As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbDuplica As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbSalva As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbRipristina As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbCancella As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbZoom As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbGuida As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbEsci As NTSInformatica.NTSBarButtonItem
  Friend WithEvents tlbPanelLeft As NTSBarButtonItem
  Public WithEvents spSplitter As NTSSplitter
  Public WithEvents fmLista As NTSGroupBox
  Public WithEvents grLista As NTSGrid
  Public WithEvents grvLista As NTSGridView
  Public WithEvents ch_codchia As NTSGridColumn
  Public WithEvents ch_deschia As NTSGridColumn
  Public WithEvents pnListaFind As NTSPanel
  Public WithEvents flImpostazione As NTSFlowLayoutPanel
  Public WithEvents fmPrincipale As NTSGroupBox
  Public WithEvents edCh_deschia As NTSTextBoxStr
  Public WithEvents lbCh_deschia As NTSLabel
  Public WithEvents lbCh_codchia As NTSLabel
  Public WithEvents lbCh_urlchia As NTSLabel
  Public WithEvents lbListaFind As NTSLabel
  Public WithEvents edLista As NTSTextBoxStr
  Friend WithEvents edCh_codchia As NTSTextBoxNum
  Public WithEvents edCh_urlchia As NTSTextBoxStr
  Public WithEvents tlbStrumenti As NTSBarSubItem
#End Region

#Region "Inizializzazione"
  Public Overridable Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMHHCNMG))
    Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
    Me.tlbMain = New NTSInformatica.NTSBar()
    Me.tlbPanelLeft = New NTSInformatica.NTSBarButtonItem()
    Me.tlbNuovo = New NTSInformatica.NTSBarButtonItem()
    Me.tlbSalva = New NTSInformatica.NTSBarButtonItem()
    Me.tlbRipristina = New NTSInformatica.NTSBarButtonItem()
    Me.tlbZoom = New NTSInformatica.NTSBarButtonItem()
    Me.tlbStrumenti = New NTSInformatica.NTSBarSubItem()
    Me.tlbCancella = New NTSInformatica.NTSBarButtonItem()
    Me.tlbGuida = New NTSInformatica.NTSBarButtonItem()
    Me.tlbEsci = New NTSInformatica.NTSBarButtonItem()
    Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
    Me.tlbDuplica = New NTSInformatica.NTSBarButtonItem()
    Me.fmLista = New NTSInformatica.NTSGroupBox()
    Me.grLista = New NTSInformatica.NTSGrid()
    Me.grvLista = New NTSInformatica.NTSGridView()
    Me.ch_codchia = New NTSInformatica.NTSGridColumn()
    Me.ch_deschia = New NTSInformatica.NTSGridColumn()
    Me.pnListaFind = New NTSInformatica.NTSPanel()
    Me.edLista = New NTSInformatica.NTSTextBoxStr()
    Me.lbListaFind = New NTSInformatica.NTSLabel()
    Me.spSplitter = New NTSInformatica.NTSSplitter()
    Me.flImpostazione = New NTSInformatica.NTSFlowLayoutPanel()
    Me.fmPrincipale = New NTSInformatica.NTSGroupBox()
    Me.edCh_urlchia = New NTSInformatica.NTSTextBoxStr()
    Me.edCh_codchia = New NTSInformatica.NTSTextBoxNum()
    Me.lbCh_urlchia = New NTSInformatica.NTSLabel()
    Me.edCh_deschia = New NTSInformatica.NTSTextBoxStr()
    Me.lbCh_deschia = New NTSInformatica.NTSLabel()
    Me.lbCh_codchia = New NTSInformatica.NTSLabel()
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fmLista, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmLista.SuspendLayout()
    CType(Me.grLista, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grvLista, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pnListaFind, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnListaFind.SuspendLayout()
    CType(Me.edLista.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.flImpostazione.SuspendLayout()
    CType(Me.fmPrincipale, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmPrincipale.SuspendLayout()
    CType(Me.edCh_urlchia.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edCh_codchia.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edCh_deschia.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
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
    Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbNuovo, Me.tlbDuplica, Me.tlbSalva, Me.tlbRipristina, Me.tlbCancella, Me.tlbZoom, Me.tlbGuida, Me.tlbEsci, Me.tlbPanelLeft, Me.tlbStrumenti})
    Me.NtsBarManager1.MaxItemId = 18
    '
    'tlbMain
    '
    Me.tlbMain.BarName = "tlbMain"
    Me.tlbMain.DockCol = 0
    Me.tlbMain.DockRow = 0
    Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
    Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbPanelLeft), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbNuovo), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbSalva), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbRipristina), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbZoom), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbStrumenti, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbGuida, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci)})
    Me.tlbMain.OptionsBar.AllowQuickCustomization = False
    Me.tlbMain.OptionsBar.DisableClose = True
    Me.tlbMain.OptionsBar.DrawDragBorder = False
    Me.tlbMain.OptionsBar.UseWholeRow = True
    Me.tlbMain.Text = "tlbMain"
    '
    'tlbPanelLeft
    '
    Me.tlbPanelLeft.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check
    Me.tlbPanelLeft.Caption = """"""
    Me.tlbPanelLeft.Down = True
    Me.tlbPanelLeft.Glyph = CType(resources.GetObject("tlbPanelLeft.Glyph"), System.Drawing.Image)
    Me.tlbPanelLeft.Id = 12
    Me.tlbPanelLeft.Name = "tlbPanelLeft"
    Me.tlbPanelLeft.Visible = True
    '
    'tlbNuovo
    '
    Me.tlbNuovo.Caption = "Nuovo"
    Me.tlbNuovo.Glyph = CType(resources.GetObject("tlbNuovo.Glyph"), System.Drawing.Image)
    Me.tlbNuovo.Id = 0
    Me.tlbNuovo.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2)
    Me.tlbNuovo.Name = "tlbNuovo"
    Me.tlbNuovo.Visible = True
    '
    'tlbSalva
    '
    Me.tlbSalva.Caption = "Salva"
    Me.tlbSalva.Glyph = CType(resources.GetObject("tlbSalva.Glyph"), System.Drawing.Image)
    Me.tlbSalva.Id = 2
    Me.tlbSalva.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F9)
    Me.tlbSalva.Name = "tlbSalva"
    Me.tlbSalva.Visible = True
    '
    'tlbRipristina
    '
    Me.tlbRipristina.Caption = "Ripristina"
    Me.tlbRipristina.Glyph = CType(resources.GetObject("tlbRipristina.Glyph"), System.Drawing.Image)
    Me.tlbRipristina.Id = 3
    Me.tlbRipristina.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F8)
    Me.tlbRipristina.Name = "tlbRipristina"
    Me.tlbRipristina.Visible = True
    '
    'tlbZoom
    '
    Me.tlbZoom.Caption = "Zoom"
    Me.tlbZoom.Glyph = CType(resources.GetObject("tlbZoom.Glyph"), System.Drawing.Image)
    Me.tlbZoom.Id = 5
    Me.tlbZoom.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
    Me.tlbZoom.Name = "tlbZoom"
    Me.tlbZoom.Visible = True
    '
    'tlbStrumenti
    '
    Me.tlbStrumenti.Caption = "Strumenti"
    Me.tlbStrumenti.Glyph = CType(resources.GetObject("tlbStrumenti.Glyph"), System.Drawing.Image)
    Me.tlbStrumenti.Id = 13
    Me.tlbStrumenti.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCancella)})
    Me.tlbStrumenti.Name = "tlbStrumenti"
    Me.tlbStrumenti.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionInMenu
    Me.tlbStrumenti.Visible = True
    '
    'tlbCancella
    '
    Me.tlbCancella.Caption = "Cancella"
    Me.tlbCancella.Glyph = CType(resources.GetObject("tlbCancella.Glyph"), System.Drawing.Image)
    Me.tlbCancella.Id = 4
    Me.tlbCancella.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4)
    Me.tlbCancella.Name = "tlbCancella"
    Me.tlbCancella.Visible = True
    '
    'tlbGuida
    '
    Me.tlbGuida.Caption = "Guida"
    Me.tlbGuida.Glyph = CType(resources.GetObject("tlbGuida.Glyph"), System.Drawing.Image)
    Me.tlbGuida.Id = 4
    Me.tlbGuida.Name = "tlbGuida"
    Me.tlbGuida.Visible = True
    '
    'tlbEsci
    '
    Me.tlbEsci.Caption = "Esci"
    Me.tlbEsci.Glyph = CType(resources.GetObject("tlbEsci.Glyph"), System.Drawing.Image)
    Me.tlbEsci.Id = 11
    Me.tlbEsci.Name = "tlbEsci"
    Me.tlbEsci.Visible = True
    '
    'barDockControlTop
    '
    Me.barDockControlTop.CausesValidation = False
    Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
    Me.barDockControlTop.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.barDockControlTop.Size = New System.Drawing.Size(684, 35)
    '
    'barDockControlBottom
    '
    Me.barDockControlBottom.CausesValidation = False
    Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.barDockControlBottom.Location = New System.Drawing.Point(0, 511)
    Me.barDockControlBottom.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.barDockControlBottom.Size = New System.Drawing.Size(684, 0)
    '
    'barDockControlLeft
    '
    Me.barDockControlLeft.CausesValidation = False
    Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
    Me.barDockControlLeft.Location = New System.Drawing.Point(0, 35)
    Me.barDockControlLeft.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.barDockControlLeft.Size = New System.Drawing.Size(0, 476)
    '
    'barDockControlRight
    '
    Me.barDockControlRight.CausesValidation = False
    Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
    Me.barDockControlRight.Location = New System.Drawing.Point(684, 35)
    Me.barDockControlRight.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.barDockControlRight.Size = New System.Drawing.Size(0, 476)
    '
    'tlbDuplica
    '
    Me.tlbDuplica.Glyph = CType(resources.GetObject("tlbDuplica.Glyph"), System.Drawing.Image)
    Me.tlbDuplica.Id = 1
    Me.tlbDuplica.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D))
    Me.tlbDuplica.Name = "tlbDuplica"
    Me.tlbDuplica.Visible = True
    '
    'fmLista
    '
    Me.fmLista.AllowDrop = True
    Me.fmLista.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmLista.Appearance.Options.UseBackColor = True
    Me.fmLista.Controls.Add(Me.grLista)
    Me.fmLista.Controls.Add(Me.pnListaFind)
    Me.fmLista.Dock = System.Windows.Forms.DockStyle.Left
    Me.fmLista.Location = New System.Drawing.Point(0, 35)
    Me.fmLista.Name = "fmLista"
    Me.fmLista.Size = New System.Drawing.Size(216, 476)
    Me.fmLista.Text = "LISTA CHIAMATE"
    '
    'grLista
    '
    Me.grLista.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grLista.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.grLista.Location = New System.Drawing.Point(2, 41)
    Me.grLista.MainView = Me.grvLista
    Me.grLista.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.grLista.Name = "grLista"
    Me.grLista.Size = New System.Drawing.Size(212, 433)
    Me.grLista.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvLista})
    '
    'grvLista
    '
    Me.grvLista.ActiveFilterEnabled = False
    Me.grvLista.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.ch_codchia, Me.ch_deschia})
    Me.grvLista.CustomizationFormBounds = New System.Drawing.Rectangle(680, 326, 208, 170)
    Me.grvLista.Enabled = False
    Me.grvLista.GridControl = Me.grLista
    Me.grvLista.Name = "grvLista"
    Me.grvLista.OptionsBehavior.Editable = False
    Me.grvLista.OptionsCustomization.AllowRowSizing = True
    Me.grvLista.OptionsFilter.AllowFilterEditor = False
    Me.grvLista.OptionsNavigation.EnterMoveNextColumn = True
    Me.grvLista.OptionsNavigation.UseTabKey = False
    Me.grvLista.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.grvLista.OptionsView.ColumnAutoWidth = False
    Me.grvLista.OptionsView.EnableAppearanceEvenRow = True
    Me.grvLista.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden
    Me.grvLista.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.grvLista.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
    Me.grvLista.OptionsView.ShowGroupPanel = False
    '
    'ch_codchia
    '
    Me.ch_codchia.AppearanceCell.Options.UseBackColor = True
    Me.ch_codchia.AppearanceCell.Options.UseTextOptions = True
    Me.ch_codchia.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.ch_codchia.Caption = "Codice"
    Me.ch_codchia.Enabled = True
    Me.ch_codchia.FieldName = "ch_codchia"
    Me.ch_codchia.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.ch_codchia.Name = "ch_codchia"
    Me.ch_codchia.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
    Me.ch_codchia.OptionsFilter.AllowFilter = False
    Me.ch_codchia.Visible = True
    Me.ch_codchia.VisibleIndex = 0
    '
    'ch_deschia
    '
    Me.ch_deschia.AppearanceCell.Options.UseBackColor = True
    Me.ch_deschia.AppearanceCell.Options.UseTextOptions = True
    Me.ch_deschia.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.ch_deschia.Caption = "Descrizione"
    Me.ch_deschia.Enabled = True
    Me.ch_deschia.FieldName = "ch_deschia"
    Me.ch_deschia.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.ch_deschia.Name = "ch_deschia"
    Me.ch_deschia.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
    Me.ch_deschia.OptionsFilter.AllowFilter = False
    Me.ch_deschia.Visible = True
    Me.ch_deschia.VisibleIndex = 1
    '
    'pnListaFind
    '
    Me.pnListaFind.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.pnListaFind.Controls.Add(Me.edLista)
    Me.pnListaFind.Controls.Add(Me.lbListaFind)
    Me.pnListaFind.Dock = System.Windows.Forms.DockStyle.Top
    Me.pnListaFind.Location = New System.Drawing.Point(2, 21)
    Me.pnListaFind.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.pnListaFind.Name = "pnListaFind"
    Me.pnListaFind.Size = New System.Drawing.Size(212, 20)
    '
    'edLista
    '
    Me.edLista.Cursor = System.Windows.Forms.Cursors.Default
    Me.edLista.Dock = System.Windows.Forms.DockStyle.Top
    Me.edLista.EditValue = ""
    Me.edLista.Location = New System.Drawing.Point(44, 0)
    Me.edLista.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
    Me.edLista.Name = "edLista"
    Me.edLista.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
    Me.edLista.Properties.Appearance.ForeColor = System.Drawing.SystemColors.WindowText
    Me.edLista.Properties.Appearance.Options.UseBackColor = True
    Me.edLista.Properties.Appearance.Options.UseForeColor = True
    Me.edLista.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edLista.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edLista.Properties.AutoHeight = False
    Me.edLista.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edLista.Properties.MaxLength = 65536
    Me.edLista.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edLista.Size = New System.Drawing.Size(168, 20)
    '
    'lbListaFind
    '
    Me.lbListaFind.BackColor = System.Drawing.Color.Transparent
    Me.lbListaFind.Dock = System.Windows.Forms.DockStyle.Left
    Me.lbListaFind.Location = New System.Drawing.Point(0, 0)
    Me.lbListaFind.Margin = New System.Windows.Forms.Padding(3, 2, 3, 14)
    Me.lbListaFind.Name = "lbListaFind"
    Me.lbListaFind.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.None
    Me.lbListaFind.Size = New System.Drawing.Size(44, 20)
    Me.lbListaFind.Text = "Cerca"
    Me.lbListaFind.UseMnemonic = False
    '
    'spSplitter
    '
    Me.spSplitter.Location = New System.Drawing.Point(216, 35)
    Me.spSplitter.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.spSplitter.Name = "spSplitter"
    Me.spSplitter.Size = New System.Drawing.Size(6, 476)
    Me.spSplitter.TabIndex = 621
    Me.spSplitter.TabStop = False
    '
    'flImpostazione
    '
    Me.flImpostazione.AllowDrop = True
    Me.flImpostazione.AutoScroll = True
    Me.flImpostazione.Controls.Add(Me.fmPrincipale)
    Me.flImpostazione.Dock = System.Windows.Forms.DockStyle.Fill
    Me.flImpostazione.Location = New System.Drawing.Point(222, 35)
    Me.flImpostazione.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
    Me.flImpostazione.Name = "flImpostazione"
    Me.flImpostazione.Size = New System.Drawing.Size(462, 476)
    Me.flImpostazione.TabIndex = 627
    '
    'fmPrincipale
    '
    Me.fmPrincipale.AllowDrop = True
    Me.fmPrincipale.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmPrincipale.Appearance.Options.UseBackColor = True
    Me.fmPrincipale.Controls.Add(Me.edCh_urlchia)
    Me.fmPrincipale.Controls.Add(Me.edCh_codchia)
    Me.fmPrincipale.Controls.Add(Me.lbCh_urlchia)
    Me.fmPrincipale.Controls.Add(Me.edCh_deschia)
    Me.fmPrincipale.Controls.Add(Me.lbCh_deschia)
    Me.fmPrincipale.Controls.Add(Me.lbCh_codchia)
    Me.fmPrincipale.Location = New System.Drawing.Point(4, 4)
    Me.fmPrincipale.Margin = New System.Windows.Forms.Padding(4)
    Me.fmPrincipale.Name = "fmPrincipale"
    Me.fmPrincipale.Size = New System.Drawing.Size(456, 248)
    Me.fmPrincipale.Text = "DATI PRINCIPALI"
    Me.fmPrincipale.Tile = True
    Me.fmPrincipale.TileIndex = 0
    '
    'edCh_urlchia
    '
    Me.edCh_urlchia.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.edCh_urlchia.Cursor = System.Windows.Forms.Cursors.Default
    Me.edCh_urlchia.Location = New System.Drawing.Point(108, 72)
    Me.edCh_urlchia.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.edCh_urlchia.Name = "edCh_urlchia"
    Me.edCh_urlchia.NTSSetparam__DbField = "CHIAMATE.*"
    Me.edCh_urlchia.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edCh_urlchia.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edCh_urlchia.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edCh_urlchia.Properties.MaxLength = 65536
    Me.edCh_urlchia.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edCh_urlchia.Size = New System.Drawing.Size(340, 20)
    '
    'edCh_codchia
    '
    Me.edCh_codchia.EditValue = "0"
    Me.edCh_codchia.Location = New System.Drawing.Point(108, 24)
    Me.edCh_codchia.Name = "edCh_codchia"
    Me.edCh_codchia.NTSSetparam__DbField = "CHIAMATE.*"
    Me.edCh_codchia.Properties.Appearance.Options.UseTextOptions = True
    Me.edCh_codchia.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edCh_codchia.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edCh_codchia.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edCh_codchia.Properties.AutoHeight = False
    Me.edCh_codchia.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edCh_codchia.Properties.MaxLength = 65536
    Me.edCh_codchia.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edCh_codchia.Size = New System.Drawing.Size(48, 20)
    '
    'lbCh_urlchia
    '
    Me.lbCh_urlchia.BackColor = System.Drawing.Color.Transparent
    Me.lbCh_urlchia.Location = New System.Drawing.Point(4, 72)
    Me.lbCh_urlchia.Name = "lbCh_urlchia"
    Me.lbCh_urlchia.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbCh_urlchia.Size = New System.Drawing.Size(100, 20)
    Me.lbCh_urlchia.Text = "URL"
    Me.lbCh_urlchia.UseMnemonic = False
    '
    'edCh_deschia
    '
    Me.edCh_deschia.Cursor = System.Windows.Forms.Cursors.Default
    Me.edCh_deschia.Location = New System.Drawing.Point(108, 48)
    Me.edCh_deschia.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.edCh_deschia.Name = "edCh_deschia"
    Me.edCh_deschia.NTSSetparam__DbField = "CHIAMATE.*"
    Me.edCh_deschia.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edCh_deschia.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edCh_deschia.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edCh_deschia.Properties.MaxLength = 65536
    Me.edCh_deschia.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edCh_deschia.Size = New System.Drawing.Size(340, 20)
    '
    'lbCh_deschia
    '
    Me.lbCh_deschia.BackColor = System.Drawing.Color.Transparent
    Me.lbCh_deschia.Location = New System.Drawing.Point(4, 48)
    Me.lbCh_deschia.Name = "lbCh_deschia"
    Me.lbCh_deschia.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbCh_deschia.Size = New System.Drawing.Size(100, 20)
    Me.lbCh_deschia.Text = "Descrizione"
    Me.lbCh_deschia.UseMnemonic = False
    '
    'lbCh_codchia
    '
    Me.lbCh_codchia.BackColor = System.Drawing.Color.Transparent
    Me.lbCh_codchia.Location = New System.Drawing.Point(4, 24)
    Me.lbCh_codchia.Name = "lbCh_codchia"
    Me.lbCh_codchia.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbCh_codchia.Size = New System.Drawing.Size(100, 20)
    Me.lbCh_codchia.Text = "Codice"
    Me.lbCh_codchia.UseMnemonic = False
    '
    'FRMHHCNMG
    '
    Me.ClientSize = New System.Drawing.Size(684, 511)
    Me.Controls.Add(Me.flImpostazione)
    Me.Controls.Add(Me.spSplitter)
    Me.Controls.Add(Me.fmLista)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.MinimizeBox = False
    Me.Name = "FRMHHCNMG"
    Me.Text = "IMPOSTAZIONE CHIAMATE MAGAZZINO"
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fmLista, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmLista.ResumeLayout(False)
    CType(Me.grLista, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grvLista, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pnListaFind, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnListaFind.ResumeLayout(False)
    CType(Me.edLista.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.flImpostazione.ResumeLayout(False)
    CType(Me.fmPrincipale, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmPrincipale.ResumeLayout(False)
    CType(Me.edCh_urlchia.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edCh_codchia.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edCh_deschia.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

  Public Overridable Sub InitControls()
    '--------------------------------------------------
    InitControlsBeginEndInit(Me, False)
    '--------------------------------------------------
    Try
      '--------------------------------------------------
      '--- Carico le immagini della toolbar
      '--------------------------------------------------
      Try
        tlbPanelLeft.GlyphPath = (oApp.ChildImageDir & "\PnLeftOpen.png")
        tlbNuovo.GlyphPath = (oApp.ChildImageDir & "\new.png")
        tlbDuplica.GlyphPath = (oApp.ChildImageDir & "\duplica.png")
        tlbSalva.GlyphPath = (oApp.ChildImageDir & "\save.png")
        tlbRipristina.GlyphPath = (oApp.ChildImageDir & "\restore.png")
        tlbCancella.GlyphPath = (oApp.ChildImageDir & "\delete.png")
        tlbZoom.GlyphPath = (oApp.ChildImageDir & "\zoom.png")
        tlbStrumenti.GlyphPath = (oApp.ChildImageDir & "\options.png")
        tlbGuida.GlyphPath = (oApp.ChildImageDir & "\help.png")
        tlbEsci.GlyphPath = (oApp.ChildImageDir & "\exit.png")
      Catch ex As Exception
        'non gestisco l'errore: se non c'è una immagine prendo quella standard
      End Try
      tlbMain.NTSSetToolTip()

      '-------------------------------------------------
      '--- Carica il combo dei tipi connessione
      '-------------------------------------------------
      'Dim dttTipo As New DataTable
      'With dttTipo
      '  .Columns.Add("cod", GetType(String))
      '  .Columns.Add("val", GetType(String))
      '  .Rows.Add(New Object() {"1", "Sku Insert/Update"})
      '  .Rows.Add(New Object() {"2", "Sku Delete"})
      '  .Rows.Add(New Object() {"3", "Advice Insert/Update"})
      '  .Rows.Add(New Object() {"4", "Advice Delete"})
      '  .Rows.Add(New Object() {"5", "Order Insert/Update"})
      '  .Rows.Add(New Object() {"6", "Order Delete"})
      'End With
      'cbch_codchia.DataSource = dttTipo
      'cbch_codchia.ValueMember = "cod"
      'cbch_codchia.DisplayMember = "val"

      '--------------------------------------------------
      '--- Griglia Lista
      '--------------------------------------------------
      grvLista.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964289, "Elenco voci"))
      edLista.NTSSetParam(oMenu, oApp.Tr(Me, 131218715269943804, "Filtra elenco voci"), 0, True)
      grvLista.NTSAllowInsert = False
      grvLista.NTSHideMenuSx()
      ch_codchia.NTSSetParamNUM(oMenu, oApp.Tr(Me, 130984639852359332, ch_codchia.Caption), "0", 4, 0, 9999)
      'ch_codchia.NTSSetParamCMB(oMenu, oApp.Tr(Me, 130984639852359332, ch_codchia.Caption), dttTipo, "val", "cod")
      ch_deschia.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130984640188772754, ch_deschia.Caption), 50)

      '--------------------------------------------------
      '--- Controlli testata profilo
      '--------------------------------------------------
      'cbch_codchia.NTSSetParam(oApp.Tr(Me, 128662037962656361, lbch_codchia.Text))
      edCh_codchia.NTSSetParam(oMenu, oApp.Tr(Me, 128306284873352655, lbCh_codchia.Text), "0", 4, 0, 9999)
      edCh_deschia.NTSSetParam(oMenu, oApp.Tr(Me, 128306284873040011, lbCh_deschia.Text), 255, False)
      edCh_urlchia.NTSSetParam(oMenu, oApp.Tr(Me, 128662037962968750, lbCh_urlchia.Text), 255, True)

      'Imposta gli Zoom particolari 
      edCh_deschia.NTSSetParamZoom(".")

      '--------------------------------------------------
      '--- Non faccio lanciare lo zoom dal controllo visto che 
      '--- non devo cambiare il contenuto del campo ma riposizionare il dataset!!!
      '--------------------------------------------------
      'cbch_codchia.NTSSetRichiesto()
      edCh_codchia.TextInt = 1
      edCh_codchia.NTSForzaVisZoom = False
      edCh_codchia.NTSSetRichiesto()

      '--------------------------------------------------
      '--- Alla fine lancio lo script per eventuali controlli personalizzati
      '--------------------------------------------------
      NTSScriptExec("InitControls", Me, Nothing)

    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    Finally
      '----------------------------------------------------
      InitControlsBeginEndInit(Me, True)
      '----------------------------------------------------
    End Try
  End Sub

  Public Overloads Function Init(ByRef Menu As CLE__MENU, ByRef Param As CLE__CLDP, Optional ByVal Ditta As String = "", Optional ByRef SharedControls As CLE__EVNT = Nothing) As Boolean
    oMenu = Menu
    oApp = oMenu.App
    oCallParams = Param
    If Ditta <> "" Then
      DittaCorrente = Ditta
    Else
      DittaCorrente = oApp.Ditta
    End If
    Me.GctlTipoDoc = ""

    InitializeComponent()
    Me.MinimumSize = Me.Size

    '--------------------------------------------------
    '--- Creo e attivo l'entity e inizializzo la funzione 
    '--- che dovrà rilevare gli eventi dall'ENTITY
    '--------------------------------------------------
    Dim strErr As String = ""
    Dim oTmp As Object = Nothing
    If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNHHCNMG", "BEHHCNMG", oTmp, strErr, False, "", "") = False Then
      oApp.MsgBoxErr(oApp.Tr(Me, 128704611604674454, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
      Return False
    End If

    ocleCnmg = CType(oTmp, CLEHHCNMG)

    '--------------------------------------------------
    '--- Aggiunge gestore eventi
    '--------------------------------------------------
    AddHandler ocleCnmg.RemoteEvent, AddressOf GestisciEventiEntity
    If ocleCnmg.Init(oApp, oScript, oMenu.oCleComm, "", False, "", "") = False Then Return False

    '-------------------------------------------------------------
    '--- Valore funzione
    '-------------------------------------------------------------
    Return True
  End Function

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
        Case "SETFOCUS"
          Select Case e.Message.ToLower   'CONTROLLARE TUTTI CON MINUSCOLO
            Case "edch_codchia"
              edCh_codchia.Focus()
            'Case "cbch_codchia"
            '  cbch_codchia.Focus()
            Case "edch_deschia"
              edCh_deschia.Focus()
          End Select
      End Select

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub
#End Region

#Region "Funzioni e Routines"
  Public Overridable Sub Bindcontrols()
    Try
      '--------------------------------------------------
      '--- Se i controlli erano già stati precedentemente collegati, li scollego
      '--------------------------------------------------
      NTSFormClearDataBinding(Me)

      '--------------------------------------------------
      '--- Per agganciare al dataset i vari controlli
      '--------------------------------------------------
      NTSFormAddDataBinding(dcCnmg, Me)

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Function Apri() As Boolean
    Try
      '--------------------------------------------------
      '--- Se se in caricamento Form
      '--------------------------------------------------
      If bInLoad = True Then Return True
      If dsCnmg Is Nothing Then Return True

      '--------------------------------------------------
      '--- Apre le connessioni impostate
      '--------------------------------------------------
      dsCnmg = Nothing
      dsCnmg = New DataSet
      If Not ocleCnmg.Apri(dsCnmg) Then
        Return False
      End If
      'dsCnmg.Tables("CHIAMATE").DefaultView.Sort = "ch_codchia ASC"
      dcCnmg.DataSource = dsCnmg.Tables("CHIAMATE")
      dsCnmg.AcceptChanges()

      '--------------------------------------------------
      '--- Collega i controlli della Form se ci sono CONNESSIONI
      '--------------------------------------------------
      Bindcontrols()

      '--------------------------------------------------
      '--- Imposta datacontrol per lista
      '--------------------------------------------------
      grLista.DataSource = dcCnmg

      '--------------------------------------------------
      '-- Se non presenti CONNESSIONI.impostati predispone nuovo
      '--------------------------------------------------
      If dsCnmg.Tables("CHIAMATE").Rows.Count = 0 Then
        Nuovo()
        dcCnmg.ResetBindings(False)
        dcCnmg.MoveFirst()
      Else
        'cbch_codchia.Enabled = False
        edCh_codchia.Enabled = False
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

  Public Overridable Sub Cancella()
    Dim bRemovBinding As Boolean = False
    Try
      '-------------------------------------------------
      '--- Cancellazione il profilo
      '-------------------------------------------------
      Select Case Me.NTSChiediCancella(Nothing, oApp.Tr(Me, 128306256431033721, "Cancellare il profilo?"))
        Case System.Windows.Forms.DialogResult.No
          Return
        Case System.Windows.Forms.DialogResult.Yes

          If dsCnmg.Tables("CHIAMATE").Rows.Count = 1 Then
            bRemovBinding = True
            NTSFormClearDataBinding(Me)
          End If

          dcCnmg.RemoveAt(dcCnmg.Position)
          ocleCnmg.Salva(True)

          If bRemovBinding Then
            Nuovo()
            NTSFormAddDataBinding(dcCnmg, Me)
            bRemovBinding = False
            'cbch_codchia.Enabled = True
            edCh_codchia.Enabled = True
          Else
            'cbch_codchia.Enabled = False
            edCh_codchia.Enabled = False
          End If
      End Select

    Catch ex As Exception
      If bRemovBinding Then NTSFormAddDataBinding(dcCnmg, Me)
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub Nuovo()
    Try
      '--------------------------------------------------
      '--- Creo un nuovo profilo
      '--------------------------------------------------
      If Not Salva(False) Then Return

      '--------------------------------------------------
      '--- Azzera stgringa di ricerca su lista
      '--------------------------------------------------
      edLista.Text = ""
      edLista_ValidatedAndChanged(edLista, Nothing)

      '--------------------------------------------------
      '--- Altrimenti se c'è un ordinamento con la dcBanc.MoveLast() 
      '--- non si posiziona sulla riga nuova, ma sull'ultima...
      '--------------------------------------------------
      grvLista.ClearSorting()
      grvLista.NTSAllowInsert = True

      '--------------------------------------------------
      '--- Crea un nuovo profilo
      '--------------------------------------------------
      ocleCnmg.Nuovo()

      '--------------------------------------------------
      '--- Mi riposiziono sul nuovo record
      '--------------------------------------------------
      dcCnmg.MoveLast()
      grvLista.NTSAllowInsert = False
      'cbch_codchia.Enabled = True
      'cbch_codchia.Focus()
      edCh_codchia.Enabled = True
      edCh_codchia.Focus()

      '--------------------------------------------------
      '-- Così se faccio nuovo e, senza toccare nulla, esco non salvo
      '--------------------------------------------------
      ocleCnmg.bHasChanges = False

      '--------------------------------------------------
      '--- Imposto i valori di default come impostato nella GCTL
      '--------------------------------------------------
      Me.GctlApplicaDefaultValue()

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub Ripristina(ByVal bShowDialog As Boolean)
    Dim bRemovBinding As Boolean = False
    Try
      '--------------------------------------------------
      '--- Se se in caricamento Form
      '--------------------------------------------------
      If bInLoad = True Then Return

      '--------------------------------------------------
      '--- Valido l'ultimo controllo che ha il focus
      '--------------------------------------------------
      Me.ValidaLastControl()      '

      '--------------------------------------------------
      '--- Ripristina profilo
      '--------------------------------------------------
      Dim dlgRes As DialogResult
      If bShowDialog Then
        'chiamato facendo pressione sulla funzione 'ripristina'
        dlgRes = oApp.MsgBoxInfoYesNo_DefYes(oApp.Tr(Me, 128306256614868393, "Ripristinare le modifiche apportate al profilo?"))
      Else
        'chiamato dalla 'salva
        dlgRes = System.Windows.Forms.DialogResult.Yes
      End If

      Select Case dlgRes
        Case System.Windows.Forms.DialogResult.No
          Return
        Case System.Windows.Forms.DialogResult.Yes
          If dsCnmg.Tables("CHIAMATE").Rows.Count = 1 And dsCnmg.Tables("CHIAMATE").Rows(0).RowState = DataRowState.Added Then
            bRemovBinding = True
            NTSFormClearDataBinding(Me)
          End If

          'Esegue il ripristino 
          If Not ocleCnmg.Ripristina(dcCnmg.Position, dcCnmg.Filter) Then Return

          'Lo imposto qui e non in Entity 
          ocleCnmg.bHasChanges = False

          If bRemovBinding Then
            tlbNuovo_ItemClick(tlbNuovo, Nothing)
            NTSFormAddDataBinding(dcCnmg, Me)
            bRemovBinding = False
            'cbch_codchia.Enabled = True
            edCh_codchia.Enabled = True
          Else
            'cbch_codchia.Enabled = False
            edCh_codchia.Enabled = False
          End If
      End Select

    Catch ex As Exception
      If bRemovBinding Then NTSFormAddDataBinding(dcCnmg, Me)
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Function Salva(ByVal bDelete As Boolean) As Boolean
    Dim bResult As Boolean = False
    Try
      '--------------------------------------------------
      '--- Se se in caricamento Form
      '--------------------------------------------------
      If bInLoad = True Then Return True

      '--------------------------------------------------
      '--- Valido l'ultimo controllo che ha il focus
      '--------------------------------------------------
      If Not edLista.Focused Then Me.ValidaLastControl()

      '--------------------------------------------------
      '--- Salva profilo
      '--------------------------------------------------
      Me.NTSRipristinaNuovoSenzaModifiche(ocleCnmg.bHasChanges, dcCnmg)
      Select Case Me.NTSChiediSalva(Nothing, ocleCnmg.bHasChanges)
        Case DialogResult.Yes
          'salvo
          '--------------------------------------------------
          '--- Salva record 
          '--------------------------------------------------
          'Salva profilo
          If Not ocleCnmg.Salva(False) Then Return False
          If dsCnmg.Tables("CHIAMATE").Rows.Count > 0 Then
            'cbch_codchia.Enabled = False
            edCh_codchia.Enabled = False
            Apri()
          End If
        Case DialogResult.No
          'ripristino
          tlbRipristina_ItemClick(Nothing, Nothing)
        Case DialogResult.Cancel
          'ho abortito l'uscita dalla form o non è¨ stato superato il GctlControllaOutNotEqual
          Return False
      End Select

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

  Public Overridable Sub Trova()
    Try
      If ocleCnmg.lCodconnSel > 0 Then
        Dim lPos As Integer = dcCnmg.Find("ch_codchia", ocleCnmg.lCodconnSel)
        If lPos >= 0 Then dcCnmg.Position = lPos
      End If
      'Dim lPos As Integer = dcCnmg.Find("ch_deschia", edLista.Text)
      'If lPos >= 0 Then dcCnmg.Position = lPos
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Function StampaAdvice(ByVal strAdviceId As String) As Boolean
    Try
      'GRAZIANO 5-11-2022 da sistemare
      Dim strNomeRpt As String = "BARCODE_DOC.RPT"
      Dim strTitleRpt As String = "Ricevimento Merce"
      Dim strKey As String = "Reports99"
      Dim nPjob As Object
      Dim nRis As Integer = 0
      Dim strCrpe As String = ""
      Dim i As Integer

      'Lancio il report di ricevimento merce
      With strAdviceId
        'strCrpe =
        '  "({movmag.codditt} = " & ConvStrRpt(DittaCorrente) &
        '  " AND {movmag.mm_tipork} = " & ConvStrRpt(NTSCStr(!tm_tipork)) &
        '  " AND {movmag.mm_anno} = " & NTSCInt(!tm_anno) &
        '  " AND {movmag.mm_serie} = " & ConvStrRpt(NTSCStr(!tm_serie)) &
        '  " AND {movmag.mm_numdoc} = " & NTSCInt(!tm_numdoc) & ")" &
        '  " OR " &

        'strCrpe = "{testmag.codditt} = " & ConvStrRpt(DittaCorrente) &
        '  " AND {testmag.tm_tipork} = " & ConvStrRpt(NTSCStr(!tm_tipork)) &
        '  " AND {testmag.tm_anno} = " & NTSCInt(!tm_anno) &
        '  " AND {testmag.tm_serie} = " & ConvStrRpt(NTSCStr(!tm_serie)) &
        '  " AND {testmag.tm_numdoc} = " & NTSCInt(!tm_numdoc)
      End With


      nPjob = Nothing
      nPjob = oMenu.ReportPEInit(oApp.Ditta, Me, "BSRMRICM", strKey, " ", 0, 1, strNomeRpt, False, strTitleRpt, False)
      If nPjob Is Nothing Then Return False

      '--------------------------------------------------
      'Lancio tutti gli eventuali reports (gestisce già il multireport)
      For i = 1 To UBound(CType(nPjob, Array), 2)
        nRis = oMenu.PESetSelectionFormula(NTSCInt(CType(nPjob, Array).GetValue(0, i)), strCrpe)
        nRis = oMenu.ReportPEVai(NTSCInt(CType(nPjob, Array).GetValue(0, i)))
      Next
      'AAAAAAAAAAAAAAAAAAAAAAAA

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

#End Region

#Region "Eventi Form"
  Public Overridable Sub FRMHHCNMG_ActivatedFirst(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ActivatedFirst
    Try
      '--------------------------------------------------
      '--- Apre le CHIAMATE impostate
      '--------------------------------------------------
      If Not Apri() Then
        Me.Close()
        Return
      End If

      '-------------------------------------------------------------
      '--- Cerca ed imposta il profilo
      '-------------------------------------------------------------
      Trova()

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub FRMHHCNMG_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
    Try
      'Disabilito eventi di validazione su Entity
      ocleCnmg.lCodconn = 0
      ocleCnmg.lCodconnSel = 0
      'Distrugge oggetti
      dcCnmg.Dispose()
      dsCnmg.Dispose()
    Catch
    End Try
  End Sub

  Public Overridable Sub FRMHHCNMG_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    Try
      '--------------------------------------------------
      '--- Salva il profilo
      '--------------------------------------------------
      If Not Salva(False) Then e.Cancel = True

      '--------------------------------------------------
      '--- Scrive nel recent la visibilità lista e la posizione dello splitter
      '--------------------------------------------------
      oMenu.SaveSettingBus("BSHHCNMG", "Recent", ".", "HH_PanelLeftCNMG", IIf(tlbPanelLeft.Down, "-1", "0").ToString, " ", True, True, False)
      oMenu.SaveSettingBus("BSHHCNMG", "Recent", ".", "HH_spSplitterCNMG", fmLista.Width.ToString, " ", True, True, False)

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub FRMHHCNMG_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Try
      '--------------------------------------------------
      '--- Imposta flag di Load Form
      '--------------------------------------------------
      bInLoad = True

      '--------------------------------------------------
      '--- Leggo i parametri passati dal child chiamante
      '--------------------------------------------------
      Me.Cursor = Cursors.WaitCursor
      If Not oCallParams Is Nothing Then
        '--------------------------------------------------
        '--- Legge parametri per impostazioni connessione
        '--------------------------------------------------
      End If

      '--------------------------------------------------
      '--- Predispongo i controlli
      '--------------------------------------------------
      InitControls()

      '--------------------------------------------------
      '--- Legge dal recent la visibilità di tlbPanelLeft
      '--------------------------------------------------
      Dim bPanelLeft As Boolean = CBool(Val(oMenu.GetSettingBus("BSHHCNMG", "Recent", ".", "HH_PanelLeftCNMG", "-1", " ", "-1")))
      'Visibile
      tlbPanelLeft.Down = bPanelLeft
      tlbPanelLeft_ItemClick(Me, Nothing)
      'Imposta larghezza lista
      If bPanelLeft Then
        Dim nLeftRecent As Integer = NTSCInt(oMenu.GetSettingBus("BSHHCNMG", "Recent", ".", "HH_spSplitterCNMG", "0", " ", "0"))
        If nLeftRecent <= Me.Width - 100 AndAlso nLeftRecent > 0 Then fmLista.Width = nLeftRecent
      End If

      '--------------------------------------------------
      '--- Sempre alla fine di questa funzione: applico le regole della gctl
      '--------------------------------------------------
      GctlSetRoules()

      'Dim dtArt As New DataTable
      'ocleCnmg.ApriAnagrafiche(dtArt)
      'Dim dr As DataRow = dtArt.NewRow
      'dr!Articolo = "PROVA"
      'dr!Descrizione = "PROVA2"
      'dtArt.Rows.Add(dr)
      'dr = dtArt.NewRow
      'dr!Articolo = "PROVA FALCO"
      'dr!Descrizione = "PROVA FALCO2"
      'dtArt.Rows.Add(dr)
      'ocleCnmg.postAnagrafiche("", "")

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    Finally
      '--------------------------------------------------
      '--- Imposta flag di Load Form
      '--------------------------------------------------
      bInLoad = False
    End Try
  End Sub
#End Region

#Region "Eventi Controlli"
  'Public Overridable Sub cbch_codchia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbch_codchia.SelectedIndexChanged
  '  Try
  '    'Seleziona righe e permessi del profilo corrente
  '    ocleCnmg.lCodconn = NTSCInt(cbch_codchia.SelectedValue)
  '    'ocleCnmg.lCodconnSel = edch_codchia.TextInt
  '  Catch ex As Exception
  '    '--------------------------------------------------
  '    CLN__STD.GestErr(ex, Me, "")
  '    '--------------------------------------------------
  '  End Try
  'End Sub

  Public Overridable Sub edLista_ValidatedAndChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    'non posso filtrare direttamente sulla descrizione: 
    'se applico un filtro, poi tra i record filtrfati cambio la descrizione, come esco dal campo descrizione quel record sparisce dalla vista
    Dim strFiltro As String = ""
    Dim dtrT() As DataRow = Nothing
    Dim i As Integer = 0
    Dim strColCod As String = "ch_codchia"
    Dim strColDes As String = "ch_deschia"

    Try
      If edLista.Text.Trim = "" Then
        dcCnmg.Filter = ""
      Else
        dtrT = dsCnmg.Tables("CHIAMATE").Select(strColDes & " Like " & CStrSQL("*" & edLista.Text & "*"))
        If dtrT.Length = 0 Then
          oApp.MsgBoxInfo(oApp.Tr(Me, 131218720457904857, "La ricerca non ha restituito nessun risultato"), True)
          dcCnmg.Filter = ""
        Else
          strFiltro = strColCod & " IN ("
          For i = 0 To dtrT.Length - 1
            If CType(grvLista.Columns(strColCod), NTSGridColumn).NTSTipoColonna = "S" Then
              strFiltro += CStrSQL(dtrT(i).Item(strColCod)) & ","
            Else
              strFiltro += NTSCStr(dtrT(i).Item(strColCod)) & ","
            End If
          Next
          strFiltro = strFiltro.Substring(0, strFiltro.Length - 1) & ")"
          dcCnmg.Filter = strFiltro
        End If
      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub edLista_LostFocus(sender As Object, e As EventArgs) Handles edLista.LostFocus
    Try
      Trova()
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub
#End Region

#Region "Eventi Griglia Lista"
  Public Overridable Sub grvLista_NTSBeforeRowUpdate(sender As Object, e As DevExpress.XtraGrid.Views.Base.RowAllowEventArgs) Handles grvLista.NTSBeforeRowUpdate
    Try
      If grvLista.NTSAllowInsert Then Return
      If Not Salva(False) Then
        e.Allow = False
        Return
      End If
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub grvLista_NTSFocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles grvLista.NTSFocusedRowChanged
    Try
      'Seleziona righe e permessi del profilo corrente
      'ocleCnmg.lCodconn = NTSCInt(cbch_codchia.SelectedValue)
      ocleCnmg.lCodconn = edCh_codchia.TextInt
      ocleCnmg.lCodconnSel = edCh_codchia.TextInt
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub
#End Region

#Region "Eventi Toolbar"
  Public Overridable Sub tlbPanelLeft_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbPanelLeft.ItemClick
    Try
      If tlbPanelLeft.Down Then
        spSplitter.Enabled = True
        fmLista.Visible = True
      Else
        spSplitter.Enabled = False
        fmLista.Visible = False
      End If
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbNuovo_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbNuovo.ItemClick
    Try
      Nuovo()
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbSalva_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbSalva.ItemClick
    Try
      If Not Salva(False) Then Return
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbRipristina_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbRipristina.ItemClick
    Try
      Ripristina(True)
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbZoom_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbZoom.ItemClick
    Dim frmHlpr As FRMHHHLCN = Nothing
    Dim oParam As New CLE__CLDP
    Dim ctrlTmp As Control = NTSFindControlForZoom()
    Try
      '-------------------------------------------------
      '--- Esce se controllo non impostato
      '-------------------------------------------------
      If ctrlTmp Is Nothing Then Return

      '-------------------------------------------------
      '--- Zoom su griglie / controlli Form
      '-------------------------------------------------
      If ctrlTmp.GetType.ToString.IndexOf("NTSGrid") > -1 Then
      Else
        Select Case ctrlTmp.Name.ToLower
          Case "edch_deschia"
            'Salva profilo in gestione
            If Not Salva(False) Then Return
            'Zoom CONNESSIONI.
            frmHlpr = CType(NTSNewFormModal("FRMHHHLCN"), FRMHHHLCN)
            frmHlpr.Init(oMenu, oCallParams, DittaCorrente)
            frmHlpr.InitEntity(ocleCnmg)
            frmHlpr.dttHlcn = dsCnmg.Tables("CHIAMATE").Copy
            frmHlpr.ShowDialog()
            frmHlpr.dttHlcn = Nothing
            If ocleCnmg.lCodconnSel > 0 Then Trova()
          Case Else
            'Per eventuali altri controlli caricati al volo
            'Zoom standard di Textbox e Griglia
            NTSCallStandardZoom()
        End Select
      End If

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      If Not frmHlpr Is Nothing Then frmHlpr.Dispose()
      frmHlpr = Nothing
    End Try
  End Sub

  Public Overridable Sub tlbCancella_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbCancella.ItemClick
    Try
      If grvLista.RowCount = 0 Then Return
      Cancella()
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbGuida_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbGuida.ItemClick
    SendKeys.Send("{F1}")
  End Sub

  Public Overridable Sub tlbEsci_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbEsci.ItemClick
    Try
      Me.Close()
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub
#End Region

End Class
