using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Report;
using Kingdee.BOS.Util;
using Kingdee.K3.SCM.App.Sal.Report;
using System.ComponentModel;

namespace hqObject
{
    [Description("销售排名明细表增加字段"), HotUpdate]
    public class xspmmx_add_col : SaleRankingDetailListService
    {
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            base.BuilderReportSqlAndTempTable(filter, tableName);
            DBUtils.Execute(this.Context, "alter table " + tableName + " add F_VBDA_Text varchar(2048) ");
            DBUtils.Execute(this.Context, "/*dialect*/update a set F_VBDA_Text = isnull(b.FNOTE, '') from " + tableName + " a left join T_SAL_ORDER b on a.FBILLNO = b.FBILLNO");
        }
    }
}
