using Net.Sz.Framework.Log;
using Net.Sz.Framework.Utils;
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
namespace Net.Sz.Framework.Threading
{
    /// <summary>
    /// 线程模型
    /// </summary>    
    public class ThreadModel
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
                thread.IsBackground = true;
                //仅次于最高级
                thread.Priority = ThreadPriority.AboveNormal;
                thread.Name = "< " + name + "线程 >";                
                thread.Start();
                Logger.Info("初始化 " + thread.Name);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
                    thread.IsBackground = true;
                    //仅次于最高级
                    thread.Priority = ThreadPriority.AboveNormal;
                    thread.Name = "< " + name + "线程" + "_" + (i + 1) + " >";
                    thread.Start();
                    Logger.Info("初始化 " + thread.Name);
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
        protected List<TimerTask> timerTaskQueue = new List<TimerTask>();

        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="t"></param>
        public virtual void AddTask(TaskModel t)
        {
            t.SetSubmitTime();
            taskQueue.Enqueue(t);
            //防止线程正在阻塞时添加进入了新任务
            are.Set();
        }

        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="t"></param>
        public void AddTimerTask(TimerTask t)
        {
            t.RunAttribute["lastactiontime"] = Utils.TimeUtil.CurrentTimeMillis();
            if (t.IsStartAction)
            {
                AddTask(t);
            }
            lock (timerTaskQueue)
            {               
                timerTaskQueue.Add(t);
            }
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

                    if (!taskQueue.TryDequeue(out task))
                    {
                        break;
                    }

                    /* 执行任务 */
                    //r.setSubmitTimeL();
                    long submitTime = TimeUtil.CurrentTimeMillis();
                    try
                    {
                        task.Run();
                    }
                    catch (Exception e)
                    {
                        Logger.Error(System.Threading.Thread.CurrentThread.Name + " 执行任务：" + task.ToString() + " 遇到错误", e);
                        continue;
                    }
                    long timeL1 = TimeUtil.CurrentTimeMillis() - submitTime;
                    //long timeL2 = SzExtensions.CurrentTimeMillis() - task.GetSubmitTime();
                    long timeL2 = 0;
                    if (timeL1 < 10) { }
                    else if (timeL1 < 100) { Logger.Debug(System.Threading.Thread.CurrentThread.Name + " 完成了任务：" + task.ToString() + " 执行耗时：" + timeL1 + " 提交耗时：" + timeL2); }
                    else if (timeL1 <= 200L) { Logger.Debug(System.Threading.Thread.CurrentThread.Name + " 完成了任务：" + task.ToString() + " 执行耗时：" + timeL1 + " 提交耗时：" + timeL2); }
                    else if (timeL1 <= 1000L) { Logger.Debug(System.Threading.Thread.CurrentThread.Name + " 长时间执行 完成任务：" + task.ToString() + " “考虑”任务脚本逻辑 耗时：" + timeL1 + " 提交耗时：" + timeL2); }
                    else if (timeL1 <= 4000L) { Logger.Error(System.Threading.Thread.CurrentThread.Name + " 超长时间执行完成 任务：" + task.ToString() + " “检查”任务脚本逻辑 耗时：" + timeL1 + " 提交耗时：" + timeL2); }
                    else
                    {
                        Logger.Error(System.Threading.Thread.CurrentThread.Name + " 超长时间执行完成 任务：" + task.ToString() + " “考虑是否应该删除”任务脚本 耗时：" + timeL1 + " 提交耗时：" + timeL2);
                    }
                    task = null;
                }
                are.Reset();
                //队列为空等待200毫秒继续
                are.WaitOne(200);
            }
            Console.WriteLine(DateTime.Now.NowString() + " " + System.Threading.Thread.CurrentThread.Name + " Destroying");
        }

        /// <summary>
        /// 定时器线程处理器
        /// </summary>
        internal void TimerRun()
        {
            ///无限循环执行函数器

            if (!this.IsDelete && timerTaskQueue.Count > 0)
            {
                IEnumerable<TimerTask> collections = null;
                lock (timerTaskQueue)
                {
                    collections = new List<TimerTask>(timerTaskQueue);
                }
                foreach (TimerTask timerEvent in collections)
                {
                    int execCount = timerEvent.RunAttribute.GetintValue("Execcount");
                    long lastTime = timerEvent.RunAttribute.GetlongValue("LastExecTime");
                    long nowTime = Utils.TimeUtil.CurrentTimeMillis();
                    if (nowTime > timerEvent.StartTime //是否满足开始时间
                            && (nowTime - timerEvent.GetSubmitTime() > timerEvent.IntervalTime)//提交以后是否满足了间隔时间
                            && (timerEvent.EndTime <= 0 || nowTime < timerEvent.EndTime) //判断结束时间
                            && (nowTime - lastTime >= timerEvent.IntervalTime))//判断上次执行到目前是否满足间隔时间
                    {
                        //提交执行
                        this.AddTask(timerEvent);
                        //记录
                        execCount++;
                        timerEvent.RunAttribute["Execcount"] = execCount;
                        timerEvent.RunAttribute["LastExecTime"] = nowTime;
                    }
                    nowTime = Utils.TimeUtil.CurrentTimeMillis();
                    //判断删除条件
                    if ((timerEvent.EndTime > 0 && nowTime < timerEvent.EndTime)
                            || (timerEvent.ActionCount > 0 && timerEvent.ActionCount <= execCount))
                    {
                        timerTaskQueue.Remove(timerEvent);
                    }
                }
            }
        }
    }
}
