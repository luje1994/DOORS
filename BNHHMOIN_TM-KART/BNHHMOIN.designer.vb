
Partial Public Class FRMHHMOIN
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
  Friend WithEvents fmFiltri As NTSGroupBox
  Friend WithEvents tlbCerca As NTSBarButtonItem
  Friend WithEvents tlbCancellaRiga As NTSBarButtonItem
  Friend WithEvents fmCorpo As NTSGroupBox
  Friend WithEvents grCorpo As NTSGrid
  Friend WithEvents grvCorpo As NTSGridView
  Friend WithEvents idriga As NTSGridColumn
  Friend WithEvents articleNumber As NTSGridColumn
  Friend WithEvents ar_descr As NTSGridColumn
  Friend WithEvents nominalQuantity As NTSGridColumn
  Friend WithEvents NtsSplitter1 As NTSSplitter
  Friend WithEvents fmTesta As NTSGroupBox
  Friend WithEvents grTesta As NTSGrid
  Friend WithEvents grvTesta As NTSGridView
  Friend WithEvents xx_sel As NTSGridColumn
  Friend WithEvents jobNumber As NTSGridColumn
  Friend WithEvents jobDataOra As NTSGridColumn
  Friend WithEvents NtsPanel3 As NTSPanel
  Public WithEvents pbElabora As NTSProgressBar
  Public WithEvents lbElabora As NTSLabel
  Friend WithEvents NtsGridView1 As NTSGridView
  Friend WithEvents NtsGridView2 As NTSGridView
  Friend WithEvents NtsGridView3 As NTSGridView
  Friend WithEvents NtsGridView4 As NTSGridView
  Friend WithEvents NtsGridView5 As NTSGridView
  Friend WithEvents tlbElabora As NTSBarButtonItem
  Friend WithEvents edDataAJob As NTSTextBoxData
  Friend WithEvents lbDataDaDataAJob As NTSLabel
  Friend WithEvents edDataDaJob As NTSTextBoxData
  Friend WithEvents id As NTSGridColumn
  Friend WithEvents jobPriority As NTSGridColumn
  Friend WithEvents jobStatus As NTSGridColumn
  Friend WithEvents busTipork As NTSGridColumn
  Friend WithEvents busAnno As NTSGridColumn
  Friend WithEvents busSerie As NTSGridColumn
  Friend WithEvents busNumero As NTSGridColumn
  Friend WithEvents tm_datdoc As NTSGridColumn
  Friend WithEvents tm_conto As NTSGridColumn
  Friend WithEvents xx_descrConto As NTSGridColumn
  Friend WithEvents operation As NTSGridColumn
  Friend WithEvents actualQuantity As NTSGridColumn
  Friend WithEvents ar_unmis As NTSGridColumn
  Friend WithEvents containerSize As NTSGridColumn
  Friend WithEvents positionStatus As NTSGridColumn
  Friend WithEvents cbTipork As NTSComboBox
  Friend WithEvents lbTipork As NTSLabel
  Friend WithEvents edSerie As NTSTextBoxStr
  Friend WithEvents lbAnnoSerieNum As NTSLabel
  Friend WithEvents edNumero As NTSTextBoxNum
  Friend WithEvents edAnno As NTSTextBoxNum
  Friend WithEvents edArticolo As NTSTextBoxStr
  Friend WithEvents lbNomeArticolo As NTSLabel
  Friend WithEvents lbArticolo As NTSLabel
  Friend WithEvents lbNomeConto As NTSLabel
  Friend WithEvents edConto As NTSTextBoxNum
  Friend WithEvents lbConto As NTSLabel
  Friend WithEvents edDataADoc As NTSTextBoxData
  Friend WithEvents lbDataDaDataADoc As NTSLabel
  Friend WithEvents edDataDaDoc As NTSTextBoxData
  Friend WithEvents tlbFileRettificaGiacenze As NTSBarMenuItem
  Friend WithEvents tlbConfrontaGiacenze As NTSBarMenuItem
  Friend WithEvents timerElabora As Timer
  Friend WithEvents timerGiacenze As Timer
  Friend WithEvents taskIcon As NotifyIcon
  Friend WithEvents cmTask As ContextMenuStrip
  Friend WithEvents cmTaskChiudi As ToolStripMenuItem
End Class