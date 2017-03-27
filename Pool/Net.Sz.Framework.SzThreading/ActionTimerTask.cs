using Net.Sz.Framework.Szlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Sz.Framework.SzThreading
{
    public class ActionTimerTask : TimerTaskModel
    {
        Action<TimerTaskModel> ARun;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime">指定开始时间 0 没有开始时间</param>
        /// <param name="isStartAction">提交立即执行一次</param>
        /// <param name="endTime">指定结束时间 0 没有结束时间 </param>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        /// <param name="ID">任务ID</param>
        /// <param name="Name">任务名称</param>
        public ActionTimerTask(Int64 startTime, bool isStartAction, Int64 endTime, int actionCount, int intervalTime, String Name, Action<TimerTaskModel> run)
            : base(startTime, isStartAction, endTime, actionCount, intervalTime, Name)
        {
            this.ARun = run;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime">指定开始时间 0 没有开始时间</param>
        /// <param name="isStartAction">提交立即执行一次</param>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        /// <param name="ID">任务ID</param>
        /// <param name="Name">任务名称</param>
        public ActionTimerTask(Int64 startTime, bool isStartAction, int actionCount, int intervalTime, String Name, Action<TimerTaskModel> run)
            : this(startTime, isStartAction, 0, actionCount, intervalTime, Name, run)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endTime">指定结束时间 0 没有结束时间 </param>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        /// <param name="ID">任务ID</param>
        /// <param name="Name">任务名称</param>
        public ActionTimerTask(bool isStartAction, Int64 endTime, int actionCount, int intervalTime, String Name, Action<TimerTaskModel> run)
            : this(0, isStartAction, endTime, actionCount, intervalTime, Name, run)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime">指定开始时间 0 没有开始时间</param>
        /// <param name="endTime">指定结束时间 0 没有结束时间 </param>
        /// <param name="intervalTime">指定间隔时间</param>
        /// <param name="ID">任务ID</param>
        /// <param name="Name">任务名称</param>
        public ActionTimerTask(Int64 startTime, Int64 endTime, int intervalTime, String Name, Action<TimerTaskModel> run)
            : this(startTime, false, endTime, -1, intervalTime, Name, run)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        /// <param name="ID">任务ID</param>
        /// <param name="Name">任务名称</param>
        public ActionTimerTask(int actionCount, int intervalTime, String Name, Action<TimerTaskModel> run)
            : this(0, false, 0, actionCount, intervalTime, Name, run)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        public ActionTimerTask(int actionCount, int intervalTime, Action<TimerTaskModel> run)
            : this(0, false, 0, actionCount, intervalTime, "无名", run)
        { }


        /// <summary>
        /// 指定间隔时间无限执行
        /// </summary>
        /// <param name="intervalTime">指定间隔时间</param>
        public ActionTimerTask(int intervalTime, Action<TimerTaskModel> run)
            : this(0, false, 0, -1, intervalTime, "无名", run)
        { }


        public override void Run()
        {
            ARun(this);
        }
    }
}
