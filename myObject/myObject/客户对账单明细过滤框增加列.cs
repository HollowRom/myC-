using Kingdee.BOS.Core.CommonFilter.PlugIn.Args;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Util;
using Kingdee.K3.FIN.AR.Report.PlugIn;
using System;
using System.ComponentModel;

namespace myObject
{
    [Description("客户对账单明细过滤框增加列")]
    [HotUpdate]
    public class khdzd_filter_add_col_dic : AgingAnalysisFilter
    {
        public override void AfterBindData(EventArgs e)
        {
            int id = 0;
            id++;
            base.AfterBindData(e);
        }

        public override void DataChanged(DataChangedEventArgs e)
        {
            int id = 0;
            id++;
            base.DataChanged(e);
        }
        public override void AfterCreateNewData(EventArgs e)
        {
            int id = 0;
            id++;
            base.AfterCreateNewData(e);
        }
        public override void OnParseSetting(OnParseSettingEventArgs e)
        {
            int id = 0;
            id++;
            base.OnParseSetting(e);
        }
        public override void ButtonClick(ButtonClickEventArgs e)
        {
            int id = 0;
            id++;
            base.ButtonClick(e);
        }

        public override void OnInitialize(InitializeEventArgs e)
        {
            int id = 0;
            id++;
            base.OnInitialize(e);
        }
    }
}
