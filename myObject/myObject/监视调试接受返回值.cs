//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Kingdee.BOS;
//using Kingdee.BOS.Core.Bill.PlugIn;
//using System.ComponentModel;
////本节新引用
//using Kingdee.BOS.Core.List;
//using Kingdee.BOS.Core.Enums;
//using Kingdee.BOS.Core.DynamicForm;


//namespace Kingdee.Bos.ListShow.Plugln
//{
//    [Description("监视调试接受返回值")]
//    [Kingdee.BOS.Util.HotUpdate]
//    public class ClassName : AbstractBillPlugIn
//    {
//        public override void BarItemClick(BOS.Core.DynamicForm.PlugIn.Args.BarItemClickEventArgs e)
//        {
//            base.BarItemClick(e);
//            //如果点击按钮,弹窗
//            if (e.BarItemKey == "YDIE_tbTest")
//            {
//                //定义一个字段,新建一个listShowParameter实例
//                ListShowParameter listShowParameter = new ListShowParameter();
//                //FormId你要调用那个单据的列表,通过打开未扩展的销售订单,找到唯一标识
//                listShowParameter.FormId = "SAL_SaleOrder";
//                //IsLookUp弹出的列表界面是否有“返回数据”按钮
//                listShowParameter.IsLookUp = true;
//                //列表显示类型
//                //只显示基本信息
//                listShowParameter.ListType = Convert.ToInt32(BOSEnums.Enu_ListType.BaseList);
//                //全部显示,默认全部显示
//                listShowParameter.ListType = Convert.ToInt32(BOSEnums.Enu_ListType.List);
//                //是否显示复选框。默认是true，如果false就是不显示
//                listShowParameter.MultiSelect = false;
//                ////接收返回值
//                //this.View.ShowForm(listShowParameter);




//                //方法2,传result返回
//                this.View.ShowForm(listShowParameter, delegate (FormResult result)
//                {
//                    //读取返回值
//                    object returnData = result.ReturnData;

//                    //判断是否是RowCollection
//                    if (returnData is ListSelectedRowCollection)
//                    {
//                        //如果是,执行,转换格式
//                        ListSelectedRowCollection listSelectedRowCollection = returnData as ListSelectedRowCollection;

//                        //如果不是空值,说明有返回值
//                        if (listSelectedRowCollection != null)
//                        {
//                            //获取值.0代表第一行值
//                            DynamicObjectDataRow datarow = (DynamicObjectDataRow)listSelectedRowCollection[0].DataRow;


//                            //赋值
//                            //单据日期 F_YDIE_Text62 
//                            //单据编号 F_YDIE_Text61
//                            //单据ID F_YDIE_Text6
//                            this.View.Model.SetValue("F_YDIE_Text6", datarow.DynamicObject["FID"].ToString());
//                            this.View.Model.SetValue("F_YDIE_Text61", datarow.DynamicObject["FBillNo"].ToString());
//                            this.View.Model.SetValue("F_YDIE_Text62", datarow.DynamicObject["FDate"].ToString());
//                        }
//                    }
//                });

//            }
//        }
//    }
//}


