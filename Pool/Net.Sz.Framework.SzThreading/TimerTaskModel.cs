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
    /// 
    /// </summary>
    public abstract class TimerTaskModel : TaskModel
    {

        /// <summary>
        /// 开始执行的时间
        /// </summary>
        public long StartTime { get; set; }

        /// <summary>
        /// 提交立即执行一次
        /// </summary>
        public bool IsStartAction { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long EndTime { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int ActionCount { get; set; }

        /// <summary>
        /// 间隔执行时间
        /// </summary>
        public int IntervalTime { get; set; }
        /// <summary>
        /// 最后一次是否执行完成
        /// </summary>
        internal bool LastRun { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tID">指定执行线程 0 默认后台执行</param>
        /// <param name="startTime">指定开始时间 0 没有开始时间</param>
        /// <param name="isStartAction">提交立即执行一次</param>
        /// <param name="endTime">指定结束时间 0 没有结束时间 </param>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        /// <param name="Name">任务名称</param>
        public TimerTaskModel(long startTime, bool isStartAction, long endTime, int actionCount, int intervalTime, string Name)
            : base(Name)
        {
            this.IsStartAction = isStartAction;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.ActionCount = actionCount;
            this.IntervalTime = intervalTime;
            this.LastRun = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tID">指定执行线程 0 默认后台执行</param>
        /// <param name="startTime">指定开始时间 0 没有开始时间</param>
        /// <param name="isStartAction">提交立即执行一次</param>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        /// <param name="ID">任务ID</param>
        /// <param name="Name">任务名称</param>
        public TimerTaskModel(long startTime, bool isStartAction, int actionCount, int intervalTime, string Name)
            : this(startTime, isStartAction, 0, actionCount, intervalTime, Name)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tID">指定执行线程 0 默认后台执行</param>
        /// <param name="startTime">指定开始时间 0 没有开始时间</param>
        /// <param name="isStartAction">提交立即执行一次</param>
        /// <param name="endTime">指定结束时间 0 没有结束时间 </param>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        /// <param name="ID">任务ID</param>
        /// <param name="Name">任务名称</param>
        public TimerTaskModel(bool isStartAction, long endTime, int actionCount, int intervalTime, string Name)
            : this(0, isStartAction, endTime, actionCount, intervalTime, Name)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tID">指定执行线程 0 默认后台执行</param>
        /// <param name="startTime">指定开始时间 0 没有开始时间</param>
        /// <param name="isStartAction">提交立即执行一次</param>
        /// <param name="endTime">指定结束时间 0 没有结束时间 </param>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        /// <param name="ID">任务ID</param>
        /// <param name="Name">任务名称</param>
        public TimerTaskModel(long startTime, long endTime, int intervalTime, string Name)
            : this(startTime, false, endTime, -1, intervalTime, Name)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tID">指定执行线程 0 默认后台执行</param>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        /// <param name="ID">任务ID</param>
        /// <param name="Name">任务名称</param>
        public TimerTaskModel(int actionCount, int intervalTime, string Name)
            : this(0, false, 0, actionCount, intervalTime, Name)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tID">指定执行线程 0 默认后台执行</param>
        /// <param name="actionCount">指定执行次数 0 表示无限次</param>
        /// <param name="intervalTime">指定间隔时间</param>
        public TimerTaskModel(int actionCount, int intervalTime)
            : this(0, false, 0, actionCount, intervalTime, "无名")
        { }


        /// <summary>
        /// 指定间隔时间无限执行
        /// </summary>
        /// <param name="intervalTime">指定间隔时间</param>
        public TimerTaskModel(int intervalTime)
            : this(0, false, 0, -1, intervalTime, "无名")
        { }
    }
}
