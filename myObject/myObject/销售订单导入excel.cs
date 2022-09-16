//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.ComponentModel;
//using Kingdee.BOS.Core.DynamicForm.PlugIn;
//using System.Data;
//using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
//using Kingdee.BOS.Core.DynamicForm.PlugIn.ControlModel;
//using Kingdee.BOS.Util;
//using Kingdee.BOS.JSON;

//namespace Kingdee.Bos.OnLoad.List
//{
//    [Description("导入Excel文件")]

//    [Kingdee.BOS.Util.HotUpdate]


//    public class ImportSeorderEntry : AbstractDynamicFormPlugIn
//    {
//        DataTable dtexcel;
//        string sql;
//        private string _filePath;
//        private string _fileId = string.Empty;


//        //点击导入Excel按钮
//        public override void ButtonClick(ButtonClickEventArgs e)
//        {
//            base.ButtonClick(e);
//            string a;
//            //当点击事件不为空时
//            //不管大小写,都转换成大写
//            if ((a = e.Key.ToUpperInvariant()) != null)
//            {
//                //当点击的按钮名字为FImportData
//                if (a == "FIMPORTDATA")
//                {
//                    //判断是否有Excel表格
//                    if (this.IsNotNull())
//                    {
//                        string filePath = @"D:\123.txt"; //测试用附件地址。
//                        FileUploadResult uploadResult = UploadAttachment(filePath);
//                        this.View.ShowMessage(uploadResult.FileName);
//                        //数据的操作
//                        //Impoort();
//                        //this.View.ShowMessage("导入完成");
//                        return;
//                    }
//                }
//            }
//        }

//        private FileUploadResult UploadAttachment(string filePath)
//        {
//            FileUploadResult uploadResult = new FileUploadResult();
//            long blockSize = 1024 * 1024; //分块上传大小。
//            string fileName = Path.GetFileName(filePath);

//            //获取上传下载服务。
//            IUpDownloadService service = FileServiceContainer.GetUpDownloadService();
//            TFileInfo tFile = new TFileInfo()
//            {
//                FileId = string.Empty, //文件编码。
//                FileName = fileName, //文件名。
//                CTX = this.Context, //登录上下文环境。
//            };
//            using (FileStream fsRead = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
//            {
//                if (fsRead.Length <= 0) return uploadResult;
//                while (true)
//                {
//                    //放里面声明，第一次填入完整字节数据，第二次若只填充一部分，此时后面的直接数组并不会被清空。
//                    byte[] content = new byte[blockSize];
//                    int size = fsRead.Read(content, 0, content.Length);
//                    if (size == 0) break;

//                    //不相等时说明已经是最后一次上传，也可能文件大小刚好等于1M，后面再读时size为0，不会再次上传。
//                    tFile.Last = (size != blockSize);
//                    tFile.Stream = new MemoryStream(content, 0, size);
//                    uploadResult = service.UploadAttachment(tFile);
//                    _fileId = tFile.FileId = uploadResult.FileId;
//                    if (!uploadResult.Success || tFile.Last) break;
//                }
//            }
//            return uploadResult;
//        }


//        //自定义方法,判断是否上传Excel表格
//        private bool IsNotNull()
//        {
//            ExcelOperation excelOperation = new ExcelOperation();
//            DataSet dataSet = null;
//            dataSet = excelOperation.ReadFromFile(this._filePath, 0, 0);
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
//            //this.View.Model.CreateNewData();
//            //this.View.Model.SetValue("FNote", "aabb");
//            //this.View.ShowMessage("新建了表单");
//            //return;
//            ExcelOperation excelOperation = new ExcelOperation();
//            DataSet dataSet = null;
//            dataSet = excelOperation.ReadFromFile(this._filePath, 0, 0);
//            //在dataSet中获取一个Table的表中的数据
//            dtexcel = dataSet.Tables["Sheet1"];

//            //if (dtexcel.Rows[1][0].ToString() != "")
//            //{
//            //    this.View.ShowMessage("第一个识别的单元格为:" + dtexcel.Rows[1][0].ToString());
//            //}
//            //if (dtexcel.Rows[dtexcel.Rows.Count - 1][0].ToString() != "")
//            //{
//            //    this.View.ShowMessage("第一个识别的最后单元格为:" + dtexcel.Rows[dtexcel.Rows.Count - 1][0].ToString());
//            //}

