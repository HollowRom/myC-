using Kingdee.BOS;
using Kingdee.BOS.BusinessEntity.Organizations;
using Kingdee.BOS.Cache;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Const;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Kingdee.BOS.WebApi.Client;
using Kingdee.BOS.App;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.App.Core;

namespace autoCleanObject
{
    //[Description("自动清理物料缓存"), HotUpdate]
    public class auto_clean_mat : IScheduleService
    {
        public void Run(Context ctx, Schedule schedule)
        {
            //const string url = "http://127.0.0.1/k3cloud/";
            /*const string dbid = "610bbd142a6e15";
            const string userName = "administrator";
            const string password = "kingdee@123";*/

            //string dbid = ctx.DBId;//"610bbd142a6e15";
            //string userName = "administrator";
            //string password = "kingdee@123";

            //const int lcid = 2052;
            //const int sleepTime = 300;

            //        var apiClient = new K3CloudApiClient(url);
            //        var isLoginOk = apiClient.Login(dbid, userName, password, lcid);
            //        //var rval = false; // Action
            //        if (isLoginOk)
            //        {
            //            var times = 100;
            //            while (times > 0)
            //            {
            //                times--;
            //                apiClient.Execute<bool>(
            //                    "autoCleanObject.cleanBase,autoCleanObject",
            //                    new object[] { new List<string>(new[] { "BD_MATERIAL" }) });
            //                Console.WriteLine("执行了一次清理:" + DateTime.Now.ToLocalTime());
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("登录失败:" + DateTime.Now.ToLocalTime());
            //        }


            //}


            if ((ctx.CurrentOrganizationInfo == null) || ctx.CurrentOrganizationInfo.ID < 0)
            {
                Organization curOrg = OrganizationServiceHelper.ReadOrgInfoByOrgId(ctx, 1); //管理员默认组织
                List<long> functions = new List<long>();
                if (!curOrg.OrgFunctions.IsNullOrEmptyOrWhiteSpace())
                {
                    functions = Array.ConvertAll(curOrg.OrgFunctions.Split(','), (a) => { return Convert.ToInt64(a); }).ToList();
                }
                var CurrentOrganizationInfo = new OrganizationInfo()
                { ID = curOrg.Id, Name = curOrg.Name, FunctionIds = functions, AcctOrgType = curOrg.AcctOrgType };
                ctx.CurrentOrganizationInfo = CurrentOrganizationInfo;
            }

            



            //var kcmgr = KCacheManagerFactory.Instance.GetCacheManager("T_BD_MATERIAL", /*ctx.DBId + "True"*/"610bbd142a6e15True");
            //if (kcmgr != null)
            //{
            //    kcmgr.ClearRegion();
            //}
            //else
            //{
            //    throw new Exception("KCacheManagerFactory == null");
            //}

            IMetaDataService metaService = ServiceHelper.GetService<IMetaDataService>();

            FormMetadata meta = metaService.Load(ctx, "BD_MATERIAL") as FormMetadata; //加载基础属性的元数据

            DataEntityCacheManager cacheManager = new DataEntityCacheManager(ctx, meta.BusinessInfo.GetDynamicObjectType());

            cacheManager.RemoveCacheByDt();

            var area = ctx.GetAreaCacheKey();
            var formId = "BD_MATERIAL";
            var metadata = FormMetaDataCache.GetCachedFormMetaData(ctx, formId);

            if (metadata != null)
            {
                CacheUtil.ClearCache(area, metadata.BusinessInfo.GetEntity(0).TableName);
                CacheUtil.ClearCache(ctx.DBId + formId, CacheRegionConst.BOS_QuickBaseDataCache);
            }

        }

        //private bool ClearCacheByFormIds(Context ctx, List<string> formIds)
        //{
        //    if (formIds == null || formIds.Count == 0)
        //    {
        //        return false;
        //    }
        //    var area = ctx.GetAreaCacheKey();
        //    foreach (var formId in formIds)
        //    {
        //        var metadata = FormMetaDataCache.GetCachedFormMetaData(ctx, formId);
        //        if (metadata != null)
        //        {
        //            CacheUtil.ClearCache(area, metadata.BusinessInfo.GetEntity(0).TableName);
        //            CacheUtil.ClearCache(ctx.DBId + formId, CacheRegionConst.BOS_QuickBaseDataCache);
        //        }
        //    }
        //    return true;
        //}
        /// <summary>        
        /// 清理缓存（只清理指定内码的数据缓存）
        /// </summary>    
        /// <param name="formId">业务对象标识</param>  
        /// <param name="keys">数据内码</param>     
        //private bool ClearCacheByPrimaryKeys(Context ctx, string formId, List<string> keys)
        //{
        //    if (keys == null || keys.Count == 0)
        //    {
        //        return false;
        //    }
        //    var metadata = FormMetaDataCache.GetCachedFormMetaData(ctx, formId);
        //    if (metadata == null)
        //    {
        //        return false;
        //    }
        //    using (DataEntityCacheManager cacheManager = new DataEntityCacheManager(ctx, metadata.BusinessInfo.GetDynamicObjectType()))
        //    {
        //        cacheManager.RemoveCacheByPrimaryKeys(keys);
        //    }
        //    CacheUtil.ClearCache(ctx.DBId + formId, CacheRegionConst.BOS_QuickBaseDataCache);
        //    return true;
        //}

    }
}
