using Net.Sz.Framework.Szlog;
using Net.Sz.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
namespace Net.Sz.Framework.SzThreading
{
    /// <summary>
    /// 线程模型
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary> 
    public class SzThread
    {

        private static SzLogger log = SzLogger.getLogger();

        private static LongId0Util ids = new LongId0Util();


        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete = false;
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; protected set; }

        public string Name;
        public ThreadType threadType;

        public enum ThreadType
        {
            /// <summary>
            /// 系统线程
            /// </summary>
            Sys,
            /// <summary>
            /// 用户线程
            /// </summary>
            User
        }

        /// <summary>
        /// 初始化线程模型，
        /// </summary>
        /// <param name="name"></param>
        public SzThread(String name, ThreadType threadtype = ThreadType.User) : this(name, threadtype, 1) { }

        /// <summary>
        /// 初始化线程模型
        /// </summary>
        /// <param name="name">线程名称</param>
        /// <param name="count">线程数量</param>
        public SzThread(String name, ThreadType threadtype = ThreadType.User, Int32 count = 1)
        {
            this.threadType = threadtype;
            this.Name = name;
            this.ID = ids.GetId();
            if (count == 1)
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
                thread.IsBackground = true;
                //仅次于最高级
                thread.Priority = ThreadPriority.AboveNormal;
                thread.Name = "<" + name + "线程>";
                thread.Start();
                if (log.IsInfoEnabled())
                    log.Info("初始化 " + thread.Name);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
                    thread.IsBackground = true;
                    //仅次于最高级
                    thread.Priority = ThreadPriority.AboveNormal;
                    thread.Name = "<" + name + "线程" + "_" + (i + 1) + ">";
                    thread.Start();
                    if (log.IsInfoEnabled())
                        log.Info("初始化 " + thread.Name);
                }
            }
        }


        /// <summary>
        /// 任务队列
        /// </summary>
        protected System.Collections.Concurrent.ConcurrentQueue<TaskModel> taskQueue = new System.Collections.Concurrent.ConcurrentQueue<TaskModel>();
        /// <summary>
        /// 任务队列
        /// </summary>
        protected System.Collections.Concurrent.ConcurrentDictionary<long, TimerTaskModel> timerTaskQueue = new System.Collections.Concurrent.ConcurrentDictionary<long, TimerTaskModel>();

        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="t"></param>
        public virtual void AddTask(TaskModel t)
        {
            t.CreateTime = TimeUtil.CurrentTimeMillis();
            taskQueue.Enqueue(t);
            //防止线程正在阻塞时添加进入了新任务
            are.Set();
        }

        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="timer"></param>
        public void AddTimerTask(TimerTaskModel timer)
        {
            timer.RunAttribute["lastactiontime"] = Utils.TimeUtil.CurrentTimeMillis();
            if (timer.IsStartAction)
            {
                /*立马执行一次*/
                AddTask(timer);
            }
            /*添加队列*/
            timerTaskQueue.TryAdd(timer.ID, timer);
        }

        /// <summary>
        /// 取消一个任务
        /// </summary>
        /// <param name="t"></param>
        public TimerTaskModel RemoveTimerTask(TimerTaskModel t)
        {
            return RemoveTimerTask(t.ID);
        }

        /// <summary>
        /// 取消一个任务,键不存在，返回null
        /// </summary>
        /// <param name="timerId"></param>
        public TimerTaskModel RemoveTimerTask(long timerId)
        {
            TimerTaskModel timer = null;
            if (timerTaskQueue.TryGetValue(timerId, out timer))
            {
                timer.Cancel = true;
                return timer;
            }
            return null;
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
                while ((taskQueue.Count > 0))
                {
                    TaskModel task = null;
                    /*获取并移除当前任务*/
                    if (!taskQueue.TryDequeue(out task))
                    {
                        break;
                    }

                    /* 执行任务 */
                    long submitTime = TimeUtil.CurrentTimeMillis();
                    try
                    {
                        if (!task.Cancel)
                        {
                            task.Run();
                        }
                    }
                    catch (Exception e)
                    {
                        if (log.IsErrorEnabled())
                            log.Error(System.Threading.Thread.CurrentThread.Name + " 执行任务：" + task.GetType().FullName + " 遇到错误", e);
                    }
                    if (task is TimerTaskModel)
                    {
                        ((TimerTaskModel)task).LastRun = true;
                    }

                    long timeL1 = TimeUtil.CurrentTimeMillis() - submitTime;

                    if (!task.GetType().FullName.StartsWith("Net.Sz.Framework"))
                    {
                        if (timeL1 > 10) { if (log.IsDebugEnabled()) log.Debug(System.Threading.Thread.CurrentThread.Name + " 完成了任务：" + task.GetType().FullName + " 执行耗时：" + timeL1); }
                    }
                    task = null;
                }
                are.Reset();
                /*队列为空等待200毫秒继续*/
                are.WaitOne(200);
            }
            if (log.IsErrorEnabled()) log.Error(DateTime.Now.NowString() + " " + System.Threading.Thread.CurrentThread.Name + " Destroying");
        }

        /// <summary>
        /// 定时器线程处理器
        /// </summary>
        internal void TimerRun()
        {
            ///无限循环执行函数器

            if (!this.IsDelete && timerTaskQueue.Count > 0)
            {
                IEnumerable<TimerTaskModel> collections = new List<TimerTaskModel>(timerTaskQueue.Values);

                foreach (TimerTaskModel timerEvent in collections)
                {
                    if (!timerEvent.LastRun) continue;/*如果上一次没有执行完成不在执行下一次*/
                    int execCount = timerEvent.RunAttribute.GetintValue("Execcount");
                    long lastTime = timerEvent.RunAttribute.GetlongValue("LastExecTime");
                    long nowTime = Utils.TimeUtil.CurrentTimeMillis();

                    if (!timerEvent.Cancel && nowTime > timerEvent.StartTime /*是否满足开始时间*/
                            && (nowTime - timerEvent.CreateTime > timerEvent.IntervalTime)/*提交以后是否满足了间隔时间*/
                            && (timerEvent.EndTime <= 0 || nowTime < timerEvent.EndTime) /*判断结束时间*/
                            && (nowTime - lastTime >= timerEvent.IntervalTime))/*判断上次执行到目前是否满足间隔时间*/
                    {
                        timerEvent.LastRun = false;
                        /*提交执行*/
                        this.AddTask(timerEvent);
                        /*记录*/
                        execCount++;
                        timerEvent.RunAttribute["Execcount"] = execCount;
                        timerEvent.RunAttribute["LastExecTime"] = nowTime;
                    }
                    nowTime = Utils.TimeUtil.CurrentTimeMillis();
                    /*判断删除条件*/
                    if (timerEvent.Cancel || (timerEvent.EndTime > 0 && nowTime < timerEvent.EndTime)
                            || (timerEvent.ActionCount > 0 && timerEvent.ActionCount <= execCount))
                    {
                        TimerTaskModel timer;
                        timerTaskQueue.TryRemove(timerEvent.ID, out timer);
                    }
                }
            }
        }
    }
}
