using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// 写入控制台
    /// </summary>
    public class WriterConsole
    {

        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete = false;

        string Name;

        public WriterConsole(string threadName)
        {
            this.Name = threadName;
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
            thread.Name = "<" + threadName + "线程>";
            thread.Start();
        }

        internal System.Collections.Concurrent.ConcurrentQueue<String> msgs = new System.Collections.Concurrent.ConcurrentQueue<string>();

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
                if (msgs.IsEmpty)
                {
                    /*设置无限制等待*/
                    are.Reset();
                    are.WaitOne();
                }
                while (!msgs.IsEmpty)
                {
                    String msg;
                    if (msgs.TryDequeue(out msg))
                        Console.Write(msg);                    
                }
            }
        }
    }
}
