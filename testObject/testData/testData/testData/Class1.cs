using Kingdee.BOS.JSON;
using Kingdee.BOS.Web;
using Kingdee.BOS.WebApi.Client;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testData
{
    public class Class1
    {
        public Class1()
        {
            string s = Aaa("");

        }

        string Aaa(string pJson)
        {
            string objResult = "";
            try
            {
                ApiClient objC = new ApiClient("http://121.37.169.235/K3Cloud");
                Object[] aryPara = new object[] { "62b401e5cc72c9", "张文龙", "kd@123", 2052 };
                var ret = objC.Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser", aryPara);
                string strSuccessFlag = JSONObject.Parse(ret)["LoginResultType"].ToString();
                if (strSuccessFlag == "1")
                {
                    string strResult = objC.Execute<string>("Kingdee.K3.MFG.WebApi.ServicesStub.OptPlanOptTransApiService.OptPlanningPushTrans,Kingdee.K3.MFG.WebApi.ServicesStub", new object[] { pJson });
                    JSONObject objView = JSONObject.Parse(strResult);
                    if (objView["IsSuccess"].ToString() == "true")
                    {
                        MessageBox.Show("成功");
                    }
                    else
                    {
                        MessageBox.Show(objView["IsSuccess"].ToString() + objView["Message"].ToString());
                    }
                }
                else
                {
                    objResult = "OptPlanningPushTrans:" + JSONObject.Parse(ret)["LoginResultType"].ToString();
                }
            }
            catch (Exception ex)
            {

                return "err:" + ex.ToString();
            }

            return "";
        }

    }

    
}
