using System;

using System.Net;
using Kingdee.BOS.WebApi.Client;
using Kingdee.BOS.JSON;
using System.IO;

namespace XkPdaJumpServer
{
    class XkPdaJumpServer
    {
        const string LogUrl = "http://121.37.169.235/K3Cloud/";
        const string LoginApi = "Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser";
        static readonly object[] loginInfo = { "62b401e5cc72c9", "张文龙", "kd@123", 2052 };
        
        static void Main(string[] args)
        {
            using (HttpListener listerner = new HttpListener())
            {
                
                listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous; //指定身份验证 Anonymous匿名访问
                listerner.Prefixes.Add("http://127.0.0.1:8080/jumpApi/");

                // listerner.Prefixes.Add("http://localhost/web/");
                listerner.Start();
                Console.WriteLine("WebServer Start Successed.......");

                // ApiClient objC2 = GetApiClient(LogUrl, LoginApi, loginInfo);
                //
                // return;
                int maxTime = 1;
               while (maxTime > 0)
                {
                    // maxTime--;
                    // Console.WriteLine("maxTime:" + maxTime);
                    //等待请求连接
                    //没有请求则GetContext处于阻塞状态
                   HttpListenerContext ctx = listerner.GetContext();

                   ctx.Response.StatusCode = 404; //设置返回给客服端http状态代码 

                   JSONObject reMes = new JSONObject();

                   try
                   {
                        if (ctx.Request.HttpMethod.ToUpper() != "POST")
                        {
                            Console.WriteLine("接收的不是post:" + ctx.Request.HttpMethod);
                            continue;
                        }

                        var responseObject =
                            JSONObject.Parse(new StreamReader(ctx.Request.InputStream).ReadToEnd());
                        Console.WriteLine("接收数据:" + responseObject);
                        ApiClient objC = GetApiClient(LogUrl, LoginApi, loginInfo);
                        if (objC == null)
                        {
                            Console.WriteLine("获取apiclient失败");
                            continue;
                        }

                        JSONObject reO = ExecuteApi(objC, responseObject["api"].ToString(),
                            responseObject.GetJSONObject("jsp"));
                        if (!IsSuccess(reO))
                        {
                            Console.WriteLine(reO.ToString());
                        }
         
                        reMes = reO;
                        ctx.Response.StatusCode = 200;
                   }
                   catch (Exception e)
                   {
                     Console.WriteLine(e.ToString());
                          reMes.Put("err", e.ToString());
                   }
                   finally
                   {
                      using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
                      {
                          writer.Write(reMes);
                          writer.Close();
                      }    
                   
                       ctx.Response.Close(); 
                   }

                }
                Console.WriteLine("退出server");
                listerner.Stop();
            }
        }
                                       
        static ApiClient GetApiClient(string logUrl, string loginApi, object[] loginInfo)
        {
            ApiClient objC = new ApiClient(logUrl);
            var ret = objC.Execute<string>(loginApi, loginInfo);
            if (JSONObject.Parse(ret)["LoginResultType"].ToString() == "1")
            {
                return objC;
            }

            Console.WriteLine(ret);
            return null;
        }

        static JSONObject ExecuteApi(ApiClient apiClient, string api, JSONObject psJson)
        {
            Console.WriteLine("api:" + api);
            Console.WriteLine("psJson:" + psJson);
            string reStr = apiClient.Execute<String>(api, new object[] { psJson.ToString() });
            if (string.IsNullOrEmpty(reStr))
            {
                return null;
            }
            return JSONObject.Parse(reStr);
        }

        static bool IsSuccess(JSONObject jso)
        {
            if (jso == null)
            {
                return false;
            }
            return jso["IsSuccess"].ToString() == "true";
        }
    }
}

