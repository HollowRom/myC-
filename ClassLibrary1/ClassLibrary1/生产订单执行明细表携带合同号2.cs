using Kingdee.K3.MFG.PRD.App.ReportPlugIn.MOExecute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Util;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Report;

namespace Kingdee.K3.MFG.PRD.App.ReportPlugIn.MOExecute {
    [Description("生产订单执行明细表携带合同号2")]
    public class ReportService2 : MOExecuteDetailRpt {
        private string[] customRptTempTableNames;
        public override void BuilderReportSqlAndTempTable(IRptParams filter, string tableName)
        {
            base.BuilderReportSqlAndTempTable(filter, tableName);
            IDBService dbservice = Kingdee.BOS.App.ServiceHelper.GetService<IDBService>();
            customRptTempTableNames = dbservice.CreateTemporaryTableName(this.Context, 1);
            string strTable = customRptTempTableNames[0];
            //调用基类的方法，获取初步的查询结果到临时表          
            base.BuilderReportSqlAndTempTable(filter, strTable);
            //初步结果处理，然后回写积累的数据到临时表       
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT T1.*,MOE.F_VBDA_Text");
            sb.AppendFormat(" into {0} ", tableName);
            sb.AppendFormat(" FROM {0} T1", strTable);
            sb.AppendFormat(" LEFT JOIN T_PRD_MO MOE ON T1.FID=MOE.FID");
            DBUtils.Execute(this.Context, "DROP TABLE " + tableName);
            DBUtils.Execute(this.Context, sb.ToString());
        }
        public override void CloseReport()
        {
            if (customRptTempTableNames.IsNullOrEmptyOrWhiteSpace())
                return;
            IDBService dbservice = Kingdee.BOS.App.ServiceHelper.GetService<IDBService>();
            dbservice.DeleteTemporaryTableName(this.Context, customRptTempTableNames);
            base.CloseReport();
        }
    }
}


//Kingdee.K3.MFG.PRD.App.ReportPlugIn.MOExecute.MOExecuteDetailRpt, Kingdee.K3.MFG.PRD.App.ReportPlugIn