using System;
using System.ComponentModel;
using Kingdee.BOS.Core.DynamicForm.PlugIn;

namespace autoUpdPluin
{
    [Description("定时更新表头单据值")]
    [Kingdee.BOS.Util.HotUpdate]
    public class autoUpdLater : AbstractDynamicFormPlugIn
    {
        public override void OnTimerElapsed(EventArgs e)

        {
            base.OnTimerElapsed(e);

            this.View.Model.SetValue("F_VBDA_Date99", DateTime.Now);

        }
    }
}
