Imports System.Data
Imports System.IO
Imports System.Net
Imports DevExpress.XtraGrid.Views.Base
Imports NTSInformatica.CLN__STD

Public Class FRMHHIOPC

#Region "Variabili"
  Public oCallParams As CLE__CLDP
  Public ocleIopc As CLEHHIOPC

  Public dsIopc As New DataSet
  Public dcIopcTesta As New BindingSource
  Public dcIopcCorpo As New BindingSource

  Public dsMacc As New DataSet
  Public dcMacc As New BindingSource

  Public strProgramma As String = "BSHHIOPC"

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
  Public WithEvents tlbDuplica As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbSalva As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbRipristina As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbCancella As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbZoom As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbGuida As NTSInformatica.NTSBarButtonItem
  Public WithEvents tlbEsci As NTSInformatica.NTSBarButtonItem
  Friend WithEvents tlbPanelLeft As NTSBarButtonItem
  Friend WithEvents pnlOpz As NTSPanel
  Friend WithEvents tlbCancellaRiga As NTSBarButtonItem
  Friend WithEvents NtsBarSubItem1 As NTSBarSubItem
  Friend WithEvents NtsBarMenuItem1 As NTSBarMenuItem
  Friend WithEvents NtsBarMenuItem2 As NTSBarMenuItem
  Friend WithEvents NtsBarMenuItem3 As NTSBarMenuItem
  Friend WithEvents NtsBarMenuItem4 As NTSBarMenuItem
  Friend WithEvents NtsBarMenuItem5 As NTSBarMenuItem
  Friend WithEvents tlbApri As NTSBarButtonItem
  Friend WithEvents fmMacchina As NTSGroupBox
  Friend WithEvents edTb_codice As NTSTextBoxStr
  Friend WithEvents pnlCorpo As NTSPanel
  Friend WithEvents fmImpostazioni As NTSGroupBox
  Public WithEvents NtsLabel1 As NTSLabel
  Friend WithEvents edTb_secelaborazione As NTSTextBoxNum
  Friend WithEvents ckTb_elaborazioneautom As NTSCheckBox
  Public WithEvents lbTb_elaborazioneautom As NTSLabel
  Friend WithEvents btn_connetti As NTSButton
  Public WithEvents lbTb_password As NTSLabel
  Public WithEvents edTb_password As NTSTextBoxStr
  Public WithEvents lbTb_utente As NTSLabel
  Public WithEvents edTb_utente As NTSTextBoxStr
  Public WithEvents lbTb_indirizzo As NTSLabel
  Public WithEvents edTb_indirizzo As NTSTextBoxStr
  Friend WithEvents pnlMacchine As NTSPanel
  Public WithEvents fmMacchine As NTSGroupBox
  Friend WithEvents grMacchine As NTSGrid
  Friend WithEvents grvMacchine As NTSGridView
  Friend WithEvents tb_codice As NTSGridColumn
  Friend WithEvents tlbNuovo As NTSBarButtonItem
  Friend WithEvents NtsPanel3 As NTSPanel
  Friend WithEvents btn_cerca As NTSButton
  Friend WithEvents NtsPanel5 As NTSPanel
  Public WithEvents fmNodi As NTSGroupBox
  Friend WithEvents grNodi As NTSGrid
  Friend WithEvents grvNodi As NTSGridView
  Friend WithEvents tb_nodo As NTSGridColumn
  Friend WithEvents tb_tiponodo As NTSGridColumn
  Friend WithEvents NtsSplitter1 As NTSSplitter
  Friend WithEvents NtsPanel1 As NTSPanel
  Friend WithEvents treeNodi As NTSTreeView
  Friend WithEvents NtsPanel2 As NTSPanel
  Friend WithEvents edTb_nodo As NTSTextBoxStr
  Friend WithEvents tb_tipo As NTSGridColumn
  Friend WithEvents tb_ordinamento As NTSGridColumn
  Friend WithEvents tlbStrumenti As NTSBarSubItem
  Friend WithEvents NtsBarButtonItem1 As NTSBarButtonItem
  Friend WithEvents tlbModalita As NTSBarMenuItem
  Public WithEvents lbTb_modalita As NTSLabel
  Friend WithEvents cbTb_modalita As NTSComboBox
  Friend WithEvents tlbRipristinaRiga As NTSBarButtonItem
#End Region

