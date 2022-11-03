using System;
using System.ComponentModel;
using Kingdee.BOS;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Validation;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.FormService;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.JSON;
using Kingdee.BOS.WebApi.Client;
using System.Collections.Generic;

[Description("采购订单审核后事件")]
[HotUpdate]
public class shenheshijian : AbstractOperationServicePlugIn
{
    private const string fieldKey = "FBILLNO";
    private const string successFlag = "success";
    private const string ruleId = "SUB_PPBOM_Pick";
    private Context cloneCtx;

    public override void OnPreparePropertys(PreparePropertysEventArgs e)
    {
        base.OnPreparePropertys(e);
        initCtx();
        e.FieldKeys.Add(fieldKey);
    }

    public override void EndOperationTransaction(EndOperationTransactionArgs e)
    {
        base.EndOperationTransaction(e);
        var biln = this.SubBusinessInfo.GetField(fieldKey);
        if (biln == null)
        {
            throw new Exception("未加载billno");
        }

        foreach (var billObj in e.DataEntitys)
        {        // 读取单据头字段值        
            var fldBillNoValue = biln.DynamicProperty.GetValue(billObj);
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
        //第一种方式：通过客户端调用接口，先调用登陆接口，再调用其他接口，
        //K3CloudApiClient cloneCtx = new K3CloudApiClient("http://localhost/");
        //var isSucc = cloneCtx.Login("610bbd142a6e15", "demo", "kingdee@123", 2052);
        //var isSucc = cloneCtx.Login(this.Context.DBId, this.Context.UserName, "888888", 2052);
        //apiClient.Save(ctx, "KKK_BillA", "{\"Model\": {\"FBillTypeID\": {\"FNUMBER\": \"FBillANumber2\"}}");

        //第二种方式：通过克隆创建一个新的上下文，再使用克隆的上下文直接调用api接口，
        //var cloneCtx = ObjectUtils.CreateCopy(Context) as Context;
        //cloneCtx.ServiceType = WebType.WebService;//写死    
        //cloneCtx.ClientInfo = Context.ClientInfo;
        //cloneCtx.CharacterSet = Context.CharacterSet;
        //// cloneCtx.IsStartTimeZoneTransfer = ctx.IsStartTimeZoneTransfer;  
        //cloneCtx.LoginName = Context.LoginName;
        //cloneCtx.EntryRole = Context.EntryRole;
        //// cloneCtx.Salt = ctx.Salt;      
        //cloneCtx.UserPhone = Context.UserPhone;
        //cloneCtx.UserEmail = Context.UserEmail;
        //cloneCtx.UserLoginType = Context.UserLoginType;

        DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(cloneCtx, "/*dialect*/select distinct a.ylBillNo as FBILLNO " +
        " from cgdd_wwyl_view a where a.cgBillNo = '" + billno + "'");
        if (Dyobj.Count < 1)
        {
            return "没有符合条件的未领料用料清单";
        }
        for (int idx = 0; idx < Dyobj.Count; idx++)
        {
            var parJsonStr = "{\"Ids\":\"\",\"Numbers\":[\"" + Dyobj[idx]["FBILLNO"].ToString() + "\"],\"EntryIds\":\"\",\"RuleId\":\"" + ruleId + "\",\"TargetBillTypeId\":\"\",\"TargetOrgId\":0,\"TargetFormId\":\"\",\"IsEnableDefaultRule\":\"false\",\"IsDraftWhenSaveFail\":\"false\",\"CustomParams\":{}}";
            try
            {
                var reDic = WebApiServiceCall.Push(cloneCtx, "SUB_PPBOM", parJsonStr) as Dictionary<string, object>;
                if (!reDic.ContainsKey("Result"))
                {
                    return "下推失败:返回结果不存在Result字段";
                }
                var result = reDic["Result"] as Dictionary<string, object>;
                //parJsonStr = result.ToString();
                if (!(result["ResponseStatus"] as Dictionary<string, object>)["IsSuccess"].ToString().ToLower().Equals("true"))
                {
                    return "下推失败:" + parJsonStr;
                }
            }
            catch (Exception e)
            {
                return "下推异常:" + e.ToString() + ":::发送json返回内容:" + parJsonStr;
            }

            //var llbill = result.GetJSONObject("ResponseStatus").GetJSONObject("SuccessEntitys");
        }
        return successFlag;

    }

    private string shtjBill(string billno)
    {
        //var cloneCtx = ObjectUtils.CreateCopy(Context) as Context;
        //cloneCtx.ServiceType = WebType.WebService;//写死    
        //cloneCtx.ClientInfo = Context.ClientInfo;
        //cloneCtx.CharacterSet = Context.CharacterSet;
        //// cloneCtx.IsStartTimeZoneTransfer = ctx.IsStartTimeZoneTransfer;  
        //cloneCtx.LoginName = Context.LoginName;
        //cloneCtx.EntryRole = Context.EntryRole;
        //// cloneCtx.Salt = ctx.Salt;      
        //cloneCtx.UserPhone = Context.UserPhone;
        //cloneCtx.UserEmail = Context.UserEmail;
        //cloneCtx.UserLoginType = Context.UserLoginType;

        DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(cloneCtx, "/*dialect*/select distinct a.llBillNo as FBILLNO " +
        " from cgdd_wwll_view a where a.cgBillNo = '" + billno + "'");
        if (Dyobj.Count < 1)
        {
            return "没有符合条件的未领料用料清单";
        }
        for (int idx = 0; idx < Dyobj.Count; idx++)
        {
            var parJsonStr = "{\"Numbers\":[\"" + Dyobj[idx]["FBILLNO"].ToString() + "\"]}";
            try
            {
                var reDic = WebApiServiceCall.Submit(cloneCtx, "SUB_PickMtrl", parJsonStr) as Dictionary<string, object>;
                if (!reDic.ContainsKey("Result"))
                {
                    return "提交失败:返回结果不存在Result字段";
                }
                var result = reDic["Result"] as Dictionary<string, object>;
                if (!(result["ResponseStatus"] as Dictionary<string, object>)["IsSuccess"].ToString().ToLower().Equals("true"))
                {
                    
                    return "提交失败:" + ((result["ResponseStatus"] as Dictionary<string, object>)["Errors"] as List<Dictionary<string, object>>)[0]["Message"].ToString();//(result["ResponseStatus"] as Dictionary<string, object>)["IsSuccess"].ToString();
                }
                reDic = WebApiServiceCall.Audit(cloneCtx, "SUB_PickMtrl", parJsonStr) as Dictionary<string, object>;
                if (!reDic.ContainsKey("Result"))
                {
                    return "审核失败:返回结果不存在Result字段";
                }
                result = reDic["Result"] as Dictionary<string, object>;
                if (!(result["ResponseStatus"] as Dictionary<string, object>)["IsSuccess"].ToString().ToLower().Equals("true"))
                {
                    return "审核失败:" + parJsonStr;
                }
            }
            catch (Exception e)
            {
                return "提交审核异常:" + e.ToString() + ":::发送json返回内容:" + parJsonStr;
            }
        }
        return successFlag;
    }
}


