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
namespace Net.Sz.Framework.Szlog
{

    /// <summary>
    /// 日志辅助
    /// <para>默认是不打印栈桢的，因为比较耗时：如果需要请设置 LOGSTACKTRACE = true 或者 ↓↓↓</para>
    /// <para>AppSettings 设置 log_print_stackrace         日志是否输出调用栈桢 true or false</para>
    /// <para>AppSettings 设置 log_print_console           日志是否输出到控制台 true or false</para>
    /// <para>AppSettings 设置 log_print_level             日志的等级,忽律大小写 DEBUG INFO WARN ERROR</para>
    /// <para>AppSettings 设置 log_print_path              日志的文件名带目录，log/sz.log</para>
    /// <para>AppSettings 设置 log_print_file              日志是否输出到文件 true or false</para>
    /// <para>AppSettings 设置 log_print_file_buffer       日志双缓冲输出到文件 true or false</para>
    /// </summary>
    public class SzLogger
    {

        private static SzLogger me = null;

        public static SzLogger getLogger()
        {
            if (me == null)
                lock (typeof(SzLogger))
                    if (me == null)
                        /*双重判定单例模式，防止并发*/
                        me = new SzLogger(CommUtil.LOGPATH);
            return me;
        }

        private string _LogFileName;

        private SzLogger(string logfile)
        {
            _LogFileName = logfile;
            CommUtil.InitConfig();
        }

        public bool IsDebugEnabled()
        {
            return CommUtil.LOG_PRINT_LEVEL <= LogLevel.DEBUG;
        }

        public bool IsInfoEnabled()
        {
            return CommUtil.LOG_PRINT_LEVEL <= LogLevel.INFO;
        }

        public bool IsWarnEnabled()
        {
            return CommUtil.LOG_PRINT_LEVEL <= LogLevel.WARN;
        }

        public bool IsErrorEnabled()
        {
            return CommUtil.LOG_PRINT_LEVEL <= LogLevel.ERROR;
        }




        /// <summary>
        /// 输出到控制台
        /// </summary>
        /// <param name="msg"></param>
        public void Debug(Object msg, Exception exception = null)
        {
            if (IsDebugEnabled())
                AddLog("DEBUG", msg, exception);
        }

        /// <summary>
        /// 控制台和文本文件
        /// </summary>
        /// <param name="msg"></param>
        public void Info(Object msg, Exception exception = null)
        {
            if (IsInfoEnabled())
                AddLog("INFO ", msg, exception);
        }

        /// <summary>
        /// 控制台和文本文件
        /// </summary>
        /// <param name="msg"></param>
        public void Warn(Object msg, Exception exception = null)
        {
            if (IsWarnEnabled())
                AddLog("WARN ", msg, exception);
        }

        /// <summary>
        /// 控制台和文本文件
        /// </summary>
        public void Error(Object msg, Exception exception = null)
        {
            if (IsErrorEnabled())
                AddLog("ERROR", msg, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="msg"></param>
        /// <param name="exception"></param>
        public void WriterLog(String logPath, Object msg, Exception exception = null)
        {
            CommUtil.ResetLogDirectory(logPath);
            WriterFile wfile = new WriterFile(logPath);
            String logmsg = GetLogString("FILE ", msg, exception, 2);
            wfile.Writer(logmsg);
            wfile.Close();
        }

        /// <summary>
        /// 增加日志
        /// </summary>
        /// <param name="level"></param>
        /// <param name="msg"></param>
        /// <param name="exception"></param>
        void AddLog(string level, Object msg, Exception exception)
        {
            string logmsg = GetLogString(level, msg, exception, 3);
            if (exception != null)
            {
                if (CommUtil.LOG_PRINT_FILE)
                {
                    /*处理如果有异常，需要把异常信息打印到单独的文本文件*/
                    if (wfileerror == null)
                        lock (typeof(WriterFile))
                            if (wfileerror == null)
                                /*双重判定单例模式，防止并发*/
                                wfileerror = new WriterFile(CommUtil.LOGPATH, "log-error-file", true);
                    wfileerror.Add(logmsg);
                }
            }
            if (CommUtil.LOG_PRINT_FILE)
            {
                /*处理到日志文件*/
                if (wfile == null)
                    lock (typeof(WriterFile))
                        if (wfile == null)
                            /*双重判定单例模式，防止并发*/
                            wfile = new WriterFile(CommUtil.LOGPATH, "log-file", false);
                wfile.Add(logmsg);
            }
            if (CommUtil.LOG_PRINT_CONSOLE)
            {
                /*处理到控制台*/
                if (wconsole == null)
                    lock (typeof(WriterFile))
                        if (wconsole == null)
                            /*双重判定单例模式，防止并发*/
                            wconsole = new WriterConsole("log-console");
                wconsole.Add(logmsg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="msg"></param>
        /// <param name="exception"></param>
        /// <param name="f">栈桢深度</param>
        /// <returns></returns>
        string GetLogString(string level, Object msg, Exception exception, int f)
        {
            string tmp1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff: ");
            StringBuilder sb = new StringBuilder();

            sb.Append("[")
                .Append(tmp1)
                .Append(level);


            if (CommUtil.LOG_PRINT_STACKTRACE)
            {
                /*获取堆栈信息非常耗性能 默认是不开放的*/
                StackFrame frame = new StackTrace(f, true).GetFrame(0);
                sb.Append(":");
                sb.Append(frame.GetMethod().DeclaringType.FullName);
                sb.Append(".");
                sb.Append(frame.GetMethod().Name);
                sb.Append(".");
                sb.Append(frame.GetFileLineNumber());
            }

            sb.Append("] ");
            sb.AppendLine(msg.ToString());

            if (exception != null)
            {
                sb
                .Append(exception.GetType().FullName)
                .Append(": ")
                .AppendLine(exception.Message)
                .AppendLine(exception.StackTrace)
                .AppendLine("----------------------Exception--------------------------");
            }
            return sb.ToString();
        }

        WriterFile wfile = null;
        WriterFile wfileerror = null;
        WriterConsole wconsole = null;

        public int Count { get { return wfile == null ? 0 : wfile.msgs == null ? 0 : wfile.msgs.Count; } }

    }
}
