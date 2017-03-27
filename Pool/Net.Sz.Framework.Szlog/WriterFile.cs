using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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
    /// 写入文件辅助类
    /// </summary>
    internal class WriterFile
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete = false;
        string ThreadName;
        string FileName;
        bool Error = false;

        public WriterFile(string fileName, string threadName, bool error)
        {
            /*创建别要性*/
            msgs = new System.Collections.Concurrent.ConcurrentQueue<string>();
            this.ThreadName = threadName;
            this.FileName = fileName;
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
            thread.Name = "<" + threadName + "线程>";
            thread.Start();
            this.Error = error;
        }

        public WriterFile(string fileName)
        {
            this.FileName = fileName;
        }

        internal System.Collections.Concurrent.ConcurrentQueue<String> msgs = null;

        /// <summary>
        /// 加入队列并且通知
        /// </summary>
        /// <param name="msg"></param>
        public void Add(string msg)
        {
            msgs.Enqueue(msg);
            are.Set();
        }

        /// <summary>
        /// 通知一个或多个正在等待的线程已发生事件
        /// </summary>
        protected ManualResetEvent are = new ManualResetEvent(false);

        /// <summary>
        /// 线程处理器
        /// </summary>
        protected virtual void Run()
        {
            while (!this.IsDelete)
            {
                while (msgs.Count > 0)
                {
                    CreateFile();
                    if (CommUtil.LOG_PRINT_FILE_BUFFER)
                    {
                        /*双缓冲，减少磁盘IO*/
                        for (int i = 0; i < 1000; i++)
                        {
                            String msg;
                            if (msgs.TryDequeue(out msg))
                            {
                                wStream.Write(msg);
                            }
                            else break;
                        }
                        /*输入流到文件*/
                        wStream.Flush();
                        fStream.Flush();
                    }
                    else
                    {
                        String msg;
                        if (msgs.TryDequeue(out msg))
                        {
                            /*输入流到文件*/
                            wStream.Write(msg);
                            wStream.Flush();
                            fStream.Flush();
                        }
                        else break;
                    }
                }
                /*设置无限制等待*/
                are.Reset();
                are.WaitOne();
            }
        }

        StreamWriter wStream = null;
        FileStream fStream = null;

        /// <summary>
        /// 创建文件以及备份文件操作
        /// </summary>
        public void CreateFile()
        {
            String logPath = FileName;

            if (this.Error)
            {
                logPath += "_error.log";
            }

            if (File.Exists(logPath))
            {
                /*检查文件备份，每日一个备份*/
                DateTime dtime = File.GetLastWriteTime(logPath);

                string day1 = dtime.ToString("yyyy-MM-dd");
                string day2 = DateTime.Now.ToString("yyyy-MM-dd");
                /*获取文件的上一次写入时间是否不是今天日期*/
                if (!day1.Equals(day2))
                {
                    Close();
                    wStream = null;
                    fStream = null;
                    /*备份*/
                    File.Move(logPath, logPath + "_" + day1 + ".log");
                }
            }

            if (fStream == null)
            {
                /*追加文本*/
                fStream = new FileStream(logPath, System.IO.FileMode.Append);
                /*重建流*/
                wStream = new System.IO.StreamWriter(fStream);
            }

        }

        /// <summary>
        /// 文件io操作
        /// </summary>
        /// <param name="msg"></param>
        public void Writer(string msg)
        {
            wStream.Write(msg);
            wStream.Flush();
            fStream.Flush();
        }

        /// <summary>
        /// 关闭流
        /// </summary>
        public void Close()
        {
            try
            {
                /*关闭流*/
                wStream.Flush();
                wStream.Dispose();
                wStream.Close();

                fStream.Flush();
                fStream.Dispose();
                fStream.Close();
            }
            catch { }
        }

    }
}
