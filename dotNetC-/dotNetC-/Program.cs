using Kingdee.BOS.WebApi.Client;
using Kingdee.BOS.JSON;
//using System.Windows.Forms;

internal class Program
{
    private static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        //string objResult = "";
        try
        {
            ApiClient objC = new ApiClient("http://121.37.169.235/K3Cloud/");
            object[] aryPara = new object[] { "62b401e5cc72c9", "张文龙", "kd@123", 2052 };
            var ret = objC.Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser", aryPara);
            //string strSuccessFlag = JSONObject.Parse(ret)[""].ToString();
            if (JSONObject.Parse(ret)["LoginResultType"].ToString() == "1")
            {
                string strResult = objC.Execute<string>("Kingdee.K3.MFG.WebApi.ServicesStub.OptPlanOptTransApiService.OptPlanningPushTrans,Kingdee.K3.MFG.WebApi.ServicesStub", new object[] { "{\n    \"IsAutoAudit\": \"true\",\n    \"Datas\": [\n        {\n            \"TransOutDetailId\": 102434,\n            \"TransInDetailId\": 102435,\n            \"ConvertRules\": 0,\n            \"TransDetailInfo\": [\n                {\n                    \"InEmpId\": 145173,\n                    \"OutEmpId\": 145173,\n                    \"InSupplierId\": 102714,\n                    \"PriceListId\": 0,\n                    \"IsDefaultPriceValue\": 0,\n                    \"SendQualifiedTaxPrice\": 0,\n                    \"SendProFailTaxPrice\": 2,\n                    \"SendMatFailTaxPrice\": 1,\n                    \"SendTaxRate\": 0,\n                    \"OperTransferQty\": 1,\n                    \"OperQualifiedQty\": 0,\n                    \"OperProFailQty\": 1,\n                    \"OperMatFailQty\": 0,\n                    \"OperPreReworkQty\": 0\n                }\n            ]\n        }\n    ]\n}" });

                JSONObject objView = JSONObject.Parse(strResult);
                if (objView["IsSuccess"].ToString() == "true")
                {
                    Console.WriteLine("Hello, World!");
                }
                
                //MessageBox.Show("");
            }
        } catch (Exception e)
        {
            Console.WriteLine("err:" + e.ToString());
        }
    }
}