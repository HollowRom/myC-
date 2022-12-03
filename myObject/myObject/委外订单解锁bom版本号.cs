using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;
using Kingdee.K3.MFG.SUB.Business.PlugIn.Bill;

namespace myObject
{
    [Description("委外订单解锁bom版本号")]
    [HotUpdate]
    public class unlock_wwdd_bom_filed : SubReqOrderEdit
    {
        public override void EntryBarItemClick(BarItemClickEventArgs e)
        {
            base.EntryBarItemClick(e);
            if (e.BarItemKey.ToUpperInvariant() == "VBDA_TBBUTTON")
            {
                DynamicObjectCollection entityDataObject = Model.GetEntityDataObject(View.Model.BusinessInfo.GetEntity("FTreeEntity"));

                for (int idx = 0; idx < entityDataObject.Count; idx++)
                {
                    //this.View.GetFieldEditor("FMaterialId", idx).SetEnabled("", true);
                    //this.View.GetFieldEditor("FBomId", idx).SetEnabled("", true);
                    //this.View.GetFieldEditor("FUnitID", idx).SetEnabled("", true);
                    //this.View.GetFieldEditor("F_VBDA_Base1", idx).SetEnabled("", true);

                }
                this.View.ShowMessage("aabb3");
            }
        }

        public override void AfterBindData(EventArgs e)
        {
            //this.View.GetControl("FBomId").Enabled = true;
            //this.View.StyleManager.SetEnabled("FBomId", "", true);
            //this.View.StyleManager.SetEnabled("FBomId", "BillStatusByHead", true);


            //this.View.GetControl("FMaterialId").Enabled = true;
            //this.View.StyleManager.SetEnabled("FMaterialId", "", true);
            //this.View.StyleManager.SetEnabled("FMaterialId", "BillStatusByHead", true);


            //this.View.GetControl("FDate").Enabled = true;
            //this.View.StyleManager.SetEnabled("FDate", "", true);
            //this.View.StyleManager.SetEnabled("FDate", "BillStatusByHead", true);
            
            DynamicObjectCollection entityDataObject = Model.GetEntityDataObject(View.Model.BusinessInfo.GetEntity("FTreeEntity"));

            for (int idx = 0; idx < entityDataObject.Count; idx++)
            {
                //this.View.GetFieldEditor("FMaterialId", idx).SetEnabled("", true);
                //this.View.GetFieldEditor("FBomId", idx).SetEnabled("", true);
                //this.View.GetFieldEditor("FUnitID", idx).SetEnabled("", true);
                //this.View.GetFieldEditor("F_VBDA_Base1", idx).SetEnabled("", true);
            }

            
            this.View.ShowMessage("aabb3");
        }
    }
}
