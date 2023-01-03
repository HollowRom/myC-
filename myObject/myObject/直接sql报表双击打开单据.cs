using Kingdee.BOS.Core.Bill;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.List;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Permission;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using Kingdee.BOS.Web.Report.SQLReport;
using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace myObject
{
    /// <summary>
    /// 【表单插件】直接SQL账表双击数据行打开新窗体
    /// </summary>
    [Description("直接SQL账表双击数据行打开新窗体"), HotUpdate]
    public class OpenListByRowDoubleClickFormPlugIn : AbstractDynamicFormPlugIn
    {
        public override void EntityRowDoubleClick(EntityRowClickEventArgs e)
        {
            base.EntityRowDoubleClick(e);
            var view = this.View as SQLReportView;
            if (view == null)
            {
                return;
            }

            if (e.ColKey.Equals("单据编号", StringComparison.OrdinalIgnoreCase))
            {
                // 只有在双击单据编号列时，才打开该编号对应的采购订单
                // 版本PT-146850  [7.5.1.202008]前用此方法
                //var currentRowValue = GetCurrentRowValue(view, "单据编号");
                // 版本PT-146850  [7.5.1.202008]后用此方法
                var currentRowValue = view.GetCurrentRowValue("单据编号");
                if (currentRowValue == null)
                {
                    return;
                }
                var billNo = currentRowValue.ToString();
                // 使用单据视图打开
                ShowBillForm(billNo);
                // 使用列表视图打开
                //ShowListForm(billNo);
            }
        }
               
        /// <summary>
        /// 使用单据视图打开
        /// </summary>
        /// <param name="billNo">单据编号</param>
        private void ShowBillForm(string billNo)
        {
            var sql = string.Format("SELECT FID FROM T_SAL_ORDER WHERE FBILLNO='{0}'", billNo);
            //var sql = string.Format("SELECT FID FROM T_PUR_POORDER WHERE FBILLNO='{0}'", billNo);
            
            var pkId = DBServiceHelper.ExecuteScalar<long>(this.Context, sql, 0);
            if (pkId <= 0)
            {
                this.View.ShowMessage("关联单据不存在");
                return;
            }
            var param = new BillShowParameter();
            //param.FormId = "PUR_PurchaseOrder"; // 业务对象标识
            param.FormId = "SAL_SaleOrder"; // 业务对象标识
            param.PKey = pkId.ToString(); // 单据内码
            param.Status = OperationStatus.VIEW; // 查看模式打开
            //param.Status = OperationStatus.EDIT; // 编辑模式打开
            this.View.ShowForm(param);
        }

        /// <summary>
        /// 使用列表视图打开
        /// </summary>
        /// <param name="billNo">单据编号</param>
        private void ShowListForm(string billNo)
        {
            var param = new ListShowParameter();
            param.FormId = "PUR_PurchaseOrder"; // 业务对象标识
            param.PermissionItemId = PermissionConst.View; // 验证当前用户查看权限
            param.OpenStyle.ShowType = ShowType.MainNewTabPage; // 主控台开新页签
            param.ListFilterParameter.Filter = string.Format("FBillNo='{0}'", billNo); // 查看指定单据
            this.View.ShowForm(param);
        }
               
        /// <summary>
        /// 获取当前选中行指定列的数据
        /// </summary>
        /// <param name="view"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private object GetCurrentRowValue(IDynamicFormView view, string columnName)
        {
            var currentPageData = GetValue(view, "currentPageData") as DataTable;
            if (currentPageData == null)
            {
                return null;
            }
            
            if (!currentPageData.Columns.Contains(columnName))
            {
                return null;
            }

            if (view.OpenParameter.GetCustomParameter("FLIST_selectedRows") == null)
            {
                return null;
            }
            var curRow = view.OpenParameter.GetCustomParameter("FLIST_selectedRows").ToString();
            // 反射获取当前账表数据包
            var rows = currentPageData.Select(string.Format("{0}={1}", "FIDENTITYID", curRow));
            if (rows.Length > 0)
            {
                return rows[0][columnName];
            }
            return null;
        }

        /// <summary>
        /// 反射获取当前视图的某个值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private object GetValue(object obj, string propertyName)
        {
            var field = typeof(SQLReportView).GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                return field.GetValue(obj);
            }                        
            return null;
        }
    }
}



