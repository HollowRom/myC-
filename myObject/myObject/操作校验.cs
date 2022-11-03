using System;
using System.ComponentModel;
using Kingdee.BOS;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Validation;
using Kingdee.BOS.Util;
using Kingdee.BOS.WebApi.FormService;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.JSON;
using Kingdee.BOS.WebApi.Client;

[Description("操作校验")]
[Kingdee.BOS.Util.HotUpdate]
public class caozujiaoyan : AbstractOperationServicePlugIn
{
    //OnAddValidators操作执行前，加载操作校验器
    public override void OnAddValidators(AddValidatorsEventArgs e)
    {
        base.OnAddValidators(e);
        TestValidator validator = new TestValidator();
            
        //是否需要校验,true需要
        validator.AlwaysValidate = true;
            
        //校验单据头FBillHead
        // validator.EntityKey = "FBillHead";
        validator.EntityKey = "FBillHead";
        //加载校验器
        e.Validators.Add(validator);
    }



    private class TestValidator : AbstractValidator
    {
        //重写方法
        //数组ExtendedDataEntity,传递全部的信息
        public override void Validate(ExtendedDataEntity[] dataEntities, ValidateContext validateContext, Context ctx)
        {
            //for循环,读取数据
            foreach (ExtendedDataEntity obj in dataEntities)
            {
                //采购员PurchaserId
                // object PurchaserId = obj.DataEntity["F_VBDA_Text"];
                //object PurchaserId = obj.DataEntity["MaterialId"];
                //采购员是否为空
                //if (PurchaserId == null || !(doSomeThing(obj.BillNo)))
                if (obj.DataEntity["BillNo"] != null && !obj.DataEntity["BillNo"].Equals(""))
                {
                    var reStr = doSomeThing(obj.BillNo);
                    if (reStr == "下推成功过")
                    {
                        continue;
                    }
                    //报错
                    validateContext.AddError(obj.DataEntity,
                    new ValidationErrorInfo
                    (
                        "FNumber",
                       // "F_VBDA_Text", //出错的字段Key，可以空
                       null, // 出错的字段Key，可以空
                        obj.DataEntityIndex, // 出错的数据包在全部数据包中的顺序
                        obj.RowIndex, // 出错的数据行在全部数据行中的顺序，如果校验基于单据头，此为0
                        "001", //错误编码，可以任意设定一个字符，主要用于追查错误来源
                               // "单据编号" + obj.BillNo + "采购订单没有录入校验字段", //错误的详细提示信息 
                               //"单据编号" + obj.BillNo + "2:" + obj.DataEntityIndex,
                       "单据编号" + obj.BillNo + ":采购订单审核自动生成委外领料失败:" + reStr, //错误的详细提示信息 
                        "审核" + obj.BillNo, // 错误的简明提示信息
                        ErrorLevel.Error // 错误级别：警告、错误...
                    ));
                }
            }
        }

