using Kingdee.BOS;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core;
using Kingdee.BOS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Kingdee.BOS.WebApi.Client;

namespace autoCleanObject
{
    [Description("自动清理物料缓存"), HotUpdate]
    public class auto_clean_mat : IScheduleService
    {
        public void Run(Context ctx, Schedule schedule)
        {
            //读取Common.config配置
            const string url = "http://127.0.0.1/k3cloud/";
            
            // <add key="apiUserName" value="administrator" />
            // <add key="apiUserPwd" value="kingdee@123" />    
            string dbid = ctx.DBId;//"610bbd142a6e15";
            string userName = KDConfiguration.Current.GetAppSettingItemValue("apiUserName");
            string password = KDConfiguration.Current.GetAppSettingItemValue("apiUserPwd");

            if (userName == null || userName.Equals("") || password == null || password.Equals(""))
            {
                throw new Exception("Common.config内未配置api账号密码");
            }
            const int lcid = 2052;
            
            var apiClient = new K3CloudApiClient(url);
            var isLoginOk = apiClient.Login(dbid, userName, password, lcid);
            if (isLoginOk)
            {
                apiClient.Execute<bool>(
                    // "shoudongClean.CacheManagerWebApiService.ClearCacheByFormIds,shoudongClean",
                    "autoCleanObject.CacheManagerWebApiService.ClearCacheByFormIds,autoCleanObject",
                    new object[] { new List<string>(new[] { /*"BD_MATERIAL"*/ "BD_UNIT" }) });
                Console.WriteLine("执行了一次清理:" + DateTime.Now.ToLocalTime());
            }
            else
            {
                throw new Exception("登录失败:" + DateTime.Now.ToLocalTime());
            }

        }
    }
}
