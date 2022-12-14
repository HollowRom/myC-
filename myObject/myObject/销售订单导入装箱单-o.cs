using System.Collections.Generic;
using System.ComponentModel;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System.Data;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Util;
using Kingdee.BOS.ServiceHelper.Excel;
using Kingdee.BOS.JSON;

namespace myObject
{
    [Description("销售订单装箱单导入Excel文件"), HotUpdate]
    public class ImportZXDT0Seorder : AbstractDynamicFormPlugIn
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

