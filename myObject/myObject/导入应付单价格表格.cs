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
using Kingdee.BOS.Core.Bill.PlugIn;
using System.Data;
using Kingdee.BOS.ServiceHelper.Excel;
using Kingdee.BOS.JSON;
using Newtonsoft.Json.Linq;

namespace myObject
{
    [Description("导入应付单价格表格"), HotUpdate]
    public class input_yfd_price : AbstractBillPlugIn
    {
        private const string successFlag = "success";
        private Context cloneCtx = null;

        private const string sheetName = "Sheet1";

        private List<excelInfo> excelInfos = new List<excelInfo>();

        private DataTable dtexcel;

        private const string updField = "fnote";//更新的字段名
        private const string btnName = "FFileUpdate"; //触发的上传控件的标识

        public override void CustomEvents(CustomEventsArgs e)
        {
            if (e.Key.EqualsIgnoreCase(btnName) && e.EventName.EqualsIgnoreCase("FILECHANGED")) //上传附件的按钮标识
            {
                JSONObject jSONObject = KDObjectConverter.DeserializeObject<JSONObject>(e.EventArgs);
                JSONArray jSONArray = null;
                string text = null;
                if (jSONObject != null)
                {
                    jSONArray = new JSONArray(jSONObject["NewValue"].ToString());
                    if (jSONArray.Count > 0)
                    {
                        text = (jSONArray[0] as Dictionary<string, object>)["ServerFileName"].ToString();
                        if (this.CheckFile(text))
                        {
                            Impoort(PathUtils.GetPhysicalPath("FileUpLoadServices\\UploadFiles", text));
                        }
                    }
                }
            }
            base.CustomEvents(e);
        }

        private struct excelInfo
        {
            public string orgNumber;
            public int orgId;
            public string suppNumber;
            public int suppId;
            public string FNumber;
            public int numberId;
            public string price;
            public float priceF;
        }
        
        //自定义方法,引入的数据
        private void Impoort(string filePath)
        {
            ExcelOperation excelOperation = new ExcelOperation();
            DataSet dataSet = null;
            dataSet = excelOperation.ReadFromFile(filePath, 0, 0);
            dtexcel = dataSet.Tables[sheetName];//读取的页名

            if (dtexcel.Rows.Count == 0)
            {
                this.View.ShowWarnningMessage("获取表格数据失败,请确认是否存在标签页[" + sheetName + "]");
                return;
            }

            //i=1第二行开始读取:第一列订单号,第二列行号,第三列装箱单号,第一行列名不能重复
            for (int i = 1; i < dtexcel.Rows.Count; i++)
            {
                try
                {
                    excelInfo temp = new excelInfo();
                    temp.orgNumber = dtexcel.Rows[i][0].ToString();
                    temp.suppNumber = dtexcel.Rows[i][1].ToString();
                    temp.FNumber = dtexcel.Rows[i][3].ToString();
                    temp.price = dtexcel.Rows[i][4].ToString();
                    if (temp.orgNumber == "" || temp.suppNumber == "" || temp.FNumber == "" || temp.price == "")
                    {
                        this.View.ShowWarnningMessage("解析表格数据失败,第" + i.ToString() + "行数据存在空值");
                        return;
                    }
                    temp.orgId = getOrgId(temp.orgNumber);
                    if (temp.orgId == 0)
                    {
                        this.View.ShowWarnningMessage("解析表格数据失败,第" + i.ToString() + "行组织不存在,请确认是否输入组织编码");
                        return;
                    }
                    temp.numberId = getNumberId(temp.FNumber + "|" + temp.orgNumber);
                    if (temp.numberId == 0)
                    {
                        this.View.ShowWarnningMessage("解析表格数据失败,第" + i.ToString() + "行编码不存在,请确认是否输入物料编码");
                        return;
                    }
                    temp.suppId = getSuppId(temp.suppNumber + "|" + temp.orgNumber);
                    if (temp.suppId == 0)
                    {
                        this.View.ShowWarnningMessage("解析表格数据失败,第" + i.ToString() + "行供应商不存在,请确认是否输入供应商编码");
                        return;
                    }
                    excelInfos.Add(temp);
                } catch(Exception e)
                {
                    this.View.ShowWarnningMessage("解析表格数据失败,第" + i.ToString() + "行数据存在异常," + e.ToString());
                    return;
                }

                //if (dtexcel.Rows[i][0].ToString() != "" && dtexcel.Rows[i][1].ToString() != "" && dtexcel.Rows[i][2].ToString() != "")
                //{
                //    DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/update b set " + updField + /*fnote =*/ "= '" + dtexcel.Rows[i][2].ToString() + "' from T_SAL_ORDER a, T_SAL_ORDERENTRY b where a.FID = b.FID and a.FDOCUMENTSTATUS = 'C' and a.FBILLNO = '" + dtexcel.Rows[i][0].ToString() + "' and b.FSEQ = " + dtexcel.Rows[i][1].ToString() + " and isnull(b." + updField + ",'') <> '" + dtexcel.Rows[i][2].ToString() + "'");
                //}
            }
            initCtx();
            for (int i = 0; i < excelInfos.Count; i++)
            {
                try
                {
                    string sqlStr = string.Format("/*dialect*/select a.FID, a.FENTRYID, a.FPRICE, a.FTAXPRICE, a.FISTAX from v_zw_yfd_change_price a where a.FSETTLEORGID = {0} and a.FSUPPLIERID = {1} and a.FMATERIALID = {2}", excelInfos[i].orgId, excelInfos[i].suppId, excelInfos[i].numberId);
                    DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, sqlStr);
                    if (Dyobj.Count < 1)
                    {
                        continue;
                    }
                    
                    for (int i2 = 0; i2 < Dyobj.Count; i2++)
                    {
                        string updPriceFieldName = Dyobj[i2]["FISTAX"].ToString().Equals("1") ? "FTAXPRICE" : "FPRICE";

                        var dataBillDicObj = new Dictionary<string, object>();

                        dataBillDicObj["NeedUpDateFields"] = new List<string> { "FEntityDetail", updPriceFieldName };
                        dataBillDicObj["IsDeleteEntry"] = "false";

                        var ModelDicObj = new Dictionary<string, object>();

                        dataBillDicObj["Model"] = ModelDicObj;

                        ModelDicObj["FID"] = Dyobj[i2]["FID"];

                        var entryObject = new List<Dictionary<string, object>>();

                        ModelDicObj["FEntityDetail"] = entryObject;

                        var entry = new Dictionary<string, object>();

                        entryObject.Add(entry);

                        entry["FEntryID"] = Dyobj[i2]["FENTRYID"];
                        entry[updPriceFieldName] = excelInfos[i].priceF;
                        
                        Dictionary<string, object> reDic = WebApiServiceCall.Save(cloneCtx, "AP_PAYABLE", JObject.FromObject(dataBillDicObj).ToString()) as Dictionary<string, object>;
                        string reStrParse = getErrorMess(reDic);
                        if (!reStrParse.Equals(successFlag))
                        {
                            this.View.ShowWarnningMessage("下推失败:" + reStrParse + "\r\n" + "\r\n" + JObject.FromObject(dataBillDicObj).ToString());
                            return;
                        }
                    }
                }
                catch (Exception e)
                {
                    this.View.ShowWarnningMessage("执行发生异常:第" + (i + 1).ToString() + "发生异常:" + e.ToString());
                    return;
                }
            }
            this.View.ShowMessage("执行完成");
        }

