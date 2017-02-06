using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Utils
{
    /// <summary>
    /// 时间验证器
    /// </summary>
    public static class TimeUtil
    {

        //static void Main(string[] args)
        //{
        //    Console.WriteLine("验证当期时间是否满足活动开放时间：[*][*][20/22][*][10:00-11:59/16:00-17:59]");
        //    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        //    watch.Restart();
        //    for (int i = 0; i < 5; i++)
        //    {
        //        long ticks = TimeUtil.VerifyDateTime("[2014/2016][9][*][*][10:00-11:59/16:00-22:59]");
        //        Console.WriteLine(ticks + " 倒计时：" + (ticks / 1000) + "秒");
        //    }
        //    watch.Stop();
        //    Console.WriteLine(watch.ElapsedMilliseconds);
        //    Console.ReadLine();
        //}


        private static DateTime _TS1970_Local = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Local);
        private static Int64 _TS1970_Local_Ticks = Convert.ToInt64(TimeSpan.FromTicks(_TS1970_Local.Ticks).TotalMilliseconds);
        private static Int64 _TS1970_UTC_Ticks =_TS1970_Local_Ticks- Convert.ToInt64(TimeSpan.FromTicks(_TS1970_Local.ToUniversalTime().Ticks).TotalMilliseconds);

        /// <summary>
        /// 将毫秒数转化成当前时间
        /// </summary>
        public static DateTime DateNow(Int64 milliseconds)
        {
            DateTime ret = new DateTime(TimeSpan.FromMilliseconds(Convert.ToDouble(milliseconds)).Ticks, System.DateTimeKind.Local);
            return ret;
        }

        public static DateTime DateNow_Java(Int64 milliseconds)
        {
            //加上本地时间差，就和国家UTC时间一致
            DateTime ret = new DateTime(TimeSpan.FromMilliseconds(Convert.ToDouble(milliseconds + _TS1970_Local_Ticks + _TS1970_UTC_Ticks)).Ticks);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static Int64 CurrentTimeMillis()
        {
            return CurrentTimeMillis(DateTime.Now);
        }

        /// <summary>
        /// 获取当前时间和 1970-01-01 00:00:00 的时间差
        /// <para>为了和java一致使用的是 UTC 时间</para>
        /// </summary>
        /// <returns></returns>
        public static Int64 CurrentTimeMillis_Java()
        {
            //为了和java保持一致
            return CurrentTimeMillis_Java(DateTime.Now);
        }

        /// <summary>
        /// 获取当前时间所表示的毫秒数
        /// </summary>
        public static Int64 CurrentTimeMillis(DateTime dt)
        {
            return Convert.ToInt64(TimeSpan.FromTicks(dt.Ticks).TotalMilliseconds);
        }

        /// <summary>
        /// 获取当前时间所表示的毫秒数
        /// <para>为了和java一致使用的是 UTC 时间</para>
        /// </summary>
        public static Int64 CurrentTimeMillis_Java(DateTime dt)
        {
            //减去本地时间差，就和国家UTC时间一致
            return Convert.ToInt64(TimeSpan.FromTicks(dt.ToUniversalTime().Ticks).TotalMilliseconds) - _TS1970_Local_Ticks;
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss:fff：
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static String NowString(this System.DateTime d)
        {
            return d.ToString("yyyy-MM-dd HH:mm:ss:ffff: ");
        }

        #region 获取开始倒计时 验证时间:[*][*][20/22][*][10:00-11:59/16:00-17:59] static public long VerifyDateTime(string timeStr)
        /// <summary>
        /// 获取开始倒计时 验证时间:[*][*][20/22][*][10:00-11:59/16:00-17:59]
        /// <para>第一个是年，，第二个是月，第三个是日期，第四个是星期，第五个是时间，</para>
        /// <para>每一个参数，"-" 表示 到 如：“2015-2017”表示 2015 到 2017, "/"  表示 或者 如： “2015/2017”表示2015 或者 2017</para>
        /// <para>返回值 -1 表示永久过期，0 表示在时间规则内，大于 0 表示倒计时</para>
        /// </summary> 
        static public long VerifyDateTime(string timeStr)
        {
            DateTime date = DateTime.Now;
            return VerifyDateTime(date, timeStr);
        }
        /// <summary>
        /// 获取开始倒计时 验证时间:[*][*][20/22][*][10:00-11:59/16:00-17:59]
        /// <para>第一个是年，，第二个是月，第三个是日期，第四个是星期，第五个是时间，</para>
        /// <para>每一个参数，"-" 表示 到 如：“2015-2017”表示 2015 到 2017, "/"  表示 或者 如： “2015/2017”表示2015 或者 2017</para>
        /// <para>返回值 -1 表示永久过期，0 表示在时间规则内，大于 0 表示倒计时</para>
        /// </summary> 
        static public long VerifyDateTime(DateTime date, string timeStr)
        {
            var items = Regex.Split(timeStr, @";|；");
            items.Reverse();
            long ret = -1;

            foreach (var item in items)
            {
                //验证时间匹配
                if (VerifyConfigTimeStr(date, item))
                {
                    ret = 0;
                    goto Lab_Exit;
                }
                //未通过时间匹配，检查返回剩余时间
                string[] timeStrs = item.Replace("[", "").Split(new char[] { ']' });

                string times = timeStrs[4];
                string weeks = timeStrs[3];
                string days = timeStrs[2];
                string months = timeStrs[1];
                string years = timeStrs[0];

                int hour = 0, minute = 0, second = 0;
                var tempYears = GetConfigDate(date, date.Year, years);
                var tempMonths = GetConfigDate(date, date.Month, months);
                var tempDays = GetConfigDate(date, date.Day, days);
                //由于星期比较特殊所以获取与星期相关的日期的时候有点诡异。
                if (!"*".Equals(weeks))
                {
                    if (weeks.IndexOf("-") > 0)
                    {
                        //星期的间隔模式
                        string[] weekSplit = weeks.Split('-');
                        int weekmin = 9999;
                        int.TryParse(weekSplit[0], out weekmin);
                        int weekmax = 9999;
                        int.TryParse(weekSplit[1], out weekmax);
                        ActionWeekDay(weekmin, weekmax, ref tempDays, ref tempMonths, ref tempYears);
                    }
                    else if (weeks.IndexOf("/") > 0)
                    {
                        //星期的或模式
                        string[] weeksSplit = weeks.Split('/');
                        int tempWeek;
                        if (int.TryParse(weeksSplit[0], out tempWeek))
                        {
                            if (0 <= tempWeek && tempWeek <= 7)
                            {
                                ActionWeekDay(tempWeek, tempWeek, ref tempDays, ref tempMonths, ref tempYears);
                            }
                        }
                    }
                    else
                    {
                        //特定星期的模式
                        int tempweek = 0;
                        if (int.TryParse(weeks, out tempweek))
                        {
                            ActionWeekDay(tempweek, tempweek, ref tempDays, ref tempMonths, ref tempYears);
                        }
                    }
                }
                else
                {
                    //未指定星期的模式
                    ActionWeekDay(1, 7, ref tempDays, ref tempMonths, ref tempYears);
                }

                var tempHHMMs = GetConfigTimeStr(times);

                //进行简单的排序
                tempYears.Sort();
                tempMonths.Sort();
                tempDays.Sort();
                tempHHMMs.Sort();

                //接下来这里是天坑，就是构造时间器比较，然后计算出倒计时
                for (int y = 0; y < tempYears.Count; y++)
                {
                    for (int m = 0; m < tempMonths.Count; m++)
                    {
                        for (int d = 0; d < tempDays.Count; d++)
                        {
                            for (int h = 0; h < tempHHMMs.Count; h++)
                            {
                                string[] hhmm = Regex.Split(tempHHMMs[h], ":|：");
                                if (int.TryParse(hhmm[0], out hour) && int.TryParse(hhmm[1], out minute))
                                {
                                    DateTime actionTime = new DateTime(tempYears[y], tempMonths[m], tempDays[d], hour, minute, second);
                                    if (actionTime > date)
                                    {
                                        if (VerifyConfigTimeStr(actionTime, item))
                                        {
                                            Console.WriteLine(actionTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                            TimeSpan ts = (actionTime - date);
                                            long tmpRet = ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds;
                                            tmpRet *= 1000;
                                            tmpRet += ts.Milliseconds;
                                            if (ret == -1 || ret > tmpRet)
                                            {
                                                ret = tmpRet;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        Lab_Exit:
            return ret;
        }
        #endregion

        #region 获取活动结束时间倒计时 验证时间:[*][*][20/22][*][10:00-11:59/16:00-17:59] static public long VerifyDateTime(string timeStr)
        /// <summary>
        /// 获取活动结束时间倒计时 验证时间:[*][*][20/22][*][10:00-11:59/16:00-17:59]
        /// <para>第一个是年，，第二个是月，第三个是日期，第四个是星期，第五个是时间，</para>
        /// <para>每一个参数，"-" 表示 到 如：“2015-2017”表示 2015 到 2017, "/"  表示 或者 如： “2015/2017”表示2015 或者 2017</para>
        /// <para>返回值 -1 表示永久过期，0 表示在时间规则内，大于 0 表示倒计时</para>
        /// </summary> 
        static public long VerifyDateEndTime(string timeStr)
        {
            DateTime date = DateTime.Now;
            return VerifyDateEndTime(date, timeStr);

        }
        /// <summary>
        /// 获取活动结束时间倒计时 验证时间:[*][*][20/22][*][10:00-11:59/16:00-17:59]
        /// <para>第一个是年，，第二个是月，第三个是日期，第四个是星期，第五个是时间，</para>
        /// <para>每一个参数，"-" 表示 到 如：“2015-2017”表示 2015 到 2017, "/"  表示 或者 如： “2015/2017”表示2015 或者 2017</para>
        /// <para>返回值 -1 表示永久过期，0 表示在时间规则内，大于 0 表示倒计时</para>
        /// </summary> 
        static public long VerifyDateEndTime(DateTime date, string timeStr)
        {
            var items = Regex.Split(timeStr, @";|；");
            items.Reverse();
            long ret = -1;
            foreach (var item in items)
            {
                //验证时间匹配
                if (VerifyConfigTimeStr(date, item))
                {
                    //未通过时间匹配，检查返回剩余时间
                    string[] timeStrs = item.Replace("[", "").Split(new char[] { ']' });

                    string times = timeStrs[4];
                    string weeks = timeStrs[3];
                    string days = timeStrs[2];
                    string months = timeStrs[1];
                    string years = timeStrs[0];

                    int hour = 0, minute = 0, second = 0;

                    var tempYears = GetConfigDate(date, date.Year, years);
                    var tempMonths = GetConfigDate(date, date.Month, months);
                    var tempDays = GetConfigDate(date, date.Day, days);
                    //由于星期比较特殊所以获取与星期相关的日期的时候有点诡异。
                    if (!"*".Equals(weeks))
                    {
                        if (weeks.IndexOf("-") > 0)
                        {
                            //星期的间隔模式
                            string[] weekSplit = weeks.Split('-');
                            int weekmin = 9999;
                            int.TryParse(weekSplit[0], out weekmin);
                            int weekmax = 9999;
                            int.TryParse(weekSplit[1], out weekmax);
                            ActionWeekDay(weekmin, weekmax, ref tempDays, ref tempMonths, ref tempYears);
                        }
                        else if (weeks.IndexOf("/") > 0)
                        {
                            //星期的或模式
                            string[] weeksSplit = weeks.Split('/');
                            int tempWeek;
                            if (int.TryParse(weeksSplit[0], out tempWeek))
                            {
                                if (0 <= tempWeek && tempWeek <= 7)
                                {
                                    ActionWeekDay(tempWeek, tempWeek, ref tempDays, ref tempMonths, ref tempYears);
                                }
                            }
                        }
                        else
                        {
                            //特定星期的模式
                            int tempweek = 0;
                            if (int.TryParse(weeks, out tempweek))
                            {
                                ActionWeekDay(tempweek, tempweek, ref tempDays, ref tempMonths, ref tempYears);
                            }
                        }
                    }
                    else
                    {
                        //未指定星期的模式
                        //ActionWeekDay(1, 7, ref tempDays, ref tempMonths, ref tempYears);
                    }

                    var tempHHMMs = GetConfigEndTimeStr(times);

                    //进行简单的排序
                    tempYears.Sort();
                    tempMonths.Sort();
                    tempDays.Sort();
                    tempHHMMs.Sort();

                    //接下来这里是天坑，就是构造时间器比较，然后计算出倒计时
                    for (int y = 0; y < tempYears.Count; y++)
                    {
                        for (int m = 0; m < tempMonths.Count; m++)
                        {
                            for (int d = 0; d < tempDays.Count; d++)
                            {
                                for (int h = 0; h < tempHHMMs.Count; h++)
                                {
                                    string[] hhmm = Regex.Split(tempHHMMs[h], ":|：");
                                    if (int.TryParse(hhmm[0], out hour) && int.TryParse(hhmm[1], out minute))
                                    {
                                        try
                                        {
                                            DateTime actionTime = new DateTime(tempYears[y], tempMonths[m], tempDays[d], hour, minute, second);
                                            if (VerifyConfigEndTimeStr(actionTime, item))
                                            {
                                                if (hour == 23 && minute == 59)//如果是23时59分，那么到第二天凌晨在做对比
                                                {
                                                    actionTime = actionTime.AddMinutes(1);
                                                }
                                                //if (actionTime >= date)
                                                {
                                                    //Console.WriteLine(actionTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                                    TimeSpan ts = (actionTime - date);
                                                    long tmpRet = ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds;
                                                    tmpRet *= 1000;
                                                    tmpRet += ts.Milliseconds;
                                                    if (ret == -1 || ret < tmpRet)
                                                    {
                                                        ret = tmpRet;
                                                    }
                                                }
                                            }
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        Lab_Exit:
            return ret;
        }
        #endregion

        #region 处理星期包含的日期 日  static void ActionWeekDay(int weekmin, int weekmax, ref List<int> days, ref List<int> months, ref List<int> years)
        /// <summary>
        /// 处理星期包含的日期 日
        /// </summary>
        /// <param name="weekmin"></param>
        /// <param name="weekmax"></param>
        /// <param name="days"></param>
        /// <param name="months"></param>
        /// <param name="years"></param>
        static void ActionWeekDay(int weekmin, int weekmax, ref List<int> days, ref List<int> months, ref List<int> years)
        {
            DateTime nowWeekDate = DateTime.Now;
            List<int> tempDays, tempMonths, tempYears;
            tempYears = years.ToList();
            tempMonths = months.ToList();
            tempDays = days.ToList();
            foreach (var itemYear in tempYears)
            {
                foreach (var itemMonth in tempMonths)
                {
                    int itemDay = 1;
                    if (nowWeekDate.Month == itemMonth)
                    {
                        itemDay = nowWeekDate.Day;
                    }
                    DateTime date = new DateTime(itemYear, itemMonth, itemDay);
                    for (int i = 0; i < 7; i++)
                    {
                        int week = (int)date.DayOfWeek;
                        if (week == 0)
                        {
                            week = 7;
                        }
                        if (weekmin <= week && week <= weekmax)
                        {
                            if (!days.Contains(date.Day))
                            {
                                days.Add(date.Day);
                            }
                            if (!months.Contains(date.Month))
                            {
                                months.Add(date.Month);
                            }
                            if (!years.Contains(date.Year))
                            {
                                years.Add(date.Year);
                            }
                        }
                        date = date.AddDays(1);
                    }
                }
            }
        }
        #endregion

        #region 验证是否在活动时间内 static public bool VerifyConfigTimeStr(string timeStr)
        /// <summary>
        /// 验证时间:[*][*][20/22][*][10:00-11:59/16:00-17:59]
        /// <para>第一个是年，，第二个是月，第三个是日期，第四个是星期，第五个是时间，</para>
        /// <para>每一个参数，"-" 表示 到 如：“2015-2017”表示 2015 到 2017, "/"  表示 或者 如： “2015/2017”表示2015 或者 2017</para>
        /// </summary>        
        /// <returns></returns>
        static public bool VerifyConfigTimeStr(string timeStr)
        {
            String[] items = Regex.Split(timeStr, ";|；");
            DateTime date = DateTime.Now;
            foreach (var item in items)
            {
                if (VerifyConfigTimeStr(date, item))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 验证是否在配置时间内 static public bool VerifyConfigTimeStr(DateTime date, string timeStr)
        /// <summary>
        /// 验证时间:[*][*][20/22][*][10:00-11:59/16:00-17:59]
        /// <para>第一个是年，，第二个是月，第三个是日期，第四个是星期，第五个是时间，</para>
        /// <para>每一个参数，"-" 表示 到 如：“2015-2017”表示 2015 到 2017, "/"  表示 或者 如： “2015/2017”表示2015 或者 2017</para>
        /// </summary>        
        /// <returns></returns>
        static public bool VerifyConfigTimeStr(DateTime date, string timeStr)
        {
            string[] timeStrs = timeStr.Replace("[", "").Split(new char[] { ']' });

            if (VerifyDate(date.Year, timeStrs[0]))
            {
                if (VerifyDate(date.Month, timeStrs[1]))
                {
                    int week = (int)date.DayOfWeek;
                    if (week == 0) { week = 7; }//星期天
                    if (VerifyDate(week, timeStrs[3]))
                    {
                        if (VerifyDate(date.Day, timeStrs[2]))
                        {
                            if (VerifyTime(date, timeStrs[4])) { return true; }
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region 验证活动是否结束 static public bool VerifyConfigEndTimeStr(DateTime date, string timeStr)
        /// <summary>
        /// 验证活动是否结束
        /// 验证时间:[*][*][20/22][*][10:00-11:59/16:00-17:59]
        /// <para>第一个是年，，第二个是月，第三个是日期，第四个是星期，第五个是时间，</para>
        /// <para>每一个参数，"-" 表示 到 如：“2015-2017”表示 2015 到 2017, "/"  表示 或者 如： “2015/2017”表示2015 或者 2017</para>
        /// </summary>        
        /// <returns></returns>
        static public bool VerifyConfigEndTimeStr(DateTime date, string timeStr)
        {
            string[] timeStrs = timeStr.Replace("[", "").Split(new char[] { ']' });

            if (VerifyDate(date.Year, timeStrs[0]))
            {
                if (VerifyDate(date.Month, timeStrs[1]))
                {
                    int week = (int)date.DayOfWeek;
                    if (week == 0) { week = 7; }//星期天
                    if (VerifyDate(week, timeStrs[3]))
                    {
                        if (VerifyDate(date.Day, timeStrs[2]))
                        {
                            if (VerifyEndTime(date, timeStrs[4])) { return true; }
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region 验证当前时间 年，月，日，星期，是否符合 static bool VerifyDate(int nowItem, string items)
        /// <summary>
        /// 验证当前时间 年，月，日，星期，是否符合
        /// </summary>
        /// <param name="items">1-7;表示 1 到 7 , 1/7 表示 1 或者 7</param>
        /// <returns></returns>
        static bool VerifyDate(int nowItem, string items)
        {
            string nowItemStr = nowItem.ToString();
            if ("*".Equals(items) || nowItemStr.Equals(items)) { return true; }
            else if (items.IndexOf("-") > 0)
            {//区间划分
                string[] itemSplit = items.Split('-');
                int item1 = 9999;
                int.TryParse(itemSplit[0], out item1);
                int item2 = 9999;
                int.TryParse(itemSplit[1], out item2);

                if (item1 <= nowItem && nowItem <= item2) { return true; }
            }
            else if (items.IndexOf("/") > 0)
            {//或划分
                string[] weeksSplit = items.Split('/');
                foreach (var item in weeksSplit)
                {
                    if (nowItemStr.Equals(item)) { return true; }
                }
            }
            return false;
        }
        #endregion

        #region 验证当期时间格式 static bool VerifyTime(DateTime date, string itemTime)
        /// <summary>
        /// 验证当期时间格式
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itemTime"></param>
        /// <returns></returns>
        static bool VerifyTime(DateTime date, string itemTime)
        {
            bool ret = false;
            if (!"00:00-23:59".Equals(itemTime) && !"*".Equals(itemTime))
            {
                var items = Regex.Split(itemTime, @"/");
                foreach (var item in items)
                {
                    string[] itemTimes = item.Split('-');
                    var hhmm = Regex.Split(itemTimes[0], @":|：");
                    int hh = 24;
                    int.TryParse(hhmm[0], out hh);
                    int mm = 60;
                    int.TryParse(hhmm[1], out mm);
                    if (date.Hour > hh || (date.Hour == hh && date.Minute >= mm))
                    {
                        var hhmm1 = Regex.Split(itemTimes[1], @":|：");
                        int hh1 = 24;
                        int.TryParse(hhmm1[0], out hh1);
                        int mm1 = 60;
                        int.TryParse(hhmm1[1], out mm1);
                        if (date.Hour < hh1 || (date.Hour == hh1 && date.Minute < mm1)) { ret = true; }
                        else { ret = false; }
                    }
                    else { ret = false; }
                    if (ret)
                    {
                        break;
                    }
                }
            }
            else { ret = true; }
            return ret;
        }
        #endregion

        #region 验证当期时间格式 static bool VerifyTime(DateTime date, string itemTime)
        /// <summary>
        /// 验证当期时间格式
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itemTime"></param>
        /// <returns></returns>
        static bool VerifyEndTime(DateTime date, string itemTime)
        {
            bool ret = false;
            if (!"00:00-23:59".Equals(itemTime) && !"*".Equals(itemTime))
            {
                var items = Regex.Split(itemTime, @"/");
                foreach (var item in items)
                {
                    string[] itemTimes = item.Split('-');
                    var hhmm = Regex.Split(itemTimes[1], @":|：");
                    int hh = 24;
                    int.TryParse(hhmm[0], out hh);
                    int mm = 60;
                    int.TryParse(hhmm[1], out mm);
                    if (date.Hour < hh || (date.Hour == hh && date.Minute <= mm))
                    {
                        ret = true;
                        break;
                    }
                    else { ret = false; }
                }
            }
            else { ret = true; }
            return ret;
        }
        #endregion

        #region 获取配置的年月日星期等信息 static List<int> GetConfigDate(DateTime date, int p1, string p3)
        /// <summary>
        /// 获取配置的年月日星期等信息
        /// </summary>
        /// <param name="date"></param>
        /// <param name="p1"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        static List<int> GetConfigDate(DateTime date, int p1, string p3)
        {
            List<int> rets = new List<int>();
            string p1Str = p1.ToString();
            if ("*".Equals(p3) || p1Str.Equals(p3))
            {
                rets.Add(p1);
            }
            else if (p3.IndexOf("-") > 0)
            {
                string[] weekSplit = p3.Split('-');
                int week1 = 9999;
                int.TryParse(weekSplit[0], out week1);

                int week2 = 9999;
                int.TryParse(weekSplit[1], out week2);
                for (int i = week1; i < week2 + 1; i++)
                {
                    rets.Add(i);
                }
            }
            else if (p3.IndexOf("/") > 0)
            {
                string[] weeksSplit = p3.Split('/');
                foreach (var item in weeksSplit)
                {
                    int temp = 0;
                    if (int.TryParse(item, out temp))
                    {
                        rets.Add(temp);
                    }
                }
            }
            else
            {
                int temp = 0;
                if (int.TryParse(p3, out temp))
                {
                    rets.Add(temp);
                }
            }
            return rets;
        }
        #endregion

        #region 获取配置的时间字符串 static List<string> GetConfigTimeStr(string itemTime)
        /// <summary>
        /// 获取配置的时间字符串 
        /// </summary>
        /// <param name="itemTime">必须类似的格式 单条 00:00-23:59  多条00:00-23:59/00:00-23:59</param>
        /// <returns></returns>
        static List<string> GetConfigTimeStr(string itemTime)
        {
            List<string> retObjs = new List<string>();
            // 00:00-23:59
            if (!"*".Equals(itemTime))
            {
                var items = Regex.Split(itemTime, @"/");
                foreach (var item in items)
                {
                    string[] itemTimes = item.Split('-');
                    retObjs.Add(itemTimes[0]);
                }
            }
            else
            {
                retObjs.Add("00:00");
            }
            return retObjs;
        }
        #endregion

        #region 获取配置的时间字符串 static List<string> GetConfigTimeStr(string itemTime)
        /// <summary>
        /// 获取配置的时间字符串 
        /// </summary>
        /// <param name="itemTime">必须类似的格式 单条 00:00-23:59  多条00:00-23:59/00:00-23:59</param>
        /// <returns></returns>
        static List<string> GetConfigEndTimeStr(string itemTime)
        {
            List<string> retObjs = new List<string>();
            // 00:00-23:59
            if (!"*".Equals(itemTime))
            {
                var items = Regex.Split(itemTime, @"/");
                foreach (var item in items)
                {
                    string[] itemTimes = item.Split('-');
                    retObjs.Add(itemTimes[1]);
                }
            }
            else
            {
                retObjs.Add("23:59");
            }
            return retObjs;
        }
        #endregion

    }
}
