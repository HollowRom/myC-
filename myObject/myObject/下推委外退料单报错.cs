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
                var rebl = e.GetPropertyValue("supBackFlush");
                if (rebl == null)
                {
                    e.SetPropertyValue("supBackFlush", true);
                }
            }
            catch (Exception ex)
            {
                e.SetPropertyValue("supBackFlush", true);
            }

            base.OnPrepareProperty(e);
        }
    }
}