#Region "Inizializzazione"
  Public Overridable Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMHHIOPC))
    Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
    Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
    Me.tlbMain = New NTSInformatica.NTSBar()
    Me.tlbNuovo = New NTSInformatica.NTSBarButtonItem()
    Me.tlbApri = New NTSInformatica.NTSBarButtonItem()
    Me.tlbSalva = New NTSInformatica.NTSBarButtonItem()
    Me.tlbRipristina = New NTSInformatica.NTSBarButtonItem()
    Me.tlbCancella = New NTSInformatica.NTSBarButtonItem()
    Me.tlbCancellaRiga = New NTSInformatica.NTSBarButtonItem()
    Me.tlbRipristinaRiga = New NTSInformatica.NTSBarButtonItem()
    Me.tlbStrumenti = New NTSInformatica.NTSBarSubItem()
    Me.tlbModalita = New NTSInformatica.NTSBarMenuItem()
    Me.tlbZoom = New NTSInformatica.NTSBarButtonItem()
    Me.tlbGuida = New NTSInformatica.NTSBarButtonItem()
    Me.tlbEsci = New NTSInformatica.NTSBarButtonItem()
    Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
    Me.tlbDuplica = New NTSInformatica.NTSBarButtonItem()
    Me.tlbPanelLeft = New NTSInformatica.NTSBarButtonItem()
    Me.NtsBarSubItem1 = New NTSInformatica.NTSBarSubItem()
    Me.NtsBarMenuItem1 = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarMenuItem2 = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarMenuItem5 = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarMenuItem4 = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarMenuItem3 = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarButtonItem1 = New NTSInformatica.NTSBarButtonItem()
    Me.pnlOpz = New NTSInformatica.NTSPanel()
    Me.fmMacchina = New NTSInformatica.NTSGroupBox()
    Me.btn_cerca = New NTSInformatica.NTSButton()
    Me.edTb_codice = New NTSInformatica.NTSTextBoxStr()
    Me.pnlCorpo = New NTSInformatica.NTSPanel()
    Me.NtsPanel5 = New NTSInformatica.NTSPanel()
    Me.fmNodi = New NTSInformatica.NTSGroupBox()
    Me.grNodi = New NTSInformatica.NTSGrid()
    Me.grvNodi = New NTSInformatica.NTSGridView()
    Me.tb_nodo = New NTSInformatica.NTSGridColumn()
    Me.tb_tiponodo = New NTSInformatica.NTSGridColumn()
    Me.tb_tipo = New NTSInformatica.NTSGridColumn()
    Me.tb_ordinamento = New NTSInformatica.NTSGridColumn()
    Me.NtsSplitter1 = New NTSInformatica.NTSSplitter()
    Me.NtsPanel1 = New NTSInformatica.NTSPanel()
    Me.treeNodi = New NTSInformatica.NTSTreeView()
    Me.NtsPanel2 = New NTSInformatica.NTSPanel()
    Me.edTb_nodo = New NTSInformatica.NTSTextBoxStr()
    Me.fmImpostazioni = New NTSInformatica.NTSGroupBox()
    Me.NtsPanel3 = New NTSInformatica.NTSPanel()
    Me.lbTb_modalita = New NTSInformatica.NTSLabel()
    Me.cbTb_modalita = New NTSInformatica.NTSComboBox()
    Me.lbTb_indirizzo = New NTSInformatica.NTSLabel()
    Me.NtsLabel1 = New NTSInformatica.NTSLabel()
    Me.edTb_indirizzo = New NTSInformatica.NTSTextBoxStr()
    Me.edTb_secelaborazione = New NTSInformatica.NTSTextBoxNum()
    Me.edTb_utente = New NTSInformatica.NTSTextBoxStr()
    Me.ckTb_elaborazioneautom = New NTSInformatica.NTSCheckBox()
    Me.lbTb_utente = New NTSInformatica.NTSLabel()
    Me.lbTb_elaborazioneautom = New NTSInformatica.NTSLabel()
    Me.edTb_password = New NTSInformatica.NTSTextBoxStr()
    Me.btn_connetti = New NTSInformatica.NTSButton()
    Me.lbTb_password = New NTSInformatica.NTSLabel()
    Me.pnlMacchine = New NTSInformatica.NTSPanel()
    Me.fmMacchine = New NTSInformatica.NTSGroupBox()
    Me.grMacchine = New NTSInformatica.NTSGrid()
    Me.grvMacchine = New NTSInformatica.NTSGridView()
    Me.tb_codice = New NTSInformatica.NTSGridColumn()
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pnlOpz, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlOpz.SuspendLayout()
    CType(Me.fmMacchina, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmMacchina.SuspendLayout()
    CType(Me.edTb_codice.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pnlCorpo, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlCorpo.SuspendLayout()
    CType(Me.NtsPanel5, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.NtsPanel5.SuspendLayout()
    CType(Me.fmNodi, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmNodi.SuspendLayout()
    CType(Me.grNodi, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grvNodi, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.NtsPanel1.SuspendLayout()
    CType(Me.NtsPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.NtsPanel2.SuspendLayout()
    CType(Me.edTb_nodo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fmImpostazioni, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmImpostazioni.SuspendLayout()
    CType(Me.NtsPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.NtsPanel3.SuspendLayout()
    CType(Me.cbTb_modalita.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_indirizzo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_secelaborazione.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_utente.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ckTb_elaborazioneautom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_password.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pnlMacchine, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlMacchine.SuspendLayout()
    CType(Me.fmMacchine, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmMacchine.SuspendLayout()
    CType(Me.grMacchine, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grvMacchine, System.ComponentModel.ISupportInitialize).BeginInit()
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
    Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbDuplica, Me.tlbSalva, Me.tlbRipristina, Me.tlbCancella, Me.tlbZoom, Me.tlbGuida, Me.tlbEsci, Me.tlbPanelLeft, Me.tlbCancellaRiga, Me.tlbRipristinaRiga, Me.NtsBarSubItem1, Me.NtsBarMenuItem1, Me.NtsBarMenuItem2, Me.NtsBarMenuItem3, Me.NtsBarMenuItem4, Me.NtsBarMenuItem5, Me.tlbApri, Me.tlbNuovo, Me.NtsBarButtonItem1, Me.tlbStrumenti, Me.tlbModalita})
    Me.NtsBarManager1.MaxItemId = 31
    '
    'tlbMain
    '
    Me.tlbMain.BarName = "tlbMain"
    Me.tlbMain.DockCol = 0
    Me.tlbMain.DockRow = 0
    Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
    Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbNuovo), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbApri, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbSalva, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbRipristina, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCancella, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCancellaRiga, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbRipristinaRiga, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbZoom), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, False, Me.tlbGuida, False), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci)})
    Me.tlbMain.OptionsBar.AllowQuickCustomization = False
    Me.tlbMain.OptionsBar.DisableClose = True
    Me.tlbMain.OptionsBar.DrawDragBorder = False
    Me.tlbMain.OptionsBar.UseWholeRow = True
    Me.tlbMain.Text = "tlbMain"
    '
    'tlbNuovo
    '
    Me.tlbNuovo.Caption = "Nuovo"
    Me.tlbNuovo.Id = 27
    Me.tlbNuovo.Name = "tlbNuovo"
    Me.tlbNuovo.Visible = True
    '
    'tlbApri
    '
    Me.tlbApri.Caption = "Apri"
    Me.tlbApri.Id = 26
    Me.tlbApri.Name = "tlbApri"
    Me.tlbApri.Visible = True
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
    'tlbCancella
    '
    Me.tlbCancella.Caption = "Cancella"
    Me.tlbCancella.Glyph = CType(resources.GetObject("tlbCancella.Glyph"), System.Drawing.Image)
    Me.tlbCancella.Id = 4
    Me.tlbCancella.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4)
    Me.tlbCancella.Name = "tlbCancella"
    Me.tlbCancella.Visible = True
    '
    'tlbCancellaRiga
    '
    Me.tlbCancellaRiga.Caption = "Cancella Riga"
    Me.tlbCancellaRiga.Glyph = CType(resources.GetObject("tlbCancellaRiga.Glyph"), System.Drawing.Image)
    Me.tlbCancellaRiga.Id = 18
    Me.tlbCancellaRiga.Name = "tlbCancellaRiga"
    Me.tlbCancellaRiga.Visible = True
    '
    'tlbRipristinaRiga
    '
    Me.tlbRipristinaRiga.Caption = "Ripristina Riga"
    Me.tlbRipristinaRiga.Glyph = CType(resources.GetObject("tlbRipristinaRiga.Glyph"), System.Drawing.Image)
    Me.tlbRipristinaRiga.Id = 19
    Me.tlbRipristinaRiga.Name = "tlbRipristinaRiga"
    Me.tlbRipristinaRiga.Visible = True
    '
    'tlbStrumenti
    '
    Me.tlbStrumenti.Caption = "Strumenti"
    Me.tlbStrumenti.Glyph = CType(resources.GetObject("tlbStrumenti.Glyph"), System.Drawing.Image)
    Me.tlbStrumenti.Id = 29
    Me.tlbStrumenti.Name = "tlbStrumenti"
    Me.tlbStrumenti.Visible = True
    '
    'tlbModalita
    '
    Me.tlbModalita.Caption = "Modalità OPC-UA"
    Me.tlbModalita.Id = 30
    Me.tlbModalita.Name = "tlbModalita"
    Me.tlbModalita.NTSIsCheckBox = False
    Me.tlbModalita.Visible = True
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
    'tlbGuida
    '
    Me.tlbGuida.Caption = "Guida"
    Me.tlbGuida.Glyph = CType(resources.GetObject("tlbGuida.Glyph"), System.Drawing.Image)
    Me.tlbGuida.Id = 4
    Me.tlbGuida.Name = "tlbGuida"
    Me.tlbGuida.Visible = False
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
    'NtsBarSubItem1
    '
    Me.NtsBarSubItem1.Caption = "Strumenti"
    Me.NtsBarSubItem1.Id = 20
    Me.NtsBarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.NtsBarMenuItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.NtsBarMenuItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.NtsBarMenuItem5), New DevExpress.XtraBars.LinkPersistInfo(Me.NtsBarMenuItem4), New DevExpress.XtraBars.LinkPersistInfo(Me.NtsBarMenuItem3)})
    Me.NtsBarSubItem1.Name = "NtsBarSubItem1"
    Me.NtsBarSubItem1.Visible = True
    '
    'NtsBarMenuItem1
    '
    Me.NtsBarMenuItem1.Caption = "Invia Articoli"
    Me.NtsBarMenuItem1.Id = 21
    Me.NtsBarMenuItem1.Name = "NtsBarMenuItem1"
    Me.NtsBarMenuItem1.NTSIsCheckBox = False
    Me.NtsBarMenuItem1.Visible = True
    '
    'NtsBarMenuItem2
    '
    Me.NtsBarMenuItem2.Caption = "Invia Lista di Prelievo Carico"
    Me.NtsBarMenuItem2.Id = 22
    Me.NtsBarMenuItem2.Name = "NtsBarMenuItem2"
    Me.NtsBarMenuItem2.NTSIsCheckBox = False
    Me.NtsBarMenuItem2.Visible = True
    '
    'NtsBarMenuItem5
    '
    Me.NtsBarMenuItem5.Caption = "Invia Lista di Prelievo Scarico"
    Me.NtsBarMenuItem5.Id = 25
    Me.NtsBarMenuItem5.Name = "NtsBarMenuItem5"
    Me.NtsBarMenuItem5.NTSIsCheckBox = False
    Me.NtsBarMenuItem5.Visible = True
    '
    'NtsBarMenuItem4
    '
    Me.NtsBarMenuItem4.Caption = "Cancella Lista di Prelievo"
    Me.NtsBarMenuItem4.Id = 24
    Me.NtsBarMenuItem4.Name = "NtsBarMenuItem4"
    Me.NtsBarMenuItem4.NTSIsCheckBox = False
    Me.NtsBarMenuItem4.Visible = True
    '
    'NtsBarMenuItem3
    '
    Me.NtsBarMenuItem3.Caption = "Leggi Ordini Elaborati"
    Me.NtsBarMenuItem3.Id = 23
    Me.NtsBarMenuItem3.Name = "NtsBarMenuItem3"
    Me.NtsBarMenuItem3.NTSIsCheckBox = False
    Me.NtsBarMenuItem3.Visible = True
    '
    'NtsBarButtonItem1
    '
    Me.NtsBarButtonItem1.Caption = "Strumenti"
    Me.NtsBarButtonItem1.Id = 28
    Me.NtsBarButtonItem1.Name = "NtsBarButtonItem1"
    Me.NtsBarButtonItem1.Visible = True
    '
    'pnlOpz
    '
    Me.pnlOpz.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.pnlOpz.Controls.Add(Me.fmMacchina)
    Me.pnlOpz.Dock = System.Windows.Forms.DockStyle.Top
    Me.pnlOpz.Location = New System.Drawing.Point(0, 35)
    Me.pnlOpz.Name = "pnlOpz"
    Me.pnlOpz.Size = New System.Drawing.Size(684, 64)
    '
    'fmMacchina
    '
    Me.fmMacchina.AllowDrop = True
    Me.fmMacchina.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmMacchina.Appearance.Options.UseBackColor = True
    Me.fmMacchina.Controls.Add(Me.btn_cerca)
    Me.fmMacchina.Controls.Add(Me.edTb_codice)
    Me.fmMacchina.Location = New System.Drawing.Point(4, 4)
    Me.fmMacchina.Name = "fmMacchina"
    Me.fmMacchina.Size = New System.Drawing.Size(396, 56)
    Me.fmMacchina.Text = "MACCHINA"
    '
    'btn_cerca
    '
    Me.btn_cerca.Location = New System.Drawing.Point(308, 24)
    Me.btn_cerca.Name = "btn_cerca"
    Me.btn_cerca.Size = New System.Drawing.Size(80, 26)
    Me.btn_cerca.Text = "Cerca"
    '
    'edTb_codice
    '
    Me.edTb_codice.EditValue = ""
    Me.edTb_codice.Location = New System.Drawing.Point(4, 24)
    Me.edTb_codice.Name = "edTb_codice"
    Me.edTb_codice.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_codice.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_codice.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_codice.Properties.AutoHeight = False
    Me.edTb_codice.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_codice.Properties.MaxLength = 65536
    Me.edTb_codice.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_codice.Size = New System.Drawing.Size(300, 20)
    '
    'pnlCorpo
    '
    Me.pnlCorpo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.pnlCorpo.Controls.Add(Me.NtsPanel5)
    Me.pnlCorpo.Controls.Add(Me.fmImpostazioni)
    Me.pnlCorpo.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlCorpo.Location = New System.Drawing.Point(0, 99)
    Me.pnlCorpo.Name = "pnlCorpo"
    Me.pnlCorpo.Size = New System.Drawing.Size(684, 412)
    '
    'NtsPanel5
    '
    Me.NtsPanel5.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.NtsPanel5.Controls.Add(Me.fmNodi)
    Me.NtsPanel5.Controls.Add(Me.NtsSplitter1)
    Me.NtsPanel5.Controls.Add(Me.NtsPanel1)
    Me.NtsPanel5.Dock = System.Windows.Forms.DockStyle.Fill
    Me.NtsPanel5.Location = New System.Drawing.Point(0, 156)
    Me.NtsPanel5.Name = "NtsPanel5"
    Me.NtsPanel5.Size = New System.Drawing.Size(684, 256)
    '
    'fmNodi
    '
    Me.fmNodi.AllowDrop = True
    Me.fmNodi.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmNodi.Appearance.Options.UseBackColor = True
    Me.fmNodi.Controls.Add(Me.grNodi)
    Me.fmNodi.Dock = System.Windows.Forms.DockStyle.Fill
    Me.fmNodi.Location = New System.Drawing.Point(197, 0)
    Me.fmNodi.Name = "fmNodi"
    Me.fmNodi.Size = New System.Drawing.Size(487, 256)
    Me.fmNodi.Text = "NODI"
    '
    'grNodi
    '
    Me.grNodi.AllowDrop = True
    Me.grNodi.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grNodi.Location = New System.Drawing.Point(2, 21)
    Me.grNodi.MainView = Me.grvNodi
    Me.grNodi.MenuManager = Me.NtsBarManager1
    Me.grNodi.Name = "grNodi"
    Me.grNodi.Size = New System.Drawing.Size(483, 233)
    Me.grNodi.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvNodi})
    '
    'grvNodi
    '
    Me.grvNodi.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.tb_nodo, Me.tb_tiponodo, Me.tb_tipo, Me.tb_ordinamento})
    Me.grvNodi.Enabled = True
    Me.grvNodi.GridControl = Me.grNodi
    Me.grvNodi.Name = "grvNodi"
    Me.grvNodi.OptionsCustomization.AllowRowSizing = True
    Me.grvNodi.OptionsNavigation.EnterMoveNextColumn = True
    Me.grvNodi.OptionsNavigation.UseTabKey = False
    Me.grvNodi.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.grvNodi.OptionsView.ColumnAutoWidth = False
    Me.grvNodi.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.grvNodi.OptionsView.ShowGroupPanel = False
    '
    'tb_nodo
    '
    Me.tb_nodo.AppearanceCell.Options.UseBackColor = True
    Me.tb_nodo.AppearanceCell.Options.UseTextOptions = True
    Me.tb_nodo.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_nodo.Caption = "Nodo"
    Me.tb_nodo.Enabled = True
    Me.tb_nodo.FieldName = "tb_nodo"
    Me.tb_nodo.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_nodo.Name = "tb_nodo"
    Me.tb_nodo.Visible = True
    Me.tb_nodo.VisibleIndex = 0
    '
    'tb_tiponodo
    '
    Me.tb_tiponodo.AppearanceCell.Options.UseBackColor = True
    Me.tb_tiponodo.AppearanceCell.Options.UseTextOptions = True
    Me.tb_tiponodo.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_tiponodo.Caption = "Tipo Nodo"
    Me.tb_tiponodo.Enabled = True
    Me.tb_tiponodo.FieldName = "tb_tiponodo"
    Me.tb_tiponodo.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_tiponodo.Name = "tb_tiponodo"
    Me.tb_tiponodo.Visible = True
    Me.tb_tiponodo.VisibleIndex = 1
    '
    'tb_tipo
    '
    Me.tb_tipo.AppearanceCell.Options.UseBackColor = True
    Me.tb_tipo.AppearanceCell.Options.UseTextOptions = True
    Me.tb_tipo.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_tipo.Caption = "Tipo"
    Me.tb_tipo.Enabled = True
    Me.tb_tipo.FieldName = "tb_tipo"
    Me.tb_tipo.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_tipo.Name = "tb_tipo"
    Me.tb_tipo.Visible = True
    Me.tb_tipo.VisibleIndex = 2
    '
    'tb_ordinamento
    '
    Me.tb_ordinamento.AppearanceCell.Options.UseBackColor = True
    Me.tb_ordinamento.AppearanceCell.Options.UseTextOptions = True
    Me.tb_ordinamento.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_ordinamento.Caption = "Ordinamento"
    Me.tb_ordinamento.Enabled = True
    Me.tb_ordinamento.FieldName = "tb_ordinamento"
    Me.tb_ordinamento.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_ordinamento.Name = "tb_ordinamento"
    Me.tb_ordinamento.Visible = True
    Me.tb_ordinamento.VisibleIndex = 3
    '
    'NtsSplitter1
    '
    Me.NtsSplitter1.Location = New System.Drawing.Point(194, 0)
    Me.NtsSplitter1.Name = "NtsSplitter1"
    Me.NtsSplitter1.Size = New System.Drawing.Size(3, 256)
    Me.NtsSplitter1.TabIndex = 9
    Me.NtsSplitter1.TabStop = False
    '
    'NtsPanel1
    '
    Me.NtsPanel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.NtsPanel1.Controls.Add(Me.treeNodi)
    Me.NtsPanel1.Controls.Add(Me.NtsPanel2)
    Me.NtsPanel1.Dock = System.Windows.Forms.DockStyle.Left
    Me.NtsPanel1.Location = New System.Drawing.Point(0, 0)
    Me.NtsPanel1.Name = "NtsPanel1"
    Me.NtsPanel1.Size = New System.Drawing.Size(194, 256)
    '
    'treeNodi
    '
    Me.treeNodi.Dock = System.Windows.Forms.DockStyle.Fill
    Me.treeNodi.Location = New System.Drawing.Point(0, 0)
    Me.treeNodi.Name = "treeNodi"
    Me.treeNodi.PathSeparator = "."
    Me.treeNodi.Size = New System.Drawing.Size(194, 232)
    Me.treeNodi.TabIndex = 7
    '
    'NtsPanel2
    '
    Me.NtsPanel2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple
    Me.NtsPanel2.Controls.Add(Me.edTb_nodo)
    Me.NtsPanel2.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.NtsPanel2.Location = New System.Drawing.Point(0, 232)
    Me.NtsPanel2.Name = "NtsPanel2"
    Me.NtsPanel2.Size = New System.Drawing.Size(194, 24)
    '
    'edTb_nodo
    '
    Me.edTb_nodo.Dock = System.Windows.Forms.DockStyle.Fill
    Me.edTb_nodo.EditValue = ""
    Me.edTb_nodo.Location = New System.Drawing.Point(2, 2)
    Me.edTb_nodo.Name = "edTb_nodo"
    Me.edTb_nodo.Properties.Appearance.BackColor = System.Drawing.Color.White
    Me.edTb_nodo.Properties.Appearance.Options.UseBackColor = True
    Me.edTb_nodo.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_nodo.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_nodo.Properties.AutoHeight = False
    Me.edTb_nodo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_nodo.Properties.MaxLength = 65536
    Me.edTb_nodo.Properties.ReadOnly = True
    Me.edTb_nodo.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_nodo.Size = New System.Drawing.Size(190, 20)
    '
    'fmImpostazioni
    '
    Me.fmImpostazioni.AllowDrop = True
    Me.fmImpostazioni.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmImpostazioni.Appearance.Options.UseBackColor = True
    Me.fmImpostazioni.Controls.Add(Me.NtsPanel3)
    Me.fmImpostazioni.Dock = System.Windows.Forms.DockStyle.Top
    Me.fmImpostazioni.Location = New System.Drawing.Point(0, 0)
    Me.fmImpostazioni.Name = "fmImpostazioni"
    Me.fmImpostazioni.Size = New System.Drawing.Size(684, 156)
    Me.fmImpostazioni.Text = "IMPOSTAZIONI"
    '
    'NtsPanel3
    '
    Me.NtsPanel3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.NtsPanel3.Controls.Add(Me.lbTb_modalita)
    Me.NtsPanel3.Controls.Add(Me.cbTb_modalita)
    Me.NtsPanel3.Controls.Add(Me.lbTb_indirizzo)
    Me.NtsPanel3.Controls.Add(Me.NtsLabel1)
    Me.NtsPanel3.Controls.Add(Me.edTb_indirizzo)
    Me.NtsPanel3.Controls.Add(Me.edTb_secelaborazione)
    Me.NtsPanel3.Controls.Add(Me.edTb_utente)
    Me.NtsPanel3.Controls.Add(Me.ckTb_elaborazioneautom)
    Me.NtsPanel3.Controls.Add(Me.lbTb_utente)
    Me.NtsPanel3.Controls.Add(Me.lbTb_elaborazioneautom)
    Me.NtsPanel3.Controls.Add(Me.edTb_password)
    Me.NtsPanel3.Controls.Add(Me.btn_connetti)
    Me.NtsPanel3.Controls.Add(Me.lbTb_password)
    Me.NtsPanel3.Dock = System.Windows.Forms.DockStyle.Fill
    Me.NtsPanel3.Location = New System.Drawing.Point(2, 21)
    Me.NtsPanel3.Name = "NtsPanel3"
    Me.NtsPanel3.Size = New System.Drawing.Size(680, 133)
    '
    'lbTb_modalita
    '
    Me.lbTb_modalita.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_modalita.Location = New System.Drawing.Point(4, 75)
    Me.lbTb_modalita.Name = "lbTb_modalita"
    Me.lbTb_modalita.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_modalita.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_modalita.Text = "Modalità"
    Me.lbTb_modalita.UseMnemonic = False
    '
    'cbTb_modalita
    '
    Me.cbTb_modalita.EditValue = ""
    Me.cbTb_modalita.Location = New System.Drawing.Point(106, 75)
    Me.cbTb_modalita.Name = "cbTb_modalita"
    Me.cbTb_modalita.NTSSetparam__DbField = "TESTA.*"
    Me.cbTb_modalita.Properties.AutoHeight = False
    Me.cbTb_modalita.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
    Me.cbTb_modalita.Properties.DropDownRows = 30
    Me.cbTb_modalita.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
    Me.cbTb_modalita.Size = New System.Drawing.Size(200, 20)
    '
    'lbTb_indirizzo
    '
    Me.lbTb_indirizzo.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_indirizzo.Location = New System.Drawing.Point(4, 3)
    Me.lbTb_indirizzo.Name = "lbTb_indirizzo"
    Me.lbTb_indirizzo.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_indirizzo.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_indirizzo.Text = "Indirizzo OPC"
    Me.lbTb_indirizzo.UseMnemonic = False
    '
    'NtsLabel1
    '
    Me.NtsLabel1.AutoEllipsis = True
    Me.NtsLabel1.BackColor = System.Drawing.Color.Transparent
    Me.NtsLabel1.Location = New System.Drawing.Point(194, 99)
    Me.NtsLabel1.Name = "NtsLabel1"
    Me.NtsLabel1.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.NtsLabel1.Size = New System.Drawing.Size(220, 20)
    Me.NtsLabel1.Text = "Tempo Elaborazione espresso in secondi"
    Me.NtsLabel1.UseMnemonic = False
    '
    'edTb_indirizzo
    '
    Me.edTb_indirizzo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.edTb_indirizzo.Cursor = System.Windows.Forms.Cursors.Default
    Me.edTb_indirizzo.Location = New System.Drawing.Point(106, 3)
    Me.edTb_indirizzo.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.edTb_indirizzo.Name = "edTb_indirizzo"
    Me.edTb_indirizzo.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_indirizzo.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_indirizzo.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_indirizzo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_indirizzo.Properties.MaxLength = 65536
    Me.edTb_indirizzo.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_indirizzo.Size = New System.Drawing.Size(564, 20)
    '
    'edTb_secelaborazione
    '
    Me.edTb_secelaborazione.EditValue = "0"
    Me.edTb_secelaborazione.Enabled = False
    Me.edTb_secelaborazione.Location = New System.Drawing.Point(142, 99)
    Me.edTb_secelaborazione.Name = "edTb_secelaborazione"
    Me.edTb_secelaborazione.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_secelaborazione.Properties.Appearance.Options.UseTextOptions = True
    Me.edTb_secelaborazione.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edTb_secelaborazione.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_secelaborazione.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_secelaborazione.Properties.AutoHeight = False
    Me.edTb_secelaborazione.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_secelaborazione.Properties.MaxLength = 65536
    Me.edTb_secelaborazione.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_secelaborazione.Size = New System.Drawing.Size(48, 20)
    '
    'edTb_utente
    '
    Me.edTb_utente.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.edTb_utente.Cursor = System.Windows.Forms.Cursors.Default
    Me.edTb_utente.Location = New System.Drawing.Point(106, 27)
    Me.edTb_utente.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.edTb_utente.Name = "edTb_utente"
    Me.edTb_utente.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_utente.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_utente.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_utente.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_utente.Properties.MaxLength = 65536
    Me.edTb_utente.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_utente.Size = New System.Drawing.Size(564, 20)
    '
    'ckTb_elaborazioneautom
    '
    Me.ckTb_elaborazioneautom.Location = New System.Drawing.Point(106, 99)
    Me.ckTb_elaborazioneautom.Name = "ckTb_elaborazioneautom"
    Me.ckTb_elaborazioneautom.NTSSetparam__DbField = "TESTA.*"
    Me.ckTb_elaborazioneautom.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.ckTb_elaborazioneautom.Properties.Appearance.Options.UseBackColor = True
    Me.ckTb_elaborazioneautom.Properties.Appearance.Options.UseTextOptions = True
    Me.ckTb_elaborazioneautom.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
    Me.ckTb_elaborazioneautom.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.ckTb_elaborazioneautom.Properties.AutoHeight = False
    Me.ckTb_elaborazioneautom.Properties.Caption = ""
    Me.ckTb_elaborazioneautom.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined
    Me.ckTb_elaborazioneautom.Properties.PictureChecked = CType(resources.GetObject("ckTb_elaborazioneautom.Properties.PictureChecked"), System.Drawing.Image)
    Me.ckTb_elaborazioneautom.Properties.PictureUnchecked = CType(resources.GetObject("ckTb_elaborazioneautom.Properties.PictureUnchecked"), System.Drawing.Image)
    Me.ckTb_elaborazioneautom.Size = New System.Drawing.Size(32, 20)
    '
    'lbTb_utente
    '
    Me.lbTb_utente.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_utente.Location = New System.Drawing.Point(4, 27)
    Me.lbTb_utente.Name = "lbTb_utente"
    Me.lbTb_utente.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_utente.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_utente.Text = "Utente OPC"
    Me.lbTb_utente.UseMnemonic = False
    '
    'lbTb_elaborazioneautom
    '
    Me.lbTb_elaborazioneautom.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_elaborazioneautom.Location = New System.Drawing.Point(4, 99)
    Me.lbTb_elaborazioneautom.Name = "lbTb_elaborazioneautom"
    Me.lbTb_elaborazioneautom.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_elaborazioneautom.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_elaborazioneautom.Text = "Elab. Automatica"
    Me.lbTb_elaborazioneautom.UseMnemonic = False
    '
    'edTb_password
    '
    Me.edTb_password.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.edTb_password.Cursor = System.Windows.Forms.Cursors.Default
    Me.edTb_password.Location = New System.Drawing.Point(106, 51)
    Me.edTb_password.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.edTb_password.Name = "edTb_password"
    Me.edTb_password.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_password.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_password.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_password.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_password.Properties.MaxLength = 65536
    Me.edTb_password.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_password.Size = New System.Drawing.Size(564, 20)
    '
    'btn_connetti
    '
    Me.btn_connetti.Location = New System.Drawing.Point(422, 99)
    Me.btn_connetti.Name = "btn_connetti"
    Me.btn_connetti.Size = New System.Drawing.Size(132, 26)
    Me.btn_connetti.Text = "Connetti"
    '
    'lbTb_password
    '
    Me.lbTb_password.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_password.Location = New System.Drawing.Point(4, 51)
    Me.lbTb_password.Name = "lbTb_password"
    Me.lbTb_password.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_password.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_password.Text = "Password OPC"
    Me.lbTb_password.UseMnemonic = False
    '
    'pnlMacchine
    '
    Me.pnlMacchine.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.pnlMacchine.Controls.Add(Me.fmMacchine)
    Me.pnlMacchine.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlMacchine.Location = New System.Drawing.Point(0, 99)
    Me.pnlMacchine.Name = "pnlMacchine"
    Me.pnlMacchine.Size = New System.Drawing.Size(684, 412)
    '
    'fmMacchine
    '
    Me.fmMacchine.AllowDrop = True
    Me.fmMacchine.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmMacchine.Appearance.Options.UseBackColor = True
    Me.fmMacchine.Controls.Add(Me.grMacchine)
    Me.fmMacchine.Dock = System.Windows.Forms.DockStyle.Fill
    Me.fmMacchine.Location = New System.Drawing.Point(0, 0)
    Me.fmMacchine.Name = "fmMacchine"
    Me.fmMacchine.Size = New System.Drawing.Size(684, 412)
    Me.fmMacchine.Text = "MACCHINE"
    '
    'grMacchine
    '
    Me.grMacchine.AllowDrop = True
    Me.grMacchine.Dock = System.Windows.Forms.DockStyle.Fill
    GridLevelNode1.RelationName = "Level1"
    Me.grMacchine.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
    Me.grMacchine.Location = New System.Drawing.Point(2, 21)
    Me.grMacchine.MainView = Me.grvMacchine
    Me.grMacchine.MenuManager = Me.NtsBarManager1
    Me.grMacchine.Name = "grMacchine"
    Me.grMacchine.Size = New System.Drawing.Size(680, 389)
    Me.grMacchine.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvMacchine})
    '
    'grvMacchine
    '
    Me.grvMacchine.Appearance.FocusedCell.BackColor = System.Drawing.Color.FloralWhite
    Me.grvMacchine.Appearance.FocusedCell.Options.UseBackColor = True
    Me.grvMacchine.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.tb_codice})
    Me.grvMacchine.Enabled = True
    Me.grvMacchine.GridControl = Me.grMacchine
    Me.grvMacchine.Name = "grvMacchine"
    Me.grvMacchine.OptionsCustomization.AllowRowSizing = True
    Me.grvMacchine.OptionsNavigation.EnterMoveNextColumn = True
    Me.grvMacchine.OptionsNavigation.UseTabKey = False
    Me.grvMacchine.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.grvMacchine.OptionsView.ColumnAutoWidth = False
    Me.grvMacchine.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.grvMacchine.OptionsView.ShowGroupPanel = False
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
    'FRMHHIOPC
    '
    Me.ClientSize = New System.Drawing.Size(684, 511)
    Me.Controls.Add(Me.pnlCorpo)
    Me.Controls.Add(Me.pnlMacchine)
    Me.Controls.Add(Me.pnlOpz)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.Name = "FRMHHIOPC"
    Me.Text = "IMPOSTAZIONE OPC-UA"
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pnlOpz, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlOpz.ResumeLayout(False)
    CType(Me.fmMacchina, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmMacchina.ResumeLayout(False)
    CType(Me.edTb_codice.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pnlCorpo, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlCorpo.ResumeLayout(False)
    CType(Me.NtsPanel5, System.ComponentModel.ISupportInitialize).EndInit()
    Me.NtsPanel5.ResumeLayout(False)
    CType(Me.fmNodi, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmNodi.ResumeLayout(False)
    CType(Me.grNodi, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grvNodi, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsPanel1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.NtsPanel1.ResumeLayout(False)
    CType(Me.NtsPanel2, System.ComponentModel.ISupportInitialize).EndInit()
    Me.NtsPanel2.ResumeLayout(False)
    CType(Me.edTb_nodo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fmImpostazioni, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmImpostazioni.ResumeLayout(False)
    CType(Me.NtsPanel3, System.ComponentModel.ISupportInitialize).EndInit()
    Me.NtsPanel3.ResumeLayout(False)
    CType(Me.cbTb_modalita.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_indirizzo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_secelaborazione.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_utente.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ckTb_elaborazioneautom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_password.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pnlMacchine, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlMacchine.ResumeLayout(False)
    CType(Me.fmMacchine, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmMacchine.ResumeLayout(False)
    CType(Me.grMacchine, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grvMacchine, System.ComponentModel.ISupportInitialize).EndInit()
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
        tlbApri.GlyphPath = (oApp.ChildImageDir & "\Open.png")
        tlbSalva.GlyphPath = (oApp.ChildImageDir & "\save.png")
        tlbRipristina.GlyphPath = (oApp.ChildImageDir & "\restore.png")
        tlbRipristinaRiga.GlyphPath = (oApp.ChildImageDir & "\Recrestore.png")
        tlbCancella.GlyphPath = (oApp.ChildImageDir & "\delete.png")
        tlbCancellaRiga.GlyphPath = (oApp.ChildImageDir & "\Recdelete.png")
        tlbZoom.GlyphPath = (oApp.ChildImageDir & "\zoom.png")
        tlbGuida.GlyphPath = (oApp.ChildImageDir & "\help.png")
        tlbEsci.GlyphPath = (oApp.ChildImageDir & "\exit.png")
      Catch ex As Exception
        'non gestisco l'errore: se non c'è una immagine prendo quella standard
      End Try
      tlbMain.NTSSetToolTip()

      edTb_codice.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964278, "Codice"), 255, True)

      '--------------------------------------------------
      '--- Griglia Macchine
      '--------------------------------------------------
      grvMacchine.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964279, "Elenco Macchine"))
      grvMacchine.NTSAllowInsert = False
      'grvMacchine.NTSHideMenuSx()

      tb_codice.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130995879126964280, tb_codice.Caption), 255, True)
      'tb_descrizione.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130995879126964281, tb_descrizione.Caption), 255, True)

      tb_codice.NTSSetRichiesto()

      'tb_descrizione.NTSSetRichiesto()

      '-------------------------------------------------
      '--- Carica il combo delle modalità
      '-------------------------------------------------
      'Dim dsMod As New DataSet
      Dim dttMod As New DataTable
      dttMod.Columns.Add("cod", GetType(String))
      dttMod.Columns.Add("val", GetType(String))
      'ocleIopc.ApriModalita(dsMod)
      'dttMod = dsMod.Tables("MODA").Copy
      'If dttMod.Rows.Count > 0 Then
      '  dttMod.Rows.Add("")
      'End If
      dttMod.Rows.Add("", "")
      dttMod.Rows.Add("DMG_DMU", "DMG_DMU")
      dttMod.Rows.Add("DMG_CTX", "DMG_CTX")
      dttMod.Rows.Add("IFP", "IFP")

      edTb_indirizzo.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964281, lbTb_indirizzo.Text), 255, True)
      edTb_utente.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964282, lbTb_utente.Text), 255, True)
      edTb_password.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964283, lbTb_password.Text), 255, True)
      edTb_nodo.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964284, "Nodo"), 255, True)
      cbTb_modalita.NTSSetParam(oApp.Tr(Me, 130995879126964285, lbTb_modalita.Text))
      ckTb_elaborazioneautom.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964286, lbTb_elaborazioneautom.Text), "S", "N")
      edTb_secelaborazione.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964287, lbTb_elaborazioneautom.Text), "0", 5, 0, 99999)

      cbTb_modalita.DataSource = dttMod
      cbTb_modalita.ValueMember = "cod"
      cbTb_modalita.DisplayMember = "val"

      '--------------------------------------------------
      '--- Griglia Nodi
      '--------------------------------------------------
      grvNodi.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964289, "Elenco Nodi"))
      grvNodi.NTSAllowInsert = True
      'grvNodi.NTSHideMenuSx()

      '-------------------------------------------------
      '--- Carica il combo dei tipi Dato
      '-------------------------------------------------
      Dim dttTipo As New DataTable
      dttTipo.Columns.Add("cod", GetType(String))
      dttTipo.Columns.Add("val", GetType(String))
      dttTipo.Rows.Add(ocleIopc.ARTICOLO, ocleIopc.ARTICOLO)
      dttTipo.Rows.Add(ocleIopc.COMMESSA, ocleIopc.COMMESSA)
      dttTipo.Rows.Add(ocleIopc.ORDINE, ocleIopc.ORDINE)
      dttTipo.Rows.Add(ocleIopc.RICETTA, ocleIopc.RICETTA)
      dttTipo.Rows.Add(ocleIopc.TEMPO, ocleIopc.TEMPO)
      dttTipo.Rows.Add(ocleIopc.QTADAFARE, ocleIopc.QTADAFARE)
      dttTipo.Rows.Add(ocleIopc.QTAFATTA, ocleIopc.QTAFATTA)
      dttTipo.Rows.Add(ocleIopc.SECONDI, ocleIopc.SECONDI)
      dttTipo.Rows.Add(ocleIopc.DATA, ocleIopc.DATA)
      dttTipo.Rows.Add(ocleIopc.ORA, ocleIopc.ORA)
      dttTipo.Rows.Add(ocleIopc.DATAORA, ocleIopc.DATAORA)
      dttTipo.Rows.Add(ocleIopc.STATO, ocleIopc.STATO)
      dttTipo.Rows.Add(ocleIopc.PROGRAMMA, ocleIopc.PROGRAMMA)
      dttTipo.Rows.Add(ocleIopc.DATASTART, ocleIopc.DATASTART)
      dttTipo.Rows.Add(ocleIopc.DATASTOP, ocleIopc.DATASTOP)
      dttTipo.Rows.Add(ocleIopc.FLAGSTART, ocleIopc.FLAGSTART)
      dttTipo.Rows.Add(ocleIopc.FLAGSTOP, ocleIopc.FLAGSTOP)
      dttTipo.Rows.Add(ocleIopc.ANNOSTART, ocleIopc.ANNOSTART)
      dttTipo.Rows.Add(ocleIopc.MESESTART, ocleIopc.MESESTART)
      dttTipo.Rows.Add(ocleIopc.GIORNOSTART, ocleIopc.GIORNOSTART)
      dttTipo.Rows.Add(ocleIopc.ORASTART, ocleIopc.ORASTART)
      dttTipo.Rows.Add(ocleIopc.MINUTOSTART, ocleIopc.MINUTOSTART)
      dttTipo.Rows.Add(ocleIopc.SECONDOSTART, ocleIopc.SECONDOSTART)
      dttTipo.Rows.Add(ocleIopc.ANNOSTOP, ocleIopc.ANNOSTOP)
      dttTipo.Rows.Add(ocleIopc.MESESTOP, ocleIopc.MESESTOP)
      dttTipo.Rows.Add(ocleIopc.GIORNOSTOP, ocleIopc.GIORNOSTOP)
      dttTipo.Rows.Add(ocleIopc.ORASTOP, ocleIopc.ORASTOP)
      dttTipo.Rows.Add(ocleIopc.MINUTOSTOP, ocleIopc.MINUTOSTOP)
      dttTipo.Rows.Add(ocleIopc.SECONDOSTOP, ocleIopc.SECONDOSTOP)
      dttTipo.Rows.Add(ocleIopc.STINGA1, ocleIopc.STINGA1)
      dttTipo.Rows.Add(ocleIopc.STINGA2, ocleIopc.STINGA2)
      dttTipo.Rows.Add(ocleIopc.STINGA3, ocleIopc.STINGA3)
      dttTipo.Rows.Add(ocleIopc.STINGA4, ocleIopc.STINGA4)
      dttTipo.Rows.Add(ocleIopc.STINGA5, ocleIopc.STINGA5)
      dttTipo.Rows.Add(ocleIopc.STINGA6, ocleIopc.STINGA6)
      dttTipo.Rows.Add(ocleIopc.STINGA7, ocleIopc.STINGA7)
      dttTipo.Rows.Add(ocleIopc.STINGA8, ocleIopc.STINGA8)
      dttTipo.Rows.Add(ocleIopc.FLAG1, ocleIopc.FLAG1)
      dttTipo.Rows.Add(ocleIopc.FLAG2, ocleIopc.FLAG2)
      dttTipo.Rows.Add(ocleIopc.FLAG3, ocleIopc.FLAG3)
      dttTipo.Rows.Add(ocleIopc.FLAG4, ocleIopc.FLAG4)
      dttTipo.Rows.Add(ocleIopc.FLAG5, ocleIopc.FLAG5)
      dttTipo.Rows.Add(ocleIopc.FLAG6, ocleIopc.FLAG6)
      dttTipo.Rows.Add(ocleIopc.FLAG7, ocleIopc.FLAG7)
      dttTipo.Rows.Add(ocleIopc.FLAG8, ocleIopc.FLAG8)

      tb_nodo.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130995879126964290, tb_nodo.Caption), 255, True)
      tb_tiponodo.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130995879126964291, tb_tiponodo.Caption), 255, True)
      tb_tipo.NTSSetParamCMB(oMenu, oApp.Tr(Me, 130995879126964292, tb_tipo.Caption), dttTipo, "cod", "val")
      tb_ordinamento.NTSSetParamNUM(oMenu, oApp.Tr(Me, 130995879126964293, tb_ordinamento.Caption), "0", 5, 0, 99999)

      tb_nodo.NTSSetRichiesto()
      tb_tiponodo.NTSSetRichiesto()
      tb_tipo.NTSSetRichiesto()
      tb_ordinamento.NTSSetRichiesto()


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
    If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNHHIOPC", "BEHHIOPC", oTmp, strErr, False, "", "") = False Then
      oApp.MsgBoxErr(oApp.Tr(Me, 128704611604674454, "ERRORE in fase di creazione Entity:  " & vbCrLf & "|" & strErr & "|"))
      Return False
    End If

    ocleIopc = CType(oTmp, CLEHHIOPC)

    '--------------------------------------------------
    '--- Aggiunge gestore eventi
    '--------------------------------------------------
    AddHandler ocleIopc.RemoteEvent, AddressOf GestisciEventiEntity
    If ocleIopc.Init(oApp, oScript, oMenu.oCleComm, "TESTA", False, "", "") = False Then Return False

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
        Case "SET_FOCUS"
          Select Case e.Message.ToLower   'CONTROLLARE TUTTI CON MINUSCOLO
            Case "tb_nodo"
              grvNodi.FocusedColumn = tb_nodo
          End Select
      End Select

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub
#End Region

