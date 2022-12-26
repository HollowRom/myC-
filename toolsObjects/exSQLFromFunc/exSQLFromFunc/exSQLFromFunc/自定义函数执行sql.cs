using Kingdee.BOS.Core.Metadata.Expression.FuncDefine;
using Kingdee.BOS;
using System;
using System.ComponentModel;
using Kingdee.BOS.App.Data;
using Kingdee.BOS.Orm.DataEntity;
using System.Text;

namespace exSQLFromFuncObject
{
    [Description("自定义函数执行sql")]
    [Kingdee.BOS.Util.HotUpdate]
    [Serializable]
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
            if (str.Contains("update") && !str.Contains("where"))
            {
                return "未填写where条件";
            }
            StringBuilder sqlStr = new StringBuilder();
            DynamicObjectCollection Dyobj = DBUtils.ExecuteDynamicObject(m_ctx, str);
            if (str.Contains("update"))
            {
                return "执行update";
            }
            for (int idx = 0; idx < Dyobj.Count; idx++)
            {
                sqlStr.Append(Dyobj[idx][sqlParseKey].ToString() + ",");
            }
            return sqlStr.ToString() == "" ? "查询结果为空" : sqlStr.ToString();
        }
    }
}

