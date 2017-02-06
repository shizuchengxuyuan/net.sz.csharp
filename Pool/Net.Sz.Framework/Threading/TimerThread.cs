using Net.Sz.Framework.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net.Sz.Framework.Threading
{
    internal class TimerThread
    {
        public TimerThread()
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Highest;
            thread.Name = "< 定时器线程处理线程 >";
            thread.Start();
            Logger.Info("初始化 " + thread.Name);
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
                timerAre.WaitOne(5);
                List<ThreadModel> threads = new List<ThreadModel>(ThreadPool.threads.Values);
                foreach (var item in threads)
                {
                    item.TimerRun();
                }
            }
        }
    }
}
