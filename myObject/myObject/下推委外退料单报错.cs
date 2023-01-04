using Kingdee.BOS.Core.Metadata.ConvertElement.PlugIn.Args;
using Kingdee.K3.MFG.SUB.App.BillConvertServicePlugIn;

namespace myObject
{
    public class test_some_err : PPBom2ReturnItem
    {
        public override void OnParseFilterOptions(ParseFilterOptionsEventArgs e)
        {
            base.OperateOption.SetVariableValue("supBackFlush", true);
            base.OnParseFilterOptions(e);
        }
    }
}
