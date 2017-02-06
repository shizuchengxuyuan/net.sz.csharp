using Net.Sz.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Log
{
    /// <summary>
    /// 日志辅助
    /// <para>AppSettings 设置 LogRootPath 为日志的根目录</para>
    /// </summary>
    public class Logger
    {
        static ThreadModel logConsoleThread = new ThreadModel("Console Log Thread");
        static ThreadModel logFileThread = new ThreadModel("File Log Thread");
        const string LOGINFOPATH = "log/info/";
        const string LOGERRORPATH = "log/error/";

        static string logInfoPath = LOGINFOPATH;
        static string logErrorPath = LOGERRORPATH;
        static Regex reg = new Regex("[{][0-9]+[}]");

        /// <summary>
        /// 日志等级
        /// </summary>
        public static ENUM_LOGLEVEL LOGLEVEL = ENUM_LOGLEVEL.DEBUG;

        /// <summary>
        /// 是否控制台显示消息
        /// </summary>
        public static bool LOGCONSOLE = true;

        public enum ENUM_LOGLEVEL
        {
            /// <summary>
            /// 完全不输出任何日志
            /// </summary>
            Null = 0,
            /// <summary>
            /// 输出 DEBUG 以上级别
            /// </summary>
            DEBUG = 1,
            /// <summary>
            /// 输出 INFO 以上级别
            /// </summary>
            INFO = 2,
            /// <summary>
            /// 输出 ERROR 以上级别
            /// </summary>
            ERROR = 3
        }

        /// <summary>
        /// 设置日志的输出根目录
        /// </summary>
        /// <param name="path"></param>
        static public void SetLogRootPath(string path)
        {
            try
            {
                if (!(path.EndsWith("\\") || path.EndsWith("/")))
                {
                    path = "\\";
                }
                logInfoPath = path + LOGINFOPATH;
                logErrorPath = path + LOGERRORPATH;

                if (!Directory.Exists(logInfoPath)) { Directory.CreateDirectory(logInfoPath); }
                if (!Directory.Exists(logErrorPath)) { Directory.CreateDirectory(logErrorPath); }
            }
            catch (Exception e)
            {
                throw new Exception("创建日志文件夹权限不足：logInfoPath：" + logInfoPath + "  logErrorPath：" + logErrorPath, e);
            }
        }

        static Logger()
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("LogRootPath"))
                {
                    string logPath = System.Configuration.ConfigurationManager.AppSettings["LogRootPath"].ToString();
                    if (!(logPath.EndsWith("\\") || logPath.EndsWith("/")))
                    {
                        logPath = "\\";
                    }
                    logInfoPath = logPath + LOGINFOPATH;
                    logErrorPath = logPath + LOGERRORPATH;
                }
            }
            catch (Exception)
            {
            }

            try
            {
                if (!Directory.Exists(logInfoPath)) { Directory.CreateDirectory(logInfoPath); }
                if (!Directory.Exists(logErrorPath)) { Directory.CreateDirectory(logErrorPath); }
            }
            catch (Exception)
            { }
        }

        #region 日子写入文件辅助任务 class LogTaskFile : TaskModel
        /// <summary>
        /// 日子写入文件辅助任务
        /// </summary>
        class LogTaskFile : TaskModel
        {
            string formatMsg, mathed;
            Object[] values = null;
            Exception exce;
            StackTrace trace;
            String nowString = null;
            String nowfilepath = null;
            public LogTaskFile(String nowString, String nowfilepath, StackTrace trace, string mathed, string formatMsg, Exception exce, params Object[] values)
                : base("File Log Task")
            {
                this.nowfilepath = nowfilepath;
                this.mathed = mathed;
                this.trace = trace;
                this.formatMsg = formatMsg;
                this.values = values;
                this.exce = exce;
                this.nowString = nowString;
            }

            public override void Run()
            {

                var frame = trace.GetFrame(0);


                sb.Append("[")
                    .Append(nowString)
                    .Append(mathed.PadRight(5))
                    .Append(": ")
                    .Append(frame.GetMethod().DeclaringType.FullName)
                    .Append(", ")
                    .Append(frame.GetMethod().Name)
                    .Append(", ")
                    .Append(frame.GetFileLineNumber())
                    .Append("] ").Append(reg.IsMatch(formatMsg) ? String.Format(formatMsg, values) : formatMsg);

                if (exce != null)
                {
                    sb.AppendLine("")
                    .AppendLine("----------------------Exception--------------------------")
                    .Append(exce.GetType().FullName).Append(": ").AppendLine(exce.Message)
                    .AppendLine(exce.StackTrace)
                    .Append("----------------------Exception--------------------------");
                }
                else
                {
                    sb.AppendLine("");
                }

                String logPath = "";
                logPath = String.Format("{0}error_{1}.log", logErrorPath, nowfilepath);
                if (!logPath.Equals(_logerrorPath))
                {
                    _logerrorPath = logPath;
                    if (stream_Error != null)
                    {
                        stream_Error.Dispose();
                        stream_Error = null;
                    }
                    stream_Error = new System.IO.StreamWriter(new System.IO.FileStream(_logerrorPath, System.IO.FileMode.Append));
                }
                if ("Error".Equals(mathed))
                {
                    stream_Error.Write(sb.ToString());
                    stream_Error.Flush();
                }

                logPath = String.Format("{0}info_{1}.log", logInfoPath, nowfilepath);
                if (!logPath.Equals(_logPath))
                {
                    _logPath = logPath;
                    if (stream_log != null)
                    {
                        stream_log.Dispose();
                        stream_log = null;
                    }
                    stream_log = new System.IO.StreamWriter(new System.IO.FileStream(_logPath, System.IO.FileMode.Append));
                }

                stream_log.Write(sb.ToString());
                stream_log.Flush();
                sb.Clear();
            }
        }
        #endregion

        static string _logPath = null;
        static string _logerrorPath = null;
        static System.Text.StringBuilder sb = new StringBuilder();
        static System.IO.StreamWriter stream_log = null;
        static System.IO.StreamWriter stream_Error = null;

        #region 日志写入控制台输出 class LogTaskConsole : TaskBase
        /// <summary>
        /// 日志写入控制台输出
        /// </summary>
        class LogTaskConsole : TaskModel
        {
            String formatMsg, mathed;
            Object[] values = null;
            Exception exce;
            StackTrace trace;
            String nowString = null;
            static readonly StringBuilder sb = new StringBuilder();

            public LogTaskConsole(String nowString, StackTrace trace, String mathed, String formatMsg, Exception exce, params Object[] values)
                : base("Console Log Task")
            {
                this.mathed = mathed;
                this.trace = trace;
                this.formatMsg = formatMsg;
                this.exce = exce;
                this.nowString = nowString;
                this.values = values;
            }

            public override void Run()
            {
                sb.Clear();
                var frame = trace.GetFrame(0);
                sb.Append("[")
                    .Append(nowString)
                    .Append(mathed.PadRight(5))
                    .Append("：")
                    .Append(frame.GetMethod().DeclaringType.FullName)
                    .Append(", ")
                    .Append(frame.GetMethod().Name)
                    .Append(", ")
                    .Append(frame.GetFileLineNumber())
                    .Append("] ").Append(reg.IsMatch(formatMsg) ? String.Format(formatMsg, values) : formatMsg);

                if (exce != null)
                {
                    sb.AppendLine("")
                    .AppendLine("----------------------Exception--------------------------")
                    .Append(exce.GetType().FullName).Append(": ").AppendLine(exce.Message)
                    .AppendLine(exce.StackTrace)
                    .Append("----------------------Exception--------------------------");
                }
                Console.WriteLine(sb.ToString());
            }
        }
        #endregion

        /// <summary>
        /// 输出到控制台
        /// </summary>
        /// <param name="formatMsg"></param>
        static public void Debug(String formatMsg, params Object[] values)
        {
            if (LOGLEVEL >= ENUM_LOGLEVEL.DEBUG)
                AddLog("Debug", formatMsg, null, values);
        }


        /// <summary>
        /// 输出到控制台
        /// </summary>
        /// <param name="formatMsg"></param>
        static public void Debug(String formatMsg, Exception exception, params Object[] values)
        {
            if (LOGLEVEL >= ENUM_LOGLEVEL.DEBUG)
                AddLog("Debug", formatMsg, exception, values);
        }


        /// <summary>
        /// 控制台和文本文件
        /// </summary>
        /// <param name="msg"></param>
        static public void Info(String formatMsg, params Object[] values)
        {
            if (LOGLEVEL >= ENUM_LOGLEVEL.INFO)
                AddLog("Info", formatMsg, null, values);
        }

        /// <summary>
        /// 控制台和文本文件
        /// </summary>
        /// <param name="msg"></param>
        static public void Info(String formatMsg, Exception exception, params Object[] values)
        {
            if (LOGLEVEL >= ENUM_LOGLEVEL.INFO)
                AddLog("Info", formatMsg, exception, values);
        }

        /// <summary>
        /// 控制台和文本文件
        /// </summary>
        static public void Error(String formatMsg, params Object[] values)
        {
            if (LOGLEVEL >= ENUM_LOGLEVEL.ERROR)
                AddLog("Error", formatMsg, null, values);
        }
        /// <summary>
        /// 控制台和文本文件
        /// </summary>
        static public void Error(String formatMsg, Exception exception, params Object[] values)
        {
            if (LOGLEVEL >= ENUM_LOGLEVEL.ERROR)
                AddLog("Error", formatMsg, exception, values);
        }

        static void AddLog(String mathed, String formatMsg, Exception exception, params Object[] values)
        {
            StackTrace trace = new StackTrace(2, true);

            string tmp1 = DateTime.Now.NowString();
            string tmp2 = DateTime.Now.ToString("yyyyMMdd");

            if (LOGCONSOLE)
            {
                LogTaskConsole logConsole = new LogTaskConsole(tmp1, trace, mathed, formatMsg, exception, values);
                logConsoleThread.AddTask(logConsole);
            }
            LogTaskFile logFile = new LogTaskFile(tmp1, tmp2, trace, mathed, formatMsg, exception);
            logFileThread.AddTask(logFile);
        }
    }
}
