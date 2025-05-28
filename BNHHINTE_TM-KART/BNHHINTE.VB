Imports System.Data
Imports System.IO
Imports System.Net
Imports DevExpress.XtraGrid.Views.Base
Imports NTSInformatica.CLN__STD

Public Class FRMHHINTE

#Region "Variabili"
  Public oCallParams As CLE__CLDP
  Public ocleInte As CLEHHINTE

  Public dsInte As New DataSet
  Public dcInteTesta As New BindingSource
  Public dcInteCorpo As New BindingSource

  Public strProgramma As String = "BSHHINTE"

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
  Public WithEvents fmDocumenti As NTSGroupBox
  Public WithEvents lbTb_urlchia As NTSLabel
  Public WithEvents edTb_urlchia As NTSTextBoxStr
  Friend WithEvents pnlOpz As NTSPanel
  Friend WithEvents grDoc As NTSGrid
  Friend WithEvents grvDoc As NTSGridView
  Friend WithEvents tb_tipork As NTSGridColumn
  Friend WithEvents tb_serie As NTSGridColumn
  Friend WithEvents tb_operazione As NTSGridColumn
  Friend WithEvents tb_magazmacchina As NTSGridColumn
  Friend WithEvents tb_magazprincipale As NTSGridColumn
  Friend WithEvents xx_descrmagmacchina As NTSGridColumn
  Friend WithEvents xx_descrmagprincipale As NTSGridColumn
  Friend WithEvents tlbCancellaRiga As NTSBarButtonItem
  Friend WithEvents NtsBarSubItem1 As NTSBarSubItem
  Friend WithEvents NtsBarMenuItem1 As NTSBarMenuItem
  Friend WithEvents NtsBarMenuItem2 As NTSBarMenuItem
  Friend WithEvents NtsBarMenuItem3 As NTSBarMenuItem
  Friend WithEvents NtsBarMenuItem4 As NTSBarMenuItem
  Friend WithEvents NtsBarMenuItem5 As NTSBarMenuItem
  Friend WithEvents edTb_causaleScarico As NTSTextBoxNum
  Public WithEvents lbTb_causaleScarico As NTSLabel
  Friend WithEvents edTb_causaleCarico As NTSTextBoxNum
  Public WithEvents lbTb_causaleCarico As NTSLabel
  Friend WithEvents edTb_tipoBF As NTSTextBoxNum
  Public WithEvents lbTb_tipoBF As NTSLabel
  Public WithEvents lbTb_nomecausaleScarico As NTSLabel
  Public WithEvents lbTb_nomecausaleCarico As NTSLabel
  Public WithEvents lbTb_nometipoBF As NTSLabel
  Public WithEvents lbTb_nomeconto As NTSLabel
  Friend WithEvents edTb_conto As NTSTextBoxNum
  Public WithEvents lbTb_conto As NTSLabel
  Friend WithEvents edTb_serie As NTSTextBoxStr
  Public WithEvents lbTb_serie As NTSLabel
  Friend WithEvents fmImpostazioni As NTSGroupBox
  Public WithEvents lbTb_nomemagazzinoMacchina As NTSLabel
  Public WithEvents lbTb_magazzinoMacchna As NTSLabel
  Friend WithEvents edTb_magazzinoMacchina As NTSTextBoxNum
  Public WithEvents lbTb_nomemagazzinoPrincipale As NTSLabel
  Public WithEvents lbTb_magazzinoPrincipale As NTSLabel
  Friend WithEvents edTb_magazzinoPrincipale As NTSTextBoxNum
  Friend WithEvents ckTb_elaborazioneautom As NTSCheckBox
  Public WithEvents lbTb_elaborazioneautom As NTSLabel
  Friend WithEvents ckTb_elaborazionegiac3 As NTSCheckBox
  Friend WithEvents ckTb_elaborazionegiac2 As NTSCheckBox
  Friend WithEvents ckTb_elaborazionegiac1 As NTSCheckBox
  Friend WithEvents edTb_oragiac3 As NTSTextBoxData
  Friend WithEvents edTb_oragiac2 As NTSTextBoxData
  Friend WithEvents edTb_oragiac1 As NTSTextBoxData
  Public WithEvents lbTb_elaborazionegiac As NTSLabel
  Friend WithEvents edTb_secelaborazione As NTSTextBoxNum
  Public WithEvents NtsLabel1 As NTSLabel
  Friend WithEvents tlbRipristinaRiga As NTSBarButtonItem
#End Region

