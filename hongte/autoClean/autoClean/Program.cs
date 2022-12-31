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
            /*const string dbid = "610bbd142a6e15";
            const string userName = "administrator";
            const string password = "kingdee@123";*/

            const string dbid = "610bbd142a6e15";
            const string userName = "administrator";
            const string password = "kingdee@123";

            const int lcid = 2052;
            const int sleepTime = 300;
            while (true)
            {
                try
                {
                    var apiClient = new K3CloudApiClient(url);
                    var isLoginOk = apiClient.Login(dbid, userName, password, lcid);
                    //var rval = false; // Action
                    if (isLoginOk)
                    {
                        var times = 100;
                        while (times > 0)
                        {
                            times--;
                            if (apiClient.Execute<bool>(
                                "shoudongClean.CacheManagerWebApiService.ClearCacheByFormIds,shoudongClean",
                                new object[] { new List<string>(new[] { "BD_MATERIAL" }) })) {
                                Console.WriteLine("执行了一次清理:" + DateTime.Now.ToLocalTime());
                            } else
                            {
                                Console.WriteLine("执行清理失败:" + DateTime.Now.ToLocalTime());
                            }
                            
                            Thread.Sleep(1000 * sleepTime);
                        }
                    }
                    else
                    {
                        Console.WriteLine("登录失败:" + DateTime.Now.ToLocalTime());
                        Thread.Sleep(1000 * sleepTime);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Thread.Sleep(1000 * sleepTime);
                }

            }
        }
    }
}

