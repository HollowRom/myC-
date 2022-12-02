using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Bill;
using Kingdee.BOS.Core.CommonFilter;
using Kingdee.BOS.Core.DependencyRules;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.DynamicForm.PlugIn.ControlModel;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Metadata.EntityElement;
using Kingdee.BOS.Core.Metadata.FieldElement;
using Kingdee.BOS.Core.NotePrint;
using Kingdee.BOS.JSON;
using Kingdee.BOS.Model.CommonFilter;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Resource;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.ServiceHelper.Excel;
using Kingdee.BOS.Util;
using Kingdee.BOS.VerificationHelper;
using Kingdee.K3.FIN.Business.PlugIn.ARAP;
using Kingdee.K3.FIN.Core;
using Kingdee.K3.FIN.ServiceHelper;

namespace Kingdee.K3.FIN.AR.Report.PlugI.ARAP
{
    [Description("客户对账单明细增加列")]
    [HotUpdate]
    public class khdzd_add_col_dic : AbstractDynamicFormPlugIn
    {
        private const string newFieldName = "F_VBDA_Text";

        public string FList = "FBillList";

        public string FListSum = "FListSum";

        public string FSUMCURRENCY = "FSUMCURRENCY";

        protected string tempTable;

        protected FilterParameter filterPara;

        protected Dictionary<string, string> agingKeys;

        public Dictionary<string, string> baseFields = new Dictionary<string, string>();

        public Dictionary<string, string> contactFields = new Dictionary<string, string>();

        public List<string> dateFields = new List<string>();

        public List<string> amountFieldsFor = new List<string>();

        public List<string> amountFields = new List<string>();

        protected List<EnumItem> contactUnitList = new List<EnumItem>();

        protected int printingContactIndex;

        protected bool isPrintAll;

        public List<string> printedEntitys;

        private bool isfirstEntry = true;

        protected virtual string FilterFromId
        {
            get { return "AR_StatementFilter"; }
        }

        protected virtual string MatchFormID
        {
            get { return "AR_Matck"; }
        }

        public khdzd_add_col_dic()
        {
            baseFields.Add("FContactUnitID", "FContactUnitName");
            baseFields.Add("FCurrency", "FCurrencyName");
            baseFields.Add("FMasterCurrency", "FMasterCurrencyName");
            baseFields.Add("FSUMCURRENCY", "FCurrencyName");
            baseFields.Add("FUnit", "FUnitName");
            dateFields.Add("FDate");
            dateFields.Add("FEndDate");
            amountFieldsFor.Add("FAmountFor");
            amountFieldsFor.Add("FHadAmountFor");
            amountFieldsFor.Add("FBalanceAmtFor");
            amountFieldsFor.Add("FBalanceAmtForSum");
            amountFieldsFor.Add(newFieldName);
            amountFields.Add("FAmount");
            amountFields.Add("FHadAmount");
            amountFields.Add("FBalanceAmt");
            contactFields.Add("BD_Customer", "FCustomer");
            contactFields.Add("BD_Empinfo", "FEmpinfo");
            contactFields.Add("BD_Supplier", "FSupplier");
            contactFields.Add("BD_Department", "FDepartment");
            contactFields.Add("FIN_OTHERS", "FOthers");
        }

        public override void OnInitialize(InitializeEventArgs e)
        {
            base.OnInitialize(e);
            View.RuleContainer.AddPluginRule("FBillHead", RaiseEventType.ValueChanged,
                new Action<DynamicObject, object>(ChangeCustomer), "FCombo");
        }

        public override void EntityRowDoubleClick(EntityRowClickEventArgs e)
        {
            base.EntityRowDoubleClick(e);
            if (!e.Key.EqualsIgnoreCase(FList))
            {
                return;
            }

            object value = View.Model.GetValue("FFORMID", e.Row);
            object value2 = View.Model.GetValue("FID", e.Row);
            if (!value.IsNullOrEmptyOrWhiteSpace() && !value2.IsNullOrEmptyOrWhiteSpace())
            {
                if (!AccessServiceHelper.PermissionAuthExView(base.Context, View.OpenParameter.SubSystemId,
                        value.ToString(), 0L))
                {
                    View.ShowMessage(ResManager.LoadKDString("您没有当前单据的查看权限", "003227000018180", SubSystemType.FIN));
                    return;
                }

                BillShowParameter billShowParameter = new BillShowParameter();
                billShowParameter.FormId = value.ToString();
                billShowParameter.ParentPageId = View.PageId;
                billShowParameter.Status = OperationStatus.VIEW;
                billShowParameter.AllowNavigation = false;
                billShowParameter.OpenStyle.ShowType = ShowType.MainNewTabPage;
                billShowParameter.PKey = value2.ToString();
                View.ShowForm(billShowParameter);
            }
        }

        public override void BeforeClosed(BeforeClosedEventArgs e)
        {
            base.BeforeClosed(e);
            dropTempTable();
        }

        public void InitForPrintAll(string barItemKey)
        {
            printingContactIndex = 0;
            printedEntitys = new List<string>();
            if (filterPara == null)
            {
                isPrintAll = false;
            }
            else
            {
                isPrintAll = (barItemKey == "tbPrintAll" || barItemKey == "tbExportAll") &&
                             Convert.ToBoolean(filterPara.CustomFilter["FByContact"]) && contactUnitList.Count > 1;
            }
        }

        public override void BarItemClick(BarItemClickEventArgs e)
        {
            InitForPrintAll(e.BarItemKey);
            switch (e.BarItemKey)
            {
                case "tbFilter":
                    ShowFilter();
                    break;
                case "tbRefresh":
                    Refresh();
                    break;
                case "tbMatch":
                    ShowMatch(MatchFormID);
                    break;
                case "tbExport":
                    ExportData();
                    e.Cancel = true;
                    break;
                case "tbPrintAll":
                    if (filterPara == null)
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有可打印的数据！", "003192000020065", SubSystemType.FIN));
                        e.Cancel = true;
                    }
                    else if (Convert.ToBoolean(filterPara.CustomFilter["FByContact"]) && contactUnitList.Count > 0)
                    {
                        View.Model.SetValue("FCombo", contactUnitList[0].Value);
                        View.UpdateView("FCombo");
                    }
                    else if (!Convert.ToBoolean(filterPara.CustomFilter["FByContact"]) &&
                             View.Model.GetEntryRowCount(FList) <= 0)
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有可打印的数据！", "003192000020065", SubSystemType.FIN));
                        e.Cancel = true;
                    }
                    else if (Convert.ToBoolean(filterPara.CustomFilter["FByContact"]) && contactUnitList.Count <= 0)
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有可打印的数据！", "003192000020065", SubSystemType.FIN));
                        e.Cancel = true;
                    }