#Region "Inizializzazione"
  Public Overridable Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMHHINTE))
    Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
    Me.tlbMain = New NTSInformatica.NTSBar()
    Me.tlbSalva = New NTSInformatica.NTSBarButtonItem()
    Me.tlbRipristina = New NTSInformatica.NTSBarButtonItem()
    Me.tlbCancella = New NTSInformatica.NTSBarButtonItem()
    Me.tlbCancellaRiga = New NTSInformatica.NTSBarButtonItem()
    Me.tlbRipristinaRiga = New NTSInformatica.NTSBarButtonItem()
    Me.tlbZoom = New NTSInformatica.NTSBarButtonItem()
    Me.tlbGuida = New NTSInformatica.NTSBarButtonItem()
    Me.tlbEsci = New NTSInformatica.NTSBarButtonItem()
    Me.NtsBarSubItem1 = New NTSInformatica.NTSBarSubItem()
    Me.NtsBarMenuItem1 = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarMenuItem2 = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarMenuItem5 = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarMenuItem4 = New NTSInformatica.NTSBarMenuItem()
    Me.NtsBarMenuItem3 = New NTSInformatica.NTSBarMenuItem()
    Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
    Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
    Me.tlbNuovo = New NTSInformatica.NTSBarButtonItem()
    Me.tlbDuplica = New NTSInformatica.NTSBarButtonItem()
    Me.tlbPanelLeft = New NTSInformatica.NTSBarButtonItem()
    Me.fmDocumenti = New NTSInformatica.NTSGroupBox()
    Me.grDoc = New NTSInformatica.NTSGrid()
    Me.grvDoc = New NTSInformatica.NTSGridView()
    Me.tb_tipork = New NTSInformatica.NTSGridColumn()
    Me.tb_serie = New NTSInformatica.NTSGridColumn()
    Me.tb_operazione = New NTSInformatica.NTSGridColumn()
    Me.tb_magazmacchina = New NTSInformatica.NTSGridColumn()
    Me.xx_descrmagmacchina = New NTSInformatica.NTSGridColumn()
    Me.tb_magazprincipale = New NTSInformatica.NTSGridColumn()
    Me.xx_descrmagprincipale = New NTSInformatica.NTSGridColumn()
    Me.edTb_urlchia = New NTSInformatica.NTSTextBoxStr()
    Me.lbTb_urlchia = New NTSInformatica.NTSLabel()
    Me.pnlOpz = New NTSInformatica.NTSPanel()
    Me.fmImpostazioni = New NTSInformatica.NTSGroupBox()
    Me.NtsLabel1 = New NTSInformatica.NTSLabel()
    Me.edTb_secelaborazione = New NTSInformatica.NTSTextBoxNum()
    Me.ckTb_elaborazionegiac3 = New NTSInformatica.NTSCheckBox()
    Me.ckTb_elaborazionegiac2 = New NTSInformatica.NTSCheckBox()
    Me.ckTb_elaborazionegiac1 = New NTSInformatica.NTSCheckBox()
    Me.edTb_oragiac3 = New NTSInformatica.NTSTextBoxData()
    Me.edTb_oragiac2 = New NTSInformatica.NTSTextBoxData()
    Me.edTb_oragiac1 = New NTSInformatica.NTSTextBoxData()
    Me.lbTb_elaborazionegiac = New NTSInformatica.NTSLabel()
    Me.ckTb_elaborazioneautom = New NTSInformatica.NTSCheckBox()
    Me.lbTb_elaborazioneautom = New NTSInformatica.NTSLabel()
    Me.lbTb_nomemagazzinoPrincipale = New NTSInformatica.NTSLabel()
    Me.lbTb_magazzinoPrincipale = New NTSInformatica.NTSLabel()
    Me.edTb_magazzinoPrincipale = New NTSInformatica.NTSTextBoxNum()
    Me.lbTb_nomemagazzinoMacchina = New NTSInformatica.NTSLabel()
    Me.lbTb_magazzinoMacchna = New NTSInformatica.NTSLabel()
    Me.edTb_magazzinoMacchina = New NTSInformatica.NTSTextBoxNum()
    Me.edTb_serie = New NTSInformatica.NTSTextBoxStr()
    Me.lbTb_serie = New NTSInformatica.NTSLabel()
    Me.lbTb_tipoBF = New NTSInformatica.NTSLabel()
    Me.lbTb_nomeconto = New NTSInformatica.NTSLabel()
    Me.edTb_tipoBF = New NTSInformatica.NTSTextBoxNum()
    Me.edTb_conto = New NTSInformatica.NTSTextBoxNum()
    Me.lbTb_causaleCarico = New NTSInformatica.NTSLabel()
    Me.lbTb_conto = New NTSInformatica.NTSLabel()
    Me.edTb_causaleCarico = New NTSInformatica.NTSTextBoxNum()
    Me.lbTb_nomecausaleScarico = New NTSInformatica.NTSLabel()
    Me.lbTb_causaleScarico = New NTSInformatica.NTSLabel()
    Me.lbTb_nomecausaleCarico = New NTSInformatica.NTSLabel()
    Me.edTb_causaleScarico = New NTSInformatica.NTSTextBoxNum()
    Me.lbTb_nometipoBF = New NTSInformatica.NTSLabel()
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fmDocumenti, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmDocumenti.SuspendLayout()
    CType(Me.grDoc, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grvDoc, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_urlchia.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pnlOpz, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlOpz.SuspendLayout()
    CType(Me.fmImpostazioni, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.fmImpostazioni.SuspendLayout()
    CType(Me.edTb_secelaborazione.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ckTb_elaborazionegiac3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ckTb_elaborazionegiac2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ckTb_elaborazionegiac1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_oragiac3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_oragiac2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_oragiac1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ckTb_elaborazioneautom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_magazzinoPrincipale.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_magazzinoMacchina.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_serie.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_tipoBF.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_conto.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_causaleCarico.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.edTb_causaleScarico.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
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
    Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbNuovo, Me.tlbDuplica, Me.tlbSalva, Me.tlbRipristina, Me.tlbCancella, Me.tlbZoom, Me.tlbGuida, Me.tlbEsci, Me.tlbPanelLeft, Me.tlbCancellaRiga, Me.tlbRipristinaRiga, Me.NtsBarSubItem1, Me.NtsBarMenuItem1, Me.NtsBarMenuItem2, Me.NtsBarMenuItem3, Me.NtsBarMenuItem4, Me.NtsBarMenuItem5})
    Me.NtsBarManager1.MaxItemId = 26
    '
    'tlbMain
    '
    Me.tlbMain.BarName = "tlbMain"
    Me.tlbMain.DockCol = 0
    Me.tlbMain.DockRow = 0
    Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
    Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbSalva), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbRipristina, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCancella, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCancellaRiga, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbRipristinaRiga, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbZoom, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbGuida), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci), New DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, False, Me.NtsBarSubItem1, False)})
    Me.tlbMain.OptionsBar.AllowQuickCustomization = False
    Me.tlbMain.OptionsBar.DisableClose = True
    Me.tlbMain.OptionsBar.DrawDragBorder = False
    Me.tlbMain.OptionsBar.UseWholeRow = True
    Me.tlbMain.Text = "tlbMain"
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
    'NtsBarSubItem1
    '
    Me.NtsBarSubItem1.Caption = "Strumenti"
    Me.NtsBarSubItem1.Id = 20
    Me.NtsBarSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.NtsBarMenuItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.NtsBarMenuItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.NtsBarMenuItem5), New DevExpress.XtraBars.LinkPersistInfo(Me.NtsBarMenuItem4), New DevExpress.XtraBars.LinkPersistInfo(Me.NtsBarMenuItem3)})
    Me.NtsBarSubItem1.Name = "NtsBarSubItem1"
    Me.NtsBarSubItem1.Visible = False
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
    'tlbNuovo
    '
    Me.tlbNuovo.Caption = "Nuovo"
    Me.tlbNuovo.Glyph = CType(resources.GetObject("tlbNuovo.Glyph"), System.Drawing.Image)
    Me.tlbNuovo.Id = 0
    Me.tlbNuovo.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2)
    Me.tlbNuovo.Name = "tlbNuovo"
    Me.tlbNuovo.Visible = True
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
    'fmDocumenti
    '
    Me.fmDocumenti.AllowDrop = True
    Me.fmDocumenti.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmDocumenti.Appearance.Options.UseBackColor = True
    Me.fmDocumenti.Controls.Add(Me.grDoc)
    Me.fmDocumenti.Dock = System.Windows.Forms.DockStyle.Fill
    Me.fmDocumenti.Location = New System.Drawing.Point(0, 303)
    Me.fmDocumenti.Name = "fmDocumenti"
    Me.fmDocumenti.Size = New System.Drawing.Size(684, 208)
    Me.fmDocumenti.Text = "DOCUMENTI"
    '
    'grDoc
    '
    Me.grDoc.Dock = System.Windows.Forms.DockStyle.Fill
    Me.grDoc.Location = New System.Drawing.Point(2, 21)
    Me.grDoc.MainView = Me.grvDoc
    Me.grDoc.MenuManager = Me.NtsBarManager1
    Me.grDoc.Name = "grDoc"
    Me.grDoc.Size = New System.Drawing.Size(680, 185)
    Me.grDoc.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvDoc})
    '
    'grvDoc
    '
    Me.grvDoc.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.tb_tipork, Me.tb_serie, Me.tb_operazione, Me.tb_magazmacchina, Me.xx_descrmagmacchina, Me.tb_magazprincipale, Me.xx_descrmagprincipale})
    Me.grvDoc.Enabled = True
    Me.grvDoc.GridControl = Me.grDoc
    Me.grvDoc.Name = "grvDoc"
    Me.grvDoc.OptionsCustomization.AllowRowSizing = True
    Me.grvDoc.OptionsNavigation.EnterMoveNextColumn = True
    Me.grvDoc.OptionsNavigation.UseTabKey = False
    Me.grvDoc.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.grvDoc.OptionsView.ColumnAutoWidth = False
    Me.grvDoc.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.grvDoc.OptionsView.ShowGroupPanel = False
    '
    'tb_tipork
    '
    Me.tb_tipork.AppearanceCell.Options.UseBackColor = True
    Me.tb_tipork.AppearanceCell.Options.UseTextOptions = True
    Me.tb_tipork.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_tipork.Caption = "Tipo Doc."
    Me.tb_tipork.Enabled = True
    Me.tb_tipork.FieldName = "tb_tipork"
    Me.tb_tipork.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_tipork.Name = "tb_tipork"
    Me.tb_tipork.Visible = True
    Me.tb_tipork.VisibleIndex = 0
    '
    'tb_serie
    '
    Me.tb_serie.AppearanceCell.Options.UseBackColor = True
    Me.tb_serie.AppearanceCell.Options.UseTextOptions = True
    Me.tb_serie.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_serie.Caption = "Serie"
    Me.tb_serie.Enabled = True
    Me.tb_serie.FieldName = "tb_serie"
    Me.tb_serie.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_serie.Name = "tb_serie"
    Me.tb_serie.Visible = True
    Me.tb_serie.VisibleIndex = 1
    '
    'tb_operazione
    '
    Me.tb_operazione.AppearanceCell.Options.UseBackColor = True
    Me.tb_operazione.AppearanceCell.Options.UseTextOptions = True
    Me.tb_operazione.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_operazione.Caption = "Operaz."
    Me.tb_operazione.Enabled = True
    Me.tb_operazione.FieldName = "tb_operazione"
    Me.tb_operazione.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_operazione.Name = "tb_operazione"
    Me.tb_operazione.Visible = True
    Me.tb_operazione.VisibleIndex = 2
    '
    'tb_magazmacchina
    '
    Me.tb_magazmacchina.AppearanceCell.Options.UseBackColor = True
    Me.tb_magazmacchina.AppearanceCell.Options.UseTextOptions = True
    Me.tb_magazmacchina.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_magazmacchina.Caption = "Mag. Macchina"
    Me.tb_magazmacchina.Enabled = True
    Me.tb_magazmacchina.FieldName = "tb_magazmacchina"
    Me.tb_magazmacchina.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_magazmacchina.Name = "tb_magazmacchina"
    Me.tb_magazmacchina.Visible = True
    Me.tb_magazmacchina.VisibleIndex = 3
    '
    'xx_descrmagmacchina
    '
    Me.xx_descrmagmacchina.AppearanceCell.Options.UseBackColor = True
    Me.xx_descrmagmacchina.AppearanceCell.Options.UseTextOptions = True
    Me.xx_descrmagmacchina.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.xx_descrmagmacchina.Caption = "Descrizione"
    Me.xx_descrmagmacchina.Enabled = False
    Me.xx_descrmagmacchina.FieldName = "xx_descrmagmacchina"
    Me.xx_descrmagmacchina.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.xx_descrmagmacchina.Name = "xx_descrmagmacchina"
    Me.xx_descrmagmacchina.OptionsColumn.AllowEdit = False
    Me.xx_descrmagmacchina.OptionsColumn.ReadOnly = True
    Me.xx_descrmagmacchina.Visible = True
    Me.xx_descrmagmacchina.VisibleIndex = 4
    '
    'tb_magazprincipale
    '
    Me.tb_magazprincipale.AppearanceCell.Options.UseBackColor = True
    Me.tb_magazprincipale.AppearanceCell.Options.UseTextOptions = True
    Me.tb_magazprincipale.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.tb_magazprincipale.Caption = "Mag. Principale"
    Me.tb_magazprincipale.Enabled = True
    Me.tb_magazprincipale.FieldName = "tb_magazprincipale"
    Me.tb_magazprincipale.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.tb_magazprincipale.Name = "tb_magazprincipale"
    Me.tb_magazprincipale.Visible = True
    Me.tb_magazprincipale.VisibleIndex = 5
    '
    'xx_descrmagprincipale
    '
    Me.xx_descrmagprincipale.AppearanceCell.Options.UseBackColor = True
    Me.xx_descrmagprincipale.AppearanceCell.Options.UseTextOptions = True
    Me.xx_descrmagprincipale.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.xx_descrmagprincipale.Caption = "Descrizione"
    Me.xx_descrmagprincipale.Enabled = False
    Me.xx_descrmagprincipale.FieldName = "xx_descrmagprincipale"
    Me.xx_descrmagprincipale.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
    Me.xx_descrmagprincipale.Name = "xx_descrmagprincipale"
    Me.xx_descrmagprincipale.OptionsColumn.AllowEdit = False
    Me.xx_descrmagprincipale.OptionsColumn.ReadOnly = True
    Me.xx_descrmagprincipale.Visible = True
    Me.xx_descrmagprincipale.VisibleIndex = 6
    '
    'edTb_urlchia
    '
    Me.edTb_urlchia.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.edTb_urlchia.Cursor = System.Windows.Forms.Cursors.Default
    Me.edTb_urlchia.Location = New System.Drawing.Point(108, 24)
    Me.edTb_urlchia.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.edTb_urlchia.Name = "edTb_urlchia"
    Me.edTb_urlchia.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_urlchia.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_urlchia.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_urlchia.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_urlchia.Properties.MaxLength = 65536
    Me.edTb_urlchia.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_urlchia.Size = New System.Drawing.Size(572, 20)
    '
    'lbTb_urlchia
    '
    Me.lbTb_urlchia.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_urlchia.Location = New System.Drawing.Point(4, 24)
    Me.lbTb_urlchia.Name = "lbTb_urlchia"
    Me.lbTb_urlchia.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_urlchia.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_urlchia.Text = "URL"
    Me.lbTb_urlchia.UseMnemonic = False
    '
    'pnlOpz
    '
    Me.pnlOpz.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.pnlOpz.Controls.Add(Me.fmImpostazioni)
    Me.pnlOpz.Dock = System.Windows.Forms.DockStyle.Top
    Me.pnlOpz.Location = New System.Drawing.Point(0, 35)
    Me.pnlOpz.Name = "pnlOpz"
    Me.pnlOpz.Size = New System.Drawing.Size(684, 268)
    '
    'fmImpostazioni
    '
    Me.fmImpostazioni.AllowDrop = True
    Me.fmImpostazioni.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.fmImpostazioni.Appearance.Options.UseBackColor = True
    Me.fmImpostazioni.Controls.Add(Me.NtsLabel1)
    Me.fmImpostazioni.Controls.Add(Me.edTb_secelaborazione)
    Me.fmImpostazioni.Controls.Add(Me.ckTb_elaborazionegiac3)
    Me.fmImpostazioni.Controls.Add(Me.ckTb_elaborazionegiac2)
    Me.fmImpostazioni.Controls.Add(Me.ckTb_elaborazionegiac1)
    Me.fmImpostazioni.Controls.Add(Me.edTb_oragiac3)
    Me.fmImpostazioni.Controls.Add(Me.edTb_oragiac2)
    Me.fmImpostazioni.Controls.Add(Me.edTb_oragiac1)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_elaborazionegiac)
    Me.fmImpostazioni.Controls.Add(Me.ckTb_elaborazioneautom)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_elaborazioneautom)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_nomemagazzinoPrincipale)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_magazzinoPrincipale)
    Me.fmImpostazioni.Controls.Add(Me.edTb_magazzinoPrincipale)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_nomemagazzinoMacchina)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_magazzinoMacchna)
    Me.fmImpostazioni.Controls.Add(Me.edTb_magazzinoMacchina)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_urlchia)
    Me.fmImpostazioni.Controls.Add(Me.edTb_serie)
    Me.fmImpostazioni.Controls.Add(Me.edTb_urlchia)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_serie)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_tipoBF)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_nomeconto)
    Me.fmImpostazioni.Controls.Add(Me.edTb_tipoBF)
    Me.fmImpostazioni.Controls.Add(Me.edTb_conto)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_causaleCarico)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_conto)
    Me.fmImpostazioni.Controls.Add(Me.edTb_causaleCarico)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_nomecausaleScarico)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_causaleScarico)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_nomecausaleCarico)
    Me.fmImpostazioni.Controls.Add(Me.edTb_causaleScarico)
    Me.fmImpostazioni.Controls.Add(Me.lbTb_nometipoBF)
    Me.fmImpostazioni.Dock = System.Windows.Forms.DockStyle.Fill
    Me.fmImpostazioni.Location = New System.Drawing.Point(0, 0)
    Me.fmImpostazioni.Name = "fmImpostazioni"
    Me.fmImpostazioni.Size = New System.Drawing.Size(684, 268)
    Me.fmImpostazioni.Text = "IMPOSTAZIONI"
    '
    'NtsLabel1
    '
    Me.NtsLabel1.AutoEllipsis = True
    Me.NtsLabel1.BackColor = System.Drawing.Color.Transparent
    Me.NtsLabel1.Location = New System.Drawing.Point(196, 216)
    Me.NtsLabel1.Name = "NtsLabel1"
    Me.NtsLabel1.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.NtsLabel1.Size = New System.Drawing.Size(356, 20)
    Me.NtsLabel1.Text = "Tempo Elaborazione espresso in secondi"
    Me.NtsLabel1.UseMnemonic = False
    Me.NtsLabel1.Visible = False
    '
    'edTb_secelaborazione
    '
    Me.edTb_secelaborazione.EditValue = "0"
    Me.edTb_secelaborazione.Enabled = False
    Me.edTb_secelaborazione.Location = New System.Drawing.Point(144, 216)
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
    Me.edTb_secelaborazione.Visible = False
    '
    'ckTb_elaborazionegiac3
    '
    Me.ckTb_elaborazionegiac3.Location = New System.Drawing.Point(284, 240)
    Me.ckTb_elaborazionegiac3.Name = "ckTb_elaborazionegiac3"
    Me.ckTb_elaborazionegiac3.NTSSetparam__DbField = "TESTA.*"
    Me.ckTb_elaborazionegiac3.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.ckTb_elaborazionegiac3.Properties.Appearance.Options.UseBackColor = True
    Me.ckTb_elaborazionegiac3.Properties.Appearance.Options.UseTextOptions = True
    Me.ckTb_elaborazionegiac3.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
    Me.ckTb_elaborazionegiac3.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.ckTb_elaborazionegiac3.Properties.AutoHeight = False
    Me.ckTb_elaborazionegiac3.Properties.Caption = ""
    Me.ckTb_elaborazionegiac3.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined
    Me.ckTb_elaborazionegiac3.Properties.PictureChecked = CType(resources.GetObject("ckTb_elaborazionegiac3.Properties.PictureChecked"), System.Drawing.Image)
    Me.ckTb_elaborazionegiac3.Properties.PictureUnchecked = CType(resources.GetObject("ckTb_elaborazionegiac3.Properties.PictureUnchecked"), System.Drawing.Image)
    Me.ckTb_elaborazionegiac3.Size = New System.Drawing.Size(32, 20)
    Me.ckTb_elaborazionegiac3.Visible = False
    '
    'ckTb_elaborazionegiac2
    '
    Me.ckTb_elaborazionegiac2.Location = New System.Drawing.Point(196, 240)
    Me.ckTb_elaborazionegiac2.Name = "ckTb_elaborazionegiac2"
    Me.ckTb_elaborazionegiac2.NTSSetparam__DbField = "TESTA.*"
    Me.ckTb_elaborazionegiac2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.ckTb_elaborazionegiac2.Properties.Appearance.Options.UseBackColor = True
    Me.ckTb_elaborazionegiac2.Properties.Appearance.Options.UseTextOptions = True
    Me.ckTb_elaborazionegiac2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
    Me.ckTb_elaborazionegiac2.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.ckTb_elaborazionegiac2.Properties.AutoHeight = False
    Me.ckTb_elaborazionegiac2.Properties.Caption = ""
    Me.ckTb_elaborazionegiac2.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined
    Me.ckTb_elaborazionegiac2.Properties.PictureChecked = CType(resources.GetObject("ckTb_elaborazionegiac2.Properties.PictureChecked"), System.Drawing.Image)
    Me.ckTb_elaborazionegiac2.Properties.PictureUnchecked = CType(resources.GetObject("ckTb_elaborazionegiac2.Properties.PictureUnchecked"), System.Drawing.Image)
    Me.ckTb_elaborazionegiac2.Size = New System.Drawing.Size(32, 20)
    Me.ckTb_elaborazionegiac2.Visible = False
    '
    'ckTb_elaborazionegiac1
    '
    Me.ckTb_elaborazionegiac1.Location = New System.Drawing.Point(108, 240)
    Me.ckTb_elaborazionegiac1.Name = "ckTb_elaborazionegiac1"
    Me.ckTb_elaborazionegiac1.NTSSetparam__DbField = "TESTA.*"
    Me.ckTb_elaborazionegiac1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.ckTb_elaborazionegiac1.Properties.Appearance.Options.UseBackColor = True
    Me.ckTb_elaborazionegiac1.Properties.Appearance.Options.UseTextOptions = True
    Me.ckTb_elaborazionegiac1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
    Me.ckTb_elaborazionegiac1.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
    Me.ckTb_elaborazionegiac1.Properties.AutoHeight = False
    Me.ckTb_elaborazionegiac1.Properties.Caption = ""
    Me.ckTb_elaborazionegiac1.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined
    Me.ckTb_elaborazionegiac1.Properties.PictureChecked = CType(resources.GetObject("ckTb_elaborazionegiac1.Properties.PictureChecked"), System.Drawing.Image)
    Me.ckTb_elaborazionegiac1.Properties.PictureUnchecked = CType(resources.GetObject("ckTb_elaborazionegiac1.Properties.PictureUnchecked"), System.Drawing.Image)
    Me.ckTb_elaborazionegiac1.Size = New System.Drawing.Size(32, 20)
    Me.ckTb_elaborazionegiac1.Visible = False
    '
    'edTb_oragiac3
    '
    Me.edTb_oragiac3.EditValue = ""
    Me.edTb_oragiac3.Enabled = False
    Me.edTb_oragiac3.Location = New System.Drawing.Point(320, 240)
    Me.edTb_oragiac3.Name = "edTb_oragiac3"
    Me.edTb_oragiac3.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_oragiac3.NTSSetparam__TipoData = NTSInformatica.NTSTextBoxData.TipoColonnaData.Ora_HH_MM
    Me.edTb_oragiac3.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_oragiac3.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_oragiac3.Properties.AutoHeight = False
    Me.edTb_oragiac3.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_oragiac3.Properties.MaxLength = 65536
    Me.edTb_oragiac3.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_oragiac3.Size = New System.Drawing.Size(48, 20)
    Me.edTb_oragiac3.Visible = False
    '
    'edTb_oragiac2
    '
    Me.edTb_oragiac2.EditValue = ""
    Me.edTb_oragiac2.Enabled = False
    Me.edTb_oragiac2.Location = New System.Drawing.Point(232, 240)
    Me.edTb_oragiac2.Name = "edTb_oragiac2"
    Me.edTb_oragiac2.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_oragiac2.NTSSetparam__TipoData = NTSInformatica.NTSTextBoxData.TipoColonnaData.Ora_HH_MM
    Me.edTb_oragiac2.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_oragiac2.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_oragiac2.Properties.AutoHeight = False
    Me.edTb_oragiac2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_oragiac2.Properties.MaxLength = 65536
    Me.edTb_oragiac2.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_oragiac2.Size = New System.Drawing.Size(48, 20)
    Me.edTb_oragiac2.Visible = False
    '
    'edTb_oragiac1
    '
    Me.edTb_oragiac1.EditValue = ""
    Me.edTb_oragiac1.Enabled = False
    Me.edTb_oragiac1.Location = New System.Drawing.Point(144, 240)
    Me.edTb_oragiac1.Name = "edTb_oragiac1"
    Me.edTb_oragiac1.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_oragiac1.NTSSetparam__TipoData = NTSInformatica.NTSTextBoxData.TipoColonnaData.Ora_HH_MM
    Me.edTb_oragiac1.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_oragiac1.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_oragiac1.Properties.AutoHeight = False
    Me.edTb_oragiac1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_oragiac1.Properties.MaxLength = 65536
    Me.edTb_oragiac1.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_oragiac1.Size = New System.Drawing.Size(48, 20)
    Me.edTb_oragiac1.Visible = False
    '
    'lbTb_elaborazionegiac
    '
    Me.lbTb_elaborazionegiac.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_elaborazionegiac.Location = New System.Drawing.Point(4, 240)
    Me.lbTb_elaborazionegiac.Name = "lbTb_elaborazionegiac"
    Me.lbTb_elaborazionegiac.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_elaborazionegiac.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_elaborazionegiac.Text = "Elab. Giacenze"
    Me.lbTb_elaborazionegiac.UseMnemonic = False
    Me.lbTb_elaborazionegiac.Visible = False
    '
    'ckTb_elaborazioneautom
    '
    Me.ckTb_elaborazioneautom.Location = New System.Drawing.Point(108, 216)
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
    Me.ckTb_elaborazioneautom.Visible = False
    '
    'lbTb_elaborazioneautom
    '
    Me.lbTb_elaborazioneautom.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_elaborazioneautom.Location = New System.Drawing.Point(4, 216)
    Me.lbTb_elaborazioneautom.Name = "lbTb_elaborazioneautom"
    Me.lbTb_elaborazioneautom.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_elaborazioneautom.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_elaborazioneautom.Text = "Elab. Automatica"
    Me.lbTb_elaborazioneautom.UseMnemonic = False
    Me.lbTb_elaborazioneautom.Visible = False
    '
    'lbTb_nomemagazzinoPrincipale
    '
    Me.lbTb_nomemagazzinoPrincipale.AutoEllipsis = True
    Me.lbTb_nomemagazzinoPrincipale.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_nomemagazzinoPrincipale.Location = New System.Drawing.Point(196, 192)
    Me.lbTb_nomemagazzinoPrincipale.Name = "lbTb_nomemagazzinoPrincipale"
    Me.lbTb_nomemagazzinoPrincipale.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.lbTb_nomemagazzinoPrincipale.Size = New System.Drawing.Size(356, 20)
    Me.lbTb_nomemagazzinoPrincipale.UseMnemonic = False
    '
    'lbTb_magazzinoPrincipale
    '
    Me.lbTb_magazzinoPrincipale.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_magazzinoPrincipale.Location = New System.Drawing.Point(4, 192)
    Me.lbTb_magazzinoPrincipale.Name = "lbTb_magazzinoPrincipale"
    Me.lbTb_magazzinoPrincipale.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_magazzinoPrincipale.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_magazzinoPrincipale.Text = "Mag. Principale"
    Me.lbTb_magazzinoPrincipale.UseMnemonic = False
    '
    'edTb_magazzinoPrincipale
    '
    Me.edTb_magazzinoPrincipale.EditValue = "0"
    Me.edTb_magazzinoPrincipale.Location = New System.Drawing.Point(108, 192)
    Me.edTb_magazzinoPrincipale.Name = "edTb_magazzinoPrincipale"
    Me.edTb_magazzinoPrincipale.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_magazzinoPrincipale.Properties.Appearance.Options.UseTextOptions = True
    Me.edTb_magazzinoPrincipale.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edTb_magazzinoPrincipale.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_magazzinoPrincipale.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_magazzinoPrincipale.Properties.AutoHeight = False
    Me.edTb_magazzinoPrincipale.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_magazzinoPrincipale.Properties.MaxLength = 65536
    Me.edTb_magazzinoPrincipale.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_magazzinoPrincipale.Size = New System.Drawing.Size(84, 20)
    '
    'lbTb_nomemagazzinoMacchina
    '
    Me.lbTb_nomemagazzinoMacchina.AutoEllipsis = True
    Me.lbTb_nomemagazzinoMacchina.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_nomemagazzinoMacchina.Location = New System.Drawing.Point(196, 168)
    Me.lbTb_nomemagazzinoMacchina.Name = "lbTb_nomemagazzinoMacchina"
    Me.lbTb_nomemagazzinoMacchina.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.lbTb_nomemagazzinoMacchina.Size = New System.Drawing.Size(356, 20)
    Me.lbTb_nomemagazzinoMacchina.UseMnemonic = False
    '
    'lbTb_magazzinoMacchna
    '
    Me.lbTb_magazzinoMacchna.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_magazzinoMacchna.Location = New System.Drawing.Point(4, 168)
    Me.lbTb_magazzinoMacchna.Name = "lbTb_magazzinoMacchna"
    Me.lbTb_magazzinoMacchna.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_magazzinoMacchna.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_magazzinoMacchna.Text = "Mag. Macchina"
    Me.lbTb_magazzinoMacchna.UseMnemonic = False
    '
    'edTb_magazzinoMacchina
    '
    Me.edTb_magazzinoMacchina.EditValue = "0"
    Me.edTb_magazzinoMacchina.Location = New System.Drawing.Point(108, 168)
    Me.edTb_magazzinoMacchina.Name = "edTb_magazzinoMacchina"
    Me.edTb_magazzinoMacchina.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_magazzinoMacchina.Properties.Appearance.Options.UseTextOptions = True
    Me.edTb_magazzinoMacchina.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edTb_magazzinoMacchina.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_magazzinoMacchina.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_magazzinoMacchina.Properties.AutoHeight = False
    Me.edTb_magazzinoMacchina.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_magazzinoMacchina.Properties.MaxLength = 65536
    Me.edTb_magazzinoMacchina.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_magazzinoMacchina.Size = New System.Drawing.Size(84, 20)
    '
    'edTb_serie
    '
    Me.edTb_serie.EditValue = ""
    Me.edTb_serie.Location = New System.Drawing.Point(108, 72)
    Me.edTb_serie.Name = "edTb_serie"
    Me.edTb_serie.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_serie.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_serie.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_serie.Properties.AutoHeight = False
    Me.edTb_serie.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_serie.Properties.MaxLength = 65536
    Me.edTb_serie.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_serie.Size = New System.Drawing.Size(56, 20)
    '
    'lbTb_serie
    '
    Me.lbTb_serie.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_serie.Location = New System.Drawing.Point(4, 72)
    Me.lbTb_serie.Name = "lbTb_serie"
    Me.lbTb_serie.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_serie.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_serie.Text = "Serie BMI"
    Me.lbTb_serie.UseMnemonic = False
    '
    'lbTb_tipoBF
    '
    Me.lbTb_tipoBF.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_tipoBF.Location = New System.Drawing.Point(4, 96)
    Me.lbTb_tipoBF.Name = "lbTb_tipoBF"
    Me.lbTb_tipoBF.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_tipoBF.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_tipoBF.Text = "Tipo B/F BMI"
    Me.lbTb_tipoBF.UseMnemonic = False
    '
    'lbTb_nomeconto
    '
    Me.lbTb_nomeconto.AutoEllipsis = True
    Me.lbTb_nomeconto.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_nomeconto.Location = New System.Drawing.Point(196, 48)
    Me.lbTb_nomeconto.Name = "lbTb_nomeconto"
    Me.lbTb_nomeconto.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.lbTb_nomeconto.Size = New System.Drawing.Size(356, 20)
    Me.lbTb_nomeconto.UseMnemonic = False
    '
    'edTb_tipoBF
    '
    Me.edTb_tipoBF.EditValue = "0"
    Me.edTb_tipoBF.Location = New System.Drawing.Point(108, 96)
    Me.edTb_tipoBF.Name = "edTb_tipoBF"
    Me.edTb_tipoBF.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_tipoBF.Properties.Appearance.Options.UseTextOptions = True
    Me.edTb_tipoBF.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edTb_tipoBF.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_tipoBF.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_tipoBF.Properties.AutoHeight = False
    Me.edTb_tipoBF.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_tipoBF.Properties.MaxLength = 65536
    Me.edTb_tipoBF.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_tipoBF.Size = New System.Drawing.Size(84, 20)
    '
    'edTb_conto
    '
    Me.edTb_conto.EditValue = "0"
    Me.edTb_conto.Location = New System.Drawing.Point(108, 48)
    Me.edTb_conto.Name = "edTb_conto"
    Me.edTb_conto.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_conto.Properties.Appearance.Options.UseTextOptions = True
    Me.edTb_conto.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edTb_conto.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_conto.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_conto.Properties.AutoHeight = False
    Me.edTb_conto.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_conto.Properties.MaxLength = 65536
    Me.edTb_conto.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_conto.Size = New System.Drawing.Size(84, 20)
    '
    'lbTb_causaleCarico
    '
    Me.lbTb_causaleCarico.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_causaleCarico.Location = New System.Drawing.Point(4, 120)
    Me.lbTb_causaleCarico.Name = "lbTb_causaleCarico"
    Me.lbTb_causaleCarico.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_causaleCarico.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_causaleCarico.Text = "Causale Carico"
    Me.lbTb_causaleCarico.UseMnemonic = False
    '
    'lbTb_conto
    '
    Me.lbTb_conto.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_conto.Location = New System.Drawing.Point(4, 48)
    Me.lbTb_conto.Name = "lbTb_conto"
    Me.lbTb_conto.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_conto.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_conto.Text = "Conto BMI"
    Me.lbTb_conto.UseMnemonic = False
    '
    'edTb_causaleCarico
    '
    Me.edTb_causaleCarico.EditValue = "0"
    Me.edTb_causaleCarico.Location = New System.Drawing.Point(108, 120)
    Me.edTb_causaleCarico.Name = "edTb_causaleCarico"
    Me.edTb_causaleCarico.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_causaleCarico.Properties.Appearance.Options.UseTextOptions = True
    Me.edTb_causaleCarico.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edTb_causaleCarico.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_causaleCarico.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_causaleCarico.Properties.AutoHeight = False
    Me.edTb_causaleCarico.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_causaleCarico.Properties.MaxLength = 65536
    Me.edTb_causaleCarico.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_causaleCarico.Size = New System.Drawing.Size(84, 20)
    '
    'lbTb_nomecausaleScarico
    '
    Me.lbTb_nomecausaleScarico.AutoEllipsis = True
    Me.lbTb_nomecausaleScarico.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_nomecausaleScarico.Location = New System.Drawing.Point(196, 144)
    Me.lbTb_nomecausaleScarico.Name = "lbTb_nomecausaleScarico"
    Me.lbTb_nomecausaleScarico.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.lbTb_nomecausaleScarico.Size = New System.Drawing.Size(356, 20)
    Me.lbTb_nomecausaleScarico.UseMnemonic = False
    '
    'lbTb_causaleScarico
    '
    Me.lbTb_causaleScarico.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_causaleScarico.Location = New System.Drawing.Point(4, 144)
    Me.lbTb_causaleScarico.Name = "lbTb_causaleScarico"
    Me.lbTb_causaleScarico.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldCaption
    Me.lbTb_causaleScarico.Size = New System.Drawing.Size(100, 20)
    Me.lbTb_causaleScarico.Text = "Causale Scarico"
    Me.lbTb_causaleScarico.UseMnemonic = False
    '
    'lbTb_nomecausaleCarico
    '
    Me.lbTb_nomecausaleCarico.AutoEllipsis = True
    Me.lbTb_nomecausaleCarico.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_nomecausaleCarico.Location = New System.Drawing.Point(196, 120)
    Me.lbTb_nomecausaleCarico.Name = "lbTb_nomecausaleCarico"
    Me.lbTb_nomecausaleCarico.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.lbTb_nomecausaleCarico.Size = New System.Drawing.Size(356, 20)
    Me.lbTb_nomecausaleCarico.UseMnemonic = False
    '
    'edTb_causaleScarico
    '
    Me.edTb_causaleScarico.EditValue = "0"
    Me.edTb_causaleScarico.Location = New System.Drawing.Point(108, 144)
    Me.edTb_causaleScarico.Name = "edTb_causaleScarico"
    Me.edTb_causaleScarico.NTSSetparam__DbField = "TESTA.*"
    Me.edTb_causaleScarico.Properties.Appearance.Options.UseTextOptions = True
    Me.edTb_causaleScarico.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    Me.edTb_causaleScarico.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black
    Me.edTb_causaleScarico.Properties.AppearanceDisabled.Options.UseForeColor = True
    Me.edTb_causaleScarico.Properties.AutoHeight = False
    Me.edTb_causaleScarico.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
    Me.edTb_causaleScarico.Properties.MaxLength = 65536
    Me.edTb_causaleScarico.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.edTb_causaleScarico.Size = New System.Drawing.Size(84, 20)
    '
    'lbTb_nometipoBF
    '
    Me.lbTb_nometipoBF.AutoEllipsis = True
    Me.lbTb_nometipoBF.BackColor = System.Drawing.Color.Transparent
    Me.lbTb_nometipoBF.Location = New System.Drawing.Point(196, 96)
    Me.lbTb_nometipoBF.Name = "lbTb_nometipoBF"
    Me.lbTb_nometipoBF.NTSBordeStyle = NTSInformatica.NTSLabel.NTSBorderStyle.FieldDescription
    Me.lbTb_nometipoBF.Size = New System.Drawing.Size(356, 20)
    Me.lbTb_nometipoBF.UseMnemonic = False
    '
    'FRMHHINTE
    '
    Me.ClientSize = New System.Drawing.Size(684, 511)
    Me.Controls.Add(Me.fmDocumenti)
    Me.Controls.Add(Me.pnlOpz)
    Me.Controls.Add(Me.barDockControlLeft)
    Me.Controls.Add(Me.barDockControlRight)
    Me.Controls.Add(Me.barDockControlBottom)
    Me.Controls.Add(Me.barDockControlTop)
    Me.Margin = New System.Windows.Forms.Padding(3, 14, 3, 14)
    Me.MinimizeBox = False
    Me.Name = "FRMHHINTE"
    Me.Text = "IMPOSTAZIONE CHIAMATE MAGAZZINO"
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fmDocumenti, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmDocumenti.ResumeLayout(False)
    CType(Me.grDoc, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grvDoc, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_urlchia.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pnlOpz, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlOpz.ResumeLayout(False)
    CType(Me.fmImpostazioni, System.ComponentModel.ISupportInitialize).EndInit()
    Me.fmImpostazioni.ResumeLayout(False)
    CType(Me.edTb_secelaborazione.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ckTb_elaborazionegiac3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ckTb_elaborazionegiac2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ckTb_elaborazionegiac1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_oragiac3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_oragiac2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_oragiac1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ckTb_elaborazioneautom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_magazzinoPrincipale.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_magazzinoMacchina.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_serie.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_tipoBF.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_conto.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_causaleCarico.Properties, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.edTb_causaleScarico.Properties, System.ComponentModel.ISupportInitialize).EndInit()
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

      '-------------------------------------------------
      '--- Carica il combo dei tipi Documento
      '-------------------------------------------------
      Dim dttTipo As New DataTable
      With dttTipo
        .Columns.Add("cod", GetType(String))
        .Columns.Add("val", GetType(String))
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
      'cbch_codchia.DataSource = dttTipo
      'cbch_codchia.ValueMember = "cod"
      'cbch_codchia.DisplayMember = "val"

      edTb_urlchia.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964288, lbTb_urlchia.Text), 255, True)
      edTb_conto.NTSSetParamTabe(oMenu, oApp.Tr(Me, 130995879126964289, lbTb_conto.Text), tabanagrac)
      edTb_serie.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964289, lbTb_serie.Text), CLN__STD.SerieMaxLen, True)
      edTb_tipoBF.NTSSetParamTabe(oMenu, oApp.Tr(Me, 130995879126964290, lbTb_tipoBF.Text), tabtpbf)
      edTb_causaleCarico.NTSSetParamTabe(oMenu, oApp.Tr(Me, 130995879126964291, lbTb_causaleCarico.Text), tabcaum)
      edTb_causaleScarico.NTSSetParamTabe(oMenu, oApp.Tr(Me, 130995879126964292, lbTb_causaleScarico.Text), tabcaum)
      edTb_magazzinoMacchina.NTSSetParamTabe(oMenu, oApp.Tr(Me, 130995879126964293, lbTb_magazzinoMacchna.Text), tabmaga)
      edTb_magazzinoPrincipale.NTSSetParamTabe(oMenu, oApp.Tr(Me, 130995879126964294, lbTb_magazzinoPrincipale.Text), tabmaga)

      ckTb_elaborazioneautom.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964295, lbTb_elaborazioneautom.Text), "S", "N")
      edTb_secelaborazione.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964296, lbTb_elaborazioneautom.Text), "0", 5, 0, 99999)
      ckTb_elaborazionegiac1.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964297, lbTb_elaborazionegiac.Text), "S", "N")
      edTb_oragiac1.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964298, lbTb_elaborazionegiac.Text), False, "M", NTSCDate("01/01/1900 00:00:00"), NTSCDate("31/12/9999 23:59:59"))
      edTb_oragiac1.NTSForzaVisZoom = False
      ckTb_elaborazionegiac2.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964299, lbTb_elaborazionegiac.Text), "S", "N")
      edTb_oragiac2.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964300, lbTb_elaborazionegiac.Text), False, "M", NTSCDate("01/01/1900 00:00:00"), NTSCDate("31/12/9999 23:59:59"))
      edTb_oragiac2.NTSForzaVisZoom = False
      ckTb_elaborazionegiac3.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964301, lbTb_elaborazionegiac.Text), "S", "N")
      edTb_oragiac3.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964302, lbTb_elaborazionegiac.Text), False, "M", NTSCDate("01/01/1900 00:00:00"), NTSCDate("31/12/9999 23:59:59"))
      edTb_oragiac3.NTSForzaVisZoom = False


      '--------------------------------------------------
      '--- Griglia Lista
      '--------------------------------------------------
      grvDoc.NTSSetParam(oMenu, oApp.Tr(Me, 130995879126964289, "Elenco voci"))
      grvDoc.NTSAllowInsert = True
      'grvDoc.NTSHideMenuSx()

      tb_tipork.NTSSetParamCMB(oMenu, oApp.Tr(Me, 130995879126964290, tb_tipork.Caption), dttTipo, "val", "cod")
      tb_serie.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130995879126964291, tb_serie.Caption), 3)
      tb_operazione.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130995879126964292, tb_operazione.Caption), 1)
      tb_magazmacchina.NTSSetParamNUMTabe(oMenu, oApp.Tr(Me, 130995879126964293, tb_magazmacchina.Caption), tabmaga)
      xx_descrmagmacchina.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130995879126964293, xx_descrmagmacchina.Caption), 200, True)
      tb_magazprincipale.NTSSetParamNUMTabe(oMenu, oApp.Tr(Me, 130995879126964294, tb_magazprincipale.Caption), tabmaga)
      xx_descrmagprincipale.NTSSetParamSTR(oMenu, oApp.Tr(Me, 130995879126964293, xx_descrmagprincipale.Caption), 200, True)

      tb_tipork.NTSSetRichiesto()
      tb_serie.NTSSetRichiesto()
      tb_operazione.NTSSetRichiesto()
      tb_magazmacchina.NTSSetRichiesto()
      tb_magazprincipale.NTSSetRichiesto()

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
    If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNHHINTE", "BEHHINTE", oTmp, strErr, False, "", "") = False Then
      oApp.MsgBoxErr(oApp.Tr(Me, 128704611604674454, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
      Return False
    End If

    ocleInte = CType(oTmp, CLEHHINTE)

    '--------------------------------------------------
    '--- Aggiunge gestore eventi
    '--------------------------------------------------
    AddHandler ocleInte.RemoteEvent, AddressOf GestisciEventiEntity
    If ocleInte.Init(oApp, oScript, oMenu.oCleComm, "", False, "", "") = False Then Return False

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
            Case "tb_tipork"
              grvDoc.FocusedColumn = tb_tipork
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
  Public Overridable Sub FRMHHINTE_ActivatedFirst(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ActivatedFirst
    Try
      '--------------------------------------------------
      '--- Apre le OPZIONI impostate
      '--------------------------------------------------
      If Not Apri() Then
        Me.Close()
        Return
      End If

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub FRMHHINTE_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
    Try
      'Disabilito eventi di validazione su Entity
      'Distrugge oggetti
      dcInteTesta.Dispose()
      dcInteCorpo.Dispose()
      dsInte.Dispose()
    Catch
    End Try
  End Sub

  Public Overridable Sub FRMHHINTE_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    Try
      '--------------------------------------------------
      '--- Salva il profilo
      '--------------------------------------------------
      If Not Salva(False) Then e.Cancel = True

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub FRMHHINTE_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

      '--------------------------------------------------
      '--- Sempre alla fine di questa funzione: applico le regole della gctl
      '--------------------------------------------------
      GctlSetRoules()

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
      If grvDoc.RowCount = 0 Then Return
      CancellaRiga()
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub tlbRipristinaRiga_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbRipristinaRiga.ItemClick
    Try
      If grvDoc.RowCount = 0 Then Return
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

  'AAAAAAAAAAAAAAAAAAAAAAAAAAA
  Private Sub NtsBarMenuItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles NtsBarMenuItem1.ItemClick
    Try
      Dim strCodArt As String = oApp.InputBoxNew("Articolo")
      Dim strXMLData As String
      Dim httpWebRequest As HttpWebRequest = Nothing
      Dim strEsito As String = ""
      Dim bEsito As Boolean = False
      Dim dtArt As New DataTable

      If strCodArt <> "" Then

        Dim dt As New DataTable
        Dim strDescr As String = ""

        ocleInte.ValCodcieDb(strCodArt, "artico", "S", strDescr, dt)
        If dt Is Nothing Then Return
        If dt.Rows.Count = 0 Then Return

        dtArt.Columns.Add("tb_codart")
        dtArt.Columns.Add("tb_descr")
        Dim drArt As DataRow = dtArt.NewRow()
        drArt!tb_codart = strCodArt
        drArt!tb_descr = dt.Rows(0) !ar_descr
        'drArt!tb_descr = "SEEGER E 21 mm a"

        '--------------------------------------------------
        '--- Crea XML per invio articolo 
        '--------------------------------------------------
        strXMLData = ocleInte.creaXMLAnagrafiche(drArt)

        '--------------------------------------------------
        '--- Esegue la chiamata al URL
        '--------------------------------------------------
        httpWebRequest = ocleInte.PostXML(strXMLData, edTb_urlchia.Text)

        '--------------------------------------------------
        '--- Recupera esito chiamata
        '--------------------------------------------------
        strEsito = ""
        bEsito = ocleInte.postHttp(httpWebRequest, strEsito)
        MsgBox(strEsito)
      End If
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Private Sub NtsBarMenuItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles NtsBarMenuItem2.ItemClick
    Try
      Dim strLista As String = oApp.InputBoxNew("Lista Prelievo")
      Dim strXMLData As String
      Dim httpWebRequest As HttpWebRequest = Nothing
      Dim strEsito As String = ""
      Dim bEsito As Boolean = False
      Dim dtLista As New DataTable
      If strLista <> "" Then

        dtLista.Columns.Add("tb_lista", GetType(String))
        dtLista.Columns.Add("tb_riga", GetType(Integer))
        dtLista.Columns.Add("tb_codart", GetType(String))
        dtLista.Columns.Add("tb_operazione", GetType(String))
        dtLista.Columns.Add("tb_quantita", GetType(Decimal))

        dtLista.Rows.Add(strLista, 1, "49222", "+", 10.0)
        dtLista.Rows.Add(strLista, 2, "49249", "+", 5.0)

        '--------------------------------------------------
        '--- Crea l'XML per invio lista di prelievo
        '--------------------------------------------------
        strXMLData = ocleInte.creaXMLListaDiPrelievo(dtLista)

        '--------------------------------------------------
        '--- Esegue la chiamata al URL
        '--------------------------------------------------
        httpWebRequest = ocleInte.PostXML(strXMLData, edTb_urlchia.Text)

        '--------------------------------------------------
        '--- Recupera esito chiamata
        '--------------------------------------------------
        strEsito = ""
        bEsito = ocleInte.postHttp(httpWebRequest, strEsito)
        'ocleInte.postListaDiPrelievo(edTb_urlchia.Text, strLista, strEsito)
        MsgBox(strEsito)
      End If
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub NtsBarMenuItem5_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles NtsBarMenuItem5.ItemClick
    Try
      Dim strLista As String = oApp.InputBoxNew("Lista Prelievo")
      Dim strXMLData As String
      Dim httpWebRequest As HttpWebRequest = Nothing
      Dim strEsito As String = ""
      Dim bEsito As Boolean = False
      Dim dtLista As New DataTable
      If strLista <> "" Then

        dtLista.Columns.Add("tb_lista", GetType(String))
        dtLista.Columns.Add("tb_riga", GetType(Integer))
        dtLista.Columns.Add("tb_codart", GetType(String))
        dtLista.Columns.Add("tb_operazione", GetType(String))
        dtLista.Columns.Add("tb_quantita", GetType(Decimal))

        dtLista.Rows.Add(strLista, 1, "4883", "-", 10.0)
        dtLista.Rows.Add(strLista, 2, "0438", "-", 5.0)

        '--------------------------------------------------
        '--- Crea l'XML per invio lista di prelievo
        '--------------------------------------------------
        strXMLData = ocleInte.creaXMLListaDiPrelievo(dtLista)

        '--------------------------------------------------
        '--- Esegue la chiamata al URL
        '--------------------------------------------------
        httpWebRequest = ocleInte.PostXML(strXMLData, edTb_urlchia.Text)

        '--------------------------------------------------
        '--- Recupera esito chiamata
        '--------------------------------------------------
        strEsito = ""
        bEsito = ocleInte.postHttp(httpWebRequest, strEsito)
        'ocleInte.postListaDiPrelievo(edTb_urlchia.Text, strLista, strEsito)
        MsgBox(strEsito)
      End If
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Private Sub NtsBarMenuItem4_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles NtsBarMenuItem4.ItemClick
    Try
      Dim strLista As String = oApp.InputBoxNew("Lista Prelievo")
      Dim strXMLData As String
      Dim httpWebRequest As HttpWebRequest = Nothing
      Dim strEsito As String = ""
      Dim bEsito As Boolean = False
      If strLista <> "" Then
        '--------------------------------------------------
        '--- Crea il Json per invio advice 
        '--------------------------------------------------
        strXMLData = ocleInte.creaXMLCancellaLista(strLista)

        '--------------------------------------------------
        '--- Esegue la chiamata al URL
        '--------------------------------------------------
        httpWebRequest = ocleInte.PostXML(strXMLData, edTb_urlchia.Text)

        '--------------------------------------------------
        '--- Recupera esito chiamata
        '--------------------------------------------------
        strEsito = ""
        bEsito = ocleInte.postHttp(httpWebRequest, strEsito)
        'ocleInte.postCancellaLista(edTb_urlchia.Text, strLista, strEsito)
        MsgBox(strEsito)
      End If
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Private Sub NtsBarMenuItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles NtsBarMenuItem3.ItemClick
    Try
      Dim strXMLData As String
      Dim httpWebRequest As HttpWebRequest = Nothing
      Dim strEsito As String = ""
      Dim bEsito As Boolean = False

      '--------------------------------------------------
      '--- Crea l'XML per invio lista di prelievo
      '--------------------------------------------------
      strXMLData = ocleInte.creaXMLOrdiniElaborati()

      '--------------------------------------------------
      '--- Esegue la chiamata al URL
      '--------------------------------------------------
      httpWebRequest = ocleInte.PostXML(strXMLData, edTb_urlchia.Text)

      '--------------------------------------------------
      '--- Recupera esito chiamata
      '--------------------------------------------------
      strEsito = ""
      bEsito = ocleInte.postHttp(httpWebRequest, strEsito)

      'ocleInte.postOrdiniElaborati(edTb_urlchia.Text, strEsito)
      MsgBox(strEsito)
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub
#End Region

#Region "Eventi Controlli"
  Public Overridable Sub edTb_conto_EditValueChanged(sender As Object, e As EventArgs) Handles edTb_conto.EditValueChanged
    Try
      Dim strDescr As String = ""
      If ocleInte Is Nothing Then Return

      ocleInte.ValCodcieDb(edTb_conto.Text.ToString(), "anagra", "N", strDescr, Nothing)
      lbTb_nomeconto.Text = strDescr
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub edTb_tipoBF_EditValueChanged(sender As Object, e As EventArgs) Handles edTb_tipoBF.EditValueChanged
    Try
      Dim strDescr As String = ""
      If ocleInte Is Nothing Then Return

      ocleInte.ValCodcieDb(edTb_tipoBF.Text.ToString(), "tabtpbf", "N", strDescr, Nothing)
      lbTb_nometipoBF.Text = strDescr
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub edTb_causaleCarico_EditValueChanged(sender As Object, e As EventArgs) Handles edTb_causaleCarico.EditValueChanged
    Try
      Dim strDescr As String = ""
      If ocleInte Is Nothing Then Return

      ocleInte.ValCodcieDb(edTb_causaleCarico.Text.ToString(), "tabcaum", "N", strDescr, Nothing)
      lbTb_nomecausaleCarico.Text = strDescr
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub edTb_causaleScarico_EditValueChanged(sender As Object, e As EventArgs) Handles edTb_causaleScarico.EditValueChanged
    Try
      Dim strDescr As String = ""
      If ocleInte Is Nothing Then Return

      ocleInte.ValCodcieDb(edTb_causaleScarico.Text.ToString(), "tabcaum", "N", strDescr, Nothing)
      lbTb_nomecausaleScarico.Text = strDescr
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub edTb_magazzinoMacchina_EditValueChanged(sender As Object, e As EventArgs) Handles edTb_magazzinoMacchina.EditValueChanged
    Try
      Dim strDescr As String = ""
      If ocleInte Is Nothing Then Return

      ocleInte.ValCodcieDb(edTb_magazzinoMacchina.Text.ToString(), "tabmaga", "N", strDescr, Nothing)
      lbTb_nomemagazzinoMacchina.Text = strDescr
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub edTb_magazzinoPrincipale_EditValueChanged(sender As Object, e As EventArgs) Handles edTb_magazzinoPrincipale.EditValueChanged
    Try
      Dim strDescr As String = ""
      If ocleInte Is Nothing Then Return

      ocleInte.ValCodcieDb(edTb_magazzinoPrincipale.Text.ToString(), "tabmaga", "N", strDescr, Nothing)
      lbTb_nomemagazzinoPrincipale.Text = strDescr
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

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

  Public Overridable Sub ckTb_elaborazionegiac1_CheckedChanged(sender As Object, e As EventArgs) Handles ckTb_elaborazionegiac1.CheckedChanged
    Try
      If ckTb_elaborazionegiac1.Checked Then
        edTb_oragiac1.Enabled = True
      Else
        edTb_oragiac1.Enabled = False
      End If
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub ckTb_elaborazionegiac2_CheckedChanged(sender As Object, e As EventArgs) Handles ckTb_elaborazionegiac2.CheckedChanged
    Try
      If ckTb_elaborazionegiac2.Checked Then
        edTb_oragiac2.Enabled = True
      Else
        edTb_oragiac2.Enabled = False
      End If
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub ckTb_elaborazionegiac3_CheckedChanged(sender As Object, e As EventArgs) Handles ckTb_elaborazionegiac3.CheckedChanged
    Try
      If ckTb_elaborazionegiac3.Checked Then
        edTb_oragiac3.Enabled = True
      Else
        edTb_oragiac3.Enabled = False
      End If
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub
#End Region

#Region "Eventi Griglia Lista"
  Public Overridable Sub grvDoc_NTSBeforeRowUpdate(sender As Object, e As DevExpress.XtraGrid.Views.Base.RowAllowEventArgs) Handles grvDoc.NTSBeforeRowUpdate
    Try
      Me.Cursor = Cursors.WaitCursor
      If Not ocleInte.RecordSalva(dcInteCorpo.Position, False, Nothing) Then e.Allow = False 'rimango sulla riga su cui sono
    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    Finally
      Me.Cursor = Cursors.Default
    End Try
  End Sub

  Public Overridable Sub grvDoc_NTSFocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles grvDoc.NTSFocusedRowChanged
    Try
      'If grvDoc Is Nothing Then Return

      'If grvDoc.FocusedRowHandle = NTSGrid.AutoFilterRowHandle Then Return
      'If NTSCStr(grvDoc.GetDataRow(grvDoc.FocusedRowHandle) !tb_tipork) = "" Then
      '  oApp.MsgBoxInfo("Attenzione! Il tipo non può essere vuoto")
      'End If

      'If grvDoc Is Nothing Then Return

      'If grvDoc.FocusedRowHandle = NTSGrid.AutoFilterRowHandle Then Return

      'If grvDoc.FocusedRowHandle = NTSGridView.NESSUNA_RIGA Then Return
      'If Not grvDoc.FocusedRowHandle = NTSGridView.NUOVA_RIGA Then
      '  Select Case grvDoc.GetDataRow(grvDoc.FocusedRowHandle).RowState
      '    Case DataRowState.Added
      '      If Not tb_tipork.Enabled Then
      '        GctlSetVisEnab(tb_tipork, False)
      '        GctlSetVisEnab(tb_serie, False)
      '      End If
      '    Case Else
      '      tb_tipork.Enabled = False
      '      tb_serie.Enabled = False
      '  End Select
      'Else
      '  If Not tb_tipork.Enabled Then
      '    GctlSetVisEnab(tb_tipork, False)
      '    GctlSetVisEnab(tb_serie, False)
      '  End If
      '  grvDoc.NTSMoveFirstColunn()
      'End If
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
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
      NTSFormAddDataBinding(dcInteTesta, Me)
      'NTSFormAddDataBinding(dcInteCorpo, Me)

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Function Apri() As Boolean
    Try
      If bInLoad = True Then Return True
      If dsInte Is Nothing Then Return True

      '--------------------------------------------------
      '--- Apre le connessioni impostate
      '--------------------------------------------------
      dsInte = Nothing
      dsInte = New DataSet
      If Not ocleInte.Apri(dsInte) Then
        Return False
      End If
      dcInteTesta.DataSource = dsInte.Tables("TESTA")
      dcInteCorpo.DataSource = dsInte.Tables("CORPO")
      dsInte.AcceptChanges()

      '--------------------------------------------------
      '--- Collega i controlli della Form se ci sono CONNESSIONI
      '--------------------------------------------------
      Bindcontrols()

      '--------------------------------------------------
      '--- Imposta datacontrol per lista
      '--------------------------------------------------
      grDoc.DataSource = dcInteCorpo

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

  Public Overridable Sub Cancella()
    Try
      '-------------------------------------------------
      '--- Cancellazione il profilo
      '-------------------------------------------------
      Select Case Me.NTSChiediCancella(Nothing, oApp.Tr(Me, 128306256431033721, "Cancellare i settaggi?"))
        Case System.Windows.Forms.DialogResult.No
          Return
        Case System.Windows.Forms.DialogResult.Yes
          dsInte.Tables("TESTA").Rows.Clear()
          dsInte.Tables("CORPO").Rows.Clear()
          ocleInte.Salva(True)
      End Select

    Catch ex As Exception
      '-------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '-------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub CancellaRiga()
    Try

      If grvDoc.GetFocusedDataRow Is Nothing Then Return
      If grvDoc.GetFocusedDataRow.RowState = DataRowState.Detached Then Return

      '-------------------------------------------------
      '--- Cancellazione il profilo
      '-------------------------------------------------
      Select Case Me.NTSChiediCancella(Nothing, oApp.Tr(Me, 128306256431033721, "Cancellare la riga?"))
        Case System.Windows.Forms.DialogResult.No
          Return
        Case System.Windows.Forms.DialogResult.Yes
          Dim drDelete As DataRow = Nothing
          ocleInte.RecordSalva(dcInteCorpo.Position, True, drDelete)
          'dcInteCorpo.RemoveAt(dcInteCorpo.Position)
          ocleInte.Salva(True)
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
          If Not ocleInte.RipristinaInte("TESTA", dcInteTesta.Position, dcInteTesta.Filter) Then Return
          'Esegue il ripristino 
          If Not ocleInte.RipristinaInte("CORPO", dcInteCorpo.Position, dcInteCorpo.Filter) Then Return

          'Lo imposto qui e non in Entity 
          ocleInte.bHasChanges = False
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

      If grvDoc.GetFocusedDataRow Is Nothing Then Return
      If grvDoc.GetFocusedDataRow.RowState = DataRowState.Detached Then Return

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
          If Not ocleInte.RipristinaInte("CORPO", dcInteCorpo.Position, dcInteCorpo.Filter) Then Return

          'Lo imposto qui e non in Entity 
          ocleInte.bHasChanges = False
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
      '--------------------------------------------------
      '--- Se se in caricamento Form
      '--------------------------------------------------
      If bInLoad = True Then Return True

      '--------------------------------------------------
      '--- Valido l'ultimo controllo che ha il focus
      '--------------------------------------------------
      If Not edTb_urlchia.Focused Then Me.ValidaLastControl()

      '--------------------------------------------------
      '--- Salva profilo
      '--------------------------------------------------
      'Me.NTSRipristinaNuovoSenzaModifiche(ocleInte.bHasChanges, dcInte)
      Select Case Me.NTSChiediSalva(Nothing, ocleInte.bHasChanges)
        Case DialogResult.Yes
          'salvo
          '--------------------------------------------------
          '--- Salva record 
          '--------------------------------------------------
          'Salva profilo
          If Not ocleInte.Salva(False) Then Return False
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
