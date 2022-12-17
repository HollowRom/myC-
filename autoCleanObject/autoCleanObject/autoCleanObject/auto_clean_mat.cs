using Kingdee.BOS;
using Kingdee.BOS.App.Core;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Const;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using System.Collections.Generic;
using System.ComponentModel;

namespace autoCleanObject
{
    [Description("自动清理物料缓存"), HotUpdate]
    public class auto_clean_mat : IScheduleService
    {
        public void Run(Context ctx, Schedule schedule)
        {
            this.ClearCacheByFormIds(ctx, new List<string>(new[] { "BD_MATERIAL" }));
        }

        private bool ClearCacheByFormIds(Context ctx, List<string> formIds)
        {
            if (formIds == null || formIds.Count == 0)
            {
                return false;
            }
            var area = ctx.GetAreaCacheKey();
            foreach (var formId in formIds)
            {
                var metadata = FormMetaDataCache.GetCachedFormMetaData(ctx, formId);
                if (metadata != null)
                {
                    CacheUtil.ClearCache(area, metadata.BusinessInfo.GetEntity(0).TableName);
                    CacheUtil.ClearCache(ctx.DBId + formId, CacheRegionConst.BOS_QuickBaseDataCache);
                }
            }
            return true;
        }
        /// <summary>        
        /// 清理缓存（只清理指定内码的数据缓存）
        /// </summary>    
        /// <param name="formId">业务对象标识</param>  
        /// <param name="keys">数据内码</param>     
        private bool ClearCacheByPrimaryKeys(Context ctx, string formId, List<string> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                return false;
            }
            var metadata = FormMetaDataCache.GetCachedFormMetaData(ctx, formId);
            if (metadata == null)
            {
                return false;
            }
            using (DataEntityCacheManager cacheManager = new DataEntityCacheManager(ctx, metadata.BusinessInfo.GetDynamicObjectType()))
            {
                cacheManager.RemoveCacheByPrimaryKeys(keys);
            }
            CacheUtil.ClearCache(ctx.DBId + formId, CacheRegionConst.BOS_QuickBaseDataCache);
            return true;
        }

    }
}
