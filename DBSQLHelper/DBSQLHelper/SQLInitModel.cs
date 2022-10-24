namespace System.Data
{
    /// <summary>
    /// SQLHelper初始化的对象类
    /// </summary>
    public class SQLInitModel
    {
        public SQLInitModel(string ipInstance, string userName, string password, string dbName)
        {
            IpInstance = ipInstance;
            UserName = userName;
            Password = password;
            DBName = dbName;
        }
        public string IpInstance { get; }
        public string UserName { get;  }
        public string Password { get;  }
        public string DBName { get;  }
    }
}
