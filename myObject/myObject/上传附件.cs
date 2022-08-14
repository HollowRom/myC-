using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.Const;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.FileServer.Core;
using Kingdee.BOS.FileServer.Core.Object;
using Kingdee.BOS.JSON;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.ServiceHelper.FileServer;
using Kingdee.BOS.Util;

namespace Running.Sample.PlugIn.BusinessPlugIn.Bill
{
    [Description("附件上传下载的表单插件示例。"), HotUpdate]
    public class P20200427FileServiceEdit : AbstractBillPlugIn
    {
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            //上传附件，并保存于单据关联关系。
            if (e.Key.EqualsIgnoreCase("UploadLB"))
            {
                Upload();
            }
            //下载文件至应用服务器本地。
            //if (e.Key.EqualsIgnoreCase("Download"))
            //{
            //    Download();
            //}
            ////弹出下载提示窗，下载文件至客户端本地。
            //if (e.Key.EqualsIgnoreCase("ShowDownload"))
            //{
            //    ShowDownload();
            //}
        }

        /// <summary>
                /// 上传附件，并保存于单据关联关系。
                /// </summary>
        public void Upload()
        {
            //上传附件至当前启用的文件服务实例。
            string filePath = @"D:\123.txt"; //测试用附件地址。
            FileUploadResult uploadResult = UploadAttachment(filePath);
            this.View.ShowMessage(uploadResult.FileName);
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

        private void SaveAttachmentData(FileUploadResult uploadResult)
        {
            //构建附件明细实体数据包。
            FormMetadata meta = FormMetaDataCache.GetCachedFormMetaData(this.Context, FormIdConst.BOS_Attachment);
            DynamicObject dynObj = new DynamicObject(meta.BusinessInfo.GetDynamicObjectType());
            dynObj["BillType"] = "BD_Currency"; //业务单据唯一标识：此处关联币别。
            dynObj["InterID"] = 1; //业务单据内码：此处上传至人民币，内码为1。
            dynObj["BillNo"] = "PRE001"; //业务单据编码：此处上传至人民币，编码为PRE001。
            dynObj["EntryKey"] = " ";  //关联实体标识：单据头为空格，单据体则填单据体标识。
            dynObj["EntryInterID"] = -1; //单据体内码：单据头为-1，单据体则填单据体内码。
            dynObj["FileId"] = uploadResult.FileId; //文件编码：上面上传成功拿到文件编码。
            dynObj["FileStorage"] = "1"; //存储类型：1为文件服务器、2为亚马逊云存储、3为金蝶·个人云存储、4为金蝶·企业云存储。
            dynObj["AttachmentName"] = uploadResult.FileName; //附件名。
            dynObj["ExtName"] = Path.GetExtension(uploadResult.FileName); //文件后缀名。
            dynObj["AttachmentSize"] = Math.Round(uploadResult.FileSize * 1.0 / 1024, 2); //附件大小，单位为KB。
            dynObj["FBillStatus"] = "A"; //单据状态：给默认值A即可。
            dynObj["AliasFileName"] = ""; //别名。
            dynObj["IsAllowDownLoad"] = false; //是否禁止下载：false代表允许下载。
            dynObj["CreateMen_Id"] = this.Context.UserId; //创建人。
            dynObj["CreateTime"] = DateTime.Now; //创建时间。
            dynObj["ModifyMen_Id"] = this.Context.UserId; //修改人。
            dynObj["ModifyTime"] = DateTime.Now; //修改时间。

            //调用保存接口。
            dynObj = BusinessDataServiceHelper.Save(this.Context, dynObj);
        }

        private string _fileId = string.Empty;

        /// <summary>
                /// 下载文件至应用服务器本地。
                /// </summary>
        //public void Download()
        //{
        //    //下载至应用服务器的文件，建议放 WebSite/tempFilePath 目录，开启执行计划后定时会清空。
        //    string tempFilePath = HttpContext.Current.Server.MapPath(KeyConst.TEMPFILEPATH);
        //    string filePath = Path.Combine(tempFilePath, Guid.NewGuid() + ".txt"); //用GUID做文件名，防止同名冲突。

        //    IUpDownloadService service = FileServiceContainer.GetUpDownloadService();
        //    TFileInfo tFile = new TFileInfo()
        //    {
        //        //fileId文件编码可由 T_BAS_ATTACHMENT 附件明细表查得，此处直接拿上面上传文件的编码来做示例。
        //        FileId = _fileId,
        //        CTX = this.Context,
        //        Nail = "0", //为1代表缩略图
        //        FilePath = filePath //应用服务器保存文件的路径。
        //    };
        //    FileDownloadResult downloadResult = service.DownloadSaveLocal(tFile);
        //}

        /// <summary>
                /// 弹出下载提示窗，下载文件至客户端本地。
                /// </summary>
//        public void ShowDownload()
//        {
//            string url = FileServerHelper.GetAppSiteOuterNetUrl(this.Context, HttpContext.Current.Request);
//            //fileId文件编码可由 T_BAS_ATTACHMENT 附件明细表查得，此处直接拿上面上传文件的编码来做示例。
//            string fileurl = string.Format("{0}FileUpLoadServices/download.aspx?fileId={1}&token={2}",
//url, _fileId, this.Context.UserToken);

//            JSONObject jObject = new JSONObject();
//            jObject.Put("url", HttpUtility.UrlEncode(fileurl));
//            jObject.Put("title", "文件下载");
//            jObject.Put("desc", "请点击打开附件：");
//            jObject.Put("urltitle", "这是文件名");
//            this.View.AddAction(JSAction.openUrlWindow, new JSONArray { jObject });
//        }
    }
}

