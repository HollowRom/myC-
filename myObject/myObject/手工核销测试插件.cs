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
                    DynamicObjectCollection entity = Model.GetEntityDataObject(View.Model.BusinessInfo.GetEntity("FDebitEntry"));
                    for (int idx = 0; idx < entity.Count; idx++)
                    {
                        this.View.Model.SetValue("FTheIsMatchC", (object)true, idx);
                    }
                    this.View.ShowMessage(entity.Count.ToString() + "a");
                    break;
            }
            base.BarItemClick(e);

        }
    }
}


