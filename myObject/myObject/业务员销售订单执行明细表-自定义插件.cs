using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core.Report;
using Kingdee.K3.SCM.App.Sal.Report;

namespace myObject
{
    [Description("业务员销售订单执行明细表-自定义插件")]
    [Kingdee.BOS.Util.HotUpdate]
    public class xsdd_run_entry_plugin : SalDetailRpt
    {
        private string[] tempTableNames;
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            List<string> fieldListCustomExtension = new List<string>() { "F_CHECK_USERID", "F_CHECK_NAME", "F_anfu_Text" };
            List<string> fieldListOriginal = new List<string>();
            string strFilter = "";
            bool checkSome = CheckIsNeedChange(filter, fieldListCustomExtension, fieldListOriginal);
            if (checkSome)
            {
                strFilter = filter.FilterParameter.FilterString;
                filter.FilterParameter.FilterString = " 2 > 1 ";
            }
            //else
            //{
            //    base.BuilderReportSqlAndTempTable(filter, tableName);
            //    string strSql2 = string.Format(@"select T1.* into {0} from {1} T1 join username_pwd_plugin C on T1.FSALES = C.F_CHECK_NAME where ({2})",
            //    tableName, strTable, strFilter);

            //    DBUtils.Execute(this.Context, strSql);
            //    return;
            //}

            int checkUP = 0;
            for (int i = 0; i < fieldListCustomExtension.Count; i++)
            {
                if (fieldListCustomExtension[i] == "F_CHECK_USERID" || fieldListCustomExtension[i] == "F_CHECK_NAME") {
                    checkUP++;
                }
                _ = strFilter.Replace(fieldListCustomExtension[i], "C." + fieldListCustomExtension[i]);
            }
            for (int i = 0; i < fieldListOriginal.Count; i++)
            {
                _ = strFilter.Replace(fieldListCustomExtension[i], "T1." + fieldListCustomExtension[i]);
            }

            IDBService dbService = Kingdee.BOS.App.ServiceHelper.GetService<IDBService>();
            tempTableNames = dbService.CreateTemporaryTableName(this.Context, 1);
            string strTable = tempTableNames[0];

            base.BuilderReportSqlAndTempTable(filter, strTable);

            if (!checkSome || checkUP < 2)
            {
                string strSql2 = string.Format(@"select T1.* into {0} from {1} T1 where 1 = 0",
                tableName, strTable);

                DBUtils.Execute(this.Context, strSql2);
                return;
            }

            //DBUtils.Execute(this.Context, "alter table " + tableName + " add F_anfu_Text nvarchar(255) ");
            //DBUtils.Execute(this.Context, "alter table " + strTable + " add F_anfu_Text nvarchar(255) ");

            string strSql = string.Format(@"/*dialect*/select T1.*, C.F_anfu_Text into {0} from {1} T1 join v_xsdd_zxmx_username_pwd_hth_plugin C on T1.FSALES = C.F_CHECK_NAME and T1.FBILLNO = C.FBILLNO where ({2})",
                tableName, strTable, strFilter);

            DBUtils.Execute(this.Context, strSql);


            //StringBuilder sqlStr = new StringBuilder();
            //sqlStr.AppendFormat(" MERGE INTO {0} T0 ", tableName);
            //sqlStr.AppendFormat(" using v_xsdd_zxmx_hth_plugin as T99");
            //sqlStr.AppendFormat(" on T0.FBILLNO = T99.FBILLNO ");
            //sqlStr.AppendFormat(" when matched then update set T0.F_anfu_Text = T99.hth ");
            //DBUtils.Execute(this.Context, sqlStr.ToString());
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

        //private void AddMnemonicCode(IRptParams filter, string tableName)
        //{

        //    var fieldListCustomExtension = new List<string>() { "F_VBDA_Text" };
        //    var fieldListOriginal = new List<string>();
        //    var strFilter = "";
        //    //if (CheckIsNeedChange(filter, fieldListCustomExtension, fieldListOriginal))
        //    //{
        //    StringBuilder sqlStr = new StringBuilder();
        //    sqlStr.AppendFormat("alter table {0} add F_VBDA_Text nvarchar(255) ", tableName);
        //    DBUtils.Execute(this.Context, sqlStr.ToString());

        //    sqlStr.Clear();
        //    sqlStr.AppendFormat(" MERGE INTO {0} T0 ", tableName);
        //    sqlStr.AppendFormat(" using test_view as T99");
        //    sqlStr.AppendFormat(" on T0.FBILLNO = T99.FBILLNO");
        //    sqlStr.AppendFormat(" when matched then update set F_VBDA_Text = T99.fid ");
        //    DBUtils.Execute(this.Context, sqlStr.ToString());

        //    sqlStr.Clear();
        //    strFilter = "";
        //    //strFilter = filter.FilterParameter.FilterString;
        //    //filter.FilterParameter.FilterString = " 2 > 1 ";

        //    //for (int i = 0; i < fieldListCustomExtension.Count; i++)
        //    //{
        //    //    _ = strFilter.Replace(fieldListCustomExtension[i], "T0." + fieldListCustomExtension[i]);
        //    //}

        //    //for (int i = 0; i < fieldListOriginal.Count; i++)
        //    //{
        //    //    _ = strFilter.Replace(fieldListOriginal[i], "T99." + fieldListOriginal[i]);
        //    //}

        //    sqlStr.AppendFormat(" delete {0} where not ( 1 = 1 {1})", tableName, strFilter);
        //    DBUtils.Execute(this.Context, sqlStr.ToString());

        //    base.BuilderReportSqlAndTempTable(filter, tableName);
        //    //}
        //    //else
        //    //{
        //    //    base.BuilderReportSqlAndTempTable(filter, tableName);
        //    //    return;
        //    //}

        //}

        //public override ReportHeader GetReportHeaders(IRptParams filter)
        //{
        //    ReportHeader header = base.GetReportHeaders(filter);
        //    header.AddChild("F_VBDA_Text", new LocaleValue("新增的列"));

        //    return base.GetReportHeaders(filter);
        //}

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
