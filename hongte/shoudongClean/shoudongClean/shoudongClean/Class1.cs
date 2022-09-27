using Kingdee.BOS.App.Core;
using Kingdee.BOS.Core.Const;
using Kingdee.BOS.ServiceFacade.KDServiceFx;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.ServicesStub;
using System.Collections.Generic;
namespace shoudongClean
{    /// <summary>    /// 【WebApi】缓存清理    /// </summary>    
    public class CacheManagerWebApiService : AbstractWebApiBusinessService    {
        public CacheManagerWebApiService(KDServiceContext context):base(context)
        {            //       
        }       
        /// <summary>        /// 清理缓存（整个基础资料的数据缓存全清）        /// </summary>        
            /// <param name="formIds">业务对象标识</param>    
            /// /// <returns></returns>     
            public bool ClearCacheByFormIds(List<string> formIds)
        {
            if (formIds == null || formIds.Count == 0)
            {
                return false;
            }
            var area = this.KDContext.Session.AppContext.GetAreaCacheKey();
            foreach (var formId in formIds)
            {
                var metadata = FormMetaDataCache.GetCachedFormMetaData(this.KDContext.Session.AppContext, formId);
                if (metadata != null)
                {
                    CacheUtil.ClearCache(area, metadata.BusinessInfo.GetEntity(0).TableName);
                    CacheUtil.ClearCache(this.KDContext.Session.AppContext.DBId + formId, CacheRegionConst.BOS_QuickBaseDataCache);
                }
            }
            return true;
        }       
        /// <summary>        /// 清理缓存（只清理指定内码的数据缓存）        /// </summary>        /// <param name="formId">业务对象标识</param>  
    /// <param name="keys">数据内码</param>     
    public bool ClearCacheByPrimaryKeys(string formId, List<string> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                return false;
            }
            var metadata = FormMetaDataCache.GetCachedFormMetaData(this.KDContext.Session.AppContext, formId);
            if (metadata == null)
            {
                return false;
            }
            using (DataEntityCacheManager cacheManager = new DataEntityCacheManager(this.KDContext.Session.AppContext, metadata.BusinessInfo.GetDynamicObjectType()))
            {
                cacheManager.RemoveCacheByPrimaryKeys(keys);
            }
            CacheUtil.ClearCache(this.KDContext.Session.AppContext.DBId + formId, CacheRegionConst.BOS_QuickBaseDataCache);
            return true;
        }
    }
}
