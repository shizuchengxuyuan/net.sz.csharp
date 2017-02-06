using Net.Sz.Framework.Log;
using System;
using System.Collections.Generic;
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
namespace Net.Sz.Framework.Log
{
    /// <summary>
    /// 线程模型
    /// </summary>    
    internal class ThreadModel
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete = false;
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// 已分配的自定义线程静态ID
        /// </summary>
        public static int StaticID { get; private set; }

        string Name;

        /// <summary>
        /// 初始化线程模型，
        /// </summary>
        /// <param name="name"></param>
        public ThreadModel(String name)
            : this(name, 1)
        {

        }

        /// <summary>
        /// 初始化线程模型
        /// </summary>
        /// <param name="name">线程名称</param>
        /// <param name="count">线程数量</param>
        public ThreadModel(String name, Int32 count)
        {
            lock (typeof(ThreadModel))
            {
                StaticID++;
                ID = StaticID;
            }
            this.Name = name;
            if (count == 1)
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
                thread.Name = "< " + name + "线程 >";
                thread.Start();
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
                    thread.Name = "< " + name + "线程" + "_" + (i + 1) + " >";
                    thread.Start();
                }
            }
        }


        /// <summary>
        /// 任务队列
        /// </summary>
        protected List<TaskModel> taskQueue = new List<TaskModel>();

        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="t"></param>
        public virtual void AddTask(TaskModel t)
        {
            lock (taskQueue)
            {
                taskQueue.Add(t);
            }
            //防止线程正在阻塞时添加进入了新任务
            are.Set();
        }


        /// <summary>
        /// 通知一个或多个正在等待的线程已发生事件
        /// </summary>
        protected ManualResetEvent are = new ManualResetEvent(false);

        /// <summary>
        /// 通知一个或多个正在等待的线程已发生事件
        /// </summary>
        protected ManualResetEvent timerAre = new ManualResetEvent(true);

        /// <summary>
        /// 线程处理器
        /// </summary>
        protected virtual void Run()
        {
            while (!this.IsDelete)
            {
                while ((taskQueue.Count > 0))
                {
                    TaskModel task = null;
                    lock (taskQueue)
                    {
                        if (taskQueue.Count > 0)
                        {
                            task = taskQueue[0];
                            taskQueue.RemoveAt(0);
                        }
                        else { break; }
                    }

                    /* 执行任务 */
                    //r.setSubmitTimeL();
                    long submitTime = Utils.TimeUtil.CurrentTimeMillis();
                    try { task.Run(); }
                    catch (Exception) { continue; }
                    task = null;
                }
                are.Reset();
                //队列为空等待200毫秒继续
                are.WaitOne(200);
            }
        }
    }
}
