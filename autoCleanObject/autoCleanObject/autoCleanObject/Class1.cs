using Kingdee.BOS;
using Kingdee.BOS.BusinessEntity.CloudPlatform;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Msg;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using System.ComponentModel;
using Kingdee.BOS.Cache;

namespace autoCleanObject
{
    [Description("自动清理物料缓存"), HotUpdate]
    public class auto_clean_mat : IScheduleService
    {
        public void Run(Context ctx, Schedule schedule)
        {
            ClearCacheByPrimaryKeys("610bbd142a6e15True", "T_BD_MATERIAL");
        }

        /// <summary>
        /// 清理缓存（只清理指定内码的数据缓存）
        /// </summary>
        /// <param name="ctx">上下文</param>
        /// <param name="businessInfo">业务对象元数据</param>
        /// <param name="keys">数据内码</param>
        private static bool ClearCacheByPrimaryKeys(string q, string d)
        {
            var kcmgr = KCacheManagerFactory.Instance.GetCacheManager(q, d);
            if (kcmgr != null)
            {
                kcmgr.ClearRegion();
            }

            return true;
            //var metadata = FormMetaDataCache.GetCachedFormMetaData(ctx, formId);
            //if (metadata != null)
            //{
            //    CacheUtil.ClearCache(area, metadata.BusinessInfo.GetEntity(0).TableName);
            //    CacheUtil.ClearCache(this.KDContext.Session.AppContext.DBId + formId, CacheRegionConst.BOS_QuickBaseDataCache);
            //}


            //if (keys == null || keys.Count == 0)
            //{
            //    return false;
            //}
            //using (DataEntityCacheManager cacheManager = new DataEntityCacheManager(ctx, businessInfo.GetDynamicObjectType()))
            //{
            //    cacheManager.RemoveCacheByPrimaryKeys(formId);
            //}
            //CacheUtil.ClearCache(ctx.DBId + formId, CacheRegionConst.BOS_QuickBaseDataCache);
            //return true;
        }
    }
}
