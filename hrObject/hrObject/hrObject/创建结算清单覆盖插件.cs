using System.ComponentModel;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.K3.SCM.IOS.Business.PlugIn;

namespace hrObject
{
    [Description("创建结算清单覆盖插件")]
    [HotUpdate]
    public class hrOverJSQDPlugin : IOSCreateSettleTran
    {
        public override void EntryBarItemClick(BarItemClickEventArgs e)
        {
            base.EntryBarItemClick(e);
            if (e.BarItemKey.ToUpperInvariant() == "VBDA_TBBUTTON")
            {
                int entryRowCount = this.View.Model.GetEntryRowCount("FResultEntity");
                for (int row = 0; row < entryRowCount; ++row)
                {
                    DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select top 1 a.pluin_FName from v_hr_zzjjsd_v a where a.pluin_FBillno = '"
                       + this.View.Model.GetValue("FBizBillNo", row).ToString() + "'");
                    if (Dyobj.Count < 1)
                    {
                        continue;
                    }

                    this.View.Model.SetValue("F_VBDA_Text", Dyobj[0]["pluin_FName"], row);

                }
            }
        }

    }
}