                    break;
                case "tbPrint":
                    if (View.Model.GetEntryRowCount(FList) <= 0)
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有可打印的数据！", "003192000020065", SubSystemType.FIN));
                        e.Cancel = true;
                    }

                    break;
                case "tbPrintView":
                    if (View.Model.GetEntryRowCount(FList) <= 0)
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有可预览的数据！", "003192000020066", SubSystemType.FIN));
                        e.Cancel = true;
                    }

                    break;
                case "tbSendMail":
                    LicenseVerifier.CheckViewOnlyOperation(base.Context,
                        ResManager.LoadKDString("发送邮件", "003227000038276", SubSystemType.FIN));
                    if (!AccessServiceHelper.PermissionAuthEx(base.Context, View.OpenParameter.SubSystemId,
                            View.Model.BillBusinessInfo.GetForm().Id, "545c6fb576151f", 0L))
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有发送邮件的权限", "003227000032890", SubSystemType.FIN));
                    }
                    else if (!AccessServiceHelper.PermissionAuthEx(base.Context, View.OpenParameter.SubSystemId,
                                 View.Model.BillBusinessInfo.GetForm().Id, "8dfa91ae26774d7ea46b29e29ecb3044", 0L))
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有打印的权限", "003192030037431", SubSystemType.FIN));
                    }
                    else
                    {
                        BeginSendEmail();
                    }

                    break;
                case "tbExportCurrent":
                    if (View.Model.GetEntryRowCount(FList) <= 0)
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有可套打导出的数据！", "003192000026687", SubSystemType.FIN));
                        e.Cancel = true;
                    }

                    break;
                case "tbExportAll":
                    if (filterPara == null)
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有可套打导出的数据！", "003192000026687", SubSystemType.FIN));
                        e.Cancel = true;
                    }
                    else if (Convert.ToBoolean(filterPara.CustomFilter["FByContact"]) && contactUnitList.Count > 0)
                    {
                        View.Model.SetValue("FCombo", contactUnitList[0].Value);
                        View.UpdateView("FCombo");
                    }
                    else if (!Convert.ToBoolean(filterPara.CustomFilter["FByContact"]) &&
                             View.Model.GetEntryRowCount(FList) <= 0)
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有可套打导出的数据！", "003192000026687", SubSystemType.FIN));
                        e.Cancel = true;
                    }
                    else if (Convert.ToBoolean(filterPara.CustomFilter["FByContact"]) && contactUnitList.Count <= 0)
                    {
                        View.ShowMessage(ResManager.LoadKDString("没有可套打导出的数据！", "003192000026687", SubSystemType.FIN));
                        e.Cancel = true;
                    }

                    break;
            }
        }

        private void BeginSendEmail()
        {
            if (tempTable.IsNullOrEmptyOrWhiteSpace() || filterPara == null)
            {
                View.ShowMessage(ResManager.LoadKDString("没有可发送的数据", "003227000022614", SubSystemType.FIN));
                return;
            }

            if (!Convert.ToBoolean(filterPara.CustomFilter["FByContact"]))
            {
                View.ShowMessage(ResManager.LoadKDString("发送邮件需要勾选过滤条件[按客户分页显示]", "003192030037400",
                    SubSystemType.FIN));
                return;
            }

            DynamicObjectCollection contactData = StatementServiceHelper.GetContactData(base.Context, tempTable);
            if (contactData == null || contactData.Count == 0)
            {
                View.ShowMessage(ResManager.LoadKDString("没有可发送的数据", "003227000022614", SubSystemType.FIN));
                return;
            }

            StatementSendMail statementSendMail = new StatementSendMail(View, tempTable, filterPara, contactData);
            statementSendMail.GetDefaultPrintTemplateId();
            if (statementSendMail.PrintTemplateId.Length <= 0)
            {
                View.ShowMessage(ResManager.LoadKDString("没有配置套打模板", "003192030037432", SubSystemType.FIN));
            }
            else
            {
                statementSendMail.Send();
            }
        }

        public override void BeforePrintExport(BeforePrintExportEventArgs e)
        {
            base.BeforePrintExport(e);
            if (!isPrintAll)
            {
                return;
            }

            List<PrintExportItem> exportItems = e.ExportInfo.ExportItems;
            if (exportItems.Count > 0)
            {
                List<PrintExportItem> list = new List<PrintExportItem>();
                for (int i = 0; i < contactUnitList.Count; i++)
                {
                    foreach (PrintExportItem item2 in exportItems)
                    {
                        PrintExportItem item = ObjectUtils.DeepClone(item2);
                        list.Add(item);
                    }
                }

                e.ExportInfo.ExportItems = list;
                return;
            }

            List<string> billIds = e.ExportInfo.BillIds;
            List<string> templateIds = e.ExportInfo.TemplateIds;
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            for (int j = 0; j < contactUnitList.Count; j++)
            {
                for (int k = 0; k < billIds.Count; k++)
                {
                    list2.Add(billIds[k]);
                    list3.Add(templateIds[k]);
                }
            }

            e.ExportInfo.BillIds = list2;
            e.ExportInfo.TemplateIds = list3;
        }

        public override void BeforeNotePrintCommand(BeforeNotePrintEventArgs e)
        {
            base.BeforeNotePrintCommand(e);
            if (!isPrintAll || e.PrintJobs == null || e.PrintJobs.Count == 0 || e.PrintJobs[0].PrintJobItems == null)
            {
                return;
            }

            List<PrintJobItem> printJobItems = e.PrintJobs[0].PrintJobItems;
            List<PrintJobItem> list = new List<PrintJobItem>();
            for (int i = 0; i < contactUnitList.Count; i++)
            {
                foreach (PrintJobItem item2 in printJobItems)
                {
                    PrintJobItem item = ObjectUtils.DeepClone(item2);
                    list.Add(item);
                }
            }

            e.PrintJobs[0].PrintJobItems = list;
        }

        public override void OnPrepareNotePrintData(PreparePrintDataEventArgs e)
        {
            base.OnPrepareNotePrintData(e);
            if (isPrintAll)
            {
                if (printedEntitys.Contains(e.DataSourceId))
                {
                    printedEntitys = new List<string>();
                    printingContactIndex++;
                    if (printingContactIndex == contactUnitList.Count - 1)
                    {
                        isPrintAll = false;
                    }

                    View.Model.SetValue("FCombo", contactUnitList[printingContactIndex].Value);
                    View.UpdateView("FCombo");
                }

                printedEntitys.Add(e.DataSourceId);
            }

            List<DynamicObject> list = new List<DynamicObject>();
            string dataSourceId;
            if ((dataSourceId = e.DataSourceId) != null && dataSourceId == "FBillHead")
            {
                List<DynamicObject> list2 = new List<DynamicObject>();
                list2.Add(View.Model.DataObject);
                list = list2;
            }
            else
            {
                list = (View.Model.DataObject[e.DataSourceId] as DynamicObjectCollection).ToList();
            }

            List<DynamicObject> list3 = new List<DynamicObject>();
            foreach (DynamicObject item in list)
            {
                DynamicObject dynamicObject = new DynamicObject(e.DynamicObjectType);
                foreach (object field in e.Fields)
                {
                    string text = Convert.ToString(field);
                    object obj = "";
                    if (text.Contains("."))
                    {
                        string text2 = text.Split('.')[0];
                        string text3 = text.Split('.')[1];
                        DynamicObject dynamicObject2 = (item.DynamicObjectType.Properties.Contains(text2)
                            ? (item[text2] as DynamicObject)
                            : null);
                        text = string.Format("{0}_{1}", text2, text3);
                        if (dynamicObject2 != null)
                        {
                            if (dynamicObject2.DynamicObjectType.Properties.Contains(text3.Remove(0, 1)))
                            {
                                obj = dynamicObject2[text3.Remove(0, 1)];
                            }
                            else if (dynamicObject2.DynamicObjectType.Properties.Contains(text3))
                            {
                                obj = dynamicObject2[text3];
                            }
                            else
                            {
                                obj = GetCustomerSubValue(dynamicObject2, text3, "BD_CUSTLOCATION");
                                if (Convert.ToString(obj).Trim().Length <= 0)
                                {
                                    obj = GetCustomerSubValue(dynamicObject2, text3, "BD_CUSTOMEREXT");
                                }

                                if (Convert.ToString(obj).Trim().Length <= 0)
                                {
                                    obj = GetCustomerSubValue(dynamicObject2, text3, "BD_CUSTBANK");
                                }

                                if (Convert.ToString(obj).Trim().Length <= 0)
                                {
                                    obj = GetCustomerSubValue(dynamicObject2, text3, "BD_CUSTCONTACT");
                                }

                                if (Convert.ToString(obj).Trim().Length <= 0)
                                {
                                    obj = GetCustomerContact(item, text3, "FContact");
                                }
                            }
                        }
                    }
                    else if (item.DynamicObjectType.Properties.Contains(text))
                    {
                        obj = ((!dateFields.Contains(text) || item[text] == null)
                            ? item[text]
                            : Convert.ToDateTime(item[text]).ToShortDateString());
                    }

                    if (obj is DynamicObject)
                    {
                        if ((obj as DynamicObject).DynamicObjectType.Properties.Contains("Name"))
                        {
                            dynamicObject[text] = Convert.ToString((obj as DynamicObject)["Name"]);
                        }
                    }
                    else
                    {
                        dynamicObject[text] = Convert.ToString(obj);
                    }
                }

                list3.Add(dynamicObject);
            }

            e.DataObjects = list3.ToArray();
        }

        private object GetCustomerSubValue(DynamicObject doSupplier, string baseFieldAttr, string subEntityName)
        {
            object result = null;
            if (doSupplier.DynamicObjectType.Properties.Contains(subEntityName))
            {
                DynamicObjectCollection dynamicObjectCollection = doSupplier[subEntityName] as DynamicObjectCollection;
                if (dynamicObjectCollection != null && dynamicObjectCollection.Count > 0)
                {
                    if (dynamicObjectCollection[0].DynamicObjectType.Properties.Contains(baseFieldAttr.Remove(0, 1)))
                    {
                        result = dynamicObjectCollection[0][baseFieldAttr.Remove(0, 1)];
                    }
                    else if (dynamicObjectCollection[0].DynamicObjectType.Properties.Contains(baseFieldAttr))
                    {
                        result = dynamicObjectCollection[0][baseFieldAttr];
                    }
                }
            }

            return result;
        }

        private object GetCustomerContact(DynamicObject doContact, string baseFieldAttr, string fieldName)
        {
            object result = null;
            if (doContact.DynamicObjectType.Properties.Contains(fieldName))
            {
                DynamicObject dynamicObject = doContact[fieldName] as DynamicObject;
                if (dynamicObject != null)
                {
                    if (dynamicObject.DynamicObjectType.Properties.Contains(baseFieldAttr.Remove(0, 1)))
                    {
                        result = dynamicObject[baseFieldAttr.Remove(0, 1)];
                    }
                    else if (dynamicObject.DynamicObjectType.Properties.Contains(baseFieldAttr))
                    {
                        result = dynamicObject[baseFieldAttr];
                    }
                }
            }

            return result;
        }

        protected void SetContactFieldValue(string contactUnitID)
        {
            if (contactUnitID.IsNullOrEmptyOrWhiteSpace())
            {
                return;
            }

            foreach (EnumItem contactUnit in contactUnitList)
            {
                if (contactUnit.Value == contactUnitID)
                {
                    if (contactFields.Keys.Contains(contactUnit.EnumId))
                    {
                        View.Model.SetValue(contactFields[contactUnit.EnumId], contactUnitID);
                        View.UpdateView(contactFields[contactUnit.EnumId]);
                    }

                    break;
                }
            }
        }

        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            string text;
            if ((text = e.Field.Key.ToUpperInvariant()) != null && text == "FCUSTOMER")
            {
                GetContactByCustomer();
            }
        }

        private void GetContactByCustomer()
        {
            object value = null;
            DynamicObject dataObject = Model.DataObject;
            if (dataObject.DynamicObjectType.Properties.ContainsKey("FCustomer"))
            {
                DynamicObject dynamicObject = dataObject["FCustomer"] as DynamicObject;
                if (dynamicObject != null && dynamicObject.DynamicObjectType.Properties.ContainsKey("BD_CUSTOMEREXT"))
                {
                    DynamicObjectCollection dynamicObjectCollection =
                        dynamicObject["BD_CUSTOMEREXT"] as DynamicObjectCollection;
                    if (dynamicObjectCollection != null && dynamicObjectCollection.Count > 0 &&
                        dynamicObjectCollection[0].DynamicObjectType.Properties.ContainsKey("DefaultContact_Id"))
                    {
                        value = dynamicObjectCollection[0]["DefaultContact_Id"];
                    }
                }
            }

            Model.SetValue("FContact", value);
            View.UpdateView("FContact");
        }

        protected void InitContactFields()
        {
            foreach (string value in contactFields.Values)
            {
                View.Model.SetValue(value, "");
                View.UpdateView(value);
            }
        }

        private void ChangeCustomer(DynamicObject row, dynamic dynamicRow)
        {
            InitContactFields();
            View.Model.DeleteEntryData(FList);
            View.Model.DeleteEntryData(FListSum);
            if (!tempTable.IsNullOrEmptyOrWhiteSpace())
            {
                string customerName = contactUnitList.First((EnumItem p) => p.Value == Convert.ToString(row["FCombo"]))
                    .Caption.ToString();
                SetContactFieldValue(Convert.ToString(row["FCombo"]));
                BuildEntityData(customerName);
            }
        }

        protected void ExportData()
        {
            if (filterPara == null ||
                (Convert.ToBoolean(filterPara.CustomFilter["FByContact"]) && contactUnitList.Count == 0))
            {
                return;
            }

            DataSet ds = BuildExportData();
            string text = "";
            string text2 = "";
            string text3 = "xlsx";
            SaveFileType result = SaveFileType.XLSX;
            string text4 = UserParamterServiceHelper.Load(View.Context,
                "ExportSetting" + View.BusinessInfo.GetForm().Id.ToUpperInvariant().GetHashCode(), View.Context.UserId);
            if (!text4.IsNullOrEmptyOrWhiteSpace())
            {
                JSONArray jsonArray = new JSONArray(text4);
                string valuebyKey = GetValuebyKey(jsonArray, "filetype");
                if (Enum.TryParse<SaveFileType>(valuebyKey, out result))
                {
                    switch (valuebyKey)
                    {
                        case "0":
                            text3 = "xlsx";
                            break;
                        case "1":
                            text3 = "xls";
                            break;
                        case "2":
                            text3 = "PDF";
                            break;
                        default:
                            text3 = "xlsx";
                            break;
                    }
                }
                else
                {
                    text3 = "xlsx";
                }
            }

            string fileName = string.Format("{0}_{1}_{2}.{3}", View.BusinessInfo.GetForm().Id,
                base.Context.UserLocale.LCID,
                TimeServiceHelper.GetSystemDateTime(base.Context).ToString("yyyyMMddHHmmssff"), text3);
            fileName = PathUtils.GetValidFileName(fileName);
            string path = "~" + PathUtils.GetServerPath(KeyConst.IMPORTTEMPLATE_PATH);
            string path2 = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }

            text = PathUtils.GetServerPath(KeyConst.IMPORTTEMPLATE_PATH, fileName);
            text2 = PathUtils.GetPhysicalPath(KeyConst.IMPORTTEMPLATE_PATH, fileName);
            if (!File.Exists(text2))
            {
                ExcelOperation excelOperation = new ExcelOperation(View);
                excelOperation.BeginExport();
                excelOperation.DateSetToExcel(ds, false);
                excelOperation.EndExport(text2, result);
            }

            DynamicFormShowParameter dynamicFormShowParameter = new DynamicFormShowParameter();
            dynamicFormShowParameter.FormId = "BOS_FileDownLoad";
            dynamicFormShowParameter.OpenStyle.ShowType = ShowType.Modal;
            dynamicFormShowParameter.CustomParams.Add("url", text);
            View.ShowForm(dynamicFormShowParameter);
        }

        protected DataSet BuildExportData()
        {
            List<string> fieldList = filterPara.ColumnInfo.Select((ColumnField item) => item.Key).ToList();
            Dictionary<string, string> dictionary = (from field in View.BusinessInfo.GetEntryEntity(FList).Fields
                                                     where fieldList.Contains(field.Key)
                                                     orderby field.Tabindex
                                                     select field).ToDictionary((Field p) => p.Key, (Field p) => p.Name.ToString());
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            dictionary2.Add("FSUMCURRENCY", View.BusinessInfo.GetField("FSUMCURRENCY").Name.ToString());
            dictionary2.Add("FBalanceAmtForSum", View.BusinessInfo.GetField("FBalanceAmtForSum").Name.ToString());
            foreach (KeyValuePair<string, string> agingKey in agingKeys)
            {
                dictionary2.Add(agingKey.Key, agingKey.Value);
            }

            int num = ((dictionary.Count >= dictionary2.Count) ? dictionary.Count : dictionary2.Count);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Clear();
            foreach (EnumItem contactUnit in contactUnitList)
            {
                DataTable dataTable = new DataTable(contactUnit.Caption.ToString());
                for (int i = 1; i <= num; i++)
                {
                    dataTable.Columns.Add(i.ToString(), Type.GetType("System.String"));
                }

                DataTable dataTable2 = ((!Convert.ToBoolean(filterPara.CustomFilter["FByContact"]))
                    ? StatementServiceHelper.GetDataByContactUnit(base.Context, tempTable, "")
                    : StatementServiceHelper.GetDataByContactUnit(base.Context, tempTable,
                        contactUnit.Caption.ToString()));
                ProcessExportListData(dataTable2);
                int num2 = 0;
                DataRow dataRow = dataTable.NewRow();
                foreach (KeyValuePair<string, string> item in dictionary)
                {
                    dataRow[num2] = item.Value.ToString();
                    num2++;
                }

                dataTable.Rows.Add(dataRow);
                for (int j = 0; j < dataTable2.Rows.Count; j++)
                {
                    dataRow = dataTable.NewRow();
                    num2 = 0;
                    using (Dictionary<string, string>.Enumerator enumerator4 = dictionary.GetEnumerator())
                    {
                        while (enumerator4.MoveNext())
                        {
                            dataRow[num2] = formatExportData(fieldKey: enumerator4.Current.Key, dr: dataTable2.Rows[j]);
                            num2++;
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }

                DataTable dataTable3 = ((!Convert.ToBoolean(filterPara.CustomFilter["FByContact"]))
                    ? StatementServiceHelper.GetSumDataByContactUnit(base.Context, tempTable, agingKeys.Keys.ToList(),
                        "")
                    : StatementServiceHelper.GetSumDataByContactUnit(base.Context, tempTable, agingKeys.Keys.ToList(),
                        contactUnit.Caption.ToString()));
                ProcessExportSumData(dataTable3);
                num2 = 0;
                dataRow = dataTable.NewRow();
                foreach (KeyValuePair<string, string> item2 in dictionary2)
                {
                    dataRow[num2] = item2.Value.ToString();
                    num2++;
                }

                dataTable.Rows.Add(dataRow);
                for (int k = 0; k < dataTable3.Rows.Count; k++)
                {
                    dataRow = dataTable.NewRow();
                    num2 = 0;
                    using (Dictionary<string, string>.Enumerator enumerator6 = dictionary2.GetEnumerator())
                    {
                        while (enumerator6.MoveNext())
                        {
                            dataRow[num2] = formatExportData(fieldKey: enumerator6.Current.Key, dr: dataTable3.Rows[k]);
                            num2++;
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }

        public virtual void ProcessExportListData(DataTable listData)
        {
        }

        public virtual void ProcessExportSumData(DataTable listSumData)
        {
        }

        public virtual string formatExportData(DataRow dr, string fieldKey)
        {
            if (dr[fieldKey].IsNullOrEmptyOrWhiteSpace())
            {
                return "";
            }

            decimal result = 0m;
            if (baseFields.Keys.Contains(fieldKey))
            {
                return Convert.ToString(dr[baseFields[fieldKey]]);
            }

            if (dr.Table.Columns.Contains(fieldKey) && decimal.TryParse(Convert.ToString(dr[fieldKey]), out result) &&
                result == 0m)
            {
                return "";
            }

            if (dateFields.Contains(fieldKey))
            {
                string text = Convert.ToDateTime(dr[fieldKey]).ToShortDateString();
                if (text == "0001-01-01")
                {
                    text = " ";
                }

                return text;
            }

            if (agingKeys.Keys.Contains(fieldKey) || amountFieldsFor.Contains(fieldKey))
            {
                return FINCommonFunc.GetAmountValueWithScale(dr[fieldKey], Convert.ToInt16(dr["FDigitsFor"]))
                    .ToString();
            }

            if (fieldKey == "FTaxPrice")
            {
                return FINCommonFunc.GetAmountValueWithScale(dr[fieldKey], Convert.ToInt16(dr["FPriceDigitsFor"]))
                    .ToString();
            }

            if (amountFields.Contains(fieldKey))
            {
                return FINCommonFunc.GetAmountValueWithScale(dr[fieldKey], Convert.ToInt16(dr["FDigits"])).ToString();
            }

            if (fieldKey == "FQty")
            {
                string text2 = "F" + Convert.ToInt16(dr["FQtyDigits"]);
                return decimal.Parse(dr["FQty"].ToString()).ToString(text2).Replace(",", "");
            }

            if (dr.Table.Columns.Contains(fieldKey))
            {
                return Convert.ToString(dr[fieldKey]);
            }

            return "";
        }

        protected void ShowMatch(string formID)
        {
            DynamicFormShowParameter dynamicFormShowParameter = new DynamicFormShowParameter();
            dynamicFormShowParameter.FormId = formID;
            dynamicFormShowParameter.ParentPageId = View.PageId;
            dynamicFormShowParameter.PageId = Guid.NewGuid().ToString();
            dynamicFormShowParameter.OpenStyle.ShowType = ShowType.MainNewTabPage;
            View.ShowForm(dynamicFormShowParameter);
        }

        protected virtual void Refresh()
        {
            if (filterPara == null)
            {
                return;
            }

            string oldSelectedValue = Convert.ToString(View.Model.GetValue("FCombo"));
            InitContactFields();
            View.Model.DeleteEntryData(FList);
            View.Model.DeleteEntryData(FListSum);
            tempTable = GetAgingTable();
            if (!tempTable.IsNullOrEmptyOrWhiteSpace())
            {
                string customerName = "";
                if (Convert.ToBoolean(filterPara.CustomFilter["FByContact"]))
                {
                    customerName = GetCustomerData(oldSelectedValue);
                }

                BuildEntityData(customerName);
            }
        }

        protected virtual void ShowFilter()
        {
            FilterShowParameter filterShowParameter = new FilterShowParameter();
            filterShowParameter.FormId = FilterFromId;
            filterShowParameter.ParentPageId = View.PageId;
            filterShowParameter.OpenStyle.CacheId = View.PageId;
            View.ShowForm(filterShowParameter, delegate (FormResult result)
            {
                object returnData = result.ReturnData;
                if (returnData is FilterParameter)
                {
                    filterPara = returnData as FilterParameter;
                    SetTitle();
                    InitBalanceKey(filterPara);
                    InitContactFields();
                    View.Model.DeleteEntryData(FList);
                    View.Model.DeleteEntryData(FListSum);
                    tempTable = GetAgingTable();
                    if (!tempTable.IsNullOrEmptyOrWhiteSpace())
                    {
                        string text = "";
                        text = GetCustomerData();
                        BuildEntityData(text);
                    }

                    HideColoumByFilter();
                    SetAgingVisible();
                    SetAgingTitle();
                }
            });
        }

        protected virtual string GetAgingTable()
        {
            dropTempTable();
            return StatementServiceHelper.GetAgingTable(base.Context, filterPara, View.BusinessInfo.GetForm().Id);
        }

        private void HideColoumByFilter()
        {
            List<string> list = (from item in View.BusinessInfo.GetEntryEntity(FList).Fields
                                 orderby item.Tabindex
                                 select item.Key).ToList();
            for (int idx = 0; idx < list.Count; idx++)
            {
                if (list[idx].Equals(newFieldName))
                {
                    list.RemoveAt(idx);
                }
            }
            List<string> list2 = filterPara.ColumnInfo.Select((ColumnField item) => item.Key).ToList();
            foreach (string item in list)
            {
                View.GetControl(item).Visible = false;
            }

            foreach (string item2 in list2)
            {
                View.GetControl(item2).Visible = true;
            }

            View.UpdateView(FList);
        }

        protected void SetTitle()
        {
            string text = filterPara.CustomFilter["SettleOrgLst"].ToString();
            if (!text.IsNullOrEmptyOrWhiteSpace())
            {
                View.Model.SetValue("FSettleOrg_H", StatementServiceHelper.GetOrgsName(base.Context, text));
            }

            DynamicObject dynamicObject = filterPara.CustomFilter["Affiliation"] as DynamicObject;
            if (dynamicObject != null)
            {
                View.Model.SetValue("FAffiliation_H", dynamicObject["Name"].ToString());
            }

            View.Model.SetValue("FDate_H", Convert.ToDateTime(filterPara.CustomFilter["ByDate"]).ToShortDateString());
            if (Convert.ToBoolean(filterPara.CustomFilter["FByContact"]))
            {
                View.GetControl("FCombo").Visible = true;
            }
            else
            {
                View.GetControl("FCombo").Visible = false;
            }
        }

        protected string GetCustomerData(string oldSelectedValue = "")
        {
            contactUnitList = new List<EnumItem>();
            View.GetControl<ComboFieldEditor>("FCombo").SetComboItems(contactUnitList);
            if (!Convert.ToBoolean(filterPara.CustomFilter["FByContact"]))
            {
                EnumItem enumItem = new EnumItem();
                enumItem.EnumId = "All";
                enumItem.Value = "";
                enumItem.Caption = new LocaleValue("All", base.Context.UserLocale.LCID);
                contactUnitList.Add(enumItem);
                return "";
            }

            DataTable customerData = StatementServiceHelper.GetCustomerData(base.Context, tempTable);
            if (customerData.Rows.Count > 0)
            {
                foreach (DataRow row in customerData.Rows)
                {
                    if (!row["ID"].IsNullOrEmptyOrWhiteSpace())
                    {
                        EnumItem enumItem2 = new EnumItem();
                        enumItem2.EnumId = Convert.ToString(row["Type"]);
                        enumItem2.Value = Convert.ToString(row["ID"]);
                        enumItem2.Caption =
                            new LocaleValue(Convert.ToString(row["Name"]), base.Context.UserLocale.LCID);
                        contactUnitList.Add(enumItem2);
                    }
                }
            }

            View.GetControl<ComboFieldEditor>("FCombo").SetComboItems(contactUnitList);
            if (contactUnitList.Count > 0)
            {
                string valueToSelect = contactUnitList[0].Value;
                if (!oldSelectedValue.IsNullOrEmptyOrWhiteSpace() &&
                    contactUnitList.Any((EnumItem item) => item.Value.Equals(oldSelectedValue)))
                {
                    valueToSelect = oldSelectedValue;
                }

                SetContactFieldValue(valueToSelect);
                View.Model.BeginIniti();
                View.Model.SetValue("FCombo", valueToSelect);
                View.Model.EndIniti();
                View.UpdateView("FCombo");
                return contactUnitList.First((EnumItem p) => p.Value == valueToSelect).Caption.ToString();
            }

            return "";
        }

        protected void BuildEntityData(string customerName)
        {
            DataTable dataByContactUnit =
                StatementServiceHelper.GetDataByContactUnit(base.Context, tempTable, customerName);
            DataTable sumDataByContactUnit =
                StatementServiceHelper.GetSumDataByContactUnit(base.Context, tempTable, agingKeys.Keys.ToList(),
                    customerName);
            if (dataByContactUnit.Rows.Count != 0 && sumDataByContactUnit.Rows.Count != 0)
            {
                View.Model.BeginIniti();
                SetListData(dataByContactUnit);
                SetListSumData(sumDataByContactUnit);
                View.Model.EndIniti();
                View.UpdateView(FList);
                View.UpdateView(FListSum);
            }
        }

        private void SetAgingVisible()
        {
            int num = 1;
            string text = "";
            Entity entity = View.BusinessInfo.GetEntity(FListSum);
            List<string> list = entity.Fields.Select((Field item) => item.Key).ToList();
            foreach (string key in agingKeys.Keys)
            {
                View.GetControl(key).Visible = true;
            }

            while (true)
            {
                text = string.Format("FBalance{0}AmtFor", num.ToString());
                if (!list.Contains(text))
                {
                    break;
                }

                if (!agingKeys.Keys.Contains(text))
                {
                    View.GetControl(text).Visible = false;
                }

                num++;
            }

            View.UpdateView(FListSum);
        }

        public virtual void SetListData(DataTable data)
        {
            List<string> list = View.BusinessInfo.GetEntryEntity(FList).Fields.Select((Field item) => item.Key)
                .ToList();
            List<object> list2 = new List<object>();
            List<object> list3 = new List<object>();
            List<object> list4 = new List<object>();
            List<object> list5 = new List<object>();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (!data.Rows[i]["FContactUnitID"].IsNullOrEmptyOrWhiteSpace() &&
                    Convert.ToString(data.Rows[i]["FContactUnitID"]) != "0" &&
                    !list2.Contains(data.Rows[i]["FContactUnitID"]))
                {
                    list2.Add(data.Rows[i]["FContactUnitID"]);
                }

                if (!data.Rows[i]["FCurrency"].IsNullOrEmptyOrWhiteSpace() &&
                    Convert.ToString(data.Rows[i]["FCurrency"]) != "0" && !list3.Contains(data.Rows[i]["FCurrency"]))
                {
                    list3.Add(data.Rows[i]["FCurrency"]);
                }

                if (!data.Rows[i]["FMasterCurrency"].IsNullOrEmptyOrWhiteSpace() &&
                    Convert.ToString(data.Rows[i]["FMasterCurrency"]) != "0" &&
                    !list4.Contains(data.Rows[i]["FMasterCurrency"]))
                {
                    list4.Add(data.Rows[i]["FMasterCurrency"]);
                }

                if (!data.Rows[i]["FUnit"].IsNullOrEmptyOrWhiteSpace() &&
                    Convert.ToString(data.Rows[i]["FUnit"]) != "0" && !list5.Contains(data.Rows[i]["FUnit"]))
                {
                    list5.Add(data.Rows[i]["FUnit"]);
                }
            }

            BaseDataField baseDataField = View.Model.BillBusinessInfo.GetField("FContactUnitID") as BaseDataField;
            BaseDataField baseDataField2 = View.Model.BillBusinessInfo.GetField("FCurrency") as BaseDataField;
            BaseDataField baseDataField3 = View.Model.BillBusinessInfo.GetField("FMasterCurrency") as BaseDataField;
            BaseDataField baseDataField4 = View.Model.BillBusinessInfo.GetField("FUnit") as BaseDataField;
            Dictionary<long, DynamicObject> dictionary = new Dictionary<long, DynamicObject>();
            Dictionary<long, DynamicObject> dictionary2 = new Dictionary<long, DynamicObject>();
            Dictionary<long, DynamicObject> dictionary3 = new Dictionary<long, DynamicObject>();
            Dictionary<long, DynamicObject> dictionary4 = new Dictionary<long, DynamicObject>();
            if (baseDataField != null && list2.Count > 0)
            {
                DynamicObject[] array = BusinessDataServiceHelper.LoadFromCache(base.Context, list2.ToArray(),
                    baseDataField.RefFormDynamicObjectType);
                if (array != null && array.Count() > 0)
                {
                    DynamicObject[] array2 = array;
                    foreach (DynamicObject dynamicObject in array2)
                    {
                        dictionary.AddWithoutExists(Convert.ToInt64(dynamicObject["Id"]), dynamicObject);
                    }
                }
            }

            if (baseDataField2 != null && list3.Count > 0)
            {
                DynamicObject[] array3 = BusinessDataServiceHelper.LoadFromCache(base.Context, list3.ToArray(),
                    baseDataField2.RefFormDynamicObjectType);
                if (array3 != null && array3.Count() > 0)
                {
                    DynamicObject[] array4 = array3;
                    foreach (DynamicObject dynamicObject2 in array4)
                    {
                        dictionary2.AddWithoutExists(Convert.ToInt64(dynamicObject2["Id"]), dynamicObject2);
                    }
                }
            }

            if (baseDataField3 != null && list4.Count > 0)
            {
                DynamicObject[] array5 = BusinessDataServiceHelper.LoadFromCache(base.Context, list4.ToArray(),
                    baseDataField3.RefFormDynamicObjectType);
                if (array5 != null && array5.Count() > 0)
                {
                    DynamicObject[] array6 = array5;
                    foreach (DynamicObject dynamicObject3 in array6)
                    {
                        dictionary3.AddWithoutExists(Convert.ToInt64(dynamicObject3["Id"]), dynamicObject3);
                    }
                }
            }

            if (baseDataField4 != null && list5.Count > 0)
            {
                DynamicObject[] array7 = BusinessDataServiceHelper.LoadFromCache(base.Context, list5.ToArray(),
                    baseDataField4.RefFormDynamicObjectType);
                if (array7 != null && array7.Count() > 0)
                {
                    DynamicObject[] array8 = array7;
                    foreach (DynamicObject dynamicObject4 in array8)
                    {
                        dictionary4.AddWithoutExists(Convert.ToInt64(dynamicObject4["Id"]), dynamicObject4);
                    }
                }
            }

            View.Model.BatchCreateNewEntryRow(FList, data.Rows.Count);
            for (int n = 0; n < data.Rows.Count; n++)
            {
                foreach (string item in list)
                {
                    if (item.Equals(newFieldName))
                    {
                        continue;
                    }

                    if (data.Rows[n][item].IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }

                    switch (item)
                    {
                        case "FContactUnitID":
                            if (dictionary.ContainsKey(Convert.ToInt64(data.Rows[n][item])))
                            {
                                View.Model.SetValue(item, dictionary[Convert.ToInt64(data.Rows[n][item])], n);
                                View.Model.SetValue(item + "_Id", data.Rows[n][item], n);
                            }

                            break;
                        case "FCurrency":
                            if (dictionary2.ContainsKey(Convert.ToInt64(data.Rows[n][item])))
                            {
                                View.Model.SetValue(item, dictionary2[Convert.ToInt64(data.Rows[n][item])], n);
                                View.Model.SetValue(item + "_Id", data.Rows[n][item], n);
                            }

                            break;
                        case "FMasterCurrency":
                            if (dictionary3.ContainsKey(Convert.ToInt64(data.Rows[n][item])))
                            {
                                View.Model.SetValue(item, dictionary3[Convert.ToInt64(data.Rows[n][item])], n);
                                View.Model.SetValue(item + "_Id", data.Rows[n][item], n);
                            }

                            break;
                        case "FUnit":
                            if (dictionary4.ContainsKey(Convert.ToInt64(data.Rows[n][item])))
                            {
                                View.Model.SetValue(item, dictionary4[Convert.ToInt64(data.Rows[n][item])], n);
                                View.Model.SetValue(item + "_Id", data.Rows[n][item], n);
                            }

                            break;
                        default:
                            if (data.Columns.Contains(item))
                            {
                                View.Model.SetValue(item, data.Rows[n][item], n);
                            }

                            break;
                    }
                }
            }
            Random random = new Random();

            DynamicObjectCollection entityDataObject = Model.GetEntityDataObject(View.Model.BusinessInfo.GetEntity(FList));
            for (int i = 0; i < entityDataObject.Count; i++)
            {
                DynamicObjectCollection dt = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select hth from hl_ysd_hth_v_plugin where FBILLNO = '" + Convert.ToString(entityDataObject[i]["FBillNo"]) + "'");
                if (dt.Count < 1)
                {
                    continue;
                }
                this.View.Model.SetValue(newFieldName, dt[0]["hth"], i);
            }
        }

        public virtual void SetListSumData(DataTable data)
        {
            for (int i = 0; i < data.Rows.Count; i++)
            {
                View.Model.CreateNewEntryRow(FListSum);
                View.Model.SetValue("FSUMCURRENCY", data.Rows[i]["FSUMCURRENCY"], i);
                View.Model.SetValue("FBalanceAmtForSum", data.Rows[i]["FBalanceAmtForSum"], i);
                foreach (string key in agingKeys.Keys)
                {
                    View.Model.SetValue(key, data.Rows[i][key], i);
                }
            }
        }

        private void SetAgingTitle()
        {
            foreach (string key in agingKeys.Keys)
            {
                View.GetControl(key).Text = agingKeys[key];
            }
        }

        private void InitBalanceKey(FilterParameter filterObj)
        {
            agingKeys = new Dictionary<string, string>();
            Entity entity = View.BusinessInfo.GetEntity(FListSum);
            List<string> list = entity.Fields.Select((Field item) => item.Key).ToList();
            DynamicObjectCollection dynamicObjectCollection =
                filterObj.CustomFilter["EntAgingGrpSetting"] as DynamicObjectCollection;
            if (dynamicObjectCollection == null || dynamicObjectCollection.Count == 0 ||
                (dynamicObjectCollection.Count == 1 &&
                 dynamicObjectCollection[0]["Section"].IsNullOrEmptyOrWhiteSpace()))
            {
                agingKeys.Add("FBalance1AmtFor",
                    string.Format("（{0}）{1}", ResManager.LoadKDString("原币", "003224000002077", SubSystemType.FIN),
                        ResManager.LoadKDString("0-30天", "003246000003337", SubSystemType.FIN)));
                agingKeys.Add("FBalance2AmtFor",
                    string.Format("（{0}）{1}", ResManager.LoadKDString("原币", "003224000002077", SubSystemType.FIN),
                        ResManager.LoadKDString("31-60天", "003246000003340", SubSystemType.FIN)));
                agingKeys.Add("FBalance3AmtFor",
                    string.Format("（{0}）{1}", ResManager.LoadKDString("原币", "003224000002077", SubSystemType.FIN),
                        ResManager.LoadKDString("61-90天", "003246000003343", SubSystemType.FIN)));
                agingKeys.Add("FBalance4AmtFor",
                    string.Format("（{0}）{1}", ResManager.LoadKDString("原币", "003224000002077", SubSystemType.FIN),
                        ResManager.LoadKDString("90天以上", "003246000003346", SubSystemType.FIN)));
                return;
            }

            int i = 0;
            int num = 1;
            for (; i < dynamicObjectCollection.Count; i++)
            {
                string text = dynamicObjectCollection[i]["Section"] as string;
                if (!text.IsNullOrEmptyOrWhiteSpace())
                {
                    if (!list.Contains(string.Format("FBalance{0}AmtFor", num.ToString())))
                    {
                        break;
                    }

                    agingKeys.Add(string.Format("FBalance{0}AmtFor", num.ToString()),
                        string.Format("（{0}）{1}", ResManager.LoadKDString("原币", "003224000002077", SubSystemType.FIN),
                            text));
                    num++;
                }
            }
        }

        private void dropTempTable()
        {
            if (!tempTable.IsNullOrEmptyOrWhiteSpace())
            {
                StatementServiceHelper.dropTempTable(base.Context, tempTable);
                tempTable = "";
            }
        }

        public override void AuthPermissionBeforeF7Select(AuthPermissionBeforeF7SelectEventArgs e)
        {
            base.AuthPermissionBeforeF7Select(e);
            e.IsIsolationOrg = false;
        }

        public override void BeforeF7Select(BeforeF7SelectEventArgs e)
        {
            base.BeforeF7Select(e);
            e.IsShowUsed = false;
            e.IsShowApproved = false;
        }

        public override void BeforeSetItemValueByNumber(BeforeSetItemValueByNumberArgs e)
        {
            base.BeforeSetItemValueByNumber(e);
            e.IsShowUsed = false;
            e.IsShowApproved = false;
        }

        public override void BeforeBindData(EventArgs e)
        {
            base.BeforeBindData(e);
            if (!isfirstEntry)
            {
                return;
            }

            string id = View.Model.BusinessInfo.GetForm().Id;
            FilterModel filterModel = new FilterModel(base.Context);
            FormMetadata formMetadata = MetaDataServiceHelper.Load(base.Context, "AR_StatementFilter") as FormMetadata;
            BusinessInfo businessInfo = formMetadata.BusinessInfo;
            filterModel.SetContext(base.Context, businessInfo, null);
            filterModel.FormId = id;
            filterModel.GetSchemeList();
            FilterScheme nextDirectEnterScheme = filterModel.GetNextDirectEnterScheme();
            if (nextDirectEnterScheme == null || nextDirectEnterScheme.IsDefault)
            {
                isfirstEntry = false;
                return;
            }

            filterModel.Load(nextDirectEnterScheme.Id);
            filterPara = filterModel.GetFilterParameter();
            List<ColumnField> filterByColumnSetting = GetFilterByColumnSetting(filterPara);
            filterPara.ColumnInfo = filterByColumnSetting;
            SetTitle();
            InitBalanceKey(filterPara);
            InitContactFields();
            View.Model.DeleteEntryData(FList);
            View.Model.DeleteEntryData(FListSum);
            tempTable = GetAgingTable();
            if (!tempTable.IsNullOrEmptyOrWhiteSpace())
            {
                string text = "";
                text = GetCustomerData();
                BuildEntityData(text);
            }

            HideColoumByFilter();
            SetAgingVisible();
            isfirstEntry = false;
        }

        public override void AfterBindData(EventArgs e)
        {
            base.AfterBindData(e);
            if (agingKeys != null && agingKeys.Keys.Count > 0)
            {
                SetAgingTitle();
            }
        }

        private List<ColumnField> GetFilterByColumnSetting(FilterParameter filterPara)
        {
            List<ColumnField> list = new List<ColumnField>();
            if (filterPara.SchemeEntity != null && filterPara.SchemeEntity.ColumnSetting != null)
            {
                JSONArray jSONArray = new JSONArray(filterPara.SchemeEntity.ColumnSetting);
                for (int i = 0; i < jSONArray.Count; i++)
                {
                    Dictionary<string, object> dictionary = (Dictionary<string, object>)jSONArray[i];
                    ColumnField columnField = new ColumnField();
                    if (dictionary.ContainsKey("F"))
                    {
                        columnField.Key = dictionary.GetString("F");
                    }

                    if (dictionary.ContainsKey("V"))
                    {
                        columnField.Visible = (int)dictionary["V"] == 1;
                    }
                    else
                    {
                        columnField.Visible = true;
                    }

                    columnField.ColIndex = i;
                    if (columnField.Visible && !columnField.Key.IsNullOrEmptyOrWhiteSpace())
                    {
                        list.Add(columnField);
                    }
                }
            }

            return list;
        }

        private string GetValuebyKey(JSONArray jsonArray, string key)
        {
            string result = string.Empty;
            if (jsonArray == null)
            {
                return result;
            }

            for (int i = 0; i < jsonArray.Count; i++)
            {
                Dictionary<string, object> dictionary = jsonArray[i] as Dictionary<string, object>;
                if (dictionary["key"] != null && dictionary["key"].ToString() == key)
                {
                    result = ObjectUtils.Object2String(dictionary["value"]);
                    break;
                }
            }

            return result;
        }
    }
}