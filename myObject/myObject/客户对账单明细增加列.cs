// using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
// using Kingdee.BOS.Util;
// using Kingdee.K3.FIN.Business.PlugIn.ARAP;
// using System;
// using System.ComponentModel;
// using System.Data;
// using System.Linq;
// using Kingdee.BOS.App.Data;
// using Kingdee.BOS.Contracts;
// using Kingdee.K3.FIN.ServiceHelper;
//
// namespace myObject
// {
//     [Description("客户对账单明细增加列")]
//     [HotUpdate]
//     public class khdzd_add_col_dic : Statement
//     {
//         //private static string[] fieldNames = new[] { "插件文本", "插件小数" };
//         //private static string entityKey = "FBillList";
//
//         public override void SetListData(DataTable data)
//         {
//             data.Columns.Add(new DataColumn() { ColumnName = "F_VBDA_Decimal1", DataType = typeof(decimal) });
//             data.Columns.Add(new DataColumn() { ColumnName = "F_VBDA_Text", DataType = typeof(string) });
//             base.SetListData(data);
//
//
//             //data.Columns["F_VBDA_Decimal1"].SetOrdinal(0);
//             //data.Columns["F_VBDA_Text"].SetOrdinal(0);
//             //DynamicObjectCollection entityDataObject = Model.GetEntityDataObject(View.Model.BusinessInfo.GetEntity(FList));
//             //for (int i = 0; i < entityDataObject.Count; i++)
//             //{
//             //    //entityDataObject[i]["F_VBDA_Text"] = "2.364";
//             //    this.View.Model.SetValue("F_VBDA_Decimal1", /*Convert.ToDecimal(entityDataObject[i]["FQty"])*/ 2.364, i);
//             //    this.View.Model.SetValue("F_VBDA_Text", /*Convert.ToDecimal(entityDataObject[i]["FBillNo"])*/ "aabbddxcx", i);
//             //}
//             //this.View.ShowMessage(entityDataObject.Count.ToString());
//         }
//         
//     
//
//     // public override void BeforeBindData(EventArgs e)
//     // {
//     //     int id = 1;
//     //     id++;
//     //     base.BeforeBindData(e);
//     // }
//     //
//     // public override void AfterBindData(EventArgs e)
//     // {
//     //     int id = 1;
//     //     id++;
//     //     base.AfterBindData(e);
//     // }
//     //
//     // public override void OnInitialize(InitializeEventArgs e)
//     // {
//     //     int id = 1;
//     //     id++;
//     //     base.OnInitialize(e);
//     // }
//     //
//     // public override void OnPrepareNotePrintData(PreparePrintDataEventArgs e)
//     // {
//     //     int id = 1;
//     //     id++;
//     //     base.OnPrepareNotePrintData(e);
//     // }
//     //
//     // public override void DataChanged(DataChangedEventArgs e)
//     // {
//     //     int id = 1;
//     //     id++;
//     //     base.DataChanged(e);
//     // }
//     //
//     // public override void ProcessExportListData(DataTable listData)
//     // {
//     //     int id = 1;
//     //     id++;
//     //     base.ProcessExportListData(listData);
//     // }
//     //
//     // public override void ProcessExportSumData(DataTable listSumData)
//     // {
//     //     int id = 1;
//     //     id++;
//     //     base.ProcessExportSumData(listSumData);
//     // }
//     //
//     // public override string formatExportData(DataRow dr, string fieldKey)
//     // {
//     //     int id = 0;
//     //     id++;
//     //     return base.formatExportData(dr, fieldKey);
//     // }
//     //
//     // protected override void Refresh()
//     // {
//     //     int id = 0;
//     //     id++;
//     //     base.Refresh();
//     // }
//     //
//     // protected override void ShowFilter()
//     // {
//     //     int id = 0;
//     //     id++;
//     //     base.ShowFilter();
//     // }
//     //
//     // protected override string GetAgingTable()
//     // {
//     //     int id = 0;
//     //     id++;
//     //     return base.GetAgingTable();
//     // }
//     //
//     // public override void SetListSumData(DataTable data)
//     // {
//     //     int id = 0;
//     //     id++;
//     //     base.SetListSumData(data);
//     // }
//     //
//     // public override void BeforeSetItemValueByNumber(BeforeSetItemValueByNumberArgs e)
//     // {
//     //     int id = 0;
//     //     id++;
//     //     base.BeforeSetItemValueByNumber(e);
//     // }
//
//
//     //public override void OnInitialize(InitializeEventArgs e)
//     //{
//     //    for (int idx = 0; idx < View.BusinessInfo.GetEntryEntity(FList).Fields.Count; idx++)
//     //    {
//     //        if ("F_VBDA_Text".Equals(View.BusinessInfo.GetEntryEntity(FList).Fields[idx].ToString()))
//     //        {
//     //            View.BusinessInfo.GetEntryEntity(FList).Fields.RemoveAt(idx);
//     //            idx = 0;
//     //        }
//     //    }
//     //    base.OnInitialize(e);
//     //}
//
//     //private void addCols()
//     //{
//     //    var columnNames = this.View.Model.GetValue("F_VBDA_Text") as string;
//
//     //    if (!string.IsNullOrWhiteSpace(columnNames))
//     //    {
//     //        return;
//     //    }
//
//     //    var actionData = new JSONObject();
//     //    actionData["key"] = entityKey;
//     //    actionData["methodname"] = "CreateDyanmicList";
//
//     //    JSONArray coMeta = new JSONArray();
//     //    //添加需要创建的列
//     //    coMeta.Add(GetColumns());
//     //    coMeta.Add("");
//     //    actionData["args"] = coMeta;
//
//     //    this.View.AddAction("InvokeControlMethod", actionData, x =>
//     //    {
//     //        //单据体动态创建列之后通过回调方式为该单据体添加数据
//     //        var grid = this.View.GetControl<EntryGrid>(entityKey);
//     //        grid.SetData(GetEntityData());
//     //    });
//     //}
//     //private JSONObject GetColumns()
//     //{
//     //    // 生成构建表格的指令集
//     //    var entityAppearance = this.View.LayoutInfo.GetEntityAppearance(entityKey);
//     //    var gridMeta = entityAppearance.CreateControl();
//     //    gridMeta.Put("primaryKey", ExtConst.GRID_INDEX_FIELD_KEY);
//     //    gridMeta.Put("indexKey", ExtConst.GRID_INDEX_FIELD_KEY);
//     //    gridMeta.Put("startRow", 0);
//     //    gridMeta.Put("pageSize", entityAppearance.PageRows);
//
//     //    //gridMeta.Put("editable", false);
//     //    // 生成列
//     //    var columns = (JSONArray)gridMeta["columns"];
//
//     //    for (var i = 0; i < fieldNames.Length; ++i)
//     //    {
//     //        var fieldName = fieldNames[i];
//     //        var col = new JSONObject();
//     //        var xtype = "textfield";
//     //        col.Put("xtype", xtype);
//     //        col.Put("colIndex", i + 2);
//     //        col.Put("dataIndex", fieldName);
//     //        col.Put("header", fieldName);
//     //        col.Put("width", "300");
//     //        col.Put("visible", true);
//
//     //        // 如果需要某列的单元格可编辑，必须设置该列的编辑控件属性
//     //        // col.Put("editor", CreateEditorControl(xtype));
//     //        columns.Add(col);
//     //    }
//     //    return gridMeta;
//
//     //}
//
//     //private JSONObject CreateEditorControl(string xtype)
//
//     //{
//
//     //    var ctlRet = new JSONObject();
//
//     //    ctlRet.Put("xtype", xtype);
//
//     //    ctlRet.Put("editable", true);
//
//     //    return ctlRet;
//
//     //}
//
//     //private JSONObject GetEntityData()
//     //{
//     //    var data = new JSONObject();
//     //    var rows = new JSONArray();
//     //    for (var x = 1; x <= 20; ++x)
//     //    {
//     //        var row = new JSONArray();
//     //        row.Add(x);
//     //        row.Add(x);
//     //        foreach (var fieldName in fieldNames)
//     //        {
//     //            row.Add(string.Format("{0}：{1}", fieldName, Guid.NewGuid()));
//     //        }
//     //        rows.Add(row);
//     //    }
//     //    data.Put("rows", rows);
//     //    return data;
//
//     //}
// }
//
// }