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
'Case "FCustomer" '���������Ŀ�ı� FCustomer������Ŀ�Ĺؼ���
'Set dctTemp = m_BillInterface.GetFieldInfoByKey("FCustomer", "", 0) '���ݹؼ��ֻ�ȡ�ֶ�ģ��
'Set dctTempFld = m_BillInterface.GetDataFld(dctTemp, lRow) '�����ֶ�ģ���ȡ�ֶ�ֵ���󣬾���鿴�ö���ʲô���ݣ�����������ʱADD WACTH
''dctTempFld("FFLD")��ʾ������Ŀ�ı���ֵ��dctTempFld("FDSP")���Ǻ�����Ŀ����ʾֵ
'm_BillInterface.SetFieldValue "FBase1", dctTempFld("FFLD"), Row '��������Ŀ��ֵ�����Զ���ĺ�����Ŀ�ֶ�,FBase1Ϊ�Զ���ĺ�����Ŀ�ֶιؼ���
'End Select


Dim dct As KFO.Dictionary
Dim sKey As String
sKey = "FEntrySelfP0271" 'sKeyΪ�ֶεĹؼ��֣���ӦICClassTableInfo���е�Fkey
Set dct = m_BillInterface.GetFieldInfoByKey(sKey, "", 0) '��ø��ֶε�ģ����Ϣ
'If m_BillInterface.K3Lib.User.UserName = "�ܾ���" Then
'Call m_BillInterface.LockCell(dct, -1, False) 'False��ʾ��
'Else
Call m_BillInterface.LockCell(dct, -1, True) 'True��ʾ����
'End If
Set dct = Nothing

End Sub

