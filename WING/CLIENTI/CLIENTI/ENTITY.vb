Imports CLS_STD.STD

Public Class ENTITY
  Inherits BASE

  Dim oclDal As DAL

  Public Overrides Function Init(oParam As PARAM) As Boolean
    MyBase.Init(oParam, "CLIENTI", "DAL")
    oclDal = CType(MyBase.oclBase, DAL)
    oclDal.Init(oParam)
    Return True
  End Function
End Class
