using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System;
using System.ComponentModel;

namespace autoUpdPluin
{
    [Description("随心跳触发单据编号值更新事件")]
    [Kingdee.BOS.Util.HotUpdate]
    public class autoUpdField : AbstractDynamicFormPlugIn
    {
        const string fieldName = "FBILLNO";//可以考虑放在配置文件中,但是每次执行都会重新读取
        public override void OnTimerElapsed(EventArgs e)

        {
            base.OnTimerElapsed(e);

            this.View.InvokeFieldUpdateService(fieldName, 0);
        }
    }
}
