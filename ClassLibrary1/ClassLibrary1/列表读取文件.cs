using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kingdee.BOS;
using System.IO;
using Kingdee.BOS.Core.Bill.PlugIn;
using System.ComponentModel;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.FileServer.Core;
using Kingdee.BOS.FileServer.Core.Object;
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
    [Description("列表读取本地文件")]

    [Kingdee.BOS.Util.HotUpdate]


    public class ImportSeorderLB : AbstractListPlugIn
    {

        //点击导入Excel按钮
        public override void BarItemClick(BOS.Core.DynamicForm.PlugIn.Args.BarItemClickEventArgs e)
        {
            base.BarItemClick(e);

            if (e.BarItemKey == "tbOnload")
            {
                //当点击的按钮名字为FImportData
                //this.View.ShowMessage("点了一下");
                //OpenFileDialog dialog = new OpenFileDialog();
                //dialog.Multiselect = true;//该值确定是否可以选择多个文件
                //dialog.Title = "请选择文件";
                //dialog.Filter = "所有文件(*.*)|*.*";
                //if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    this.View.ShowMessage("选择了文件路径：" + dialog.FileName);
                //}

                Upload();
                //判断是否有Excel表格
                //if (this.IsNotNull())
                //{
                //    //数据的操作
                //    Impoort();
                //    this.View.ShowMessage("导入完成");
                //    return;
                //} else {
                //    this.View.ShowMessage("没有附件");
                //}
            }

        }

        public void Upload()
        {
            //上传附件至当前启用的文件服务实例。
            string filePath = @"D://a1.xlsx"; //测试用附件地址。
            FileUploadResult uploadResult = UploadAttachment(filePath);
            //this.View.ShowMessage(uploadResult.Message.ToString());
            this.View.ShowMessage(uploadResult.FileName.ToString());
            //保存与单据的关联关系，此处以币别为例。
            //SaveAttachmentData(uploadResult);
        }

        /// <summary>
                /// 上传附件，返回上传结果。
                /// </summary>
                /// <param name="filePath">应用服务器下测试文件路径。</param>
                /// <returns>返回上传结果。</returns>
        private FileUploadResult UploadAttachment(string filePath)
        {
            FileUploadResult uploadResult = new FileUploadResult();
            long blockSize = 1024 * 1024; //分块上传大小。
            string fileName = Path.GetFileName(filePath);

            //获取上传下载服务。
            IUpDownloadService service = FileServiceContainer.GetUpDownloadService();
            TFileInfo tFile = new TFileInfo()
            {
                FileId = string.Empty, //文件编码。
                FileName = fileName, //文件名。
                CTX = this.Context, //登录上下文环境。
            };
            using (FileStream fsRead = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (fsRead.Length <= 0) return uploadResult;
                while (true)
                {
                    //放里面声明，第一次填入完整字节数据，第二次若只填充一部分，此时后面的直接数组并不会被清空。
                    byte[] content = new byte[blockSize];
                    int size = fsRead.Read(content, 0, content.Length);
                    if (size == 0) break;

                    //不相等时说明已经是最后一次上传，也可能文件大小刚好等于1M，后面再读时size为0，不会再次上传。
                    tFile.Last = (size != blockSize);
                    tFile.Stream = new MemoryStream(content, 0, size);
                    uploadResult = service.UploadAttachment(tFile);
                    _fileId = tFile.FileId = uploadResult.FileId;
                    if (!uploadResult.Success || tFile.Last) break;
                }
            }
            return uploadResult;
        }


        private string _fileId = string.Empty;

















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

