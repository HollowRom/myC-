//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using Kingdee.BOS;
//using Kingdee.BOS.App.Data;
//using Kingdee.BOS.Core.Report;
//using Kingdee.K3.SCM.App.Sal.Report;

//namespace anfuObject
//{
//    [Description("销售出库毛利润报表干预插件")]
//    [Kingdee.BOS.Util.HotUpdate]
//    public class add_xsck_mlr_plugin : SalOutStockProfitAnalyseRpt
//    {
//        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
//        {
//            base.BuilderReportSqlAndTempTable(filter, tableName);
//            this.AddMnemonicCode(tableName);
//        }

//        private void AddMnemonicCode(string tableName)
//        {
//            StringBuilder sqlStr = new StringBuilder();
//            sqlStr.AppendFormat("alter table {0} add F_VBDA_Text nvarchar(255) ", tableName);
//            DBUtils.Execute(this.Context, sqlStr.ToString());

//            sqlStr.Clear();
//            sqlStr.AppendFormat(" MERGE INTO {0} T0 ", tableName);
//            sqlStr.AppendFormat(" using test_view as T99");
//            sqlStr.AppendFormat(" on T0.FBILLNO = T99.FBILLNO ");
//            sqlStr.AppendFormat(" when matched then update set F_VBDA_Text = T99.fid ");
//            DBUtils.Execute(this.Context, sqlStr.ToString());
//        }

//        public override ReportHeader GetReportHeaders(IRptParams filter)
//        {
//            ReportHeader header = base.GetReportHeaders(filter);
//            header.AddChild("F_VBDA_Text", new LocaleValue("新增的列"));

//            return base.GetReportHeaders(filter);
//        }
//    }
//}
