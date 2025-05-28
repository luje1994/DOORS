Namespace OrderInsertUpdate

  Public Class orderInsertUpdate
    Public clientId As String
    Public orderId As String
    Public priority As Integer
    Public customerId As String
    Public additionalHostData As String
    Public earliestStartTime As DateTime
    Public latestStartTime As DateTime
    Public earliestStagingTime As DateTime
    Public latestStagingTime As DateTime
    'Public shippingTimeTarget As DateTime
    Public customerOrderType As String
    Public defaultDestinationId As String
    Public shippingCondition As String
    Public deliveryTime As DateTime
    'Public reason As String
    Public orderLines As cOrderLines
    Public Class cOrderLines
      Public orderLine As IList(Of orderLine)
    End Class
  End Class

  Public Class orderLine
    Public orderLineId As String
    'Public hostOrderType As String
    Public criteriaUsed As String
    Public inventoryCriteria As clsInventoryCriteria
    Public Class clsInventoryCriteria
      Public skuId As String
      Public batch As String
      'Public hostStorageLocation As String
      'Public holdReasonId As String
      'Public fulfillmentrequirement As String
      Public quantityBaseTargetHost As Integer
      Public quantityUnit As String
      Public unlimitedOverdeliveryAllowed As Boolean
      'Public overdeliveryTolerance As Integer
      Public underdeliveryTolerance As Integer
      'Public specialInventoryMark As String
      'Public specialInventoryReferenceId As String
      'Public bestBeforedate As Date
      'Public minQuantityPerLU As Integer
      'Public roundingMethod As String
      'Public minimumRestQuantity As Integer
    End Class
    'Public locatioId As String
    'Public loadUnitId As String
    'Public finalDestinationLocation As String
    'Public additionalHostData As String
    'Public holdOption As String
    'Public hostMoveReason As String
    'Public hostMoveType As String
  End Class

End Namespace
