using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Kingdee.BOS;
using Kingdee.BOS.App;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core.Report;
using Kingdee.K3.SCM.App.Sal.Report;

namespace anfuObject
{
    [Description("销售出库毛利润报表干预插件")]
    [Kingdee.BOS.Util.HotUpdate]
    public class add_xsck_mlr_plugin : SalOutStockProfitAnalyseRpt
    {
        private string oldFilter = "";
        private string defFilter = " 22 = 22 ";
        private List<string> fieldListCustomExtension = new List<string>() { "F_VBDA_Text".ToUpper() };
        private List<string> fieldListOriginal = new List<string>();

        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            string strFilter = "";

            printRpt(filter, "BuilderReportSqlAndTempTable");
            cleanRpt(filter);

            string strTable = ServiceHelper.GetService<IDBService>().CreateTemporaryTableName(this.Context, 1)[0];

            base.BuilderReportSqlAndTempTable(filter, strTable);

            if (filter.FilterParameter.FilterString.Trim().Equals(""))
            {
                filter.FilterParameter.FilterString = defFilter;
            }

            if (!oldFilter.Equals(""))
            {
                filter.FilterParameter.FilterString = oldFilter;
            }

            strFilter = replaceSome(filter, fieldListCustomExtension, fieldListOriginal);

            string strSql = string.Format(@"/*dialect*/select T1.*, C.F_VBDA_Text, C.F_VBDA_Text1 into {0} from {1} T1 left join v_xscklr_plugin C on T1.FBILLNO = C.pluin_FBillNo and T1.FMATERIALID = C.pluin_FNUMBER where {2}",
            tableName, strTable, strFilter);

            DBUtils.Execute(this.Context, strSql);

            filter.FilterParameter.FilterString = defFilter; //oldFilter;
            oldFilter = "";
            fieldListCustomExtension = new List<string>() { "F_VBDA_Text".ToUpper() };
            fieldListOriginal = new List<string>();
        }

        private static bool CheckIsNeedChange(IRptParams filter, List<string> filedListCustomExtension,
            List<string> fieldListOriginal)
        {
            foreach (var t in filter.FilterParameter.ColumnInfo)
            {
                if (!filedListCustomExtension.Contains(t.Key.ToUpper()))
                {
                    fieldListOriginal.Add(t.Key.ToUpper());
                }

            }

            return filter.FilterParameter.FilterRows.Count != fieldListOriginal.Count;
        }

        private static string replaceSome(IRptParams filter, List<string> fcExtension,
            List<string> flOriginal)
        {
            string strFilter = filter.FilterParameter.FilterString.ToUpper();
            CheckIsNeedChange(filter, fcExtension, flOriginal);
            for (int i = 0; i < fcExtension.Count; i++)
            {
                _ = strFilter.Replace(fcExtension[i].ToUpper(), "C." + fcExtension[i].ToUpper());
            }
            for (int i = 0; i < flOriginal.Count; i++)
            {
                _ = strFilter.Replace(flOriginal[i].ToUpper(), "T1." + flOriginal[i].ToUpper());
            }
            flOriginal.Clear();
            return strFilter;
        }

        public override DataTable GetList(IRptParams filter)
        {
            printRpt(filter, "GetList");
            cleanRpt(filter);
            var ret = base.GetList(filter);
            if (!ret.Columns.Contains("F_VBDA_Text"))
            {
                ret.Columns.Add("F_VBDA_Text");
            }

            return ret;
        }

        protected override DataTable GetReportData(string tablename, IRptParams filter)
        {
            printRpt(filter, "GetReportData");
            cleanRpt(filter);
            return base.GetReportData(tablename, filter);
        }


        public override ReportTitles GetReportTitles(IRptParams filter)
        {
            printRpt(filter, "GetReportTitles");
            cleanRpt(filter);
            return base.GetReportTitles(filter);
        }

        private void cleanRpt(IRptParams filter)
        {
            if (filter.FilterParameter.FilterString.Contains("F_VBDA_Text"))
            {
                if (oldFilter.Equals(""))
                {
                    oldFilter = filter.FilterParameter.FilterString;
                }
                filter.FilterParameter.FilterString = defFilter;
            }
        }

        private void printRpt(IRptParams filter, string flagstr)
        {
            return;
            using (StreamWriter sw = new StreamWriter("C:\\Users\\Administrator\\Desktop\\1.txt", true))
            {
                sw.WriteLine(flagstr);
                sw.WriteLine();
                sw.WriteLine("filter.FilterParameter.FilterString");
                sw.WriteLine(filter.FilterParameter.FilterString.ToString());
                sw.WriteLine();
                sw.WriteLine("filter.FilterParameter.FilterRows");
                for (int idx = 0; idx < filter.FilterParameter.FilterRows.Count; idx++)
                {
                    sw.WriteLine(filter.FilterParameter.FilterRows[idx].Value.ToString());
                }
                sw.WriteLine();
                sw.WriteLine("filter.FilterParameter.ColumnInfo");
                for (int idx = 0; idx < filter.FilterParameter.ColumnInfo.Count; idx++)
                {
                    sw.WriteLine(filter.FilterParameter.ColumnInfo[idx].Key.ToString());
                }
                sw.WriteLine();
                sw.WriteLine("filter.FilterParameter.SelectedEntities");
                for (int idx = 0; idx < filter.FilterParameter.SelectedEntities.Count; idx++)
                {
                    sw.WriteLine(filter.FilterParameter.SelectedEntities[idx].Key.ToString());
                }
                sw.WriteLine();
                sw.WriteLine("filter.FilterFieldInfo.FilterFieldList");
                for (int idx = 0; idx < filter.FilterFieldInfo.FilterFieldList.Count; idx++)
                {
                    sw.WriteLine(filter.FilterFieldInfo.FilterFieldList[idx].Key.ToString());
                }
                sw.WriteLine();
                sw.WriteLine("filter.FilterFieldInfo.DspColumnFieldList");
                for (int idx = 0; idx < filter.FilterFieldInfo.DspColumnFieldList.Count; idx++)
                {
                    sw.WriteLine(filter.FilterFieldInfo.DspColumnFieldList[idx].Key.ToString());
                }
                sw.WriteLine();
                sw.WriteLine("filter.FilterFieldInfo.GroupFieldList");
                for (int idx = 0; idx < filter.FilterFieldInfo.GroupFieldList.Count; idx++)
                {
                    sw.WriteLine(filter.FilterFieldInfo.GroupFieldList[idx].Key.ToString());
                }
                if (filter.CustomParams.ContainsKey("F_VBDA_Text"))
                {
                    sw.WriteLine();
                    sw.WriteLine("filter.CustomParams[F_VBDA_Text]");
                    sw.WriteLine(filter.CustomParams["F_VBDA_Text"].ToString());
                }

                sw.WriteLine();
                sw.WriteLine("filter.FilterParameter.CustomFilter");
                sw.WriteLine(filter.FilterParameter.CustomFilter.ToString());

                if (filter.FilterParameter.CustomOption.ContainsKey("F_VBDA_Text"))
                {
                    sw.WriteLine();
                    sw.WriteLine(" filter.FilterParameter.CustomOption[F_VBDA_Text]");
                    sw.WriteLine(filter.FilterParameter.CustomOption["F_VBDA_Text"].ToString());
                }

            }
        }
    }
}
