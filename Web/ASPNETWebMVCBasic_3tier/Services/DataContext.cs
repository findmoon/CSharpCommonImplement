using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace WebAPI_CURD.Services
{
    /// <summary>
    /// connect the SQLite database
    /// </summary>
    public class DataContext : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// 连接字符串配置的Name
        /// </summary>
        private string _connectionStringName = "SampleConnString2";

        /// <summary>
        /// Dapper 的 数据库连接
        /// </summary>
        public IDbConnection DbConnection { get; private set; }
        public DataContext()
        {
            //Debug.WriteLine(WebConfigurationManager.ConnectionStrings["SampleConnString"].ConnectionString, "ConnectionString");

            InitDbConnection();

            //DbConnection.Open();
            //Debug.WriteLine(((OracleConnection)DbConnection).ServerVersion, "Connected to Oracle Database - constructor");
        }
        /// <summary>
        /// 初始化DbConnection，构造函数或更新连接字符串后 执行
        /// </summary>
        private void InitDbConnection()
        {
            var curr = DbConnection;
            DbConnection = new OracleConnection(WebConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString);
            curr?.Dispose();
        }

        public static void Test()
        {
            try
            {
                //// Please replace the connection string attribute settings
                //string constr = "user id=scott;password=tiger;data source=SampleDataSource";

                //using (OracleConnection con = new OracleConnection(constr))
                //{
                //    con.Open();
                //    Debug.WriteLine(con.ServerVersion, "Connected to Oracle Database");
                //}

                var connStr = WebConfigurationManager.ConnectionStrings["SampleConnString"];
                using (OracleConnection con = new OracleConnection(WebConfigurationManager.ConnectionStrings["SampleConnString"].ConnectionString))
                {
                    con.Open();
                    Debug.WriteLine(con.ServerVersion, "Connected to Oracle Database -- SampleConnString");
                }

                using (OracleConnection con = new OracleConnection(WebConfigurationManager.ConnectionStrings["SampleConnString2"].ConnectionString))
                {
                    con.Open();
                    Debug.WriteLine(con.ServerVersion, "Connected to Oracle Database -- SampleConnString2");
                }

                using (OracleConnection con = new OracleConnection(WebConfigurationManager.ConnectionStrings["SampleConnString3"].ConnectionString))
                {
                    con.Open();
                    Debug.WriteLine(con.ServerVersion, "Connected to Oracle Database -- SampleConnString3");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Error");
            }
        }

        /// <summary>
        /// 更新xml配置信息中的连接字符串
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="dbName"></param>
        /// <param name="DBServer"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public void UpdateConnStr(string userName, string pwd, string dbName, string DBServer, int port = 1521)
        {
            WebConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString =
                $"User Id={userName};Password={pwd};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={DBServer})(PORT={port}))(CONNECT_DATA=(SERVICE_NAME={dbName})))";
            InitDbConnection();
        }
        /// <summary>
        /// 创建DB、tables【Program.cs 的 startup 执行一次】
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            try
            {
                await _initDBTables();

                async Task _initDBTables()
                {
                    // 初始化用户、授予权限
                    using (var con = new OracleConnection(WebConfigurationManager.ConnectionStrings["SampleConnString"].ConnectionString))
                    {
                        var user = "RetriveUser".ToUpper();
                        var userPwd = "Ask2022";

                        // ExecuteScalarAsync、QueryAsync 方法，如下方式调用，参数如果为 @user 报错 ORA-00936；
                        // 如果为 :user 报错 ORA-01745(invalid host/bind variable name) 报错原因是 user 变量名是保留关键字，所以无效
                        //var result = await con.ExecuteScalarAsync<string>($"select count(*) from all_users where username=@user", new { user });

                        //// 还可以借助 OracleDynamicParameters ，可以直接添加多个参数
                        OracleDynamicParameters dynParams = new OracleDynamicParameters();
                        dynParams.Add(":myuser", OracleDbType.Varchar2, ParameterDirection.Input, user);
                        var result = await con.ExecuteScalarAsync<string>($"select count(*) from all_users where username=:myuser", dynParams);

                        //var result = await con.ExecuteScalarAsync<string>($"select count(*) from all_users where username=:myuser", new { myuser=user });
                        if (result != "1")
                        {
                            // 不存在

                            // ORA-01935(缺失用户名或角色名)
                            //await con.ExecuteAsync($"create user :myuser identified by :userPwd", new { myuser=user, userPwd });

                            //dynParams.Clear();
                            //dynParams.Add(":myuser", OracleDbType.Varchar2, ParameterDirection.Input, user);

                            // 似乎无法这样作为参数使用 报错 ORA-01935
                            //dynParams.Add(":userPwd", OracleDbType.Varchar2, ParameterDirection.Input, userPwd);
                            //await con.ExecuteAsync($"create user :myuser identified by :userPwd", dynParams);

                            await con.ExecuteAsync($"create user {user} identified by {userPwd}");
                        }

                        //await con.ExecuteAsync($"Grant DBA to :myuser", new { myuser=user });
                        // ORA-00987: 用户名缺失或无效
                        //await con.ExecuteAsync($"Grant DBA to :myuser", dynParams);
                        await con.ExecuteAsync($"Grant DBA to {user}");
                    }

                    // 初始化创建表

                    // 一个个的判断是否存在，一个个执行创建
                    #region Users 表
                    {
                        OracleDynamicParameters dynParams = new OracleDynamicParameters();

                        // 1. 查询是否存在 table Users  
                        var UsersTableNmae = "Users".ToUpper();
                        dynParams.Add(":UsersTableNmae", OracleDbType.Varchar2, ParameterDirection.Input, UsersTableNmae);
                        var tableExists = await DbConnection.ExecuteScalarAsync<string>("select count(*) from user_tables where table_name = :UsersTableNmae", dynParams);
                        if (tableExists == "0") // 不存在
                        {
                            // Oracle 不支持 Text 类型
                            var createSql = $"CREATE TABLE {UsersTableNmae}" + @" (
                    Id INTEGER NOT NULL PRIMARY KEY,
                    UserName VARCHAR2(255) NOT NULL UNIQUE,
                    Title VARCHAR2(255),
                    FirstName VARCHAR2(30),
                    LastName VARCHAR2(30),
                    Email VARCHAR2(255) NOT NULL UNIQUE,
                    Role INT DEFAULT 0,
                    PasswordHash VARCHAR2(1000)
                )";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 2, 判断存在 sequence SEQ_USERS_ID 
                        var sequenceName = "SEQ_USERS_ID";
                        var querySequenceSql = "select count(0) from user_sequences where sequence_name = upper(:sequenceName)";
                        dynParams.Add(":sequenceName", OracleDbType.Varchar2, ParameterDirection.Input, sequenceName);
                        var querySequence = await DbConnection.ExecuteScalarAsync<string>(querySequenceSql, dynParams);
                        var seqExists = querySequence != "0";
                        if (!seqExists)
                        {
                            var createSql = $"create sequence {sequenceName}" + @"
    minvalue 1
    nomaxvalue
    increment by 1
    start with 1
    nocache";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 3, 判断存在 trigger TRI_USERS_INSERTID 
                        var triggerName = "TRI_USERS_INSERTID";
                        dynParams.Add(":triggerName", OracleDbType.Varchar2, ParameterDirection.Input, triggerName);
                        var queryTriggerSql = "select count(0) from user_triggers where trigger_name = upper(:triggerName)";
                        var queryTrigger = await DbConnection.ExecuteScalarAsync<string>(queryTriggerSql, dynParams);
                        var triggerExists = queryTrigger != "0";
                        if (!triggerExists)
                        {
                            var createSql = $@"create or replace trigger {triggerName} 
    before insert on {UsersTableNmae} for each row 
    begin
        select {sequenceName}.Nextval into:new.Id from dual;
    end;";
                            await DbConnection.ExecuteAsync(createSql);
                        }
                    }
                    #endregion


                    #region T_LINECONFIG 表
                    {
                        OracleDynamicParameters dynParams = new OracleDynamicParameters();

                        // 1. 查询是否存在 table T_LOADCOMPONENT  
                        var tableNmae = "T_LINECONFIG";
                        dynParams.Add(":tableNmae", OracleDbType.Varchar2, ParameterDirection.Input, tableNmae);
                        var tableExists = await DbConnection.ExecuteScalarAsync<string>("select count(*) from user_tables where table_name = :tableNmae", dynParams);
                        if (tableExists == "0") // 不存在
                        {
                            // Oracle 不支持 Text 类型
                            var createSql = $"CREATE TABLE {tableNmae}" + @" (
    Id INT NOT NULL PRIMARY KEY,
    LineName varchar2(64) not null,
    MachineName varchar2(64) not null,    
    ProgramName varchar2(100) DEFAULT null,
    WorkOrder varchar2(50) DEFAULT null
)";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 2, 判断存在 sequence SEQ_T_LINECONFIG_ID 
                        var sequenceName = "SEQ_T_LINECONFIG_ID";
                        dynParams.Add(":sequenceName", OracleDbType.Varchar2, ParameterDirection.Input, sequenceName);
                        var querySequenceSql = "select count(0) from user_sequences where sequence_name = upper(:sequenceName)";
                        var querySequence = await DbConnection.ExecuteScalarAsync<string>(querySequenceSql, dynParams);
                        var seqExists = querySequence != "0";
                        if (!seqExists)
                        {
                            var createSql = $"create sequence {sequenceName}" + @"
    minvalue 1
    nomaxvalue
    increment by 1
    start with 1
    nocache";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 3, 判断存在 trigger TRI_T_LINECONFIG_INSERTID 
                        var triggerName = "TRI_T_LINECONFIG_INSERTID";
                        dynParams.Add(":triggerName", OracleDbType.Varchar2, ParameterDirection.Input, triggerName);
                        var queryTriggerSql = "select count(0) from user_triggers where trigger_name = upper(:triggerName)";
                        var queryTrigger = await DbConnection.ExecuteScalarAsync<string>(queryTriggerSql, dynParams);
                        var triggerExists = queryTrigger != "0";
                        if (!triggerExists)
                        {
                            var createSql = $"create or replace trigger {triggerName}" + @"
    before insert on T_LINECONFIG for each row 
    begin
        select SEQ_T_LINECONFIG_ID.Nextval into:new.Id from dual;
    end;";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 4, 判断存在 index IDX_UNI_T_LINECONFIG_LM 
                        var indexName = "IDX_UNI_T_LINECONFIG_LM";
                        dynParams.Add(":indexName", OracleDbType.Varchar2, ParameterDirection.Input, indexName);
                        var queryIndexSql = "select count(0) from user_indexes where table_name=upper(:tableNmae) and index_name=upper(:indexName)";
                        var queryIndex = await DbConnection.ExecuteScalarAsync<string>(queryIndexSql, dynParams);
                        var indexExists = queryIndex != "0";
                        if (!indexExists)
                        {
                            var createSql = $"CREATE UNIQUE INDEX {indexName} on T_LINECONFIG(LineName,MachineName)";
                            await DbConnection.ExecuteAsync(createSql);
                        }
                    }
                    #endregion

                    #region T_LOADCOMP 表
                    {
                        OracleDynamicParameters dynParams = new OracleDynamicParameters();

                        // 1. 查询是否存在 table T_LOADCOMPONENT  
                        var tableNmae = "T_LOADCOMP";
                        dynParams.Add(":tableNmae", OracleDbType.Varchar2, ParameterDirection.Input, tableNmae);
                        var tableExists = await DbConnection.ExecuteScalarAsync<string>("select count(*) from user_tables where table_name = :tableNmae", dynParams);
                        if (tableExists == "0") // 不存在
                        {
                            // Oracle 不支持 Text 类型
                            var createSql = $"CREATE TABLE {tableNmae}" + @" (
    Id INT NOT NULL PRIMARY KEY,
    T_LINECONFIGID INT NOT NULL,
    SeqID INT NOT NULL,
    Time date not null,
    ModuleNo INT NOT NULL,
    CONSTRAINT FK_LOADCOMP_LINECONFIGID FOREIGN KEY (T_LINECONFIGID) REFERENCES T_LINECONFIG(Id)
)";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 2, 判断存在 sequence SEQ_T_LOADCOMP_ID 
                        var sequenceName = "SEQ_T_LOADCOMP_ID";
                        dynParams.Add(":sequenceName", OracleDbType.Varchar2, ParameterDirection.Input, sequenceName);
                        var querySequenceSql = "select count(0) from user_sequences where sequence_name = upper(:sequenceName)";
                        var querySequence = await DbConnection.ExecuteScalarAsync<string>(querySequenceSql, dynParams);
                        var seqExists = querySequence != "0";
                        if (!seqExists)
                        {
                            var createSql = $"create sequence {sequenceName}" + @"
    minvalue 1
    nomaxvalue
    increment by 1
    start with 1
    nocache";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 3, 判断存在 trigger TRI_T_LOADCOMP_INSERTID 
                        var triggerName = "TRI_T_LOADCOMP_INSERTID";
                        dynParams.Add(":triggerName", OracleDbType.Varchar2, ParameterDirection.Input, triggerName);
                        var queryTriggerSql = "select count(0) from user_triggers where trigger_name = upper(:triggerName)";
                        var queryTrigger = await DbConnection.ExecuteScalarAsync<string>(queryTriggerSql, dynParams);
                        var triggerExists = queryTrigger != "0";
                        if (!triggerExists)
                        {
                            var createSql = $"create or replace trigger {triggerName}" + @"
    before insert on T_LOADCOMP for each row 
    begin
        select SEQ_T_LOADCOMP_ID.Nextval into:new.Id from dual;
    end;";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 4, 判断存在 index IDX_UNI_T_LOADCOMP_LMM 
                        var indexName = "IDX_UNI_T_LOADCOMP_LMM";
                        dynParams.Add(":indexName", OracleDbType.Varchar2, ParameterDirection.Input, indexName);
                        var queryIndexSql = "select count(0) from user_indexes where table_name=upper(:tableNmae) and index_name=upper(:indexName)";
                        var queryIndex = await DbConnection.ExecuteScalarAsync<string>(queryIndexSql, dynParams);
                        var indexExists = queryIndex != "0";
                        if (!indexExists)
                        {
                            var createSql = $"CREATE UNIQUE INDEX IDX_UNI_T_LOADCOMP_LMM on T_LOADCOMP(T_LINECONFIGID,ModuleNo)";
                            await DbConnection.ExecuteAsync(createSql);
                        }
                    }
                    #endregion


                    #region T_LOADCOMPONENT 表
                    {
                        OracleDynamicParameters dynParams = new OracleDynamicParameters();

                        // 1. 查询是否存在 table T_LOADCOMPONENT  
                        var tableNmae = "T_LOADCOMPONENT";
                        dynParams.Add(":tableNmae", OracleDbType.Varchar2, ParameterDirection.Input, tableNmae);
                        var tableExists = await DbConnection.ExecuteScalarAsync<string>("select count(*) from user_tables where table_name = :tableNmae", dynParams);
                        if (tableExists == "0") // 不存在
                        {
                            // Oracle 不支持 Text 类型
                            var createSql = $"CREATE TABLE {tableNmae}" + @" (
    Id INT NOT NULL PRIMARY KEY,
    T_LOADCOMPID INT NOT NULL,
    StageNo INT NOT NULL,
    SlotNo INT NOT NULL,
    PartNo varchar2(100) NOT NULL,
    FeederID varchar2(64) NOT NULL,
    ReelID varchar2(100) NOT NULL,
    Quantity INT NOT NULL,
    BatchID  varchar2(100) DEFAULT NULL,
    CONSTRAINT FK_LOADCOMPONENT_LOADCOMPID FOREIGN KEY (T_LOADCOMPID) REFERENCES T_LOADCOMP(Id)
)";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 2, 判断存在 sequence SEQ_T_LOADCOMPONENT_ID 
                        var sequenceName = "SEQ_T_LOADCOMPONENT_ID";
                        dynParams.Add(":sequenceName", OracleDbType.Varchar2, ParameterDirection.Input, sequenceName);
                        var querySequenceSql = "select count(0) from user_sequences where sequence_name = upper(:sequenceName)";
                        var querySequence = await DbConnection.ExecuteScalarAsync<string>(querySequenceSql, dynParams);
                        var seqExists = querySequence != "0";
                        if (!seqExists)
                        {
                            var createSql = $"create sequence {sequenceName}" + @"
    minvalue 1
    nomaxvalue
    increment by 1
    start with 1
    nocache";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 3, 判断存在 trigger TRI_T_LOADCOMPONENT_INSERTID 
                        var triggerName = "TRI_T_LOADCOMPONENT_INSERTID";
                        dynParams.Add(":triggerName", OracleDbType.Varchar2, ParameterDirection.Input, triggerName);
                        var queryTriggerSql = "select count(0) from user_triggers where trigger_name = upper(:triggerName)";
                        var queryTrigger = await DbConnection.ExecuteScalarAsync<string>(queryTriggerSql, dynParams);
                        var triggerExists = queryTrigger != "0";
                        if (!triggerExists)
                        {
                            var createSql = $"create or replace trigger {triggerName}" + @"
    before insert on T_LOADCOMPONENT for each row 
    begin
        select SEQ_T_LOADCOMPONENT_ID.Nextval into:new.Id from dual;
    end;";
                            await DbConnection.ExecuteAsync(createSql);
                        }

                        // 4, 判断存在 index IDX_UNI_T_LOADCOMPONENT_LS 
                        var indexName = "IDX_UNI_T_LOADCOMPONENT_LS";
                        dynParams.Add(":indexName", OracleDbType.Varchar2, ParameterDirection.Input, indexName);
                        var queryIndexSql = "select count(0) from user_indexes where table_name=upper(:tableNmae) and index_name=upper(:indexName)";
                        var queryIndex = await DbConnection.ExecuteScalarAsync<string>(queryIndexSql, dynParams);
                        var indexExists = queryIndex != "0";
                        if (!indexExists)
                        {
                            var createSql = $"CREATE UNIQUE INDEX IDX_UNI_T_LOADCOMPONENT_LS on T_LOADCOMPONENT(T_LOADCOMPID,SlotNo)";
                            await DbConnection.ExecuteAsync(createSql);
                        }
                    }
                    #endregion

                    #region T_UserInformation 表
                    {
                        OracleDynamicParameters dynParams = new OracleDynamicParameters();

                        // 1. 查询是否存在 table T_UserInformation  
                        var tableNmae = "T_UserInformation".ToUpper();
                        dynParams.Add(":tableNmae", OracleDbType.Varchar2, ParameterDirection.Input, tableNmae);
                        var tableExists = await DbConnection.ExecuteScalarAsync<string>("select count(*) from user_tables where table_name = :tableNmae", dynParams);
                        if (tableExists == "0") // 不存在
                        {
                            // Oracle 不支持 Text 类型
                            var createSql = $"CREATE TABLE {tableNmae}" + @" ( 
    id NVARCHAR2(255) NOT NULL PRIMARY KEY, 
    Userid NVARCHAR2(255) NOT NULL UNIQUE,
    UserPassword NVARCHAR2(255) NOT NULL
)";
                            await DbConnection.ExecuteAsync(createSql);
                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 关闭DbConnection
        /// </summary>
        public void Dispose()
        {
            if (this.DbConnection == null)
                return;

            if (this._disposed)
                return;

            this._disposed = true;

            if (this.DbConnection.State != ConnectionState.Closed)
                this.DbConnection.Close(); // DbConnection.Dispose()
        }
    }
}