#Region "Eventi Form"
  Public Overridable Sub FRMHHIOPC_ActivatedFirst(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ActivatedFirst
    Try
      '--------------------------------------------------
      '--- Apre le OPZIONI impostate
      '--------------------------------------------------
      'If Not Apri() Then
      '  Me.Close()
      '  Return
      'End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub FRMHHIOPC_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
    Try
      'Disabilito eventi di validazione su Entity
      'Distrugge oggetti
      dcIopcTesta.Dispose()
      dcIopcCorpo.Dispose()
      dsIopc.Dispose()

      ocleIopc.ChiudiOPCUA(ocleIopc.client, ocleIopc.subscribe)

    Catch
    End Try
  End Sub

  Public Overridable Sub FRMHHIOPC_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    Try
      '--------------------------------------------------
      '--- Salva il profilo
      '--------------------------------------------------

      If e.Cancel Then e.Cancel = False

      If Not dsIopc.Tables("TESTA") Is Nothing Then
        If Not dsIopc.Tables("TESTA").Rows.Count = 0 Then
          If Not Salva(False) Then e.Cancel = True
        End If
      End If

      'If Not Salva(False) Then e.Cancel = True

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub FRMHHIOPC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Try
      '--------------------------------------------------
      '--- Imposta flag di Load Form
      '--------------------------------------------------
      bInLoad = True

      '--------------------------------------------------
      '--- Leggo i parametri passati dal child chiamante
      '--------------------------------------------------
      If Not oCallParams Is Nothing Then
        '--------------------------------------------------
        '--- Legge parametri per impostazioni connessione
        '--------------------------------------------------
      End If
      '--------------------------------------------------
      '--- Predispongo i controlli
      '--------------------------------------------------
      InitControls()

      'pnlMacchine.Visible = True
      'pnlCorpo.Visible = False
      'tlbApri.Enabled = False

      SetStato(0)

      ApriMacchine()

      '--------------------------------------------------
      '--- Sempre alla fine di questa funzione: applico le regole della gctl
      '--------------------------------------------------
      GctlSetRoules()

      'grvMacchine.Columns("tb_codice").BestFit()

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

#Region "Eventi Toolbar"
  Public Overridable Sub tlbNuovo_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbNuovo.ItemClick
    'Dim frmHHMacc As FRMHHMACC = Nothing
    Try

      edTb_codice.Text = ""

      SetStato(1)

      Nuovo()

      edTb_codice.Enabled = True
      edTb_codice.Focus()

      'pnlCorpo.Visible = True
      'pnlMacchine.Visible = False

      'tlbApri.Enabled = True

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub tlbApri_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbApri.ItemClick
    Try

      ocleIopc.bNew = False

      If Not dsIopc.Tables("TESTA") Is Nothing Then
        If Not dsIopc.Tables("TESTA").Rows.Count = 0 Then Salva(False)
      End If

      edTb_codice.Text = ""
      ocleIopc.strMacchina = ""

      SetStato(0)

      ApriMacchine()

      edTb_codice.Focus()
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub tlbSalva_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbSalva.ItemClick
    Try
      Salva(False)
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

  Public Overridable Sub tlbCancella_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbCancella.ItemClick
    Try
      Cancella()
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbCancellaRiga_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbCancellaRiga.ItemClick
    Try
      If grvNodi.RowCount = 0 Then Return
      CancellaRiga()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbRipristinaRiga_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbRipristinaRiga.ItemClick
    Try
      If grvNodi.RowCount = 0 Then Return
      RipristinaRiga(True)
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbZoom_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbZoom.ItemClick
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

      Select Case ctrlTmp.Name.ToLower
        Case Else
          'Per eventuali altri controlli caricati al volo
          'Zoom standard di Textbox e Griglia
          NTSCallStandardZoom()
      End Select
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      'If Not frmHlpr Is Nothing Then frmHlpr.Dispose()
      'frmHlpr = Nothing
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

#Region "Eventi Controlli"
  Public Overridable Sub ckTb_elaborazioneautom_CheckedChanged(sender As Object, e As EventArgs) Handles ckTb_elaborazioneautom.CheckedChanged
    Try
      If ckTb_elaborazioneautom.Checked Then
        edTb_secelaborazione.Enabled = True
      Else
        edTb_secelaborazione.Enabled = False
      End If
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub btn_cerca_Click(sender As Object, e As EventArgs) Handles btn_cerca.Click
    Try
      ApriMacchine()
      edTb_codice.Focus()
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub btn_connetti_Click(sender As Object, e As EventArgs) Handles btn_connetti.Click
    Try
      treeNodi.Nodes.Clear()
      If ocleIopc.ConnettiOPCUA(ocleIopc.client, ocleIopc.subscribe) Then
        Dim dtNodes As New DataTable
        ocleIopc.LeggiNodi(ocleIopc.client, "", dtNodes)
        For Each drNodes As DataRow In dtNodes.Rows
          treeNodi.Nodes.Add(NTSCStr(drNodes!tb_idnodo), NTSCStr(drNodes!tb_nodo))
          'obj.Nodes.Add(node.NodeId, node.Name)
          Dim nodeadd As TreeNode = treeNodi.Nodes.Item(NTSCStr(drNodes!tb_idnodo))
          nodeadd.Nodes.Add("")
          nodeadd.Collapse()
        Next
      Else
        oApp.MsgBoxInfo(oApp.Tr(Me, 128306256614868399, "Connessione Fallita!"))
      End If
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub treeNodi_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles treeNodi.AfterSelect
    Try
      edTb_nodo.Text = e.Node.FullPath
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub treeNodi_AfterExpand(sender As Object, e As TreeViewEventArgs) Handles treeNodi.AfterExpand
    Try
      'txtNodo.Text = e.Node.FullPath
      edTb_nodo.Text = e.Node.FullPath
      treeNodi.SelectedNode = e.Node
      Dim dtNodes As New DataTable
      ocleIopc.LeggiNodi(ocleIopc.client, e.Node.FullPath, dtNodes)
      e.Node.Nodes.Clear()
      For Each drNodes As DataRow In dtNodes.Rows
        e.Node.Nodes.Add(NTSCStr(drNodes!tb_idnodo), NTSCStr(drNodes!tb_nodo))
        'obj.Nodes.Add(node.NodeId, node.Name)
        Dim nodeadd As TreeNode = e.Node.Nodes.Item(NTSCStr(drNodes!tb_idnodo))
        nodeadd.Nodes.Add("")
        nodeadd.Collapse()
      Next
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
      'MsgBox("Errore in treeNodi AfterExpand: " & ex.Message)
    End Try
  End Sub

  Public Overridable Sub treeNodi_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles treeNodi.NodeMouseDoubleClick
    Try
      Me.Cursor = Cursors.WaitCursor
      treeNodi.SelectedNode = e.Node
      ocleIopc.AggiungiRiga(ocleIopc.client, e.Node.FullPath)
      ocleIopc.RecordSalva(dcIopcCorpo.Position, False, Nothing)
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub edTb_codice_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles edTb_codice.Validating
    Try

      If ocleIopc.bNew Then
        If Trim(edTb_codice.Text) = "" Then e.Cancel = True
      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub edTb_codice_Validated(sender As Object, e As EventArgs) Handles edTb_codice.Validated
    Try

      ocleIopc.strMacchina = edTb_codice.Text

      If ocleIopc.bNew Then
        ocleIopc.bNew = False

        edTb_codice.Enabled = False
        dsIopc.Tables("TESTA").Columns("tb_codice").DefaultValue = ocleIopc.strMacchina
        dsIopc.Tables("CORPO").Columns("tb_codice").DefaultValue = ocleIopc.strMacchina

        If dsIopc.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(ocleIopc.strMacchina)).Length > 0 Then
          Apri()
        End If

      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub edTb_codice_KeyDown(sender As Object, e As KeyEventArgs) Handles edTb_codice.KeyDown
    Try
      '---------------------------------------------------------------------------------------------------------
      If e.KeyCode = Keys.Enter Then

        If ocleIopc.bNew Then Return
        ApriMacchine()

        'If dsMacc.Tables("TESTA").Select("tb_codice = " & CStrSQL(edTb_codice.Text)).Length = 0 Then
        '  oApp.MsgBoxInfo(oApp.Tr(Me, 128306256614868393, "Macchina non presente in anagrafica"))
        '  Return
        'End If

        'ocleIopc.strMacchina = edTb_codice.Text

        'SetStato(1)

        'Apri()

        'edTb_codice.Enabled = False

        ''pnlCorpo.Visible = True
        ''pnlMacchine.Visible = False

        ''tlbApri.Enabled = True
      End If
      '---------------------------------------------------------------------------------------------------------
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try
  End Sub
