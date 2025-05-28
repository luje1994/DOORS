
Partial Public Class FRMHHCOGI
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
  Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
  Public WithEvents NtsBarButtonItem2 As NTSBarButtonItem
  Public WithEvents NtsBarButtonItem3 As NTSBarButtonItem
  Friend WithEvents tlbCaricaSuFTP As DevExpress.XtraBars.BarButtonItem
  Public WithEvents NtsBarButtonItem4 As NTSBarButtonItem
  Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
  Friend WithEvents tlbCerca As NTSBarButtonItem
  Friend WithEvents NtsBarButtonItem5 As NTSBarButtonItem
  Friend WithEvents tlbSeleziona As NTSBarButtonItem
  Friend WithEvents tlbDeseleziona As NTSBarButtonItem
  Friend WithEvents tlbZoom As NTSBarButtonItem
  Friend WithEvents frmArticoli As NTSGroupBox
  Friend WithEvents frmFiltri As NTSGroupBox
  Friend WithEvents lbArticolo As NTSLabel
  Friend WithEvents grCogi As NTSGrid
  Friend WithEvents grvCogi As NTSGridView
  Friend WithEvents tb_articolo As NTSGridColumn
  Friend WithEvents tb_quantmacchina As NTSGridColumn
  Friend WithEvents tb_quantbus As NTSGridColumn
  Friend WithEvents lbNomeArticolo As NTSLabel
  Friend WithEvents tb_descrart As NTSGridColumn
  Friend WithEvents edArticolo As NTSTextBoxStr
End Class