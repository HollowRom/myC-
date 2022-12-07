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
using Newtonsoft.Json.Linq;

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
                var renum = pushWLQD();
                this.View.ShowMessage("生成单据编号:" + renum);
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

        private string pushGYLX()
        {
            var gylxDicObj = new Dictionary<string, object>();

            gylxDicObj["IsDeleteEntry"] = "true";
            gylxDicObj["IsEntryBatchFill"] = "true";
            gylxDicObj["ValidateFlag"] = "true";
            gylxDicObj["NumberSearch"] = "true";

            var modelDicObj = new Dictionary<string, object>();

            gylxDicObj["Model"] = modelDicObj;

            modelDicObj["FCreateOrgId"] = getKVDis("FNumber", "101.2");
            modelDicObj["FUseOrgId"] = getKVDis("FNumber", "101.2");
            modelDicObj["FName"] = "电容:IND-ML,6.8nH±5%";
            modelDicObj["FProcessType"] = "M";
            
            modelDicObj["FMATERIALID"] = getKVDis("FNumber", "1.02.001.0007.00004");
            modelDicObj["FUnitID"] = getKVDis("FNumber", "Pcs");
            modelDicObj["FEFFECTDATE"] = "2022-12-07 00:00:00";
            modelDicObj["FProDepartmentId"] = getKVDis("FNumber", "BM000004");
            modelDicObj["FBatchTo"] = 9999999.0;
            modelDicObj["FExpireDate"] = "9999-12-31 00:00:00";
            modelDicObj["FCreateDate"] = "2022-12-07 16:10:56";
            modelDicObj["FRouteSrc"] = "E";
            modelDicObj["FProduceType"] = "C";
            modelDicObj["FAutoInStore"] = "C";
            modelDicObj["FFailAutoInStore"] = "A";
            
            modelDicObj["FEntity"] = getGYLXChild();

            var reDic = WebApiServiceCall.Save(cloneCtx, "ENG_BOM", JObject.FromObject(gylxDicObj).ToString()) as Dictionary<string, object>;
            var reStrParse = getErrorMess(reDic);
            if (!reStrParse.Equals(successFlag))
            {
                return "下推失败:" + reStrParse + "\r\n" + "\r\n" + JObject.FromObject(gylxDicObj).ToString();
            }

            return getNumberMess(reDic);
        }

        private List<Dictionary<string, object>> getGYLXChild()
        {
            var FEntityDicObj = new List<Dictionary<string, object>>();

            var childOne = new Dictionary<string, object>();

            return FEntityDicObj;
        }

        private string pushWLQD()
        {
            initCtx();
            var wlqdDicObj = new Dictionary<string, object>();
            wlqdDicObj["IsDeleteEntry"] = "true";
            wlqdDicObj["IsEntryBatchFill"] = "true";
            wlqdDicObj["ValidateFlag"] = "true";
            wlqdDicObj["NumberSearch"] = "true";

            var modelDicObj = new Dictionary<string, object>();
            wlqdDicObj["Model"] = modelDicObj;

            modelDicObj["FCreateOrgId"] = getKVDis("FNumber", "100");
            modelDicObj["FUseOrgId"] = getKVDis("FNumber", "100");
            modelDicObj["FBILLTYPE"] = getKVDis("FNumber", "WLQD01_SYS");

            modelDicObj["FBOMCATEGORY"] = "1";
            modelDicObj["FBOMUSE"] = "99";
            modelDicObj["FYIELDRATE"] = 100.0;

            modelDicObj["FMATERIALID"] = getKVDis("FNumber", "1.02");
            modelDicObj["FUNITID"] = getKVDis("FNumber", "Pcs");
            modelDicObj["FIsValidate"] = false;

            var FTreeEntityDicObj = getChildren();

            modelDicObj["FTreeEntity"] = FTreeEntityDicObj;

            var reDic = WebApiServiceCall.Save(cloneCtx, "ENG_BOM", JObject.FromObject(wlqdDicObj).ToString()) as Dictionary<string, object>;
            var reStrParse = getErrorMess(reDic);
            if (!reStrParse.Equals(successFlag))
            {
                return "下推失败:" + reStrParse + "\r\n" + "\r\n" + JObject.FromObject(wlqdDicObj).ToString();
            }

            return getNumberMess(reDic);
            
        }

        private List<Dictionary<string, object>> getChildren()
        {
            var reList = new List<Dictionary<string, object>>();
            var childOne = new Dictionary<string, object>();

            childOne["FReplaceGroup"] = 1;
            childOne["FMATERIALIDCHILD"] = getKVDis("FNumber", "S01.003.01");
            childOne["FMATERIALTYPE"] = 1;
            childOne["FCHILDUNITID"] = getKVDis("FNumber", "Pcs");
            childOne["FDOSAGETYPE"] = "2";
            childOne["FNUMERATOR"] = 2.0;
            childOne["FDENOMINATOR"] = 1.0;
            childOne["FOverControlMode"] = "1";
            childOne["FEntrySource"] = "1";
            childOne["FEFFECTDATE"] = "2022-12-07 00:00:00";
            childOne["FEXPIREDATE"] = "9999-12-31 00:00:00";
            childOne["FISSUETYPE"] = "1";
            childOne["FISGETSCRAP"] = true;
            childOne["FTIMEUNIT"] = "1";
            childOne["FOPERID"] = 10;
            childOne["FOWNERTYPEID"] = "BD_OwnerOrg";
            childOne["FIsMrpRun"] = true;

            var childTwo = new Dictionary<string, object>();

            childTwo["FReplaceGroup"] = 2;
            childTwo["FMATERIALIDCHILD"] = getKVDis("FNumber", "WL00001");
            childTwo["FMATERIALTYPE"] = 1;
            childTwo["FCHILDUNITID"] = getKVDis("FNumber", "Pcs");
            childTwo["FDOSAGETYPE"] = "2";
            childTwo["FNUMERATOR"] = 3.0;
            childTwo["FDENOMINATOR"] = 1.0;
            childTwo["FOverControlMode"] = "1";
            childTwo["FEntrySource"] = "1";
            childTwo["FEFFECTDATE"] = "2022-12-07 00:00:00";
            childTwo["FEXPIREDATE"] = "9999-12-31 00:00:00";
            childTwo["FISSUETYPE"] = "1";
            childTwo["FISGETSCRAP"] = true;
            childTwo["FTIMEUNIT"] = "1";
            childTwo["FOPERID"] = 10;
            childTwo["FOWNERTYPEID"] = "BD_OwnerOrg";
            childTwo["FIsMrpRun"] = true;

            reList.Add(childOne);
            reList.Add(childTwo);
            return reList;
        }

        private string pushWL()
        {
            initCtx();

            //var parJsonStr = "{ \"Model\": { \"FCreateOrgId\": { \"FNumber\": \"100\" }, \"FUseOrgId\": { \"FNumber\": \"100\" }, \"FName\": \"aabbcc\", \"FSpecification\": \"aabbcc\", \"FImgStorageType\": \"B\", \"SubHeadEntity\": { \"FErpClsID\": \"1\", \"FFeatureItem\": \"1\", \"FCategoryID\": { \"FNumber\": \"CHLB01_SYS\" }, \"FTaxType\": { \"FNumber\": \"WLDSFL01_SYS\" }, \"FTaxRateId\": { \"FNUMBER\": \"SL02_SYS\" }, \"FBaseUnitId\": { \"FNumber\": \"Pcs\" }, \"FIsPurchase\": true, \"FIsInventory\": true, \"FIsSubContract\": false, \"FIsSale\": true, \"FIsProduce\": false, \"F*/IsAsset\": false, \"FWEIGHTUNITID\": { \"FNUMBER\": \"kg\" } } }}";
            
            try
            {
                var numDicObj = new Dictionary<string, object>();
                var ModelDicObj = new Dictionary<string, object>();

                numDicObj["Model"] = ModelDicObj;

                ModelDicObj["FCreateOrgId"] = getKVDis("FNumber", "100");
                
                ModelDicObj["FUseOrgId"] = getKVDis("FNumber", "100");
                ModelDicObj["FName"] = "aabbcc";
                ModelDicObj["FImgStorageType"] = "B";

                var SubHeadEntityDicObj = new Dictionary<string, object>();
                SubHeadEntityDicObj["FErpClsID"] = "1";
                SubHeadEntityDicObj["FFeatureItem"] = "1";
                
                SubHeadEntityDicObj["FCategoryID"] = getKVDis("FNumber", "CHLB01_SYS");

                SubHeadEntityDicObj["FTaxType"] = getKVDis("FNumber", "WLDSFL01_SYS");
                SubHeadEntityDicObj["FTaxRateId"] = getKVDis("FNumber", "SL02_SYS");
                SubHeadEntityDicObj["FBaseUnitId"] = getKVDis("FNumber", "Pcs");

                SubHeadEntityDicObj["FIsPurchase"] = true;
                SubHeadEntityDicObj["FIsInventory"] = true;
                SubHeadEntityDicObj["FIsSubContract"] = true;
                SubHeadEntityDicObj["FIsSale"] = true;
                SubHeadEntityDicObj["FIsProduce"] = true;
                SubHeadEntityDicObj["FIsAsset"] = true;
                SubHeadEntityDicObj["FWEIGHTUNITID"] = getKVDis("FNumber", "kg");

                var reDic = WebApiServiceCall.Save(cloneCtx, "BD_MATERIAL", JObject.FromObject(numDicObj).ToString()) as Dictionary<string, object>;
                var reStrParse = getErrorMess(reDic);
                if (!reStrParse.Equals(successFlag))
                {
                    return "下推失败:" + reStrParse + "\r\n" + "\r\n" + JObject.FromObject(numDicObj).ToString();
                }

                return getNumberMess(reDic);
            }
            catch (Exception e)
            {
                return "下推异常:" + e.ToString() + ":::发送json返回内容:";
            }
            
        }

        private Dictionary<string, string> getKVDis(string key, string value)
        {
            var tempDic = new Dictionary<string, string>();
            tempDic[key] = value;
            return tempDic;
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
                    return "返回json没有错误信息:" + JObject.FromObject(dis).ToString();
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
                if (!result["IsSuccess"].ToString().ToLower().Equals("true"))
                {
                    return JObject.FromObject(dis).ToString();
                }
                var num = (dis["Result"] as Dictionary<string, object>)["Number"] as string;
                if (num == null || num == "")
                {
                    return "返回json没有编码信息";
                }
                return successFlag + "\r\n-->" + /*JObject.FromObject(dis).ToString() + "\r\n-->" +*/ num;
            }
            catch (Exception e)
            {
                return "解析返回值异常:" + e.ToString();
            }
        }
    }
}
