using Kingdee.BOS;
using Kingdee.BOS.BusinessEntity.CloudPlatform;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Msg;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace myObject
{
    [Description("自动发送短信"), HotUpdate]
    public class send_mess_auto: IScheduleService
    {
        public void Run(Context ctx, Schedule schedule)
        {
            var phoneinfolist = new List<PhoneInfo>();
            var phoneinfo = new PhoneInfo();
            // 接收短信的手机号码
            phoneinfo.Phone = "15658510227";
            // 接收短信的用户（如果不是当前系统的用户，可不填）
            phoneinfo.ReceiverUser = ctx.UserId;
            phoneinfolist.Add(phoneinfo);
            var smsInfo = new CloudSMSInfo();
            // 短信内容
            smsInfo.SMSMessage = "老板,现在是法治社会,欠钱不能赖账";
            smsInfo.PhoneInfos = phoneinfolist;
            var result = SMSServiceHelper.SendMessage(ctx, smsInfo);
        }
    }
}
