VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Class1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True



Private Sub m_BillInterface_Change(ByVal dct As KFO.Dictionary, ByVal dctFld As KFO.Dictionary, ByVal Col As Long, ByVal Row As Long, Cancel As Boolean)

'Dim dctTemp As KFO.Dictionary
'Dim dctTempFld As KFO.Dictionary
'Select Case dct("FKey")
'Case "FCustomer" '如果核算项目改变 FCustomer核算项目的关键字
'Set dctTemp = m_BillInterface.GetFieldInfoByKey("FCustomer", "", 0) '根据关键字获取字段模板
'Set dctTempFld = m_BillInterface.GetDataFld(dctTemp, lRow) '根据字段模板获取字段值对象，具体查看该对象什么内容，可以在运行时ADD WACTH
''dctTempFld("FFLD")表示核算项目的保存值，dctTempFld("FDSP")便是核算项目的显示值
'm_BillInterface.SetFieldValue "FBase1", dctTempFld("FFLD"), Row '将核算项目的值赋给自定义的核算项目字段,FBase1为自定义的核算项目字段关键字
'End Select


Dim dct As KFO.Dictionary
Dim sKey As String
sKey = "FEntrySelfP0271" 'sKey为字段的关键字，对应ICClassTableInfo的中的Fkey
Set dct = m_BillInterface.GetFieldInfoByKey(sKey, "", 0) '获得改字段的模板信息
'If m_BillInterface.K3Lib.User.UserName = "总经理" Then
'Call m_BillInterface.LockCell(dct, -1, False) 'False表示解
'Else
Call m_BillInterface.LockCell(dct, -1, True) 'True表示锁定
'End If
Set dct = Nothing

End Sub


