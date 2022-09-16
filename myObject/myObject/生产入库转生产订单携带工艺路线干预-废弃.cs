//using System.ComponentModel;
//using Kingdee.BOS.Core;
//using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn;
//using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn.Args;
//using Kingdee.BOS.App.Data;
//using Kingdee.BOS.Orm.DataEntity;
//using Kingdee.BOS.ServiceHelper;
//using Kingdee.BOS.Core.Metadata;

//namespace JDSample.ServicePlugIn.BillConvert
//{
//    [Description("生产入库转生产订单携带工艺路线干预")]
//    [Kingdee.BOS.Util.HotUpdate]
//    public class instockToMoWithRountIdPlugIn : AbstractConvertPlugIn
//    {
//        public override void AfterConvert(AfterConvertEventArgs e)
//        {
//            base.AfterConvert(e);

//            ExtendedDataEntity[] heads = e.Result.FindByEntityKey("FBillHead");

//            if (heads.Length == 0)
//            {
//                return;
//            }
//            DynamicObjectCollection poEntry = heads[0].DataEntity["TreeEntity"] as DynamicObjectCollection;
//            for (int idx = 0; idx < poEntry.Count; idx++)
//            {
//                //long tempOrderEntryId = (long)poEntry[idx]["DEMANDBILLENTRYID"];
//                //if (tempOrderEntryId == 0)
//                //{
//                //    continue;
//                //}

//                //DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select top 1 c1.F_HRCS_BASE " +
//                //                                           "from T_SAL_ORDERENTRY a1,  T_ENG_BOM b1,  T_ENG_BOMCHILD c1 where a1.FMATERIALID = b1.FMATERIALID " +
//                //                                             " and b1.FID = c1.FID " +
//                //                                             " and c1.F_HRCS_BASE > 0 " +
//                //                                             " and a1.FENTRYID = " + tempOrderEntryId.ToString());
//                //if (Dyobj.Count < 1)
//                //{
//                //    continue;
//                //}

//                FormMetadata formMetadata = MetaDataServiceHelper.Load(this.Context, "ENG_Route") as FormMetadata;
//                DynamicObject dynamicObject = BusinessDataServiceHelper.LoadSingle(this.Context, 101927, formMetadata.BusinessInfo.GetDynamicObjectType());

//                FormMetadata formMetadata2 = MetaDataServiceHelper.Load(this.Context, "ENG_BOM") as FormMetadata;
//                DynamicObject dynamicObject2 = BusinessDataServiceHelper.LoadSingle(this.Context, 102147, formMetadata2.BusinessInfo.GetDynamicObjectType());

//                poEntry[idx]["BomId"] = dynamicObject2;
//                poEntry[idx]["RoutingId"] = /*"RT000005";*/dynamicObject;
//                //poEntry[idx]["EntryNote"] = Dyobj[0]["F_HRCS_BASE"].ToString();

//            }
//        }
//    }

//}



