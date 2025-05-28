Namespace skuInsertUpdateImage

  Public Class skuInsertUpdate
    Public clientId As String
    Public skuId As String
    Public description As String
    Public productCode As String
    Public productGroupId As String
    Public baseQuantityUnitId As String
    Public handlingHint As String
    Public batchMandatoryForHost As Boolean
    'Public stocktakingExcluded As Boolean
    Public cycleCountingThreshold As Integer
    Public availableQuantityUnits As SkuQuantityUnits
    Public Class SkuQuantityUnits
      Public availableQuantityUnit As IList(Of SkuQuantityUnit)
    End Class
  End Class

  Public Class SkuQuantityUnit
    Public quantityUnitId As String
    'Public unitOfMeasure As Boolean
    Public totalWeight As Decimal
    Public netWeight As Decimal
    Public totalVolume As Decimal
    Public length As Decimal
    Public width As Decimal
    Public height As Decimal
    'Public hostWeightUnitId As Integer
    'Public hostVolumeUnitId As Integer
    'Public hostLengthUnitId As Boolean
    Public defaultPickQuantityUnit As Boolean
    Public factorToBaseQU As Decimal
    Public skuImageList As cSkuImageList
    Public Class cSkuImageList
      Public skuImage As IList(Of SkuImage)
    End Class
  End Class

  Public Class SkuImage
    Public imageName As String
    Public skuImageSource As String
    Public defaultImage As Boolean
  End Class

End Namespace
