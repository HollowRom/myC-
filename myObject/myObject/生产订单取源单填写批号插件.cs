//using System.ComponentModel;
//using Kingdee.BOS.App.Data;
//using Kingdee.BOS.Core.Bill.PlugIn;
//using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
//using Kingdee.BOS.Orm.DataEntity;

//namespace Kingdee.Bos.ProJect.BillBeforeSave.Plugln
//{
//    [Description("生产订单取源单填写批号插件2")]
//    [Kingdee.BOS.Util.HotUpdate]
//    public class workOrderWriteLot2 : AbstractBillPlugIn
//    {
//        public override void BarItemClick(BarItemClickEventArgs e)
//        {
//            base.BarItemClick(e);

//            //this.View.ShowMessage("214141");
//            if (e.BarItemKey.Equals("tbSplitSave"))
//            {
//                //return;
//                //string oldLot;
//                int rowNum = this.View.Model.GetEntryRowCount("FTreeEntity");

//                for (int idx = 0; idx < rowNum; idx++)
//                {

//                    if (!this.View.GetFieldEditor("FLot", idx).Enabled)
//                    {
//                        continue;
//                    }

//                    //oldLot = "";

//                    //oldLot = this.View.Model.GetValue("FLot", idx).ToString();

//                    //if (oldLot == "")
//                    //{
//                    //    continue;
//                    //}

//                    long sourceId = (long)this.View.Model.GetValue("FSALEORDERID", idx);

//                    if (sourceId == 0)
//                    {
//                        //sourceId = 100089;
//                        continue;
//                    }

//                    DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select top 1 FBILLNO from T_SAL_ORDER where fid = " + sourceId.ToString());
//                    if (Dyobj.Count < 1)
//                    {
//                        continue;
//                    }

//                    this.View.Model.SetValue("FLot", Dyobj[0]["FBILLNO"], idx);

//                }
//            }

//        }
//    }

//}



