using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices.Internal;

namespace Dapper
{
    /// <summary>
    /// Dapper 使用 Oracle.ManagedDataAccess 访问 Oracle 的 参数处理帮助类
    /// 出自 https://stackoverflow.com/questions/18772781/using-dapper-querymultiple-in-oracle
    /// 使用示例 参见后面的注释
    /// 似乎将 List<OracleParameter> 改为 Dictionary 更好写，防止重复参数
    /// </summary>
    public class OracleDynamicParameters : SqlMapper.IDynamicParameters
    {
        private readonly DynamicParameters dynamicParameters = new DynamicParameters();

        private readonly List<OracleParameter> oracleParameters = new List<OracleParameter>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="oracleDbType"></param>
        /// <param name="direction">参数方向。ParameterDirection.Input 时，应该指定传入的value值</param>
        /// <param name="value">默认null</param>
        /// <param name="size"></param>
        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction, object value = null, int? size = null)
        {
            OracleParameter oracleParameter;
            if (size.HasValue)
            {
                oracleParameter = new OracleParameter(name, oracleDbType, size.Value, value, direction);
            }
            else
            {
                oracleParameter = new OracleParameter(name, oracleDbType, value, direction);
            }

            oracleParameters.Add(oracleParameter);
        }

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction)
        {
            var oracleParameter = new OracleParameter(name, oracleDbType, direction);
            oracleParameters.Add(oracleParameter);
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            ((SqlMapper.IDynamicParameters)dynamicParameters).AddParameters(command, identity);

            var oracleCommand = command as OracleCommand;

            if (oracleCommand != null)
            {
                oracleCommand.Parameters.AddRange(oracleParameters.ToArray());
            }
        }
        /// <summary>
        /// 移除所有的OracleParameter参数
        /// </summary>
        public void Clear ()
        {
            oracleParameters.Clear();
        }

        /*  OracleDynamicParameters 使用示例：
            int selectedId = 1;
            var sql = "BEGIN OPEN :rslt1 FOR SELECT * FROM customers WHERE customerid = :id; " +
                            "OPEN :rslt2 FOR SELECT * FROM orders WHERE customerid = :id; " +
                            "OPEN :rslt3 FOR SELECT * FROM returns Where customerid = :id; " +
                      "END;";
    
            OracleDynamicParameters dynParams = new OracleDynamicParameters();
            dynParams.Add(":rslt1", OracleDbType.RefCursor, ParameterDirection.Output);
            dynParams.Add(":rslt2", OracleDbType.RefCursor, ParameterDirection.Output);
            dynParams.Add(":rslt3", OracleDbType.RefCursor, ParameterDirection.Output);
            dynParams.Add(":id", OracleDbType.Int32, ParameterDirection.Input, selectedId);
    
            using (IDbConnection dbConn = new OracleConnection("<conn string here>"))
            {
                dbConn.Open();
                var multi = dbConn.QueryMultiple(sql, param: dynParams);
        
                var customer = multi.Read<Customer>().Single();
                var orders = multi.Read<Order>().ToList();
                var returns = multi.Read<Return>().ToList();
                ...
                dbConn.Close();
            }
         */
    }
}