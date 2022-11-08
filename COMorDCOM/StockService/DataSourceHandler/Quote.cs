using System.Reflection.Utitlities;
namespace StockService.DataSourceHandler
{
    internal class Quote
    {
        [DataMap("Price")]                             //后续介绍 DataMapAttribute 
        public double regularMarketPrice { get; set; } //需要匹配 Yahoo finance API 的属性名
        [DataMap("Change")]
        public double regularMarketChange { get; set; }
        [DataMap("Trade Time")]
        public double regularMarketTime { get; set; }
        [DataMap("ChangePercent")]
        public double regularMarketChangePercent { get; set; }
        [DataMap("Day High")]
        public double regularMarketDayHigh { get; set; }
        [DataMap("Day Low")]
        public double regularMarketDayLow { get; set; }
        [DataMap("Bid")]
        public double bid { get; set; }
        [DataMap("Ask")]
        public double ask { get; set; }
        [DataMap("Open")]
        public double regularMarketOpen { get; set; }
        [DataMap("Symbol")]
        public string symbol { get; set; }
    }

}
