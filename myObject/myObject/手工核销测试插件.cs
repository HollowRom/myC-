using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata.EntityElement;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.K3.FIN.AR.Business.PlugIn.Match;

namespace myObject
{
    [Description("手工核销测试插件"), HotUpdate]
    public class not_auto_hx_test: RecSpecialEdit
    {
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            switch (e.BarItemKey)
            {
                case "VBDA_tbButton":
                    object headStr = this.View.Model.GetValue("F_VBDA_Text");
                    if (headStr == null || headStr.ToString().Equals(""))
                    {
                        this.View.ShowMessage("未填写条件");
                        return;
                    }
                    string billnos = "表头条件:" + this.View.Model.GetValue("F_VBDA_Text").ToString() + "\r\n";
                    
                    //应收
                    DynamicObjectCollection entityD = Model.GetEntityDataObject(View.Model.BusinessInfo.GetEntity("FDebitEntry"));
                    
                    for (int idx = 0; idx < entityD.Count; idx++)
                    {
                        this.View.Model.SetValue("FTheIsMatchD", (object)true, idx);
                    }

                    //应付
                    DynamicObjectCollection entityC = Model.GetEntityDataObject(View.Model.BusinessInfo.GetEntity("FCreditEntry"));
                    
                    for (int idx = 0; idx < entityC.Count; idx++)
                    {
                        billnos = billnos + this.View.Model.GetValue("FSRCBILLNOC", idx);
                        this.View.Model.SetValue("FTheIsMatchC", (object)true, idx);
                    }
                    this.View.ShowMessage(billnos);
                    break;
            }
            base.BarItemClick(e);

        }
    }
}


