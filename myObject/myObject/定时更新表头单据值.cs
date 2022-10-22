using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System;
using System.ComponentModel;

namespace myObject
{
    [Description("定时更新表头单据值")]
    [Kingdee.BOS.Util.HotUpdate]
    public class autoUpdLater : AbstractDynamicFormPlugIn
    {
        public override void OnTimerElapsed(EventArgs e)

        {
            base.OnTimerElapsed(e);

            this.View.Model.SetValue("F_VBDA_Integer", Int32.Parse(this.View.Model.GetValue("F_VBDA_Integer").ToString()) + 1 % 10);

        }
    }
}
