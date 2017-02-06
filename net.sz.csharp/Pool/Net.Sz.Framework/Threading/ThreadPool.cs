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
namespace Net.Sz.Framework.Threading
{
    /// <summary>
    /// 线程池
    /// </summary>
    public class ThreadPool
    {

        static ThreadPool()
        {
            GlobThread = GetThreadModel("全局线程");
            TimerThread timerthread = new TimerThread();
        }


        static private ThreadModel backThreadTools = null;

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
                backThreadTools = new ThreadModel("Back Thread", backthreadSize);
            }
        }

        static internal System.Collections.Concurrent.ConcurrentDictionary<long, ThreadModel> threads = new System.Collections.Concurrent.ConcurrentDictionary<long, ThreadModel>();

        /// <summary>
        /// 添加一个线程模型到线程模型管理器里面
        /// </summary>
        /// <param name="name">线程名称</param>
        /// <param name="threadCount">线程模型内线程数量</param>
        /// <returns></returns>
        static public long GetThreadModel(string name, int threadCount = 1)
        {
            ThreadModel model = new ThreadModel(name, threadCount);
            return GetThreadModel(model);
        }

        static public bool RemoveThreadModel(long tid)
        {
            ThreadModel threadmodel = null;
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
        static public long GetThreadModel(ThreadModel model)
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
        static public ThreadModel GetThreadModel(long tid)
        {
            ThreadModel tm = null;
            if (tid == 0)
            {
                Log.Logger.Info("艹艹艹艹线程id==0艹艹艹");
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
            ThreadModel tm = GetThreadModel(tid);
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
        static public void AddTimerTask(long tid, TimerTask taskbase)
        {
            ThreadModel tm = GetThreadModel(tid);
            if (tm != null)
            {
                tm.AddTimerTask(taskbase);
            }
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
        static public void AddGlobTimerTask(TimerTask taskbase)
        {
            Init();
            backThreadTools.AddTimerTask(taskbase);
        }

    }
}
