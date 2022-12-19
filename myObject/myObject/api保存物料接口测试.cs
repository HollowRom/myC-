using Kingdee.BOS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Kingdee.K3.SCM.Purchase.Business.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS;
using Kingdee.BOS.WebApi.FormService;
using Newtonsoft.Json.Linq;
using Kingdee.BOS.WebApi.Client;
using System.Text.RegularExpressions;
using Kingdee.BOS.DataEntity;
using Kingdee.BOS.Core.DynamicForm.PlugIn.ControlModel;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Core.Const;
using Kingdee.BOS.Cache;

namespace myObject
{
    [Description("api保存物料接口测试")]
    [HotUpdate]
    public class api_save_number : PurchaseBillEdit
    {
        private const string successFlag = "success";
        private Context cloneCtx = null;
        private Regex regex = new Regex("base64,");

        public override void BarItemClick(BarItemClickEventArgs e)
        {
            base.BarItemClick(e);
            if (e.BarItemKey.ToUpperInvariant() == "VBDA_TBBUTTON9")
            {
                initCtx();
                try
                {
                    ClearCache();
                    //if (!ClearCacheByFormIds(cloneCtx, new List<string>(new[] { "BD_MATERIAL" })))
                    //{
                    //    View.ShowMessage("清理缓存失败");
                    //}
                    //else
                    //{
                    //    View.ShowMessage("清理了缓存一次2");
                    //}
                } catch(Exception err)
                {
                    View.ShowMessage(err.ToString());
                }
                //string cloneCtxStr = objectToStr(cloneCtx);
                //cloneCtx = null;
                //cloneCtx = parseStrToObject<Context>(cloneCtxStr);
                //if (cloneCtx == null)
                //{
                //    this.View.ShowMessage("cloneCtx解析失败");
                //    return;
                //}
                //updFileToBill();
                //return;
                //if (Context.ClientType != ClientType.Silverlight && Context.ClientType != ClientType.WPF)
                //{
                //    this.View.ShowMessage("只能在客户端上使用此功能");
                //    return;
                //}
                //var args = new object[2];
                //args[0] = "vxzvd22撒";
                // args[1] = cloneCtx;
                //this.View.GetControl("F_VBDA_CustomCtl").InvokeControlMethod("DoCustomMethod", "sumInt", null);
                //this.View.GetControl("F_VBDA_CustomCtl").InvokeControlMethod("DoCustomMethod", "WriteString", args);

                //this.View.ShowMessage("sumInt = " + sumInt);       
                //StringBuilder sData = new StringBuilder();
                //StringBuilder sPassWord = new StringBuilder();
                //try
                //{
                //    KDGWRFID.WriteCardData(0, sData, 0, sPassWord);
                //    this.View.ShowMessage(sData + "----" + sPassWord);
                //} catch(Exception err)
                //{
                //    this.View.ShowMessage(err.ToString());

                //}

                //var renum = updFileToBill();
                //this.View.ShowMessage("生成单据编号:" + renum);
            }
        }

        private void ClearCache()
        {
            var kcmgr = KCacheManagerFactory.Instance.GetCacheManager("T_BD_MATERIAL", cloneCtx.DBId + "True");
            if (kcmgr != null)
            {
                kcmgr.ClearRegion();
            }
            View.ShowMessage("清理了缓存一次3");
        }

        private bool ClearCacheByFormIds(Context ctx, List<string> formIds)
        {
            if (formIds == null || formIds.Count == 0)
            {
                return false;
            }
            var area = ctx.GetAreaCacheKey();
            if (area == null || area.Equals(""))
            {
                return false;
            }
            foreach (var formId in formIds)
            {
                var metadata = FormMetaDataCache.GetCachedFormMetaData(ctx, formId);
                if (metadata != null)
                {
                    CacheUtil.ClearCache(area, metadata.BusinessInfo.GetEntity(0).TableName);
                    CacheUtil.ClearCache(ctx.DBId + formId, CacheRegionConst.BOS_QuickBaseDataCache);
                }
            }
            return true;
        }

        public override void CustomEvents(CustomEventsArgs e)
        {
            base.CustomEvents(e);
            if (e.Key == "F_VBDA_CustomCtl")
            {
                var recData = e.EventArgs;
                if (e.EventName == "失败")
                {
                    this.View.ShowMessage("自定义组件调用失败：" + e.EventArgs);
                }
                else
                {
                    this.View.ShowMessage("自定义组件调用成功：" + e.EventArgs);
                }
            }
        }

        //private void otherTest()
        //{
        //    var cfg = new KDSerialPortConfig();
        //    cfg.PortName = "COM2";
        //    cfg.Rate = 9600;
        //    cfg.Parity = 0;
        //    cfg.Bits = 8;
        //    cfg.StopBits = 1;
        //    cfg.Timeout = -1;
        //    cfg.EncodingName = "ASCII";
        //    this.View.GetControl<SerialPortControl>("F_VBDA_SerialPortCtrl").Init(cfg);
        //}

