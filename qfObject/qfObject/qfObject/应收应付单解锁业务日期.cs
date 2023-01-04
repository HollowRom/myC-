using System;
using System.ComponentModel;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Util;

namespace qfObject
{
    [Description("生产订单加载后解锁业务日期字段"), HotUpdate]
    public class unlock_ysd_cgbm_filed : AbstractBillPlugIn
    {
        public override void AfterBindData(EventArgs e)
        {
            base.AfterBindData(e);
            this.View.StyleManager.SetEnabled("FDATE", "BillStatusByHead", true);
        }
    }
}