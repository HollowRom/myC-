using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace myObject
{
    [Description("销售出库修改结算组织更改全部组织"), HotUpdate]
    public class DataChangedFormPlugIn : AbstractDynamicFormPlugIn
    {
        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            if (e.Field.Key.ToUpper().Equals("FSALEORGID"))
            {
                var slDic = View.Model.GetValue("FSALEORGID") as Dictionary<string, object>;
                if (slDic == null)
                {
                    View.ShowMessage("清空了销售组织");
                    return;
                }
                var newId = slDic["Id"].ToString();
                //View.ShowMessage("更改后的销售组织为:" + JObject.FromObject(View.Model.GetValue("FSALEORGID")).ToString());
                DynamicObject dynamicObject = BusinessDataServiceHelper.LoadSingle(this.Context, /*newId*/10040, (MetaDataServiceHelper.Load(this.Context, "ORG_Organizations") as FormMetadata).BusinessInfo.GetDynamicObjectType());
                this.View.OpenParameter.SetCustomParameter("ShowConfirmDialogWhenChangeOrg", false);
                DynamicFormViewPlugInProxy proxy = this.View.Model.GetService<DynamicFormViewPlugInProxy>();
                proxy.FireOnLoad();
                this.View.Model.SetValue("FSTOCKORGID", dynamicObject);
                this.View.UpdateView("FSTOCKORGID");
                this.View.OpenParameter.SetCustomParameter("ShowConfirmDialogWhenChangeOrg", true);
                proxy.FireOnLoad();

                this.View.Model.SetValue("F_VBDA_TEXT", "F_VBDA_TEXT");

                //DynamicObject dynamicObject2 = BusinessDataServiceHelper.LoadSingle(this.Context, (View.Model.GetValue("FSTOCKORGID") as Dictionary<string, object>)["Id"].ToString(), (MetaDataServiceHelper.Load(this.Context, "ORG_Organizations") as FormMetadata).BusinessInfo.GetDynamicObjectType());
                //this.View.Model.SetValue("FSALEORGID", dynamicObject2);
                //this.View.UpdateView("FSALEORGID");
                View.ShowMessage("更改了销售组织并刷新了发货组织");
            }
        }
    }
}
