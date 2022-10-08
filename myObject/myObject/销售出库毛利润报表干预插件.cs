using System.Collections.Generic;
using System.ComponentModel;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core.CommonFilter;
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
            List<string> fieldListCustomExtension = new List<string>() { "F_VBDA_TEXT" };
            List<string> fieldListOriginal = new List<string>();
            filter.FilterParameter.FilterString = filter.FilterParameter.FilterString.ToUpper();
            string strFilter = "";
            /*bool checkSome =*/ CheckIsNeedChange(filter, fieldListCustomExtension, fieldListOriginal);
            //if (!filter.FilterParameter.FilterString.ToUpper().Contains("F_VBDA_Text".ToUpper()))
            //{
            //    base.BuilderReportSqlAndTempTable(filter, tableName);
            //    return;
            //}

            //int flag = 1;
            //flag = flag / (flag - flag);

            strFilter = filter.FilterParameter.FilterString.ToUpper();
            //filter.FilterParameter = new FilterParameter();
            filter.FilterParameter.FilterString = " 12 = 12 ";
            //filter.FilterParameter.BatchFilterString = " 22 = 22 ";
            //filter.FilterParameter.QuickFilterString = " 22 = 22 ";
            //filter.FilterParameter.StatusFilterString = " 22 = 22 ";
            //filter.FilterParameter.GroupbyString = " 22 = 22 ";
            //filter.FilterParameter.SortString = " 22 = 22 ";



            for (int i = 0; i < fieldListCustomExtension.Count; i++)
            {
                _ = strFilter.Replace(fieldListCustomExtension[i].ToUpper(), "C." + fieldListCustomExtension[i].ToUpper());
            }
            for (int i = 0; i < fieldListOriginal.Count; i++)
            {
                _ = strFilter.Replace(fieldListCustomExtension[i].ToUpper(), "T1." + fieldListCustomExtension[i].ToUpper());
            }

            IDBService dbService = Kingdee.BOS.App.ServiceHelper.GetService<IDBService>();
            tempTableNames = dbService.CreateTemporaryTableName(this.Context, 1);
            string strTable = tempTableNames[0];

            //DBUtils.Execute(this.Context, "alter table " + strTable + " add F_VBDA_Text nvarchar(255) ");
            filter.FilterParameter.ColumnInfo.Clear();
            filter.FilterParameter.FilterRows.Clear();
            filter.FilterParameter.SortRows.Clear();


            filter.FilterFieldInfo.FilterFieldList.Clear();
            filter.FilterFieldInfo.DspColumnFieldList.Clear();
            //filter.FilterFieldInfo.DspColumnFieldKeyNameDic.Clear();


            //var arr = filter.FilterParameter.ColumnInfo;
            //for (int idx = 0; idx < arr.Count; idx++)
            //{
            //    if (arr[idx].FieldName.ToUpper().Contains("F_VBDA_TEXT"))
            //    {
            //        //int flag = 1;
            //        //flag = flag / (flag - flag);
            //        filter.FilterParameter.ColumnInfo.RemoveAt(idx);
            //    }
            //}

            base.BuilderReportSqlAndTempTable(filter, strTable);

            //if (strFilter.Contains("F_VBDA_TEXT"))
            //{
            //    int flag = 1;
            //    flag = flag / (flag - flag);
            //}
            

            //DBUtils.Execute(this.Context, "if not exists(select 1 from sysobjects a, syscolumns b where a.id = b.id and b.name = 'F_VBDA_Text' and a.type = 'u' and a.name = N'" + strTable + "') alter table " + strTable + " add F_VBDA_Text nvarchar(255)");

            //if (!checkSome)
            //{
            //    string strSql2 = string.Format(@"select T1.*,'' as F_VBDA_Text into {0} from {1} T1 where 1 = 0",
            //    tableName, strTable);

            //    DBUtils.Execute(this.Context, strSql2);
            //    return;
            //}

            if (strFilter.Trim() == "")
            {
                strFilter = " 22 = 22 ";
            }

            string strSql = string.Format(@"/*dialect*/select T1.*, C.F_VBDA_Text into {0} from {1} T1 left join v_xscklr_plugin C on T1.FBILLNO = C.FBILLNO where {2}",
            tableName, strTable, strFilter);

            //string strSql = string.Format(@"/*dialect*/select T1.* into {0} from {1} T1 left join v_xscklr_plugin C on T1.FBILLNO = C.FBILLNO where {2}",
            //tableName, strTable, strFilter);

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