        private string doSomeThing(string billno)
        {
            //第一种方式：通过客户端调用接口，先调用登陆接口，再调用其他接口，
            //K3CloudApiClient cloneCtx = new K3CloudApiClient("http://localhost/");
            //var isSucc = cloneCtx.Login("610bbd142a6e15", "demo", "kingdee@123", 2052);
            //var isSucc = cloneCtx.Login(this.Context.DBId, this.Context.UserName, "888888", 2052);
            //apiClient.Save(ctx, "KKK_BillA", "{\"Model\": {\"FBillTypeID\": {\"FNUMBER\": \"FBillANumber2\"}}");

            //第二种方式：通过克隆创建一个新的上下文，再使用克隆的上下文直接调用api接口，
            var cloneCtx = ObjectUtils.CreateCopy(Context) as Context;
            cloneCtx.ServiceType = WebType.WebService;//写死    
            cloneCtx.ClientInfo = Context.ClientInfo;
            cloneCtx.CharacterSet = Context.CharacterSet;
            // cloneCtx.IsStartTimeZoneTransfer = ctx.IsStartTimeZoneTransfer;  
            cloneCtx.LoginName = Context.LoginName;
            cloneCtx.EntryRole = Context.EntryRole;
            // cloneCtx.Salt = ctx.Salt;      
            cloneCtx.UserPhone = Context.UserPhone;
            cloneCtx.UserEmail = Context.UserEmail;
            cloneCtx.UserLoginType = Context.UserLoginType;

            DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(cloneCtx, "/*dialect*/select distinct c.FBILLNO " +
            " from t_PUR_POOrder a, T_PUR_POORDERENTRY_R b, T_SUB_PPBOM c " +
            " where a.FID = b.FID and b.FSRCBILLNO = c.FSUBBILLNO " +
            " and not exists(select 1 from T_SUB_PICKMTRLDATA a1 where a1.FSUBREQBILLNO = c.FSUBBILLNO) " +
              " and a.FBILLNO = '" + billno + "'");
            if (Dyobj.Count < 1)
            {
                return "没有符合条件的未领料用料清单";
            }
            for (int idx = 0; idx < Dyobj.Count; idx++)
            {
                var parJsonStr = "{\"Ids\":\"\",\"Numbers\":[\"" + Dyobj[idx]["FBILLNO"].ToString() + "\"],\"EntryIds\":\"\",\"RuleId\":\"\",\"TargetBillTypeId\":\"\",\"TargetOrgId\":0,\"TargetFormId\":\"\",\"IsEnableDefaultRule\":\"false\",\"IsDraftWhenSaveFail\":\"false\",\"CustomParams\":{}}";
                try
                {
                    var result = WebApiServiceCall.Push(cloneCtx, "SUB_PPBOM", parJsonStr).ToString();
                    //var result = JSONObject.Parse(WebApiServiceCall.Push(cloneCtx, "SUB_PPBOM", "{\"Numbers\":[\"" + Dyobj[idx]["FBILLNO"] + "\"]}").ToString());
                    //var result2 = JSONObject.Parse(result);
                    //if (!result2.GetJSONObject("ResponseStatus")["IsSuccess"].ToString().ToLower().Equals("true"))
                    //{
                    //    return "下推失败:" + result2.GetJSONObject("ResponseStatus");
                    //}
                }
                catch (Exception e)
                {
                    return "下推异常:" + e.ToString() + ":::发送json:" + parJsonStr;
                }
                
                //var llbill = result.GetJSONObject("ResponseStatus").GetJSONObject("SuccessEntitys");
            }
            return "下推成功过";

            //var result = WebApiServiceCall.Save(cloneCtx, "KKK_BillA", "{\"Model\": {\"FBillTypeID\": {\"FNUMBER\": \"FBillANumber2\"}}");
            //第三种方式：自行创建上下文，再使用此上下文去调用接口
            // var ctx = Kingdee.BOS.ServiceHelper.DataCenterService.GetDataCenterContextByID("DBID");
            // //赋值上用户
            // ctx.UserId = FormConst.AdministratorID; //服务操作用户暂时记为Administrator
            // ctx.UserName = "Administrator";
            // ctx.ServiceType = WebType.WebSite;
            //给上下文赋值上组织
            //Kingdee.BOS.BusinessEntity.Organizations.Organization curOrg = OrganizationServiceHelper.ReadOrgInfoByOrgId(Context, 1);
            //List<long> functions = new List<long>();
            //if (!curOrg.OrgFunctions.IsNullOrEmptyOrWhiteSpace())
            //{
            //    functions = Array.ConvertAll(curOrg.OrgFunctions.Split(','), (a) => { return Convert.ToInt64(a); }).ToList();
            //}
            //Context.CurrentOrganizationInfo = new OrganizationInfo()
            //{
            //    ID = curOrg.Id,
            //    Name = curOrg.Name,
            //    FunctionIds = functions,
            //    AcctOrgType = curOrg.AcctOrgType

            //};

        }

    }
}