#End Region

#Region "Eventi Griglia Nodi"
  Public Overridable Sub grvNodi_NTSBeforeRowUpdate(sender As Object, e As DevExpress.XtraGrid.Views.Base.RowAllowEventArgs) Handles grvNodi.NTSBeforeRowUpdate
    Try
      If dcIopcCorpo.Position < 0 Then Return

      Me.Cursor = Cursors.WaitCursor
      If Not ocleIopc.RecordSalva(dcIopcCorpo.Position, False, Nothing) Then e.Allow = False 'rimango sulla riga su cui sono
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub grvNodi_NTSFocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles grvNodi.NTSFocusedRowChanged
    Try
      If grvNodi Is Nothing Then Return

      'If grvNodi.FocusedRowHandle = NTSGrid.AutoFilterRowHandle Then Return
      'If grvNodi.FocusedRowHandle <> NTSGridView.NUOVA_RIGA Then
      '  If NTSCStr(grvNodi.GetDataRow(grvNodi.FocusedRowHandle) !tb_nodo) = "" Then
      '    oApp.MsgBoxInfo("Attenzione! Il Nodo non può essere vuoto")
      '  End If
      'End If

      If grvNodi Is Nothing Then Return

      If grvNodi.FocusedRowHandle = NTSGrid.AutoFilterRowHandle Then Return

      If grvNodi.FocusedRowHandle = NTSGridView.NESSUNA_RIGA Then Return
      If Not grvNodi.FocusedRowHandle = NTSGridView.NUOVA_RIGA Then
        Select Case grvNodi.GetDataRow(grvNodi.FocusedRowHandle).RowState
          Case DataRowState.Added
            If Not tb_nodo.Enabled Then
              GctlSetVisEnab(tb_nodo, False)
              '        GctlSetVisEnab(tb_tiponodo, False)
            End If
          Case Else
            tb_nodo.Enabled = False
            '      tb_tiponodo.Enabled = False
        End Select
      Else
        If Not tb_nodo.Enabled Then
          GctlSetVisEnab(tb_nodo, False)
          '    GctlSetVisEnab(tb_tiponodo, False)
        End If
        grvNodi.NTSMoveFirstColunn()
      End If
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub grvMacchine_DoubleClick(sender As Object, e As EventArgs) Handles grvMacchine.DoubleClick
    Try
      If grvMacchine.GetFocusedDataRow Is Nothing Then Return
      ocleIopc.strMacchina = NTSCStr(grvMacchine.GetFocusedDataRow!tb_codice)
      edTb_codice.Text = ocleIopc.strMacchina

      SetStato(1)

      Apri()

      edTb_codice.Enabled = False
      edTb_indirizzo.Focus()

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub
#End Region

