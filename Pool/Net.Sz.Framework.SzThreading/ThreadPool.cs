using Net.Sz.Framework.Szlog;
using System;
using System.Collections.Generic;
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
namespace Net.Sz.Framework.SzThreading
{
    /// <summary>
    /// 线程池
    /// <para>程序启动完成请将 StartEnd=true，否则定时器任务是不会执行的</para>
    /// </summary>
    public class ThreadPool
    {

        private static SzLogger log = SzLogger.getLogger();

        static ThreadPool()
        {
            GlobThread = GetThreadModel("全局线程");
            TimerThread timerthread = new TimerThread();
        }


        static private SzThread backThreadTools = null;

        static public bool StartEnd = false;
        /// <summary>
        /// 服务器是否停止
        /// </summary>
        static public bool IsStopServer = false;

        /// <summary>
        /// 全局线程
        /// </summary>
        static public long GlobThread { get; set; }

        /// <summary>
        /// 全局定时器线程
        /// </summary>
        static public long GlobTimerThread { get; set; }

        /// <summary>
        /// 初始化后台帮助线程
        /// </summary>
        /// <param name="backthreadSize">后台帮助线程的数量</param>
        static public void Init(int backthreadSize = 2)
        {
            if (backThreadTools == null)
            {
                backThreadTools = new SzThread("Back Thread", SzThread.ThreadType.Sys, backthreadSize);
                threads.TryAdd(backThreadTools.ID, backThreadTools);
            }
        }

        static internal System.Collections.Concurrent.ConcurrentDictionary<long, SzThread> threads = new System.Collections.Concurrent.ConcurrentDictionary<long, SzThread>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="threadType">线程类型</param>
        /// <param name="threadCount">线程数量</param>
        /// <returns></returns>
        static public long GetThreadModel(string name, SzThread.ThreadType threadType = SzThread.ThreadType.User, int threadCount = 1)
        {
            SzThread model = new SzThread(name, threadType, threadCount);
            return GetThreadModel(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        static public bool RemoveThreadModel(long tid)
        {
            SzThread threadmodel = null;
            if (threads.TryRemove(tid, out threadmodel))
            {
                threadmodel.IsDelete = true;
                threadmodel = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加一个线程模型到线程模型管理器里面
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        static public long GetThreadModel(SzThread model)
        {
            if (threads.TryAdd(model.ID, model))
            {
                return model.ID;
            }
            return 0;
        }

        /// <summary>
        /// 获取一个线程模型
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        static public SzThread GetThreadModel(long tid)
        {
            SzThread tm = null;
            if (tid == 0)
            {
                if (log.IsInfoEnabled())
                    log.Info("艹艹艹艹线程id==0艹艹艹");
            }
            threads.TryGetValue(tid, out tm);
            return tm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="taskbase"></param>
        static public void AddTask(long tid, TaskModel taskbase)
        {
            SzThread tm = GetThreadModel(tid);
            if (tm != null)
            {
                tm.AddTask(taskbase);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="taskbase"></param>
        static public void AddTimerTask(long tid, TimerTaskModel taskbase)
        {
            SzThread tm = GetThreadModel(tid);
            if (tm != null)
            {
                tm.AddTimerTask(taskbase);
            }
        }

        /// <summary>
        /// 取消一个任务
        /// </summary>
        /// <param name="timer"></param>
        static public TimerTaskModel RemoveTimerTask(long tid, TimerTaskModel timer)
        {
            return RemoveTimerTask(tid, timer.ID);
        }

        /// <summary>
        /// 取消一个任务,键不存在，返回null
        /// </summary>
        /// <param name="timerId"></param>
        static public TimerTaskModel RemoveTimerTask(long tid, long timerId)
        {
            SzThread thread = GetThreadModel(tid);
            if (thread != null)
            {
                return thread.RemoveTimerTask(timerId);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskbase"></param>
        static public void AddGlobTask(TaskModel taskbase)
        {
            Init();
            backThreadTools.AddTask(taskbase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskbase"></param>
        static public void AddGlobTimerTask(TimerTaskModel taskbase)
        {
            Init();
            backThreadTools.AddTimerTask(taskbase);
        }

        

    }
}
