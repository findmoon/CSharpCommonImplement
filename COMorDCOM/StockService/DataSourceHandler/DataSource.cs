using Newtonsoft.Json.Linq;
using StockService.DataSourceHandler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Utitlities;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace StockService
{
    internal class DataSource
    {
        private static Dictionary<string, Quote> myQuotes =
               new Dictionary<string, Quote>();       // 存储股票信息
        public event EventHandler<object> DataUpdated; // 通知 报价服务器(QuoteServer) 的事件

        private Timer myTimer;
        public DataSource()
        {
            myTimer = new Timer();                     // 自动刷新的timer
            myTimer.Interval = 30 * 1000;              // 初始设置为30秒刷新一次
                                                     
            myTimer.Elapsed += MyTimerElapsed;         // 定时处理
            myTimer.Start();                           // 开始定时器
        }
        /// <summary>
        /// 定时获取股票信息【获取的数据只是放在当前 QuoteServer 中，然后通知 RTD 数据更改，RTD通过 RefreshData->GetQuoteData 来获取】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (myQuotes.Count == 0)
                return;
            string symbols = string.Empty;
            foreach (var quote in myQuotes) // 从本地数据获取所有的symbols
            {
                symbols += quote.Key + ",";

            }
            symbols = symbols.Remove(symbols.Length - 1);
            // var connect = new YFConnect();
            //var data = Task.Run<List<Quote>>(async () => await connect.GetQuoteAsync(symbols));
            var data = YFConnect.GetQuoteAsync(symbols);
            var list = data.Result;
            if (list!=null)
            {
                var hadUpdated=false;
                foreach (var item in list)
                {
                    if (item.regularMarketTime != myQuotes[item.symbol].regularMarketTime)
                    {
                        myQuotes[item.symbol] = item; // 使用新数据更新旧数据
                        hadUpdated=true;
                    }
                }
                if (hadUpdated && DataUpdated != null)
                {
                    // DataUpdated?.Invoke(this, "DataUpdated");
                    DataUpdated(this, "DataUpdated");   // 通知数据更改，该事件由 QuoteServer 类 订阅
                }
            }
        }

        /// <summary>
        /// 获取股票报价信息
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private Quote GetQuote(string symbol)
        {
            if (myQuotes.ContainsKey(symbol))
                return myQuotes[symbol];


            var dataResult = YFConnect.GetQuoteAsync(symbol).Result;
            if (dataResult == null || dataResult.Count == 0)
                return null;

            myQuotes[symbol] = dataResult[0];
            return myQuotes[symbol];
        }
        /// <summary>
        /// 获取报价数据
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="topicType"></param>
        /// <returns></returns>
        internal object GetQuoteData(string symbol, string topicType)
        {
            var quote = GetQuote(symbol);
            if (quote == null)
                return null;
            return quote.ToValue(topicType);
        }

        /// <summary>
        /// 移除指定 symbol 的报价数据
        /// </summary>
        /// <param name="symbol"></param>
        internal void Remove(string symbol)
        {
            lock (myQuotes)
            {
                myQuotes.Remove(symbol);
            }
        }

        /// <summary>
        /// 关闭或清理数据源
        /// </summary>
        internal void Close()
        {
            myTimer.Stop();
            myQuotes.Clear();
        }
        /// <summary>
        /// 修改数据刷新的间隔，单位 秒
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        internal bool SetInerval(double interval)
        {
            try
            {
                myTimer.Stop();
                myTimer.Interval = interval * 1000;
                myTimer.Start();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
    }

    /// <summary>
    /// 雅虎连接类
    /// </summary>
    internal static class YFConnect
    {
        const string YahooUrl = "https://query1.finance.yahoo.com/v7/finance/quote?symbols=";
        internal static async Task<List<Quote>> GetQuoteAsync(string symbol)
        {
            string url = $"{YahooUrl}{symbol}";
            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url)) // ;
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(responseBody);
                        var result = data.Descendants()
                                .OfType<JProperty>()
                                .Where(p => p.Name == "result")
                        .First()
                        .Value;
                        var results = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Quote>>(result.ToString());

                        return results;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());

                    }
                }
            }
            return null;
        }
    }
}
