using Kingdee.BOS.BusinessEntity.CloudPlatform;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Msg;
using Kingdee.BOS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace myObject
{
    [Description("手工发送短信"), HotUpdate]
    public class send_mess_not_auto: AbstractDynamicFormPlugIn
    {
        public override void BarItemClick(BarItemClickEventArgs e)

        {

            base.BarItemClick(e);

            if (e.BarItemKey.Equals("VBDA_tbButton", StringComparison.OrdinalIgnoreCase))

            {

                SendMessage();

            }

        }



        private void SendMessage()

        {

            var phoneinfolist = new List<PhoneInfo>();

            var phoneinfo = new PhoneInfo();

            // 接收短信的手机号码

            phoneinfo.Phone = "18867710321";

            // 接收短信的用户（如果不是当前系统的用户，可不填）

            phoneinfo.ReceiverUser = this.Context.UserId;

            phoneinfolist.Add(phoneinfo);

            var smsInfo = new CloudSMSInfo();



            // 短信内容

            smsInfo.SMSMessage = "老板,现在是法治社会,欠钱不能赖账";

            smsInfo.PhoneInfos = phoneinfolist;

            var result = Kingdee.BOS.ServiceHelper.SMSServiceHelper.SendMessage(this.Context, smsInfo);

            if (result.Successful)

            {

                this.View.ShowMessage("短信发送成功！");

            }

        }
    }
}
