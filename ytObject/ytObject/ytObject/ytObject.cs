using System;
using System.ComponentModel;
using Kingdee.BOS;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.FormService;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.App.Data;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ytObject
{
    [Description("采购订单审核后事件")]
    [HotUpdate]
    public class cgddshenheshijian : AbstractOperationServicePlugIn
    {
        private const string ylqdFormStr = "SUB_PPBOM";
        private const string wwllFormStr = "SUB_PickMtrl";
        private const string fieldKey = "FBILLNO";
        private const string typeKey = "FBusinessType";
        private const string wwTypeValue = "WW";
        private const string successFlag = "success";
        private const string ruleId = "SUB_PPBOM_Pick";
        private Context cloneCtx = null;

        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            base.OnPreparePropertys(e);
            initCtx();
            e.FieldKeys.Add(fieldKey);
            e.FieldKeys.Add(typeKey);
        }

        public override void EndOperationTransaction(EndOperationTransactionArgs e)
        {
            base.EndOperationTransaction(e);
            var bilt = this.SubBusinessInfo.GetField(typeKey);
            if (bilt == null)
            {
                throw new Exception("未加载单据数据" + typeKey);
            }

            var biln = this.SubBusinessInfo.GetField(fieldKey);
            if (biln == null)
            {
                throw new Exception("未加载单据数据" + fieldKey);
            }

            foreach (var billObj in e.DataEntitys)
            {
                // 读取单据头字段值        
                //var fldBillNoValue = biln.DynamicProperty.GetValue(billObj);
                if (!bilt.DynamicProperty.GetValue(billObj).ToString().ToUpper().Equals(wwTypeValue))
                {
                    //throw new Exception(bilt.DynamicProperty.GetValue(billObj).ToString());
                    continue;
                }
                //throw new Exception(fldBillNoValue.ToString());

                var bs = biln.DynamicProperty.GetValue(billObj).ToString();
                var reStr = pushLLD(bs);
                if (!reStr.Contains(successFlag))
                {
                    throw new Exception(reStr);
                }
                reStr = shtjBill(bs);
                if (!reStr.Contains(successFlag))
                {
                    throw new Exception(reStr);
                }
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

        private string pushLLD(string billno)
        {
            initCtx();
            DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(cloneCtx, "/*dialect*/select distinct a.ylBillNo as " + fieldKey + " from cgdd_wwyl_view a where a.cgBillNo = '" + billno + "'");
            if (Dyobj.Count < 1)
            {
                return "没有符合条件的未领料用料清单";
            }
            for (int idx = 0; idx < Dyobj.Count; idx++)
            {
                var parJsonStr = "{\"Ids\":\"\",\"Numbers\":[\"" + Dyobj[idx][fieldKey].ToString() + "\"],\"EntryIds\":\"\",\"RuleId\":\"" + ruleId + "\",\"TargetBillTypeId\":\"\",\"TargetOrgId\":0,\"TargetFormId\":\"\",\"IsEnableDefaultRule\":\"false\",\"IsDraftWhenSaveFail\":\"false\",\"CustomParams\":{}}";
                Dictionary<string, object> reDic = new Dictionary<string, object>();
                try
                {
                    reDic = WebApiServiceCall.Push(cloneCtx, ylqdFormStr, parJsonStr) as Dictionary<string, object>;
                    var reStrParse = getErrorMess(reDic);
                    if (!reStrParse.Equals(successFlag))
                    {
                        return "下推失败:" + reStrParse;
                    }
                }
                catch (Exception e)
                {
                    return "下推异常:" + e.ToString() + ":::发送json返回内容:" + JObject.FromObject(reDic).ToString();
                }

            }
            return successFlag;

        }

        private string shtjBill(string billno)
        {
            initCtx();
            DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(cloneCtx, "/*dialect*/select distinct a.llBillNo as " + fieldKey +
            " from cgdd_wwll_view a where a.cgBillNo = '" + billno + "'");
            if (Dyobj.Count < 1)
            {
                return "没有符合条件的未领料用料清单";
            }
            for (int idx = 0; idx < Dyobj.Count; idx++)
            {
                var parJsonStr = "{\"Numbers\":[\"" + Dyobj[idx][fieldKey].ToString() + "\"]}";
                Dictionary<string, object> reDic = new Dictionary<string, object>();
                try
                {
                    reDic = WebApiServiceCall.Submit(cloneCtx, wwllFormStr, parJsonStr) as Dictionary<string, object>;
                    var reStrParse = getErrorMess(reDic);
                    if (!reStrParse.Equals(successFlag))
                    {
                        return "提交失败:" + reStrParse;
                    }
                    reDic = WebApiServiceCall.Audit(cloneCtx, wwllFormStr, parJsonStr) as Dictionary<string, object>;
                    //if (!reDic.ContainsKey("Result"))
                    reStrParse = getErrorMess(reDic);
                    if (!reStrParse.Equals(successFlag))
                    {
                        return "审核失败:" + reStrParse;
                    }
                }
                catch (Exception e)
                {
                    return "提交审核异常:" + e.ToString() + ":::发送json返回内容:" + JObject.FromObject(reDic).ToString();
                }
            }
            return successFlag;
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
                    return "返回json没有错误信息:原始异常信息:" + JObject.FromObject(dis).ToString();
                }
                return (reList[0] as Dictionary<string, object>)["Message"].ToString();
            }
            catch (Exception e)
            {
                return "解析返回值异常:" + e.ToString();
            }
        }
    }
}

