using Kingdee.BOS;
using Kingdee.BOS.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Kingdee.FileServiceDemo
{
    [Description("上传附件到单据"), HotUpdate]
    public class 上传附件到单据
    {

    }

    //class FileService
    //{
    //    private const string WebSiteUrl = "http://192.168.31.153/k3cloud/"; //应用服务器地址。
    //    private readonly ApiClient _client = new ApiClient(WebSiteUrl);
    //    private string _fileId;
    //    private Context cloneCtx;

    //    public FileService(Context ctx)
    //    {
    //        cloneCtx = ctx;
    //    }

    //    private string GetUserToken()
    //    {
    //        //return cloneCtx.UserToken;
    //        string dbId = "610bbd142a6e15"; //数据中心内码。
    //        string username = "金清清"; //用户名。
    //        string password = "kingdee@123"; //密码。
    //        int lcid = 2052; //登录语言。
    //        object[] _objInfor = new object[] { dbId, username, password, lcid };

    //        //string loginResultString = _client.Login(dbId, username, password, lcid);
    //        string ret = _client.Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser", _objInfor);
    //        var result = JObject.Parse(ret)["LoginResultType"].Value<int>();
    //        if (result == 1) {
    //            JObject loginResult = JsonConvert.DeserializeObject<JObject>(ret);
    //            return loginResult["Context"]["UserToken"].ToString();
    //        }
    //        return "";
    //    }

    //    public void Upload()
    //    {
    //        //上传附件至金蝶云星空系统。
    //        string filePath = @".\123.txt"; //测试用附件地址。
    //        JObject uploadResult = this.UploadToWebSite(filePath);
    //        if (uploadResult == null)
    //        {
    //            throw new Exception("上传返回结果为空。");
    //        }
    //        //保存与单据的关联关系。
    //        string res = SaveBillData(uploadResult);
    //    }

    //    /// <summary>
    //    /// 传入文件物理路径，上传附件至当前启用的文件服务实例，返回上传结果。
    //    /// </summary>
    //    /// <param name="filePath">文件物理路径。</param>
    //    /// <returns>返回上传结果。</returns>
    //    private JObject UploadToWebSite(string filePath)
    //    {
    //        JObject uploadResult = null;
    //        string fileId = string.Empty; //文件内码。
    //        long blockSize = 1024 * 1024; //分块上传大小。
    //        string fileName = Path.GetFileName(filePath); //文件名。
    //        string token = GetUserToken(); //用户登录Token。

    //        using (FileStream fsRead = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    //        {
    //            if (fsRead.Length <= 0) throw new Exception("文件内容不允许为空。");
    //            while (true)
    //            {
    //                //放里面声明，第一次填入完整字节数据，第二次若只填充一部分，此时后面的直接数组并不会被清空。
    //                byte[] content = new byte[blockSize];
    //                int size = fsRead.Read(content, 0, content.Length);
    //                if (size == 0) break;
    //                byte[] uploadBytes = new byte[size];
    //                Array.Copy(content, uploadBytes, size);

    //                //不相等时说明已经是最后一次上传，也可能文件大小刚好等于1M，后面再读时size为0，不会再次上传。
    //                bool last = (size != blockSize);
    //                uploadResult = UploadToWebSite(fileName, fileId, token, last, uploadBytes);
    //                if (uploadResult == null) throw new Exception("上传返回结果为空。");

    //                _fileId = fileId = uploadResult["FileId"].ToString(); //获取返回的文件内码。
    //                bool success;
    //                Boolean.TryParse(uploadResult["Success"].ToString(), out success);
    //                if (!success) throw new Exception(uploadResult["Message"].ToString());
    //                if (last) break;
    //            }
    //        }
    //        return uploadResult;
    //    }

    //    private JObject UploadToWebSite(string fileName, string fileId, string token, bool last, byte[] content)
    //    {
    //        string url = string.Format(
    //            "{0}FileUpLoadServices/FileService.svc/upload2attachment/?fileName={1}&fileId={2}&token={3}&last={4}",
    //            WebSiteUrl, HttpUtility.HtmlEncode(fileName), fileId, token, last);
    //        string responseString = _client.UploadData(url, fileName, content);
    //        //string responseString = _client.UploadData(url, fileName, content);
    //        JObject loginResult = JsonConvert.DeserializeObject<JObject>(responseString);
    //        return loginResult["Upload2AttachmentResult"] as JObject;
    //    }

    //    private string SaveBillData(JObject uploadResult)
    //    {
    //        //业务对象唯一标识：附件明细。
    //        string formID = "BOS_Attachment";

    //        //Model数据包。
    //        JObject jsonRoot = new JObject();
    //        jsonRoot.Add("Creator", "Demo"); //创建用户。
    //        jsonRoot.Add("NeedUpDateFields", new JArray("")); //需要更新的字段：为空则表示全部。

    //        JObject model = new JObject(); //Model: 单据详细数据参数
    //        jsonRoot.Add("Model", model);
    //        model.Add("FID", 0); //内码：为零代表新增。
    //        model.Add("FBILLTYPE", "BD_Currency"); //业务单据唯一标识：此处关联币别。
    //        model.Add("FINTERID", 1); //业务单据内码：此处上传至人民币，内码为1。
    //        model.Add("FBILLNO", "PRE001"); //业务单据编码：此处上传至人民币，编码为PRE001。
    //        model.Add("FENTRYKEY", " "); //关联实体标识：单据头为空格，单据体则填单据体标识。
    //        model.Add("FENTRYINTERID", -1); //单据体内码：单据头为-1，单据体则填单据体内码。
    //        model.Add("FFILEID", uploadResult["FileId"].ToString()); //文件编码：上面上传成功拿到文件编码。
    //        model.Add("FFILESTORAGE", "1"); //存储类型：1为文件服务器、2为亚马逊云存储、3为金蝶·个人云存储、4为金蝶·企业云存储。
    //        string fileName = uploadResult["FileName"].ToString();
    //        model.Add("FATTACHMENTNAME", fileName); //附件名。
    //        model.Add("FEXTNAME", Path.GetExtension(fileName)); //文件后缀名。
    //        Decimal fileSize = Convert.ToDecimal(uploadResult["FileSize"].ToString());
    //        model.Add("FATTACHMENTSIZE", Math.Round(fileSize / 1024, 2)); //附件大小，单位为KB。
    //        model.Add("FBILLSTATUS", "A"); //单据状态：给默认值A即可。
    //        model.Add("FALIASFILENAME", ""); //别名。
    //        model.Add("FIsAllowDownLoad", false); //是否禁止下载：false代表允许下载。
    //        JObject userJObject = new JObject { { "FUSERID", 16394 } }; //管理员内码16394。
    //        model.Add("FCREATEMEN", userJObject); //创建人。
    //        model.Add("FCREATETIME", DateTime.Now); //创建时间。
    //        model.Add("FMODIFYMEN", userJObject); //修改人。
    //        model.Add("FMODIFYTIME", DateTime.Now); //修改时间。

    //        //调用保存接口。
    //        string res = _client.Save(formID, jsonRoot.ToString());
    //        return res;
    //    }

    //    public void Download()
    //    {
    //        string filePath = @".\124.txt"; //测试用附件下载地址。
    //        string token = GetUserToken(); //用户登录Token。
    //        string nail = "1"; //是否下载缩略图，1为缩略图，其余为原图。

    //        //fileId文件编码可由 T_BAS_ATTACHMENT 附件明细表查得，此处直接拿上面上传文件的编码来做示例。
    //        string url = string.Format("{0}FileUpLoadServices/Download.aspx?fileId={1}&token={2}&nail={3}",
    //            WebSiteUrl, _fileId, token, nail);
    //        using (WebClient webClient = new WebClient() { Encoding = Encoding.UTF8 })
    //        {
    //            webClient.DownloadFile(url, filePath); //下载保存物理文件方式。
    //        }
    //    }
    //}








    ///// <summary>
    ///// API调用客户端。
    ///// </summary>
    //public class ApiClient
    //{
    //    private readonly CookieContainer _cookieContainer;
    //    private string _webSiteUrl;
    //    private Context cloneCtx;

    //    //public ApiClient(string webSiteUrl)
    //    //{
    //    //    _cookieContainer = new CookieContainer();
    //    //    _webSiteUrl = webSiteUrl;
    //    //}

    //    public ApiClient(string webSiteUrl, Context ctx)
    //    {
    //        _cookieContainer = new CookieContainer();
    //        _webSiteUrl = webSiteUrl;
    //        cloneCtx = ctx;
    //    }
    //    /// <summary>
    //    /// 传入Url，创建并返回Http请求对象。
    //    /// </summary>
    //    /// <param name="url"></param>
    //    /// <returns>返回Http请求对象。</returns>
    //    private HttpWebRequest CreateHttpRequest(string url)
    //    {
    //        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
    //        //设置请求关联的Cookie。
    //        httpWebRequest.CookieContainer = _cookieContainer;
    //        httpWebRequest.Method = "POST";
    //        return httpWebRequest;
    //    }

    //    /// <summary>
    //    /// 传入文件上传地址、文件名及文件内容，上传文件并返回上传结果。
    //    /// </summary>
    //    /// <param name="url">文件上传地址。</param>
    //    /// <param name="filename">文件名。</param>
    //    /// <param name="data">文件内容。</param>
    //    /// <returns>返回上传结果。</returns>
    //    public string UploadData(string url, string filename, byte[] data)
    //    {
    //        HttpWebRequest httpWebRequest = CreateHttpRequest(url);
    //        httpWebRequest.ContentType = "application/octet-stream";
    //        httpWebRequest.ContentLength = data.Length;
    //        using (Stream requeStream = httpWebRequest.GetRequestStream())
    //        {
    //            requeStream.Write(data, 0, data.Length);
    //            requeStream.Flush();
    //        }
    //        using (Stream responseStream = httpWebRequest.GetResponse().GetResponseStream())
    //        using (StreamReader streamReader = new StreamReader(responseStream))
    //        {
    //            return streamReader.ReadToEnd();
    //        }
    //    }

    //    /// <summary>
    //    /// 传入文件下载地址，
    //    /// </summary>
    //    /// <param name="url"></param>
    //    /// <returns></returns>
    //    public byte[] DownloadData(string url)
    //    {
    //        List<byte> content = new List<byte>();
    //        byte[] buffer = new byte[1024 * 1024];
    //        HttpWebRequest httpWebRequest = CreateHttpRequest(url);
    //        httpWebRequest.Method = "GET";
    //        using (Stream responseStream = httpWebRequest.GetResponse().GetResponseStream())
    //        {
    //            int size;
    //            while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
    //            {
    //                byte[] tempBuffer = new byte[size];
    //                Array.Copy(buffer, tempBuffer, size);
    //                content.AddRange(tempBuffer);
    //            }
    //        }
    //        return content.ToArray();
    //    }

    //    /// <summary>
    //    /// 传入数据库ID、用户名、密码及登录语言，返回登录结果。
    //    /// </summary>
    //    /// <param name="dbId">数据库ID。</param>
    //    /// <param name="username">用户名。</param>
    //    /// <param name="password">密码。</param>
    //    /// <param name="lcid">登录语言。</param>
    //    /// <returns>返回登录结果。</returns>
    //    public string Login(string dbId, string username, string password, int lcid)
    //    {
    //        object[] data = new object[] { dbId, Encode(username), Encode(password), lcid };
    //        return Execute("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode", data);
    //    }

    //    /// <summary>
    //    /// 传入业务对象唯一标识及单据数据包，保存单据并返回保存结果。
    //    /// </summary>
    //    /// <param name="formId">业务对象唯一标识。</param>
    //    /// <param name="modelData">单据数据包</param>
    //    /// <returns>返回保存结果。</returns>
    //    public string Save(string formId, string modelData)
    //    {
    //        object[] data = new object[] { formId, modelData };
    //        return Execute("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Save", data);
    //    }

    //    private string Execute(string servicename, object[] parameters = null)
    //    {
    //        JObject jObj = new JObject();
    //        jObj.Add("format", 1);
    //        jObj.Add("useragent", "ApiClient");
    //        jObj.Add("rid", Guid.NewGuid().ToString().GetHashCode().ToString());
    //        jObj.Add("parameters", JsonConvert.SerializeObject(parameters));
    //        jObj.Add("timestamp", DateTime.Now);
    //        jObj.Add("v", "1.0");
    //        string content = jObj.ToString();
    //        byte[] data = Encoding.UTF8.GetBytes(content);

    //        string url = string.Concat(_webSiteUrl, servicename, ".common.kdsvc");
    //        HttpWebRequest httpWebRequest = CreateHttpRequest(url);
    //        httpWebRequest.ContentType = "application/json";
    //        using (Stream requeStream = httpWebRequest.GetRequestStream())
    //        {
    //            requeStream.Write(data, 0, data.Length);
    //            requeStream.Flush();
    //        }
    //        using (Stream responseStream = httpWebRequest.GetResponse().GetResponseStream())
    //        using (StreamReader streamReader = new StreamReader(responseStream))
    //        {
    //            string result = streamReader.ReadToEnd();
    //            return ValidateResult(result);
    //        }
    //    }

    //    private string Encode(object data)
    //    {
    //        string KEY_64 = "KingdeeK";
    //        string IV_64 = "KingdeeK";
    //        byte[] byKey = Encoding.ASCII.GetBytes(KEY_64);
    //        byte[] byIV = Encoding.ASCII.GetBytes(IV_64);
    //        byte[] buffer = null;
    //        int msleng = 0;

    //        using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            ICryptoTransform encryptor = cryptoProvider.CreateEncryptor(byKey, byIV);
    //            using (CryptoStream cst = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
    //            using (StreamWriter sw = new StreamWriter(cst))
    //            {
    //                sw.Write(data);
    //                sw.Flush();
    //                cst.FlushFinalBlock();
    //                sw.Flush();
    //                buffer = ms.GetBuffer();
    //                msleng = (int)ms.Length;
    //            }
    //        }
    //        return Convert.ToBase64String(buffer, 0, msleng);
    //    }

    //    private string ValidateResult(string responseText)
    //    {
    //        if (responseText.StartsWith("response_error:"))
    //        {
    //            throw new Exception(responseText);
    //        }
    //        return responseText;
    //    }
    //}
}
