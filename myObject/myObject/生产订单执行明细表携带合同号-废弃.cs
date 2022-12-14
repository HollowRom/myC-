using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Util;
using System.ComponentModel;


namespace myObject
{
    [Description("生产订单执行明细表携带合同号"), HotUpdate]

    public class writeHTHToSCDDZXMX : AbstractBillPlugIn
    {

        //添加引用后,缩写函数
        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);

            //如果等于物料
            if (e.Field.Key == "FMaterialId")
            {
                this.View.Model.SetValue("F_VBDA_Text", this.View.Model.GetValue("FMOBILLNO", e.Row).ToString(), e.Row);
                return;
                //else
                //{
                //    //DataTable dt = DBUtils.ExecuteDataSet(this.Context, sql).Tables[0];
                //    DynamicObjectCollection dt = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select FMATERIALID, FNumber from T_BD_MATERIAL where FNUMBER = '1.01.001' and FDOCUMENTSTATUS = 'C' and FMATERIALID = " + e.NewValue.ToString());

                //    int i = 0;
                //    foreach (DynamicObject drItem in dt)
                //    {
                //        if (i == 0)
                //        {
                //            //给项目名称,赋值
                //            //e.Row 区别是哪一行,+动态变化
                //            this.View.Model.SetValue("F_VBDA_Text", "物料发生了变动" + drItem["FNUMBER"].ToString(), e.Row);
                //            //this.View.Model.SetValue("FEntryNote", "物料发生了变动", e.Row);
                //            this.View.Model.SetValue("F_VBDA_DECIMAL", drItem["FMATERIALID"], e.Row);

                //            this.View.GetFieldEditor("F_VBDA_DECIMAL", e.Row).SetEnabled("", false);
                //            this.View.GetFieldEditor("F_VBDA_Text", e.Row).SetEnabled("", true);
                //            this.View.GetFieldEditor("FTaxPrice", e.Row).SetEnabled("", false);
                //            //var entityKey = "FPOOrderEntry";
                //            //var entity = this.View.BusinessInfo.GetEntity(entityKey);
                //            //var fieldKey = this.Model.GetEntryCurrentFieldKey(entityKey);
                //            //var rowIndex = this.Model.GetEntryCurrentRowIndex(entityKey);
                //            //var rowData = this.Model.GetEntityDataObject(entity, rowIndex);

                //            //this.View.StyleManager.SetEnabled(fieldKey, rowData, "F_VBDA_Text", true);
                //            //this.View.StyleManager.SetEnabled(fieldKey, rowData, "F_VBDA_Decimal", false);
                //            this.View.ShowMessage("单元格已锁定！");
                //        }
                //        i++;
                //    }
                //    if (i == 0)
                //    {
                //        this.View.Model.SetValue("F_VBDA_Text", "", e.Row);
                //    }
            }

        }

        //if (e.Field.Key == "FTaxPrice")
        //{
        //    if (e.NewValue.IsNullOrEmpty())
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        if (!this.View.Model.GetValue("F_VBDA_Text", e.Row).IsNullOrEmpty())
        //        {
        //            this.View.ShowMessage("单元格已锁定！");
        //            //this.View.Model.SetValue("FTaxPrice", "99", e.Row);
        //            this.View.Model.SetValue("F_VBDA_DECIMAL", 99.0, e.Row);
        //            this.View.Model.SetValue("FPrice", 99.0, e.Row);
        //            this.View.ShowMessage("有字段不为空！");
        //            this.View.UpdateView("FTaxPrice", e.Row);
        //        }
        //    }

        //}
        //}
    }
}

