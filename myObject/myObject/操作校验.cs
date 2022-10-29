using System.ComponentModel;
using Kingdee.BOS.Core;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Validation;

[Description("操作校验")]
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
        validator.EntityKey = "FBillHead";
            
        //加载校验器
        e.Validators.Add(validator);
    }

    private class TestValidator : AbstractValidator
    {
        //重写方法
        //数组ExtendedDataEntity,传递全部的信息
        public override void Validate(ExtendedDataEntity[] dataEntities, ValidateContext validateContext, Kingdee.BOS.Context ctx)
        {
            //for循环,读取数据
            foreach (ExtendedDataEntity obj in dataEntities)
            {
                //采购员PurchaserId
                object PurchaserId = obj.DataEntity["F_VBDA_Text"];
                //采购员是否为空
                if (PurchaserId == null || PurchaserId.ToString().Equals(""))
                {
                    //报错
                    validateContext.AddError(obj.DataEntity,
                        new ValidationErrorInfo
                        ("F_VBDA_Text", //出错的字段Key，可以空
                           null, // 出错的字段Key，可以空
                            obj.DataEntityIndex, // 出错的数据包在全部数据包中的顺序
                            obj.RowIndex, // 出错的数据行在全部数据行中的顺序，如果校验基于单据头，此为0
                            "001", //错误编码，可以任意设定一个字符，主要用于追查错误来源
                            "单据编号" + obj.BillNo + "采购订单没有录入校验字段", //错误的详细提示信息 
                            "审核" + obj.BillNo, // 错误的简明提示信息
                            Kingdee.BOS.Core.Validation.ErrorLevel.Error // 错误级别：警告、错误...
                        ));
                }
            }
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