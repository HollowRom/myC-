using System.ComponentModel;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata.EntityElement;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.K3.FIN.HS.Business.PlugIn;

namespace myObject
{
    [Description("入库成本调整单按钮插件"), HotUpdate]
    public class instock_cb_plu : InStockEstimateEdit
    {
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            base.BarItemClick(e);
            if (e.BarItemKey.ToUpperInvariant() == "VBDA_tbButton".ToUpper())
            {
                Entity entity = View.Model.BusinessInfo.GetEntity("FEntity");
                DynamicObjectCollection entityDataObject = Model.GetEntityDataObject(entity);
                string showMes = "本次更新行为:";

                //if (entityDataObject != null && entityDataObject.Count > 0)
                //{
                //    foreach (DynamicObject item in entityDataObject)
                //    {
                //        showMes = "单据编号:" + item["BILLNO"].ToString();
                //        showMes = showMes + "--单据行号:" + item["BillSeq"].ToString();
                //        showMes = showMes + "--小数列1:" + item["F_VBDA_Decimal"].ToString();
                //        showMes = showMes + "--小数列2:" + item["F_VBDA_Decimal1"].ToString();
                //    }
                //}
                //this.View.ShowMessage("执行完成");
                //this.View.ShowMessage(showMes);

                int rowNum = entityDataObject.Count;

                for (int idx = 0; idx < rowNum; idx++)
                {
                    DynamicObject item = entityDataObject[idx];
                    if (!item["F_VBDA_Decimal"].ToString().Equals("0") && this.View.GetFieldEditor("F_VBDA_Decimal", idx).Enabled)
                    {
                        //showMes = "单据编号:" + item["BILLNO"].ToString();
                        //showMes = showMes + "--单据行号:" + item["BillSeq"].ToString();
                        //showMes = showMes + "--小数列1:" + item["F_VBDA_Decimal"].ToString();
                        //showMes = showMes + "--小数列2:" + item["F_VBDA_Decimal1"].ToString();
                        DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/update a set FPRICE = " + item["F_VBDA_Decimal"].ToString() + ", FAMOUNT = " + item["F_VBDA_Decimal"].ToString() + " * " + item["F_VBDA_Decimal1"].ToString() + " from T_PRD_INSTOCKENTRY a, T_PRD_INSTOCK b where a.FPRICE = 0 and a.FID = b.FID and b.FBILLNO = '" + item["BILLNO"].ToString() + "' and a.FSEQ = " + item["BillSeq"].ToString());
                        this.View.GetFieldEditor("F_VBDA_Decimal", idx).Enabled = false;
                        this.View.GetFieldEditor("F_VBDA_Decimal1", idx).Enabled = false;
                        showMes = showMes + idx.ToString() + ";";
                    }
                    
                }
                this.View.ShowMessage(showMes);
            }
        }

    }
}
