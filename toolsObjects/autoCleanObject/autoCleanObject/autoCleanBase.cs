using System.Collections.Generic;
using Kingdee.BOS;
using Kingdee.BOS.Cache;
using Kingdee.BOS.ServiceFacade.KDServiceFx;
using Kingdee.BOS.WebApi.ServicesStub;

namespace autoCleanObject
{
    public class CacheManagerWebApiService : AbstractWebApiBusinessService
    {
        public CacheManagerWebApiService(KDServiceContext context) : base(context)
        {
        }

        public bool ClearCacheByFormIds(List<string> formIds)
        {
            if (formIds == null || formIds.Count == 0)
            {
                return false;
            }

            var area = this.KDContext.Session.AppContext.GetAreaCacheKey();
            foreach (var formId in formIds)
            {
                KCacheManagerFactory.Instance.GetCacheManager( /*"T_BD_MATERIAL"*/
                    "T_" + formId, /*"639465fe4c30fdTrue"*/
                    this.KDContext.Session.AppContext.DBId + "True" /*"610bbd142a6e15True"*/)?.ClearRegion();
            }

            return true;
        }

        public static bool ClearCacheByFormIdsWithDBId(List<string> formIds, string dbid)
        {
            if (formIds == null || formIds.Count == 0)
            {
                return false;
            }

            foreach (var formId in formIds)
            {
                KCacheManagerFactory.Instance.GetCacheManager( /*"T_BD_MATERIAL"*/
                    "T_" + formId, /*"639465fe4c30fdTrue"*/dbid + "True" /*"610bbd142a6e15True"*/)?.ClearRegion();
            }

            return true;
        }
    }
}