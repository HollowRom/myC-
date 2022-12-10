using Kingdee.BOS.Util;
using System.ComponentModel;
using Kingdee.BOS;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;

namespace myObject
{
    /// <summary>
    /// 【表单插件】打开本地电脑上的exe程序
    /// </summary>
    [Description("执行带参数exe"), HotUpdate]
    public class StartupExeFormPlugIn : AbstractDynamicFormPlugIn
    {
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            base.ButtonClick(e);
            var linkBtnKey = "F_VBDA_Link";
            if (e.Key.EqualsIgnoreCase(linkBtnKey))
            {
                if (Context.ClientType != ClientType.Silverlight && Context.ClientType != ClientType.WPF)
                {
                    this.View.ShowMessage("只能在客户端上使用此功能");
                    return;
                }
                var url = @"C:\exeWithFlag.exe";
                //var url = @"D:\mySome\myGo\str\exeWithFlag\exeWithFlag\exeWithFlag.exe";
                this.View.GetControl(linkBtnKey).InvokeControlMethod("SetClickFromServerOfParameter", url, "sfaasf fsafsa xgtrg51 cxg165");
            }
        }
    }
}

