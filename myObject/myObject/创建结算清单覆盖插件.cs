using System.ComponentModel;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;

namespace Kingdee.K3.SCM.IOS.Business.PlugIn
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
                    this.View.Model.SetValue("F_VBDA_Text", this.View.Model.GetValue("FBizBillNo", row).ToString() + "--自定义的文本", row);
                }
            }
        }

    }
}


