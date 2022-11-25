using Kingdee.BOS.Core;
using Kingdee.BOS.Core.Metadata.EntityElement;
using Kingdee.BOS.KDThread;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Resource;
using Kingdee.BOS.Util;
using Kingdee.K3.MFG.ENG.Business.PlugIn.DynamicForm;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using Kingdee.BOS.Core.DynamicForm.PlugIn.ControlModel;
using Kingdee.BOS.Core.Metadata.FieldElement;
using Kingdee.BOS.App.Data;

namespace qfObject
{
    [Description("物料清单汇总查询新增列")]
    [HotUpdate]
    public class wlqd_hz_cx_add_col : BomQueryIntegration
    {
        protected override void FillBomChildData()
        {
            View.ShowProcessForm(delegate
            {
            }, true, ResManager.LoadKDString("正在查询数据，请稍候...", "015072000039150", SubSystemType.MFG));
            MainWorker.QuequeTask(delegate
            {
                CultureInfoUtils.SetCurrentLanguage(base.Context);
                try
                {
                    View.Session.Clear();
                    View.Session["ProcessRateValue"] = 10;
                    UpdateBomQueryOption();
                    Model.DeleteEntryData("FBottomEntity");
                    if (View.BillBusinessInfo.GetForm().Id.EqualsIgnoreCase("ENG_BomQueryForward2"))
                    {
                        Model.DeleteEntryData("FCobyEntity");
                        View.Model.SetValue("FSelectMaterialId", 0, -1);
                    }
                    int iForceRow = 0;
                    List<DynamicObject> list = BuildBomExpandSourceData(iForceRow);
                    View.Session["ProcessRateValue"] = 30;
                    if (list.IsEmpty())
                    {
                        View.ShowErrMessage(ResManager.LoadKDString("当前物料对应的BOM不存在，请确认12！", "015072000003341", SubSystemType.MFG), ResManager.LoadKDString("BOM不存在！", "015072000002208", SubSystemType.MFG));
                    }
                    else
                    {
                        bomQueryChildItems = GetBomChildData(list, memBomExpandOption);
                        View.Session["ProcessRateValue"] = 60;
                        bomPrintChildItems = bomQueryChildItems;
                        if (View.BillBusinessInfo.GetForm().Id.EqualsIgnoreCase("ENG_BomQueryForward2"))
                        {
                            IsShowSubstituteMaterials();
                        }
                        BindChildEntitys();
                        Entity entity = Model.BusinessInfo.GetEntity("FBottomEntity");
                        DynamicObjectCollection entityDataObject = Model.GetEntityDataObject(entity);
                        if (entityDataObject.Count != 0)
                        {
                            List<KeyValuePair<int, string>> list2 = new List<KeyValuePair<int, string>>();
                            foreach (DynamicObject item in entityDataObject)
                            {
                                foreach (Field field in entity.Fields)
                                {
                                    if (!(field.Key.ToUpper() != "FERPCLSID") && item["ErpClsId"] != null && (Convert.ToString(item["ErpClsId"]) == "2" || Convert.ToString(item["ErpClsId"]) == "3" || Convert.ToString(item["ErpClsId"]) == "5") && (string.IsNullOrEmpty(Convert.ToString(item["BomId_Id"])) || Convert.ToString(item["BomId_Id"]) == "0"))
                                    {
                                        list2.Add(new KeyValuePair<int, string>(entityDataObject.IndexOf(item), "#ff0000"));
                                    }
                                }
                            }
                            EntryGrid control = View.GetControl<EntryGrid>("FBottomEntity");
                            control.SetRowBackcolor(list2);
                            View.Session["ProcessRateValue"] = 100;
                        }
                    }
                    string orgNumber = ((DynamicObject)this.View.Model.GetValue("FBomUseOrgId"))["Number"].ToString();
                    if (orgNumber == null || orgNumber.Equals(""))
                    {
                        this.View.ShowMessage("获取组织失败");
                        return;
                    }
                    int entryRowCount = this.View.Model.GetEntryRowCount("FBottomEntity");
                    //this.View.ShowMessage("开始加列");
                    for (int row = 0; row < entryRowCount; ++row)
                    {
                        string number = ((DynamicObject)this.View.Model.GetValue("FMaterialId2", row))["Number"].ToString();
                        DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select stockQty from v_wlqd_hzcx_add_stock_field_plugin where number = '" + number + "' and orgNumber = '" + orgNumber + "'");
                        if (Dyobj.Count < 1)
                        {
                            //this.View.ShowMessage("跳过了");
                            continue;
                        }
                        //this.View.ShowMessage("开始加列:" + entryRowCount.ToString() + "--:" + Dyobj[0]["F_VBDA_Text"].ToString());
                        //this.View.Model.SetValue("F_VBDA_Text", ((DynamicObject)this.View.Model.GetValue("FMaterialId2", row))["Number"].ToString() + "---" + orgNumber, row);
                        this.View.Model.SetValue("F_VBDA_Decimal", Dyobj[0]["stockQty"].ToString(), row);
                    }
                }
                catch (Exception e)
                {
                    this.View.ShowMessage("异常:" + e.ToString());
                }
                finally
                {
                    View.Session["ProcessRateValue"] = 100;
                }
            }, delegate
            {
            });
            //base.FillBomChildData();
            //int entryRowCount = this.View.Model.GetEntryRowCount("FBottomEntity");
            //for (int row = 0; row < entryRowCount; ++row)
            //{
            //    DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select newid() as F_VBDA_Text");
            //    if (Dyobj.Count < 1)
            //    {
            //        this.View.ShowMessage("跳过了");
            //        continue;
            //    }
            //    this.View.Model.SetValue("F_VBDA_Text", Dyobj[0]["F_VBDA_Text"], row);
            //}
        }
    }
}
