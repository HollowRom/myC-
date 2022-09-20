using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using Kingdee.BOS.App.Data;
using Kingdee.K3.SCM.IOS.Business.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;

namespace anfuObject
{
    [Description("创建结算清单覆盖插件")]
    [Kingdee.BOS.Util.HotUpdate]
    public class overJSQDPlugin : IOSCreateSettleTran
    {
        public override void EntryBarItemClick(BarItemClickEventArgs e)
        {
            base.EntryBarItemClick(e);
            if (e.BarItemKey.ToUpperInvariant() == "VBDA_TBBUTTON")
            {
                int entryRowCount = this.View.Model.GetEntryRowCount("FResultEntity");
                for (int row = 0; row < entryRowCount; ++row)
                {
                   DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select top 1 a.F_ANFU_TEXT as hth, c.FNAME as gys from T_SAL_OUTSTOCK a, T_BD_CUSTOMER b, T_BD_CUSTOMER_L c where a.FCUSTOMERID = b.FCUSTID and b.FCUSTID = c.FCUSTID and c.FLOCALEID = '2052' and a.FBillNo = '"
                       + this.View.Model.GetValue("FBizBillNo", row).ToString() + "'");
                if (Dyobj.Count < 1){
                     continue;
                }

                this.View.Model.SetValue("F_VBDA_Text1", Dyobj[0]["gys"], row);
                    this.View.Model.SetValue("F_VBDA_Text", Dyobj[0]["hth"], row);
                }
            }
        }

    }
}