//            for (int i = 1; i < dtexcel.Rows.Count; i++)
//            {
//                this.View.Model.CreateNewEntryRow("FSaleOrderEntry");
//                if (dtexcel.Rows[i][0].ToString() != "")
//                {
//                    this.View.Model.SetItemValueByNumber("FMaterialId", dtexcel.Rows[i][1].ToString(), i - 2);
//                    this.View.Model.SetValue("FQty", Convert.ToDouble(dtexcel.Rows[i][2]), i - 2);
//                }
//            }


//            //for (int i = 2; i < dtexcel.Rows.Count; i++)
//            //{
//            //    this.View.Model.CreateNewEntryRow("FSaleOrderEntry");
//            //    if (dtexcel.Rows[i][0].ToString() != "")
//            //    {
//            //        this.View.Model.SetItemValueByNumber("FMaterialId", dtexcel.Rows[i][1].ToString(), i - 2);
//            //        this.View.Model.SetValue("FQty", Convert.ToDouble(dtexcel.Rows[i][2]), i - 2);
//            //    }

//            //    //if (dtexcel.Rows[i][0].ToString() != "")
//            //    //{
//            //    //    //更新销售出库单T_SAL_OUTSTOCK, 运单号F_YDH = 导入的运单号
//            //    //    sql = "/*dialect*/update T_SAL_OUTSTOCK set F_YDH = '" + dtexcel.Rows[i]["运单号"].ToString() + "',F_WLGS ='" + dtexcel.Rows[i]["物流公司"].ToString() + "' where FBillNo ='" + dtexcel.Rows[i]["单据编号"].ToString() + "'";
//            //    //    DBUtils.Execute(this.Context, sql);
//            //    //}
//            //}
//            //this.View.ShowMessage("上传完成");
//        }



//        //自定义方法,文件上传完毕触发的事件
//        public override void CustomEvents(CustomEventsArgs e)
//        {
//            if (e.Key.EqualsIgnoreCase("FFileUpdate"))
//            {
//                if (e.EventName.EqualsIgnoreCase("FILECHANGED"))
//                {
//                    JSONObject jSONObject = KDObjectConverter.DeserializeObject<JSONObject>(e.EventArgs);
//                    if (jSONObject != null)
//                    {
//                        JSONArray jSONArray = new JSONArray(jSONObject["NewValue"].ToString());
//                        if (jSONArray.Count > 0)
//                        {
//                            string text = (jSONArray[0] as Dictionary<string, object>)["ServerFileName"].ToString();
//                            if (this.CheckFile(text))
//                            {
//                                this._filePath = this.GetFilePath(text);
//                                //上传按钮变成可用状态
//                                this.EnableButton("FImportData", true);
//                            }
//                        }
//                        else
//                        {
//                            this.EnableButton("FImportData", false);
//                        }
//                    }
//                }
//                else
//                {
//                    if (e.EventName.EqualsIgnoreCase("STATECHANGED"))
//                    {
//                        JSONObject jSONObject2 = KDObjectConverter.DeserializeObject<JSONObject>(e.EventArgs);
//                        if (jSONObject2["State"].ToString() != "2")
//                        {
//                            this.EnableButton("FImportData", false);
//                        }
//                    }
//                }
//            }
//            base.CustomEvents(e);
//        }



//        //自定义方法,判断是否是上传的是Excel文件
//        private bool CheckFile(string fileName)
//        {
//            bool flag = false;
//            string[] array = fileName.Split(new char[]
//            {
//                '.'
//            });
//            //通过后缀名,判断是否是Excel
//            if (array.Length == 2 && (array[1].EqualsIgnoreCase("xls") || array[1].EqualsIgnoreCase("xlsx")))
//            {
//                flag = true;
//            }
//            if (!flag)
//            {
//                this.View.ShowWarnningMessage("请选择正确的文件进行引入");
//            }
//            return flag;
//        }



//        //自定义方法,没有上传完成Excel,那么上传按钮是灰色的
//        private void EnableButton(string key, bool bEnable)
//        {
//            this.View.GetControl<Button>(key).Enabled = bEnable;
//        }



//        //自定义方法,获取上传路径
//        private string GetFilePath(string serverFileName)
//        {
//            string directory = "FileUpLoadServices\\UploadFiles";
//            return PathUtils.GetPhysicalPath(directory, serverFileName);
//        }
//    }
//}

