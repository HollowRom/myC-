using System;
using Kingdee.BOS.Core.Bill.PlugIn;
using System.ComponentModel;
using Kingdee.BOS.Util;

namespace myObject
{
    [Description("打开窗口出现消息"), HotUpdate]

    public class OnCreateShowMess : AbstractBillPlugIn
    {

        //添加引用后,缩写函数
        public override void AfterBindData(EventArgs e)
        {
            base.AfterBindData(e);
            this.View.ShowMessage("单据编号:" + this.View.Model.GetValue("FBillNo").ToString());
        }
    }
}