// using System;
// using System.ComponentModel;
// using Kingdee.BOS.Core;
// using Kingdee.BOS.Core.DynamicForm.PlugIn;
// using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
// using Kingdee.BOS.Core.Validation;
//
// [Description("操作服务端插件示例")]
// public class caozujiaoyan : AbstractOperationServicePlugIn
// {
//     ///
//     /// 数据加载前，确保需要的属性被加载
//     ///
//     ///
//     ///
//     /// 在列表上执行操作时，单据的字段并没有被完全加载。
//     /// 如果操作插件用到了未被加载的字段，一定会中断；
//     /// 本事件允许插件，强制要求加载某些字段，避免中断
//     ///
//     public override void OnPreparePropertys(PreparePropertysEventArgs e)
//     {
// // 如下代码行，指定字段xxxxx的Key，强制要求加载字段
//         e.FieldKeys.Add("F_VBDA_Text");
//     }
//
//     ///
//     /// 操作执行前，加载操作校验器
//     ///
//     ///
//     ///
//     /// 在系统开始执行校验前，插件可以追加自己的校验器进入操作校验器集合
//     ///
//     public override void OnAddValidators(AddValidatorsEventArgs e)
//     {
// // 如下代码行，示例加入自定义的校验器
// // 自定义的校验器，必须从基类 Kingdee.BOS.Core.Validation.AbstractValidator 派生
//         e.Validators.Add(new OperationSampleValidator());
//     }
//
//     // #region 接下来按照事件的执行事件顺序，由前向后逐一介绍各事件
//
//     ///
//     /// 操作执行前，事务开始前事件
//     ///
//     ///
//     ///
//     /// 1. 此事件在操作校验之后、操作实现代码之前执行
//     /// 2. 此事件在操作事务之前，即此事件中的数据库处理，不受操作的事务保护
//     /// 3. 通常此事件，也可以用来进行数据校验
//     ///
//     public override void BeforeExecuteOperationTransaction(
//         BeforeExecuteOperationTransaction e)
//     {
// // TODO: ....
// // 如下代码，示意校验不通过时，终止操作的执行
//         e.CancelMessage = "测试终止操作执行";
//         e.Cancel = true;
//     }
//
//     ///
//     /// 操作执行前，事务开始后事件
//     ///
//     ///
//     ///
//     /// 1. 此事件在操作校验之后
//     /// 2. 此事件在操作事务开始之后
//     /// 3. 此事件在操作执行代码之前
//     /// 4. 此事件中的数据库处理，受操作的事务保护
//     /// 5. 通常此事件，可以用来做数据准备，在操作之前，提前写数据到库
//     ///
//     public override void BeginOperationTransaction(
//         BeginOperationTransactionArgs e)
//     {
// // TODO: 进行数据准备，甚至写预处理数据到数据库
//     }
//
//     ///
//     /// 操作执行后，事务结束前
//     ///
//     ///
//     ///
//     /// 1. 此事件在操作执行代码之后，操作的内部逻辑已经执行完毕
//     /// 2. 此事件在操作事务提交之前
//     /// 3. 此事件中的数据库处理，受操作的事务保护
//     /// 4. 通常此事件，可以用来做同步数据，如同步生成其他单据，而且需要受事务保护
//     ///
//     public override void EndOperationTransaction(EndOperationTransactionArgs e)
//     {
// // TODO：进行同步数据处理
//     }
//
//     ///
//     /// 操作执行后，事务结束后
//     ///
//     ///
//     ///
//     /// 1. 此事件在操作执行后，操作的内部逻辑已经执行完毕；
//     /// 2. 此事件在操作事务提交之后；
//     /// 3. 此事件中的数据库处理，不受操作的事务保护
//     /// 4. 通常此事件，也可以做同步数据，但是此同步数据的成功与否，不需影响操作
//     ///
//     public override void AfterExecuteOperationTransaction(
//         AfterExecuteOperationTransaction e)
//     {
// // TODO：进行同步数据处理，如果同步失败，不影响操作的结果
//     }
//
//     // #endregion
// }
//
// // [/code]
// // 代码段2：校验器的实现
// //         [code]
// ///
// /// 操作校验器（示例）
// ///
// // [Description("操作校验器（示例）")]
// class OperationSampleValidator : AbstractValidator
// {
//     // #region 重载函数
//
//     ///
//     /// 校验主实体，以此实体数据包进行循环，逐行校验
//     ///
//
//     public override string EntityKey
//     {
//         get
//         {
// // 根据实际校验要求，返回单据头或者单据体Key
//             return "FPOOrderEntry";
//         }
//     }
//
//     ///
//     /// 执行校验，把校验结果注入到validateContext中
//     ///
//     ///
//     ///
//     ///
//     public override void Validate(
//         ExtendedDataEntity[] dataEntities,
//         ValidateContext validateContext,
//         Kingdee.BOS.Context ctx)
//     {
//         foreach (var dataEntity in dataEntities)
//         {
// // TODO: 逐个数据包执行校验代码
//
// // 如下代码，示意如何注入校验提示，后续操作，会自动避开校验没通过的数据包
//             validateContext.AddError(dataEntity.DataEntity,
//                 new ValidationErrorInfo(
//                     "出错字段.Key", // 出错的字段Key，可以空
//                     Convert.ToString(dataEntity.DataEntity[0]), // 数据包内码，必填，后续操作会据此内码避开此数据包
//                     dataEntity.DataEntityIndex, // 出错的数据包在全部数据包中的顺序
//                     dataEntity.RowIndex, // 出错的数据行在全部数据行中的顺序，如果校验基于单据头，此为0
//                     "E1", // 错误编码，可以任意设定一个字符，主要用于追查错误来源
//                     "错误的详细提示信息", // 错误的详细提示信息
//                     "错误摘要", // 错误的简明提示信息
//                     Kingdee.BOS.Core.Validation.ErrorLevel.Error)); // 错误级别：警告、错误...
//         }
//     }
//
//     // #endregion 重载函数
// }