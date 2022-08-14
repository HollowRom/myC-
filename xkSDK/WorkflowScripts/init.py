from System import DateTime
import clr
clr.AddReference("Kingdee.BOS.Workflow")
from Kingdee.BOS.Workflow import *

#日期转换函数
def date(str):
	return DateTime.Parse(str)

#字符串比较，忽略大小写
def StringEquals(strA, strB):
    return CommonFunction.StringEquals(strA, strB)

#获取单据值
def GetBillFieldValue(pkValue, formId, key):
    return CommonFunction.GetBosVariableValue(ctx, pkValue, formId, key)

#按默单据变量和单据主键变量或取字段名
def GetBillFieldValue(key):
    return CommonFunction.GetBosVariableValue(ctx, key)

