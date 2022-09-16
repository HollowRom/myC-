//using System.ComponentModel;
//using System.Text;
//using Kingdee.BOS;
//using Kingdee.BOS.App.Data;
//using Kingdee.BOS.Core.Report;
//using Kingdee.K3.SCM.App.Sal.Report;

//namespace ClassLibrary1
//{
//    [Description("销售订单执行汇总表增加字段干预插件")]
//    [Kingdee.BOS.Util.HotUpdate]
//    public class add_SalCollectRpt_col : SalCollectRpt
//    {
//        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
//        {
//            base.BuilderReportSqlAndTempTable(filter, tableName);
//            this.AddMnemonicCode(tableName);
//        }

//        private void AddMnemonicCode(string tableName)
//        {
//            StringBuilder sqlStr = new StringBuilder();
//            sqlStr.AppendFormat("alter table {0} add F_Add_Col1 nvarchar(255) ", tableName);
//            DBUtils.Execute(this.Context, sqlStr.ToString());

//            sqlStr.Clear();
//            sqlStr.AppendFormat(" MERGE INTO {0} T0 ", tableName);
//            sqlStr.AppendFormat(" using (select FMATERIALID, fnumber as fnotee from t_bd_material) T99");
//            sqlStr.AppendFormat(" on T0.FMATERIALID = T99.FMATERIALID");
//            sqlStr.AppendFormat(" when matched then update set F_Add_Col1 = T99.fnotee ");
//            DBUtils.Execute(this.Context, sqlStr.ToString());
//        }

//        public override ReportHeader GetReportHeaders(IRptParams filter)
//        {
//            ReportHeader header = base.GetReportHeaders(filter);
//            header.AddChild("F_Add_Col1", new LocaleValue("新增的列"));

//            return base.GetReportHeaders(filter);
//        }
//    }
//}
