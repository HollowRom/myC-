using System.ComponentModel;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn;
using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn.Args;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Util;

namespace myObject
{
    [Description("销售订单转采购申请单取BOM包材材质工艺干预插件"), HotUpdate]
    public class seodToPurWithBOMIdPlugIn : AbstractConvertPlugIn
    {
        public override void AfterConvert(AfterConvertEventArgs e)
        {
            base.AfterConvert(e);

            ExtendedDataEntity[] heads = e.Result.FindByEntityKey("FBillHead");

            if (heads.Length == 0)
            {
                return;
            }
            DynamicObjectCollection poEntry = heads[0].DataEntity["ReqEntry"] as DynamicObjectCollection;
            for (int idx = 0; idx < poEntry.Count; idx++)
            {
                long tempOrderEntryId = (long)poEntry[idx]["DEMANDBILLENTRYID"];
                if (tempOrderEntryId == 0)
                {
                    continue;
                }

                DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select top 1 c1.F_HRCS_BASE " +
                                                           "from T_SAL_ORDERENTRY a1,  T_ENG_BOM b1,  T_ENG_BOMCHILD c1 where a1.FMATERIALID = b1.FMATERIALID " +
                                                             " and b1.FID = c1.FID " +
                                                             " and c1.F_HRCS_BASE > 0 " +
                                                             " and a1.FENTRYID = " + tempOrderEntryId.ToString());
                if (Dyobj.Count < 1)
                {
                    continue;
                }

                FormMetadata formMetadata = MetaDataServiceHelper.Load(this.Context, "BAS_PreBaseDataOne") as FormMetadata;
                DynamicObject dynamicObject = BusinessDataServiceHelper.LoadSingle(this.Context, Dyobj[0]["F_HRCS_BASE"], formMetadata.BusinessInfo.GetDynamicObjectType());

                poEntry[idx]["F_HRCS_BASE"] = dynamicObject;
                //poEntry[idx]["EntryNote"] = Dyobj[0]["F_HRCS_BASE"].ToString();

            }
        }
    }

}



