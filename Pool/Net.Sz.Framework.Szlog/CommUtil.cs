using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Szlog
{

    /// <summary>
    /// 初始化辅助函数
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// <para>默认是不打印栈桢的，因为比较耗时：如果需要请设置 LOGSTACKTRACE = true 或者 ↓↓↓</para>
    /// <para>AppSettings 设置 log_print_stackrace         日志是否输出调用栈桢 true or false</para>
    /// <para>AppSettings 设置 log_print_console           日志是否输出到控制台 true or false</para>
    /// <para>AppSettings 设置 log_print_level             日志的等级,忽律大小写 DEBUG INFO WARN ERROR</para>
    /// <para>AppSettings 设置 log_print_path              日志的文件名带目录，log/sz.log</para>
    /// <para>AppSettings 设置 log_print_file              日志是否输出到文件 true or false</para>
    /// <para>AppSettings 设置 log_print_file_buffer       日志双缓冲输出到文件 true or false</para>
    /// </summary>
    public class CommUtil
    {
        /// <summary>
        /// 日志路径存储
        /// </summary>
        internal static string LOGPATH = "log/sz.log";

        /// <summary>
        /// 日志等级
        /// <para>默认 LogLevel.DEBUG 打印</para>
        /// </summary>
        public static LogLevel LOG_PRINT_LEVEL = LogLevel.DEBUG;

        /// <summary>
        /// 是否显示控制台消息
        /// <para>默认 true 打印</para>
        /// </summary>
        public static bool LOG_PRINT_CONSOLE = true;

        /// <summary>
        /// 是否输出文件消息
        /// <para>默认 true 打印</para>
        /// </summary>
        public static bool LOG_PRINT_FILE = true;
        /// <summary>
        /// 输出日志到文件的时候使用buff双缓冲减少磁盘IO,可能导致日志打印不及时
        /// <para>双缓冲对输出到控制台不受印象</para>
        /// <para>默认 true</para>
        /// </summary>
        public static bool LOG_PRINT_FILE_BUFFER = true;

        /// <summary>
        /// 是否打印栈桢
        /// <para>默认 false 不打印</para>
        /// </summary>
        public static bool LOG_PRINT_STACKTRACE = false;


        /// <summary>
        /// 设置日志输出目录
        /// </summary>
        /// <param name="path"></param>
        static public void SetLogRootPath(string logPath)
        {
            ResetLogDirectory(logPath);
            LOGPATH = logPath;
        }

        /// <summary>
        /// 构建输出目录
        /// </summary>
        /// <param name="logPath"></param>
        static public void ResetLogDirectory(string logPath)
        {
            string bpath = System.IO.Path.GetDirectoryName(logPath);
            if (!Directory.Exists(bpath)) { Directory.CreateDirectory(bpath); }
        }


        /// <summary>
        /// 友好方法，不对外，初始化
        /// </summary>
        internal static void InitConfig()
        {
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("log_print_path"))
            {
                string log_print_path = System.Configuration.ConfigurationManager.AppSettings["log_print_path"].ToString();
                SetLogRootPath(log_print_path);
            }
            else SetLogRootPath(LOGPATH);

            Console.WriteLine("当前日志存储路径：" + LOGPATH);

            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("log_print_level"))
            {
                string log_print_level = System.Configuration.ConfigurationManager.AppSettings["log_print_level"].ToString();
                if (!Enum.TryParse(log_print_level, false, out LOG_PRINT_LEVEL))
                    LOG_PRINT_LEVEL = LogLevel.DEBUG;
            }

            Console.WriteLine("当前日志级别：" + LOG_PRINT_LEVEL);

            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("log_print_file"))
            {
                string log_print_file = System.Configuration.ConfigurationManager.AppSettings["log_print_file"].ToString();
                if (!Boolean.TryParse(log_print_file, out LOG_PRINT_FILE))
                    LOG_PRINT_FILE = true;
            }

            Console.WriteLine("当前日志是否输出文件：" + LOG_PRINT_FILE);

            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("log_print_file_buffer"))
            {
                string log_print_file_buffer = System.Configuration.ConfigurationManager.AppSettings["log_print_file_buffer"].ToString();
                if (!Boolean.TryParse(log_print_file_buffer, out LOG_PRINT_FILE_BUFFER))
                    LOG_PRINT_FILE_BUFFER = true;
            }

            Console.WriteLine("当前日志buff双缓冲输出文件：" + LOG_PRINT_FILE_BUFFER);

            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("log_print_console"))
            {
                string log_print_console = System.Configuration.ConfigurationManager.AppSettings["log_print_console"].ToString();
                if (!Boolean.TryParse(log_print_console, out LOG_PRINT_CONSOLE))
                    LOG_PRINT_CONSOLE = true;
            }

            Console.WriteLine("当前日志是否输出控制台：" + LOG_PRINT_CONSOLE);

            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("logs_print_tackrace"))
            {
                string logs_print_tackrace = System.Configuration.ConfigurationManager.AppSettings["logs_print_tackrace"].ToString();
                if (!Boolean.TryParse(logs_print_tackrace, out LOG_PRINT_STACKTRACE))
                    LOG_PRINT_STACKTRACE = false;
            }

            Console.WriteLine("当前日志是否输出栈桢：" + LOG_PRINT_STACKTRACE);
        }

    }
}