        private static string objectToStr(object o)
        {
            return JObject.FromObject(o).ToString();
        }

        private static T parseStrToObject<T>(string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }


        private string updFileToBill()
        {
            //K3CloudApiClient client = new K3CloudApiClient("http://ps2020kbwqdywz/k3cloud/");
            //var loginResult = client.ValidateLogin("610bbd142a6e15", "Administrator", "kingdee@123", 2052);
            //var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();

            initCtx();
            
            var dataDicObj = new Dictionary<string, object>();

            dataDicObj["FileName"] = "0616.txt";
            dataDicObj["FormId"] = "PUR_PurchaseOrder";
            dataDicObj["IsLast"] = "true";
            dataDicObj["InterId"] = "100137";
            dataDicObj["BillNO"] = "CGDD000060";
            dataDicObj["AliasFileName"] = "fuJianTest";
            //dataDicObj["SendByte"] = "56KN5LqL5rOV5biI5ZiO5ZiO6JCo5ZiO";
            dataDicObj["SendByte"] = "data:text/plain;base64,56KN5LqL5rOV5biI5ZiO5ZiO6JCo5ZiO";

            dataDicObj["SendByte"] = regex.Split(dataDicObj["SendByte"].ToString())[1];

            //if (resultType == 1)
            //{
            //    var ret = client.Execute<Dictionary<string, object>>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.AttachmentUpLoad", new Object[]{ JObject.FromObject(dataDicObj).ToString() });
            //    return JObject.FromObject(ret).ToString();
            //}
            
            var reDic = WebApiServiceCall.UploadFile(cloneCtx, JObject.FromObject(dataDicObj).ToString()) as Dictionary<string, object>;
            var reStrParse = getErrorMess(reDic);
            if (!reStrParse.Equals(successFlag))
            {
                return "下推失败:" + reStrParse + "\r\n" + "\r\n" + JObject.FromObject(dataDicObj).ToString();
            }
            string fid;
            try
            {
                fid = getFileId(reDic).Replace(successFlag, "");
                if (fid == null || fid.Equals(""))
                {
                    return "fileId获取失败";
                }
            } catch(Exception e)
            {
                return "fileId获取失败\r\n" + e;
            }
            
            var dataBillDicObj = new Dictionary<string, object>();

            var ModelDicObj = new Dictionary<string, object>();


            ModelDicObj["FFileId"] = fid;
            ModelDicObj["FAttachmentName"] = dataDicObj["FileName"];
            ModelDicObj["FBillType"] = dataDicObj["FormId"];
            ModelDicObj["FInterID"] = dataDicObj["InterId"];
            ModelDicObj["FBillNo"] = dataDicObj["BillNO"];
            ModelDicObj["FAttachmentSize"] = (decimal)(dataDicObj["SendByte"].ToString().Length / (8 / 2 * 1024)) + 1;
            //ModelDicObj["FAttachmentSize"] = 1.0;
            ModelDicObj["FExtName"] = dataDicObj["FileName"].ToString().SubStr(dataDicObj["FileName"].ToString().LastIndexOf("."), 99);
            //ModelDicObj["FExtName"] = ".txt";
            ModelDicObj["FEntryinterId"] = "-1";
            ModelDicObj["FEntrykey"] = " ";
            ModelDicObj["FaliasFileName"] = dataDicObj["AliasFileName"];
            ModelDicObj["FCreateMen"] = getKVDis("FUserID", cloneCtx.UserId.ToString());
            ModelDicObj["FCreateTime"] = DateTime.Now.ToString();

            dataBillDicObj["Model"] = ModelDicObj;
            
            reDic = WebApiServiceCall.Save(cloneCtx, "BOS_Attachment", JObject.FromObject(dataBillDicObj).ToString()) as Dictionary<string, object>;
            reStrParse = getErrorMess(reDic);
            if (!reStrParse.Equals(successFlag))
            {
                return "下推失败:" + reStrParse + "\r\n" + "\r\n" + JObject.FromObject(dataBillDicObj).ToString();
            }
            return successFlag + "\r\n" + JObject.FromObject(reDic).ToString();
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
            initCtx();

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
            modelDicObj["FEFFECTDATE"] = DateTime.Now.ToString();
            modelDicObj["FProDepartmentId"] = getKVDis("FNumber", "BM000004");
            modelDicObj["FBatchTo"] = 9999999.0;
            modelDicObj["FExpireDate"] = "9999-12-31 00:00:00";
            modelDicObj["FCreateDate"] = DateTime.Now.ToString();
            modelDicObj["FRouteSrc"] = "E";
            modelDicObj["FProduceType"] = "C";
            modelDicObj["FAutoInStore"] = "C";
            modelDicObj["FFailAutoInStore"] = "A";
            
            modelDicObj["FEntity"] = getGYLXChild();
            var reDic = WebApiServiceCall.Save(cloneCtx, "ENG_Route", JObject.FromObject(gylxDicObj).ToString()) as Dictionary<string, object>;
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
            var eDicObj = new Dictionary<string, object>();
            FEntityDicObj.Add(eDicObj);

            eDicObj["FSeqName"] = "主序列";
            eDicObj["FSeqType"] = "M";

            var FSubEntityDicObj = new List<Dictionary<string, object>>();
            eDicObj["FSubEntity"] = FSubEntityDicObj;

            var childOne = new Dictionary<string, object>();

            FSubEntityDicObj.Add(childOne);

            childOne["FOperNumber"] = 10;
            childOne["FProOrgId"] = getKVDis("FNUMBER", "101.2");
            childOne["FWorkCenterId"] = getKVDis("FNUMBER", "WC000004");
            childOne["FDepartmentId"] = getKVDis("FNUMBER", "BM000004");
            childOne["FProcessId"] = getKVDis("FNUMBER", "PRC000002");
            childOne["FOperDescription"] = "视觉测试";
            childOne["FOptCtrlCodeId"] = getKVDis("FNUMBER", "OCC000002");
            childOne["FBaseBatch"] = 1.0;
            childOne["FOperUnitID"] = getKVDis("FNumber", "Pcs");
            
            var childTwo = new Dictionary<string, object>();

            FSubEntityDicObj.Add(childTwo);

            childTwo["FOperNumber"] = 20;
            childTwo["FProOrgId"] = getKVDis("FNUMBER", "101.2");
            childTwo["FWorkCenterId"] = getKVDis("FNUMBER", "WC000006");
            childTwo["FDepartmentId"] = getKVDis("FNUMBER", "BM000005");
            childTwo["FProcessId"] = getKVDis("FNUMBER", "PRC000004");
            childTwo["FOperDescription"] = "软件下载";
            childTwo["FOptCtrlCodeId"] = getKVDis("FNUMBER", "OCC000001_SYS");
            childTwo["FBaseBatch"] = 1.0;
            childTwo["FOperUnitID"] = getKVDis("FNumber", "Pcs");
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

        //获取文件id
        private string getFileId(Dictionary<string, object> dis)
        {
            try
            {
                var result = (dis["Result"] as Dictionary<string, object>)["ResponseStatus"] as Dictionary<string, object>;
                if (!result["IsSuccess"].ToString().ToLower().Equals("true"))
                {
                    return JObject.FromObject(dis).ToString();
                }
                var num = (dis["Result"] as Dictionary<string, object>)["FileId"] as string;
                if (num == null || num == "")
                {
                    return "返回json没有编码信息";
                }
                return successFlag + /*JObject.FromObject(dis).ToString() + "\r\n-->" +*/ num;
            }
            catch (Exception e)
            {
                return "解析返回值异常:" + e.ToString();
            }
        }
    }


