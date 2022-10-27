using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerHelperUse
{
    internal interface ITest1
    {
        DbConnection Conn
        {
            get;
        }

        void Test(DbConnection conn);
    }
}
