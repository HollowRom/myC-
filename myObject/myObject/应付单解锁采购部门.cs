using Kingdee.BOS.Business.Bill.PlugIn;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Util;
using Kingdee.K3.MFG.SUB.Business.PlugIn.Bill;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace myObject
{
    //[Description("应收单解锁采购部门")]
    //[HotUpdate]
    //public class unlock_ysd_cgbm_filed : AbstractBillPlugIn
    //{
    //    public override void BarItemClick(BarItemClickEventArgs e)
    //    {
    //        base.EntryBarItemClick(e);
    //        if (e.BarItemKey.ToUpperInvariant() == "VBDA_TBBUTTON")
    //        {
    //            this.View.StyleManager.SetEnabled("FPURCHASEDEPTID", "BillStatusByHead", true);
    //        }
    //    }

    //}

    [Description("生产订单加载后解锁业务日期字段"), HotUpdate]
    public class unlock_ysd_cgbm_filed : AbstractBillPlugIn
    {
        //public override void AfterBindData(EventArgs e)
        //{
        //    base.AfterBindData(e);
        //    this.View.GetControl("FPURCHASEDEPTID").Enabled = true;
        //    this.View.GetControl("FPURCHASEDEPTID").Enabled = true;
        //    this.View.ShowMessage("aabb");
        //}

        public override void AfterBindData(EventArgs e)
        {
            base.AfterBindData(e);
            this.View.StyleManager.SetEnabled("FPURCHASEDEPTID", "BillStatusByHead", true);
            //this.View.GetControl("FPURCHASEDEPTID").Enabled = true;
            //this.View.GetControl("FPURCHASEDEPTID").Enabled = true;
            this.View.ShowMessage("aabb2");
        }
    }
}
