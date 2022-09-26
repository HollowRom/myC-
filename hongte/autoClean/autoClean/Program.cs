using Kingdee.BOS.WebApi.Client;
using System.Collections.Generic;
using System.Threading;
using System;

namespace autoClean
{
    class autoClean
    {
        static void Main(string[] args)
        {
            const string url = "http://127.0.0.1/k3cloud/";
            const string dbid = "5f3a43d65be160";
            const string userName = "administrator";
            const string password = "888888";
            const int lcid = 2052;
            const int sleepTime = 300;
            while (true)
            {
                var apiClient = new K3CloudApiClient(url);
                var isLoginOk = apiClient.Login(dbid, userName, password, lcid);
                var rval = false; // Action
                if (isLoginOk)
                {
                    var times = 100;
                    while (times > 0)
                    {
                        times--;
                        apiClient.Execute<bool>(
                            "Jac.XkDemo.BOS.WebApi.CacheManagerWebApiService.ClearCacheByFormIds,Jac.XkDemo.BOS.WebApi",
                            new object[] { new List<string>(new[] { "BD_MATERIAL" }) });
                        Console.WriteLine("执行了一次清理:" + DateTime.Now.ToLocalTime());  
                        Thread.Sleep(1000 * sleepTime);
                    }
                }
                else
                {
                    Console.WriteLine("登录失败:" + DateTime.Now.ToLocalTime());  
                    Thread.Sleep(1000 * sleepTime);;
                }
            }
        }
    }
}

