using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace StockService
{
    [Guid("0764A600-04AA-479A-B0D2-62FF118B55BF"), // 需要自己生成GUID 
     ProgId("Stock.QuoteServer.forTest"),          // 程序id
     ComVisible(true)]                             // 对当前COM的可访问性
    public class QuoteServer: IRtdServer
    {
        private IRTDUpdateEvent rtdCallback;       // 表示 real-time data 事件
        private Dictionary<int, Topic> myTopics;    // 存放excel RTD打开或创建时发送过来的数据元素
        private DataSource myData;                 // 存放股票信息的数据类
        public QuoteServer()
        {
            myTopics = new Dictionary<int, Topic>(); //来自Excel的输入
            myData = new DataSource();               //存放报价记录
            myData.DataUpdated += MyDataUpdated;     //数据源的数据更新事件
        }

        /// <summary>
        /// The ServerStart method is called immediately after a real-time data server is instantiated. 
        /// Negative value or zero indicates failure to start the server; positive value indicates success.
        /// </summary>
        /// <param name="CallbackObject"></param>
        /// <returns></returns>
        public int ServerStart(IRTDUpdateEvent CallbackObject)
        {
            rtdCallback = CallbackObject;
            return 1;
        }

        /// <summary>
        /// Adds new topics from a real-time data server. 
        /// The ConnectData method is called when a file is opened that contains real-time data functions 
        /// or when a user types in a new formula which contains the RTD function.
        /// </summary>
        /// <param name="TopicID"></param>
        /// <param name="Strings"></param>
        /// <param name="GetNewValues"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public dynamic ConnectData(int TopicID, ref Array Strings, ref bool GetNewValues)
        {
            if (Strings.Length < 2)
            {
                return "Two Parameters required"; // 返回给Excel
            }


            string symbol = Strings.GetValue(0).ToString();
            string topicType = Strings.GetValue(1).ToString();
            if (symbol == "RefreshRate")
            {
                double interval;
                if (!double.TryParse(topicType, out interval))
                {
                    return "Excel Formular : \"=RTD(\"Stock.QuoteServer.forTest\",,\"RefreshRate\",30\") for 30 second";
                }
                if (myData.SetInerval(interval))
                    return $"RereshRate set to {interval} seconds";
                return "Refresh set failed";
            }

            var topic = new Topic() { TopicId = TopicID, Symbol = symbol, TopicType = topicType };
            myTopics[TopicID] = topic;
            var ret = myData.GetQuoteData(symbol, topicType);
            return ret;
        }

        /// <summary>
        /// This method is called by Microsoft Excel to get new data.
        /// </summary>
        /// <param name="TopicCount"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Array RefreshData(ref int TopicCount)
        {
            // 返回多列的2行数据
            object[,] data = new object[2, this.myTopics.Count];
            int index = 0;
            foreach (var item in myTopics)
            {
                data[0, index] = item.Key;
                data[1, index] = myData.GetQuoteData(item.Value.Symbol, item.Value.TopicType);
                index++;
            }

            TopicCount = myTopics.Count;
            return data;
        }
        /// <summary>
        /// Notifies a real-time data (RTD) server application that a topic is no longer in use.
        /// 当公式更改时由Excel调用
        /// </summary>
        /// <param name="TopicID"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DisconnectData(int TopicID)
        {
            var symbol = myTopics[TopicID].Symbol;

            myTopics.Remove(TopicID);

            var v = myTopics.Where(n => n.Value.Symbol == symbol);
            if (v.Count() == 0)
            {
                myData.Remove(symbol);
            }
        }

        /// <summary>
        /// Determines if the real-time data server is still active. 
        /// Zero or a negative number indicates failure; a positive number indicates that the server is active.
        /// 由 Excel 每 15 秒调用一次，保持 server和连接 alive
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Heartbeat()
        {
            return 1; // 仅返回1。 可考虑使用其他判断当前服务处于可用状态
        }
        /// <summary>
        /// Terminates the connection to the real-time data server.
        /// 当Excel应用退出时，由Excel调用
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void ServerTerminate()
        {
            myData.Close();
            myTopics.Clear();   // 清除本地数据

        }

        /// <summary>
        /// 订阅的数据源的数据更新事件方法，在新数据更新时通知Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void MyDataUpdated(object sender, object arg)
        {
            // The real-time data (RTD) server uses this method to notify Microsoft Excel that new data has been received
            rtdCallback?.UpdateNotify(); // rtdCallback 从Excel传递过来。通知Excel已经数据更新
        }
    }

    /// <summary>
    /// 从 Excel RTD 初次使用时传递过来的主题数据
    /// </summary>
    internal class Topic
    {
        public int TopicId { get; set; }         //the value passed from Excel
        public string Symbol { get; set; }       //the value passed from Excel
        public string TopicType { get; set; }    //the value passed from Excel
    }
}