        private static Dictionary<string, int> orgIdMap = new Dictionary<string, int>();

        private int getOrgId(string orgNumber)
        {
            if (orgIdMap.ContainsKey(orgNumber))
            {
                return orgIdMap[orgNumber];
            }
            DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select top 1 FORGID from T_ORG_Organizations where FNUMBER = '" + orgNumber + "'");
            if (Dyobj.Count < 1)
            {
                orgIdMap[orgNumber] = 0;
            } else
            {
                orgIdMap[orgNumber] = Convert.ToInt32(Dyobj[0]["FORGID"]);
            }
            return orgIdMap[orgNumber];
        }

        private static Dictionary<string, int> numberIdMap = new Dictionary<string, int>();

        private int getNumberId(string NumberAndOrgNumber)
        {
            if (orgIdMap.ContainsKey(NumberAndOrgNumber))
            {
                return orgIdMap[NumberAndOrgNumber];
            }
            string[] l = NumberAndOrgNumber.Split('|');
            if (l.Length != 2)
            {
                orgIdMap[NumberAndOrgNumber] = 0;
                return 0;
            }
            DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select top 1 a.FMATERIALID from T_BD_MATERIAL a, T_ORG_Organizations b where a.FUSEORGID = b.FORGID and a.FNUMBER = '" + l[0] + "' and b.FNUMBER = '" + l[1] + "'");
            if (Dyobj.Count < 1)
            {
                orgIdMap[NumberAndOrgNumber] = 0;
            }
            else
            {
                orgIdMap[NumberAndOrgNumber] = Convert.ToInt32(Dyobj[0]["FMATERIALID"]);
            }
            return orgIdMap[NumberAndOrgNumber];
        }

        private static Dictionary<string, int> suppIdMap = new Dictionary<string, int>();

        private int getSuppId(string NumberAndOrgNumber)
        {
            if (orgIdMap.ContainsKey(NumberAndOrgNumber))
            {
                return orgIdMap[NumberAndOrgNumber];
            }
            string[] l = NumberAndOrgNumber.Split('|');
            if (l.Length != 2)
            {
                orgIdMap[NumberAndOrgNumber] = 0;
                return 0;
            }
            DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select top 1 a.FSUPPLIERID from T_BD_SUPPLIER a, T_ORG_Organizations b where a.FUSEORGID = b.FORGID and a.FNUMBER = '" + l[0] + "' and b.FNUMBER = '" + l[1] + "'");
            if (Dyobj.Count < 1)
            {
                orgIdMap[NumberAndOrgNumber] = 0;
            }
            else
            {
                orgIdMap[NumberAndOrgNumber] = Convert.ToInt32(Dyobj[0]["FSUPPLIERID"]);
            }
            return orgIdMap[NumberAndOrgNumber];
        }
        //自定义方法,判断是否是上传的是Excel文件
        private bool CheckFile(string fileName)
        {
            if (!fileName.Contains(".xlsx") && !fileName.Contains(".xlx"))
            {
                this.View.ShowWarnningMessage("请选择正确的文件进行引入,只支持xlsx/xlx格式文件");
                return false;
            }
            return true;
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
                    return "返回json没有编码信息:" + JObject.FromObject(dis).ToString();
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
