using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingdee.BOS;
using Kingdee.BOS.Core.Bill.PlugIn;
using System.ComponentModel;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Core.DynamicForm;
using System.Data;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.DynamicForm.PlugIn.ControlModel;
using Kingdee.BOS.Resource;
using Kingdee.BOS.Util;
using Kingdee.BOS.ServiceHelper.Excel;
using Kingdee.BOS.JSON;
using Kingdee.BOS.Core.List.PlugIn;
using Kingdee.BOS.Core.List;

namespace Kingdee.Bos.OnLoad.List
{
    [Description("列表导入Excel文件")]

    [Kingdee.BOS.Util.HotUpdate]


    public class OnLoadFieldlb : AbstractDynamicFormPlugIn
    {
        DataTable dtexcel;
        private string _filePath;

        //自定义方法,引入的数据
        private void Impoort()
        {
            ExcelOperation excelOperation = new ExcelOperation();
            DataSet dataSet = null;
            dataSet = excelOperation.ReadFromFile(this._filePath, 0, 0);
            dtexcel = dataSet.Tables["Sheet1"];//读取的页名

            //i=1第二行开始读取:第一列订单号,第二列行号,第三列装箱单号
            for (int i = 1; i < dtexcel.Rows.Count; i++)
            {
                if (dtexcel.Rows[i][0].ToString() != "" && dtexcel.Rows[i][1].ToString() != "" && dtexcel.Rows[i][2].ToString() != "")
                {
                    DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/update b set fnote = '" + dtexcel.Rows[i][2].ToString() + "' from T_SAL_ORDER a, T_SAL_ORDERENTRY b where a.FID = b.FID and a.FDOCUMENTSTATUS = 'C' and a.FBILLNO = '" + dtexcel.Rows[i][0].ToString() + "' and b.FSEQ = " + dtexcel.Rows[i][1].ToString());
                }
                this.View.ShowMessage("执行成功请手动删除附件");
            }

        }



        //自定义方法,文件上传完毕触发的事件
        public override void CustomEvents(CustomEventsArgs e)
        {
            if (e.Key.EqualsIgnoreCase("FFileUpdate")) //上传附件的按钮标识
            {
                if (e.EventName.EqualsIgnoreCase("FILECHANGED"))
                {
                    JSONObject jSONObject = KDObjectConverter.DeserializeObject<JSONObject>(e.EventArgs);
                    if (jSONObject != null)
                    {
                        JSONArray jSONArray = new JSONArray(jSONObject["NewValue"].ToString());
                        if (jSONArray.Count > 0)
                        {
                            string text = (jSONArray[0] as Dictionary<string, object>)["ServerFileName"].ToString();
                            if (this.CheckFile(text))
                            {
                                this._filePath = this.GetFilePath(text);
                                Impoort();
                            }
                        }
                    }
                }
            }
            base.CustomEvents(e);
        }

        //自定义方法,判断是否是上传的是Excel文件
        private bool CheckFile(string fileName)
        {
            bool flag = false;
            string[] array = fileName.Split(new char[]
            {
                '.'
            });
            //通过后缀名,判断是否是Excel
            if (array.Length == 2 && (array[1].EqualsIgnoreCase("xls") || array[1].EqualsIgnoreCase("xlsx")))
            {
                flag = true;
            }
            if (!flag)
            {
                this.View.ShowWarnningMessage("请选择正确的文件进行引入");
            }
            return flag;
        }

        //自定义方法,获取上传路径
        private string GetFilePath(string serverFileName)
        {
            string directory = "FileUpLoadServices\\UploadFiles";
            return PathUtils.GetPhysicalPath(directory, serverFileName);
        }
    }
}





























