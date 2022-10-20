using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata.FieldElement;
using Kingdee.BOS.JSON;
using Kingdee.BOS.Util;
using System;
using System.ComponentModel;

namespace myObject
{
    [Description("销售订单即时库存显示方案")]
    [Kingdee.BOS.Util.HotUpdate]
    class xsddStockShow : AbstractDynamicFormPlugIn
    {
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            base.BarItemClick(e);
            if (e.BarItemKey.EqualsIgnoreCase("test"))
            {
                // 刷新字段时也会触发BeforeControlDataBinder事件
                this.View.UpdateView("F_Jac_Text");
                this.View.UpdateView("F_Jac_Decimal");
            }
        }
        public override void BeforeControlDataBinder(BeforeControlDataBinder e)
        {
            base.BeforeControlDataBinder(e);
            if (e.Control.Key.EqualsIgnoreCase("F_Jac_Text"))
            {
                var value = this.Model.GetValue("F_Jac_Text");
                e.BindValue = new JSONObject();
                e.BindValue[FieldAppearance.FS_VALUE] = string.Format("{0}-{1}", value, Context.UserName);
            }
            else if (e.Control.Key.EqualsIgnoreCase("F_Jac_Decimal"))
            {
                var value = Convert.ToDecimal(this.Model.GetValue("F_Jac_Decimal"));
                e.BindValue = new JSONObject();
                e.BindValue[FieldAppearance.FS_VALUE] = value * 100;
            }
        }
    }
}
