<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TimbratureForm
  Inherits System.Windows.Forms.Form

  'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Richiesto da Progettazione Windows Form
  Private components As System.ComponentModel.IContainer

  'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
  'Può essere modificata in Progettazione Windows Form.  
  'Non modificarla mediante l'editor del codice.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
        Me.LabelDate = New System.Windows.Forms.Label()
        Me.ListBoxEntries = New System.Windows.Forms.ListBox()
        Me.DateTimePickerEntry = New System.Windows.Forms.DateTimePicker()
        Me.ButtonAdd = New System.Windows.Forms.Button()
        Me.DateTimePickerExit = New System.Windows.Forms.DateTimePicker()
        Me.LabelEntrata = New System.Windows.Forms.Label()
        Me.LabelUscita = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'LabelDate
        '
        Me.LabelDate.AutoSize = True
        Me.LabelDate.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelDate.Location = New System.Drawing.Point(0, 0)
        Me.LabelDate.Name = "LabelDate"
        Me.LabelDate.Size = New System.Drawing.Size(0, 20)
        Me.LabelDate.TabIndex = 0
        '
        'ListBoxEntries
        '
        Me.ListBoxEntries.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxEntries.Font = New System.Drawing.Font("Monotype Corsiva", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxEntries.FormattingEnabled = True
        Me.ListBoxEntries.ItemHeight = 15
        Me.ListBoxEntries.Location = New System.Drawing.Point(3, 33)
        Me.ListBoxEntries.Name = "ListBoxEntries"
        Me.ListBoxEntries.Size = New System.Drawing.Size(298, 199)
        Me.ListBoxEntries.TabIndex = 1
        '
        'DateTimePickerEntry
        '
        Me.DateTimePickerEntry.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.DateTimePickerEntry.Location = New System.Drawing.Point(3, 238)
        Me.DateTimePickerEntry.Name = "DateTimePickerEntry"
        Me.DateTimePickerEntry.ShowUpDown = True
        Me.DateTimePickerEntry.Size = New System.Drawing.Size(100, 20)
        Me.DateTimePickerEntry.TabIndex = 2
        '
        'ButtonAdd
        '
        Me.ButtonAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonAdd.Location = New System.Drawing.Point(12, 293)
        Me.ButtonAdd.Name = "ButtonAdd"
        Me.ButtonAdd.Size = New System.Drawing.Size(282, 23)
        Me.ButtonAdd.TabIndex = 3
        Me.ButtonAdd.Text = "Aggiungi"
        Me.ButtonAdd.UseVisualStyleBackColor = True
        '
        'DateTimePickerExit
        '
        Me.DateTimePickerExit.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.DateTimePickerExit.Location = New System.Drawing.Point(3, 264)
        Me.DateTimePickerExit.Name = "DateTimePickerExit"
        Me.DateTimePickerExit.ShowUpDown = True
        Me.DateTimePickerExit.Size = New System.Drawing.Size(100, 20)
        Me.DateTimePickerExit.TabIndex = 4
        '
        'LabelEntrata
        '
        Me.LabelEntrata.AutoSize = True
        Me.LabelEntrata.Location = New System.Drawing.Point(109, 240)
        Me.LabelEntrata.Name = "LabelEntrata"
        Me.LabelEntrata.Size = New System.Drawing.Size(41, 13)
        Me.LabelEntrata.TabIndex = 5
        Me.LabelEntrata.Text = "Entrata"
        '
        'LabelUscita
        '
        Me.LabelUscita.AutoSize = True
        Me.LabelUscita.Location = New System.Drawing.Point(109, 267)
        Me.LabelUscita.Name = "LabelUscita"
        Me.LabelUscita.Size = New System.Drawing.Size(37, 13)
        Me.LabelUscita.TabIndex = 6
        Me.LabelUscita.Text = "Uscita"
        '
        'TimbratureForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(306, 328)
        Me.Controls.Add(Me.LabelUscita)
        Me.Controls.Add(Me.LabelEntrata)
        Me.Controls.Add(Me.DateTimePickerExit)
        Me.Controls.Add(Me.ButtonAdd)
        Me.Controls.Add(Me.DateTimePickerEntry)
        Me.Controls.Add(Me.ListBoxEntries)
        Me.Controls.Add(Me.LabelDate)
        Me.Name = "TimbratureForm"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelDate As Label
    Friend WithEvents ListBoxEntries As ListBox
    Friend WithEvents DateTimePickerEntry As DateTimePicker
    Friend WithEvents ButtonAdd As Button
    Friend WithEvents DateTimePickerExit As DateTimePicker
    Friend WithEvents LabelEntrata As Label
    Friend WithEvents LabelUscita As Label
End Class
