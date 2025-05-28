Namespace AdviceInsertUpdate

  Public Class adviceInsertUpdate
    Public clientId As String
    Public adviceId As String
    Public adviceType As String
    Public referenceBarcode As String
    Public supplierId As String
    'Public purchaseOrderReference As String
    'Public expectationDate As DateTime
    'Public deliveryText As String
    Public additionalHostData As String
    Public adviceLines As cAdviceLines

    Public Class cAdviceLines
      Public adviceLine As IList(Of adviceLine)
    End Class
  End Class

  Public Class adviceLine
    Public adviceLineId As String
    Public skuId As String
    'Public supplierSkuId As String
    'Public purchaseOrderLine As String
    Public quantityTarget As Integer
    Public receivingQuantityUnit As String
    'Public unlimitedOverdeliveryAllowed As Boolean
    'Public overdeliveryTolerance As Integer
    'Public undeliveryTolerance As Integer
    Public batch As String
    'Public hostStorageLocation As String
    Public bestBeforeDate As Date
    'Public deliveryText As String
    'Public HoldReasonld As String
    'Public dateOfReceipt As DateTime
    'Public dateOfManufacture As Date
    'Public additionalHostData As String
  End Class

End Namespace

