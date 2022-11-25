using Kingdee.BOS.Business.Bill;
using Kingdee.BOS.Business.Bill.PlugIn;
using Kingdee.BOS.Util;
using System;
using System.ComponentModel;

namespace myObject
{
    [Description("生产订单加载后解锁业务日期字段"), HotUpdate]
    public class scdd_unlock_billDateField : TemplateBillEdit
    {
        public override void AfterBindData(EventArgs e)
        {
            base.AfterBindData(e);
            this.View.GetControl("FDate").Enabled = true;
            this.View.GetControl("FDATE").Enabled = true;
            this.View.ShowMessage("aabb");
        }

        public override void AfterLoadData(EventArgs e)
        {
            base.AfterLoadData(e);
            this.View.GetControl("FDate").Enabled = true;
            this.View.GetControl("FDATE").Enabled = true;
            this.View.ShowMessage("aabb");
        }
    }
}
