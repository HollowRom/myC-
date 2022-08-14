using System.ComponentModel;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;

namespace Kingdee.Bos.ProJect.BillBeforeSave.Plugln
{
    [Description("生产入库单转生产订单保存携带工艺路线插件")]
    [Kingdee.BOS.Util.HotUpdate]
    public class workInStockToMoOrderWithRountIdPlugIn : AbstractBillPlugIn
    {
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            
            if (e.BarItemKey.Equals("tbSplitSave"))
            {
                int rowNum = this.View.Model.GetEntryRowCount("FTreeEntity");
                for (int idx = 0; idx < rowNum; idx++)
                {
                    //long tempOrderEntryId = (long)this.View.Model.GetValue("FDEMANDBILLENTRYID", idx);
                    //if (tempOrderEntryId == 0)
                    //{
                    //    continue;
                    //}
                    //DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select top 1 c1.F_HRCS_BASE " +
                    //                                           "from T_SAL_ORDERENTRY a1,  T_ENG_BOM b1,  T_ENG_BOMCHILD c1 where a1.FMATERIALID = b1.FMATERIALID " +
                    //                                             " and b1.FID = c1.FID " +
                    //                                             " and c1.F_HRCS_BASE > 0 " +
                    //                                             " and a1.FENTRYID = " + tempOrderEntryId.ToString());
                    //if (Dyobj.Count < 1)
                    //{
                    //    continue;
                    //}
                    this.View.Model.SetValue("RoutingId", "RT000005", idx);
                    this.View.UpdateView("RoutingId", idx);
                }
            }
            this.View.ShowMessage("一个弹窗");
            base.BarItemClick(e);
        }
    }
}

