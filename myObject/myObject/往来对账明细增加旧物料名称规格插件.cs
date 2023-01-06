using System;
using System.ComponentModel;
using System.Text;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Util;
using Kingdee.K3.FIN.AP.App.Report;

namespace myObject
{
    [Description("往来对账明细增加旧物料名称规格插件"), HotUpdate]
    public class wldzmx_add_jwl_col : GoAndComReportService_New
    {
        protected override void BuildData(string tableName)
        {
            base.BuildData(tableName);
            DBUtils.Execute(this.Context, "alter table " + tableName + " add F_VBDA_Text nvarchar(255) ");//名称
            DBUtils.Execute(this.Context, "alter table " + tableName + " add F_VBDA_Text1 nvarchar(255) ");//规格

            try
            {
                DBUtils.Execute(this.Context, "/*dialect*/if COL_LENGTH('" + tableName + "', 'FMaterialNumber') is not null update a set F_VBDA_Text = b.F_VBDA_Text11, F_VBDA_Text1 = b.F_VBDA_Text1 from " + tableName + " a, T_BD_MATERIAL b where a.FMaterialNumber = b.FNUMBER");
            } catch(Exception e)
            {
                if (e != null) { }
            }

        }
    }
}
