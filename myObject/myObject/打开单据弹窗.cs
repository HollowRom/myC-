//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Kingdee.BOS;
//using Kingdee.BOS.Core.Bill.PlugIn;
//using Kingdee.BOS.App.Data;
//using Kingdee.BOS.Orm.DataEntity;
//using Kingdee.BOS.Core.DynamicForm.PlugIn;
//using Kingdee.BOS.Core.DynamicForm.PlugIn.ControlModel;
//using Kingdee.BOS.Core.Metadata;
//using Kingdee.BOS.Util;
////添加引用后,缩写函数
//using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
//using System.ComponentModel;


//namespace Kingdee.Bos.Project.BillDataChanged.plugln
//{
//    [Description("打开窗口出现消息")]
//    [Kingdee.BOS.Util.HotUpdate]

//    public class OnCreateShowMess : AbstractBillPlugIn
//    {

//        //添加引用后,缩写函数
//        public override void AfterBindData(EventArgs e)
//        {
//            base.AfterBindData(e);
//            this.View.ShowMessage("单据编号:" + this.View.Model.GetValue("FBillNo").ToString());
//        }
//    }
//}

