using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.FormService;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace hqObject
{
    [Description("销售订单审核自动生成出库单"), HotUpdate]
    public class xsdd_sh_ck : AbstractOperationServicePlugIn
    {

        private const string xsddFormStr = "SAL_SaleOrder";
        private const string xsckFormStr = "SAL_OUTSTOCK";
        private const string fieldKey = "FBILLNO";
        private const string seqKey = "FSEQS";
        private const string entryKey = "FENTRYS";
        private const string typeKey = "FBillTypeID"; //FNumber = "XSDD01_SYS",F_VBDA_CheckBox=1
        private const string flagKey = "F_VBDA_CheckBox";
        private const string kcxsTypeValue = "XSDD01_SYS";
        private const string successFlag = "success";
        private const string ruleId = "SaleOrder-OutStock";
        private Context cloneCtx = null;

        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            base.OnPreparePropertys(e);
            initCtx();
            e.FieldKeys.Add(fieldKey);
            e.FieldKeys.Add(typeKey);
            e.FieldKeys.Add(flagKey);
        }

        public override void EndOperationTransaction(EndOperationTransactionArgs e)
        {
            base.EndOperationTransaction(e);
            var bilt = this.SubBusinessInfo.GetField(typeKey);
            if (bilt == null)
            {
                throw new Exception("未加载单据类型数据" + typeKey);
            }

            var biln = this.SubBusinessInfo.GetField(fieldKey);
            if (biln == null)
            {
                throw new Exception("未加载单据编号数据" + fieldKey);
            }

            var bilf = this.SubBusinessInfo.GetField(flagKey);
            if (biln == null)
            {
                throw new Exception("未加载单据出库标记数据" + flagKey);
            }
            foreach (var billObj in e.DataEntitys)
            {
                // 读取单据头字段值        
                //var fldBillNoValue = biln.DynamicProperty.GetValue(billObj);
                if (!(((DynamicObject)bilt.DynamicProperty.GetValue(billObj))["Number"].ToString().ToUpper().Equals(kcxsTypeValue) && bilf.DynamicProperty.GetValue(billObj).ToString().ToLower().Equals("true")))
                {
                    //throw new Exception("当前单据标记状态:" + ((DynamicObject)bilt.DynamicProperty.GetValue(billObj))["Number"].ToString().ToUpper() + "---" + bilf.DynamicProperty.GetValue(billObj).ToString());
                    continue;
                }
                //throw new Exception(fldBillNoValue.ToString());

                var bs = biln.DynamicProperty.GetValue(billObj).ToString();
                var reStr = pushCKD(bs);
                if (!reStr.Contains(successFlag))
                {
                    throw new Exception(reStr);
                }

                //reStr = shtjBill(bs);
                //if (!reStr.Contains(successFlag))
                //{
                //    throw new Exception(reStr);
                //}
            }
        }

        private void initCtx()
        {
            if (cloneCtx != null)
            {
                return;
            }
            cloneCtx = ObjectUtils.CreateCopy(Context) as Context;
            cloneCtx.ServiceType = WebType.WebService;//写死    
            cloneCtx.ClientInfo = Context.ClientInfo;
            cloneCtx.CharacterSet = Context.CharacterSet;
            // cloneCtx.IsStartTimeZoneTransfer = ctx.IsStartTimeZoneTransfer;  
            cloneCtx.LoginName = Context.LoginName;
            cloneCtx.EntryRole = Context.EntryRole;
            // cloneCtx.Salt = ctx.Salt;      
            cloneCtx.UserPhone = Context.UserPhone;
            cloneCtx.UserEmail = Context.UserEmail;
            cloneCtx.UserLoginType = Context.UserLoginType;
        }

        private string pushCKD(string billno)
        {
            initCtx();
            DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(cloneCtx, "/*dialect*/select distinct a.FENTRYS as " + entryKey + ", FSEQS as " + seqKey + " from xsdd_send_xsck_view a where a.FBILLNO = '" + billno + "'");
            if (Dyobj.Count < 1)
            {
                return successFlag + ":没有足够库存的销售订单行";
            }
            var parJsonStr = "{\"Ids\":\"\",\"Numbers\":[\"\"],\"EntryIds\":\"" + Dyobj[0][entryKey].ToString().Substring(0, Dyobj[0][entryKey].ToString().Length - 1) + "\",\"RuleId\":\"" + ruleId + "\",\"TargetBillTypeId\":\"\",\"TargetOrgId\":0,\"TargetFormId\":\"\",\"IsEnableDefaultRule\":\"false\",\"IsDraftWhenSaveFail\":\"false\",\"CustomParams\":{}}";
            var successSEQ = "";
            try
            {
                var reDic = WebApiServiceCall.Push(cloneCtx, xsddFormStr, parJsonStr) as Dictionary<string, object>;
                var reStrParse = getErrorMess(reDic);
                if (!reStrParse.Equals(successFlag))
                {
                    return "下推失败:" + reStrParse;
                }
                var shDis = shtjBill(billno);
                if (!shDis.Equals(successFlag))
                {
                    return shDis;
                }
                successSEQ = successSEQ + Dyobj[0][seqKey].ToString().Substring(0, Dyobj[0][seqKey].ToString().Length - 1) + ",";
            }
            catch (Exception e)
            {
                return "下推异常:" + e.ToString() + ":::发送json返回内容:" + parJsonStr;
            }
            return successFlag + "\r\n成功下推行号:" + successSEQ;

        }

        //需要原始的返回值
        private string shtjBill(string billno)
        {
            initCtx();
            DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(cloneCtx, "/*dialect*/select distinct a.FBILLNO, c.FSOORDERNO from T_SAL_OUTSTOCK a,T_SAL_OUTSTOCKENTRY b, T_SAL_OUTSTOCKENTRY_R c where a.FID = b.FID and b.FENTRYID = c.FENTRYID and c.FSOORDERNO = '" + billno + "'");
            if (Dyobj.Count < 1)
            {
                return "生成销售出库单失败";
            }

            var parJsonStr = "";
            var addFlag = "0";
            try
            {
                var bills = "";
                for (int idx = 0; idx < Dyobj.Count; idx++)
                {
                    bills = bills + Dyobj[idx]["FBILLNO"].ToString() + ",";

                }
                bills = bills.Substring(0, bills.Length - 1);
                parJsonStr = "{\"Numbers\":[\"" + bills + "\"]}";
                var reDic = WebApiServiceCall.Submit(cloneCtx, xsckFormStr, parJsonStr) as Dictionary<string, object>;
                var reStrParse = getErrorMess(reDic);
                //addFlag = "2";
                if (!reStrParse.Equals(successFlag))
                {
                    return "提交失败:" + reStrParse;
                }
                //addFlag = "3";
                reDic = WebApiServiceCall.Audit(cloneCtx, xsckFormStr, parJsonStr) as Dictionary<string, object>;
                reStrParse = getErrorMess(reDic);
                //addFlag = "4";
                if (!reStrParse.Equals(successFlag))
                {
                    return "审核失败:" + reStrParse;
                }
                //addFlag = "5";
                return successFlag;
            }
            catch (Exception e)
            {
                return "提交审核异常:" + e.ToString() + "---flag:" + addFlag + ":::发送json返回内容:" + parJsonStr;
            }
        }

        //需要原始的返回值
        private string getErrorMess(Dictionary<string, object> dis)
        {
            try
            {
                var result = (dis["Result"] as Dictionary<string, object>)["ResponseStatus"] as Dictionary<string, object>;
                if (result["IsSuccess"].ToString().ToLower().Equals("true"))
                {
                    return successFlag;
                }
                var reList = result["Errors"] as List<Dictionary<string, object>>;
                if (reList == null || reList.Count < 1)
                {
                    return "返回json没有错误信息";
                }
                return (reList[0] as Dictionary<string, object>)["Message"].ToString();
            }
            catch (Exception e)
            {
                return "解析返回值异常:" + e.ToString();
            }
        }

        private string getDisKeys(Dictionary<string, object> result)
        {
            var keys = result.Keys;
            var reKey = "";
            foreach (var k in keys)
            {
                reKey = reKey + k.ToString() + ",";
            }
            return reKey;
        }
    }
}
