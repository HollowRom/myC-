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
            //phoneinfo.ReceiverUser = ctx.UserId;
            phoneinfolist.Add(phoneinfo);
            var smsInfo = new CloudSMSInfo();
            // 短信内容
            smsInfo.SMSMessage = "\r\n日报表:\r\n本日销售订单总金额:123.45\r\n本日出库的数量:321.2\r\n本日生产入库数量:258.3\r\n本日收款金额:6321.32\r\n\r\n本月报表:\r\n月销售订单总金额:1234.5\r\n本月出库的数量:3212.2\r\n本月生产入库数量:25632.3\r\n本月收款金额:261566.9";
            smsInfo.PhoneInfos = phoneinfolist;
            var result = SMSServiceHelper.SendMessage(ctx, smsInfo);
        }
    }
}
