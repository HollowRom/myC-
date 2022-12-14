using Kingdee.BOS;
using Kingdee.BOS.Core.Bill;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.Core.Metadata.FormElement;
using Kingdee.BOS.ServiceHelper;
using Kingdee.BOS.Util;
using Kingdee.K3.Core.SCM;
using Kingdee.K3.SCM.Purchase.Business.PlugIn;
using System;
using System.ComponentModel;
//参考:https://vip.kingdee.com/article/14072?productLineId=1

namespace myObject
{
    [Description("加载其他单据并修改")]
    [HotUpdate]
    public class load_bill_change: PurchaseBillEdit
    {
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            base.BarItemClick(e);
            if (e.BarItemKey.ToUpperInvariant() == "VBDA_TBBUTTON9")
            {
                string flage = "0";
                IDynamicFormView billview = null;
                try
                {
                    flage = "1";
                    FormMetadata meta = MetaDataServiceHelper.Load(Context, SCMFormIdConst.SAL_OUTSTOCK) as FormMetadata;//单据唯一标识
                    if (meta == null)
                    {
                        View.ShowMessage("meta == null");
                        return;
                    }
                    flage = "2";
                    billview = OpenWebView(Context, meta, 100003);
                    if (billview == null)
                    {
                        View.ShowMessage("billview == null");
                        return;
                    }

                    flage = "2.1";
                    billview.Model.SetValue("FRealQty", 8, 1);//含税单价字段赋值，row为具体行号
                    flage = "2.2";
                    billview.InvokeFieldUpdateService("FRealQty", 1);//联动触发含税单价值更新事件，row为具体行号
                    flage = "2.3";
                    billview.InvokeFormOperation(FormOperationEnum.Save);


                flage = "3";
                    object ooo = billview.Model.GetValue("FTaxPrice", 1);
                    if (ooo == null)
                    {
                        View.ShowMessage("ooo == null");
                        return;
                    }
                    flage = "3.1";
                    View.ShowMessage(ooo.ToString());
                    flage = "4";
                }
                catch (Exception err)
                {
                    View.ShowMessage(flage + "---" + err.ToString());
                }
                finally
                {
                    if (!(billview == null))
                    {
                        billview.Close();
                    }
                }
                //billview.Model.SetValue("FTaxPrice", 3.21, 1);//含税单价字段赋值，row为具体行号
                //billview.InvokeFieldUpdateService("FTaxPrice", 1);//联动触发含税单价值更新事件，row为具体行号
                //billview.InvokeFormOperation(FormOperationEnum.Save);
            }
        }

        public IDynamicFormView OpenWebView(Context ctx, FormMetadata meta, object pkid = null)
        {
            BusinessInfo info = meta.BusinessInfo;
            Form form = info.GetForm();
            BillOpenParameter param = new BillOpenParameter(SCMFormIdConst.SAL_OUTSTOCK, null);
            param.SetCustomParameter("formID", form.Id);
            //根据主键是否为空 置为新增或修改状态
            param.SetCustomParameter("status", !IsPrimaryValueEmpty(pkid) ? "Edit" : "AddNew");
            param.SetCustomParameter("PlugIns", form.CreateFormPlugIns());  //插件实例模型
            //修改主业务组织无须用户确认
            param.SetCustomParameter("ShowConformDialogWhenChangeOrg", false);
            param.Context = this.Context;
            param.FormMetaData = meta;
            param.LayoutId = param.FormMetaData.GetLayoutInfo().Id;
            param.PkValue = !IsPrimaryValueEmpty(pkid) ? pkid : null;//单据主键内码FID
            param.Status = !IsPrimaryValueEmpty(pkid) ? OperationStatus.EDIT : OperationStatus.ADDNEW;
            IResourceServiceProvider provider = form.GetFormServiceProvider();
            //普通的动态表单模式DynamicFormView
            IDynamicFormView billview = provider.GetService(typeof(IDynamicFormView)) as IDynamicFormView;
            //这里模拟为引入模式的WebView，否则遇到交互的时候会有问题，移动端目前无法直接交互
            Type type = Type.GetType("Kingdee.BOS.Web.Import.ImportBillView,Kingdee.BOS.Web");
            //IDynamicFormView billview = (IDynamicFormView)Activator.CreateInstance(type);
            (billview as IBillViewService).Initialize(param, provider);//初始化
            (billview as IBillViewService).LoadData();//加载单据数据
                                                      //如果是普通DynamicFormView时，LoadData的时候会加网控，要清除。//引入模式View不需要
            (billview as IBillView).CommitNetworkCtrl();
            return billview;
        }

        private bool IsPrimaryValueEmpty(object pk)
        {
            return pk == null || pk.ToString() == "0" || string.IsNullOrWhiteSpace(pk.ToString());
        }
     }
}
