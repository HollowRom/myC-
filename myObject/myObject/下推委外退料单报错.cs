using System;
using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn.Args;
using Kingdee.BOS.Core.Metadata.Util;
using Kingdee.K3.Core.MFG.EntityHelper;
using Kingdee.K3.MFG.SUB.App.BillConvertServicePlugIn;

namespace myObject
{
    // public class test_some_err : PPBom2ReturnItem
    // {
    //     public override void OnParseFilterOptions(ParseFilterOptionsEventArgs e)
    //     {
    //         base.OperateOption.SetVariableValue("supBackFlush", true);
    //         base.OnParseFilterOptions(e);
    //     }
    // }

    public class test_some_err : PPBOM2ReturnMtrlConvert
    {
        protected override void OnPrepareProperty(ConvertContext e)
        {
            try
            {
                var rebl = this.Option.GetVariableValue<bool>("supBackFlush");
            }
            catch (Exception ex)
            {
                this.Option.SetPropertyValue("supBackFlush", true);
            }

            base.OnPrepareProperty(e);
        }

        public override void OnInSelectedRow(InSelectedRowEventArgs e)
        {
            try
            {
                var rebl = this.Option.GetVariableValue<bool>("supBackFlush");
            }
            catch (Exception ex)
            {
                this.Option.SetPropertyValue("supBackFlush", true);
            }

            base.OnInSelectedRow(e);
        }
    }
}
