using Kingdee.BOS;
using Kingdee.BOS.BusinessEntity.BusinessFlow;
using Kingdee.BOS.Core.Warn.Message;
using Kingdee.BOS.Core.Warn.PlugIn;
using Kingdee.BOS.Core.Warn.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kingdee.Bos.WarnTestPlugin.Demo.AdMsgWarn
{
    /// <summary>
    /// 预警服务插件,需要绑定到对应监控对象的监控方案解析插件
    /// </summary>
    [Description("采购订单预警_高级消息预警,监控方案解析插件"), HotUpdate]
    public class WarnServicePlugInDemo : AbstractWarnServicePlugIn
    {
        /// <summary>
        /// 解析预警方法之前
        /// </summary>
        /// <param name="e"></param>
        public override void BeforeParseWarnMessage(BeforeParseWarnMessageEventArgs e)
        {
            base.BeforeParseWarnMessage(e);
            //添加消息查询数据字段 
            // 格式1：entitykey.fieldkey(分录key.字段key值)
            // 格式2：fieldkey(单据字段key值)
            // 示例：增加采购订单， 分录明细.采购单位（FUnitId）
            e.KeyValueFieldNames.Add("FUnitId");
        }

        /// <summary>
        /// 发送消息之前
        /// </summary>
        /// <param name="e"></param>
        public override void BeforeSendWarnMessage(BeforeSendWarnMessageEventArgs e)
        {
            base.BeforeSendWarnMessage(e);
            int number = 1;
            foreach (var messageItem in e.WarnMessage.MessageEntityList)
            {
                //修改消息体内容
                MessageEntity messageEntity = messageItem.MessageEntity as MessageEntity;

                DynamicObject objRef = messageItem.WarnRowData["FUnitId_Ref"] as DynamicObject;
                string strVal = string.Empty;
                if (objRef != null)
                {
                    strVal = objRef["NAME"].ToString();
                }

                var filedId = string.Format("{{{0}}}", "_DemoEntityKey.FDemoVeriableId");//key值 entityKey.fieldName 替换是用{字段ID}更新
                if (messageEntity.Content != null)
                {
                    messageEntity.Content = messageEntity.Content.Replace(filedId, strVal);
                }
                // TODO 后续版本规划

                if (Kingdee.BOS.KDConfiguration.Current.GetAppSettingItemValue("_warnLocaleValue") == "true")
                {
                    if (messageItem.MessageEntity_L != null && messageItem.MessageEntity_L.Content != null)
                    {
                        var localeContent = messageItem.MessageEntity_L.Content.Clone() as LocaleValue;
                        foreach (var item in messageItem.MessageEntity_L.Content)
                        {
                            localeContent[item.Key] = item.Value.Replace(filedId, strVal);
                        }

                        messageItem.MessageEntity_L.Content = localeContent;
                    }
                }

                //---二开实现发送明细短信消息---//				
                //可以在这里取到具体明细消息，再调用发送短信的接口，去发送消息，
                //发送短信的接口：https://vip.kingdee.com/article/172033409387063040?productLineId=1&isKnowledge=2

                number++;
            }
        }

        /// <summary>
        /// 发送消息之后
        /// </summary>
        /// <param name="e"></param>
        public override void AfterSendWarnMessage(AfterSendWarnMessageEventArgs e)
        {
            base.AfterSendWarnMessage(e);
        }

        /// <summary>
        /// 触发监控条件解析之后的事件
        /// 用途：主要用于添加额外的一些过滤条件
        /// </summary>
        /// <param name="e"></param>
        public override void AfterWarnConditionParse(AfterWarnConditionParseArgs e)
        {
            base.AfterWarnConditionParse(e);
        }

        /// <summary>
        /// 执行监控解析
        /// 用途：主要用于修改执行后的数据集合，不建议重写
        /// </summary>
        public override void ExcuteWarnParse(ExcuteWarnParseEventArgs e)
        {
            base.ExcuteWarnParse(e);
        }
    }
}
