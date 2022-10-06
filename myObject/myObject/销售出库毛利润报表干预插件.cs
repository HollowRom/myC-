using System.Collections.Generic;
using System.ComponentModel;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core.Report;
using Kingdee.K3.SCM.App.Sal.Report;

namespace myObject
{
    [Description("销售出库毛利润报表干预插件")]
    [Kingdee.BOS.Util.HotUpdate]
    public class xsck_lr_plugin : SalOutStockProfitAnalyseRpt
    {
        private string[] tempTableNames;
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            List<string> fieldListCustomExtension = new List<string>() { "F_VBDA_Text".ToUpper() };
            List<string> fieldListOriginal = new List<string>();
            string strFilter = "";
            bool checkSome = CheckIsNeedChange(filter, fieldListCustomExtension, fieldListOriginal);
            //if (checkSome)
            //{
                strFilter = filter.FilterParameter.FilterString;
                filter.FilterParameter.FilterString = " 2 > 1 ";
            //}
           
            for (int i = 0; i < fieldListCustomExtension.Count; i++)
            {
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

            if (!checkSome)
            {
                string strSql2 = string.Format(@"select T1.* into {0} from {1} T1 where 1 = 0",
                tableName, strTable);

                DBUtils.Execute(this.Context, strSql2);
                return;
            }

            string strSql = string.Format(@"/*dialect*/select T1.*, C.F_VBDA_Text into {0} from {1} T1 left join v_xscklr_plugin C on T1.FBILLNO = C.FBILLNO where {2}",
            tableName, strTable, strFilter);

            DBUtils.Execute(this.Context, strSql);


            //base.BuilderReportSqlAndTempTable(filter, strTable);

            //DBUtils.Execute(this.Context, "alter table " + tableName + " add F_VBDA_Text nvarchar(255) ");

            //if (strFilter.Trim().Equals(""))
            //{
            //    strFilter = "1 = 1";
            //}
            
            //string strSql = string.Format(@"/*dialect*/select T1.*, C.F_VBDA_Text into {0} from {1} T1 left join v_xscklr_plugin C on T1.FBILLNO = C.FBILLNO where {2}",
            //    tableName, strTable, strFilter);

            //DBUtils.Execute(this.Context, strSql);
            
        }

        private static bool CheckIsNeedChange(IRptParams filter, List<string> filedListCustomExtension,
            List<string> fieldListOriginal)
        {
            foreach (var t in filter.FilterParameter.FilterRows)
            {
                string strFieldName = t.FilterField.FieldName;
                if (!filedListCustomExtension.Contains(strFieldName.ToUpper()))
                {
                    fieldListOriginal.Add(strFieldName.ToUpper());
                }

            }

            return filter.FilterParameter.FilterRows.Count != fieldListOriginal.Count;
        }
    }
}