#Region "Funzioni e Routines"
  Public Overridable Function SetStato(ByVal nStato As Integer) As Boolean
    Try

      Select Case nStato
        Case 0
          pnlMacchine.Visible = True
          pnlCorpo.Visible = False
          tlbApri.Enabled = False
          tlbSalva.Enabled = False
          tlbRipristina.Enabled = False
          tlbRipristinaRiga.Enabled = False
          tlbCancella.Enabled = False
          tlbCancellaRiga.Enabled = False

          'tlbNuovo.Enabled = True

          edTb_codice.Enabled = True

          btn_cerca.Visible = True

        Case 1
          pnlCorpo.Visible = True
          pnlMacchine.Visible = False
          tlbApri.Enabled = True
          tlbSalva.Enabled = True
          tlbRipristina.Enabled = True
          tlbRipristinaRiga.Enabled = True
          tlbCancella.Enabled = True
          tlbCancellaRiga.Enabled = True

          'tlbNuovo.Enabled = False

          btn_cerca.Visible = False

          treeNodi.Nodes.Clear()

      End Select

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Function Apri() As Boolean
    Try
      'If bInLoad = True Then Return True
      If dsIopc Is Nothing Then Return True

      '--------------------------------------------------
      '--- Apre le connessioni impostate
      '--------------------------------------------------
      dsIopc = Nothing
      dsIopc = New DataSet
      'If Not ocleIopc.Apri(dsIopc) Then Return False

      Dim ds As New DataSet

      'If Trim(ocleIopc.strMacchina) = "" Then Return False
      If Not ocleIopc.Apri(dsIopc) Then Return False

      dcIopcTesta = New BindingSource
      dcIopcCorpo = New BindingSource

      dcIopcTesta.DataSource = dsIopc.Tables("TESTA")
      dcIopcCorpo.DataSource = dsIopc.Tables("CORPO")

      dsIopc.AcceptChanges()

      '--------------------------------------------------
      '--- Collega i controlli della Form 
      '--------------------------------------------------
      Bindcontrols()

      '--------------------------------------------------
      '--- Imposta datacontrol per lista
      '--------------------------------------------------
      grNodi.DataSource = dcIopcCorpo

      ''--------------------------------------------------
      ''--- Valore funzione
      ''--------------------------------------------------
      Return True

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Function Nuovo() As Boolean
    Try
      'If bInLoad = True Then Return True
      If dsIopc Is Nothing Then Return True

      '--------------------------------------------------
      '--- Apre le connessioni impostate
      '--------------------------------------------------
      dsIopc = Nothing
      dsIopc = New DataSet
      'If Not ocleIopc.Apri(dsIopc) Then Return False

      Dim ds As New DataSet

      ocleIopc.strMacchina = ""

      'If Trim(ocleIopc.strMacchina) = "" Then Return False
      If Not ocleIopc.Apri(dsIopc) Then Return False

      dsIopc.Tables("TESTA").Rows.Add(dsIopc.Tables("TESTA").NewRow)

      dcIopcTesta = New BindingSource
      dcIopcCorpo = New BindingSource

      dcIopcTesta.DataSource = dsIopc.Tables("TESTA")
      dcIopcCorpo.DataSource = dsIopc.Tables("CORPO")

      'dsIopc.AcceptChanges()

      '--------------------------------------------------
      '--- Collega i controlli della Form 
      '--------------------------------------------------
      Bindcontrols()

      '--------------------------------------------------
      '--- Imposta datacontrol per lista
      '--------------------------------------------------
      grNodi.DataSource = dcIopcCorpo

      ocleIopc.bNew = True

      ''--------------------------------------------------
      ''--- Valore funzione
      ''--------------------------------------------------
      Return True

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Function ApriMacchine() As Boolean
    Try

      If dsMacc Is Nothing Then Return True

      '--------------------------------------------------
      '--- Apre le connessioni impostate
      '--------------------------------------------------
      dsMacc = Nothing
      dsMacc = New DataSet
      'If Not ocleIopc.Apri(dsIopc) Then Return False

      Dim ds As New DataSet

      'If Trim(ocleIopc.strMacchina) = "" Then Return False
      If Not ocleIopc.ApriMacchina(edTb_codice.Text, dsMacc) Then Return False

      dcMacc = New BindingSource

      dcMacc.DataSource = dsMacc.Tables("MACCHINE")

      dsMacc.AcceptChanges()

      '--------------------------------------------------
      '--- Imposta datacontrol per lista
      '--------------------------------------------------
      grMacchine.DataSource = dcMacc

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Function

  Public Overridable Sub Bindcontrols()
    Try
      '--------------------------------------------------
      '--- Se i controlli erano già stati precedentemente collegati, li scollego
      '--------------------------------------------------
      NTSFormClearDataBinding(Me)

      '--------------------------------------------------
      '--- Per agganciare al dataset i vari controlli
      '--------------------------------------------------
      NTSFormAddDataBinding(dcIopcTesta, Me)
      'NTSFormAddDataBinding(dcInteCorpo, Me)

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub Cancella()
    Try
      '-------------------------------------------------
      '--- Cancellazione il profilo
      '-------------------------------------------------

      Select Case Me.NTSChiediCancella(Nothing, oApp.Tr(Me, 128306256431033721, "Confermi la cancellazione dei settaggi?"))
        Case System.Windows.Forms.DialogResult.No
          Return
        Case System.Windows.Forms.DialogResult.Yes
          'dsIopc.Tables("TESTA").Rows.Clear()
          'dsIopc.Tables("CORPO").Rows.Clear()

          'dsIopc.AcceptChanges()

          'If ocleIopc.dsShared.Tables("TESTA").Select("tb_codice = " & CStrSQL(ocleIopc.strMacchina)).Length > 0 Then
          '  ocleIopc.dsShared.Tables("TESTA").Select("tb_codice = " & CStrSQL(ocleIopc.strMacchina))(0).Delete()
          '  ocleIopc.dsShared.AcceptChanges()
          'End If

          'For Each drDelete As DataRow In ocleIopc.dsShared.Tables("CORPO").Select("tb_codice = " & CStrSQL(ocleIopc.strMacchina))
          '  drDelete.Delete()
          'Next

          'ocleIopc.dsShared.AcceptChanges()

          If dsIopc.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(ocleIopc.strMacchina)).Length > 0 Then
            Dim drMacchine As DataRow = dsIopc.Tables("MACCHINE").Select("tb_codice = " & CStrSQL(ocleIopc.strMacchina))(0)
            If NTSCInt(drMacchine!tb_startstop) = 1 Then
              oApp.MsgBoxExclamation(oApp.Tr(Me, 128306256431033721, "Non è possibile cancellare questa macchina perchè c'è già un processo aperto." & vbCrLf & "prima chiudere i processi aperti per questa macchina"))
              Return
            End If
          End If

          dsIopc.Tables("MACCHINE").Rows(0).Delete()

          If dsIopc.Tables("TESTA").Rows.Count = 0 Then Return
          dsIopc.Tables("TESTA").Rows(0).Delete()

          If ocleIopc.Salva(True) Then
            ocleIopc.strMacchina = ""
            edTb_codice.Enabled = True
            edTb_codice.Focus()
            Nuovo()
            treeNodi.Nodes.Clear()
          End If

      End Select

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub CancellaRiga()
    Try

      If grvNodi.GetFocusedDataRow Is Nothing Then Return
      If grvNodi.GetFocusedDataRow.RowState = DataRowState.Detached Then Return

      '-------------------------------------------------
      '--- Cancellazione il profilo
      '-------------------------------------------------
      Select Case Me.NTSChiediCancella(Nothing, oApp.Tr(Me, 128306256431033721, "Cancellare la riga?"))
        Case System.Windows.Forms.DialogResult.No
          Return
        Case System.Windows.Forms.DialogResult.Yes
          Dim drDelete As DataRow = Nothing
          ocleIopc.RecordSalva(dcIopcCorpo.Position, True, drDelete)
          'dcInteCorpo.RemoveAt(dcInteCorpo.Position)
          'ocleIopc.Salva(True)

          'Lo imposto qui e non in Entity 
          ocleIopc.bHasChanges = True
      End Select

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub Ripristina(ByVal bShowDialog As Boolean)
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
        dlgRes = oApp.MsgBoxInfoYesNo_DefYes(oApp.Tr(Me, 128306256614868393, "Ripristinare le modifiche apportate ai settaggi?"))
      Else
        'chiamato dalla 'salva
        dlgRes = System.Windows.Forms.DialogResult.Yes
      End If

      Select Case dlgRes
        Case System.Windows.Forms.DialogResult.No
          Return
        Case System.Windows.Forms.DialogResult.Yes
          'Esegue il ripristino 
          ocleIopc.RipristinaIopc(dsIopc, "TESTA", dcIopcTesta.Position, dcIopcTesta.Filter)
          'Esegue il ripristino 
          ocleIopc.RipristinaIopc(dsIopc, "CORPO", dcIopcCorpo.Position, dcIopcCorpo.Filter)

          Dim bReset As Boolean = False
          If dsIopc.Tables("TESTA").Rows.Count = 0 Then bReset = True
          If Not bReset Then
            If Trim(NTSCStr(dsIopc.Tables("TESTA").Rows(0) !tb_codice)) = "" Then bReset = True
          End If

          If bReset Then
            ocleIopc.strMacchina = ""
            edTb_codice.Enabled = True
            edTb_codice.Focus()
            ocleIopc.bNew = True
            treeNodi.Nodes.Clear()
          End If

          'Esegue il ripristino 
          'If Not ocleIopc.RipristinaIopc(dsIopc, "TESTA", dcIopcTesta.Position, dcIopcTesta.Filter) Then Return
          'Esegue il ripristino 
          'If Not ocleIopc.RipristinaIopc(dsIopc, "CORPO", dcIopcCorpo.Position, dcIopcCorpo.Filter) Then Return

          'Lo imposto qui e non in Entity 
          ocleIopc.bHasChanges = False
      End Select

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub RipristinaRiga(ByVal bShowDialog As Boolean)
    Try
      '--------------------------------------------------
      '--- Se se in caricamento Form
      '--------------------------------------------------
      If bInLoad = True Then Return

      '--------------------------------------------------
      '--- Valido l'ultimo controllo che ha il focus
      '--------------------------------------------------
      Me.ValidaLastControl()      '

      If grvNodi.GetFocusedDataRow Is Nothing Then Return
      If grvNodi.GetFocusedDataRow.RowState = DataRowState.Detached Then Return

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
          'Esegue il ripristino 
          If Not ocleIopc.RipristinaIopc(dsIopc, "CORPO", dcIopcCorpo.Position, dcIopcCorpo.Filter) Then Return

          'Lo imposto qui e non in Entity 
          'ocleIopc.bHasChanges = False
      End Select

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Function Salva(ByVal bDelete As Boolean) As Boolean
    Dim bResult As Boolean = False
    Try

      If Trim(ocleIopc.strMacchina) = "" Then Return True

      '--------------------------------------------------
      '--- Se se in caricamento Form
      '--------------------------------------------------
      If bInLoad = True Then Return True

      '--------------------------------------------------
      '--- Valido l'ultimo controllo che ha il focus
      '--------------------------------------------------
      If Not edTb_codice.Focused Then Me.ValidaLastControl()

      '--------------------------------------------------
      '--- Salva profilo
      '--------------------------------------------------
      'Me.NTSRipristinaNuovoSenzaModifiche(ocleInte.bHasChanges, dcInte)
      'Select Case Me.NTSChiediSalva(Nothing, ocleIopc.bHasChanges)
      Select Case Me.NTSChiediSalva(Nothing, ocleIopc.bHasChanges)
        Case DialogResult.Yes
          'salvo
          '--------------------------------------------------
          '--- Salva record 
          '--------------------------------------------------
          'Salva profilo
          If Not ocleIopc.Salva(False) Then Return False
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
#End Region

End Class
