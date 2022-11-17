using System.ComponentModel;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.Bill.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Util;

namespace myObject
{
    [Description("用料清单手动携带生产订单批号字段"), HotUpdate]
    public class ppbomWriteLot : AbstractBillPlugIn
    {
        public override void BeforeSave(BeforeSaveEventArgs e)
        {
            base.BeforeSave(e);
            string oldLot;
            int rowNum = this.View.Model.GetEntryRowCount("FEntity");
            for (int idx = 0; idx < rowNum; idx++)
            {

                if (!this.View.GetFieldEditor("FLot", idx).Enabled)
                {
                    continue;
                }
                oldLot = "";

                oldLot = this.View.Model.GetValue("FLot", idx).ToString();

                if (oldLot == "" || oldLot == "1")
                {
                    continue;
                }
                long sourceId = (long)this.View.Model.GetValue("FMOENTRYID", idx);
                this.View.Model.SetValue("FLot", sourceId.ToString(), idx);
                if (sourceId == 0)
                {
                    //sourceId = 100089;
                    continue;
                }

                DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(this.Context, "/*dialect*/select top 1 FLOT_TEXT from T_PRD_MOENTRY where FENTRYID = " + sourceId.ToString());
                if (Dyobj.Count < 1)
                {
                    continue;
                }

                this.View.Model.SetValue("FLot", Dyobj[0]["FLOT_TEXT"], idx);

            }

        }
    }

}

