using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerHelperUse
{
    /// <summary>
    /// 子类实现中使用的类型，必须和父类中的完全匹配（必须一致）
    /// </summary>
    internal class Test2 //: ITest1
    {
        public SqlConnection Conn
        {
            get;
        }

        void Test(SqlConnection conn)
        {
            
        }
    }
}
