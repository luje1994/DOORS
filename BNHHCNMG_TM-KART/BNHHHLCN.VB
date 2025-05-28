Imports System.Data
Imports NTSInformatica.CLN__STD

Public Class FRMHHHLCN

#Region "Variabili"
  Public ocleCnmg As CLEHHCNMG
  Public oCallParams As CLE__CLDP
  Public dsHlcn As New DataSet
  Public dcHlcn As New BindingSource()
  Public dttHlcn As New DataTable
#End Region

#Region "Controlli"
  Private components As System.ComponentModel.IContainer
  Public WithEvents pnHlcn As NTSInformatica.NTSPanel
  Public WithEvents grHlcn As NTSInformatica.NTSGrid
  Public WithEvents grvHlcn As NTSInformatica.NTSGridView
  Public WithEvents ch_codchia As NTSInformatica.NTSGridColumn
  Public WithEvents ch_deschia As NTSInformatica.NTSGridColumn
  Public WithEvents cmdAnnulla As NTSInformatica.NTSButton
  Public WithEvents cmdConferma As NTSInformatica.NTSButton
#End Region

#Region "Inizialzzazione"
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
    '--- Valore funzione
    '--------------------------------------------------
    Return True
  End Function

  Public Overridable Sub InitControls()
    InitControlsBeginEndInit(Me, False)
    Try
      '--------------------------------------------------
      '--- Imposta controlli
      '--------------------------------------------------
      grvHlcn.NTSSetParam(oMenu, oApp.Tr(Me, 128309722050644032, "Zoom Connessioni database"))
      ch_codchia.NTSSetParamNUM(oMenu, oApp.Tr(Me, 128898040669076019, ch_codchia.Caption), "0", 4, 0, 9999)
      ch_deschia.NTSSetParamSTR(oMenu, oApp.Tr(Me, 128898040687670007, ch_deschia.Caption), 0, True)
      grvHlcn.NTSAllowInsert = False
      grvHlcn.NTSAllowUpdate = False
      grvHlcn.Enabled = False

      '--------------------------------------------------
      '--- Chiamo lo script per inizializzare i controlli caricati con source ext
      '--------------------------------------------------
      NTSScriptExec("InitControls", Me, Nothing)
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    Finally
      InitControlsBeginEndInit(Me, True)
    End Try
  End Sub

  Public Overridable Sub InitializeComponent()
    Me.pnHlcn = New NTSInformatica.NTSPanel()
    Me.grHlcn = New NTSInformatica.NTSGrid()
    Me.grvHlcn = New NTSInformatica.NTSGridView()
    Me.ch_codchia = New NTSInformatica.NTSGridColumn()
    Me.ch_deschia = New NTSInformatica.NTSGridColumn()
    Me.cmdAnnulla = New NTSInformatica.NTSButton()
    Me.cmdConferma = New NTSInformatica.NTSButton()
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pnHlcn, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnHlcn.SuspendLayout()
    CType(Me.grHlcn, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.grvHlcn, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'pnHlcn
    '
    Me.pnHlcn.Appearance.BackColor = System.Drawing.Color.Transparent
    Me.pnHlcn.Appearance.Options.UseBackColor = True
    Me.pnHlcn.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    Me.pnHlcn.Controls.Add(Me.grHlcn)
    Me.pnHlcn.Controls.Add(Me.cmdAnnulla)
    Me.pnHlcn.Controls.Add(Me.cmdConferma)
    Me.pnHlcn.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnHlcn.Location = New System.Drawing.Point(0, 0)
    Me.pnHlcn.Name = "pnHlcn"
    Me.pnHlcn.Size = New System.Drawing.Size(506, 226)
    '
    'grHlcn
    '
    Me.grHlcn.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grHlcn.Location = New System.Drawing.Point(4, 4)
    Me.grHlcn.MainView = Me.grvHlcn
    Me.grHlcn.Name = "grHlcn"
    Me.grHlcn.Size = New System.Drawing.Size(392, 216)
    Me.grHlcn.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvHlcn})
    '
    'grvHlcn
    '
    Me.grvHlcn.ActiveFilterEnabled = False
    Me.grvHlcn.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.ch_codchia, Me.ch_deschia})
    Me.grvHlcn.CustomizationFormBounds = New System.Drawing.Rectangle(680, 326, 208, 170)
    Me.grvHlcn.Enabled = True
    Me.grvHlcn.GridControl = Me.grHlcn
    Me.grvHlcn.Name = "grvHlcn"
    Me.grvHlcn.OptionsCustomization.AllowRowSizing = True
    Me.grvHlcn.OptionsNavigation.EnterMoveNextColumn = True
    Me.grvHlcn.OptionsNavigation.UseTabKey = False
    Me.grvHlcn.OptionsSelection.EnableAppearanceFocusedRow = False
    Me.grvHlcn.OptionsView.ColumnAutoWidth = False
    Me.grvHlcn.OptionsView.EnableAppearanceEvenRow = True
    Me.grvHlcn.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden
    Me.grvHlcn.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
    Me.grvHlcn.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
    Me.grvHlcn.OptionsView.ShowGroupPanel = False
    '
    'ch_codchia
    '
    Me.ch_codchia.AppearanceCell.Options.UseTextOptions = True
    Me.ch_codchia.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
    Me.ch_codchia.Caption = "Cod. Connessione"
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
    'cmdAnnulla
    '
    Me.cmdAnnulla.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdAnnulla.Location = New System.Drawing.Point(404, 44)
    Me.cmdAnnulla.Name = "cmdAnnulla"
    Me.cmdAnnulla.Size = New System.Drawing.Size(68, 26)
    Me.cmdAnnulla.Text = "&Annulla"
    '
    'cmdConferma
    '
    Me.cmdConferma.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdConferma.Location = New System.Drawing.Point(404, 12)
    Me.cmdConferma.Name = "cmdConferma"
    Me.cmdConferma.Size = New System.Drawing.Size(68, 26)
    Me.cmdConferma.Text = "&Conferma"
    '
    'FRMHHHLCN
    '
    Me.ClientSize = New System.Drawing.Size(506, 226)
    Me.Controls.Add(Me.pnHlcn)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "FRMHHHLCN"
    Me.Text = "ZOOM CHIAMATE"
    CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pnHlcn, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnHlcn.ResumeLayout(False)
    CType(Me.grHlcn, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.grvHlcn, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

  Public Overridable Sub InitEntity(ByRef cleCnmg As CLEHHCNMG)
    ocleCnmg = cleCnmg
    AddHandler ocleCnmg.RemoteEvent, AddressOf GestisciEventiEntity
  End Sub
#End Region

#Region "Eventi Form"
  Public Overridable Sub FRMHHHLCN_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Try
      '--------------------------------------------------
      '--- Predispongo i controlli
      '--------------------------------------------------
      InitControls()

      '--------------------------------------------------
      '--- Collego il NTSBindingNavigator
      '--------------------------------------------------
      dsHlcn.Tables.Add(dttHlcn)
      dcHlcn.DataSource = dsHlcn.Tables(0)
      grHlcn.DataSource = dcHlcn

      '--------------------------------------------------
      '--- Sempre alla fine di questa funzione: applico le regole della gctl
      '--------------------------------------------------
      GctlSetRoules()

    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub FRMHHHLCN_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
    Try
      dcHlcn.Dispose()
      dsHlcn.Dispose()
    Catch
    End Try
  End Sub
#End Region

#Region "Eventi Controlli"
  Public Overridable Sub cmdAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAnnulla.Click
    Try
      CType(ocleCnmg, CLEHHCNMG).lCodconnSel = 0
      Me.Close()
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub cmdConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConferma.Click
    Try
      CType(ocleCnmg, CLEHHCNMG).lCodconnSel = NTSCInt(dsHlcn.Tables(0).Rows(dcHlcn.Position) !ch_codchia)
      Me.Close()
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------
    End Try
  End Sub

  Public Overridable Sub grvHlcn_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grvHlcn.DoubleClick
    Try
      If cmdConferma.Enabled Then cmdConferma_Click(cmdConferma, Nothing)
    Catch ex As Exception
      '--------------------------------------------------
      CLN__STD.GestErr(ex, Me, "")
      '--------------------------------------------------	
    End Try
  End Sub
#End Region

End Class