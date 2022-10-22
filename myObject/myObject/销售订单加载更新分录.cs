using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace myObject
{
    [Description("销售订单加载更新表头跳转更新分录")]
    [Kingdee.BOS.Util.HotUpdate]
    public class sorderInitUpdData : AbstractBillPlugIn
    {
        public override void AfterBindData(EventArgs e)
        {
            base.AfterBindData(e);



            //这个方法是判断单据状态。
            //有四种：ADDNEW、EDIT、VIEW、DISASSEMBLY
            //新增、编辑、查看、卸载

            //如果是,新增状态 ADDNEW
            if (!this.View.OpenParameter.Status.Equals(OperationStatus.ADDNEW))
            {
                //给备注和备注1,赋值

                //this.View.Model.SetValue("F_VBDA_INTEGER", 3);
                //this.View.Model.SetValue("FNote1", "备注1", 1);


                //刷新这2个字段
                //this.View.UpdateView("F_VBDA_INTEGER");
                //this.View.UpdateView("FNote1", 1);

                this.View.InvokeFieldUpdateService("FBILLNO", 0);
            }

        }
    }
}
