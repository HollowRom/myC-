using Kingdee.BOS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Kingdee.K3.SCM.Purchase.Business.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.WebApi.FormService;

namespace myObject
{
    [Description("api保存物料接口测试"), HotUpdate]
    public class api_save_number : PurchaseBillEdit
    {
        private const string successFlag = "success";
        private Context cloneCtx = null;
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            base.BarItemClick(e);
            if (e.BarItemKey.ToUpperInvariant() == "VBDA_TBBUTTON9")
            {
                var renum = pushWL();
                this.View.ShowMessage("生成编码:" + renum);
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

        private string pushWL()
        {
            initCtx();

            var parJsonStr = "{ \"Model\": { \"FCreateOrgId\": { \"FNumber\": \"100\" }, \"FUseOrgId\": { \"FNumber\": \"100\" }, \"FName\": \"aabbcc\", \"FSpecification\": \"aabbcc\", \"FImgStorageType\": \"B\", \"SubHeadEntity\": { \"FErpClsID\": \"1\", \"FFeatureItem\": \"1\", \"FCategoryID\": { \"FNumber\": \"CHLB01_SYS\" }, \"FTaxType\": { \"FNumber\": \"WLDSFL01_SYS\" }, \"FTaxRateId\": { \"FNUMBER\": \"SL02_SYS\" }, \"FBaseUnitId\": { \"FNumber\": \"Pcs\" }, \"FIsPurchase\": true, \"FIsInventory\": true, \"FIsSubContract\": false, \"FIsSale\": true, \"FIsProduce\": false, \"FIsAsset\": false, \"FWEIGHTUNITID\": { \"FNUMBER\": \"kg\" } }, }}";

            //var parJsonStr = "{ \"FCreateOrgId\": { \"FNumber\": \"100\" }, \"FUseOrgId\": { \"FNumber\": \"100\" }, \"FName\": \"aabbcc\", \"FSpecification\": \"aabbcc\", \"FImgStorageType\": \"B\", \"SubHeadEntity\": { \"FErpClsID\": \"1\", \"FFeatureItem\": \"1\", \"FCategoryID\": { \"FNumber\": \"CHLB01_SYS\" }, \"FTaxType\": { \"FNumber\": \"WLDSFL01_SYS\" }, \"FTaxRateId\": { \"FNUMBER\": \"SL02_SYS\" }, \"FBaseUnitId\": { \"FNumber\": \"Pcs\" }, \"FIsPurchase\": true, \"FIsInventory\": true, \"FIsSubContract\": false, \"FIsSale\": true, \"FIsProduce\": false, \"FIsAsset\": false, \"FWEIGHTUNITID\": { \"FNUMBER\": \"kg\" } } }";
            try
            {
                var reDic = WebApiServiceCall.Save(cloneCtx, "BD_MATERIAL", parJsonStr) as Dictionary<string, object>;
                var reStrParse = getErrorMess(reDic);
                if (!reStrParse.Equals(successFlag))
                {
                    return "下推失败:" + reStrParse;
                }
                return getNumberMess(reDic);
            }
            catch (Exception e)
            {
                return "下推异常:" + e.ToString() + ":::发送json返回内容:" + parJsonStr;
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
                //return result["ErrorCode"].ToString().ToLower();
                var reList = result["Errors"] as List<Dictionary<string, object>>;
                if (reList == null || reList.Count < 1)
                {
                    return "返回json没有错误信息1";
                }
                return (reList[0] as Dictionary<string, object>)["Message"].ToString();
            }
            catch (Exception e)
            {
                return "解析返回值异常:" + e.ToString();
            }
        }

        //需要原始的返回值
        private string getNumberMess(Dictionary<string, object> dis)
        {
            try
            {
                var result = (dis["Result"] as Dictionary<string, object>)["ResponseStatus"] as Dictionary<string, object>;
                if (result["IsSuccess"].ToString().ToLower().Equals("true"))
                {
                    return successFlag;
                }
                var num = (dis["Result"] as Dictionary<string, object>)["Number"] as string;
                if (num == null || num == "")
                {
                    return "返回json没有编码信息";
                }
                return num;
            }
            catch (Exception e)
            {
                return "解析返回值异常:" + e.ToString();
            }
        }
    }
}
