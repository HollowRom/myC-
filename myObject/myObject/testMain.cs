//using System;
//using System.Data;
//using System.Text;
//using Kingdee.BOS.WebApi.Client;
//using Newtonsoft.Json.Linq;

//namespace Kingdee.BOS.WebApi
//{
//    class testMain
//    {
//        static void Main(string[] args)
//        {
//            Sample2016050601();
//        }

//        static string Sample2016050601()
//    {
//        try
//        {
//            K3CloudApiClient client = new K3CloudApiClient("http://121.37.169.235/K3Cloud");
//            // 调用登录接口：                // 参数说明：
//            // // dbid     : 数据中心id。到管理中心数据库搜索：                //
//            // select FDataCenterId, * from T_BAS_DataCenter
//            // // userName : 用户名
//            // // password ：原始密码（未加密）
//            // // loid     : 语言id，中文为2052，中文繁体为3076，英文为1033
//            var loginResult = client.ValidateLogin("62b401e5cc72c9", "administrator", "kd@888888", 2052);
//            var resulttype = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
//            string result = "登录失败，请检查与站点地址、数据中心Id，用户名及密码！";
//            // 登陆成功，开始保存数据
//            if (resulttype == 1)
//            {
//                // 开始构建Web API参数对象
//                JObject jsonRoot = new JObject();
//                // 查询参数需要包括：
//                // // FormId       ：单据标识
//                jsonRoot.Add("FormId", "PUR_PurchaseOrder");
//                // FieldKeys    : 需返回的字段标识，以逗号隔开
//                jsonRoot.Add("FieldKeys",
//                    "FID,FBillNo,FDate,FPOOrderEntry_FEntryID,FMaterialId,FMaterialId.FName,FQty");
//                // FilterString : 过滤条件
//                jsonRoot.Add("FilterString", "");
//                // OrderString  ：排序字段，建议使用单据头字段排序
//                jsonRoot.Add("OrderString", "FBillNo");
//                // StartRow     : 分页取数开始
//                jsonRoot.Add("StartRow", 0);
//                // Limit        : 每页行数
//                jsonRoot.Add("Limit", 50);
//                // TopRowCount  : 查询最大行数
//                jsonRoot.Add("TopRowCount", 2000);
//                // 调用Web API接口服务
//                result = client.Execute<string>(
//                    "Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteBillQuery",
//                    new object[] { jsonRoot.ToString() });
//                // 把结果转换为JObject
//                Newtonsoft.Json.Linq.JObject resultObj = JObject.Parse(result);

//            }

//            return result;

//        }
//        catch (Exception exp)
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.AppendLine("程序运行遇到了未知的错误：");
//            sb.Append("错误提示：").AppendLine(exp.Message);
//            sb.Append("错误堆栈：").AppendLine(exp.StackTrace);
//            return sb.ToString();
//        }

//    }
//    }

    
//}