//using System;
//using System.Collections.Generic;
////using System.Windows.Forms;
//using System.Linq;
//using System.Text;
//using Kingdee.BOS;
//using Kingdee.BOS.Core.Bill.PlugIn;
//using System.ComponentModel;
//using Kingdee.BOS.Core.DynamicForm.PlugIn;
//using Kingdee.BOS.Core.DynamicForm;
//using System.Data;
//using Kingdee.BOS.App.Data;
//using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
//using Kingdee.BOS.Core.DynamicForm.PlugIn.ControlModel;
//using Kingdee.BOS.Resource;
//using Kingdee.BOS.Util;
//using Kingdee.BOS.ServiceHelper.Excel;
//using Kingdee.BOS.JSON;
//using Kingdee.BOS.Core.List.PlugIn;
//using Kingdee.BOS.Core.List;

//namespace Kingdee.Bos.OnLoad.List
//{
//    [Description("列表导入Excel文件")]

//    [Kingdee.BOS.Util.HotUpdate]


//    public class OnLoadFieldlb : AbstractListPlugIn
//    {
//        DataTable dtexcel;


//        //点击导入Excel按钮
//        public override void BarItemClick(BOS.Core.DynamicForm.PlugIn.Args.BarItemClickEventArgs e)
//        {
//            base.BarItemClick(e);

//            if (e.BarItemKey == "tbOnload")
//            {
//                //当点击的按钮名字为FImportData
//                //this.View.ShowMessage("点了一下");
//                //OpenFileDialog dialog = new OpenFileDialog();
//                //dialog.Multiselect = true;//该值确定是否可以选择多个文件
//                //dialog.Title = "请选择文件";
//                //dialog.Filter = "所有文件(*.*)|*.*";
//                //if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
//                //{
//                //    this.View.ShowMessage("选择了文件路径：" + dialog.FileName);
//                //}

//                Impoort();
//                //判断是否有Excel表格
//                //if (this.IsNotNull())
//                //{
//                //    //数据的操作
//                //    Impoort();
//                //    this.View.ShowMessage("导入完成");
//                //    return;
//                //} else {
//                //    this.View.ShowMessage("没有附件");
//                //}
//            }

//        }



//        //自定义方法,判断是否上传Excel表格
//        private bool IsNotNull()
//        {
//            ExcelOperation excelOperation = new ExcelOperation();
//            DataSet dataSet = null;
//            dataSet = excelOperation.ReadFromFile("D://a1.xlsx", 0, 0);

//            //dataSet = excelOperation.ReadFromFile(this._filePath, 0, 0);
//            dtexcel = dataSet.Tables["Sheet1"];
//            if (dtexcel == null)
//            {
//                this.View.ShowMessage("没有找到模板请重新选择");
//                return false;

//            }
//            return true;
//        }


//        //自定义方法,引入的数据
//        private void Impoort()
//        {
//            ExcelOperation excelOperation = new ExcelOperation();
//            DataSet dataSet = null;
//            dataSet = excelOperation.ReadFromFile("D://a1.xlsx", 0, 0);
//            //在dataSet中获取一个Table的表中的数据
//            dtexcel = dataSet.Tables["Sheet1"];
//            string aa = "1122";
//            for (int i = 1; i < dtexcel.Rows.Count; i++)
//            {
//                if (dtexcel.Rows[i][0].ToString() != "")
//                {
//                    aa = aa + "--**--" + dtexcel.Rows[i][0].ToString();
//                    //更新销售出库单T_SAL_OUTSTOCK, 运单号F_YDH = 导入的运单号
//                    //sql = "/*dialect*/update T_SAL_OUTSTOCK set F_YDH = '" + dtexcel.Rows[i]["运单号"].ToString() + "',F_WLGS ='" + dtexcel.Rows[i]["物流公司"].ToString() + "' where FBillNo ='" + dtexcel.Rows[i]["单据编号"].ToString() + "'";
//                    //DBUtils.Execute(this.Context, sql);
//                }
//            }
//            this.View.ShowMessage(aa);
//        }
//    }

//}