    //public static class KDGWRFID
    //{
    //    [DllImport("doNothingObject.dll")]
    //    public static extern int Sum(int a, int b);
        
    //}


    //public static class KDGWRFID
    //{
    //    [DllImport("noThingObject.dll")]
    //    public static extern bool LinkCard();

    //    //初试化        
    //    [DllImport("noThingObject.dll")]
    //    public static extern bool UnlinkCard();  //初始化       

    //    [DllImport("noThingObject.dll")]
    //    public static extern int ReadCardID(StringBuilder sCardId);  //初始化    

    //    [DllImport("noThingObject.dll")]
    //    public static extern bool WriteCardData(int nBlock, StringBuilder sData, int sPassType, StringBuilder sPassWord);

    //    [DllImport("noThingObject.dll")]
    //    public static extern bool ReadCardData(StringBuilder sData, int nBlock, int sPassType, StringBuilder sPassWord);

    //    [DllImport("noThingObject.dll")]
    //    public static extern void GetErr(StringBuilder ErrStr);

    //    public static string ReadCardID()
    //    {
    //        string retStr = "";
    //        var retb = LinkCard();
    //        if (!retb) return retStr;
    //        var sb = new StringBuilder();
    //        var sbRet = new StringBuilder();
    //        var ret = ReadCardID(sb);
    //        if (ret == 0)
    //        {
    //            GetErr(sbRet);
    //        }
    //        else
    //        {
    //            var chrs = Encoding.Default.GetBytes(sb.ToString().Substring(0, ret));
    //            for (int i = 0; i < chrs.Length; i++)
    //            {
    //                var chr = chrs;
    //                var chrX2 = chr.ToString();
    //                //var chrX2 = chr.ToString("X2");
    //                sbRet.Append(chrX2);
    //            }
    //        }
    //        retStr = sbRet.ToString();
    //        retb = UnlinkCard();
    //        return retStr;
    //    }
    //}
}
