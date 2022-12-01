using System.ComponentModel;
using System.Text;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Util;
using Kingdee.K3.FIN.AP.App.Report;

namespace myObject
{
    [Description("往来对账明细增加合同号插件"), HotUpdate]
    public class wldzmx_add_hth_col : GoAndComReportService_New
    {
        protected override void BuildData(string tableName)
        {
            base.BuildData(tableName);
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("alter table {0} add F_VBDA_Text nvarchar(255) ", tableName);
            DBUtils.Execute(this.Context, sqlStr.ToString());

            sqlStr.Clear();
            sqlStr.AppendFormat(" MERGE INTO {0} T0 ", tableName);
            //sqlStr.AppendFormat(" using (select N'采购入库单' as typeName,FDELIVERYBILL as newCol, FBILLNO from t_STK_InStock \r\n union all \r\n select N'生产领料单' as typeName,F_VBDA_Text as newCol, FBILLNO from T_PRD_PICKMTRL ) T99");
            sqlStr.AppendFormat(" using hl_ysd_hth_v_plugin as T99");
            sqlStr.AppendFormat(" on T0.FBILLNO = T99.FBILLNO ");
            sqlStr.AppendFormat(" when matched then update set F_VBDA_Text = T99.hth ");
            DBUtils.Execute(this.Context, sqlStr.ToString());
        }
    }
}
