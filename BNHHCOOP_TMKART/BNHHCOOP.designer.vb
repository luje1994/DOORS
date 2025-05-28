
Partial Public Class FRMHHCOOP
  Inherits FRM__CHIL

  <System.Diagnostics.DebuggerNonUserCode()>
  Public Sub New()
    MyBase.New()
  End Sub

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()>
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing AndAlso components IsNot Nothing Then
      components.Dispose()
    End If
    MyBase.Dispose(disposing)
  End Sub
  Friend WithEvents NtsBarButtonItem1 As NTSBarButtonItem
  Public WithEvents tlbConverti As NTSBarMenuItem
  Friend WithEvents sel As NTSGridColumn
  Friend WithEvents tlbStrumenti As NTSBarSubItem
  Friend WithEvents tlbSeleziona As NTSBarButtonItem
  Friend WithEvents tlbDeseleziona As NTSBarButtonItem
  Friend WithEvents tlbZoom As NTSBarButtonItem
  Friend WithEvents tlbLavorati As NTSBarButtonItem
  Friend WithEvents tlbScarica As NTSBarButtonItem
  Public WithEvents pbElabora1 As NTSProgressBar
  Friend WithEvents fmConnettore As NTSGroupBox
  Friend WithEvents tlbCancellaRiga As NTSBarButtonItem
  Friend WithEvents NtsGridView1 As NTSGridView
  Friend WithEvents NtsGridView2 As NTSGridView
  Friend WithEvents NtsGridView3 As NTSGridView
  Friend WithEvents NtsGridView4 As NTSGridView
  Friend WithEvents NtsGridView5 As NTSGridView
  Friend WithEvents tlbElabora As NTSBarButtonItem
  Friend WithEvents tlbFileRettificaGiacenze As NTSBarMenuItem
  Friend WithEvents tlbConfrontaGiacenze As NTSBarMenuItem
  Friend WithEvents timerElabora As Timer
  Friend WithEvents timerConnect As Timer
  Friend WithEvents grCoop As NTSGrid
  Friend WithEvents grvCoop As NTSGridView
  Friend WithEvents tb_codice As NTSGridColumn
  Friend WithEvents tb_batch As NTSGridColumn
  Friend WithEvents RepositoryItemRadioGroup1 As DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup
  Friend WithEvents RepositoryItemButtonEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
  Friend WithEvents RepositoryItemToggleSwitch1 As DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch
  Friend WithEvents RepositoryItemToggleSwitch2 As DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch
  Friend WithEvents tb_stato As NTSGridColumn
  Friend WithEvents RepositoryItemToggleSwitch3 As DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch
  Friend WithEvents RepositoryItemToggleSwitch4 As DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch
  Friend WithEvents RepositoryItemZoomTrackBar1 As DevExpress.XtraEditors.Repository.RepositoryItemZoomTrackBar
  Friend WithEvents RepositoryItemTrackBar1 As DevExpress.XtraEditors.Repository.RepositoryItemTrackBar
  Friend WithEvents RepositoryItemTokenEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTokenEdit
  Friend WithEvents tlbCerca As NTSBarButtonItem
  Friend WithEvents tb_startstop As NTSGridColumn
  Friend WithEvents tlbImpostazioni As NTSBarButtonItem
End Class