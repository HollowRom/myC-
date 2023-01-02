using System.ComponentModel;
using System.Text;
using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Report;
using Kingdee.K3.SCM.App.Stock.Report;

namespace anfuObject
{
    [Description("物料收发明细表增加合同号干预插件")]
    [Kingdee.BOS.Util.HotUpdate]
    public class add_StockDetailRpt_hth_col : StockDetailRpt
    {
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            base.BuilderReportSqlAndTempTable(filter, tableName);
            this.AddMnemonicCode(tableName);
        }

        private void AddMnemonicCode(string tableName)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("alter table {0} add F_anfu_Text nvarchar(255) ", tableName);
            DBUtils.Execute(this.Context, sqlStr.ToString());

            sqlStr.Clear();
            sqlStr.AppendFormat(" MERGE INTO {0} T0 ", tableName);
            sqlStr.AppendFormat(" using v_af_wlsfmxb_add_hth_col as T99");
            sqlStr.AppendFormat(" on T0.FBILLNAME = T99.typeName and T0.FBILLNO = T99.FBILLNO ");
            sqlStr.AppendFormat(" when matched then update set F_anfu_Text = T99.newCol ");
            DBUtils.Execute(this.Context, sqlStr.ToString());
        }

        public override ReportHeader GetReportHeaders(IRptParams filter)
        {
            ReportHeader header = base.GetReportHeaders(filter);
            header.AddChild("F_anfu_Text", new LocaleValue("新增的列"));

            return base.GetReportHeaders(filter);
        }
    }
}
