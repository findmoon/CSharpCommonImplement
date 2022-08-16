using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeStampsCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timestampsTxt.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var unixTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var milliTimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            //var unixTimeStamp1 = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //var milliTimeStamp1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            
            //var dateTimeOffset = DateTimeOffset.Parse("2022-01-01 12:12:12");
        
            var datetimeFromSeconds = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp);
            var datetimeFromMilliSeconds= DateTimeOffset.FromUnixTimeMilliseconds(milliTimeStamp);

            button2_Click(null, null);

            var calcTimeStamp = CalcTimeStamps();
            var calcMilliTimeStamp = CalcTimeStamps(true);

            var calcDateTimeFromSeconds = GetDateTimeFromTimeStamp(calcTimeStamp);
            var calcDateTimeFromMilliSeconds = GetDateTimeFromMilliTimeStamp(calcMilliTimeStamp);

            var info = new string[]
            {
                $"当前(Unix)时间戳：{unixTimeStamp}",
                $"当前(Unix)毫秒时间戳：{milliTimeStamp}",
                $"计算的时间戳：{calcTimeStamp}",
                $"计算的毫秒时间戳：{calcMilliTimeStamp}",
                $"从时间戳获取UTC时间：{datetimeFromSeconds.DateTime}",
                $"从时间戳获取本地时间：{datetimeFromSeconds.LocalDateTime}",
                $"从毫秒时间戳获取UTC时间：{datetimeFromMilliSeconds.DateTime}",
                $"从毫秒时间戳获取本地时间：{datetimeFromMilliSeconds.LocalDateTime}",
                $"从时间戳计算本地时间：{calcDateTimeFromSeconds}",
                $"从毫秒时间戳计算本地时间：{calcDateTimeFromMilliSeconds}",
            };
            timestampsTxt.Text = string.Join("\r\n\r\n", info);
        }

        /// <summary>
        /// 获取当前时间的时间戳
        /// </summary>
        /// <param name="milliSeconds"></param>
        /// <returns></returns>
        public static long CalcTimeStamps(bool milliSeconds=false)
        {
            return CalcTimeStamps(DateTime.Now,milliSeconds);
        }
        /// <summary>
        /// 获取指定时间的时间戳
        /// </summary>
        /// <param name="dateTime">当地时间</param>
        /// <param name="milliSeconds"></param>
        /// <returns></returns>
        public static long CalcTimeStamps(DateTime dateTime,bool milliSeconds = false)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区的“1970, 1, 1”
            var timeSpan = dateTime - startTime;
            return (long)(milliSeconds ? timeSpan.TotalMilliseconds : timeSpan.TotalSeconds);
        }

        /// <summary>
        /// 获取Utc时间的时间戳
        /// </summary>
        /// <param name="milliSeconds"></param>
        /// <returns></returns>
        public static long CalcTimeStampsUTC(bool milliSeconds = false)
        {
            return CalcTimeStampsUTC(DateTime.UtcNow, milliSeconds);
        }
        /// <summary>
        /// 获取指定Utc时间的时间戳
        /// </summary>
        /// <param name="dateTime">当地时间</param>
        /// <param name="milliSeconds"></param>
        /// <returns></returns>
        public static long CalcTimeStampsUTC(DateTime utcDateTime, bool milliSeconds = false)
        {
            var timeSpan = utcDateTime - new DateTime(1970, 1, 1);
            return (long)(milliSeconds ? timeSpan.TotalMilliseconds : timeSpan.TotalSeconds);
        }

        /// <summary>
        /// 从Unix时间戳获取对应的时间
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromTimeStamp(long seconds)
        {
            var dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1).AddSeconds(seconds)); // 当地时区时间
            return dt;
        }
        /// <summary>
        /// 从毫秒时间戳获取对应的时间
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromMilliTimeStamp(long milliSeconds)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1).AddMilliseconds(milliSeconds));
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // 获取Ticks
            var datetimeTicks = DateTime.Now.Ticks;
            var dateTimeOffsetTicks = DateTimeOffset.Now.Ticks;
            var dateTimeOffsetUtcTicks = DateTimeOffset.Now.UtcTicks;
            var utcDateTimeOffsetTicks = DateTimeOffset.UtcNow.Ticks;
            var utcDateTimeOffsetUtcTicks = DateTimeOffset.UtcNow.UtcTicks;
            var info = new string[]
            {
                $"Datetime对象当前时间的Ticks：{datetimeTicks}",
                $"DatetimeOffset对象当前时间的Ticks：{dateTimeOffsetTicks}",
                $"DatetimeOffset对象当前时间的UtcTicks：{dateTimeOffsetUtcTicks}",
                $"DatetimeOffset对象UTC时间的Ticks：{utcDateTimeOffsetTicks}",
                $"DatetimeOffset对象UTC时间的UtcTicks：{utcDateTimeOffsetUtcTicks}",
            };
            ticksTxt.Text = string.Join("\r\n\r\n", info);

            // 由ticks计算DateTimeOffset
            // 由Tick获取当前北京时间的dateTimeOffset
            var dateTimeOffset = new DateTimeOffset(datetimeTicks,TimeSpan.FromHours(8));
            // UtcTick获取DateTimeOffset
            var dateTimeOffsetUtc = new DateTimeOffset(dateTimeOffsetUtcTicks, TimeSpan.FromSeconds(0));

            ticksTxt.Text += "\r\n\r\ndateTimeOffset：" + dateTimeOffset;
            ticksTxt.Text += "\r\n\r\n当前时间："+ dateTimeOffset.LocalDateTime;
            ticksTxt.Text += "\r\n\r\n当前时间："+ dateTimeOffset.DateTime;
            ticksTxt.Text += "\r\n\r\ndateTimeOffsetUtc：" + dateTimeOffsetUtc;
            ticksTxt.Text += "\r\n\r\n当前本地时间："+ dateTimeOffsetUtc.LocalDateTime;
            ticksTxt.Text += "\r\n\r\n当前UTC时间："+ dateTimeOffsetUtc.DateTime;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dtNowUtcNowTxt.Text = $"DateTime.Now:{DateTime.Now} 和 DateTime.UtcNow:{DateTime.UtcNow}";
            dtNowUtcNowTxt.Text += $"\r\n DateTimeOffset.Now:{DateTimeOffset.Now} 和 DateTimeOffset.UtcNow:{DateTimeOffset.UtcNow}";
            dtNowUtcNowTxt.Text += $"\r\n DateTimeOffset.Now.DateTime:{DateTimeOffset.Now.DateTime} 和 DateTimeOffset.Now.LocalDateTime:{DateTimeOffset.Now.LocalDateTime}";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var dt = DateTime.Now.ToString("u"); //2021-12-23 16:38:02Z
            var dt2 = DateTime.Parse(dt); // 2021-12-24 0:38:02

            var k= DateTime.UtcNow;
        }
    }
}
