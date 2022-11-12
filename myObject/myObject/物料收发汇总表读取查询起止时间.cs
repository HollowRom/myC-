using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Report;
using Kingdee.BOS.Util;
using Kingdee.K3.SCM.App.Purchase.Report;
using System.ComponentModel;
using System.Text;
using Kingdee.K3.SCM.App.Stock.Report;
using System;

namespace myObject
{
    [Description("物料收发汇总表读取查询起止时间"), HotUpdate]
    public class wlsfhz_add_seTimeField : StockSummaryRpt
    {
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            base.BuilderReportSqlAndTempTable(filter, tableName);

            var bdate = GetDataByKey(filter.FilterParameter.CustomFilter, "BeginDate");

            var edate = GetDataByKey(filter.FilterParameter.CustomFilter, "EndDate");
            //var fcnum = GetDataByKey(filter.FilterParameter.CustomFilter, "STOCKOUTQTY");
            //var jcnum = GetDataByKey(filter.FilterParameter.CustomFilter, "STOCKJCQTY");

            var dateDiff = Math.Abs(Convert.ToDateTime(bdate).Subtract(Convert.ToDateTime(edate)).Duration().Days);

            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("alter table {0} add F_VBDA_Text varchar(50) ", tableName);
            DBUtils.Execute(this.Context, sqlStr.ToString());
            sqlStr.Clear();
            sqlStr.AppendFormat("alter table {0} add F_VBDA_Text1 varchar(50) ", tableName);
            DBUtils.Execute(this.Context, sqlStr.ToString());
            sqlStr.Clear();
            //sqlStr.AppendFormat("update {0} set F_VBDA_Text = '{1}', F_VBDA_Text1 = '{2}' ", tableName, bdate, edate);
            sqlStr.AppendFormat("update {0} set F_VBDA_Text = '{1}', F_VBDA_Text1 = '{2}' ", tableName, bdate, dateDiff);
            DBUtils.Execute(this.Context, sqlStr.ToString());
        }

    }
}
