using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Report;
using Kingdee.K3.SCM.App.Purchase.Report;
using System.ComponentModel;
using System.Text;

namespace hlObject
{
    [Description("采购订单执行明细表合计开票金额干预插件")]
    [Kingdee.BOS.Util.HotUpdate]
    public class cgddzxmx_kpje_plugin : PurchaseOrderExecuteRpt
    {
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            base.BuilderReportSqlAndTempTable(filter, tableName);

            var edate = GetDataByKey(filter.FilterParameter.CustomFilter, "OrderEndDate");
            
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("alter table {0} add F_VBDA_Decimal decimal(23, 10) ", tableName);
            DBUtils.Execute(this.Context, sqlStr.ToString());
            sqlStr.Clear();
            sqlStr.AppendFormat("alter table {0} add F_VBDA_Decimal1 decimal(23, 10) ", tableName);
            DBUtils.Execute(this.Context, sqlStr.ToString());

            sqlStr.Clear();
            sqlStr.AppendFormat("select a1.FNAME                                                           as pluin_name, " +
       "isnull(sum(a1.FALLAMOUNTFOR - isnull(b1.FREALPAYAMOUNTFOR, 0)), 0) as pluin_undone, " +
       "isnull(sum(isnull(b1.FREALPAYAMOUNTFOR, 0)), 0) as pluin_done " +
        "from(select sum(a.FALLAMOUNTFOR) as FALLAMOUNTFOR, c.FNAME " +
        "from T_AP_PAYABLE a, " +
       "t_BD_Supplier b, " +
           "t_BD_Supplier_L c " +
      "where a.FALLAMOUNTFOR > 0 " +
    "    and a.FSUPPLIERID = b.FSUPPLIERID " +
    "    and b.FSUPPLIERID = c.FSUPPLIERID " +
    "    and c.FLOCALEID = 2052 " +
    "    and a.FDATE <= '{0}' " +
    "  group by c.FNAME) a1 " +
    "     left join " +
    " (select sum(a.FREALPAYAMOUNTFOR) as FREALPAYAMOUNTFOR, c.FNAME " +
    "  from T_AP_PAYBILL a, " +
    "       t_BD_Supplier b, " +
    "       t_BD_Supplier_L c " +
    "  where a.FREALPAYAMOUNTFOR > 0 " +
    "    and a.FRECTUNIT = b.FSUPPLIERID " +
    "    and b.FSUPPLIERID = c.FSUPPLIERID " +
    "    and c.FLOCALEID = 2052 " +
    "    and a.FDATE <= '{1}' " +
   "   group by c.FNAME) b1 " +
   "  on a1.FNAME = b1.FNAME " +
    "group by a1.FNAME ", edate, edate);
            var updTab = sqlStr.ToString();

            sqlStr.Clear();
            sqlStr.AppendFormat("/*dialect*/update a9 set F_VBDA_Decimal = a8.pluin_done, F_VBDA_Decimal1 = a8.pluin_undone from {0} a9, ({1}) a8 where a9.FSUPPLIERNAME = a8.pluin_name", tableName, updTab);
            
            DBUtils.Execute(this.Context, sqlStr.ToString());

        }
    }
}
