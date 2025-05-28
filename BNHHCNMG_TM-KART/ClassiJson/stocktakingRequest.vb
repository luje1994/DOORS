Namespace StocktakingRequest

  Public Class stocktakingRequest
    Public clientId As String
    Public orderId As String
    Public orderLines As cOrderLines
    Public Class cOrderLines
      Public orderLine As IList(Of orderLine)
    End Class
  End Class

  Public Class orderLine
    Public orderLineId As String
    Public criteriaUsed As String
    Public inventoryCriteria As clsInventoryCriteria

    Public Class clsInventoryCriteria
      Public clientId As String
      Public skuId As String
    End Class
  End Class

End Namespace
