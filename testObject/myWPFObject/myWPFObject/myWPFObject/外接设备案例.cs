//客制化控件集成读卡器代码实例：
using Kingdee.BOS.Client.Core;
using Kingdee.BOS;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

/// 有界面插件    ///     
namespace myWPFObject
{
    public class SmartCardReader : ContentControl, IKDCustomControl
    {
        public IKDCustomControlProxy Proxy { get; set; }
        TextBlock _txtDsp = null;

        protected void FireOnCustomEvent(CustomEventArgs e)
        {
            if (this.Proxy != null)
            {
                this.Proxy.FireCustomEvent(e);
            }
        }

        public void InitComponent()
        {
            var label = "This is Custom control in Kingdee.XPF.CustomControlPlugins, named SmartCardReader";
            this._txtDsp = new TextBlock()
            {
                ToolTip = label,
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Text = label
            };
            this.Content = new Border()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Blue),
                Child = this._txtDsp
            };
        }

        public void Release()
        {
            this.Content = null; }
        ///************************* 以下是客制化控件自定义函数入口 ****************************///        ///         
        /// 定制控件入口，服务端调用方法：this.View.GetControl("FCUSTOMCONTROL").InvokeControlMethod("DoCustomMethod","WriteString",args)，args是对象数组        ///        
        
        public void sumInt()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                int reInt = KDGWRFID.Sum3(3, 9);
                this._txtDsp.Text = reInt.ToString();
            }));
        }

        public void WriteString(string data)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (this._txtDsp != null)
                {
                    this._txtDsp.Text = data + "---"+ KDGWRFID.Sum3(3, 9);
                    // MessageBox.Show("用户名或者密码不能为空");
                    this.Proxy.FireCustomEvent(new CustomEventArgs("F_VBDA_CustomCtl", "Success", "{message:'WriteString() Success!'}"));
                }
                else
                {
                    this.Proxy.FireCustomEvent(new CustomEventArgs("F_VBDA_CustomCtl", "Error", "{message:'WriteString() Error, component uninitilization!'}"));
                }
            }));
        }

        ///// 获取Card ID        ///         ///      
        //public void ReadCardID()
        //{
        //    this.Dispatcher.BeginInvoke(new Action(() =>
        //    {
        //        try
        //        {
        //            var ret = KDGWRFID.ReadCardID();
        //            if (string.IsNullOrWhiteSpace(ret))
        //            {
        //                this.GetErr();
        //            }
        //            else
        //            {
        //                this.FireOnCustomEvent(new CustomEventArgs("", "ReadCardID", ret));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.FireOnCustomEvent(new CustomEventArgs("", "ReadCardID", ex.Message));
        //        }
        //    }
        //    )
        //    );
        //}        
        ///// 获取错误提示信息        ///         ///      
        //public void GetErr()
        //{
        //    this.Dispatcher.BeginInvoke(new Action(() =>
        //    {
        //        try
        //        {
        //            var ret = new StringBuilder();
        //            KDGWRFID.GetErr(ret);
        //            this.FireOnCustomEvent(new CustomEventArgs("", "GetErr", ret.ToString()));
        //        }
        //        catch (Exception ex)
        //        {
        //            this.FireOnCustomEvent(new CustomEventArgs("", "GetErr", ex.Message));
        //        }
        //    }));
        //}        ///         /// 连接卡        ///         ///     
        //public void LinkCard()
        //{
        //    this.Dispatcher.BeginInvoke(new Action(() =>
        //    {
        //        try
        //        {
        //            var ret = KDGWRFID.LinkCard();
        //            this.FireOnCustomEvent(new CustomEventArgs("", "LinkCard", ret.ToString()));
        //        }
        //        catch (Exception ex)
        //        {
        //            this.FireOnCustomEvent(new CustomEventArgs("", "LinkCard", ex.Message));
        //        }
        //    }));
        //}        ///         /// 脱离连接卡        ///         ///      
        //public void UnlinkCard()
        //{
        //    this.Dispatcher.BeginInvoke(new Action(() =>
        //    {
        //        try
        //        {
        //            var ret = KDGWRFID.UnlinkCard();
        //            this.FireOnCustomEvent(new CustomEventArgs("", "UnlinkCard", ret.ToString()));
        //        }
        //        catch (Exception ex)
        //        {
        //            this.FireOnCustomEvent(new CustomEventArgs("", "UnlinkCard", ex.Message));
        //        }
        //    }));
        //}
    }


    public static class KDGWRFID
    {
        [DllImport("C:/Program Files (x86)/Kingdee/K3Cloud/DeskClient/K3CloudClient/controlplugins/doNothingObject3.dll")]
        public static extern int Sum3(int a, int b);

    }

    //public static class KDGWRFID
    //{
    //    [DllImport("rfid.dll")]
    //    public static extern bool LinkCard();

    //    //初试化        
    //    [DllImport("rfid.dll")]
    //    public static extern bool UnlinkCard();  //初始化       

    //    [DllImport("rfid.dll")]
    //    public static extern int ReadCardID(StringBuilder sCardId);  //初始化    

    //    [DllImport("rfid.dll")]
    //    public static extern bool WriteCardData(int nBlock, StringBuilder sData, int sPassType, StringBuilder sPassWord);

    //    [DllImport("rfid.dll")]
    //    public static extern bool ReadCardData(StringBuilder sData, int nBlock, int sPassType, StringBuilder sPassWord);

    //    [DllImport("rfid.dll")]
    //    public static extern void GetErr(StringBuilder ErrStr);

    //    public static string ReadCardID()
    //    {
    //        string retStr = "";
    //        var retb = LinkCard();
    //        if (!retb) return retStr;
    //        var sb = new StringBuilder();
    //        var sbRet = new StringBuilder();
    //        var ret = ReadCardID(sb);
    //        if (ret == 0)
    //        {
    //            GetErr(sbRet);
    //        }
    //        else
    //        {
    //            var chrs = Encoding.Default.GetBytes(sb.ToString().Substring(0, ret));
    //            for (int i = 0; i < chrs.Length; i++)
    //            {
    //                var chr = chrs;
    //                var chrX2 = chr.ToString();
    //                //var chrX2 = chr.ToString("X2");
    //                sbRet.Append(chrX2);
    //            }
    //        }
    //        retStr = sbRet.ToString();
    //        retb = UnlinkCard();
    //        return retStr;
    //    }
    //}
}


