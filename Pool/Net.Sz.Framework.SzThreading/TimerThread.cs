using Net.Sz.Framework.Szlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net.Sz.Framework.SzThreading
{
    internal class TimerThread
    {
        private static SzLogger log = SzLogger.getLogger();

        public TimerThread()
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Highest;
            thread.Name = "< 定时器线程处理线程 >";
            thread.Start();
            if (log.IsInfoEnabled()) log.Info("初始化 " + thread.Name);
        }

        /// <summary>
        /// 通知一个或多个正在等待的线程已发生事件
        /// </summary>
        protected ManualResetEvent timerAre = new ManualResetEvent(true);

        protected void Run()
        {
            ///无限循环执行函数器
            while (true)
            {
                timerAre.Reset();
                timerAre.WaitOne(2);
                List<SzThread> threads = new List<SzThread>(ThreadPool.threads.Values);
                foreach (var item in threads)
                    if (item.threadType == SzThread.ThreadType.Sys || ThreadPool.StartEnd)
                        item.TimerRun();
            }
        }
    }
}
