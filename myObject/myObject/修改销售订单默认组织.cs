using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using System;
using System.ComponentModel;

namespace myObject
{
    [Description("修改销售订单默认组织为100"), HotUpdate]
    public class editxsddOrg : AbstractBillPlugIn
    {
        public override void AfterBindData(EventArgs e)
        {
            base.AfterBindData(e);
            if (this.View.OpenParameter.Status.Equals(OperationStatus.ADDNEW))
            {
                FormMetadata formMetadata = MetaDataServiceHelper.Load(this.Context, "ORG_Organizations") as FormMetadata;
                DynamicObject dynamicObject = BusinessDataServiceHelper.LoadSingle(this.Context, 1, formMetadata.BusinessInfo.GetDynamicObjectType());
                //"101.3"
                //100039
                this.View.OpenParameter.SetCustomParameter("ShowConfirmDialogWhenChangeOrg", false);
                DynamicFormViewPlugInProxy proxy = this.View.Model.GetService<DynamicFormViewPlugInProxy>();
                proxy.FireOnLoad();
                this.View.Model.SetValue("FSALEORGID", dynamicObject);
                this.View.UpdateView("FSALEORGID");
                this.View.OpenParameter.SetCustomParameter("ShowConfirmDialogWhenChangeOrg", true);
                proxy.FireOnLoad();
            }
         }
    }
}