////using System;
////using System.Data;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using Kingdee.BOS;
////using Kingdee.BOS.Core.Bill.PlugIn;
////using Kingdee.BOS.App.Data;
////using Kingdee.BOS.Orm.DataEntity;
////using Kingdee.BOS.Core.DynamicForm.PlugIn;
////using Kingdee.BOS.Core.DynamicForm.PlugIn.ControlModel;
////using Kingdee.BOS.Core.Metadata;
////using Kingdee.BOS.Util;
//////添加引用后,缩写函数
////using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
////using System.ComponentModel;


////namespace Kingdee.Bos.Project.BillDataChanged.plugln
////{
////    [Description("DataChanged值更新后触发")]
////    [Kingdee.BOS.Util.HotUpdate]

////    public class ClassName : AbstractBillPlugIn
////    {

////        //添加引用后,缩写函数
////        public override void DataChanged(DataChangedEventArgs e)
////        {
////            base.DataChanged(e);


////            //如果等于物料
////            if (e.Field.Key == "FMaterialId")
////            {
////                if (e.NewValue.ToString() == "")
////                {
////                    this.View.Model.SetValue("F_VBDA_Text", "", e.Row);
////                    return;
////                }
////                else
////                {
////                    //DataTable dt = DBUtils.ExecuteDataSet(this.Context, sql).Tables[0];
////                    DynamicObjectCollection dt = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select FMATERIALID, FNumber from T_BD_MATERIAL where FNUMBER = '1.01.001' and FDOCUMENTSTATUS = 'C' and FMATERIALID = " + e.NewValue.ToString());

////                    int i = 0;
////                    foreach (DynamicObject drItem in dt)
////                    {
////                        if (i == 0)
////                        {
////                            //给项目名称,赋值
////                            //e.Row 区别是哪一行,+动态变化
////                            this.View.Model.SetValue("F_VBDA_Text", "物料发生了变动" + drItem["FNUMBER"].ToString(), e.Row);
////                            //this.View.Model.SetValue("FEntryNote", "物料发生了变动", e.Row);
////                            this.View.Model.SetValue("F_VBDA_DECIMAL", drItem["FMATERIALID"], e.Row);

////                            this.View.GetFieldEditor("F_VBDA_DECIMAL", e.Row).SetEnabled("", false);
////                            this.View.GetFieldEditor("F_VBDA_Text", e.Row).SetEnabled("", true);
////                            this.View.GetFieldEditor("FTaxPrice", e.Row).SetEnabled("", false);
////                            //var entityKey = "FPOOrderEntry";
////                            //var entity = this.View.BusinessInfo.GetEntity(entityKey);
////                            //var fieldKey = this.Model.GetEntryCurrentFieldKey(entityKey);
////                            //var rowIndex = this.Model.GetEntryCurrentRowIndex(entityKey);
////                            //var rowData = this.Model.GetEntityDataObject(entity, rowIndex);

////                            //this.View.StyleManager.SetEnabled(fieldKey, rowData, "F_VBDA_Text", true);
////                            //this.View.StyleManager.SetEnabled(fieldKey, rowData, "F_VBDA_Decimal", false);
////                            this.View.ShowMessage("单元格已锁定！");                        
////                        }
////                        i++;
////                    }
////                    if (i == 0)
////                    {
////                        this.View.Model.SetValue("F_VBDA_Text", "", e.Row);
////                    }
////                }

////            }

////            if (e.Field.Key == "FTaxPrice")
////            {
////                if (e.NewValue.IsNullOrEmpty())
////                {
////                    return;
////                }
////                else
////                {
////                    if (!this.View.Model.GetValue("F_VBDA_Text", e.Row).IsNullOrEmpty())
////                    {
////                        this.View.ShowMessage("单元格已锁定！");
////                        //this.View.Model.SetValue("FTaxPrice", "99", e.Row);
////                        this.View.Model.SetValue("F_VBDA_DECIMAL", 99.0, e.Row);
////                        this.View.Model.SetValue("FPrice", 99.0, e.Row);
////                        this.View.ShowMessage("有字段不为空！");
////                        this.View.UpdateView("FTaxPrice", e.Row);
////                    }
////                }

////            }
////        }
////    }
////}

