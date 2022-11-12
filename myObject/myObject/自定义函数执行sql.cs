using Kingdee.BOS.Core.Metadata.Expression.FuncDefine;
using Kingdee.BOS;
using System;
using System.ComponentModel;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Orm.DataEntity;
using System.Text;
using Kingdee.BOS.Util;

namespace myObject
{
    [Description("自定义函数执行sql"), HotUpdate, Serializable]
    public class exSQLFromFunc : AbstractFuncDefine
    {
        private string sqlParseKey = "someKey";
        public exSQLFromFunc() { }
        public exSQLFromFunc(Context ctx, dynamic obj)
        {
            m_ctx = ctx;
            m_obj = obj;
        }
        public override IFuncDefine GetFunctionDefine(Context ctx, dynamic obj)
        {
            return new exSQLFromFunc(ctx, obj);
        }
        public override object GetFuncDefine()
        {
            return new Func<string, string>(exSQLFromFunc1);
        }

        //返回只能有一列,且需要as someKey
        string exSQLFromFunc1(string str)
        {
            str = str.ToLower();
            _ = str.Replace("uudate", "update");
            if (str.Contains("update") && !str.Contains("where"))
            {
                return "未填写where条件";
            }
            StringBuilder sqlStr = new StringBuilder();
            DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(m_ctx, str);
            if (str.Contains("update")) {
                return "执行update";
            }
            for (int idx = 0; idx < Dyobj.Count; idx++)
            {
                sqlStr.Append(Dyobj[idx][sqlParseKey].ToString() + ",");
            }
            return sqlStr.ToString() == "" ? "查询结果为空" : sqlStr.ToString();
        }

        //private void logStr(string title, string body)
        //{
        //    var logs = new List<LogObject>();

        //    var log = new LogObject();

        //    log.pkValue = this.View.Model.GetPKValue().ToString();

        //    log.Description = title;

        //    log.OperateName = body;

        //    log.ObjectTypeId = this.View.Model.BillBusinessInfo.GetForm().Id;

        //    log.SubSystemId = this.View.OpenParameter.SubSystemId;

        //    log.Environment = OperatingEnvironment.BizOperate;

        //    logs.Add(log);

        //    LogServiceHelper.BatchWriteLog(this.Context, logs);
        //}
    }
}
