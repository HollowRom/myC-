using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Report;
using Kingdee.K3.SCM.App.Sal.Report;

namespace myObject
{
    [Description("销售出库毛利润报表干预插件")]
    [Kingdee.BOS.Util.HotUpdate]
    public class add_xsck_mlr_plugin : SalOutStockProfitAnalyseRpt
    {
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            base.BuilderReportSqlAndTempTable(filter, tableName);
            this.AddMnemonicCode(filter, tableName);
        }

        // private void AddMnemonicCode(string tableName)
        // {
        //     StringBuilder sqlStr = new StringBuilder();
        //     sqlStr.AppendFormat("alter table {0} add F_VBDA_Text nvarchar(255) ", tableName);
        //     DBUtils.Execute(this.Context, sqlStr.ToString());
        //
        //     sqlStr.Clear();
        //     sqlStr.AppendFormat(" MERGE INTO {0} T0 ", tableName);
        //     sqlStr.AppendFormat(" using test_view as T99");
        //     sqlStr.AppendFormat(" on T0.FBILLNO = T99.FBILLNO ");
        //     sqlStr.AppendFormat(" when matched then update set F_VBDA_Text = T99.fid ");
        //     DBUtils.Execute(this.Context, sqlStr.ToString());
        // }
              
        private void AddMnemonicCode(IRptParams filter, string tableName)
        {
            var fieldListCustomExtension = new List<string>() { "", "" };
            var fieldListOriginal = new List<string>();
            var strFilter = "";
            if (CheckIsNeedChange(filter, fieldListCustomExtension, fieldListOriginal))
            {
                strFilter = filter.FilterParameter.FilterString;
                filter.FilterParameter.FilterString = " 2 > 1 ";
            }
            else
            {
                base.BuilderReportSqlAndTempTable(filter, tableName);
            }

            for (int i = 0; i < fieldListCustomExtension.Count; i++)
            {
                _ = strFilter.Replace(fieldListCustomExtension[i], "T0." + fieldListCustomExtension[i]);
            }

            for (int i = 0; i < fieldListOriginal.Count; i++)
            {
                _ = strFilter.Replace(fieldListOriginal[i], "T99." + fieldListOriginal[i]);
            }

            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("alter table {0} add F_VBDA_Text nvarchar(255) ", tableName);
            DBUtils.Execute(this.Context, sqlStr.ToString());

            sqlStr.Clear();
            sqlStr.AppendFormat(" MERGE INTO {0} T0 ", tableName);
            sqlStr.AppendFormat(" using test_view as T99");
            sqlStr.AppendFormat(" on T0.FBILLNO = T99.FBILLNO and {0}", strFilter);
            sqlStr.AppendFormat(" when matched then update set F_VBDA_Text = T99.fid ");
            DBUtils.Execute(this.Context, sqlStr.ToString());
        }
        public override ReportHeader GetReportHeaders(IRptParams filter)
        {
            ReportHeader header = base.GetReportHeaders(filter);
            header.AddChild("F_VBDA_Text", new LocaleValue("新增的列"));

            return base.GetReportHeaders(filter);
        }

        private static bool CheckIsNeedChange(IRptParams filter, List<string> filedListCustomExtension,
            List<string> fieldListOriginal)
        {
            for (int i = 0; i < filter.FilterParameter.FilterRows.Count; i++)
            {
                string strFieldName = filter.FilterParameter.FilterRows[i].FilterField.FieldName;
                if (!filedListCustomExtension.Contains(strFieldName))
                {
                    fieldListOriginal.Add(strFieldName);
                }
            }

            return filter.FilterParameter.FilterRows.Count != fieldListOriginal.Count;
        }
    }
}
