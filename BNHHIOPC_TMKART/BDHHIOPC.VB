Imports NTSInformatica.CLN__STD
Imports System.Data.OleDb

Public Class CLDHHIOPC
  Inherits CLD__BASE

#Region "Funzioni e Routines"
  Public Overridable Function GetData(ByVal strDitta As String,
                                      ByVal strMacchina As String,
                                      ByRef dsOut As DataSet) As Boolean
    Dim strSQL As String = ""

    Try
      strSQL = "SELECT * FROM tabhhtestaopc " &
                " WHERE codditt = " & CStrSQL(strDitta) &
                " AND tb_codice = " & CStrSQL(strMacchina)

      OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "TESTA", dsOut)

      strSQL = "SELECT * FROM tabhhcorpoopc " &
                " WHERE codditt = " & CStrSQL(strDitta) &
                " AND tb_codice = " & CStrSQL(strMacchina)

      OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "CORPO", dsOut)

      strSQL = "SELECT * FROM tabhhmacchineopc " &
                " WHERE codditt = " & CStrSQL(strDitta)

      OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "MACCHINE", dsOut)

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

  Public Overridable Function GetDataMacchine(ByVal strDitta As String,
                                              ByVal strMacchina As String,
                                              ByRef dsOut As DataSet) As Boolean
    Dim strSQL As String = ""

    Try
      strSQL = "SELECT * FROM tabhhmacchineopc " &
                " WHERE codditt = " & CStrSQL(strDitta)

      If Trim(strMacchina) <> "" Then
        strSQL &= " AND tb_codice LIKE " & CStrSQL("%" & strMacchina & "%") 
      End If

      OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "MACCHINE", dsOut)

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

  Public Overridable Function GetData(ByRef dsOut As DataSet) As Boolean
    Try
      Dim ds As New DataSet
      ds = dsOut.Copy

      '--------------------------------------------------
      '--- Azzero/Creo datatable TESTA
      '--------------------------------------------------
      If ds.Tables.Contains("TESTA") Then
        ds.Tables("TESTA").Clear()
      Else
        ds.Tables.Add("TESTA")
        With ds.Tables("TESTA")
          .Columns.Add("tb_codice", GetType(String))
          .Columns.Add("tb_indirizzo", GetType(String))
          .Columns.Add("tb_utente", GetType(String))
          .Columns.Add("tb_password", GetType(String))
          .Columns.Add("tb_elaborazioneautom", GetType(String))
          .Columns.Add("tb_secelaborazione", GetType(Integer))
        End With
      End If

      '--------------------------------------------------
      '--- Azzero/Creo datatable CORPO
      '--------------------------------------------------
      If ds.Tables.Contains("CORPO") Then
        ds.Tables("CORPO").Clear()
      Else
        ds.Tables.Add("CORPO")
        With ds.Tables("CORPO")
          .Columns.Add("tb_codice", GetType(String))
          .Columns.Add("tb_nodo", GetType(String))
          .Columns.Add("tb_valore", GetType(String))
          .Columns.Add("tb_tiponodo", GetType(String))
        End With
      End If

      '--------------------------------------------------
      '--- Azzero/Creo datatable MACCHINE
      '--------------------------------------------------
      If ds.Tables.Contains("MACCHINE") Then
        ds.Tables("MACCHINE").Clear()
      Else
        ds.Tables.Add("MACCHINE")
        With ds.Tables("MACCHINE")
          .Columns.Add("tb_codice", GetType(String))
          .Columns.Add("tb_batch", GetType(String))
          .Columns.Add("tb_stato", GetType(Integer))
          .Columns.Add("tb_idprocess", GetType(Integer))
        End With
      End If

      '--------------------------------------------------
      '--- Legge le CHIAMATE.da registro di Business
      '--- e le carica nel DataSet
      '--------------------------------------------------
      Dim strTesta As String = GetSettingBus("BSHHIOPC", "OPZIONI", ".", "HH_OPTESTA", " ", " ", " ").Trim
      If strTesta.Trim.Length > 0 Then
        Dim srXmlTesta As System.IO.StringReader = New System.IO.StringReader(strTesta)
        ds.ReadXml(srXmlTesta)
      End If

      Dim strCorpo As String = GetSettingBus("BSHHIOPC", "OPZIONI", ".", "HH_OPCORPO", " ", " ", " ").Trim
      If strCorpo.Trim.Length > 0 Then
        Dim srXmlCorpo As System.IO.StringReader = New System.IO.StringReader(strCorpo)
        ds.ReadXml(srXmlCorpo)
      End If

      Dim strMacchine As String = GetSettingBus("BSHHIOPC", "OPZIONI", ".", "HH_OPMACCHINE", " ", " ", " ").Trim
      If strMacchine.Trim.Length > 0 Then
        Dim srXmlMacchine As System.IO.StringReader = New System.IO.StringReader(strMacchine)
        ds.ReadXml(srXmlMacchine)
      End If

      If dsOut.Tables.Contains("TESTA") Then
        dsOut.Tables("TESTA").Clear()
      End If
      ds.Tables("TESTA").DefaultView.Sort = "tb_codice ASC"
      Dim dtTesta As New DataTable
      dtTesta = dsOut.Tables("TESTA")
      dtTesta = ds.Tables("TESTA").DefaultView.ToTable.Copy

      'If dtTesta.Rows.Count = 0 Then
      '  dtTesta.Rows.Add(dtTesta.NewRow)
      'End If

      dsOut = New DataSet

      dsOut.Tables.Add(dtTesta)

      If dsOut.Tables.Contains("CORPO") Then
        dsOut.Tables("CORPO").Clear()
      End If
      'ds.Tables("CORPO").DefaultView.Sort = "tb_nodo ASC "
      Dim dtCorpo As New DataTable
      dtCorpo = dsOut.Tables("CORPO")
      dtCorpo = ds.Tables("CORPO").DefaultView.ToTable.Copy
      dsOut.Tables.Add(dtCorpo)

      If dsOut.Tables.Contains("MACCHINE") Then
        dsOut.Tables("MACCHINE").Clear()
      End If
      ds.Tables("MACCHINE").DefaultView.Sort = "tb_codice ASC "
      Dim dtMacchine As New DataTable
      dtMacchine = dsOut.Tables("MACCHINE")
      dtMacchine = ds.Tables("MACCHINE").DefaultView.ToTable.Copy
      dsOut.Tables.Add(dtMacchine)

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

  Public Overridable Function SetData(ByVal ds As DataSet, ByVal bDelete As Boolean) As Boolean
    Dim strSQL As String = ""
    Dim lResult As Integer = 0
    Try

      If bDelete Then

        For Each drTesta As DataRow In ds.Tables("TESTA").Select("", "", DataViewRowState.Deleted)

          strSQL = " DELETE FROM tabhhtestaopc " &
                    " WHERE codditt = " & CStrSQL(drTesta("codditt", DataRowVersion.Original)) &
                    " AND tb_codice = " & CStrSQL(drTesta("tb_codice", DataRowVersion.Original))

          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False

          strSQL = " DELETE FROM tabhhcorpoopc " &
                    " WHERE codditt = " & CStrSQL(drTesta("codditt", DataRowVersion.Original)) &
                    " AND tb_codice = " & CStrSQL(drTesta("tb_codice", DataRowVersion.Original))

          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False

          strSQL = " DELETE FROM tabhhmacchineopc " &
                    " WHERE codditt = " & CStrSQL(drTesta("codditt", DataRowVersion.Original)) &
                    " AND tb_codice = " & CStrSQL(drTesta("tb_codice", DataRowVersion.Original))

          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False

        Next

        'For Each drCorpo As DataRow In ds.Tables("CORPO").Select("", "", DataViewRowState.Deleted)

        '  strSQL = " DELETE FROM tabhhcorpoopc " &
        '              " WHERE codditt = " & CStrSQL(drCorpo!codditt) &
        '              " AND tb_codice = " & CStrSQL(drCorpo!tb_codice) &
        '              " AND tb_nodo = " & CStrSQL(drCorpo!tb_nodo)

        '  lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
        '  If lResult < 0 Then Return False

        'Next

        'For Each drMacchine As DataRow In ds.Tables("MACCHINE").Select("", "", DataViewRowState.Deleted)

        '  strSQL = " DELETE FROM tabhhmacchineopc " &
        '              " WHERE codditt = " & CStrSQL(drMacchine!codditt) &
        '              " AND tb_codice = " & CStrSQL(drMacchine!tb_codice)

        '  lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
        '  If lResult < 0 Then Return False

        'Next

      Else

        For Each drTesta As DataRow In ds.Tables("TESTA").Rows

          Select Case drTesta.RowState
            Case DataRowState.Added
              strSQL = "INSERT INTO tabhhtestaopc " & GetQueryInsertField(ds.Tables("TESTA"), "tb_") &
                                         " VALUES " & GetQueryInsertValue(ds.Tables("TESTA"), drTesta, "tb_")

              ds.Tables("MACCHINE").Columns("tb_codice").DefaultValue = NTSCStr(drTesta!tb_codice)
              ds.Tables("MACCHINE").Rows.Add(ds.Tables("MACCHINE").NewRow())

              lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
              If lResult < 0 Then Return False
            Case DataRowState.Modified
              strSQL = "UPDATE tabhhtestaopc SET " & GetQueryUpdate(ds.Tables("TESTA"), drTesta, "tb_") &
                      " WHERE codditt = " & CStrSQL(drTesta!codditt) &
                      " AND tb_codice = " & CStrSQL(drTesta!tb_codice)
              lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
              If lResult < 0 Then Return False
          End Select

          strSQL = " DELETE FROM tabhhcorpoopc " &
                      " WHERE codditt = " & CStrSQL(drTesta!codditt) &
                      " AND tb_codice = " & CStrSQL(drTesta!tb_codice)

          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False

        Next


        For Each drCorpo As DataRow In ds.Tables("CORPO").Select("ISNULL(tb_nodo,'') <> ''")

          strSQL = "INSERT INTO tabhhcorpoopc " & GetQueryInsertField(ds.Tables("CORPO"), "tb_") &
                                         " VALUES " & GetQueryInsertValue(ds.Tables("CORPO"), drCorpo, "tb_")
          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False

          'Select Case drCorpo.RowState
          '  Case DataRowState.Added
          '    strSQL = "INSERT INTO tabhhcorpoopc " & GetQueryInsertField(ds.Tables("CORPO"), "tb_") &
          '                               " VALUES " & GetQueryInsertValue(ds.Tables("CORPO"), drCorpo, "tb_")
          '    lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          '    If lResult < 0 Then Return False
          '  Case DataRowState.Modified
          '    strSQL = "UPDATE tabhhcorpoopc SET " & GetQueryUpdate(ds.Tables("CORPO"), drCorpo, "tb_") &
          '            " WHERE codditt = " & CStrSQL(drCorpo!codditt) &
          '            " AND tb_codice = " & CStrSQL(drCorpo!tb_codice) &
          '            " AND tb_nodo = " & CStrSQL(drCorpo!tb_nodo)
          '    lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          '    If lResult < 0 Then Return False
          'End Select

        Next

        For Each drMacchine As DataRow In ds.Tables("MACCHINE").Select("", "", DataViewRowState.Added)

          strSQL = "INSERT INTO tabhhmacchineopc " & GetQueryInsertField(ds.Tables("MACCHINE"), "tb_") &
                                            " VALUES " & GetQueryInsertValue(ds.Tables("MACCHINE"), drMacchine, "tb_")
          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False

        Next

      End If

      ds.AcceptChanges()

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

  Public Overridable Function SetData2(ByVal ds As DataSet) As Boolean
    Try

      '--------------------------------------------------
      '--- Salva il dataset in formato XML nel registro Business
      '--------------------------------------------------
      'Scrive il dataset in XML
      Dim sbStrTesta As New System.Text.StringBuilder
      Dim swXmlTesta As System.IO.StringWriter = New System.IO.StringWriter(sbStrTesta)

      ds.Tables("TESTA").WriteXml(swXmlTesta, XmlWriteMode.IgnoreSchema)
      Dim strXmlTesta As String = sbStrTesta.ToString()
      SaveSettingBus("BSHHIOPC", "OPZIONI", ".", "HH_OPTESTA", strXmlTesta, " ", False, True, False)

      Dim sbStrCorpo As New System.Text.StringBuilder
      Dim swXmlCorpo As System.IO.StringWriter = New System.IO.StringWriter(sbStrCorpo)

      ds.Tables("CORPO").WriteXml(swXmlCorpo, XmlWriteMode.IgnoreSchema)
      Dim strXmlCorpo As String = sbStrCorpo.ToString()
      SaveSettingBus("BSHHIOPC", "OPZIONI", ".", "HH_OPCORPO", strXmlCorpo, " ", False, True, False)

      Dim sbStrMacchine As New System.Text.StringBuilder
      Dim swXmlMacchine As System.IO.StringWriter = New System.IO.StringWriter(sbStrMacchine)

      ds.Tables("MACCHINE").WriteXml(swXmlMacchine, XmlWriteMode.IgnoreSchema)
      Dim strXmlMacchine As String = sbStrMacchine.ToString()
      SaveSettingBus("BSHHIOPC", "OPZIONI", ".", "HH_OPMACCHINE", strXmlMacchine, " ", False, True, False)

      ds.AcceptChanges()

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

  Public Overridable Function SetDataMacchina(ByVal ds As DataSet, ByVal bDelete As Boolean) As Boolean
    Dim strSQL As String = ""
    Dim lResult As Integer = 0
    Try

      '--------------------------------------------------
      If bDelete Then

        For Each drMacchine As DataRow In ds.Tables("MACCHINE").Select("", "", DataViewRowState.Deleted)

          strSQL = " DELETE FROM tabhhmacchineopc " &
                    " WHERE codditt = " & CStrSQL(drMacchine("codditt", DataRowVersion.Original)) &
                    " AND tb_codice = " & CStrSQL(drMacchine("tb_codice", DataRowVersion.Original))

          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False

        Next

      Else

        For Each drMacchine As DataRow In ds.Tables("MACCHINE").Select("", "", DataViewRowState.ModifiedCurrent)

          strSQL = "UPDATE tabhhmacchineopc SET " & GetQueryUpdate(ds.Tables("MACCHINE"), drMacchine, "tb_") &
                      " WHERE codditt = " & CStrSQL(drMacchine!codditt) &
                      " AND tb_codice = " & CStrSQL(drMacchine!tb_codice)
          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False
          'Select Case drMacchine.RowState
          '  Case DataRowState.Added
          '    strSQL = "INSERT INTO tabhhmacchineopc " & GetQueryInsertField(ds.Tables("MACCHINE"), "tb_") &
          '                                  " VALUES " & GetQueryInsertValue(ds.Tables("MACCHINE"), drMacchine, "tb_")
          '    lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          '    If lResult < 0 Then Return False
          '  Case DataRowState.Modified
          '    strSQL = "UPDATE tabhhmacchineopc SET " & GetQueryUpdate(ds.Tables("MACCHINE"), drMacchine, "tb_") &
          '            " WHERE codditt = " & CStrSQL(drMacchine!codditt) &
          '            " AND tb_codice = " & CStrSQL(drMacchine!tb_codice)
          '    lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          '    If lResult < 0 Then Return False
          'End Select

        Next

      End If

      ds.AcceptChanges()

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

  Public Overridable Function SetDataMacchina2(ByVal ds As DataSet) As Boolean
    Try

      '--------------------------------------------------
      '--- Salva il dataset in formato XML nel registro Business
      '--------------------------------------------------
      'Scrive il dataset in XML
      Dim sbStrMacchine As New System.Text.StringBuilder
      Dim swXmlMacchine As System.IO.StringWriter = New System.IO.StringWriter(sbStrMacchine)

      ds.Tables("MACCHINE").WriteXml(swXmlMacchine, XmlWriteMode.IgnoreSchema)
      Dim strXmlMacchine As String = sbStrMacchine.ToString()
      SaveSettingBus("BSHHIOPC", "OPZIONI", ".", "HH_OPMACCHINE", strXmlMacchine, " ", False, True, False)

      ds.AcceptChanges()

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

  Public Overridable Function ApriModalita(strDitta As String, ByRef dsOut As DataSet) As Boolean

    Dim strSQL As String = ""

    Try
      If dsOut IsNot Nothing Then
        If dsOut.Tables("MODA") IsNot Nothing Then dsOut.Tables.Remove("MODA")
      End If

      '--------------------------------------------------------------------------------------------------------------
      strSQL = "SELECT * FROM tabhhmodopc " &
                " WHERE codditt = " & CStrSQL(strDitta) &
                " ORDER BY tb_codice "

      OpenRecordset(strSQL, CLE__APP.DBTIPO.DBAZI, "MODA", dsOut)

      '--------------------------------------------------------------------------------------------------------------
      Return True
      '--------------------------------------------------------------------------------------------------------------
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
      Return False
    End Try
  End Function

  Public Overridable Function SalvaModalita(dtMod As DataTable, bDelete As Boolean) As Boolean
    Dim bReturn As Boolean = False
    Dim strSQL As String = ""
    Dim lResult As Integer = 0

    Try

      If bDelete Then
        For Each dr As DataRow In dtMod.Select("", "", DataViewRowState.Deleted)
          strSQL = " DELETE FROM tabhhmodopc " &
                    " WHERE codditt = " & CStrSQL(dr("codditt", DataRowVersion.Original)) &
                    " AND tb_codice = " & CStrSQL(dr("tb_codice", DataRowVersion.Original))
          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False
        Next
      Else
        For Each dr As DataRow In dtMod.Select("", "", DataViewRowState.Added Or DataViewRowState.ModifiedCurrent)
          Select Case dr.RowState
            Case DataRowState.Added
              strSQL = " INSERT INTO tabhhmodopc " & GetQueryInsertField(dtMod, "tb_") &
                                          " VALUES " & GetQueryInsertValue(dtMod, dr, "tb_")

            Case DataRowState.Modified
              strSQL = " UPDATE tabhhmodopc SET " & GetQueryUpdate(dtMod, dr, "tb_") &
                                          " WHERE codditt = " & CStrSQL(dr!codditt) &
                                          " AND tb_codice = " & CStrSQL(dr!tb_codice)
          End Select
          lResult = Execute(strSQL, CLE__APP.DBTIPO.DBAZI)
          If lResult < 0 Then Return False
        Next
      End If

      dtMod.AcceptChanges()

      bReturn = True
    Catch ex As Exception
      CLN__STD.GestErr(ex, Me, "")
    End Try

    Return bReturn
  End Function
#End Region

End Class

