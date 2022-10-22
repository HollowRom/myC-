using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System;
using System.ComponentModel;

namespace myObject
{
    [Description("定时更新表头单据值")]
    [Kingdee.BOS.Util.HotUpdate]
    public class autoUpdLater : AbstractDynamicFormPlugIn
    {
        const string fieldName = "F_VBDA_Integer";//可以考虑放在配置文件中,但是每次执行都会重新读取
        public override void OnTimerElapsed(EventArgs e)

        {
            base.OnTimerElapsed(e);
            var oldInt = Int32.Parse(this.View.Model.GetValue(fieldName).ToString() + 1 % 10);
            this.View.Model.SetValue(fieldName, oldInt);
            if (oldInt > 0)
            {
                this.View.InvokeFieldUpdateService(fieldName, 0);
            }
        }
    }
}
